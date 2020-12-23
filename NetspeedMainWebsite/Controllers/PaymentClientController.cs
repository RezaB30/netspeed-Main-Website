using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models;
using NetspeedMainWebsite.Models.ViewModel;

namespace NetspeedMainWebsite.Controllers
{
    public class PaymentClientController : Controller
    {
        // GET: PaymentBill
        [HttpGet]
        public ActionResult PaymentBill()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentBill(PaymentBillViewModel client)
        {
            var message = string.Empty;
            var ClientInfoList = new List<PaymentBillViewModel>();

            if (ModelState.IsValid)
            {
                ClientInfoList.Add(new PaymentBillViewModel()
                {
                    ClientInfo = client.ClientInfo,
                    PhoneNumber = client.PhoneNumber
                });
            }
            else
            {
                return View(client);
            }
            return RedirectToAction("PaymentBillAndResult", "PaymentClient");

        }

        [HttpGet]
        public ActionResult PaymentBillAndResult()
        {
            DateTime dt = new DateTime(2020, 12, 30);
            DateTime edt = new DateTime(2021, 12, 30);

            var results = new BillInfoViewModel[]
            {
                new BillInfoViewModel()
                {
                    BillId = 2,
                    TariffName = "Fiber Ham Çökelek",
                    BillDate = dt,
                    ExpiryDate = edt,
                    BillAmount = 55
                },
                new BillInfoViewModel()
                {
                    BillId = 4,
                    TariffName = "Fiber Ham Çökelek2",
                    BillDate = dt,
                    ExpiryDate = edt,
                    BillAmount = 80.55m
                }
            };
            return View(results);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentBillAndResult(int Id)
        {

            //PaymentBill.BillId = Id;
            //PaymentBill.TariffName = "Fiber Ham Çökelek";
            //DateTime dt = new DateTime(2020, 12, 30);
            //DateTime edt = new DateTime(2021, 12, 30);
            //PaymentBill.BillDate = dt;
            //PaymentBill.ExpiryDate = edt;
            //PaymentBill.BillAmount = 55;


            //if (PaymentBill == null)
            //{
            //    return RedirectToAction("", "");
            //}
            //var viewResults = BillInfoList.Select(a => new BillInfoViewModel()
            //{
            //    BillId = Id,
            //    TariffName = a.TariffName,
            //    BillDate = a.BillDate,
            //    ExpiryDate = a.ExpiryDate,
            //    BillAmount = a.BillAmount
            //});


            return RedirectToAction("PaymentBillWithCard", "Payment");
        }



    }
}