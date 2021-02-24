using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.MainSiteServiceReference;
using NetspeedMainWebsite.Models.ViewModel;
using RezaB.Mailing;

namespace NetspeedMainWebsite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        [Route("giris")]
        public ActionResult RedirectCustomerServiceSite()
        {
            return Redirect("https://online.netspeed.com.tr");
        }
        [Route("destek/{title?}")]
        public ActionResult Support(string title)
        {
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }
            if (string.IsNullOrEmpty(title))
            {
                return View();
            }
            if (title == "fatura-odeme-sonuc")
            {
                if (Session["BillList"] == null)
                {
                    return RedirectToAction("Support", "Home", new { title = "fatura-odeme" });
                }
                var bills = (Models.ViewModel.BillInfoViewModel[])Session["BillList"];
                Session.Remove("BillList");
                return View($"~/Views/Home/SupportParts/fatura-odeme-sonuc.cshtml", bills);
            }
            if (title == "fatura-odeme-toplam")
            {
                if (Session["HtmlForm"] == null || Session["TotalCount"] == null || Session["IsPaid"] == null)
                {
                    Session.Remove("HtmlForm");
                    Session.Remove("TotalCount");                    
                    return RedirectToAction("Support", "Home", new { title = "fatura-odeme" });
                }
                Session.Remove("IsPaid");
            }
            return View($"~/Views/Home/SupportParts/{title}.cshtml");
        }
        [Route("hiz-testi")]
        public ActionResult SpeedTest()
        {
            return View();
        }
        [Route("altyapi-sorgula")]
        public ActionResult InfrastructureCheck()
        {
            var responseProvince = new WebServiceWrapper().GetProvinces();
            var ProvinceList = responseProvince.ValueNamePairList.Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Code.ToString()
            });

            ViewBag.ProvinceList = ProvinceList;
            var DistrictList = new List<SelectListItem>();
            var RegionList = new List<SelectListItem>();
            var NeighborhoodList = new List<SelectListItem>();
            var StreetList = new List<SelectListItem>();
            var BuildingList = new List<SelectListItem>();
            var ApartmentList = new List<SelectListItem>();

            ViewBag.DistrictList = DistrictList;
            ViewBag.RegionList = RegionList;
            ViewBag.NeighborhoodList = NeighborhoodList;
            ViewBag.StreetList = StreetList;
            ViewBag.BuildingList = BuildingList;
            ViewBag.ApartmentList = ApartmentList;

            return View();
        }
        [Route("sorgu-sonuc")]
        public ActionResult InfrastructureResult()
        {
            if (Session["InfrastructureResult"] == null)
            {
                return RedirectToAction("InfrastructureCheck", "Home");
            }
            var infrastructureResult = (Models.ViewModel.InfrastructureInquiryResultViewModel)Session["InfrastructureResult"];
            Session.Remove("InfrastructureResult");
            return View(infrastructureResult);
        }
        [Route("sozlesme-ve-form")]
        public ActionResult ContractAndForms()
        {
            return View();
        }
        [Route("kvkk")]
        public ActionResult KVKK()
        {
            return PartialView("~/Views/Home/SupportParts/kvkk.cshtml");
        }
        [Route("kampanyalar/{title?}")]
        public ActionResult SpecialOffers(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return View();
            }
            return View($"~/Views/Home/SpecialOfferParts/{title}.cshtml");
        }
        [HttpPost]
        public ActionResult BillCollection(string gsmNo, string tckNo)
        {
            var clearGsmNo = gsmNo.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("_", "").Replace("-", "");
            var clearTckNo = tckNo.Replace(" ", "").Replace("_", "");
            var gsmNoError = string.Empty;
            var tckNoError = string.Empty;
            var IsValid = true;
            if (string.IsNullOrEmpty(clearGsmNo) || clearGsmNo.Length != 10)
            {
                IsValid = false;
                gsmNoError = "<span style='color:red;'>Lütfen geçerli bir telefon Numarası giriniz</span>";
            }
            if (string.IsNullOrEmpty(clearTckNo) || clearTckNo.Length != 11)
            {
                IsValid = false;
                tckNoError = "<span style='color:red;'>Lütfen geçerli bir TC Kimlik Numarası giriniz</span>";
            }
            if (!IsValid)
            {
                TempData["tckNoError"] = tckNoError;
                TempData["gsmNoError"] = gsmNoError;
                return RedirectToAction("Support", "Home", new { title = "fatura-odeme" });
            }
            var wrapper = new WebServiceWrapper();
            var response = wrapper.GetBills(clearGsmNo, clearTckNo);
            if (response.ResponseMessage.ErrorCode == 0 && response.SubscriberGetBillsResponse != null)
            {
                Session["gsmNo"] = clearGsmNo;
                Session["tckNo"] = clearTckNo;
                Session["BillList"] = response.SubscriberGetBillsResponse.OrderBy(b => b.BillDate).Select(b => new Models.ViewModel.BillInfoViewModel()
                {
                    BillDate = b.BillDate,
                    CanBePaid = b.CanBePaid,
                    BillId = b.ID,
                    LastPaymentDate = b.LastPaymentDate,
                    ServiceName = b.ServiceName,
                    Total = b.Total,
                }).ToArray();
                return RedirectToAction("Support", "Home", new { title = "fatura-odeme-sonuc" });
            }
            if (response.ResponseMessage.ErrorCode == 4)
            {
                TempData["gsmNoError"] = "<span style='color:red;' class='h4'>Ödenmemiş faturanız bulunmamaktadır.</span>";
            }
            if (response.ResponseMessage.ErrorCode == 5)
            {
                TempData["gsmNoError"] = "<span style='color:red;' class='h4'>Aboneliğiniz bu işlem için uygun değil. Online işlem merkezi üzerinden ödemenizi yapabilirsiniz.</span>";
            }
            return RedirectToAction("Support", "Home", new { title = "fatura-odeme" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment()
        {
            if (Session["HtmlForm"] == null)
            {
                return RedirectToAction("Support", "Home", new { title = "fatura-odeme" });
            }
            var htmlForm = Session["HtmlForm"] as string;
            Session.Remove("HtmlForm");
            Session.Remove("TotalCount");
            return Content(htmlForm);
        }
        [HttpPost]
        public ActionResult PayBills(long[] selectedBills)
        {
            if (selectedBills == null || selectedBills.Count() == 0)
            {
                var wrapper = new WebServiceWrapper();
                var response = wrapper.GetBills(Session["gsmNo"] as string, Session["tckNo"] as string);
                if (response.ResponseMessage.ErrorCode == 0 && response.SubscriberGetBillsResponse != null)
                {
                    var bills = response.SubscriberGetBillsResponse.Select(b => new Models.ViewModel.BillInfoViewModel()
                    {
                        BillDate = b.BillDate,
                        CanBePaid = b.CanBePaid,
                        BillId = b.ID,
                        LastPaymentDate = b.LastPaymentDate,
                        ServiceName = b.ServiceName,
                        Total = b.Total,
                    }).ToArray();
                    Session["BillList"] = bills;
                    TempData["errorMessage"] = "Lütfen ödemek istediğiniz faturaları seçiniz";
                    return RedirectToAction("Support", "Home", new { title = "fatura-odeme-sonuc" });
                }
                return RedirectToAction("Support", "Home", new { title = "fatura-odeme" });
            }
            {
                var wrapper = new WebServiceWrapper();
                var response = wrapper.GetBills(Session["gsmNo"] as string, Session["tckNo"] as string);
                if (response.ResponseMessage.ErrorCode == 0 && response.SubscriberGetBillsResponse != null)
                {
                    var bills = response.SubscriberGetBillsResponse.Select(b => new Models.ViewModel.BillInfoViewModel()
                    {
                        BillDate = b.BillDate,
                        CanBePaid = b.CanBePaid,
                        BillId = b.ID,
                        LastPaymentDate = b.LastPaymentDate,
                        ServiceName = b.ServiceName,
                        Total = b.Total,
                    }).ToArray();
                    var customerBills = response.SubscriberGetBillsResponse.OrderBy(b => b.BillDate).Take(selectedBills.Count()).Select(b => b.ID).ToArray();
                    if (customerBills.SequenceEqual(selectedBills))
                    {
                        var Key = Guid.NewGuid().ToString();
                        MemoryCache.Default.Add(Key, customerBills, DateTimeOffset.Now.AddMinutes(15));
                        var payWrapper = new WebServiceWrapper();
                        var payResult = payWrapper.SubscriberPaymentVPOS(customerBills, Url.Action("PaymentFail", "Home", new { id = Key }, Request.Url.Scheme),
                            Url.Action("PaymentConfirm", "Home", new { id = Key }, Request.Url.Scheme));
                        if (payResult.ResponseMessage.ErrorCode == 0)
                        {
                            var billsTotal = bills.Where(b => customerBills.Contains(b.BillId)).Select(b => b.Total).ToArray();
                            double total = 0;
                            foreach (var item in billsTotal)
                            {
                                total += Double.Parse(item, CultureInfo.InvariantCulture);
                            }
                            Session["TotalCount"] = total;
                            var htmlForm = payResult.PaymentVPOSResponse.HtmlForm;
                            Session["htmlForm"] = htmlForm;
                            Session["IsPaid"] = true;
                            return RedirectToAction("Support", "Home", new { title = "fatura-odeme-toplam" });
                            //return Content(htmlForm);
                        }
                        TempData["errorMessage"] = payResult.ResponseMessage.ErrorMessage;
                        Session["BillList"] = bills;
                        return RedirectToAction("Support", "Home", new { title = "fatura-odeme-sonuc" });
                    }
                    TempData["errorMessage"] = "Lütfen fatura ödemenizi son ödeme tarihine (eskiden yeniye) göre yapınız.";
                    Session["BillList"] = bills;
                    return RedirectToAction("Support", "Home", new { title = "fatura-odeme-sonuc" });
                }
                return RedirectToAction("Support", "Home", new { title = "fatura-odeme" });
            }
        }
        public ActionResult PaymentConfirm(string id)
        {
            var billIds = MemoryCache.Default.Get(id) as long[];

            WebServiceWrapper clientsPayBills = new WebServiceWrapper();
            var response = clientsPayBills.PayBills(billIds);

            if (response.ResponseMessage.ErrorCode == 0)
            {
                // log
                return RedirectToAction("Support", "Home", new { title = "odeme-tamam" });
            }
            // log fail
            MemoryCache.Default.Remove(id);
            return RedirectToAction("Support", "Home", new { title = "odeme-tamam" });
        }
        public ActionResult PaymentFail(string id)
        {
            MemoryCache.Default.Remove(id);
            TempData["pay-generic-error"] = "Ödeme işlemi başarısız. Tekrar deneyiniz.";
            return RedirectToAction("Support", "Home", new { title = "fatura-odeme" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactForm(ContactViewModel contact)
        {
            var mailClient = new RezaB.Mailing.Client.MailClient(Properties.Settings.Default.MailHostName, Properties.Settings.Default.MailHostPort, Properties.Settings.Default.MailUseSSL, Properties.Settings.Default.MailUserName, Properties.Settings.Default.MailPassword);

            var ContactList = new List<ContactViewModel>();
            if (ModelState.IsValid)
            {
                ContactList.Add(new ContactViewModel()
                {
                    FullName = contact.FullName,
                    PhoneNumber = contact.PhoneNumber,
                    EmailAddress = contact.EmailAddress,
                    Message = contact.Message
                });

                var body = string.Join("\n", new[] { "Ad Soyad:", contact.FullName, "Telefon Numarası:", contact.PhoneNumber, "E-Posta Adresi:", contact.EmailAddress, "Mesaj:", contact.Message });

                string[] mailTo = { Properties.Settings.Default.MailUserName };
                string[] mailCc = null;
                string[] mailBc = null;
                IEnumerable<MailFileAttachment> mailAttachment = Enumerable.Empty<MailFileAttachment>();

                var mailMessage = new StandardMailMessage(new MailAddress(mailClient.Username, "Netspeed Contact Form"), mailTo, mailCc, mailBc, "Müşteri İletişim Formu", body, null, mailAttachment);

                mailClient.SendMail(mailMessage);
                ModelState.AddModelError("resultMessage", "Mesajınız İletilmiştir. En Kısa Sürede Size Dönüş Yapılacaktır.");
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Support", "Home", new { title = "iletisimformu" });
            }
            TempData["ViewData"] = ViewData;
            return RedirectToAction("Support", "Home", new { title = "iletisimformu" });
        }
        [HttpPost]
        public ActionResult CallUs(string name, string phone)
        {
            WebServiceWrapper wrapper = new WebServiceWrapper();
            var response = wrapper.RegisterCustomerContact(name, phone);
            if (response.ResponseMessage.ErrorCode == 0)
            {
                return Json(new { message = "Talebiniz alındı. En kısa sürede geri dönüş yapacağız.", errorCode = "success" }, JsonRequestBehavior.AllowGet);
            }
            if (response.ResponseMessage.ErrorCode == 200)
            {
                return Json(new { message = response.ResponseMessage.ErrorMessage, errorCode = "error" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.", errorCode = "error" }, JsonRequestBehavior.AllowGet);
        }
    }
}