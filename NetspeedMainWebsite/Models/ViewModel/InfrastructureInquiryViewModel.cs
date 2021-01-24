using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class InfrastructureInquiryViewModel
    {
        public string BBK { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? RegionId { get; set; }
        public long? NeighborhoodId { get; set; }
        public long? StreetId { get; set; }
        public long? BuildingId { get; set; }
        public long? ApartmentId { get; set; }
       
    }
}