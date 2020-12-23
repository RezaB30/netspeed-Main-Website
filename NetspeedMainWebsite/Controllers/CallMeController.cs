using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.Models;
using NetspeedMainWebsite.Models.ViewModel;

namespace NetspeedMainWebsite.Controllers
{
    public class CallMeController : Controller
    {
        // GET: CallMe
        public ActionResult Index()
        {
            return View();
        }


        //[HttpGet]
        //public ActionResult AddCallMe()
        //{
        //    CallMeViewModel model = new CallMeViewModel();
        //    return View(model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCallMe(CallMeViewModel addCallMe)
        {
            var message = string.Empty;
            var CallMeList = new List<CallMeViewModel>();

            if (ModelState.IsValid)
            {
                CallMeList.Add(new CallMeViewModel()
                {
                    FullName = addCallMe.FullName,
                    PhoneNumber = addCallMe.PhoneNumber
                });
                message = "message is successful";
            }
            else
            {
                message = "message isn't successful";
            }
            ViewBag.message=message;
            return View();
        }


    }

}