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
        public static string SelectClassTableImg(UserRequest userrequest)
        {
            string path = root + BufferSubdirectory.ClassTable + "/" + userrequest.FromUserName;
            if (System.IO.File.Exists(path))
            {
                ClassTableDrawer drawer = new ClassTableDrawer();
                Stream stream = drawer.DrawClassTableInStream(userrequest.FromUserName);
                WeChatHttpHelper.GetToken();
                return userrequest.Get_Img(WeChatHttpHelper.GetMediaID(stream));
            }
            else
            {
                return userrequest.Get_Reply(isnotexist(userrequest.FromUserName));
            }
        }
        public static string Select(string detail, UserRequest userrequest)
        {
            string result = "";
            string path = root + detail + "/" + userrequest.FromUserName;
            if (System.IO.File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                result = sr.ReadToEnd();
                if (detail != BufferSubdirectory.ClassTable)
                    return userrequest.Get_Reply(result);
                else
                {
                    return userrequest.Get_Reply(JWCHttpHelper.ClassTableFromHtml<string>(result));
                }
            }
            else
            {
                return userrequest.Get_Reply(isnotexist(userrequest.FromUserName));
            }
        }
        private static string isnotexist(string openid)
        {
            StudentInfo studentinfo = DBManual.SelectUser(openid);
            studentinfo.Check();
            if (studentinfo.IsSuccess)
            {
                return "请确认您的密码:" + HttpUtility.UrlDecode(studentinfo.pwd) + "正确，我们将在24小时内为您开通功能。";
            }
            else
            {
                return studentinfo.errormessage;
            }
        }
    }
}