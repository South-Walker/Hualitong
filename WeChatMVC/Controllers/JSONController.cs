using System.Text.RegularExpressions;
using WeChatMVC.Models;
using System.Web.Mvc;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace WeChatMVC.Controllers
{
    public class JSONController : Controller
    {
        // GET: JSON
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ty(string studentnum, string pwd)
        {
            string post = string.Format("__EVENTTARGET = &__EVENTARGUMENT = &__LASTFOCUS = &__VIEWSTATE =% 2FwEPDwULLTIxMjk4Nzk4MzUPZBYCZg9kFghmDxBkZBYBZmQCBQ8PFgIeBFRleHRlZGQCBg8PFgIfAAUSMjAxNC85LzE2IDExOjI1OjUyZGQCBw8PFgIfAAUSMjAzNC85LzE2IDExOjI1OjUyZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFBWJ0bm9rg7wsmKWoIGv % 2B2o6SEkUuPN0q80w % 3D & dlljs = st & " +
                "txtuser={0}&txtpwd={1}&__VIEWSTATEGENERATOR=5B21F7B0&__EVENTVALIDATION=%2FwEWCAKAvaGBDAKBwaG%2FAQLMrvvQDQLd8tGoBALWwdnoCALB2tiCDgKd%2B7q4BwL9kpmqCm3geVz7Uj28fz9x4hImTVbLX%2BiL&btnok.x=0&btnok.y=0",
                studentnum, pwd);

            MyHttpHelper httphelper = new MyHttpHelper(@"http://59.78.92.38:8888/sportscore/");
            httphelper.HttpGet();
            httphelper = new MyHttpHelper(@"http://59.78.92.38:8888/sportscore/");
            httphelper.HttpPost(post);
            httphelper = new MyHttpHelper(@"http://59.78.92.38:8888/sportscore/stScore.aspx");
            httphelper.HttpGet();

            Regex re = new Regex("<span id=\"lblname\">\\s*(?<name>[^\\s]*)\\s*</span>[\\s\\S]*?<span id=\"lblmsg\">[\\s\\S]*?早操：[\\s\\S]*?>(?<morningexercises>\\d+)<[\\s\\S]*?课外活动1：[\\s\\S]*?>(?<outclassactivite1>\\d+)<[\\s\\S]*?</span>");
            Match match = re.Match(httphelper.ToString());
            GroupCollection gc = match.Groups;
            string name = gc["name"].Value;
            string morningexercise = gc["morningexercises"].Value;
            string outclassactivite1 = gc["outclassactivite1"].Value;
            string reply = name + "同学，您晨跑次数为：" + morningexercise + ",课外活动次数为：" + outclassactivite1;


            JsonResult json = new JsonResult();
            var data = new object[3];

            data[0] = name;
            data[1] = morningexercise;
            data[2] = outclassactivite1;
            json.Data = data;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
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
        public JsonResult test()
        {
            JWCHttpHelper.Login("10150111", "***ak96101");
                return (JsonResult)JWCHttpHelper.largetable(true);

        }
    }
}