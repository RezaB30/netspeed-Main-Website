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
        //WebServiceWrapper client = new WebServiceWrapper();


        //public ActionResult ApplicationSummary()
        //{
        //    return View();
        //}

        public ActionResult AlreadyHaveCustomer()
        {
            return View();
        }

        public ActionResult _IDInformation()
        {
            return View();
        }
        public ActionResult GsmVerification()
        {
            return View();
        }

        public ActionResult ApplicationConfirm()

        {
            return View();
        }

        public ActionResult Index()
        {
            int NowYear;
            int NowMonth;
            int NowDay;
            NowYear = DateTime.Now.Year;
            NowMonth = DateTime.Now.Month;
            NowDay = DateTime.Now.Day;

            int DaysInMonth = DateTime.DaysInMonth(NowYear, 2);

            var responseIDCard = new WebServiceWrapper().GetIDCardTypes();

            var IDCardItems = responseIDCard.ValueNamePairList.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Code.ToString()
            });

            var responseProvince = new WebServiceWrapper().GetProvinces();
            var ProvinceItems = responseProvince.ValueNamePairList.Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Code.ToString()

            });

            List<SelectListItem> Years = new List<SelectListItem>();
            for (int y = 1940; y <= NowYear; y++)
            {
                Years.Add(new SelectListItem { Text = y.ToString(), Value = y.ToString() });
            }
            ViewBag.Years = Years;

            List<SelectListItem> Months = new List<SelectListItem>();
            for (int m = 1; m <= 12; m++)
            {
                Months.Add(new SelectListItem { Text = m.ToString(), Value = m.ToString() });
            }
            ViewBag.Months = Months;

            List<SelectListItem> Days = new List<SelectListItem>();
            for (int d = 0; d < DaysInMonth; d++)
            {
                Days.Add(new SelectListItem { Text = d.ToString(), Value = d.ToString() });
            }
            ViewBag.Days = Days;



            var responseNat = new WebServiceWrapper().GetNationalities();
            var NatItems = responseNat.ValueNamePairList.Select(n => new SelectListItem()
            {
                Text = n.Name,
                Value = n.Code.ToString()
            });

            var responseSex = new WebServiceWrapper().GetSexes();
            var SexItems = responseSex.ValueNamePairList.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Code.ToString()
            });
            return View(new ApplicationViewModel() { ProvinceList = ProvinceItems, IDCardTypeList = IDCardItems, NationalityList = NatItems, SexList = SexItems, });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDistricts(long code)
        {
            var response = new WebServiceWrapper().GetProvinceDistricts(code);
            var DistrictItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (DistrictItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(DistrictItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRegions(long code)
        {
            var response = new WebServiceWrapper().GetDistrictRuralRegions(code);

            var RegionItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (RegionItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(RegionItems, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetNeighborhoods(long code)
        {
            var response = new WebServiceWrapper().GetRuralRegionNeighbourhoods(code);

            var NeighborhoodsItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (NeighborhoodsItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(NeighborhoodsItems, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetStreets(long code)
        {
            var response = new WebServiceWrapper().GetNeighbourhoodStreets(code);

            var StreetItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (StreetItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(StreetItems, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetBuildings(long code)
        {
            var response = new WebServiceWrapper().GetStreetBuildings(code);

            var BuildingItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (BuildingItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(BuildingItems, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetApartments(long code)
        {
            var response = new WebServiceWrapper().GetBuildingApartments(code);

            var BuildingItems = response.ValueNamePairList.Select(r => new
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });
            if (BuildingItems == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(BuildingItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDays(int year, int month)
        {
            int DaysInMonth = DateTime.DaysInMonth(year, month);

            List<SelectListItem> Days = new List<SelectListItem>();
            for (int d = 1; d <= DaysInMonth; d++)
            {
                Days.Add(new SelectListItem { Text = d.ToString(), Value = d.ToString() });
            }

            if (Days == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(Days, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult test1(ApplicationViewModel application)
        public ActionResult Index(ApplicationViewModel application)
        //public ActionResult Index(ApplicationViewModel application)
        {
            //var message = string.Empty;
            //var exTime = new DateTime();


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
                    //BirthYear = application.BirthYear,
                    //BirthMonth = application.BirthMonth,
                    //BirthDay = application.BirthDay,
                    //BirthDate = new DateTime(application.BirthYear, application.BirthMonth, application.BirthDay),
                    BirthDate = application.BirthDate,
                    DateOfIssue = application.DateOfIssue,
                    SMSCode = application.SMSCode
                };

                var message = string.Empty;
                WebServiceWrapper clientPhone = new WebServiceWrapper();
                var response = clientPhone.SendGenericSMS(application.PhoneNumber);//sending phone number for sms code

                var expirationDate = DateTime.Now + Properties.Settings.Default.SMSValidationDuration;

                var Key = application.PhoneNumber;

                List<ApplicationViewModel> valuelist = new List<ApplicationViewModel>();

                //valuelist.Add (new ApplicationViewModel()
                //{
                //    SMSCode = response.SMSCode,//true sms code
                //    ExpirationDate = expirationDate //expiration date for sms code  
                //});

                var Value = new ApplicationViewModel()
                {
                    SMSCode = response.SMSCode,//true sms code
                    ExpirationDate = expirationDate //expiration date for sms code  
                };

                //MemoryCache.Default.Set($"{application.PhoneNumber}");

                //MemoryCache.Default.Add(Key, Value, DateTimeOffset.Now.AddMinutes(Properties.Settings.Default.SMSValidationDuration.Minutes));
                MemoryCache.Default.Add(Key, Value, DateTimeOffset.Now.AddMinutes(Properties.Settings.Default.SMSValidationDuration.Minutes));

                var exTime = (Value.ExpirationDate - DateTime.Now).Seconds;
                ViewBag.exTime = exTime;
                //Session["smscode"] = response.SMSCode;

                return View(viewName: "GsmVerificationWithSms", model: result);
                //return Url.Action("GsmVerificationWithSms", "Application",new { id=Value, new ApplicationViewModel()});

                //return Url.Action("GsmVerificationWithSms", "Application",new { id=Key} );
            }
            return View(application);
        }




        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GsmVerificationWithSms(ApplicationViewModel result)
        {
            var message = string.Empty;

            //var smscode = Session["smscode"] as string;
            var Value = MemoryCache.Default.Get(result.PhoneNumber) as ApplicationViewModel;


            //var smsCode2= MemoryCache.Default.Get(result.PhoneNumber);

            //if (ModelState.IsValid)
            //{
            if (Value.ExpirationDate > DateTime.Now)
            {
                if (Value.SMSCode == result.SMSCode)//Is true sms code?
                {
                    //return View(viewName: "GsmVerification", model: result);
                    return View(viewName: "ApplicationSummary", model: result);

                }
                else
                {
                    message = "Lütfen Sms Kodunuzu Kontrol Edip Tekrar Deneyiniz.";
                    ViewBag.message = "message";
                    //customers have 2 minutes
                    return View(viewName: "GsmVerificationWithSms", model: result);
                    //return PartialView("~/Views/Application/ApplicationParts/_SMSValidation.cshtml" , model: result);
                }
            }
            else
            {
                return RedirectToAction("Index", "Application");
            }

            //}
            //return View(result);
        }


        //[HttpPost]
        //public ActionResult GsmVerification(ApplicationViewModel application, string smsCode)
        //{
        //    string Gsm = application.PhoneNumber;
        //    var client = new WebServiceWrapper();
        //    var response = client.SendGenericSMS(application.PhoneNumber);
        //    if (response.SMSCode == smsCode)
        //    {

        //    }

        //    return View(viewName: "GsmVerificationWithSms", model: application);
        //    //return RedirectToAction("ApplicationGet", "Application");
        //    return RedirectToAction("GsmVerificationWithSms", "Application");

        //}

        //[HttpPost]

        //public ActionResult GsmVerificationWithSms(string phoneNo, string smsCode)
        //{
        //    //var Key = Guid.NewGuid().ToString();
        //    //var Value = DateTime.Now;

        //var client = new WebServiceWrapper();
        //var response = client.SendGenericSMS(phoneNo);

        //if (response.SMSCode == smsCode)
        //{

        //}
        //var expirationDate = DateTime.Now + Properties.Settings.Default.SMSValidationDuration;
        //MemoryCache.Default.Set($"{phoneNo}_{}")
        //var AlertValue = Value + 2;


        //MemoryCache.Default.Add(Key, Value, DateTimeOffset.Now.AddMinutes(2));

        //if (DateTime.Now.Minute - Value > 2)
        //{
        //    RedirectToAction("Index", "Application", new { id = Key });
        //}

        //if (DateTime.Now.Minute - Value < 2)
        //{
        //    Url.Action("GsmVerificationWithSms", "Application", new { id = Key });
        //}

        //var RemainingTime = AlertValue - DateTime.Now.Minute;

        //ViewBag.RemainingTime = RemainingTime;

        //    return View();
        //}

        ////[HttpPost]
        //public ActionResult GsmVerificationWithSms(string phoneNo, VerificationViewModel smsCode)
        //{

        //    var message = string.Empty;
        //    string Gsm = Session["Gsm"].ToString();

        //    //Session["Action"] = DateTime.Now.Minute;

        //    //if (Session["Action"] == null || (DateTime.Now - (DateTime)(Session["Action"])).Minutes < 2)
        //    //{
        //    //var response = new WebServiceWrapper().RegisterSMSValidation(Gsm, smsCode.Code);//buraya dön

        //    Session["Action"] = DateTime.Now;
        //    //if (response.ResponseMessage.ErrorCode == 0)
        //    {
        //        return RedirectToAction("ApplicationSummary", "Application");
        //    }
        //    //}
        //    //else
        //    //{
        //    return View();
        //    //}
        //    //return View();


        //    ViewBag.message = "Lütfen Kodu Kontrol Ediniz.";

        //    return View();
        //}


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ApplicationSummary(ApplicationViewModel result)
        {
            //var ApplicationItemList = (List<ApplicationViewModel>)Session["ApplicationItemList"];
            //var Gsm = Session["Gsm"];
            //string SerialNo = "A25I96170";
            //DateTime dti = new DateTime(2029, 12, 26);

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
            //return View(Confirm);


            //string SerialNo = "A25I96170";
            //DateTime dti = new DateTime(2029, 12, 26);
            //WebServiceWrapper clientAddress = new WebServiceWrapper();
            //var getAddress = clientAddress.GetApartmentAddress(result.ApartmentId);

            //var address = getAddress.AddressDetailsResponse;

            var response = new WebServiceWrapper().NewCustomerRegister(1, 1, 1, address.ProvinceID, address.ProvinceName, address.DistrictID, address.DistrictName,
                address.RuralCode, address.NeighbourhoodID, address.NeighbourhoodName, address.StreetID, address.StreetName, address.ApartmentID,
                  address.ApartmentNo, address.AddressText, address.AddressNo, address.DoorID, address.DoorNo, result.Floor,
                result._PostalCode, result.BirthPlace, result.FatherName,
               result.MotherFirstSurname, result.MotherName, result.Nationality, 962,
               result.Sex, result._BirthDate, result.IDCardType, result.FirstName,
               result.LastName, result.TC, result.SerialNo, result.PlaceOfIssue,
               result.DateOfIssue, null, result.PhoneNumber, "tr-tr", 1, result.EmailAddress, result.ReferenceCode
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



        //[HttpPost]
        public ActionResult InfrastructureInquiryForApplication()
        {
            WebServiceWrapper clientProvince = new WebServiceWrapper();

            var response = clientProvince.GetProvinces();
            var ProvinceItems = response.ValueNamePairList.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });

            return View(new ApplicationViewModel() { ProvinceList = ProvinceItems });
        }

        [HttpPost]
        public ActionResult InfrastructureAndTariffs(string ApartmentId)
        {
            WebServiceWrapper clientBBK = new WebServiceWrapper();

            var response = clientBBK.ServiceAvailability(ApartmentId);
         

            return View();
        }
    }
}