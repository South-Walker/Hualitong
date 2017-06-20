using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeChatMVC.Controllers
{
    public class BaiduMapController : Controller
    {
        // GET: BaiduMap
        public ActionResult Index()
        {
            return View("MapView");
        }
    }
}