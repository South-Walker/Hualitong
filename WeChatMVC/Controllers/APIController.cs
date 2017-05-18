using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;
using System.Web;
using System.Collections;
using System.Net.Sockets;
using System.Web.Mvc;

namespace WeChatMVC.Controllers
{
    class MyHttpHelper
    {
        static CookieContainer cookiecontainer = new CookieContainer();
        static CookieCollection cookiecollection = new CookieCollection();
        HttpWebRequest request;
        HttpWebResponse response;
        string html = string.Empty;
        public MyHttpHelper(string url)
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = MyHttpHelper.cookiecontainer;
            cookiecontainer.Add(request.RequestUri, MyHttpHelper.cookiecollection);
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            request.Accept = "*/*;";
            request.UserAgent = "Mozilla/5.0";
            request.ContentType = "application/x-www-form-urlencoded";
        }
        public void HttpGet()
        {
            GetResponse();
        }
        public void HttpPost(string postcontent)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(postcontent);
            request.ContentLength = bytes.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            GetResponse();
        }
        private void GetResponse()
        {
            response = (HttpWebResponse)request.GetResponse();
            ReadHtml();
            EndCookie();
        }
        private void ReadHtml()
        {
            StreamReader sr = new StreamReader(response.GetResponseStream());
            html = sr.ReadToEnd();
        }
        private void EndCookie()
        {
            cookiecollection = response.Cookies;
        }
        public override string ToString()
        {
            return html;
        }
    }
    public class APIController : BaseController
    {
        // GET: API
        public string ty(string studentnum, string pwd)
        {
            string post = "__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwULLTIxMjk4Nzk4MzUPZBYCZg9kFghmDxBkZBYBZmQCBQ8PFgIeBFRleHRlZGQCBg8PFgIfAAUSMjAxNC85LzE2IDExOjI1OjUyZGQCBw8PFgIfAAUSMjAzNC85LzE2IDExOjI1OjUyZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFBWJ0bm9rg7wsmKWoIGv%2B2o6SEkUuPN0q80w%3D&dlljs=st&" +
                "txtuser=" + studentnum +
                "&txtpwd=" + pwd +
                "&__VIEWSTATEGENERATOR=5B21F7B0&__EVENTVALIDATION=%2FwEWCAKAvaGBDAKBwaG%2FAQLMrvvQDQLd8tGoBALWwdnoCALB2tiCDgKd%2B7q4BwL9kpmqCm3geVz7Uj28fz9x4hImTVbLX%2BiL&btnok.x=0&btnok.y=0";

            MyHttpHelper httphelper = new MyHttpHelper(@"http://59.78.92.38:8888/sportscore/");
            httphelper.HttpGet();
            httphelper = new MyHttpHelper(@"http://59.78.92.38:8888/sportscore/");
            httphelper.HttpPost(post);


            Regex re = new Regex("<span id=\"lblname\">\\s*(?<name>[^\\s]*)\\s*</span>[\\s\\S]*?<span id=\"lblmsg\">[\\s\\S]*?早操：[\\s\\S]*?>(?<morningexercises>\\d+)<[\\s\\S]*?课外活动1：[\\s\\S]*?>(?<outclassactivite1>\\d+)<[\\s\\S]*?</span>");
            Match match = re.Match(httphelper.ToString());
            GroupCollection gc = match.Groups;
            string name = gc["name"].Value;
            string morningexercise = gc["morningexercises"].Value;
            string outclassactivite1 = gc["outclassactivite1"].Value;

            return Get_Reply(Get_Score(studentnum, pwd));
        }

        public JsonResult lib()
        {
            Regex re = new Regex("(?<=>)\\d+(?=<)");
            MyHttpHelper library = new MyHttpHelper(@"http://lib.ecust.edu.cn:8081/gateseat/lrp.aspx");
            library.HttpGet();
            MatchCollection mc = re.Matches(library.ToString());
            JsonResult json = new JsonResult();
            var data = new object[6];
            for (int i = 1; i <= 6; i++)
            {
                var used = mc[2 * i - 2].Value;
                var remain = mc[2 * i - 1].Value;
                data[i - 1] = new { used, remain };
            }
            json.Data = data;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        static string Get_Reply(string html)
        {
            string reply = "";

            Regex re = new Regex("<span id=\"lblname\">\\s*(?<name>[^\\s]*)\\s*</span>[\\s\\S]*?<span id=\"lblmsg\">[\\s\\S]*?早操：[\\s\\S]*?>(?<morningexercises>\\d+)<[\\s\\S]*?课外活动1：[\\s\\S]*?>(?<outclassactivite1>\\d+)<[\\s\\S]*?</span>");
            Match match = re.Match(html);
            GroupCollection gc = match.Groups;
            string name = gc["name"].Value;
            string morningexercise = gc["morningexercises"].Value;
            string outclassactivite1 = gc["outclassactivite1"].Value;
            reply = name + "同学，您晨跑次数为：" + morningexercise + ",课外活动次数为：" + outclassactivite1;

            return reply;
        }
        static string Get_Score(string studentnum, string password)
        {
            string html = "";
            CookieCollection cc = new CookieCollection();
            CookieContainer ccon = new CookieContainer();
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(@"http://59.78.92.38:8888/sportscore/");
            hwr.CookieContainer = ccon;
            HttpWebResponse hwro = (HttpWebResponse)hwr.GetResponse();
            cc = hwro.Cookies;

            HttpWebRequest hwr2 = (HttpWebRequest)WebRequest.Create(@"http://59.78.92.38:8888/sportscore/");
            hwr2.AllowAutoRedirect = false;
            hwr2.Method = "POST";
            hwr2.CookieContainer = ccon;
            ccon.Add(hwr2.RequestUri, cc);
            hwr2.ContentType = "application/x-www-form-urlencoded";
            string post = "__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwULLTIxMjk4Nzk4MzUPZBYCZg9kFghmDxBkZBYBZmQCBQ8PFgIeBFRleHRlZGQCBg8PFgIfAAUSMjAxNC85LzE2IDExOjI1OjUyZGQCBw8PFgIfAAUSMjAzNC85LzE2IDExOjI1OjUyZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFBWJ0bm9rg7wsmKWoIGv%2B2o6SEkUuPN0q80w%3D&dlljs=st&" +
                "txtuser=10150111&txtpwd=660602" +
                "&__VIEWSTATEGENERATOR=5B21F7B0&__EVENTVALIDATION=%2FwEWCAKAvaGBDAKBwaG%2FAQLMrvvQDQLd8tGoBALWwdnoCALB2tiCDgKd%2B7q4BwL9kpmqCm3geVz7Uj28fz9x4hImTVbLX%2BiL&btnok.x=0&btnok.y=0";
            byte[] bytes = Encoding.UTF8.GetBytes(post);
            hwr2.ContentLength = bytes.Length;
            Stream stream = hwr2.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();

            HttpWebResponse hwro2 = (HttpWebResponse)hwr2.GetResponse();


            StreamReader sr = new StreamReader(hwro2.GetResponseStream());
            html = sr.ReadToEnd();
            if (Regex.IsMatch(html, "Object moved"))
            {
                HttpWebRequest hwr3 = (HttpWebRequest)WebRequest.Create(@"http://59.78.92.38:8888/sportscore/stScore.aspx");
                hwr3.CookieContainer = ccon;
                ccon.Add(hwr3.RequestUri, cc);
                HttpWebResponse hwro3 = (HttpWebResponse)hwr3.GetResponse();
                sr = new StreamReader(hwro3.GetResponseStream());
                html = sr.ReadToEnd();
                return html;
            }
            else
                throw new Exception("用户名或密码错误！");
        }
    }
}