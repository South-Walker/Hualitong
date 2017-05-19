using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace WeChatMVC.Controllers
{
    public class BindingController : BaseController
    {
        // GET: Binding
        public ActionResult Index()
        {
            string openid = Request.QueryString["openid"];
            string secret = Request.QueryString["secret"];
            if (false)//secret == MD5Encrypter(openid))
            {
                return View("BindingElseView");
            }
            else
                return View("BindingStudentNumView");
        }
    }
}