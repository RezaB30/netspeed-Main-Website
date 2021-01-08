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
            List<SelectListItem> Days = new List<SelectListItem>();
            for (int d = 1; d <= 31; d++)
            {
                Days.Add(new SelectListItem { Text = d.ToString(), Value = d.ToString() });
            }

            ViewBag.Days = Days;

            List<SelectListItem> Months = new List<SelectListItem>();
            for (int m = 1; m <= 12; m++)
            {
                Months.Add(new SelectListItem { Text = m.ToString(), Value = m.ToString() });
            }

            ViewBag.Months = Months;

            List<SelectListItem> Years = new List<SelectListItem>();
            for (int y = 1940; y <= 2003; y++)
            {
                Years.Add(new SelectListItem { Text = y.ToString(), Value = y.ToString() });
            }
            ViewBag.Years = Years;


            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var genericHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetProvinces(new NetspeedServiceRequests()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = genericHash,
                Username = username,
            });

            var ProvinceItems = response.ValueNamePairList.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });

            return View(new ApplicationViewModel() { ProvinceList = ProvinceItems });
        }



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
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");


            var response = client.GetDistrictRuralRegions(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
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
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");


            var response = client.GetRuralRegionNeighbourhoods(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
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
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetNeighbourhoodStreets(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
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
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetStreetBuildings(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
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
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.GetBuildingApartments(new NetspeedServiceArrayListRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
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
                    FirstName = application.FirstName,
                    LastName = application.LastName,
                    BirthDate = dt,
                    BirthPlace = application.BirthPlace,
                    ContactPhoneNo = application.PhoneNumber,
                    //PhoneNumber = application.PhoneNumber,
                    TC = "12862964604",
                    EmailAddress = application.EmailAddress,
                    MotherName = application.MotherName,
                    FatherName = application.FatherName,
                    MotherFirstSurname = application.MotherFirstSurname,
                    PostalCode = application.PostalCode,
                    Floor = application.Floor,
                    ApartmentId = application.ApartmentId,
                    DistrictId = application.DistrictId,
                    BuildingId = application.BuildingId,
                    ProvinceId = application.ProvinceId,
                    NeighborhoodId = application.NeighborhoodId,
                    RegionId = application.RegionId,
                    StreetId = application.StreetId,
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

            //Gsm.Add(new ApplicationViewModel()
            //{
            //    PhoneNumber = application.PhoneNumber
            //});
            Gsm = application.PhoneNumber;
            //return RedirectToAction("GsmVerificationWithSms", "Application");

            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");


            var response = client.SendGenericSMS(new NetspeedServiceSendGenericSMSRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
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


            var response = client.RegisterSMSValidation(new NetspeedServiceRegisterSMSValidationRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                RegisterSMSValidationParameters = new RegisterSMSValidationRequest
                {
                    CustomerPhoneNo = Gsm.ToString(),
                    Password = verification.Code
                }
            });
            if (response.ResponseMessage.ErrorCode==0)
            {
                return RedirectToAction("ApplicationSummary", "Application");
            }
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
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");
            var getAddress = client.GetApartmentAddress(new NetspeedServiceAddressDetailsRequest()
            {
                //BBK = ApplicationItemList[0].BuildingId,
                BBK = ApplicationItemList[0].ApartmentId,
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
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
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");
            var getAddress = client.GetApartmentAddress(new NetspeedServiceAddressDetailsRequest()
            {
                //BBK = ApplicationItemList[0].BuildingId,
                BBK = ApplicationItemList[0].ApartmentId,
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
            });
            var address = getAddress.AddressDetailsResponse;
            var randomKeyGetApartment = Guid.NewGuid().ToString();
            //var username = "elif";
            var passwordHashGetApartment = HashUtilities.HashCalculate("123456");
            var serviceRequestHashGetApartment = HashUtilities.HashCalculate($"{username}{randomKeyGetApartment}{passwordHashGetApartment}");
            var response = client.NewCustomerRegister(new NetspeedServiceNewCustomerRegisterRequest()
            {
                Culture = "tr-tr",
                Rand = randomKeyGetApartment,
                Hash = serviceRequestHashGetApartment,
                Username = username,
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
                        Nationality = 228,
                        Profession = 962,
                        Sex = 2,
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
                        CardType = 1,
                        FirstName =/* "ELİF",*/ ApplicationItemList[0].FirstName,
                        LastName = /*"FINDIK",*/ ApplicationItemList[0].LastName,
                        TCKNo = /*"12862964604",*/ ApplicationItemList[0].TC,
                        SerialNo = /*"A25I96170",*/SerialNo,
                        PlaceOfIssue = "İZMİR",/*ApplicationItemList[0].BirthPlace*/
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

            //var Applist = new List<ApplicationViewModel>();
            //Applist.Add(new ApplicationViewModel()
            //{
            //    FirstName = application.FirstName,
            //    LastName = application.LastName,
            //    PhoneNumber = application.PhoneNumber,
            //    EmailAddress = application.EmailAddress
            //});

            //Session["Applist"] = Applist;

            //return RedirectToAction("ApplicationSummary", "Application");

            return RedirectToAction("ApplicationFail", "Application");
        }
    }
}