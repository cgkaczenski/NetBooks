using BootstrapIntro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BootstrapIntro.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Basics()
        {
            return View();
        }

        public ActionResult Advanced()
        {
            var person = new Person
            {
                FirstName = "Chris",
                LastName = "Kaczenski"
            };

            return View(person);
        }
    }
}