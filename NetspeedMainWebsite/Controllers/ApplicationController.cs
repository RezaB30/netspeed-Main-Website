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
        WebSeviceWrapper client = new WebSeviceWrapper();


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

            var responseProvince = client.GetProvinces();
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


            var responseIDCard = client.GetIDCardTypes();

            var IDCardItems = responseIDCard.ValueNamePairList.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Code.ToString()
            });
            
            var responseNat = client.GetNationalities();
            var NatItems = responseNat.ValueNamePairList.Select(n => new SelectListItem()
            {
                Text = n.Name,
                Value = n.Code.ToString()
            });

            var responseSex = client.GetSexes();
            var SexItems = responseSex.ValueNamePairList.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Code.ToString()
            });

           
            return View(new ApplicationViewModel() { ProvinceList = ProvinceItems, SexList = SexItems, NationalityList = NatItems, IDCardTypeList = IDCardItems });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDistricts(long code)
        {
            var response = client.GetProvinceDistricts(code);
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
            var response = client.GetDistrictRuralRegions(code);

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
            var response = client.GetRuralRegionNeighbourhoods(code);

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
            var response = client.GetNeighbourhoodStreets(code);

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
            var response = client.GetStreetBuildings(code);

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
            var response = client.GetBuildingApartments(code);

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
                    IDCardType = application.IDCardType,
                    Nationality = application.Nationality,
                    Sex = application.Sex

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
            string Gsm = application.PhoneNumber;

            var response = client.SendGenericSMS(application.PhoneNumber);
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
        public ActionResult GsmVerificationWithSms(VerificationViewModel smsCode)
        {
            var message = string.Empty;
            string Gsm = Session["Gsm"].ToString();

            //var randomKey = Guid.NewGuid().ToString();
            //var username = "elif";
            //var passwordHash = HashUtilities.HashCalculate("123456");
            //var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            if (Session["Action"] == null || (DateTime.Now - (DateTime)(Session["Action"])).Minutes > 3)
            {
                var response = client.RegisterSMSValidation(Gsm, smsCode.ToString());//buraya dön

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

            var getAddress = client.GetApartmentAddress(ApplicationItemList[0].ApartmentId);
            var address = getAddress.AddressDetailsResponse;

            var ConfirmList = new List<ApplicationViewModel>();
            ConfirmList.Add(new ApplicationViewModel()
            {
                FirstName = ApplicationItemList[0].FirstName,
                LastName = ApplicationItemList[0].LastName,
                PhoneNumber = ApplicationItemList[0].PhoneNumber,
                EmailAddress = ApplicationItemList[0].EmailAddress,
                AddressText = address.AddressText

            });
            return View(ConfirmList);
        }


        [HttpPost]
        public ActionResult ApplicationGet(ApplicationViewModel application)
        //public ActionResult Index(ApplicationViewModel application)
        {
            var ApplicationItemList = (List<ApplicationViewModel>)Session["ApplicationItemList"];
            var Gsm = Session["Gsm"];
            string SerialNo = "A25I96170";
            DateTime dti = new DateTime(2029, 12, 26);


            var getAddress = client.GetApartmentAddress(ApplicationItemList[0].ApartmentId);

            var address = getAddress.AddressDetailsResponse;



            var response = client.NewCustomerRegister(1, 1, 1, address.ProvinceID, address.ProvinceName, address.DistrictID, address.DistrictName,
                address.RuralCode, address.NeighbourhoodID, address.NeighbourhoodName, address.StreetID, address.StreetName, address.ApartmentID,
                  address.ApartmentNo, address.AddressText, address.AddressNo, address.DoorID, address.DoorNo, ApplicationItemList[0].Floor,
                ApplicationItemList[0].PostalCode, ApplicationItemList[0].BirthPlace, ApplicationItemList[0].FatherName,
               ApplicationItemList[0].MotherFirstSurname, ApplicationItemList[0].MotherName, (int)ApplicationItemList[0].Nationality, 962,
               (int)ApplicationItemList[0].Sex, new DateTime(1995, 03, 28), (int)ApplicationItemList[0].IDCardType, ApplicationItemList[0].FirstName,
               ApplicationItemList[0].LastName, ApplicationItemList[0].TC, ApplicationItemList[0].SerialNo, ApplicationItemList[0].PlaceOfIssue,
               new DateTime(2019, 12, 26), null, Gsm.ToString(), "tr-tr", 1, ApplicationItemList[0].EmailAddress
               );
            {

                if (response.ResponseMessage.ErrorCode == 0)
                {
                    return RedirectToAction("ApplicationConfirm", "Application");
                }

                return RedirectToAction("ApplicationFail", "Application");
            }
        }
    }
}