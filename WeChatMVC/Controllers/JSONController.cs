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
        public JsonResult largetable(string studentnum, string pwd)
        {
            MyHttpHelper a = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
            a.HttpGet();
            MyHttpHelper b = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/Base/VerifyCode.aspx");
            Bitmap input = b.HttpGetImage();
            IdentificatImage id = new IdentificatImage(input);
            string vc = id.result;
            MyHttpHelper c = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
            c.HttpPost("__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE2OTIxNDU0MTMPZBYCAgEPZBYCAgcPDxYCHgRUZXh0BVDlrabnlJ%2FliJ3lp4vlr4bnoIHkuLrouqvku73or4Hlj7flkI7lha3kvY3jgILlr4bnoIHplb%2FluqbkuI3otoXov4cxMOS4quWtl%2BespuOAgmRkZDanEMgmeoYOigCgOHJXPnTdIOtq&TxtStudentid=" + studentnum + "&TxtPassword=" + pwd + "&txt_verifyCode=" + vc + "&BtnLogin=%E7%99%BB%E5%BD%95&__EVENTVALIDATION=%2FwEWBQKMjOWyBAKf8ICgBwLVqbaRCwLW2qK1CALi44eGDA67X3bLsDOxfx3HDe98WpJ8%2Bncw");

            MyHttpHelper d = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_BigScoreTableDetail.aspx?key=0");
            d.HttpGet();
            string html = d.ToString();
            Regex regex = new Regex("<td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\"" +
                " valign=\"middle\"><font face=\"[^\"]*\">(?<kemu>[^<]*)</font></td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font>" +
                "</td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\" valign=\"middle\">" +
                "<font face=\"[^\"]*\">(?<fengshu>\\d+)</font></td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\" " +
                "valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td>"
                );
            JsonResult json = new JsonResult();
            MatchCollection mc = regex.Matches(html);
            var data = new object[mc.Count + 1];

            Regex jidian = new Regex("平均学分绩点:(?<jidian>\\d+(\\.\\d+)?)");
            data[0] = jidian.Match(html).Groups["jidian"].Value;

            for (int i = 1; i <= mc.Count; i++) 
            {
                GroupCollection gc = mc[i - 1].Groups;
                var kemu = gc["kemu"].Value;
                var fengshu = gc["fengshu"].Value;
                data[i] = new { kemu, fengshu };
            }
            json.Data = data;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    }
}