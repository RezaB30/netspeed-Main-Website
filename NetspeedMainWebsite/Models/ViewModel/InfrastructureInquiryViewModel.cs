using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class InfrastructureInquiryViewModel
    {
        public string BBK { get; set; }
        [Required(ErrorMessage = "*Şehir Alanı Zorunlu Alandır.")]
        public long? ProvinceId { get; set; }
        [Required(ErrorMessage = "*İlçe Alanı Zorunlu Alandır.")]
        public long? DistrictId { get; set; }
        [Required(ErrorMessage = "*Semt Alanı Zorunlu Alandır.")]
        public long? RegionId { get; set; }
        [Required(ErrorMessage = "*Mahalle Alanı Zorunlu Alandır.")]
        public long? NeighborhoodId { get; set; }
        [Required(ErrorMessage = "*Sokak/Cadde Alanı Zorunlu Alandır.")]
        public long? StreetId { get; set; }
        [Required(ErrorMessage = "*Apt./Bina No Alanı Zorunlu Alandır.")]
        public long? BuildingId { get; set; }
        [Required(ErrorMessage = "*Kapı No Alanı Zorunlu Alandır.")]
        public long? ApartmentId { get; set; }

    }
}