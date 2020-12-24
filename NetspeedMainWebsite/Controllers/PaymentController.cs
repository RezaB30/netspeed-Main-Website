using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetspeedMainWebsite.Controllers
{
    public class PaymentController : BaseController
    {
        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult PaymentBillWithCard()
        {
            return View();
        }
        
        public ActionResult PaymentBillAndResult()
        {
            return View();
        }


        public ActionResult PaymentOk()
        {
            return View();
        }
    }
}