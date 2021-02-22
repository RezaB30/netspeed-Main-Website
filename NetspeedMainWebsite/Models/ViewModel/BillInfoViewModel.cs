using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class BillInfoViewModel
    {
        public bool BillCheck { get; set; }
        public long BillId { get; set; }
        public string ServiceName { get; set; }
        public bool CanBePaid { get; set; }
        [UIHint("MontAndYear")]
        public DateTime BillDate { get; set; }
        public DateTime LastPaymentDate  { get; set; }
        public string Total { get; set; }

       
    }
}