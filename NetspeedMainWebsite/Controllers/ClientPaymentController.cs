using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models;
using NetspeedMainWebsite.Models.ViewModel;
using NetspeedMainWebsite.MainSiteServiceReference;
using System.Runtime.Caching;
using System.Net;
using Newtonsoft.Json;
using NLog;

namespace NetspeedMainWebsite.Controllers
{
    public class ClientPaymentController : BaseController
    {
        Logger paymentLogger = LogManager.GetLogger("payments");

        [HttpGet]
        public ActionResult BillPaymentLogin()
        {
            WebServiceWrapper genericSettings = new WebServiceWrapper();
            var googleRecaptcha = genericSettings.GenericAppSettings();

            Session.Remove("HasCustomCaptcha");
           
            ViewBag.clientCaptchaKey = googleRecaptcha.GenericAppSettings == null ? "" : googleRecaptcha.GenericAppSettings.RecaptchaClientKey;
            if (googleRecaptcha != null && !googleRecaptcha.GenericAppSettings.UseGoogleRecaptcha)
            {
                Session["HasCustomCaptcha"] = true;
            }
            return View();
        }
             

        [HttpGet]
        public ActionResult PaymentBillWithCard()
        {
            return View();
        }        
              

        [ValidateAntiForgeryToken]
        [HttpPost]

        public ActionResult BillPaymentLogin(PaymentBillViewModel clientInfos)
        {
            WebServiceWrapper genericSettings = new WebServiceWrapper();
            var googleRecaptcha = genericSettings.GenericAppSettings();

            var invalidCaptcha = Session["HasCustomCaptcha"];

            if (invalidCaptcha != null)
            {
                var customCaptcha = clientInfos.Captcha;

                var loginCaptcha = Session["LoginCaptcha"] as string;
                if (customCaptcha != loginCaptcha)
                {
                    ViewBag.clientMessage = "Lütfen Doğrulama Kodunu Doğru Doldurunuz.";
                    return View();
                }
            }
            else
            {
                var recaptchaServerkey = googleRecaptcha.GenericAppSettings == null ? "" : googleRecaptcha.GenericAppSettings.RecaptchaServerKey;
                var captchaResponseKey = Request.Form["g-Recaptcha-Response"];
                var captcha = RezaB.Web.Captcha.GoogleRecaptchaValidator.Check(recaptchaServerkey, captchaResponseKey);

                if (captcha == RezaB.Web.Captcha.GoogleRecaptchaResultType.Fail)//if not marked google captcha 
                {                    
                    ViewBag.clientMessage= "Lütfen Doğrulama Alanını Doldurunuz.";                  
                    Session.Remove("HasCustomCaptcha");

                    ViewBag.clientCaptchaKey = googleRecaptcha.GenericAppSettings == null ? "" : googleRecaptcha.GenericAppSettings.RecaptchaClientKey;
                    if (googleRecaptcha != null && !googleRecaptcha.GenericAppSettings.UseGoogleRecaptcha)
                    {
                        Session["HasCustomCaptcha"] = true;
                    }
                    return View();
                }
                if (captcha == RezaB.Web.Captcha.GoogleRecaptchaResultType.NotWorking)//not working google captcha 
                {
                    Session["HasCustomCaptcha"] = true;
                    var netspeedCaptcha = Session["HasCustomCaptcha"];

                    if (clientInfos.Captcha == null)
                    {
                        ViewBag.clientMessage = "Lütfen Doğrulama Alanını Doldurunuz.";
                    }
                }
                if (clientInfos.Captcha == null && captcha == RezaB.Web.Captcha.GoogleRecaptchaResultType.Fail)
                {
                    ViewBag.clientMessage = "Lütfen Doğrulama Alanını Doldurunuz.";
                }
            }


            if (!ModelState.IsValid)
            {
                return View();
            }

            WebServiceWrapper clientLogin = new WebServiceWrapper();

            var result = clientLogin.GetBills(clientInfos.PhoneNumber, clientInfos.ClientInfo);

            if (result.ResponseMessage.ErrorCode == 2)
            {
                Session["NotFindSub"] = "Abone Bulunamadı.";

                return RedirectToAction("ClientLoginFails", "ClientPayment");
            }

            if (result.ResponseMessage.ErrorCode == 4)
            {
                Session["NotFindBill"] = "Fatura Bulunamadı.";
                return RedirectToAction("ClientLoginFails", "ClientPayment");
            }

            if (result.ResponseMessage.ErrorCode == 5)
            {
                return RedirectToAction("AlreadyHaveCustomer", "Application");
            }

            if (result.ResponseMessage.ErrorCode != 0)
            {

                ViewBag.clientMessage = result.ResponseMessage.ErrorMessage;

                ViewBag.clientCaptchaKey = googleRecaptcha.GenericAppSettings == null ? "" : googleRecaptcha.GenericAppSettings.RecaptchaClientKey;
                if (googleRecaptcha != null && !googleRecaptcha.GenericAppSettings.UseGoogleRecaptcha)
                {
                    Session["HasCustomCaptcha"] = true;
                }
                return View();
              
            }

            if (result.ResponseMessage.ErrorCode == 199)
            {
                paymentLogger.Error($"{result.ResponseMessage.ErrorMessage} - Internal Server Error (GetBills)");
                return RedirectToAction("Home", "InternalServerError");
            }

            var ClientBillItems = result.SubscriberGetBillsResponse.Select(r => new BillInfoViewModel()
            {
                ServiceName = r.ServiceName,
                CanBePaid = r.CanBePaid,
                BillDate = r.BillDate,
                BillId = r.ID,
                Total = r.Total,
                LastPaymentDate = r.LastPaymentDate
            });
            var ClientBillList = ClientBillItems.ToList();
            if (ClientBillList.Count()==0)
            {
                //TempData["clientMessage"] = "Fatura Bulunamadı.";
                Session["HasNotBill"] = "Fatura Bulunamadı.";
                return RedirectToAction("ClientLoginFails", "ClientPayment");
            }                       

            Session["ClientBillList"] = ClientBillList;

            return RedirectToAction("PaymentBillAndResult", "ClientPayment");
        }

