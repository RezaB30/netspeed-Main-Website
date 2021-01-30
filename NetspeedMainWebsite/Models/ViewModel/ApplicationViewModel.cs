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
        public string BirthDate
        {
            get
            {
                return _BirthDate.HasValue ? _BirthDate.Value.ToString() : null;
            }
            set
            {
                DateTime BirthDate;
                if (DateTime.TryParse(value, out BirthDate))
                {
                    _BirthDate = BirthDate;
                }
            }
        }
        [Required(ErrorMessage = "*Doğum Günü Alanı Gerekli ")]
        public DateTime? _BirthDate { get; set; }



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
        //public DateTime? DateOfIssue { get; set; }
        public string DateOfIssue
        {
            get
            {
                return _DateOfIssue.HasValue ? _DateOfIssue.Value.ToString() : null;
            }
            set
            {
                DateTime DateOfIssue;
                if (DateTime.TryParse(value, out DateOfIssue))
                {
                    _DateOfIssue = DateOfIssue;
                }
            }
        }
        [Required(ErrorMessage = "*Verildiği Tarih Alanı Gerekli ")]
        public DateTime? _DateOfIssue { get; set; }

        //[Required(ErrorMessage = "*SMS Doğrulama Kodu Alanı Gerekli ")]
        public string SMSCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        [Required(ErrorMessage = "*Tarife Seçimi Gerekli ")]
        public int? TariffId { get; set; }
        public string DisplayName { get; set; }
    }
}
