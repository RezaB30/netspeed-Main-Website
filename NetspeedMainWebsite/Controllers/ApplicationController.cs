﻿using NetspeedMainWebsite.MainSiteServiceReference;
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

        public ActionResult ApplicationConfirm()

        {
            return View();
        }

        public ActionResult Index()
        {
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
                return Json(new { });
            }
            return Json(BuildingItems);
        }

        //[HttpPost]
        //public ActionResult GetServiceAvailability()
        //{
        //    var addressMessage = string.Empty;
        //    addressMessage = "Lütfen Adresinizi Giriniz";
        //    TempData["addressMessage"] = addressMessage;

        //    //return PartialView("~/Views/Application/ApplicationParts/_InfrastructureInquiryForApplication.cshtml");
        //    return PartialView("~/Views/Application/ApplicationParts/_HasNotInfrastructure.cshtml");
        //}

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
                return PartialView("~/Views/Application/ApplicationParts/_HasNotInfrastructure.cshtml");
            }

            return PartialView("~/Views/Application/ApplicationParts/_InfrastructureAndTariffs.cshtml", model:infrastructureTariff);
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
        public ActionResult Index(ApplicationViewModel application)
        {
            ApplicationViewModel InfrastructureResult = new ApplicationViewModel();
                       
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
                    BirthDate = application.BirthDate,
                    DateOfIssue = application.DateOfIssue,
                    SMSCode = application.SMSCode,
                    TariffId=application.TariffId,
                    DisplayName=application.DisplayName

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

                var exTime = (Value.ExpirationDate - DateTime.Now).Seconds*2;
                ViewBag.exTime = exTime;
                //Session["smscode"] = response.SMSCode;

                return View(viewName: "GsmVerificationWithSms", model: result);
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

            if (Value.ExpirationDate > DateTime.Now)
            {
                if (Value.SMSCode == result.SMSCode)//Is true sms code?
                {
                    //return View(viewName: "GsmVerification", model: result);
                    WebServiceWrapper clientGetAddress = new WebServiceWrapper();
                    var getAddress = clientGetAddress.GetApartmentAddress(result.ApartmentId);
                    var address = getAddress.AddressDetailsResponse;
                    result.AddressText=getAddress.AddressDetailsResponse.AddressText;
                    return View(viewName: "ApplicationSummary", model: result);
                }
                else
                {
                    message = "Lütfen Sms Kodunuzu Kontrol Edip Tekrar Deneyiniz.";
                    TempData["message"]="message";
                   
                    //customers have 2 minutes
                    return View(viewName: "GsmVerificationWithSms", model: result);
                    //return PartialView("~/Views/Application/ApplicationParts/_SMSValidation.cshtml" , model: result);
                }
            }
            else
            {
                return RedirectToAction("Index", "Application");
            }
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

            var response = new WebServiceWrapper().NewCustomerRegister(/*1,*//* 1,*/ 1, address.ProvinceID, address.ProvinceName, address.DistrictID, address.DistrictName,
                address.RuralCode, address.NeighbourhoodID, address.NeighbourhoodName, address.StreetID, address.StreetName, address.ApartmentID,
                  address.ApartmentNo, address.AddressText, address.AddressNo, address.DoorID, address.DoorNo, result.Floor,
                result._PostalCode, result.BirthPlace, result.FatherName,
               result.MotherFirstSurname, result.MotherName, result.Nationality, 962,
               result.Sex, result.BirthDate, result.IDCardType, result.FirstName,
               result.LastName, result.TC, result.SerialNo, result.PlaceOfIssue,
               result.DateOfIssue, null, result.PhoneNumber, "tr-tr", /*1,*/ result.EmailAddress, result.ReferenceCode
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





    


        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public ActionResult ApplicationGet(ApplicationViewModel application)
        ////public ActionResult Index(ApplicationViewModel application)
        //{
        //    //var ApplicationItemList = (List<ApplicationViewModel>)Session["ApplicationItemList"];
        //    //var Gsm = Session["Gsm"];
        //    //string SerialNo = "A25I96170";
        //    ////DateTime dti = new DateTime(2029, 12, 26);
        //    //WebServiceWrapper clientAddress = new WebServiceWrapper();
        //    //var getAddress = clientAddress.GetApartmentAddress(application.ApartmentId);

        //    //var address = getAddress.AddressDetailsResponse;

        //    //var response = new WebServiceWrapper().NewCustomerRegister(1, 1, 1, address.ProvinceID, address.ProvinceName, address.DistrictID, address.DistrictName,
        //    //    address.RuralCode, address.NeighbourhoodID, address.NeighbourhoodName, address.StreetID, address.StreetName, address.ApartmentID,
        //    //      address.ApartmentNo, address.AddressText, address.AddressNo, address.DoorID, address.DoorNo, application.Floor,
        //    //    application.PostalCode, application.BirthPlace, application.FatherName,
        //    //   application.MotherFirstSurname, application.MotherName, (int)application.Nationality, 962,
        //    //   application.Sex, application.BirthDate, (int)application.IDCardType, application.FirstName,
        //    //   application.LastName, application.TC, application.SerialNo, application.PlaceOfIssue,
        //    //   new DateTime(2019, 12, 26), null, application.PhoneNumber, "tr-tr", 1, application.EmailAddress, application.ReferenceCode
        //    //   );


        //    //    if (response.ResponseMessage.ErrorCode == 0)
        //    //    {
        //    //        return RedirectToAction("ApplicationConfirm", "Application");
        //    //    }

        //    //    return RedirectToAction("ApplicationFail", "Application");

        //}
    }
}