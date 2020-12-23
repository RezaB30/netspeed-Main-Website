using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class BillInfo
    {
        
        public int BillId { get; set; }
        public int TariffName { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime ExpiryDate  { get; set; }
        public int BillAmount { get; set; }
    }
}