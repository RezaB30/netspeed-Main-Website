using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetspeedMainWebsite.Models.ViewModel
{
    public class TariffsViewModel
    {
        public int TariffID { get; set; }
        public int DomainID { get; set; }
        public string DisplayName { get; set; }
        public string Speed { get; set; }
        public decimal Price { get; set; }
    }
    public class InfrastructureTariffViewModel
    {        
        public string MaxSpeed { get; set; }
        public string Distance { get; set; }
        public string XDSLType { get; set; }
        public string PortState { get; set; }
        public string SVUID { get; set; }
        public TariffsViewModel[] TariffList { get; set; }
        //public IEnumerable<TariffsViewModel> TariffList { get; set; }
    }
}