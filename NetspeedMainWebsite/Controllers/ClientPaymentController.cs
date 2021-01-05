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
        // GET: PaymentBill
        //HashUtilities hash = new HashUtilities();
        MainSiteServiceClient client = new MainSiteServiceClient();



        [HttpGet]
        public ActionResult PaymentBill()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PaymentBill(PaymentBillViewModel payment, string PhoneNumber, string ClientInfo)
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
            if (ModelState.IsValid)
            {
                MainSiteServiceClient client = new MainSiteServiceClient();
                var randomKey = Guid.NewGuid().ToString();
                var username = "elif";
                var passwordHash = HashUtilities.HashCalculate("123456");
                var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");


                var response = client.GetBills(new NetspeedServiceSubscriberGetBillsRequest()
                {
                    Culture = "tr-tr",
                    Rand = randomKey,
                    Hash = serviceRequestHash,
                    Username = username,
                    GetBillParameters = new SubscriberGetBillsRequest
                    {
                        TCKOrSubscriberNo = ClientInfo,
                        PhoneNo = PhoneNumber
                    }
                });
                //var url = NetspeedMainWebsite.Properties.Settings.Default.oimUrl;
                //if (response.ResponseMessage.ErrorMessage ==)
                //{

                //Redirect();
                //} 
               

                BillInfoViewModel Bills = new BillInfoViewModel();
                //modelstate.isvalid
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
        //public ActionResult PaymentBillAndResult(object SelectedBills)
        {
            var message = string.Empty;

            var CurrentSelectedBills = ((string[])SelectedBills)[0].ToString().Split(',');
            var GetSelectedBills = new List<long>();//selected bills

            //if (PayableBillIdList.Count == 0)
            //{
            //    message = "Eski Tarihli Faturalarınızı Ödemeden Diğer Faturalarınızı Ödeyemezsiniz. Lütfen Eski Tarihli Faturalarınızı Seçin.";
            //    ViewBag.message = message;
            //    //tempdata
            //    return RedirectToAction("PaymentBillAndResult", "ClientPayment");
            //}
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
                TempData["message"]= "Eski Tarihli Faturalarınızı Ödemeden Diğer Faturalarınızı Ödeyemezsiniz. Lütfen Eski Tarihli Faturalarınızı Seçin.";
                //message = "Eski Tarihli Faturalarınızı Ödemeden Diğer Faturalarınızı Ödeyemezsiniz. Lütfen Eski Tarihli Faturalarınızı Seçin.";
                //ViewBag.message = message;
                //tempdata
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

            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.SubscriberPaymentVPOS(new NetspeedServicePaymentVPOSRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                PaymentVPOSParameters = new PaymentVPOSRequest
                {
                    BillIds = BillList,
                    FailUrl = Url.Action("PaymentFail", "ClientPayment", new { id = Key }, Request.Url.Scheme),
                    OkUrl = Url.Action("PaymentConfirm", "ClientPayment", new { id = Key }, Request.Url.Scheme),
                }
            });


            ViewBag.VPOSForm = response.PaymentVPOSResponse.HtmlForm;
            return View();
        }

        public ActionResult PaymentConfirm(string id)
        {
            var billIds = MemoryCache.Default.Get(id) as long[];

            var randomKey = Guid.NewGuid().ToString();
            var username = "elif";
            var passwordHash = HashUtilities.HashCalculate("123456");
            var serviceRequestHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");

            var response = client.PayBills(new NetspeedServicePayBillsRequest()
            {
                Culture = "tr-tr",
                Rand = randomKey,
                Hash = serviceRequestHash,
                Username = username,
                PayBillsParameters = billIds
            });

            MemoryCache.Default.Remove(id);
            return View();
        }

        public ActionResult PaymentFail(string id)
        {
            MemoryCache.Default.Remove(id);
            return View();
        }
    }
}