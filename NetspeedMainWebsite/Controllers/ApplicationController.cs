using NetspeedMainWebsite.MainSiteServiceReference;
using NetspeedMainWebsite.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Controllers
{
    public class ApplicationController : Controller
    {
        MainSiteServiceClient client = new MainSiteServiceClient();


        //public ActionResult ApplicationSummary()
        //{
        //    return View();
        //}

        //public ActionResult ApplicationSummary(ApplicationViewModel application)
        //{

        //    return View();
        //}

        public ActionResult _IDInformation()
        {
            return View();
        }
        public ActionResult GsmVerification(){
            return View();
        }
        public ActionResult ApplicationConfirm()

        {
            return View();
        }
        public ActionResult Index()
        {
            int NowYear ;
            int NowMonth;
            int NowDay;
            NowYear = DateTime.Now.Year;
            NowMonth = DateTime.Now.Month;
            NowDay = DateTime.Now.Day;

            int DaysInMonth = DateTime.DaysInMonth(NowYear, 2);
          

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


            var randomKeyIDCard = Guid.NewGuid().ToString();
            var serviceRequestHashIDCard= Utilities.ClientUtilities(randomKeyIDCard, Properties.Settings.Default.WebUserName,Properties.Settings.Default.PasswordForHash);
           
            var responseIDCard = client.GetIDCardTypes(new NetspeedServiceRequests()
            {
                Culture = "tr-tr", //burada veriyoruz zaten
                Rand = randomKeyIDCard, //gönder
                Hash = serviceRequestHashIDCard, //sonucu al
                Username = Properties.Settings.Default.WebUserName
            });
            var IDCardItems = responseIDCard.ValueNamePairList.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Code.ToString()
            });



            var randomKeySex = Guid.NewGuid().ToString();
            var serviceRequestHashSex = Utilities.ClientUtilities(randomKeySex, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var responseSex = client.GetSexes(new NetspeedServiceRequests()
            {
                Culture = "tr-tr",
                Rand = randomKeySex,
                Hash = serviceRequestHashSex,
                Username = Properties.Settings.Default.WebUserName
            });
            var SexItems = responseSex.ValueNamePairList.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Code.ToString()
            });

            var randomKeyNat = Guid.NewGuid().ToString();
            var serviceRequestHashNat = Utilities.ClientUtilities(randomKeyNat, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var responseNat = client.GetNationalities(new NetspeedServiceRequests()
            {
                Culture = "tr-tr",
                Rand = randomKeyNat,
                Hash = serviceRequestHashNat,
                Username = Properties.Settings.Default.WebUserName
            });
            var NatItems = responseNat.ValueNamePairList.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Code.ToString()
            });

            var randomKeyPro = Guid.NewGuid().ToString();
            var serviceRequestHashPro = Utilities.ClientUtilities(randomKeyPro, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var response = client.GetProvinces(new NetspeedServiceRequests()
            {
                Culture = "tr-tr",
                Rand = randomKeyPro,
                Hash = serviceRequestHashPro,
                Username = Properties.Settings.Default.WebUserName
            });

            var ProvinceItems = response.ValueNamePairList.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Code.ToString()
                
            });

            return View(new ApplicationViewModel() { ProvinceList = ProvinceItems, SexList = SexItems, NationalityList= NatItems, IDCardTypeList= IDCardItems });
        }


        //public ActionResult k()
        //{
        //    var randomKey = Guid.NewGuid().ToString();
        //    var username = "elif";
        //    var passwordHash = HashUtilities.HashCalculate("123456");
        //    var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

        //    var response = client.GetSexes(new NetspeedServiceRequests()
        //    {
        //        Culture = "tr-tr",
        //        Rand = randomKey,
        //        Hash = serviceRequestHash,
        //        Username = username
        //    });
        //    var SexItems=response.ValueNamePairList.Select(s => new SelectListItem()
        //    { 
        //        Text=s.Name,
        //        Value=s.Code.ToString()
        //    });

        //    return View(new ApplicationViewModel() { SexList = SexItems });
        //}
       

            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDistricts(long code)
        {
            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetProvinceDistricts(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                ItemCode = code
            });

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
            var randomKey = Guid.NewGuid().ToString();
            var serviceRequestHash = Utilities.ClientUtilities(randomKey, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var response = client.GetDistrictRuralRegions(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = Properties.Settings.Default.WebUserName,
                ItemCode = code
            });

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
            var randomKey = Guid.NewGuid().ToString();
            var serviceRequestHash = Utilities.ClientUtilities(randomKey, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var response = client.GetRuralRegionNeighbourhoods(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = Properties.Settings.Default.WebUserName,
                ItemCode = code
            });
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

          
            var randomKey = Guid.NewGuid().ToString();
            var serviceRequestHash = Utilities.ClientUtilities(randomKey, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var response = client.GetNeighbourhoodStreets(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = Properties.Settings.Default.WebUserName,
                ItemCode = code
            });

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
            var randomKey = Guid.NewGuid().ToString();
            var serviceRequestHash = Utilities.ClientUtilities(randomKey, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var response = client.GetStreetBuildings(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = Properties.Settings.Default.WebUserName,
                ItemCode = code
            });

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
            var randomKey = Guid.NewGuid().ToString();
            var serviceRequestHash = Utilities.ClientUtilities(randomKey, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);


            var response = client.GetBuildingApartments(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = Properties.Settings.Default.WebUserName,
                ItemCode = code
            });
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
            //int NowYear;
            //int NowMonth;
            //int NowDay;
            //NowYear = DateTime.Now.Year;
            //NowMonth = DateTime.Now.Month;
            //NowDay = DateTime.Now.Day;
            int DaysInMonth = DateTime.DaysInMonth(year, month);

            List<SelectListItem> Days = new List<SelectListItem>();
            for (int d = 1; d <= DaysInMonth; d++)
            {
                Days.Add(new SelectListItem { Text = d.ToString(), Value = d.ToString() });
            }

            //ViewBag.Days = Days;

            //return View(new ApplicationViewModel() { BirthDayList = Days });



            //var DistrictItems = response.ValueNamePairList.Select(r => new
            //{
            //    Text = r.Name,
            //    Value = r.Code.ToString()
            //});

            if (Days == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(Days, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        //public ActionResult test1(ApplicationViewModel application)
        public ActionResult Index(ApplicationViewModel application)
        //public ActionResult Index(ApplicationViewModel application)
        {
            DateTime dt = new DateTime(1995, 03, 28);
            if (ModelState.IsValid)
            {

                //DateTime bd = new DateTime(application.BirthYear, application.BirthMonth, application.BirthDay);
                var ApplicationList = new List<ApplicationViewModel>();

                ApplicationList.Add(new ApplicationViewModel()
                {
                    FirstName = application.FirstName.ToUpper(),
                    LastName = application.LastName.ToUpper(),
                    BirthDate = dt,
                    BirthPlace = application.BirthPlace.ToUpper(),
                    ContactPhoneNo = application.PhoneNumber,
                    //PhoneNumber = application.PhoneNumber,
                    TC = application.TC,
                    EmailAddress = application.EmailAddress,
                    MotherName = application.MotherName.ToUpper(),
                    FatherName = application.FatherName.ToUpper(),
                    MotherFirstSurname = application.MotherFirstSurname.ToUpper(),
                    PostalCode = application.PostalCode,
                    Floor = application.Floor,
                    ApartmentId = application.ApartmentId,
                    DistrictId = application.DistrictId,
                    BuildingId = application.BuildingId,
                    ProvinceId = application.ProvinceId,
                    NeighborhoodId = application.NeighborhoodId,
                    RegionId = application.RegionId,
                    StreetId = application.StreetId,
                    SerialNo = application.SerialNo.ToUpper(),
                    IDCardType=application.IDCardType,
                    Nationality=application.Nationality,
                    Sex=application.Sex
                    
                });
                var ApplicationItemList = ApplicationList.ToList();

                Session["ApplicationItemList"] = ApplicationItemList;
                return RedirectToAction("GsmVerification", "Application");

            }
            //return PartialView("ApplicationParts/_ApplicationValidation");
            return View(application);
        }

        [HttpPost]
        public ActionResult GsmVerification(ApplicationViewModel application)
        {
            var Gsm = string.Empty;

            Gsm = application.PhoneNumber;

            var randomKey = Guid.NewGuid().ToString();
            var serviceRequestHash = Utilities.ClientUtilities(randomKey, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var response = client.SendGenericSMS(new NetspeedServiceSendGenericSMSRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = Properties.Settings.Default.WebUserName,
                SendGenericSMSParameters = new SendGenericSMSRequest
                {
                    CustomerPhoneNo = application.PhoneNumber
                }

            });
            Session["Gsm"] = Gsm;
            //return RedirectToAction("ApplicationGet", "Application");
            return RedirectToAction("GsmVerificationWithSms", "Application");

        }

        //[HttpPost]

        public ActionResult GsmVerificationWithSms()
        {
           return View();
        }

        [HttpPost]
        public ActionResult GsmVerificationWithSms(VerificationViewModel verification)
        {
            var message = string.Empty;
            var Gsm = Session["Gsm"];

            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            if (Session["Action"] == null || (DateTime.Now - (DateTime)(Session["Action"])).Minutes > 3)
            {
                var response = client.RegisterSMSValidation(new NetspeedServiceRegisterSMSValidationRequest()
                {
                    Culture = "tr-tr",
                    Rand = randomKey,
                    Hash = serviceRequestHash,
                    Username = Properties.Settings.Default.WebUserName,
                    RegisterSMSValidationParameters = new RegisterSMSValidationRequest
                    {
                        CustomerPhoneNo = Gsm.ToString(),
                        Password = verification.Code
                    }
                });
                Session["Action"] = DateTime.Now;
                if (response.ResponseMessage.ErrorCode == 0)
                {
                    return RedirectToAction("ApplicationSummary", "Application");
                }
               
            }
            else
            {
                return View();
            }
            return View();

            
            ViewBag.message = "Lütfen Kodu Kontrol Ediniz.";

            return View();
        }

        public ActionResult ApplicationFail()
        {
            return View();
        }

        public ActionResult ApplicationSummary()
        {
            var ApplicationItemList = (List<ApplicationViewModel>)Session["ApplicationItemList"];
            var Gsm = Session["Gsm"];
            string SerialNo = "A25I96170";
            DateTime dti = new DateTime(2029, 12, 26);

            var randomKey = Guid.NewGuid().ToString();
            var serviceRequestHash = Utilities.ClientUtilities(randomKey, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var getAddress = client.GetApartmentAddress(new NetspeedServiceAddressDetailsRequest()
            {
                //BBK = ApplicationItemList[0].BuildingId,
                BBK = ApplicationItemList[0].ApartmentId,
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = Properties.Settings.Default.WebUserName
            });
            var address = getAddress.AddressDetailsResponse;

            var ConfirmList = new List<ApplicationViewModel>();
            ConfirmList.Add(new ApplicationViewModel()
            {
                FirstName = ApplicationItemList[0].FirstName,
                LastName = ApplicationItemList[0].LastName,
                PhoneNumber = ApplicationItemList[0].PhoneNumber,
                EmailAddress= ApplicationItemList[0].EmailAddress,
                AddressText = address.AddressText

            });
            

            return View(ConfirmList);
        }



        //[HttpPost]
        public ActionResult ApplicationGet(ApplicationViewModel application)
        //public ActionResult Index(ApplicationViewModel application)
        {
            var ApplicationItemList = (List<ApplicationViewModel>)Session["ApplicationItemList"];
            var Gsm = Session["Gsm"];
            string SerialNo = "A25I96170";
            DateTime dti = new DateTime(2029, 12, 26);

            var randomKey = Guid.NewGuid().ToString();
            var serviceRequestHash = Utilities.ClientUtilities(randomKey, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);

            var getAddress = client.GetApartmentAddress(new NetspeedServiceAddressDetailsRequest()
            {
                //BBK = ApplicationItemList[0].BuildingId,
                BBK = ApplicationItemList[0].ApartmentId,
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = Properties.Settings.Default.WebUserName,
            });
            var address = getAddress.AddressDetailsResponse;

            var randomKeyGetApartment = Guid.NewGuid().ToString();
            var serviceRequestHashGetApartment = Utilities.ClientUtilities(randomKey, Properties.Settings.Default.WebUserName, Properties.Settings.Default.PasswordForHash);


            var response = client.NewCustomerRegister(new NetspeedServiceNewCustomerRegisterRequest()
            {
                Culture = "tr-tr",
                Rand = randomKeyGetApartment,
                Hash = serviceRequestHashGetApartment,
                Username = Properties.Settings.Default.WebUserName,
                CustomerRegisterParameters = new NewCustomerRegisterRequest()
                {
                    SubscriptionInfo = new SubscriptionRegistrationInfo()
                    {
                        BillingPeriod = 1,
                        DomainID = 1,
                        ServiceID = 1,
                        SetupAddress = new AddressInfo()
                        {
                            ProvinceID = address.ProvinceID,
                            RuralCode = address.RuralCode,
                            DistrictID = address.DistrictID,
                            NeighbourhoodID = address.NeighbourhoodID,
                            StreetID = address.StreetID,
                            ApartmentID = address.ApartmentID,
                            DoorID = address.DoorID,
                            ProvinceName = address.ProvinceName,
                            DistrictName = address.DistrictName,
                            NeighbourhoodName = address.NeighbourhoodName,
                            StreetName = address.StreetName,
                            ApartmentNo = address.ApartmentNo,
                            AddressText = address.AddressText,
                            DoorNo = address.DoorNo,
                            Floor = ApplicationItemList[0].Floor,
                            PostalCode = ApplicationItemList[0].PostalCode,
                            AddressNo = address.AddressNo,

                        }
                    },
                    IndividualCustomerInfo = new IndividualCustomerInfo()
                    {

                        BirthPlace = ApplicationItemList[0].BirthPlace/*"İZMİR"*/,
                        FathersName = ApplicationItemList[0].FatherName/*"HÜSEYİN"*/,
                        MothersMaidenName = ApplicationItemList[0].MotherFirstSurname /*"KALAYCIOĞULLARI"*/,
                        MothersName = ApplicationItemList[0].MotherName/*"GÜLŞAH"*/,
                        Nationality = (int)ApplicationItemList[0].Nationality,
                        Profession = 962,
                        Sex = (int)ApplicationItemList[0].Sex,
                        ResidencyAddress = new AddressInfo()
                        {
                            ProvinceID = address.ProvinceID,
                            RuralCode = address.RuralCode,
                            DistrictID = address.DistrictID,
                            NeighbourhoodID = address.NeighbourhoodID,
                            StreetID = address.StreetID,
                            ApartmentID = address.ApartmentID,
                            DoorID = address.DoorID,
                            ProvinceName = address.ProvinceName,
                            DistrictName = address.DistrictName,
                            NeighbourhoodName = address.NeighbourhoodName,
                            StreetName = address.StreetName,
                            ApartmentNo = address.ApartmentNo,
                            AddressText = address.AddressText,
                            DoorNo = address.DoorNo,
                            Floor = ApplicationItemList[0].Floor,
                            PostalCode = ApplicationItemList[0].PostalCode,
                            AddressNo = address.AddressNo
                        },
                    },
                    IDCardInfo = new IDCardInfo()
                    {
                        BirthDate = new DateTime(1995, 03, 28), //ApplicationItemList[0].BirthDate,
                        CardType = (int)ApplicationItemList[0].IDCardType,
                        FirstName =/* "ELİF",*/ ApplicationItemList[0].FirstName,
                        LastName = /*"FINDIK",*/ ApplicationItemList[0].LastName,
                        TCKNo = /*"12862964604",*/ ApplicationItemList[0].TC,
                        SerialNo = /*"A25I96170",*/ApplicationItemList[0].SerialNo,
                        PlaceOfIssue = /*"İZMİR",*/ApplicationItemList[0].PlaceOfIssue,
                        DateOfIssue = new DateTime(2019, 12, 26),
                    },
                    CustomerGeneralInfo = new CustomerGeneralInfo()
                    {
                        OtherPhoneNos = null,
                        BillingAddress = new AddressInfo()
                        {
                            ProvinceID = address.ProvinceID,
                            RuralCode = address.RuralCode,
                            DistrictID = address.DistrictID,
                            NeighbourhoodID = address.NeighbourhoodID,
                            StreetID = address.StreetID,
                            ApartmentID = address.ApartmentID,
                            DoorID = address.DoorID,
                            ProvinceName = address.ProvinceName,
                            DistrictName = address.DistrictName,
                            NeighbourhoodName = address.NeighbourhoodName,
                            StreetName = address.StreetName,
                            ApartmentNo = address.ApartmentNo,
                            AddressText = address.AddressText,
                            DoorNo = address.DoorNo,
                            Floor = ApplicationItemList[0].Floor,
                            PostalCode = ApplicationItemList[0].PostalCode,
                            AddressNo = address.AddressNo

                        },
                        //ContactPhoneNo = ApplicationItemList[0].PhoneNumber,
                        ContactPhoneNo = Gsm.ToString()/*"5465939624"*/,
                        Culture = "tr-tr",
                        //CustomerType = application.CustomerType,
                        CustomerType = 1,
                        Email = ApplicationItemList[0].EmailAddress,

                        //OtherPhoneNos=new PhoneNoListItem()
                        //{
                        //    //Number=,
                        //}

                    },
                    CorporateCustomerInfo = null
                }
            });

            if (response.ResponseMessage.ErrorCode == 0)
            {
                return RedirectToAction("ApplicationConfirm", "Application");
            }

            return RedirectToAction("ApplicationFail", "Application");
        }
    }
}