using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class ApplicationViewModel
    {

        public ApplicationViewModel()
        {
            ProvinceList = new List<SelectListItem>();
            DistrictList = new List<SelectListItem>();
            RegionList = new List<SelectListItem>();
            NeighborhoodList = new List<SelectListItem>();
            StreetList = new List<SelectListItem>();
            BuildingList = new List<SelectListItem>();
            ApartmentList = new List<SelectListItem>();
            //DayList = new List<SelectListItem>();
            //MonthList= new List<SelectListItem>();
            //YearList= new List<SelectListItem>();
            BirthDayList = new List<SelectListItem>();
            BirthMonthList = new List<SelectListItem>();
            BirthYearList = new List<SelectListItem>();
            SexList = new List<SelectListItem>();
            NationalityList = new List<SelectListItem>();
            IDCardTypeList = new List<SelectListItem>();

        }
        [Required(ErrorMessage = "Province is Required")]
        public long ProvinceId { get; set; }
        [Required(ErrorMessage = "District is Required")]
        public long DistrictId { get; set; }
        [Required(ErrorMessage = "Region is Required")]
        public long RegionId { get; set; }
        [Required(ErrorMessage = "Neighborhood is Required")]
        public long NeighborhoodId { get; set; }
        [Required(ErrorMessage = "Street is Required")]
        public long StreetId { get; set; }
        [Required(ErrorMessage = "Apartment is Required")]
        public long BuildingId { get; set; }
        [Required(ErrorMessage = "DoorNo is Required")]
        public long ApartmentId { get; set; }

        //public string Province { get; set; }
        //public string District { get; set; }
        //public string Region { get; set; }
        //public string Neighborhood { get; set; }
        //public string Street { get; set; }
        //public string Building { get; set; }
        //public string Apartment { get; set; }
        public string AddressText { get; set; }

        public IEnumerable<SelectListItem> ProvinceList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> RegionList { get; set; }
        public IEnumerable<SelectListItem> NeighborhoodList { get; set; }
        public IEnumerable<SelectListItem> StreetList { get; set; }
        public IEnumerable<SelectListItem> BuildingList { get; set; }
        public IEnumerable<SelectListItem> ApartmentList { get; set; }
        public IEnumerable<SelectListItem> BirthDayList { get; set; }
        public IEnumerable<SelectListItem> BirthMonthList { get; set; }
        public IEnumerable<SelectListItem> BirthYearList { get; set; }
        public IEnumerable<SelectListItem> SexList { get; set; }
        public IEnumerable<SelectListItem> NationalityList { get; set; }
        public IEnumerable<SelectListItem> IDCardTypeList { get; set; }

        //[Required(ErrorMessage = "PhoneNumber is Required")]
        public string PhoneNumber { get; set; }
        //public int SmsCode { get; set; }
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "EmailAddress is Required")]
        public string EmailAddress { get; set; }
        public long Nationality { get; set; }
        public long IDCardType { get; set; }
        

        [Required(ErrorMessage = "PostalCode is Required")]
        public int PostalCode { get; set; }
       
        [Required(ErrorMessage = "TC is Required")]
        public string TC { get; set; }
      
        public DateTime BirthDate { get; set; }

        public int BirthDay { get; set; }

        public int BirthMonth { get; set; }
        public int Sex { get; set; }


        public int BirthYear { get; set; }
        [Required(ErrorMessage = "FatherName is Required")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "MotherName is Required")]
        public string MotherName { get; set; }
      
        [Required(ErrorMessage = "BirthPlace is Required")]
        public string BirthPlace { get; set; }
        [Required(ErrorMessage = "MotherFirstSurname is Required")]
        public string MotherFirstSurname { get; set; }

        [Required(ErrorMessage = "SeriNo is Required")]
        public string SerialNo { get; set; }
     
        [Required(ErrorMessage = "Floor is Required")]
        public string Floor { get; set; }
        public string ReferenceCode { get; set; }
        public string PlaceOfIssue { get; set; }
        public string DateOfIssue { get; set; }
        public string ContactPhoneNo { get; set; }
        public string SMSCode { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
}
