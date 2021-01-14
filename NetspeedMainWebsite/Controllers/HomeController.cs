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

            //var Key = Guid.NewGuid().ToString();
            //var Value = DateTime.Now.Minute;

            //if (true)
            //{
            //    Url.Action("PaymentFail", "ClientPayment", new { id = Key });
            //}



            //MemoryCache.Default.Add(Key, Value, DateTimeOffset.Now.AddMinutes(15));



            //Session["Action"] = DateTime.Now.Minute;
            //var SmsCountDown = Session["Action"];
            ////ViewBag.SmsCountDown = DateTime.Now.Minute;

            //if (Session["Action"] == null || (DateTime.Now - (DateTime)(Session["Action"])).Minutes > 3)
            //{
            //    //var response = client.RegisterSMSValidation(Gsm, smsCode.ToString());//buraya dön

            //    //Session["Action"] = DateTime.Now;
            //    //if (response.ResponseMessage.ErrorCode == 0)
            //    //{
            //    //    return RedirectToAction("ApplicationSummary", "Application");
            //    //}
            //}
            //else
            //{
            //    return View();
            //}
            return View();

        }

        //public ActionResult SpeedTest()
        //{
        //    return View();
        //}



        //public ActionResult QuestioningAndConclusion()
        //{
        //    return View();
        //}

        public ActionResult Error()
        {
            return View();
        }
    }
}