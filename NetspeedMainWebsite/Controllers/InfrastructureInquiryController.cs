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
            var response = client.GetProvinces();
            var ProvinceItems = response.ValueNamePairList.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Code.ToString()
            });

            return View(new InfrastructureInquiryViewModel() { Province = ProvinceItems });
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
        public ActionResult InfrastructureInquiryResult(string ApartmentId)
        {
            var response = client.ServiceAvailability(ApartmentId);
            //var InfrastructureItems = response.

            InfrastructureInquiryResultViewModel InfrastructureResult = new InfrastructureInquiryResultViewModel();

            var Fiber = response.ServiceAvailabilityResponse.FIBER;
            var Vdsl = response.ServiceAvailabilityResponse.VDSL;
            var Adsl = response.ServiceAvailabilityResponse.ADSL;



            if (Fiber.HasInfrastructureFiber)
            {
                //var displaySpeed = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)response.ServiceAvailabilityResponse.FIBER.FiberSpeed.Value) * 1024, true);

                var displaySpeed = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Fiber.FiberSpeed.Value) * 1024, true);
                InfrastructureResult.Distance = Fiber.FiberDistance.ToString();
                //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.FiberSpeed.ToString();
                InfrastructureResult.MaxSpeed = $"{displaySpeed.FieldValue} {displaySpeed.RateSuffix}";
                InfrastructureResult.XDSLType = Fiber.HasInfrastructureFiber.ToString(); //"fiber"
                InfrastructureResult.PortState = Fiber.FiberPortState.ToString();
                InfrastructureResult.SVUID = Fiber.FiberSVUID.ToString();
                return View(InfrastructureResult);
            }

            if (Vdsl.HasInfrastructureVdsl  && Vdsl.VdslSpeed > Adsl.AdslSpeed)
            {
                var displaySpeedVdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Vdsl.VdslSpeed.Value) * 1024, true);
                InfrastructureResult.MaxSpeed = $"{displaySpeedVdsl.FieldValue} {displaySpeedVdsl.RateSuffix}";

                InfrastructureResult.Distance = Vdsl.VdslDistance.ToString();
                //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.VdslSpeed.ToString();
                InfrastructureResult.XDSLType = Vdsl.HasInfrastructureVdsl.ToString(); //"vdsl"
                InfrastructureResult.PortState = Vdsl.VdslPortState.ToString();
                InfrastructureResult.SVUID = Vdsl.VdslSVUID.ToString();
                return View(InfrastructureResult);
            }


            if (Adsl.HasInfrastructureAdsl && Adsl.AdslSpeed > Vdsl.VdslSpeed)
            {
                //var displaySpeed = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)response.ServiceAvailabilityResponse.FiberSpeed.Value) * 1024, true);
                var displaySpeedAdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Adsl.AdslSpeed.Value) * 1024, true);
                InfrastructureResult.MaxSpeed = $"{displaySpeedAdsl.FieldValue} {displaySpeedAdsl.RateSuffix}";


                InfrastructureResult.Distance = Adsl.AdslDistance.ToString();
                //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.AdslSpeed.ToString();
                InfrastructureResult.XDSLType = Adsl.HasInfrastructureAdsl.ToString(); //"adsl"
                InfrastructureResult.PortState = Adsl.AdslPortState.ToString();
                InfrastructureResult.SVUID = Adsl.AdslSVUID.ToString();
                return View(InfrastructureResult);
            }



            //if (response.ServiceAvailabilityResponse.AdslSpeed != 0 && response.ServiceAvailabilityResponse.AdslSpeed > response.ServiceAvailabilityResponse.VdslSpeed)
            //{
            //    //var displaySpeed = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)response.ServiceAvailabilityResponse.FiberSpeed.Value) * 1024, true);
            //    var displaySpeedAdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)response.ServiceAvailabilityResponse.AdslSpeed) * 1024, true);
            //    InfrastructureResult.MaxSpeed = $"{displaySpeedAdsl.FieldValue} {displaySpeedAdsl.RateSuffix}";


            //    InfrastructureResult.Distance = response.ServiceAvailabilityResponse.AdslSpeed.ToString();
            //    //InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.AdslSpeed.ToString();
            //    InfrastructureResult.XDSLType = response.ServiceAvailabilityResponse.HasInfrastructureAdsl.ToString(); //"adsl"
            //    InfrastructureResult.PortState = response.ServiceAvailabilityResponse.VdslPortState.ToString();
            //    InfrastructureResult.SVUID = response.ServiceAvailabilityResponse.AdslSVUID.ToString();
            //    return View(InfrastructureResult);
            //}


            InfrastructureResult.Message = response.ResponseMessage.ErrorMessage;
            InfrastructureResult.Distance = "-";
            InfrastructureResult.MaxSpeed = "Sorguladığınız haneye ait altyapı bilgisi bulunamadı.";
            InfrastructureResult.XDSLType = "";
            InfrastructureResult.PortState = "Yok";


            return View(InfrastructureResult);



        }
    }
}