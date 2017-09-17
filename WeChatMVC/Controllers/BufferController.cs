using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WeChatMVC.Models;

namespace WeChatMVC.Controllers
{
    public class BufferSubdirectory
    {
        public static string ClassTable = "classtablehtml";
        public static string GradePoint = "gradepoint";
        public static string LargeTable = "largetable";
        public static string SmallTable = "smalltable";
    }
    public class BufferController : Controller
    {
        static string root = @"C:\Users\Administrator\Desktop\hualitongbuffer\";
        // GET: Buffer
        public ActionResult Index()
        {
            return View();
        }
        public static string Select(string detail, string openid)
        {
            string result = "";
            string path = root + detail + "/" + openid;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            result = sr.ReadToEnd();
            if (detail != BufferSubdirectory.ClassTable)
                return result;
            else
            {
                return JWCHttpHelper.ClassTableFromHtml<string>(result); 
            }
        }
    }
}