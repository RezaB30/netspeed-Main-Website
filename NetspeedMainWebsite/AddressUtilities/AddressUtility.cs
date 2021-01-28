using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.AddressUtilities
{
    public class AddressUtility
    {
        public WebServiceWrapper InternalClient { get; private set; }

        public AddressUtility()
        {
            InternalClient = new WebServiceWrapper();
        }

        public Dictionary<long, string> GetProvinces()
        {
            var result = InternalClient.GetProvinces();
            if(result.ResponseMessage.ErrorCode == 0)
            {
                return result.ValueNamePairList.ToDictionary(r => r.Code, r => r.Name);
            }
            else
            {
                return null;
            }
        }

        public Dictionary<long, string> GetProvinceDistricts(long provinceId)
        {
            var result = InternalClient.GetProvinceDistricts(provinceId);
            if (result.ResponseMessage.ErrorCode == 0)
            {
                return result.ValueNamePairList.ToDictionary(r => r.Code, r => r.Name);
            }
            else
            {
                return null;
            }
        }

        public Dictionary<long, string> GetDistrictRegions(long districtId)
        {
            var result = InternalClient.GetDistrictRuralRegions(districtId);
            if (result.ResponseMessage.ErrorCode == 0)
            {
                return result.ValueNamePairList.ToDictionary(r => r.Code, r => r.Name);
            }
            else
            {
                return null;
            }
        }

        public Dictionary<long, string> GetRegionNeighbourhoods(long regionId)
        {
            var result = InternalClient.GetRuralRegionNeighbourhoods(regionId);
            if (result.ResponseMessage.ErrorCode == 0)
            {
                return result.ValueNamePairList.ToDictionary(r => r.Code, r => r.Name);
            }
            else
            {
                return null;
            }
        }

        public Dictionary<long, string> GetNeighbourhoodStreets(long neighbourhoodId)
        {
            var result = InternalClient.GetNeighbourhoodStreets(neighbourhoodId);
            if (result.ResponseMessage.ErrorCode == 0)
            {
                return result.ValueNamePairList.ToDictionary(r => r.Code, r => r.Name);
            }
            else
            {
                return null;
            }
        }

        public Dictionary<long, string> GetStreetBuildings(long streetId)
        {
            var result = InternalClient.GetStreetBuildings(streetId);
            if (result.ResponseMessage.ErrorCode == 0)
            {
                return result.ValueNamePairList.ToDictionary(r => r.Code, r => r.Name);
            }
            else
            {
                return null;
            }
        }

        public Dictionary<long, string> GetBuildingAparments(long buildingId)
        {
            var result = InternalClient.GetBuildingApartments(buildingId);
            if (result.ResponseMessage.ErrorCode == 0)
            {
                return result.ValueNamePairList.ToDictionary(r => r.Code, r => r.Name);
            }
            else
            {
                return null;
            }
        }

        //public Dictionary<long, string> GetBuildingAparments(long buildingId)
        //{
        //    var result = InternalClient.GetStreetBuildings(buildingId);
        //    if (result.ResponseMessage.ErrorCode == 0)
        //    {
        //        return result.ValueNamePairList.ToDictionary(r => r.Code, r => r.Name);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}