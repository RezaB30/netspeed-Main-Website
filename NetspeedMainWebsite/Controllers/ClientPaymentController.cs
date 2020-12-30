using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models;
using NetspeedMainWebsite.Models.ViewModel;
using NetspeedMainWebsite.MainSiteServiceReference;

namespace NetspeedMainWebsite.Controllers
{
    public class ClientPaymentController : BaseController
    {
        // GET: PaymentBill
        //HashUtilities hash = new HashUtilities();

        [HttpGet]
        public ActionResult PaymentBill()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentBill(PaymentBillViewModel clientBill)
        {
            var message = string.Empty;
           
            if (ModelState.IsValid)
            {
                clientBill.PhoneNumber = clientBill.PhoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                MainSiteServiceClient client = new MainSiteServiceClient();
                var randomKey = Guid.NewGuid().ToString();
                var username = "elif";
                var passwordHash = HashUtilities.HashCalculate("123456");
                var genericHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");
                var response = client.GetBills(new  NetspeedServiceSubscriberGetBillsRequest()
                {
                    Culture = "tr-tr",
                    Rand = randomKey,
                    Hash = genericHash,
                    Username = username,
                    GetBillParameters = new SubscriberGetBillsRequest()
                    {
                        SubscriberNo = clientBill.ClientInfo,
                        PhoneNo = clientBill.PhoneNumber
                    }
                });
                var result = response;
              
            }
            else
            {
                return View(clientBill);
            }
            return RedirectToAction("PaymentBillAndResult", "ClientPayment");
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
        public ActionResult PaymentBillAndResult(BillInfoViewModel[] selectedBills)
        {
            var ClientBillList = new List<BillInfoViewModel>();
            int cb = 0;
            var message = string.Empty;

            for (int l = 0; l < selectedBills.Length; l++)
            {
                if (selectedBills[l].BillCheck == true)
                {
                    cb++;
                }
            }
            if (cb >= 1)
            {
                for (int i = 0; i < selectedBills.Length; i++)
                {
                    if (ModelState.IsValid)
                    {
                        if (selectedBills[i].BillCheck == true)
                        {
                            ClientBillList.Add(new BillInfoViewModel()
                            {
                                BillId = selectedBills[i].BillId,
                                TariffName = selectedBills[i].TariffName,
                                BillAmount = selectedBills[i].BillAmount,
                                BillDate = selectedBills[i].BillDate,
                                ExpiryDate = selectedBills[i].ExpiryDate
                            });
                        }
                    }
                }
                return View(ClientBillList);
            }
            else
            {
                message = "Lütfen Ödenecek Faturayı/Faturaları Seçiniz.";
            }
            ViewBag.message = message;

            return View(selectedBills);
        }
    }
}