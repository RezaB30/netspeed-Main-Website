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
        }
        public long ProvinceId { get; set; }
        public long DistrictId { get; set; }
        public long RegionId { get; set; }
        public long NeighborhoodId { get; set; }
        public long StreetId { get; set; }
        public long BuildingId { get; set; }
        public long ApartmentId { get; set; }

        public string Province { get; set; }
        public string District { get; set; }
        public string Region{ get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Apartment { get; set; }
        public IEnumerable<SelectListItem> ProvinceList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> RegionList { get; set; }
        public IEnumerable<SelectListItem> NeighborhoodList { get; set; }
        public IEnumerable<SelectListItem> StreetList { get; set; }
        public IEnumerable<SelectListItem> BuildingList { get; set; }
        public IEnumerable<SelectListItem> ApartmentList { get; set; }



        public string PhoneNumber { get; set; }
        public int SmsCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ReferenceType { get; set; }
        public string HasFixedPhone { get; set; }
        public long DoorId { get; set; }
        public string FloorNumber { get; set; }
        public int PostalCode { get; set; }
        public string TariffPreference { get; set; }
        public string Modem { get; set; }
        public string StaticIp { get; set; }
        public string TC { get; set; }
        public DateTime BirthDay { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int Sex { get; set; }
        public string BirthPlace { get; set; }
        public string MotherFirstSurname { get; set; }
        public bool SubscriptionAgreement { get; set; }
        public bool PersonalDataAllow { get; set; }
        public bool Attestation { get; set; }
        public string ReferenceCode { get; set; }

        public long AddressNo { get; set; }
        public string AddressText { get; set; }
        public string Floor { get; set; }
        public long RuralCode { get; set; }

        [Required(ErrorMessage = "TaxNo is Required")]
        public string TaxNo { get; set; }
        [Required(ErrorMessage = "TaxOffice is Required")]
        public string TaxOffice { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "TradeRegistrationNo is Required")]
        public string TradeRegistrationNo { get; set; }
        public string ContactPhoneNo { get; set; }

        [Required(ErrorMessage = "Culture is Required")]
        public string Culture { get; set; }
        public int CustomerType { get; set; }
        public string Number { get; set; }
        public int Nationality { get; set; }
        public int Profession { get; set; }

        public int CardType { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string PageNo { get; set; }
        public string PassportNo { get; set; }
        public string PlaceOfIssue { get; set; }
        public string RowNo { get; set; }
        public string SerialNo { get; set; }
        public string VolumeNo { get; set; }

        public string ServiceName { get; set; }
        public string OperatorName { get; set; }

        public string ServiceNumber { get; set; }

        public string FixedPhoneNumber { get; set; }
        public int ApplicationType { get; set; }
        [Required(ErrorMessage = "ServiceId is Required")]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "DomainId is Required")]
        public int DomainId { get; set; }
        [Required(ErrorMessage = "BillingPeriod is Required")]
        public int BillingPeriod { get; set; }
        [Required(ErrorMessage = "CentralSystemNo is Required")]
        public string CentralSystemNo { get; set; }
       
      
    }
}
   