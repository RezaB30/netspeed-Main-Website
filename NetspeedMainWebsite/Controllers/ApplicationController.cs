﻿using NetspeedMainWebsite.AddressUtilities;
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
    public class ApplicationController : BaseController
    {
        Logger applicationLogger = LogManager.GetLogger("applications");

        Logger countdownLogger = LogManager.GetLogger("countdown");

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

        public ActionResult Index()
        {
            //ViewBag.ValidationError = Session["ValidationError"];

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
        public ActionResult GetIDCardValidation(int IDCardType, string TCKNo, string FirstName, string LastName, DateTime BirthDate, string RegistirationNo)
        {        
            applicationLogger.Error($"birthdate: {BirthDate} -GetIDCardValidation bEGIN");


            string ChangeDate = DateUtilities.ConvertToWebServiceDate(BirthDate);

            //string upperFirstName = UpperUtilities.ToUpperWebServiceWord(FirstName);
            //string upperLastName = UpperUtilities.ToUpperWebServiceWord(LastName);
            //string upperRegistirationNo = UpperUtilities.ToUpperWebServiceWord(RegistirationNo);

            string upperFirstName = FirstName.ToUpper();
            string upperLastName = LastName.ToUpper();
            string upperRegistirationNo = RegistirationNo.ToUpper();


            var IDCardValidationError = "Kimlik Bilgilerinizi Lütfen Kontrol Ediniz.";
            WebServiceWrapper idCardServiceClient = new WebServiceWrapper();
            var idCardValidationResponse = idCardServiceClient.IDCardValidationResponse(IDCardType, TCKNo, upperFirstName, upperLastName, ChangeDate, upperRegistirationNo);

            var idCardIsValid = idCardValidationResponse.IDCardValidationResponse;

            applicationLogger.Error($"upperFirstName: {upperFirstName} -GetIDCardValidation END");

            applicationLogger.Error($"upperLastName: {upperLastName} -GetIDCardValidation END");

            applicationLogger.Error($"upperRegistirationNo: {upperRegistirationNo} -GetIDCardValidation END");

            applicationLogger.Error($"birthdate: {ChangeDate} -GetIDCardValidation END");



            return Json(new { isValid = idCardIsValid, errorMessage = !idCardIsValid ? IDCardValidationError : null });
        }


        [HttpPost]
        public ActionResult GetServiceAvailability(long apartmentId)
        {
            var message = string.Empty;

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
                    infrastructureTariff.XDSLType = "VDSL";
                    infrastructureTariff.PortState = Vdsl.VdslPortState.ToString();
                    infrastructureTariff.SVUID = Vdsl.VdslSVUID.ToString();

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
                    var displaySpeedAdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Adsl.AdslSpeed.Value) * 1024, true);
                    infrastructureTariff.MaxSpeed = $"{displaySpeedAdsl.FieldValue} {displaySpeedAdsl.RateSuffix}";
                    infrastructureTariff.Distance = Adsl.AdslDistance.ToString();
                    infrastructureTariff.XDSLType = "ADSL";
                    infrastructureTariff.PortState = Adsl.AdslPortState.ToString();
                    infrastructureTariff.SVUID = Adsl.AdslSVUID.ToString();
                    var TariffItems = getTariff.ExternalTariffList.Where(f => f.HasXDSL == true).Select(t => new TariffsViewModel
                    {
                        TariffID = t.TariffID,
                        DisplayName = t.DisplayName,
                        Price = t.Price,
                        Speed = t.Speed,
                    });
                    infrastructureTariff.TariffList = TariffItems.ToArray();
                }
                else
                {
                    return PartialView("ApplicationParts/_HasNotInfrastructure");
                }
            }
            else
            {
                return PartialView("~ApplicationParts/_HasNotInfrastructure");
            }

            return PartialView("ApplicationParts/_InfrastructureAndTariffs", model: infrastructureTariff);

        }



        [HttpPost]
        public ActionResult Index(ApplicationViewModel application)
        {
            ViewBag.ValidationError = Session["ValidationError"];

            var Checked = application.TariffId;

            applicationLogger.Error($"birthdate: {application.BirthDate}- dateofissue: {application.DateOfIssue}-Index");


            ApplicationViewModel InfrastructureResult = new ApplicationViewModel();

            if (ModelState.IsValid)
            {
                var result = new ApplicationViewModel()
                {

                    FirstName = application.FirstName.ToUpper(new CultureInfo("tr-TR", false)),
                    LastName = application.LastName,
                    BirthPlace = application.BirthPlace.ToUpper(new CultureInfo("tr-TR", false)),
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
                    TariffId = application.TariffId,
                    DisplayName = application.DisplayName,

                    RowNo=application.RowNo,
                    VolumeNo=application.VolumeNo,
                    PageNo=application.PageNo,
                    IDCardProvince=application.IDCardProvince,
                    IDCardDistrict=application.IDCardDistrict,
                    IDCardNeighbourhood=application.IDCardNeighbourhood

                };


                var message = string.Empty;
                WebServiceWrapper clientPhone = new WebServiceWrapper();
                var response = clientPhone.SendGenericSMS(application.PhoneNumber);//sending phone number for sms code

                countdownLogger.Error($"now: { DateTime.Now}- Properties.Settings.Default.SMSValidationDuration:{Properties.Settings.Default.SMSValidationDuration} ");

                var expirationDate = DateTime.Now + Properties.Settings.Default.SMSValidationDuration;
                countdownLogger.Error($"now: { DateTime.Now}- Properties.Settings.Default.SMSValidationDuration:{Properties.Settings.Default.SMSValidationDuration}- expirationDate:{expirationDate} ---");

                //DateTime k = expirationDate;
                //DateTime t = DateTime.Now;

                ////var exTimeT = (int)(k - t).TotalSeconds;

                //TempData["exTimeT"] = exTimeT;


                var Key = application.PhoneNumber;

                List<ApplicationViewModel> valuelist = new List<ApplicationViewModel>();

                var Value = new ApplicationViewModel()
                {
                    SMSCode = response.SMSCode,//true sms code
                    ExpirationDate = expirationDate //expiration date for sms code  
                };





                MemoryCache.Default.Add(Key, Value, DateTimeOffset.Now.AddMinutes(Properties.Settings.Default.SMSValidationDuration.Minutes));

                //var exTime = (Value.ExpirationDate - DateTime.Now).Seconds * 2;
                DateTime ck = Value.ExpirationDate;
                DateTime ct = DateTime.Now;

                var exTimeT = (int)(ck - ct).TotalSeconds;
                //var exTimeT = (expirationDate - DateTime.Now).;




                //countdownLogger.Error($"first extime: {exTime}");
                countdownLogger.Error($"first extimeT: {exTimeT}");


                countdownLogger.Error($"Value.ExpirationDate: { Value.ExpirationDate} ---first");
                countdownLogger.Error($"expirationDate(extimeT): { expirationDate} ---first");








                //Session["exTime"] = exTime;
                TempData["exTimeT"] = exTimeT;

              
                countdownLogger.Error($"session extime: {Session["exTime"]}" );

                countdownLogger.Error($"TEMPDATA extime: {TempData["exTime"]} ");

                countdownLogger.Error($"TEMPDATA exTimeT: {TempData["exTimeT"]} ");


                //TempData["exTime"] = Value.ExpirationDate;

                applicationLogger.Error($"birthdate: {application.BirthDate}- dateofissue: {application.DateOfIssue}-Index end");

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
            ViewBag.Checked = Checked;
            return View(application);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GsmVerificationWithSms(ApplicationViewModel result)
        {
            var SmsValidationMessage = string.Empty;

            var Value = MemoryCache.Default.Get(result.PhoneNumber) as ApplicationViewModel;

            if (Value.ExpirationDate >= DateTime.Now)
            {
                if (Value.SMSCode == result.SMSCode)//Is true sms code?
                {
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
                    var exTimeT = (Value.ExpirationDate - DateTime.Now).Seconds * 2;
                    //Session["exTime"] = exTime;
                    //TempData["exTime"] = exTime;
                    TempData["exTimeT"] = exTimeT;


                    countdownLogger.Error($"now: { DateTime.Now}- Properties.Settings.Default.SMSValidationDuration:{Properties.Settings.Default.SMSValidationDuration} ");

                    var expirationDate = DateTime.Now + Properties.Settings.Default.SMSValidationDuration;
                    countdownLogger.Error($"now: { DateTime.Now}- Properties.Settings.Default.SMSValidationDuration:{Properties.Settings.Default.SMSValidationDuration}- expirationDate:{expirationDate} ---");

                    countdownLogger.Error($"Value.ExpirationDate: { Value.ExpirationDate} ---again");

                    countdownLogger.Error($" TempData[exTimeT]: - {TempData["exTimeT"]} ---again");
                    countdownLogger.Error($"Value.ExpirationDate: { Value.ExpirationDate} ---again");

                    countdownLogger.Error($"exTimeT: {exTimeT} ---again");








                    //countdownLogger.Error($"session extime: {Session["exTime"]}-again");

                    //countdownLogger.Error($"TEMPDATA extime: {TempData["exTime"]}-again ");

                    countdownLogger.Error($"TEMPDATA extime: {TempData["exTimet"]}-again ");


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
            var ValidationError = string.Empty;
            WebServiceWrapper client = new WebServiceWrapper();
            var getAddress = client.GetApartmentAddress(result.ApartmentId);
            var address = getAddress.AddressDetailsResponse;

            var Confirm = new ApplicationViewModel()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                EmailAddress = result.EmailAddress,
                AddressText = address.AddressText
            };

          
            var clientNewRegister = new WebServiceWrapper();

            applicationLogger.Error($"birthdate: {result.BirthDate}- dateofissue: {result.DateOfIssue}-applicationSummary-before-responseNewCustomerRegister");
            applicationLogger.Error($"sbirthdate: {result.sBirthDate}- sdateofissue: {result.sDateOfIssue}-applicationSummary-before-responseNewCustomerRegister");

           

            applicationLogger.Error($"FirstName: {result.FirstName}-  result.LastName: {result.LastName} -result.SerialNo {result.SerialNo}-applicationSummary-before-responseNewCustomerRegister");

            var responseNewCustomerRegister = clientNewRegister.NewCustomerRegister(1, address.ProvinceID, address.ProvinceName, address.DistrictID, address.DistrictName,
                address.RuralCode, address.NeighbourhoodID, address.NeighbourhoodName, address.StreetID, address.StreetName, address.ApartmentID,
                  address.ApartmentNo, address.AddressText, address.AddressNo, address.DoorID, address.DoorNo, result.Floor,
                result._PostalCode, result.BirthPlace, result.FatherName,
               result.MotherFirstSurname, result.MotherName, result.Nationality, 962,
               result.Sex, result.sBirthDate, result.IDCardType, result.FirstName,
               result.LastName, result.TC, result.SerialNo, result.PlaceOfIssue,
               result.sDateOfIssue, null, result.PhoneNumber, "tr-tr", result.EmailAddress, result.ReferenceCode, result.TariffId,

               result.RowNo, result.VolumeNo, result.PageNo, result.IDCardProvince, result.IDCardDistrict, result.IDCardNeighbourhood);

            applicationLogger.Error($"{responseNewCustomerRegister.ResponseMessage.ErrorMessage} -  Application result (end)");


            if (responseNewCustomerRegister.ResponseMessage.ErrorCode == 0)
            {
                return RedirectToAction("ApplicationConfirm", "Application");
            }

            if (responseNewCustomerRegister.ResponseMessage.ErrorCode == 7)
            {
                return RedirectToAction("AlreadyHaveCustomer", "Application");
            }

            if (responseNewCustomerRegister.ResponseMessage.ErrorCode == 199)
            {
                applicationLogger.Error($"{responseNewCustomerRegister.ResponseMessage.ErrorMessage} - Internal Server Error (NewCustomerRegister)");
                return RedirectToAction("InternalServerError", "Application");
            }

            if (responseNewCustomerRegister.ResponseMessage.ErrorCode == 200)
            {
                ValidationError = "Kimlik Bilginiz Hatalı, Lütfen Kimlik Bilgilerinizi Kontrol Ediniz.";
                //Session["ValidationError"] = ValidationError;
                TempData["ValidationError"] = ValidationError;
                return RedirectToAction("Index", "Application");

            }
            return RedirectToAction("ApplicationFail", "Application");
        }

        public ActionResult ApplicationFail()
        {
            return View();
        }

        public ActionResult Test()
        {
            //var exTimek = DateTime.Now;
            //var exTime = exTimek.AddMinutes(2);
            //var exTime = 120;

            //var exTimeT = (Value.ExpirationDate - DateTime.Now).TotalSeconds;


            //TempData["exTimeT"] = exTime;
            //TempData["exTime"] = exTime;

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


        public ActionResult TestId()
        {
            //var exTimek = DateTime.Now;
            //var exTime = exTimek.AddMinutes(2);
            var exTime = 120;

            TempData["exTime"] = exTime;
            //TempData["exTime"] = exTime;

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

    }
}