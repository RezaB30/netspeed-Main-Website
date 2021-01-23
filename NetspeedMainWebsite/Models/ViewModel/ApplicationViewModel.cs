using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class ApplicationViewModel
    {
        //public ApplicationViewModel()
        //{
        //    //ProvinceList = new List<SelectListItem>();
            //DistrictList = new List<SelectListItem>();
            //RegionList = new List<SelectListItem>();
            //NeighborhoodList = new List<SelectListItem>();
            //StreetList = new List<SelectListItem>();
            //BuildingList = new List<SelectListItem>();
            //ApartmentList = new List<SelectListItem>();
            //BirthDayList = new List<SelectListItem>();
            //BirthMonthList = new List<SelectListItem>();
            //BirthYearList = new List<SelectListItem>();
            //SexList = new List<SelectListItem>();
            //NationalityList = new List<SelectListItem>();
            //IDCardTypeList = new List<SelectListItem>();
        //}

     
        [Required(ErrorMessage = "*İl Alanı Gerekli")]
        public long? ProvinceId { get; set; }
        [Required(ErrorMessage = "*İlçe Alanı Gerekli")]
        public long? DistrictId { get; set; }
        [Required(ErrorMessage = "*Semt Alanı Gerekli")]
        public long? RegionId { get; set; }
        [Required(ErrorMessage = "*Mahalle Alanı Gerekli")]
        public long? NeighborhoodId { get; set; }
        [Required(ErrorMessage = "*Cadde/Sokak Alanı Gerekli")]
        public long? StreetId { get; set; }
        [Required(ErrorMessage = "*Apartman Alanı Gerekli")]
        public long? BuildingId { get; set; }

        [Required(ErrorMessage = "*Kapı Numarası Alanı Gerekli")]
        public long? ApartmentId { get; set; }
        public string AddressText { get; set; }

        //public IEnumerable<SelectListItem> ProvinceList { get; set; }
        //public IEnumerable<SelectListItem> DistrictList { get; set; }
        //public IEnumerable<SelectListItem> RegionList { get; set; }
        //public IEnumerable<SelectListItem> NeighborhoodList { get; set; }
        //public IEnumerable<SelectListItem> StreetList { get; set; }
        //public IEnumerable<SelectListItem> BuildingList { get; set; }
        //public IEnumerable<SelectListItem> ApartmentList { get; set; }
        //public IEnumerable<SelectListItem> BirthDayList { get; set; }
        //public IEnumerable<SelectListItem> BirthMonthList { get; set; }
        //public IEnumerable<SelectListItem> BirthYearList { get; set; }
        //public IEnumerable<SelectListItem> SexList { get; set; }
        //public IEnumerable<SelectListItem> NationalityList { get; set; }
        //public IEnumerable<SelectListItem> IDCardTypeList { get; set; }


        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "*Lütfen Telefon Numaranızı Doğru Giriniz.")]
        [Required(ErrorMessage = "*Telefon Numarası Alanı Gerekli ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "*Ad Alanı Gerekli ")]

        private string _FirstName;
        [Required(ErrorMessage = "*Ad Alanı Gerekli ")]
        public string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(_FirstName))
                {
                    return _FirstName;
                }
                return _FirstName.ToUpper(CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            set
            {
                _FirstName = value;
            }
        }

        [Required(ErrorMessage = "*Soyad Alanı Gerekli ")]
        private string _LastName;
        [Required(ErrorMessage = "*Soyad Alanı Gerekli ")]
        public string LastName
        {
            get
            {
                if (string.IsNullOrEmpty(_LastName))
                {
                    return _LastName;
                }
                return _LastName.ToUpper(CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            set
            {
                _LastName = value;
            }
        }

        [Required(ErrorMessage = "*E-Posta Alanı Gerekli ")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "*Uyruk Alanı Gerekli ")]
        [Range(1, 255, ErrorMessage = "*Lütfen Geçerli Bir Değer Giriniz.")]
        public int? Nationality { get; set; }

        [Required(ErrorMessage = "*Kart Tipi Alanı Gerekli ")]
        [Range(1, 22, ErrorMessage = "*Lütfen Geçerli Bir Değer Giriniz.")]
        public int? IDCardType { get; set; }

        [Required(ErrorMessage = "*Posta Kodu Alanı Gerekli ")]
        //public int PostalCode { get; set; }

        public string PostalCode
        {
            get
            {
                return _PostalCode.HasValue ? _PostalCode.Value.ToString() : null;
            }
            set
            {
                int PostalCode;
                if (int.TryParse(value, out PostalCode))
                {
                    _PostalCode = PostalCode;
                }
            }
        }
        public int? _PostalCode { get; set; }

        [Required(ErrorMessage = "*T.C Kimlik Numarası Alanı Gerekli ")]
        [RegularExpression(@"^([1-9]{1}[0-9]{9}[02468]{1})$", ErrorMessage = "*Lütfen T.C Kimlik Numaranızı Doğru Giriniz.")]
        [MaxLength(11), MinLength(11)]
        public string TC { get; set; }

        [Required(ErrorMessage = "*Doğum Günü Alanı Gerekli ")]
        public DateTime? BirthDate { get; set; }


        [Required(ErrorMessage = "*Cinsiyet Alanı Gerekli ")]
        [Range(1, 2, ErrorMessage = "*Lütfen Geçerli Bir Değer Giriniz.")]
        public int? Sex { get; set; }

        [Required(ErrorMessage = "*Baba Adı Alanı Gerekli ")]
        private string _FatherName;
        [Required(ErrorMessage = "*Baba Adı Alanı Gerekli ")]
        public string FatherName
        {
            get
            {
                if (string.IsNullOrEmpty(_FatherName))
                {
                    return _FatherName;
                }
                return _FatherName.ToUpper(CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            set
            {
                _FatherName = value;
            }
        }

        [Required(ErrorMessage = "*Anne Adı Alanı Gerekli ")]
        private string _MotherName;
        [Required(ErrorMessage = "*Anne Adı Alanı Gerekli ")]
        public string MotherName
        {
            get
            {
                if (string.IsNullOrEmpty(_MotherName))
                {
                    return _MotherName;
                }
                return _MotherName.ToUpper(CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            set
            {
                _MotherName = value;
            }
        }

        [Required(ErrorMessage = "*Doğum Yeri Alanı Gerekli ")]
        private string _BirthPlace;
        [Required(ErrorMessage = "*Doğum Yeri Alanı Gerekli ")]
        public string BirthPlace
        {
            get
            {
                if (string.IsNullOrEmpty(_BirthPlace))
                {
                    return _BirthPlace;
                }
                return _BirthPlace.ToUpper(CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            set
            {
                _BirthPlace = value;
            }
        }

        [Required(ErrorMessage = "*Anne Kızlık Soyadı Alanı Gerekli ")]
        private string _MotherFirstSurname;
        [Required(ErrorMessage = "*Anne Kızlık Soyadı Alanı Gerekli ")]

        public string MotherFirstSurname
        {
            get
            {
                if (string.IsNullOrEmpty(_MotherFirstSurname))
                {
                    return _MotherFirstSurname;
                }
                return _MotherFirstSurname.ToUpper(CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            set
            {
                _MotherFirstSurname = value;
            }
        }

        [Required(ErrorMessage = "*Seri No Alanı Gerekli")]
        //public string SerialNo { get; set; }
        private string _SerialNo;
        [Required(ErrorMessage = "*Seri No Alanı Gerekli")]
        public string SerialNo
        {
            get
            {
                if (string.IsNullOrEmpty(_SerialNo))
                {
                    return _SerialNo;
                }
                return _SerialNo.ToUpper(CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            set
            {
                _SerialNo = value;
            }
        }

        [Required(ErrorMessage = "*Kat No Alanı Gerekli ")]
        public string Floor { get; set; }

        private string _ReferenceCode;
        public string ReferenceCode
        {
            get
            {
                if (string.IsNullOrEmpty(_ReferenceCode))
                {
                    return _ReferenceCode;
                }
                return _ReferenceCode.ToUpper(CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            set
            {
                _ReferenceCode = value;
            }
        }


        [Required(ErrorMessage = "*Verildiği Yer Alanı Gerekli ")]
        private string _PlaceOfIssue;
        [Required(ErrorMessage = "*Verildiği Yer Alanı Gerekli ")]
        public string PlaceOfIssue
        {
            get
            {
                if (string.IsNullOrEmpty(_PlaceOfIssue))
                {
                    return _PlaceOfIssue;
                }
                return _PlaceOfIssue.ToUpper(CultureInfo.CreateSpecificCulture("tr-TR"));
            }
            set
            {
                _PlaceOfIssue = value;
            }
        }
        [Required(ErrorMessage = "*Verildiği Tarih Alanı Gerekli ")]
        public DateTime? DateOfIssue { get; set; }
     
        //[Required(ErrorMessage = "*SMS Doğrulama Kodu Alanı Gerekli ")]
        public string SMSCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        [Required(ErrorMessage = "*Tarife Seçimi Gerekli ")]
        public int? TariffId { get; set; }
        //[Required(ErrorMessage = "*Abonelik Sözleşmesini Okudum Alanı Zorunludur. ")]
        //public int SubscriptionAgreement { get; set; }
        //[Required(ErrorMessage = "*Abonelik Sözleşmesini Okudum Alanı Zorunludur. ")]
        //[Range(typeof(bool), "true", "true", ErrorMessage = "*Abonelik Sözleşmesini Okudum Alanı Zorunludur.")]
        //[Required(ErrorMessage = "*Abonelik Sözleşmesini Okudum Alanı Zorunludur. ")]
        //[Range(typeof(bool), "true", "true", ErrorMessage = "*Abonelik Sözleşmesini Okudum Alanı Zorunludur.")]
    
        public string DisplayName { get; set; }
    }
}
