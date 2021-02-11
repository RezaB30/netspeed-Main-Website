using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models.ViewModel;
using NetspeedMainWebsite.MainSiteServiceReference;
using NLog;
using System.Threading;
using System.Globalization;

namespace NetspeedMainWebsite.Controllers
{
    public class BaseController : Controller
    {
        protected static Logger logger = LogManager.GetLogger("main");
        Logger helpBubbleLogger = LogManager.GetLogger("help-bubble");

        WebServiceWrapper client = new WebServiceWrapper();

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("tr-tr");
            return base.BeginExecuteCore(callback, state);
        }

        [HttpPost]
        public ActionResult CallMe(CallMeViewModel callMe, string returnUrl)
        {
            var callMessages = string.Empty;

            if (ModelState.IsValid)
            {
                var response = client.RegisterCustomerContact(callMe.FullName, callMe.PhoneNumber);

                callMessages = "Talebiniz Alınmıştır.";
                TempData["callMessages"] = callMessages;

                if (response.ResponseMessage.ErrorCode == 199)
                {
                    helpBubbleLogger.Error($"{response.ResponseMessage.ErrorMessage} - Internal Server Error (RegisterCustomerContact)");
                }

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
                ViewName = "~/Views/Home/Error.cshtml",
            };
            filterContext.ExceptionHandled = true;
            logger.Error(filterContext.Exception);

        }

    }
}