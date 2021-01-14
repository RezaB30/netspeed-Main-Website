using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class VerificationViewModel
    {
        [Required]
        public string Code { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}