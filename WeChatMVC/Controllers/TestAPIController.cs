using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeChatMVC.Controllers
{
    public class TestAPIController : BaseController
    {
        // GET: TestAPI
        public string SMS()
        {
            SendSMS("17077706886");

            return "success";
        }
    }
}