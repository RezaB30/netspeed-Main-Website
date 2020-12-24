using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models;
using NetspeedMainWebsite.Models.ViewModel;

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
            var message = string.Empty;
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
                message = "Message is successful";
            }
            else
            {
                message = "Message isn't successful";
            }
            //ViewBag.message = message;
            return View(contact);
        }
    }
}