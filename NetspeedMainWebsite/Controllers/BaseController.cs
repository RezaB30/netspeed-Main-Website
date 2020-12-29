using System;
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
                MainSiteServiceReference.MainSiteServiceClient client = new MainSiteServiceClient();
                var randomKey = Guid.NewGuid().ToString();
                var username = "elif";
                var passwordHash = HashUtilities.HashCalculate("123456");
                var genericHash = HashUtilities.HashCalculate($"{username}{randomKey}{passwordHash}");
                var response = client.RegisterCustomerContact(new NetspeedServiceCustomerContactRequest()
                {
                    Culture = "tr-tr",
                    Rand = randomKey,
                    Hash = genericHash,
                    Username = username,
                    CustomerContactParameters = new CustomerContactRequest()
                    {
                        FullName = callMe.FullName,
                        PhoneNo = callMe.PhoneNumber,
                        RequestTypeID = 1022,
                        RequestSubTypeID = 1048
                    }
                });

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