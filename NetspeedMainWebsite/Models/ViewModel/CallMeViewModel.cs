using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class CallMeViewModel
    {
        [Required(ErrorMessage = "*Adınız Soyadınız Alanı Gereklidir.")]
        //[MaxLength(50)]
        //[RegularExpression(@"^[\p{L}]{2,}(\s[\p{L}]{2,})+$", ErrorMessage = "*Lütfen Adınızı Soyadınızı Giriniz.")]
        public string FullName { get; set; }
        public string FullNameValidationMessage { get; set; }
        //[MaxLength(11)]
        [Required(ErrorMessage = "*İletişim Numaranız Alanı Zorunlu Alandır.")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "*Lütfen İletişim Numaranızı Doğru Giriniz.")]
        public string PhoneNumber { get; set; }
        public string PhoneNumberValidationMessage { get; set; }
    }
}