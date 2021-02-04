using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetspeedMainWebsite.MainSiteServiceReference;

namespace NetspeedMainWebsite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Application()
        {
            return View();
        }

        public ActionResult ContractAndForm()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult FastLogin()
        {
            return View();
        }

        public ActionResult SpeedTest()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult InternalServerError()
        {
            return View();
        }

        public ActionResult TestMe()
        {
            return View();
        }

        public ActionResult TestMeForApp()
        {
            return View();
        }
    }
}