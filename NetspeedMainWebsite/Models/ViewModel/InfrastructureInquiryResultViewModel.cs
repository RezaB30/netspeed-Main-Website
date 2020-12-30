using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class InfrastructureInquiryResultViewModel
    {
        public string BBK { get; set; }
        public string  Distance { get; set; }
        public string MaxSpeed { get; set; }
        public string XDSLType { get; set; }
        public string PortState { get; set; }
        public string SVUID { get; set; }
        public string Message { get; set; }

    }
}