using NetspeedMainWebsite.Models.ViewModel;
using NetspeedMainWebsite.MainSiteServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.AddressUtilities;
using NLog;

namespace NetspeedMainWebsite.Controllers
{
    public class InfrastructureInquiryController : BaseController
    {
        //GET: InfrastructureInquiry
        AddressUtility addressUtil = new AddressUtility();

        Logger infrastructureLogger = LogManager.GetLogger("infrastructure");
        Logger tariffLogger = LogManager.GetLogger("tariffs");

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
        public ActionResult GetDistricts(long code)
        {
            var result = addressUtil.GetProvinceDistricts(code);
            if (result == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Select(item => new
            {
                Text = item.Value,
                Value = item.Key
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRegions(long code)
        {
            var result = addressUtil.GetDistrictRegions(code);

            if (result == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Select(item => new
            {
                Text = item.Value,
                Value = item.Key
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetNeighborhoods(long code)
        {
            var result = addressUtil.GetRegionNeighbourhoods(code);

            if (result == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Select(item => new
            {
                Text = item.Value,
                Value = item.Key
            }), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetStreets(long code)
        {
            var result = addressUtil.GetNeighbourhoodStreets(code);

            if (result == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Select(item => new
            {
                Text = item.Value,
                Value = item.Key
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetBuildings(long code)
        {
            var result = addressUtil.GetStreetBuildings(code);

            if (result == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Select(item => new
            {
                Text = item.Value,
                Value = item.Key
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetApartments(long code)
        {
            var result = addressUtil.GetBuildingAparments(code);

            if (result == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(result.Select(item => new
            {
                Text = item.Value,
                Value = item.Key
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InfrastructureInquiryResult(InfrastructureInquiryResultViewModel model)
        {
            return View();
        }
        [HttpPost]
        public ActionResult InfrastructureInquiryResult(string apartmentId)
        {
            if (!string.IsNullOrEmpty(apartmentId))
            {
                var client = new WebServiceWrapper();
                //var response = client.ServiceAvailability(apartmentId);

                InfrastructureInquiryResultViewModel InfrastructureResult = new InfrastructureInquiryResultViewModel();

                WebServiceWrapper clientAddres = new WebServiceWrapper();
                var getAddress = clientAddres.ServiceAvailability(apartmentId);                

                if (getAddress.ResponseMessage.ErrorCode != 0)
                {
                    infrastructureLogger.Error($"{getAddress.ResponseMessage.ErrorMessage} - Error (ServiceAvailability)");
                }
                var Fiber = getAddress.ServiceAvailabilityResponse.FIBER;
                var Vdsl = getAddress.ServiceAvailabilityResponse.VDSL;
                var Adsl = getAddress.ServiceAvailabilityResponse.ADSL;
                if (Fiber.HasInfrastructureFiber)
                {
                    var displaySpeed = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Fiber.FiberSpeed.Value) * 1024, true);
                    InfrastructureResult.Distance = Fiber.FiberDistance.ToString();
                    InfrastructureResult.MaxSpeed = $"{displaySpeed.FieldValue} {displaySpeed.RateSuffix}";
                    InfrastructureResult.XDSLType = "FİBER";
                    InfrastructureResult.PortState = Fiber.FiberPortState.ToString();
                    InfrastructureResult.SVUID = Fiber.FiberSVUID.ToString();
                }
                else if (Vdsl.HasInfrastructureVdsl && Vdsl.VdslSpeed > Adsl.AdslSpeed)
                {
                    var displaySpeedVdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Vdsl.VdslSpeed.Value) * 1024, true);
                    InfrastructureResult.MaxSpeed = $"{displaySpeedVdsl.FieldValue} {displaySpeedVdsl.RateSuffix}";
                    InfrastructureResult.Distance = Vdsl.VdslDistance.ToString();
                    InfrastructureResult.XDSLType = "VDSL";
                    InfrastructureResult.PortState = Vdsl.VdslPortState.ToString();
                    InfrastructureResult.SVUID = Vdsl.VdslSVUID.ToString();
                }
                else if (Adsl.HasInfrastructureAdsl && Adsl.AdslSpeed > Vdsl.VdslSpeed)
                {
                    var displaySpeedAdsl = RezaB.Data.Formating.RateLimitFormatter.ToTrafficMixedResults(((decimal)Adsl.AdslSpeed.Value) * 1024, true);
                    InfrastructureResult.MaxSpeed = $"{displaySpeedAdsl.FieldValue} {displaySpeedAdsl.RateSuffix}";
                    InfrastructureResult.Distance = Adsl.AdslDistance.ToString();
                    InfrastructureResult.XDSLType = "ADSL";
                    InfrastructureResult.PortState = Adsl.AdslPortState.ToString();
                    InfrastructureResult.SVUID = Adsl.AdslSVUID.ToString();
                }
                else
                {
                    InfrastructureResult.Message = getAddress.ResponseMessage.ErrorMessage;
                    InfrastructureResult.Distance = null;
                    InfrastructureResult.MaxSpeed = "Sorguladığınız haneye ait altyapı bilgisi bulunamadı.";
                    InfrastructureResult.XDSLType = "";
                    InfrastructureResult.PortState = "Yok";
                }
                Session["InfrastructureResult"] = InfrastructureResult;
                return RedirectToAction("InfrastructureResult", "Home");
            }
            return RedirectToAction("InfrastructureCheck", "Home");
        }
    }
}