using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class BillInfoViewModel
    {

        //[Range(typeof(bool), "true", "true", ErrorMessage = "Lütfen Ödenecek Faturayı Seçiniz.")]

        //[Required(ErrorMessage ="Lütfen Ödenecek Faturayı Seçiniz.")]
        public bool BillCheck { get; set; }
        public long BillId { get; set; }
        public string ServiceName { get; set; }
        public bool CanBePaid { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime LastPaymentDate  { get; set; }
        public string Total { get; set; }

       
    }
}