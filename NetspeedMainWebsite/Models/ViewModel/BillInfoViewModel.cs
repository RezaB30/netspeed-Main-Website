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
        
        public int BillId { get; set; }
        public string TariffName { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime ExpiryDate  { get; set; }
        public decimal BillAmount { get; set; }
    }
}