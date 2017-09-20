using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WeChatMVC.Models;
using System.Collections;

namespace WeChatMVC.Controllers
{
    public class BufferSubdirectory
    {
        public static string ClassTable = "classtablehtml";
        public static string GradePoint = "gradepoint";
        public static string LargeTable = "largetable";
        public static string SmallTable = "smalltable";
        public static Dictionary<string, JWCHttpHelper.CrawlerDetail<string>> events = new Dictionary<string, JWCHttpHelper.CrawlerDetail<string>>();
        public BufferSubdirectory()
        {
            if (events.Count == 0)
            {
                events.Add(ClassTable, JWCHttpHelper.classtablehtml);
                events.Add(GradePoint, JWCHttpHelper.gradepoint);
                events.Add(LargeTable, JWCHttpHelper.largetable);
                events.Add(SmallTable, JWCHttpHelper.smalltable);
            }
        }
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
            if (System.IO.File.Exists(path))
            {
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
            else
            {
                StudentInfo stif = DBManual.SelectUser(openid);
                stif.Check();
                if(stif.IsSuccess)
                {
                    return "请确认您的密码:" + HttpUtility.UrlDecode(stif.pwd) + "正确，我们将在24小时内为您开通功能。";
                }
                else
                {
                    return stif.errormessage;
                }
            }
        }
    }
}