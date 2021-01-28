using NetspeedMainWebsite.AddressUtilities;
using NetspeedMainWebsite.MainSiteServiceReference;
using NetspeedMainWebsite.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Controllers
{
    public class ApplicationController : Controller
    {
        public ActionResult AlreadyHaveCustomer()
        {
            return View();
        }

        public ActionResult GsmVerification()
        {
            return View();
        }
        public ActionResult ApplicationSummary()
        {
            return View();
        }

        public ActionResult ApplicationConfirm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CallMe(CallMeViewModel callMe, string returnUrl)
        {
            WebServiceWrapper client = new WebServiceWrapper();
            var message = string.Empty;

            if (ModelState.IsValid)
            {
                var response = client.RegisterCustomerContact(callMe.FullName, callMe.PhoneNumber);

                message = "Talebiniz Alınmıştır.";
                TempData["message"] = message;

                return Redirect(returnUrl);
            }

            if (!ModelState.IsValid)
            {
                TempData["CallMeModel"] = callMe;
                var errors = ModelState.ToArray().Select(ms => new { Key = ms.Key, ErrorMessages = string.Join(Environment.NewLine, ms.Value.Errors.Select(e => e.ErrorMessage)) }).ToArray();
                foreach (var errorItem in errors)
                {
                    if (errorItem.Key == "callMe.FullName")
                    {
                        callMe.FullNameValidationMessage = errorItem.ErrorMessages;
                    }
                    else if (errorItem.Key == "callMe.PhoneNumber")
                    {
                        callMe.PhoneNumberValidationMessage = errorItem.ErrorMessages;
                    }
                }
            }

            return Redirect(returnUrl);
        }

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

        [HttpPost]
        public ActionResult GetServiceAvailability(long apartmentId)
        {
            var message = string.Empty;

            //ApplicationViewModel InfrastructureResult = new ApplicationViewModel();
            var applicationTariff = new ApplicationViewModel();
            var infrastructureTariff = new InfrastructureTariffViewModel();
            WebServiceWrapper clientAddres = new WebServiceWrapper();
            var getAddress = clientAddres.ServiceAvailability(apartmentId.ToString());

            WebServiceWrapper clientTariff = new WebServiceWrapper();
            var getTariff = clientTariff.GetTariffList();

            var Fiber = getAddress.ServiceAvailabilityResponse.FIBER;
            var Vdsl = getAddress.ServiceAvailabilityResponse.VDSL;
            var Adsl = getAddress.ServiceAvailabilityResponse.ADSL;

            if (ModelState.IsValid)
            {
                if (Fiber.HasInfrastructureFiber)
                {
                    var displaySpeed = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Fiber.FiberSpeed.Value) * 1024, true);
                    infrastructureTariff.Distance = Fiber.FiberDistance.ToString();
                    //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.FiberSpeed.ToString();
                    infrastructureTariff.MaxSpeed = $"{displaySpeed.FieldValue} {displaySpeed.RateSuffix}";
                    infrastructureTariff.XDSLType = "FİBER";
                    infrastructureTariff.PortState = Fiber.FiberPortState.ToString();
                    infrastructureTariff.SVUID = Fiber.FiberSVUID.ToString();

                    var TariffItems = getTariff.ExternalTariffList.Where(f => f.HasFiber == true).Select(t => new TariffsViewModel
                    {
                        TariffID = t.TariffID,
                        DisplayName = t.DisplayName,
                        Price = t.Price,
                        Speed = t.Speed,
                    });
                    infrastructureTariff.TariffList = TariffItems.ToArray();
                }
                else if (Vdsl.HasInfrastructureVdsl && Vdsl.VdslSpeed > Adsl.AdslSpeed)
                {
                    var displaySpeedVdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Vdsl.VdslSpeed.Value) * 1024, true);
                    infrastructureTariff.MaxSpeed = $"{displaySpeedVdsl.FieldValue} {displaySpeedVdsl.RateSuffix}";
                    infrastructureTariff.Distance = Vdsl.VdslDistance.ToString();
                    //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.VdslSpeed.ToString();
                    infrastructureTariff.XDSLType = "VDSL";
                    infrastructureTariff.PortState = Vdsl.VdslPortState.ToString();
                    infrastructureTariff.SVUID = Vdsl.VdslSVUID.ToString();
                    //return View(InfrastructureResult);

                    var TariffItems = getTariff.ExternalTariffList.Where(f => f.HasXDSL == true).Select(t => new TariffsViewModel
                    {
                        TariffID = t.TariffID,
                        DisplayName = t.DisplayName,
                        Price = t.Price,
                        Speed = t.Speed,
                    });
                    infrastructureTariff.TariffList = TariffItems.ToArray();
                }
                else if (Adsl.HasInfrastructureAdsl && Adsl.AdslSpeed > Vdsl.VdslSpeed)
                {
                    //var displaySpeed = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)response.ServiceAvailabilityResponse.FiberSpeed.Value) * 1024, true);
                    var displaySpeedAdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Adsl.AdslSpeed.Value) * 1024, true);
                    infrastructureTariff.MaxSpeed = $"{displaySpeedAdsl.FieldValue} {displaySpeedAdsl.RateSuffix}";
                    infrastructureTariff.Distance = Adsl.AdslDistance.ToString();
                    //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.AdslSpeed.ToString();
                    infrastructureTariff.XDSLType = "ADSL";
                    infrastructureTariff.PortState = Adsl.AdslPortState.ToString();
                    infrastructureTariff.SVUID = Adsl.AdslSVUID.ToString();
                    //return View(InfrastructureResult);
                    var TariffItems = getTariff.ExternalTariffList.Where(f => f.HasXDSL == true).Select(t => new TariffsViewModel
                    {
                        TariffID = t.TariffID,
                        DisplayName = t.DisplayName,
                        Price = t.Price,
                        Speed = t.Speed,
                    });
                    infrastructureTariff.TariffList = TariffItems.ToArray();
                }
                else//DÜZENLE
                {
                    //message = "Haneye Ait Altyapı Bulunamadığından Başvuruya Devam Edemezsiniz.";
                    //TempData["message"]=message;
                    //ViewBag.message = message;
                    //return RedirectToAction("Index", "Application");

                    message = "altyapiyok";
                    TempData["message"] = "messsage";
                    return PartialView("ApplicationParts/_HasNotInfrastructure");
                }
            }
            else
            {
                return PartialView("~ApplicationParts/_HasNotInfrastructure");
            }

            return PartialView("ApplicationParts/_InfrastructureAndTariffs", model: infrastructureTariff);
            //return Json(new
            //{
            //    tariffs = tariffs.ToArray(),
            //    maxSpeed = InfrastructureResult.MaxSpeed,
            //    distance = InfrastructureResult.Distance,
            //    XDSLType = InfrastructureResult.XDSLType,
            //    portState = InfrastructureResult.PortState,
            //    SVUID = InfrastructureResult.SVUID,
            //});

        }



        [HttpPost]
        public ActionResult Index(ApplicationViewModel application)
        {
            ApplicationViewModel InfrastructureResult = new ApplicationViewModel();

            var dateAndTime = application.BirthDate;
            application.BirthDate = dateAndTime.GetValueOrDefault().Date;

            if (ModelState.IsValid)
            {
                //DateTime bd = new DateTime(application.BirthYear, application.BirthMonth, application.BirthDay);
                var ApplicationList = new List<ApplicationViewModel>();



                var result = new ApplicationViewModel()
                {
                    FirstName = application.FirstName.ToUpper(new CultureInfo("tr-TR", false)),
                    LastName = application.LastName,
                    BirthPlace = application.BirthPlace.ToUpper(new CultureInfo("tr-TR", false)),
                    //ContactPhoneNo = application.PhoneNumber,
                    PhoneNumber = application.PhoneNumber,
                    TC = application.TC,
                    EmailAddress = application.EmailAddress,
                    MotherName = application.MotherName.ToUpper(new CultureInfo("tr-TR", false)),
                    FatherName = application.FatherName.ToUpper(new CultureInfo("tr-TR", false)),
                    MotherFirstSurname = application.MotherFirstSurname.ToUpper(new CultureInfo("tr-TR", false)),
                    PostalCode = application.PostalCode,
                    Floor = application.Floor,
                    ApartmentId = application.ApartmentId,
                    DistrictId = application.DistrictId,
                    BuildingId = application.BuildingId,
                    ProvinceId = application.ProvinceId,
                    NeighborhoodId = application.NeighborhoodId,
                    RegionId = application.RegionId,
                    StreetId = application.StreetId,
                    SerialNo = application.SerialNo.ToUpper(new CultureInfo("tr-TR", false)),
                    IDCardType = application.IDCardType,
                    ReferenceCode = application.ReferenceCode,
                    Nationality = application.Nationality,
                    Sex = application.Sex,
                    //BirthDate =Convert.ToDateTime(application.ReturnDateForDisplay),
                    BirthDate = application.BirthDate,
                    DateOfIssue = application.DateOfIssue,
                    SMSCode = application.SMSCode,
                    TariffId = application.TariffId,
                    DisplayName = application.DisplayName
                };

                var message = string.Empty;
                WebServiceWrapper clientPhone = new WebServiceWrapper();
                var response = clientPhone.SendGenericSMS(application.PhoneNumber);//sending phone number for sms code

                var expirationDate = DateTime.Now + Properties.Settings.Default.SMSValidationDuration;

                var Key = application.PhoneNumber;

                List<ApplicationViewModel> valuelist = new List<ApplicationViewModel>();

                var Value = new ApplicationViewModel()
                {
                    SMSCode = response.SMSCode,//true sms code
                    ExpirationDate = expirationDate //expiration date for sms code  
                };

                MemoryCache.Default.Add(Key, Value, DateTimeOffset.Now.AddMinutes(Properties.Settings.Default.SMSValidationDuration.Minutes));

                var exTime = (Value.ExpirationDate - DateTime.Now).Seconds * 2;
                //ViewBag.exTime = exTime;
                TempData["exTime"] = exTime;
                //Session["smscode"] = response.SMSCode;

                return View(viewName: "GsmVerificationWithSms", model: result);
            }

            var responseIDCard = new WebServiceWrapper().GetIDCardTypes();
            var IDCardTypeList = responseIDCard.ValueNamePairList.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Code.ToString()
            });

            var responseNat = new WebServiceWrapper().GetNationalities();
            var NationalityList = responseNat.ValueNamePairList.Select(n => new SelectListItem()
            {
                Text = n.Name,
                Value = n.Code.ToString()
            });

            var responseSex = new WebServiceWrapper().GetSexes();
            var SexList = responseSex.ValueNamePairList.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Code.ToString()
            });

            ViewBag.IDCardTypeList = IDCardTypeList;
            ViewBag.NationalityList = NationalityList;
            ViewBag.SexList = SexList;

            var addressUtil = new AddressUtility();

            ViewBag.ProvinceList = new SelectList(addressUtil.GetProvinces(), "Key", "Value", application.ProvinceId);
            ViewBag.DistrictList = application.ProvinceId.HasValue ? new SelectList(addressUtil.GetProvinceDistricts(application.ProvinceId.Value), "Key", "Value", application.DistrictId) : new SelectList(Enumerable.Empty<object>());
            ViewBag.RegionList = application.DistrictId.HasValue ? new SelectList(addressUtil.GetDistrictRegions(application.DistrictId.Value), "Key", "Value", application.RegionId) : new SelectList(Enumerable.Empty<object>());
            ViewBag.NeighborhoodList = application.RegionId.HasValue ? new SelectList(addressUtil.GetRegionNeighbourhoods(application.RegionId.Value), "Key", "Value", application.NeighborhoodId) : new SelectList(Enumerable.Empty<object>());
            ViewBag.StreetList = application.NeighborhoodId.HasValue ? new SelectList(addressUtil.GetNeighbourhoodStreets(application.NeighborhoodId.Value), "Key", "Value", application.StreetId) : new SelectList(Enumerable.Empty<object>());
            ViewBag.BuildingList = application.StreetId.HasValue ? new SelectList(addressUtil.GetStreetBuildings(application.StreetId.Value), "Key", "Value", application.BuildingId) : new SelectList(Enumerable.Empty<object>());
            ViewBag.ApartmentList = application.BuildingId.HasValue ? new SelectList(addressUtil.GetBuildingAparments(application.BuildingId.Value), "Key", "Value", application.ApartmentId) : new SelectList(Enumerable.Empty<object>());


            //ViewBag.DistrictList = DistrictList;               
            //ViewBag.RegionList = RegionList;
            //ViewBag.NeighborhoodList = NeighborhoodList;
            //ViewBag.StreetList = StreetList;
            //ViewBag.BuildingList = BuildingList;
            //ViewBag.ApartmentList = ApartmentList;

            //ViewBag.DistrictId = application.DistrictId;
            //ViewBag.RegionId = application.RegionId;
            //ViewBag.NeighborhoodId = application.NeighborhoodId;
            //ViewBag.StreetId = application.StreetId;
            //ViewBag.BuildingId = application.BuildingId;
            //ViewBag.ApartmentId = application.ApartmentId;

            return View(application);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GsmVerificationWithSms(ApplicationViewModel result)
        {
            var SmsValidationMessage = string.Empty;

            //var smscode = Session["smscode"] as string;
            var Value = MemoryCache.Default.Get(result.PhoneNumber) as ApplicationViewModel;

            if (Value.ExpirationDate > DateTime.Now)
            {
                if (Value.SMSCode == result.SMSCode)//Is true sms code?
                {
                    //return View(viewName: "GsmVerification", model: result);
                    WebServiceWrapper clientGetAddress = new WebServiceWrapper();
                    var getAddress = clientGetAddress.GetApartmentAddress(result.ApartmentId);
                    var address = getAddress.AddressDetailsResponse;
                    result.AddressText = getAddress.AddressDetailsResponse.AddressText;

                    WebServiceWrapper clientGetTariff = new WebServiceWrapper();
                    var getTariff = clientGetTariff.GetTariffList();

                    var clientSelectTariff = getTariff.ExternalTariffList.Where(f => f.TariffID == result.TariffId).First().DisplayName;

                    result.DisplayName = clientSelectTariff;

                    return View(viewName: "ApplicationSummary", model: result);
                }
                else
                {
                    TempData["SmsValidationMessage"] = "Lütfen Sms Kodunuzu Kontrol Edip Tekrar Deneyiniz.";
                    var exTime = (Value.ExpirationDate - DateTime.Now).Seconds * 2;
                    //ViewBag.exTime = exTime;
                    TempData["exTime"] = exTime;
                    //customers have 2 minutes
                    return View(viewName: "GsmVerificationWithSms", model: result);
                }
            }
            else
            {
                return RedirectToAction("Index", "Application");
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ApplicationSummary(ApplicationViewModel result)
        {
            WebServiceWrapper client = new WebServiceWrapper();
            var getAddress = client.GetApartmentAddress(result.ApartmentId);
            var address = getAddress.AddressDetailsResponse;

            //var ConfirmList = new List<ApplicationViewModel>();

            var Confirm = new ApplicationViewModel()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                EmailAddress = result.EmailAddress,
                AddressText = address.AddressText
            };

            var response = new WebServiceWrapper().NewCustomerRegister(1, address.ProvinceID, address.ProvinceName, address.DistrictID, address.DistrictName,
                address.RuralCode, address.NeighbourhoodID, address.NeighbourhoodName, address.StreetID, address.StreetName, address.ApartmentID,
                  address.ApartmentNo, address.AddressText, address.AddressNo, address.DoorID, address.DoorNo, result.Floor,
                result._PostalCode, result.BirthPlace, result.FatherName,
               result.MotherFirstSurname, result.MotherName, result.Nationality, 962,
               result.Sex, result.BirthDate, result.IDCardType, result.FirstName,
               result.LastName, result.TC, result.SerialNo, result.PlaceOfIssue,
               result.DateOfIssue, null, result.PhoneNumber, "tr-tr", result.EmailAddress, result.ReferenceCode, result.TariffId
               );


            if (response.ResponseMessage.ErrorCode == 0)
            {
                return RedirectToAction("ApplicationConfirm", "Application");
            }

            if (response.ResponseMessage.ErrorCode == 7)
            {
                return RedirectToAction("AlreadyHaveCustomer", "Application");
            }

            if (response.ResponseMessage.ErrorCode == 200)
            {
                return RedirectToAction("Index", "Application");
            }

            return RedirectToAction("ApplicationFail", "Application");

        }

        //[ValidateAntiForgeryToken]
        public ActionResult ApplicationFail()
        {
            return View();
        }

    }
}