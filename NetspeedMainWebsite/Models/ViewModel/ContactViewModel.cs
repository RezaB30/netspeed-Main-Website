using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class ContactViewModel
    {
        //[Required(ErrorMessage = "*Adınız Soyadınız Alanı Zorunlu Alandır.")]
        ////[MaxLength(50)]
        //[RegularExpression(@"^[\p{L}]{2,}(\s[\p{L}]{2,})+$", ErrorMessage = "*Lütfen Adınızı ve Soyadınızı Giriniz.")]
        public string FullName { get; set; }
        ////[MaxLength(11)]
        //[Required(ErrorMessage = "*İletişim Numaranız Alanı Zorunlu Alandır.")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "*Lütfen İletişim Numaranızı Doğru Giriniz.")]
        public string PhoneNumber { get; set; }

        //[Required(ErrorMessage = "*E-Posta Adresiniz Alanı Zorunlu Alandır.")]
        ////[MaxLength(50)]
        //[RegularExpression(@"^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "*Lütfen E-Posta Adresinizi Doğru Giriniz.")]
        public string EmailAddress { get; set; }

        //[Required(ErrorMessage = "*Mesajınız Alanı Zorunlu Alandır.")]
        //[MaxLength(1000)]
        public string Message { get; set; }
    }
}