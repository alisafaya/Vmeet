using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vmeet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Toplantilar()
        {
            ViewBag.Message = "Toplantilar";
            //Redirect to Toplantilar controller
            return View();
        }

        public ActionResult Hakkimizda()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}