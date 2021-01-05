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
        MainSiteServiceClient client = new MainSiteServiceClient();

        public ActionResult Test()
        {
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

            //ViewBag.ProvinceList = new SelectList(ProvinceItems, "Value", "Text");
            //var DistrictItems = new SelectList("", "");
            //ViewBag.DistrictList = DistrictItems;
            return View(new InfrastructureInquiryViewModel() { Province = ProvinceItems });
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

        [HttpGet]
        public ActionResult InfrastructureInquiryResult(InfrastructureInquiryResultViewModel model)
        {
            return View();
        }


        [HttpPost]
        public ActionResult InfrastructureInquiryResult(string ApartmentId)
        {
            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.ServiceAvailability(new NetspeedServiceServiceAvailabilityRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                ServiceAvailabilityParameters = new ServiceAvailabilityRequest
                {
                    bbk = ApartmentId
                }
            });
            //var InfrastructureItems = response.

            InfrastructureInquiryResultViewModel InfrastructureResult = new InfrastructureInquiryResultViewModel();

            if (response.ServiceAvailabilityResponse.FiberSpeed > 0 )
            {
                InfrastructureResult.Distance = response.ServiceAvailabilityResponse.FiberDistance.ToString();
                InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.FiberSpeed.ToString();
                InfrastructureResult.XDSLType = response.ServiceAvailabilityResponse.HasInfrastructureFiber.ToString(); //"fiber"
                InfrastructureResult.PortState = response.ServiceAvailabilityResponse.FiberPortState.ToString();
                InfrastructureResult.SVUID = response.ServiceAvailabilityResponse.FiberSVUID.ToString();
                return View(InfrastructureResult);
            }
            if (response.ServiceAvailabilityResponse.VdslSpeed != 0 && response.ServiceAvailabilityResponse.VdslSpeed > response.ServiceAvailabilityResponse.AdslSpeed)
            {
                InfrastructureResult.Distance = response.ServiceAvailabilityResponse.VdslDistance.ToString();
                InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.VdslSpeed.ToString();
                InfrastructureResult.XDSLType = response.ServiceAvailabilityResponse.HasInfrastructureVdsl.ToString(); //"vdsl"
                InfrastructureResult.PortState = response.ServiceAvailabilityResponse.VdslPortState.ToString();
                InfrastructureResult.SVUID = response.ServiceAvailabilityResponse.VdslSVUID.ToString();
                return View(InfrastructureResult);
            }
            if (response.ServiceAvailabilityResponse.AdslSpeed != 0 && response.ServiceAvailabilityResponse.AdslSpeed > response.ServiceAvailabilityResponse.VdslSpeed)
            {
                InfrastructureResult.Distance = response.ServiceAvailabilityResponse.AdslSpeed.ToString();
                InfrastructureResult.MaxSpeed = response.ServiceAvailabilityResponse.AdslSpeed.ToString();
                InfrastructureResult.XDSLType = response.ServiceAvailabilityResponse.HasInfrastructureAdsl.ToString(); //"adsl"
                InfrastructureResult.PortState = response.ServiceAvailabilityResponse.VdslPortState.ToString();
                InfrastructureResult.SVUID = response.ServiceAvailabilityResponse.AdslSVUID.ToString();
                return View(InfrastructureResult);
            }


            InfrastructureResult.Message = response.ResponseMessage.ErrorMessage;
            InfrastructureResult.Distance = "??";
            InfrastructureResult.MaxSpeed = "??";
            InfrastructureResult.XDSLType = "??";
            InfrastructureResult.PortState = "??";

            return View(InfrastructureResult);



        }
    }
}