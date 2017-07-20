using System.Text.RegularExpressions;
using WeChatMVC.Models;
using System.Web.Mvc;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace WeChatMVC.Controllers
{
    public class APIController : BaseController
    {
        // GET: API
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
        private const string log_success = "success";
        private const string log_fail_pwd = "pwd";
        private const string log_fail_vc = "vc";
        private const string log_fail_xh = "xh";
        public string jwc_largetable(string studentnum, string pwd)
        {
            string sessionresponse = getjwcsession(studentnum, pwd);
            if (sessionresponse == log_fail_pwd)
            {
                return "密码错误，请回复jwc+密码重新绑定密码，如jwc123456";
            }
            else if (sessionresponse == log_fail_xh)
            {
                return "您输入的学号：" + studentnum + "，长度不正确";
            }
            MyHttpHelper d = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_BigScoreTableDetail.aspx?key=0");
            d.HttpGet();
            string html = d.ToString();
            Regex regex = new Regex("<td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\"" +
                " valign=\"middle\"><font face=\"[^\"]*\">(?<kemu>[^<]*)</font></td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font>" +
                "</td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\" valign=\"middle\">" +
                "<font face=\"[^\"]*\">(?<fengshu>\\d+)</font></td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\" " +
                "valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td>"
                );
            string result = "";
            MatchCollection mc = regex.Matches(html);
            foreach (Match item in mc)
            {
                GroupCollection gc = item.Groups;
                result = result + gc["kemu"].Value + ":" + gc["fengshu"].Value + "\n";
            }
            Regex jidian = new Regex("平均学分绩点:(?<jidian>\\d+(\\.\\d+)?)");
            result = result + "平均绩点："+ jidian.Match(html).Groups["jidian"].Value; ;
            return result;
        }
        public string jwc_smarttable(string studentnum, string pwd)
        {
            if (getjwcsession(studentnum, pwd) == log_fail_pwd)
            {
                return "密码错误，请回复jwc+密码重新绑定密码，如jwc123456";
            }
            MyHttpHelper d = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_ScoreTableYearTerm.aspx?i=0%3a26%3a46");
            d.HttpPost("__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUKLTM1MDcwMDg1MQ9kFgICAQ9kFgICAQ9kFgZmD2QWAmYPZBYCAgMPDxYCHgdFbmFibGVkaGRkAgEPZBYCZg9kFgICAQ8QDxYGHg1EYXRhVGV4dEZpZWxkBQhUZXJtTmFtZR4ORGF0YVZhbHVlRmllbGQFCFllYXJUZXJtHgtfIURhdGFCb3VuZGdkEBUFFeKAlOKAlOivt%2BmAieaLqeKAlOKAlBYyMDE2LTIwMTflrablubQy5a2m5pyfFjIwMTYtMjAxN%2BWtpuW5tDHlrabmnJ8WMjAxNS0yMDE25a2m5bm0MuWtpuacnxYyMDE1LTIwMTblrablubQx5a2m5pyfFQUBMAUyMDE2MgUyMDE2MQUyMDE1MgUyMDE1MRQrAwVnZ2dnZ2RkAgMPZBYCZg9kFgJmDzwrAAsAZGSDV9YWPjkZzs%2BQA3Jxh1jr8S5yVA%3D%3D&ddlYearTerm=20162&btnSelect=%E6%9F%A5%E8%AF%A2&__EVENTVALIDATION=%2FwEWCQLCq5zYDAKC5sFXApf3trkEAtjpwosFAo%2F6pfQIAoD6pfQIAo%2F6iQkCgPqJCQLax9vVBk%2F0%2B3xjQYQIiqbgEfy%2FW8XcekCs");
            string html = d.ToString();
            Regex songti = new Regex("<font face=\"宋体\" color=\"Black\">");
            html = songti.Replace(html, "");
            Regex zhiti = new Regex("</font>");
            html = zhiti.Replace(html, "");
            Regex regex = new Regex(@"<td>(?<kemu>[^<]*)</td><td>[^<]*</td><td>[^<]*</td><td>[^<]*</td><td>[^<]*</td><td>[^<]*</td><td>(?<juanmianfen>\d+(\.\d+)?)[^<]*</td><td>(?<pingshifen>\d+(\.\d+)?)[^<]*</td><td>(<font[^>]*>)?(?<fengshu>\d+(\.\d+)?)[^<]*(</font[^>]*>)?</td><td>\d+(\.\d+)?[^<]*</td><td>\d+(\.\d+)?[^<]*</td>");
            string result = "";
            MatchCollection mc = regex.Matches(html);
            foreach (Match item in mc)
            {
                GroupCollection gc = item.Groups;
                result = result + gc["kemu"].Value + ":" + gc["fengshu"].Value + "\n";
            }
            result = result + "以上";
            return result;
        }
        public string jwc_gradepoint(string studentnum, string pwd)
        {
            string sessionresponse = getjwcsession(studentnum, pwd);
            if (sessionresponse == log_fail_pwd)
            {
                return "密码错误，请回复jwc+密码重新绑定密码，如jwc123456";
            }
            else if (sessionresponse == log_fail_xh)
            {
                return "您输入的学号：" + studentnum + "，长度不正确";
            }
            MyHttpHelper e = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_BigScoreTableDetail.aspx?key=0");
            e.HttpGet();
            string html = e.ToString();
            Regex jidian = new Regex("平均学分绩点:(?<jidian>\\d+(\\.\\d+)?)");
            return jidian.Match(html).Groups["jidian"].Value;
        }
        public string jwc_classtable(string studentnum, string pwd)
        {
            return "今天并没有安排课程，享受你的暑假吧！";
            string sessionresponse = getjwcsession(studentnum, pwd);
            if (sessionresponse == log_fail_pwd)
            {
                return "密码错误，请回复jwc+密码重新绑定密码，如jwc123456";
            }
            else if (sessionresponse == log_fail_xh)
            {
                return "您输入的学号：" + studentnum + "，长度不正确";
            }
            MyHttpHelper d = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/E_SelectCourse/ScInFormation/syllabus.aspx");
            d.HttpPost("__VIEWSTATE=%2FwEPDwUKLTg3NzgzODIwNw9kFgICAQ9kFgICAw8QDxYGHg1EYXRhVGV4dEZpZWxkBQhZZWFyVGVybR4ORGF0YVZhbHVlRmllbGQFAnNtHgtfIURhdGFCb3VuZGdkEBUCBTIwMTcxBTIwMTYyFQIJ5LiL5a2m5pyfCeacrOWtpuacnxQrAwJnZ2RkZNO%2Fri3X13dLfsVR9NFAAfI1ATzP&selyeartermflag=%E4%B8%8B%E5%AD%A6%E6%9C%9F&bttn_search=%E6%9F%A5%E8%AF%A2&__EVENTVALIDATION=%2FwEWBAKX%2B67KDQKukO%2FqDwLJpuDqDwK1man8CYWGxTfqcteijecSaCWqU1U3a0ll");
            string html = d.ToString();
            Regex songti = new Regex("<font face=\"宋体\" color=\"Black\">");
            html = songti.Replace(html, "");
            Regex zhiti = new Regex("</font>");
            html = zhiti.Replace(html, "");
            Regex regex = new Regex("<td[^>]*>(?<class>[^<]*)</td><td[^>]*>\\d+</td><td[^>]*>(?<teacher>[^<]*)</td><td[^>]*><font size=1>(?<date>[^<]*)</td><td [^>]*><font size=1>(?<room>[^<]*)</td><td[^>]*>[^<]*</td><td[^>]*>[^<]*</td><td[^>]*>[^<]*</td>");
            MatchCollection mc = regex.Matches(html);
            foreach (Match m in mc)
            {
                GroupCollection gc = m.Groups;
                string teacher = gc["teacher"].Value;
                string classname = gc["class"].Value;
                string date = gc["date"].Value;
                string room = gc["room"].Value;
                classtableob now = new classtableob(teacher, classname, date, room);
                classtableob.classtable[now.weekcode].Add(now);
            }
            string result = "";
            int nowweekday = (int)DateTime.Now.DayOfWeek;
            foreach (classtableob item in classtableob.classtable[nowweekday])
            {
                result = result + item.classname + ",\n授课教师:" + item.teacher  +"(" + item.room + ")\n";
            }
            classtableob.classtable.Clear();
            if (result == "")
                return "今天并没有安排课程！";
            return result;
        }
        private string getjwcsession(string studentnum, string pwd)
        {
            if (studentnum.Length != 8)
            {
                return log_fail_xh;
            }
            MyHttpHelper a = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
            a.HttpGet();
            MyHttpHelper b = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/Base/VerifyCode.aspx");
            Bitmap input = b.HttpGetImage();
            IdentificatImage id = new IdentificatImage(input);
            string vc = id.result;
            MyHttpHelper c = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
            c.HttpPost("__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE2OTIxNDU0MTMPZBYCAgEPZBYCAgcPDxYCHgRUZXh0BVDlrabnlJ%2FliJ3lp4vlr4bnoIHkuLrouqvku73or4Hlj7flkI7lha3kvY3jgILlr4bnoIHplb%2FluqbkuI3otoXov4cxMOS4quWtl%2BespuOAgmRkZDanEMgmeoYOigCgOHJXPnTdIOtq&TxtStudentid=" + studentnum + "&TxtPassword=" + pwd + "&txt_verifyCode=" + vc + "&BtnLogin=%E7%99%BB%E5%BD%95&__EVENTVALIDATION=%2FwEWBQKMjOWyBAKf8ICgBwLVqbaRCwLW2qK1CALi44eGDA67X3bLsDOxfx3HDe98WpJ8%2Bncw");
            string html = c.ToString();
            if (MyHttpHelper.regexsuccess.IsMatch(html))
            {
                return log_success;
            }
            else if (MyHttpHelper.regexpwdfail.IsMatch(html))
            {
                return log_fail_pwd;
            }
            else if (MyHttpHelper.regexvcfail.IsMatch(html))
            {
                return getjwcsession(studentnum, pwd);
            }
            return "unknownfail";
        }
    }
    class classtableob
    {
        public static List<List<classtableob>> classtable = new List<List<classtableob>>();
        public string teacher = "";
        public string classname = "";
        public int timebegin = 0;
        public int timeend = 11;
        public int datebegin = 0;
        public int dateend = 20;
        public string weekday = "";
        public int weekcode = 0;
        public string room = "";
        public classtableob(string thisteacher, string thisclassname, string thisdate, string thisroom)
        {
            if (classtable.Count == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    List<classtableob> list = new List<classtableob>();
                    classtable.Add(list);
                }
            }
            teacher = thisteacher;
            classname = thisclassname;
            room = thisroom;
            Regex regex = new Regex("(?<weekday>[^\\s]*)\\s+(?<timebegin>\\d+)-(?<timeend>\\d+)节\\s+(?<datebegin>\\d+)-(?<dateend>\\d+)(?<quanorsuang>[.]*)");
            GroupCollection gc = regex.Match(thisdate).Groups;
            timebegin = Convert.ToInt32(gc["timebegin"].Value);
            timeend = Convert.ToInt32(gc["timeend"].Value);
            datebegin = Convert.ToInt32(gc["datebegin"].Value);
            dateend = Convert.ToInt32(gc["dateend"].Value);
            weekday = gc["weekday"].Value;
            switch (weekday)
            {
                case "周一":
                    weekcode = 1;
                    break;
                case "周二":
                    weekcode = 2;
                    break;
                case "周三":
                    weekcode = 3;
                    break;
                case "周四":
                    weekcode = 4;
                    break;
                case "周五":
                    weekcode = 5;
                    break;
                default:
                    break;
            }
        }
    }
}