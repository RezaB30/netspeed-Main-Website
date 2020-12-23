using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class PaymentBillViewModel
    {
        [Required(ErrorMessage = "TC - Client Number is Required.")]
        [MinLength(10),MaxLength(11) ]
        [RegularExpression(@"^([1-9]{1}[0-9]{9}[02468]{1} | [1-9]{1}[0-9]{9} )$", ErrorMessage = "Invalid Information")]//or kullan
        public string ClientInfo { get; set; }

        [Required(ErrorMessage = "PhoneNumber is Required.")]
        [MaxLength(10)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
    }
}