using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class PaymentBillViewModel
    {
        [Required(ErrorMessage = "*TC Kimlik Numarası veya Müşteri Numarası Zorunlu Alandır.")]
      
        [RegularExpression(@"^([1-9]{1}[0-9]{9}[02468]{1})|([1-9]{1}[0-9]{9})$", ErrorMessage = "*Lütfen Numaranızı Doğru Giriniz.")]//or kullan
        //[RegularExpression(@"^([1-9]{1}[0-9]{9}[02468]{1}$) | (^[1-9]{1}[0-9]{9}$)", ErrorMessage = "*Lütfen Numaranızı Doğru Giriniz.")]//or kullan
        public string ClientInfo { get; set; }

       
        [Required(ErrorMessage = "*GSM Numarası Alanı Zorunlu Alandır.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "*Lütfen İletişim Numaranızı Doğru Giriniz.")]
        public string PhoneNumber { get; set; }
    }
}