using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class PaymentCardInfo
    {
        [Required]
        [MaxLength(50), MinLength(5)]
        [RegularExpression(@"^[\p{L}]{2,}(\s[\p{L}]{2,})+$", ErrorMessage = "Invalid Name")]
        public string FullName { get; set; }

        [Required]
        [MinLength(16)]
        [RegularExpression(@"^[\p{L}]{2,}(\s[\p{L}]{2,})+$", ErrorMessage = "Invalid Name")]
        public string CardNumber { get; set; }

        [Required]
        [MinLength(16)]
        [RegularExpression(@"^[\p{L}]{2,}(\s[\p{L}]{2,})+$", ErrorMessage = "Invalid Mont")]
        public int Month { get; set; }

        [Required]
        [MinLength(16)]
        [RegularExpression(@"^[\p{L}]{2,}(\s[\p{L}]{2,})+$", ErrorMessage = "Invalid Year")]
        public int Year { get; set; }

        [Required]
        [MinLength(3)]
        [RegularExpression(@"^[1-9]{1}[0-9]{2}$", ErrorMessage = "Invalid Security Code")]
        public string SecurityCode { get; set; }
    }
}