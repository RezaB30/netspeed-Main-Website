﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models.ViewModel;
using NetspeedMainWebsite.MainSiteServiceReference;

namespace NetspeedMainWebsite.Controllers
{
    public class BaseController : Controller
    {
        WebServiceWrapper client = new WebServiceWrapper();
        public ActionResult OnException()
        {
            return View();
        }

        //HashUtilities hash = new HashUtilities();

        [HttpPost]
        public ActionResult CallMe(CallMeViewModel callMe, string returnUrl)
        {
            var message = string.Empty;
            //var CallMeList = new List<CallMeViewModel>();

            if (ModelState.IsValid)
            {
                var response = client.RegisterCustomerContact(callMe.FullName, callMe.PhoneNumber);
                
                ViewBag.message = message;
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

        protected override void OnException(ExceptionContext filterContext)
        {
            var message = string.Empty;

            filterContext.Result = new ViewResult
            {
                //ViewName = "~/Views/Home/Error.cshtml",
            };
            filterContext.ExceptionHandled = true;
        }

    }
}