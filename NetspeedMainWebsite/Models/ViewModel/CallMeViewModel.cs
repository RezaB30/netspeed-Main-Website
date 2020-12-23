using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class CallMeViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50), MinLength(5)]
        [RegularExpression(@"^[\p{L}]{2,}(\s[\p{L}]{2,})+$", ErrorMessage = "Invalid Name")]
        public string FullName { get; set; }
        
        [MaxLength(10)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Number")]
        [Required(ErrorMessage = "Phone Number is required.")]
        public string PhoneNumber { get; set; }
    }
}