using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models;
using NetspeedMainWebsite.Models.ViewModel;
using NetspeedMainWebsite.MainSiteServiceReference;
using System.Runtime.Caching;

namespace NetspeedMainWebsite.Controllers
{
    public class ClientPaymentController : BaseController
    {
        [HttpGet]
        public ActionResult BillPaymentLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BillPaymentLogin(PaymentBillViewModel payment, string PhoneNumber, string ClientInfo)
        {
            var ClientInfoList = new List<PaymentBillViewModel>();

            if (ModelState.IsValid)
            {
                ClientInfoList.Add(new PaymentBillViewModel()
                {
                    ClientInfo = payment.ClientInfo,
                    PhoneNumber = payment.PhoneNumber
                });
                return RedirectToAction("PaymentBillAndResult", "ClientPayment");
            }
            return View();
        }


        [HttpGet]
        public ActionResult PaymentBillWithCard()
        {
            return View();
        }
        public ActionResult PaymentBillAndResult()
        {
            var ClientBillList = (List<BillInfoViewModel>)Session["ClientBillList"];
            return View(ClientBillList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentBillAndResult(string PhoneNumber, string ClientInfo)
        {
            var message = string.Empty;
            if (ModelState.IsValid)
            {
                WebServiceWrapper clientsBills = new WebServiceWrapper();
                var response = clientsBills.GetBills(PhoneNumber, ClientInfo);

                //var url = NetspeedMainWebsite.Properties.Settings.Default.oimUrl;


                if (response.ResponseMessage.ErrorCode == 2)
                {                    
                    TempData["message"] = "Kayıtlı Abone Bulunamadı.";
                    return RedirectToAction("BillPaymentLogin", "ClientPayment");
                }

                if (response.ResponseMessage.ErrorCode == 4)
                {
                    TempData["message"] = "Fatura Bulunamadı.";
                    return RedirectToAction("BillPaymentLogin", "ClientPayment");
                }

                if (response.ResponseMessage.ErrorCode == 5)
                {
                    return RedirectToAction("AlreadyHaveCustomer", "Application");
                }

                BillInfoViewModel Bills = new BillInfoViewModel();

                var ClientBillItems = response.SubscriberGetBillsResponse.Select(r => new BillInfoViewModel()
                {
                    ServiceName = r.ServiceName,
                    CanBePaid = r.CanBePaid,
                    BillDate = r.BillDate,
                    BillId = r.ID,
                    Total = r.Total,
                    LastPaymentDate = r.LastPaymentDate
                });
                var ClientBillList = ClientBillItems.ToList();

                //Session.Add("BillCheckList", ClientBillList);//kullanma hata çıkyor içinde varsa
                Session["ClientBillList"] = ClientBillList;
                return View(ClientBillList);
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentSelectBill(object SelectedBills)
        {
            var message = string.Empty;

            var CurrentSelectedBills = ((string[])SelectedBills)[0].ToString().Split(',');
            var GetSelectedBills = new List<long>();//selected bills


            foreach (var item in CurrentSelectedBills)
            {
                GetSelectedBills.Add(Convert.ToInt64(item));
            }
            var BillList = (List<BillInfoViewModel>)Session["ClientBillList"];//all bills


            var selectBills = BillList.Where(bill => GetSelectedBills.Contains(bill.BillId)).ToArray();

            var HasBillList = new List<long>();//correct bills for BillId

            for (int c = 0; c < GetSelectedBills.Count(); c++)
            {
                var HasBill = BillList.Select(i => i.BillId).Contains(GetSelectedBills[c]);
                if (HasBill == true)
                {
                    HasBillList.Add(GetSelectedBills[c]);
                }
            }

            DateTime temp;

            for (int i = 0; i < BillList.Count() - 1; i++)//order datetime all billlist
            {
                for (int j = i; j < BillList.Count(); j++)
                {
                    {
                        temp = BillList[j].BillDate;
                        BillList[j].BillDate = BillList[i].BillDate;
                        BillList[i].BillDate = temp;
                    }
                }
            }

            var BillIds = new List<long>();

            var PayableBillIdList = new List<long>();//payable bill for canbepaid and date

            for (int hasCanBePaid = 0; hasCanBePaid < BillList.Count(); hasCanBePaid++)
            {
                if (BillList[hasCanBePaid].CanBePaid == true && HasBillList.Contains(BillList[hasCanBePaid].BillId))
                {
                    PayableBillIdList.Add(BillList[hasCanBePaid].BillId);

                    for (int payable = 0; payable < HasBillList.Count(); payable++)
                    {
                        if (HasBillList.Contains(BillList[payable].BillId))
                        {
                            if (!PayableBillIdList.Contains(BillList[payable].BillId))
                            {
                                PayableBillIdList.Add(BillList[payable].BillId);
                            }
                        }
                    }
                }
            }
            if (PayableBillIdList.Count == 0)
            {
                TempData["message"] = "Eski Tarihli Faturalarınızı Ödemeden Diğer Faturalarınızı Ödeyemezsiniz. Lütfen Eski Tarihli Faturalarınızı Seçin.";
                return RedirectToAction("PaymentBillAndResult", "ClientPayment");
            }
            else
            {
                Session["BillIds"] = PayableBillIdList.ToArray();
                return RedirectToAction("PaymentVPOS", "ClientPayment");
            }
        }

        public ActionResult PaymentVPOS(/*long[] BillIds*/)
        {
            var BillList = (long[])Session["BillIds"];
            //long[] BillListt = new long[10];

            var Key = Guid.NewGuid().ToString();
            var Value = BillList;

            if (BillList.Count() == 0)
            {
                Url.Action("PaymentFail", "ClientPayment", new { id = Key });
            }

            MemoryCache.Default.Add(Key, Value, DateTimeOffset.Now.AddMinutes(15));

            //cacheToken.Remove(Key);
            WebServiceWrapper clientVPOS = new WebServiceWrapper();
            var response = clientVPOS.SubscriberPaymentVPOS(BillList, Url.Action("PaymentFail", "ClientPayment", new { id = Key }, Request.Url.Scheme),
                Url.Action("PaymentConfirm", "ClientPayment", new { id = Key }, Request.Url.Scheme));

            ViewBag.VPOSForm = response.PaymentVPOSResponse.HtmlForm;
            return View();
        }

        public ActionResult PaymentConfirm(string id)
        {
            var billIds = MemoryCache.Default.Get(id) as long[];

            WebServiceWrapper clientsPayBills = new WebServiceWrapper();
            var response = clientsPayBills.PayBills(billIds);

            MemoryCache.Default.Remove(id);
            return View();
        }

        public ActionResult PaymentFail(string id)
        {
            MemoryCache.Default.Remove(id);
            return View();
        }

        [HttpPost]
        public ActionResult CallMe(CallMeViewModel callMe, string returnUrl)
        {
            WebServiceWrapper client = new WebServiceWrapper();
            var message = string.Empty;

            if (ModelState.IsValid)
            {
                var response = client.RegisterCustomerContact(callMe.FullName, callMe.PhoneNumber);

                message = "Talebiniz Alınmıştır.";
                TempData["CallMeModel"] = message;

                return Redirect(returnUrl);
            }

            if (!ModelState.IsValid)
            {
                TempData["CallMeModel"] = callMe;
                var errors = ModelState.ToArray().Select(ms => new { Key = ms.Key, ErrorMessages = string.Join(Environment.NewLine, ms.Value.Errors.Select(e => e.ErrorMessage)) }).ToArray();
                foreach (var errorItem in errors)
                {
                    if (errorItem.Key == "callMe.FullName")
                    {
                        callMe.FullNameValidationMessage = errorItem.ErrorMessages;
                    }
                    else if (errorItem.Key == "callMe.PhoneNumber")
                    {
                        callMe.PhoneNumberValidationMessage = errorItem.ErrorMessages;
                    }
                }
            }

            return Redirect(returnUrl);
        }
    }
}