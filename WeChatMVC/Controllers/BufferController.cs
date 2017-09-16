using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace WeChatMVC.Controllers
{
    public class BufferController : Controller
    {
        static string root = @"C:\Users\Administrator\Desktop\hualitongbuffer\";
        // GET: Buffer
        public ActionResult Index()
        {
            return View();
        }
        public static string Select(string openid)
        {
            string result = "";
            string path = root + openid;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            result = sr.ReadToEnd();
            return result;
        }
    }
}