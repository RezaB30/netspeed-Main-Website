using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models;
using NetspeedMainWebsite.Models.ViewModel;

namespace NetspeedMainWebsite.Controllers
{
    public class PaymentBillController : Controller
    {
        // GET: PaymentBill
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FindClientBill(PaymentBillViewModel client)
        {
            var message = string.Empty;
            var ClientInfoList = new List<PaymentBillViewModel>();

            if (ModelState.IsValid)
            {
                ClientInfoList.Add(new PaymentBillViewModel()
                {
                   ClientInfo=client.ClientInfo,
                   PhoneNumber=client.PhoneNumber
                });
                message = " is successful";
            }
            else
            {
                message = " isn't successful";
            }
            TempData["Message"] = message;
            return RedirectToAction("", "Payment");
        }
    }
}