        public ActionResult PaymentBillAndResult()
        {
            var ClientBillList = (List<BillInfoViewModel>)Session["ClientBillList"];
            return View(ClientBillList);
        }

        public ActionResult ClientLoginFails()
        {
            if (Session["HasNotBill"]!=null)
            {
                ViewBag.HasNotBill = Session["HasNotBill"];
            }

            if (Session["NotFindBill"] != null)
            {
                ViewBag.NotFindBill = Session["NotFindBill"];
            }

            if (Session["NotFindSub"] != null)
            {
                ViewBag.NotFindSub = Session["NotFindSub"];
            }
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentBillAndResult(object SelectedBills)
        {
            //var message = string.Empty;
            var CurrentSelectedBills = ((string[])SelectedBills)[0].ToString().Split(',');
            var GetSelectedBills = new List<long>();//selected bills

            if (CurrentSelectedBills.First() == "")
            {
                ViewBag.MessageForBills= "Fatura Seçmeniz Gerekmektedir.";
                return View();
            }

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
                ViewBag.MessageForBills = "Eski Tarihli Faturalarınızı Ödemeden Diğer Faturalarınızı Ödeyemezsiniz. Lütfen Eski Tarihli Faturalarınızı Seçin.";
                return RedirectToAction("PaymentBillAndResult", "ClientPayment");
            }
            else
            {
                Session["BillIds"] = PayableBillIdList.ToArray();
                return RedirectToAction("PaymentVPOS", "ClientPayment");
            }
        }

        public ActionResult PaymentVPOS()
        {
            var BillList = (long[])Session["BillIds"];
          
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

            if (response.ResponseMessage.ErrorCode == 199)
            {
                paymentLogger.Error($"{response.ResponseMessage.ErrorMessage} - Internal Server Error (SubscriberPaymentVPOS)");
            }

            ViewBag.VPOSForm = response.PaymentVPOSResponse.HtmlForm;
            return View();
        }

        public ActionResult PaymentConfirm(string id)
        {
            var billIds = MemoryCache.Default.Get(id) as long[];

            WebServiceWrapper clientsPayBills = new WebServiceWrapper();
            var response = clientsPayBills.PayBills(billIds);

            if (response.ResponseMessage.ErrorCode == 199)
            {
                paymentLogger.Error($"{response.ResponseMessage.ErrorMessage} - Internal Server Error (PayBills)");
            }

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