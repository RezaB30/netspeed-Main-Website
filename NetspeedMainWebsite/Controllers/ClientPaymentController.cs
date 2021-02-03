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
        //private static Logger logger = LogManager.GetLogger("router_api_errors");

        [HttpGet]
        public ActionResult BillPaymentLogin()

        {
            WebServiceWrapper genericSettings = new WebServiceWrapper();
            var googleRecaptcha = genericSettings.GenericAppSettings();

            //var t = true;

            Session.Remove("HasCustomCaptcha");

            //if (t == true)
            //{
            //    Session["HasCustomCaptcha"] = true;
            //    ViewBag.HasCustomCaptcha = true;
            //}

            //if (t == false)
            //{
            //    ViewBag.clientCaptchaKey = googleRecaptcha.GenericAppSettings == null ? "" : googleRecaptcha.GenericAppSettings.RecaptchaClientKey;

            //}

            ViewBag.clientCaptchaKey = googleRecaptcha.GenericAppSettings == null ? "" : googleRecaptcha.GenericAppSettings.RecaptchaClientKey;
            if (googleRecaptcha != null && !googleRecaptcha.GenericAppSettings.UseGoogleRecaptcha)
            {
                Session["HasCustomCaptcha"] = true;
            }
            return View();
        }

        [HttpPost]
        public ActionResult BillPaymentLogin(PaymentBillViewModel payment)
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult PaymentBillAndResult(PaymentBillViewModel clientInfos)
        //{
        //    var message = string.Empty;

        //    if (ModelState.IsValid)
        //    {
        //        WebServiceWrapper genericSettings = new WebServiceWrapper();
        //        var captcha = genericSettings.GenericAppSettings();

        //        var response = Request["g-recaptcha-response"];
        //        //const string secret = Properties.Settings.Default.CaptchaKey;

        //        //string secret = Properties.Settings.Default.CaptchaSecretKey;

        //        bool googleCaptcha = captcha.GenericAppSettings.UseGoogleRecaptcha;//false ise

        //        if (googleCaptcha == true)
        //        {
        //            string secretKey = captcha.GenericAppSettings.RecaptchaServerKey;
        //            string clientKey = captcha.GenericAppSettings.RecaptchaClientKey; 
        //            ViewBag.clientKey = clientKey;
        //        }






        //        var client = new WebClient();
        //        var reply =
        //            client.DownloadString(
        //                string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));

        //        var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponseViewModel>(reply);

        //        if (captchaResponse.Success)
        //        {
        //            WebServiceWrapper clientsBills = new WebServiceWrapper();
        //            var responseClient = clientsBills.GetBills(clientInfos.PhoneNumber, clientInfos.ClientInfo);

        //            //var url = NetspeedMainWebsite.Properties.Settings.Default.oimUrl;

        //            if (responseClient.ResponseMessage.ErrorCode == 2)
        //            {
        //                TempData["clientMessage"] = "Kayıtlı Abone Bulunamadı.";
        //                return RedirectToAction("BillPaymentLogin", "ClientPayment");
        //            }

        //            if (responseClient.ResponseMessage.ErrorCode == 4)
        //            {
        //                TempData["clientMessage"] = "Fatura Bulunamadı.";
        //                return RedirectToAction("BillPaymentLogin", "ClientPayment");
        //            }

        //            if (responseClient.ResponseMessage.ErrorCode == 5)
        //            {
        //                return RedirectToAction("AlreadyHaveCustomer", "Application");
        //            }

        //            if (responseClient.ResponseMessage.ErrorCode == 199)
        //            {
        //                paymentLogger.Error($"{responseClient.ResponseMessage.ErrorMessage} - Internal Server Error (PayBills)");
        //            }

        //            //BillInfoViewModel Bills = new BillInfoViewModel();



        //            //if (responseClient.ResponseMessage.ErrorCode == 199)
        //            //{
        //            //    paymentLogger.Error($"{responseClient.ResponseMessage.ErrorMessage} - Internal Server Error (PayBills)");
        //            //}

        //            var ClientBillItems = responseClient.SubscriberGetBillsResponse.Select(r => new BillInfoViewModel()
        //            {
        //                ServiceName = r.ServiceName,
        //                CanBePaid = r.CanBePaid,
        //                BillDate = r.BillDate,
        //                BillId = r.ID,
        //                Total = r.Total,
        //                LastPaymentDate = r.LastPaymentDate
        //            });
        //            var ClientBillList = ClientBillItems.ToList();

        //            //Session.Add("BillCheckList", ClientBillList);//kullanma hata çıkyor içinde varsa
        //            Session["ClientBillList"] = ClientBillList;

        //            //Session["ClientInfo"] = clientInfos;

        //            return View(ClientBillList);
        //        }
        //        else
        //            TempData["CaptchaMessage"] = "Lütfen güvenliği doğrulayınız.";
        //    }

        //    return View(viewName: "BillPaymentLogin", model: clientInfos);
        //}

        [ValidateAntiForgeryToken]
        [HttpPost]

        public ActionResult PaymentBillAndResult(PaymentBillViewModel clientInfos)
        //public ActionResult Login(LoginViewModel login)
        {
            WebServiceWrapper genericSettings = new WebServiceWrapper();
            var googleRecaptcha = genericSettings.GenericAppSettings();

            var invalidCaptcha = Session["HasCustomCaptcha"];

            if (invalidCaptcha != null)
            {
                //var customCaptcha = Request.Form["customCaptcha"];
                var customCaptcha = clientInfos.Captcha;

                var loginCaptcha = Session["LoginCaptcha"] as string;
                if (customCaptcha != loginCaptcha)
                {
                    return Json(new { invalidCaptcha = true, valid = "CMS.Localization.Errors.InvalidCaptcha" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //var googleRecaptcha = new ServiceUtilities().CustomerWebsiteGenericSettings();
                var recaptchaServerkey = googleRecaptcha.GenericAppSettings == null ? "" : googleRecaptcha.GenericAppSettings.RecaptchaServerKey;
                var captchaResponseKey = Request.Form["g-Recaptcha-Response"];
                var captcha = RezaB.Web.Captcha.GoogleRecaptchaValidator.Check(recaptchaServerkey, captchaResponseKey);

                // captcha control end
                if (captcha == RezaB.Web.Captcha.GoogleRecaptchaResultType.Fail)//google captcha işaretlenmediyse
                {
                    TempData["clientMessage"] = "Lütfen Doğrulama Alanını Doldurunuz.";
                    return RedirectToAction("BillPaymentLogin", "ClientPayment");
                }
                if (captcha == RezaB.Web.Captcha.GoogleRecaptchaResultType.NotWorking)//google captcha çalışmıyorsa
                {
                    Session["HasCustomCaptcha"] = true;
                    var netspeedCaptcha = Session["HasCustomCaptcha"];

                    if (clientInfos.Captcha == null)
                    {
                        TempData["clientMessage"] = "Lütfen Doğrulama Alanını Doldurunuz.";
                    }
                }
                if (clientInfos.Captcha == null && captcha == RezaB.Web.Captcha.GoogleRecaptchaResultType.Fail)
                {
                    TempData["clientMessage"] = "Lütfen Doğrulama Alanını Doldurunuz.";
                }
            }


            if (!ModelState.IsValid)
            {
                return RedirectToAction("BillPaymentLogin", "ClientPayment");
            }

            WebServiceWrapper clientLogin = new WebServiceWrapper();

            var result = clientLogin.GetBills(clientInfos.PhoneNumber, clientInfos.ClientInfo);

            if (result.ResponseMessage.ErrorCode == 4)
            {
                TempData["clientMessage"] = "Fatura Bulunamadı.";
                return RedirectToAction("ClientLoginFails", "ClientPayment");
            }

            if (result.ResponseMessage.ErrorCode == 5)
            {
                return RedirectToAction("AlreadyHaveCustomer", "Application");
            }

            if (result.ResponseMessage.ErrorCode != 0)
            {
                TempData["clientMessage"] = result.ResponseMessage.ErrorMessage;
                //return View(viewName: "BillPaymentLogin", model: clientInfos);
                return RedirectToAction("BillPaymentLogin", "ClientPayment");
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


            // sign in
            //Session.Remove("HasCustomCaptcha");
            //if (invalidCaptcha == null)
            //{
            //    return Json(new { valid = "" }, JsonRequestBehavior.AllowGet);
            //}
            //return Json(new { valid = "", invalidCaptcha = true }, JsonRequestBehavior.AllowGet);

            //Session.Add("BillCheckList", ClientBillList);//kullanma hata çıkyor içinde varsa
            Session["ClientBillList"] = ClientBillList;

            //Session["ClientInfo"] = clientInfos;

            return View(ClientBillList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentSelectBill(object SelectedBills)
        {
            var message = string.Empty;

            var CurrentSelectedBills = ((string[])SelectedBills)[0].ToString().Split(',');
            var GetSelectedBills = new List<long>();//selected bills

            if (CurrentSelectedBills.First() == "")
            {
                message = "Fatura Seçmeniz Gerekmektedir.";
                TempData["message"] = message;
                return RedirectToAction("PaymentBillAndResult", "ClientPayment");
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

        [HttpPost]
        public ActionResult GetCustomCaptcha()
        {
            Session["HasCustomCaptcha"] = true;
            var img = $"<div class='form-group py-2 m-0' style='padding-top:0 !important;padding-bottom:0 !important;'><img src='{Url.Action("LoginCaptcha", "Captcha", new { id = DateTime.Now.Ticks })}' class='custom-captcha form-control h-auto font-size-h5 border-0 px-0 text-dark py-4 px-8 rounded-lg' /></div>";
            var input = $"<div class='form-group py-2  m-0'><input autocomplete='off' type='text' name='customCaptcha' id='customCaptcha' " +
                $"class='form-control h-auto font-size-h5 border-0 px-0 text-dark py-4 px-8 rounded-lg' placeholder='{"placeholder"}' /></div>";
            var content = img + input;

            return Content(content);
        }

        public ActionResult ClientLoginFails()
        {
            return View();
        }
    }
}