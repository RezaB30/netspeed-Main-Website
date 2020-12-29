using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class ApplicationViewModel
    {
        public string PhoneNumber { get; set; }
        public int SmsCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string ReferenceType { get; set; }
        public string HasFixedPhone { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Locality { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string DoorNumber { get; set; }
        public string FloorNumber { get; set; }
        public long Postcode { get; set; }
        public string TariffPreference { get; set; }
        public string Modem { get; set; }
        public string StaticIp { get; set; }
        public string TC { get; set; }
        public DateTime BirthDay { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Sex { get; set; }
        public string BirthPlace { get; set; }
        public string MotherFirstSurname { get; set; }
        public bool SubscriptionAgreement { get; set; }
        public bool PersonalDataAllow { get; set; }
        public bool Attestation { get; set; }
        


    }
}