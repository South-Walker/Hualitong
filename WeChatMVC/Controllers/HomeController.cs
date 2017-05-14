using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeChatMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public string Index()
        {
            return "aaa";
        }
        public ActionResult GetResult()
        {
            return View("firstView");
        }
        public string Posttest()
        {
            return "IJIJI";
        }
    }
}