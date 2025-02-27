﻿using NLog;
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
        //Logger applicationLogger = LogManager.GetLogger("applications");
        public string RowNo { get; set; }
        public string VolumeNo { get; set; }
        public string PageNo { get; set; }
        public string IDCardProvince { get; set; }
        public string IDCardDistrict { get; set; }
        public string IDCardNeighbourhood { get; set; }

             


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

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "*Lütfen Telefon Numaranızı Doğru Giriniz.")]
        [Required(ErrorMessage = "*Telefon Numarası Alanı Gerekli ")]
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "*Ad Alanı Gerekli ")]

        private string _FirstName;

        [MaxLength(50)]
        [Required(ErrorMessage = "*Ad Alanı Gerekli ")]
        public string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(_FirstName))
                {
                    return _FirstName;
                }
                return _FirstName.ToUpper();
            }
            set
            {
                _FirstName = value;
            }
        }
        [MaxLength(50)]
        [Required(ErrorMessage = "*Soyad Alanı Gerekli ")]
        private string _LastName;
        [MaxLength(50)]
        [Required(ErrorMessage = "*Soyad Alanı Gerekli ")]
        public string LastName
        {
            get
            {
                if (string.IsNullOrEmpty(_LastName))
                {
                    return _LastName;
                }
                return _LastName.ToUpper();
            }
            set
            {
                _LastName = value;
            }
        }


        [Required(ErrorMessage = "*E-Posta Alanı Gerekli ")]
        [StringLength(100, ErrorMessage = "En Fazla 100 Karakter Girebilirsiniz.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "*Lütfen Telefon E-Posta Adresinizi Doğru Giriniz.")]
        public string EmailAddress { get; set; }     
        public int? Nationality { get; set; }

        [Required(ErrorMessage = "*Kart Tipi Alanı Gerekli ")]
        [Range(1, 2, ErrorMessage = "*Lütfen Geçerli Bir Değer Giriniz.")]
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
        public string TC { get; set; }

      
        public DateTime BirthDate
        {
            get
            {
                return Convert.ToDateTime(sBirthDate);
            }
            set
            {
                sBirthDate = DateUtilities.ConvertToWebServiceDate(value);
            }
        }
        public string sBirthDate { get; set; }

                      

        [Required(ErrorMessage = "*Cinsiyet Alanı Gerekli ")]
        [Range(1, 2, ErrorMessage = "*Lütfen Geçerli Bir Değer Giriniz.")]
        public int? Sex { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "*Baba Adı Alanı Gerekli ")]
        private string _FatherName;

        [MaxLength(50)]
        [Required(ErrorMessage = "*Baba Adı Alanı Gerekli ")]
        public string FatherName
        {
            get
            {
                if (string.IsNullOrEmpty(_FatherName))
                {
                    return _FatherName;
                }
                return _FatherName.ToUpper();
            }
            set
            {
                _FatherName = value;
            }
        }

        [MaxLength(50)]
        [Required(ErrorMessage = "*Anne Adı Alanı Gerekli ")]
        private string _MotherName;

        [MaxLength(50)]
        [Required(ErrorMessage = "*Anne Adı Alanı Gerekli ")]
        public string MotherName
        {
            get
            {
                if (string.IsNullOrEmpty(_MotherName))
                {
                    return _MotherName;
                }
                return _MotherName.ToUpper();
            }
            set
            {
                _MotherName = value;
            }
        }

        [MaxLength(50)]
        [Required(ErrorMessage = "*Doğum Yeri Alanı Gerekli ")]
        private string _BirthPlace;

        [MaxLength(50)]
        [Required(ErrorMessage = "*Doğum Yeri Alanı Gerekli ")]
        public string BirthPlace
        {
            get
            {
                if (string.IsNullOrEmpty(_BirthPlace))
                {
                    return _BirthPlace;
                }
                return _BirthPlace.ToUpper();
            }
            set
            {
                _BirthPlace = value;
            }
        }

        [MaxLength(50)]
        [Required(ErrorMessage = "*Anne Kızlık Soyadı Alanı Gerekli ")]
        private string _MotherFirstSurname;

        [MaxLength(50)]
        [Required(ErrorMessage = "*Anne Kızlık Soyadı Alanı Gerekli ")]

        public string MotherFirstSurname
        {
            get
            {
                if (string.IsNullOrEmpty(_MotherFirstSurname))
                {
                    return _MotherFirstSurname;
                }
                return _MotherFirstSurname.ToUpper();
            }
            set
            {
                _MotherFirstSurname = value;
            }
        }

        [MaxLength(9), MinLength(9)]

        [Required(ErrorMessage = "*Seri No Alanı Gerekli")]
        private string _SerialNo;

        [MaxLength(10)]
        [Required(ErrorMessage = "*Seri No Alanı Gerekli")]
        public string SerialNo
        {
            get
            {
                if (string.IsNullOrEmpty(_SerialNo))
                {
                    return _SerialNo;
                }
                return _SerialNo.ToUpper();
            }
            set
            {
                _SerialNo = value;
            }
        }


        [Required(ErrorMessage = "*Kat No Alanı Gerekli ")]
        public string Floor { get; set; }

        [MaxLength(10)]
        private string _ReferenceCode;
        public string ReferenceCode
        {
            get
            {
                if (string.IsNullOrEmpty(_ReferenceCode))
                {
                    return _ReferenceCode;
                }
                return _ReferenceCode.ToUpper();
            }
            set
            {
                _ReferenceCode = value;
            }
        }

        [MaxLength(50)]
        [Required(ErrorMessage = "*Verildiği Yer Alanı Gerekli ")]
        private string _PlaceOfIssue;

        [MaxLength(50)]
        [Required(ErrorMessage = "*Verildiği Yer Alanı Gerekli ")]
        public string PlaceOfIssue
        {
            get
            {
                if (string.IsNullOrEmpty(_PlaceOfIssue))
                {
                    return _PlaceOfIssue;
                }
                return _PlaceOfIssue.ToUpper();
            }
            set
            {
                _PlaceOfIssue = value;
            }
        }


        public DateTime DateOfIssue
        {
            get
            {
                return Convert.ToDateTime(sDateOfIssue);
            }
            set
            {
                sDateOfIssue = DateUtilities.ConvertToWebServiceDate(value);
            }
        }

        public string sDateOfIssue { get; set; }


        

        [MaxLength(6), MinLength(6)]
        public string SMSCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        [Required(ErrorMessage = "*Tarife Seçimi Gerekli ")]
        public int? TariffId { get; set; }
        public string DisplayName { get; set; }
    }
}
