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
        static string root = @"C:\Users\Administrator\Desktop\hualitongbuffer\";
        // GET: Server
        public ActionResult Index()
        {
            return View();
        }
        public static bool IsExist(string openid)
        {
            foreach (var del in BufferSubdirectory.events)
            {
                string path = root + del.Key + @"\" + openid;
                if (!System.IO.File.Exists(path))
                    return false;
            }
            return true;
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
                if (IsExist(item.wechatid))
                {
                    result += item.studentnum + ":existed<br>";
                    continue;
                }
                JWCHttpHelper.Login(item.studentnum, item.pwd);
                if (JWCHttpHelper.IsLogin)
                {
                    foreach (var del in BufferSubdirectory.events)
                    {
                        path = root + del.Key + @"\" + item.wechatid;
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