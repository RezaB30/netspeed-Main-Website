using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models;
using NetspeedMainWebsite.Models.ViewModel;
using RezaB.Mailing;

namespace NetspeedMainWebsite.Controllers
{
    public class ContactController : BaseController
    {
        [HttpGet]
        public ActionResult ContactForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactForm(ContactViewModel contact)
        {
            var mailClient = new RezaB.Mailing.Client.MailClient(Properties.Settings.Default.MailHostName, Properties.Settings.Default.MailHostPort, Properties.Settings.Default.MailUseSSL, Properties.Settings.Default.MailUserName, Properties.Settings.Default.MailPassword);

            var ContactList = new List<ContactViewModel>();
            var ContactMessageList = new List<ContactViewModel>();
            if (ModelState.IsValid)
            {
                ContactList.Add(new ContactViewModel()
                {
                    FullName = contact.FullName,
                    PhoneNumber = contact.PhoneNumber,
                    EmailAddress = contact.EmailAddress,
                    Message = contact.Message
                });

                var body = string.Join("\n", new[] {  "Ad Soyad:", contact.FullName, "Telefon Numarası:", contact.PhoneNumber, "E-Posta Adresi:" , contact.EmailAddress,"Mesaj:", contact.Message });

                var Message = string.Empty;

                string[] mailTo = { Properties.Settings.Default.MailUserName };
                string[] mailCc = null;
                string[] mailBc = null;
                IEnumerable<MailFileAttachment> mailAttachment = Enumerable.Empty<MailFileAttachment>();

                var mailMessage = new StandardMailMessage(new MailAddress(mailClient.Username, "Netspeed Contact Form"), mailTo, mailCc, mailBc, "Müşteri İletişim Formu", body, null, mailAttachment);

                mailClient.SendMail(mailMessage);

                Message = "Mesajınız İletilmiştir. En Kısa Sürede Size Dönüş Yapılacaktır.";

                ViewBag.Message = Message;
                return View();
            }

            return View();
        }
    }
}