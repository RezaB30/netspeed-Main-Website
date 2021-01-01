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
        MainSiteServiceClient client = new MainSiteServiceClient();

        [HttpGet]
        public ActionResult PaymentBill()
        {
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentSelectBill(object SelectedBills)
        //public ActionResult PaymentBillAndResult(object SelectedBills)
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

            var PayableBillIdList = new List<long>();

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
                message = "Eski Tarihli Faturalarınızı Ödemeden Diğer Faturalarınızı Ödeyemezsiniz. Lütfen Eski Tarihli Faturalarınızı Seçin.";
                ViewBag.message = message;
                return RedirectToAction("PaymentBillAndResult", "ClientPayment");
            }
            else
            {
                Session["BillIds"] = PayableBillIdList;
                return RedirectToAction("PaymentVPOS", "ClientPayment");
            }


        }
        public ActionResult PaymentVPOS(long[] BillIds)
        {

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
                    BillIds = BillIds,
                    FailUrl = "/",
                    OkUrl = "/",//paybills
                    
                }

            });
            return View();
        }
    }
}