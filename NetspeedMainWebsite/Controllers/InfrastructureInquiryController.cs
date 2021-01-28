using NetspeedMainWebsite.Models.ViewModel;
using NetspeedMainWebsite.MainSiteServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Controllers
{
    public class InfrastructureInquiryController : Controller
    {
        //GET: InfrastructureInquiry
        //MainSiteServiceClient client = new MainSiteServiceClient();
        WebServiceWrapper client = new WebServiceWrapper();
        public ActionResult Index()
        {
            var responseProvince = new WebServiceWrapper().GetProvinces();
            var ProvinceList = responseProvince.ValueNamePairList.Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Code.ToString()
            });

            ViewBag.ProvinceList = ProvinceList;
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

            return View();
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

        [HttpGet]
        public ActionResult InfrastructureInquiryResult(InfrastructureInquiryResultViewModel model)
        {
            return View();
        }


        [HttpPost]
        public ActionResult InfrastructureInquiryResult(InfrastructureInquiryViewModel inf, string apartmentId)
        {
            if(ModelState.IsValid)
            {
                var response = client.ServiceAvailability(apartmentId);

                InfrastructureInquiryResultViewModel InfrastructureResult = new InfrastructureInquiryResultViewModel();

                WebServiceWrapper clientAddres = new WebServiceWrapper();
                var getAddress = clientAddres.ServiceAvailability(apartmentId);

                WebServiceWrapper clientTariff = new WebServiceWrapper();
                var getTariff = clientTariff.GetTariffList();

                var Fiber = getAddress.ServiceAvailabilityResponse.FIBER;
                var Vdsl = getAddress.ServiceAvailabilityResponse.VDSL;
                var Adsl = getAddress.ServiceAvailabilityResponse.ADSL;



                if (Fiber.HasInfrastructureFiber)
                {
                    var displaySpeed = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Fiber.FiberSpeed.Value) * 1024, true);
                    InfrastructureResult.Distance = Fiber.FiberDistance.ToString();
                    //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.FiberSpeed.ToString();
                    InfrastructureResult.MaxSpeed = $"{displaySpeed.FieldValue} {displaySpeed.RateSuffix}";
                    InfrastructureResult.XDSLType = "FİBER";
                    InfrastructureResult.PortState = Fiber.FiberPortState.ToString();
                    InfrastructureResult.SVUID = Fiber.FiberSVUID.ToString();

                    var TariffItems = getTariff.ExternalTariffList.Where(f => f.HasFiber == true).Select(t => new TariffsViewModel
                    {
                        TariffID = t.TariffID,
                        DisplayName = t.DisplayName,
                        Price = t.Price,
                        Speed = t.Speed,
                    });
                    //InfrastructureResult.TariffList = TariffItems.ToArray();
                }
                else if (Vdsl.HasInfrastructureVdsl && Vdsl.VdslSpeed > Adsl.AdslSpeed)
                {
                    var displaySpeedVdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Vdsl.VdslSpeed.Value) * 1024, true);
                    InfrastructureResult.MaxSpeed = $"{displaySpeedVdsl.FieldValue} {displaySpeedVdsl.RateSuffix}";
                    InfrastructureResult.Distance = Vdsl.VdslDistance.ToString();
                    //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.VdslSpeed.ToString();
                    InfrastructureResult.XDSLType = "VDSL";
                    InfrastructureResult.PortState = Vdsl.VdslPortState.ToString();
                    InfrastructureResult.SVUID = Vdsl.VdslSVUID.ToString();
                    //return View(InfrastructureResult);

                    var TariffItems = getTariff.ExternalTariffList.Where(f => f.HasXDSL == true).Select(t => new TariffsViewModel
                    {
                        TariffID = t.TariffID,
                        DisplayName = t.DisplayName,
                        Price = t.Price,
                        Speed = t.Speed,
                    });
                    //InfrastructureResult.TariffList = TariffItems.ToArray();
                }
                else if (Adsl.HasInfrastructureAdsl && Adsl.AdslSpeed > Vdsl.VdslSpeed)
                {
                    //var displaySpeed = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)response.ServiceAvailabilityResponse.FiberSpeed.Value) * 1024, true);
                    var displaySpeedAdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Adsl.AdslSpeed.Value) * 1024, true);
                    InfrastructureResult.MaxSpeed = $"{displaySpeedAdsl.FieldValue} {displaySpeedAdsl.RateSuffix}";
                    InfrastructureResult.Distance = Adsl.AdslDistance.ToString();
                    //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.AdslSpeed.ToString();
                    InfrastructureResult.XDSLType = "ADSL";
                    InfrastructureResult.PortState = Adsl.AdslPortState.ToString();
                    InfrastructureResult.SVUID = Adsl.AdslSVUID.ToString();
                    //return View(InfrastructureResult);
                    var TariffItems = getTariff.ExternalTariffList.Where(f => f.HasXDSL == true).Select(t => new TariffsViewModel
                    {
                        TariffID = t.TariffID,
                        DisplayName = t.DisplayName,
                        Price = t.Price,
                        Speed = t.Speed,
                    });
                    //InfrastructureResult.TariffList = TariffItems.ToArray();
                }
                else//DÜZENLE
                {
                    InfrastructureResult.Message = response.ResponseMessage.ErrorMessage;
                    InfrastructureResult.Distance = "-";
                    InfrastructureResult.MaxSpeed = "Sorguladığınız haneye ait altyapı bilgisi bulunamadı.";
                    InfrastructureResult.XDSLType = "";
                    InfrastructureResult.PortState = "Yok";
                }
                //}
                //else
                //{
                //    var message = string.Empty;
                //    message = "Lütfen Tüm Alanları Doldurunuz.";
                //    TempData["message"] = message;
                //    return RedirectToAction("Index", "InfrastructureInquiry");
                //}
                return View(InfrastructureResult);
            }
            var responseProvince = new WebServiceWrapper().GetProvinces();
            var ProvinceList = responseProvince.ValueNamePairList.Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Code.ToString()
            });

            ViewBag.ProvinceList = ProvinceList;
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

            return View(viewName: "Index", model: inf);

       
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
    }
}