using System;
using System.Collections.Generic;
using System.Linq;
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
            var client = new RezaB.Mailing.Client.MailClient(Properties.Settings.Default.HostName, Properties.Settings.Default.HostPort, Properties.Settings.Default.UseSSL, Properties.Settings.Default.UserName, Properties.Settings.Default.Password);

            var ContactList = new List<ContactViewModel>();

            //client.SendMail();

            //if (ModelState.IsValid)
            //{


            //    client.SendMail(clientMessage);
            //}


            //var k = StandardMailMessage(new StandardMailMessage()
            //{ 
            //    Body=contact.Message,

            //});

            //var contactMessage = client.SendMail(new StandardMailMessage()
            //{
            //    Body=contact.Message,

            //});

            //client.SendMail(contact.Message);


            //var message = string.Empty;
            //var ContactList = new List<ContactViewModel>();

            //if (ModelState.IsValid)
            //{
            //    ContactList.Add(new ContactViewModel()
            //    {
            //        FullName = contact.FullName,
            //        PhoneNumber = contact.PhoneNumber,
            //        EmailAddress = contact.EmailAddress,
            //        Message = contact.Message
            //    });
            //    message = "Message is successful";
            //}
            //else
            //{
            //    message = "Message isn't successful";
            //}
            ////ViewBag.message = message;
            return View(contact);
        }
    }
}