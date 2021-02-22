using NetspeedMainWebsite.AddressUtilities;
using NetspeedMainWebsite.MainSiteServiceReference;
using NetspeedMainWebsite.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Controllers
{
    [RoutePrefix("basvur")]
    [Route("{action=Index}")]
    public class ApplicationController : BaseController
    {
        //Logger applicationLogger = LogManager.GetLogger("applications");

        //Logger countdownLogger = LogManager.GetLogger("countdown");

        public ActionResult Index()
        {
            var responseIDCard = new WebServiceWrapper().GetIDCardTypes();
            var IDCardTypeList = responseIDCard.ValueNamePairList.Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Code.ToString()
            });

            ViewBag.IDCardTypeList = IDCardTypeList;

            var addressUtil = new AddressUtility();
            var responseProvince = addressUtil.GetProvinces();

            ViewBag.ProvinceList = new SelectList(responseProvince, "Key", "Value");

            var responseNat = new WebServiceWrapper().GetNationalities();
            var NationalityList = responseNat.ValueNamePairList.Select(n => new SelectListItem()
            {
                Text = n.Name,
                Value = n.Code.ToString()
            });

            ViewBag.NationalityList = NationalityList;

            var responseSex = new WebServiceWrapper().GetSexes();
            var SexList = responseSex.ValueNamePairList.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Code.ToString()
            });

            ViewBag.SexList = SexList;
            var DistrictList = new List<SelectListItem>();
            var RegionList = new List<SelectListItem>();
            var NeighborhoodList = new List<SelectListItem>();
            var StreetList = new List<SelectListItem>();
            var BuildingList = new List<SelectListItem>();
            var ApartmentList = new List<SelectListItem>();

            ViewBag.DistrictList = DistrictList;
            ViewBag.RegionList = RegionList;
            ViewBag.NeighborhoodList = NeighborhoodList;
            ViewBag.StreetList = StreetList;
            ViewBag.BuildingList = BuildingList;
            ViewBag.ApartmentList = ApartmentList;

            ViewBag.SexList = SexList;
            ViewBag.NationalityList = NationalityList;
            ViewBag.IDCardTypeList = IDCardTypeList;

            return View();
        }
        //news
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(Models.ViewModel.RegisterViewModel register)
        {
            var addressWrapper = new WebServiceWrapper();
            var address = addressWrapper.GetApartmentAddress(Convert.ToInt64(register.kapino));
            var hasAddress = address.ResponseMessage.ErrorCode == 0;
            if (!hasAddress)
            {
                return Json(new { message = "Address Bilgisi Alınamadı. Tekrar başvuruyu göndermeyi deneyiniz.", errorCode = 1 }, JsonRequestBehavior.AllowGet);
            }
            DateTime birthDate;
            DateTime IDCardDate;
            DateTime.TryParse($"{register.dogumyili}-{register.dogumay}-{register.dogumgun}", out birthDate);
            DateTime.TryParse($"{register.kimlikyil}-{register.kimlikay}-{register.kimlikgun}", out IDCardDate);
            var wrapper = new WebServiceWrapper();
            var response = wrapper.NewCustomerRegister(
                register.tariff,
                address.AddressDetailsResponse.ProvinceID,
                address.AddressDetailsResponse.ProvinceName,
                address.AddressDetailsResponse.DistrictID,
                address.AddressDetailsResponse.DistrictName,
                address.AddressDetailsResponse.RuralCode,
                address.AddressDetailsResponse.NeighbourhoodID,
                address.AddressDetailsResponse.NeighbourhoodName,
                address.AddressDetailsResponse.StreetID,
                address.AddressDetailsResponse.StreetName,
                address.AddressDetailsResponse.ApartmentID,
                address.AddressDetailsResponse.ApartmentNo,
                address.AddressDetailsResponse.AddressText,
                address.AddressDetailsResponse.AddressNo,
                address.AddressDetailsResponse.DoorID,
                address.AddressDetailsResponse.DoorNo,
                register.katno,
                register.postakodu,
                register.dogumyeri,
                register.babaadi,
                register.annekizliksoyad,
                register.anneadi,
                228,
                962,
                register.cinsiyet,
                DateUtilities.ConvertToWebServiceDate(birthDate),
                register.idCardType,
                register.firstname,
                register.lastname,
                register.tcno,
                register.serino,
                register.kimlikil, // verildiği yer olacak
                DateUtilities.ConvertToWebServiceDate(IDCardDate),
                null,
                register.gsmno.Replace("(","").Replace(")","").Replace(" ","").Replace("-","").Replace("_",""),
                "tr-tr",
                register.email,
                register.referans,
                register.tariff, // ?

                register.idCardType == (int)IDCardTypes.TCIDCardWithChip ? null : register.sirano,
                register.idCardType == (int)IDCardTypes.TCIDCardWithChip ? null : register.ciltno,
                register.idCardType == (int)IDCardTypes.TCIDCardWithChip ? null : register.ailesirano,
                register.idCardType == (int)IDCardTypes.TCIDCardWithChip ? null : register.kimlikil,
                register.idCardType == (int)IDCardTypes.TCIDCardWithChip ? null : register.kimlikilce,
                register.idCardType == (int)IDCardTypes.TCIDCardWithChip ? null : register.kimlikmahalle
                );

            if (response.ResponseMessage.ErrorCode == 0)
            {
                return Json(new { message = "Başvurunuz başarıyla alındı.", errorCode = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (response.ResponseMessage.ErrorCode == 7)
            {
                return Json(new { message = "Başvurunuz alınamadı. Mevcut aboneliğiniz bulunmakta. Yeni abonelik için Online İşlem Merkezi üzerinden başvuru yapabilirsiniz.", errorCode = 1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Başvurunuz alınamadı. Lütfen daha sonra tekrar deneyiniz", errorCode = 1 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SendValidationSMS(string phoneNo)
        {
            var customerWrapper = new WebServiceWrapper();
            var customerResponse = customerWrapper.IsRegisteredCustomer(phoneNo);
            if (customerResponse.ResponseMessage.ErrorCode == 0)
            {
                if (customerResponse.ValidateResult != true)
                {
                    var wrapper = new WebServiceWrapper();
                    var response = wrapper.SendGenericSMS(phoneNo);
                    if (response.ResponseMessage.ErrorCode == 0)
                    {
                        Session["validationSMS"] = response.SMSCode;
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("invalid", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CheckValidationSMS(string smsCode)
        {
            var getsmsCode = Session["validationSMS"] == null ? null : Session["validationSMS"] as string;
            if (getsmsCode == smsCode)
            {
                Session.Remove("validationSMS");                
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            var smsCount = Session["smsCount"] == null ? 0 : (int)Session["smsCount"];
            if (smsCount > 2)
            {
                Session.Remove("validationSMS");
                Session.Remove("smsCount");
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            Session["smsCount"] = smsCount + 1;
            return Json($"error : code -> {getsmsCode}", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTariffs(string apartmentCode)
        {
            var availability = new WebServiceWrapper();
            var response = availability.ServiceAvailability(apartmentCode);
            if (response.ResponseMessage.ErrorCode != 0)
            {
                return Content("error");
            }
            var responseTariffs = new WebServiceWrapper().GetTariffList();
            if (responseTariffs.ResponseMessage.ErrorCode != 0)
            {
                return Content("error");
            }
            if (response.ServiceAvailabilityResponse.FIBER.HasInfrastructureFiber)
            {
                var getTariffs = responseTariffs.ExternalTariffList?.Where(ext => ext.HasFiber).Select(ext => new Models.ViewModel.TariffsViewModel()
                {
                    DisplayName = ext.DisplayName,
                    TariffID = ext.TariffID,
                    Price = ext.Price,
                    Speed = ext.Speed
                });
                return PartialView("~/Views/Home/_Tariffs.cshtml", getTariffs);
            }
            {
                var getTariffs = responseTariffs.ExternalTariffList?.Where(ext => ext.HasXDSL).Select(ext => new Models.ViewModel.TariffsViewModel()
                {
                    DisplayName = ext.DisplayName,
                    TariffID = ext.TariffID,
                    Price = ext.Price,
                    Speed = ext.Speed
                });
                return PartialView("~/Views/Home/_Tariffs.cshtml", getTariffs);
            }

        }
        [HttpPost]
        public ActionResult GetAddressText(string apartmentCode)
        {
            if (!string.IsNullOrEmpty(apartmentCode))
            {
                var availability = new WebServiceWrapper();
                var response = availability.GetApartmentAddress(Convert.ToInt64(apartmentCode));
                if (response.ResponseMessage.ErrorCode != 0)
                {
                    return Content("-");
                }
                return Content(response.AddressDetailsResponse.AddressText);
            }
            return Content("-");
        }
        [HttpPost]
        public ActionResult DateTimeValidaiton(int year, int month, int day)
        {
            if (DateTime.TryParse($"{year}-{month}-{day}", out DateTime date))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult IDCardValidation(int idCardType, string tckNo, string firstName, string lastName, int birthDateDay, int birthDateMonth, int birthDateYear, string serialNumber)
        {
            if (DateTime.TryParse($"{birthDateYear}-{birthDateMonth}-{birthDateDay}", out DateTime date))
            {
                WebServiceWrapper idCardValidation = new WebServiceWrapper();
                var validateBirthDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                var response = idCardValidation.IDCardValidationResponse(idCardType, tckNo, firstName, lastName, validateBirthDate, serialNumber);
                if (response.IDCardValidationResponse == true)
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("error", JsonRequestBehavior.AllowGet);
        }     
    }
}