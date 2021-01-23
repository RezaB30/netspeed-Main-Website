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
        //public InfrastructureInquiryViewModel()
        //{
        //    //Province = new List<SelectListItem>();
        //    //District = new List<SelectListItem>();
        //    //Region = new List<SelectListItem>();
        //    //Neighborhood = new List<SelectListItem>();
        //    //Street = new List<SelectListItem>();
        //    //Building = new List<SelectListItem>();
        //    //Apartment = new List<SelectListItem>();
        //}
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? RegionId { get; set; }
        public long? NeighborhoodId { get; set; }
        public long? StreetId { get; set; }
        public long? BuildingId { get; set; }
        public long? ApartmentId { get; set; }
        //public IEnumerable<SelectListItem> Province { get; set; }
        //public IEnumerable<SelectListItem> District { get; set; }
        //public IEnumerable<SelectListItem> Region { get; set; }
        //public IEnumerable<SelectListItem> Neighborhood { get; set; }
        //public IEnumerable<SelectListItem> Street { get; set; }
        //public IEnumerable<SelectListItem> Building { get; set; }
        //public IEnumerable<SelectListItem> Apartment { get; set; }     
    }
}