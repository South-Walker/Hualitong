using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeChatMVC.Models;
using System.Text.RegularExpressions;
using System.IO;

namespace WeChatMVC.Controllers
{
    public class ServerController : Controller
    {
        // GET: Server
        public ActionResult Index()
        {
            return View();
        }
        public static string UpdateJWC()
        {
            string result = "";
            string path;
            FileStream fs;StreamWriter sw;
            List<StudentInfo> alluser = DBManual.SelectAll();
            foreach (StudentInfo item in alluser)
            {
                Regex regex = new Regex(@"^\d+$");
                if (regex.IsMatch(item.pwd))
                    continue;
                JWCHttpHelper.Login(item.studentnum, item.pwd);
                if (JWCHttpHelper.IsLogin)
                {
                    foreach (var del in BufferSubdirectory.events)
                    {
                        string root = @"C:\Users\Administrator\Desktop\hualitongbuffer\" + del.Key + @"\";
                        path = root + item.wechatid;
                        if (System.IO.File.Exists(path))
                        {
                            result += del.Key + " of " + item.studentnum + ":hasexist<br>";
                            continue;
                        }
                        string now;
                        now = del.Value();
                        fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        sw = new StreamWriter(fs);
                        sw.WriteLine(now);
                        sw.Flush();
                        sw.Close();
                        fs.Close();
                        result += del.Key + " of " + item.studentnum + ":success<br>";
                    }
                    JWCHttpHelper.ClearCookies();
                }
                else
                {
                    result += item.studentnum + ":" + JWCHttpHelper.ErrorMsg + "<br>";
                }
            }
            return result;
        }
    }
}