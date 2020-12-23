using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Name is Required.")]
        [MaxLength(50), MinLength(5)]
        [RegularExpression(@"^[\p{L}]{2,}(\s[\p{L}]{2,})+$", ErrorMessage = "Invalid Name")]
        public string FullName { get; set; }
        [MaxLength(10)]
        [Required(ErrorMessage = "Phone Number is Required.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Name is Email Address.")]
        [MaxLength(50)]
        [RegularExpression(@"^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }
    }
}