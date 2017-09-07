using System.Net;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using WeChatMVC.Models;
using System.Web.Mvc;

namespace WeChatMVC.Models
{
    public class JWCHttpHelper:MyHttpHelper
    {
        public static Regex regexsuccess = new Regex("您好！");
        public static Regex regexpwdfail = new Regex("密码错误");
        public static Regex regexvcfail = new Regex("验证码不正确！");
        public static bool IsLogin = false;
        public static string ErrorMsg = "";
        public delegate object CrawlerDetail(bool isjson = false);
        public static CrawlerDetail largetable = new CrawlerDetail(jwc_largetable);
        public static CrawlerDetail smalltable = new CrawlerDetail(jwc_smarttable);
        public static CrawlerDetail gradepoint = new CrawlerDetail(jwc_gradepoint);
        public static CrawlerDetail classtable = new CrawlerDetail(jwc_classtable);
        public static CrawlerDetail examtable = new CrawlerDetail(jwc_examtable);
        private const string log_success = "success";
        private const string log_fail_pwd = "pwd";
        private const string log_fail_vc = "vc";
        private const string log_fail_xh = "xh";
        public static object jwc_largetable(bool isjson = false)
        {
            JWCHttpHelper d = new JWCHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_BigScoreTableDetail.aspx?key=0");
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
            if (!isjson)
            {
                foreach (Match item in mc)
                {
                    GroupCollection gc = item.Groups;
                    result = result + gc["kemu"].Value + ":" + gc["fengshu"].Value + "\n";
                }
                Regex jidian = new Regex("平均学分绩点:(?<jidian>\\d+(\\.\\d+)?)");
                result = result + "平均绩点：" + jidian.Match(html).Groups["jidian"].Value;
                return result;
            }
            else
            {
                Regex jidian = new Regex("平均学分绩点:(?<jidian>\\d+(\\.\\d+)?)");
                JsonResult json = new JsonResult();
                var data = new object[mc.Count + 1];
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
        public static object jwc_smarttable(bool isjson = false)
        {
            JWCHttpHelper d = new JWCHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_ScoreTableYearTerm.aspx?i=0%3a26%3a46");
            d.HttpPost("__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUKLTM1MDcwMDg1MQ9kFgICAQ9kFgICAQ9kFgZmD2QWAmYPZBYCAgMPDxYCHgdFbmFibGVkaGRkAgEPZBYCZg9kFgICAQ8QDxYGHg1EYXRhVGV4dEZpZWxkBQhUZXJtTmFtZR4ORGF0YVZhbHVlRmllbGQFCFllYXJUZXJtHgtfIURhdGFCb3VuZGdkEBUFFeKAlOKAlOivt%2BmAieaLqeKAlOKAlBYyMDE2LTIwMTflrablubQy5a2m5pyfFjIwMTYtMjAxN%2BWtpuW5tDHlrabmnJ8WMjAxNS0yMDE25a2m5bm0MuWtpuacnxYyMDE1LTIwMTblrablubQx5a2m5pyfFQUBMAUyMDE2MgUyMDE2MQUyMDE1MgUyMDE1MRQrAwVnZ2dnZ2RkAgMPZBYCZg9kFgJmDzwrAAsAZGSDV9YWPjkZzs%2BQA3Jxh1jr8S5yVA%3D%3D&ddlYearTerm=20162&btnSelect=%E6%9F%A5%E8%AF%A2&__EVENTVALIDATION=%2FwEWCQLCq5zYDAKC5sFXApf3trkEAtjpwosFAo%2F6pfQIAoD6pfQIAo%2F6iQkCgPqJCQLax9vVBk%2F0%2B3xjQYQIiqbgEfy%2FW8XcekCs");
            string html = d.ToString();
            Regex songti = new Regex("<font face=\"宋体\" color=\"Black\">");
            html = songti.Replace(html, "");
            Regex zhiti = new Regex("</font>");
            html = zhiti.Replace(html, "");
            Regex regex = new Regex(@"<td>(?<kemu>[^<]*)</td><td>[^<]*</td><td>[^<]*</td><td>[^<]*</td><td>[^<]*</td><td>[^<]*</td><td>(?<juanmianfen>\d+(\.\d+)?)[^<]*</td><td>(?<pingshifen>\d+(\.\d+)?)[^<]*</td><td>(<font[^>]*>)?(?<fengshu>\d+(\.\d+)?)[^<]*(</font[^>]*>)?</td><td>\d+(\.\d+)?[^<]*</td><td>\d+(\.\d+)?[^<]*</td>");
            string result = "";
            MatchCollection mc = regex.Matches(html);
            if (!isjson)
            {
                foreach (Match item in mc)
                {
                    GroupCollection gc = item.Groups;
                    result = result + gc["kemu"].Value + ":" + gc["fengshu"].Value + "\n";
                }
                result = result + "以上为本学期成绩";
                return result;
            }
            else
            {
                JsonResult json = new JsonResult();
                var data = new object[mc.Count];
                for (int i = 0; i < mc.Count; i++)
                {
                    GroupCollection gc = mc[i].Groups;
                    var kemu = gc["kemu"].Value;
                    var fengshu = gc["fengshu"].Value;
                    data[i] = new { kemu, fengshu };
                }
                json.Data = data;
                json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return json;
            }
        }
        public static object jwc_gradepoint(bool isjson = false)
        {
            JWCHttpHelper e = new JWCHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_BigScoreTableDetail.aspx?key=0");
            e.HttpGet();
            string html = e.ToString();
            Regex jidian = new Regex("平均学分绩点:(?<jidian>\\d+(\\.\\d+)?)");
            if (!isjson)
                return jidian.Match(html).Groups["jidian"].Value;
            else
            {
                JsonResult json = new JsonResult();
                return json.ToString();
            }
        }
        public static object jwc_classtable(bool isjson = false)
        {
            // 功能待启用
            JWCHttpHelper d = new JWCHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/E_SelectCourse/ScInFormation/syllabus.aspx");
            d.HttpPost("__VIEWSTATE=%2FwEPDwUKLTg3NzgzODIwNw9kFgICAQ9kFgICAw8QDxYGHg1EYXRhVGV4dEZpZWxkBQhZZWFyVGVybR4ORGF0YVZhbHVlRmllbGQFAnNtHgtfIURhdGFCb3VuZGdkEBUCBTIwMTcxBTIwMTYyFQIJ5LiL5a2m5pyfCeacrOWtpuacnxQrAwJnZ2RkZNO%2Fri3X13dLfsVR9NFAAfI1ATzP&selyeartermflag=%E4%B8%8B%E5%AD%A6%E6%9C%9F&bttn_search=%E6%9F%A5%E8%AF%A2&__EVENTVALIDATION=%2FwEWBAKX%2B67KDQKukO%2FqDwLJpuDqDwK1man8CYWGxTfqcteijecSaCWqU1U3a0ll");
            string html = d.ToString();
            Regex songti = new Regex("<font face=\"宋体\" color=\"Black\">");
            html = songti.Replace(html, "");
            Regex zhiti = new Regex("</font>");
            html = zhiti.Replace(html, "");
            ClassTableob classtable = new ClassTableob(html, new DateTime(2017, 9, 11));
            //启用时日期改为当前时间
            if (!isjson)
            {
                return "今天并没有安排课程，享受你的暑假吧！";
                return classtable.GetStringToday(new DateTime(2017, 9, 18));
            }
            else
            {
                return classtable.GetJson();
            }
        }
        public static object jwc_examtable(bool isjson = false)
        {
            JWCHttpHelper d = new JWCHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_TestTableDetail.aspx?key=0");
            d.HttpPost("__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwULLTE2NTU5MjUyNDUPZBYCAgEPZBYCAgEPZBYIAgEPZBYCZg9kFgQCAQ8QZBAVDQnor7fpgInmi6kFMjAxNzEFMjAxNjIFMjAxNjEFMjAxNTIFMjAxNTEFMjAxNDIFMjAxNDEFMjAxMzIFMjAxMzEFMjAxMjIFMjAxMjEFMjAxMTIVDQnor7fpgInmi6kFMjAxNzEFMjAxNjIFMjAxNjEFMjAxNTIFMjAxNTEFMjAxNDIFMjAxNDEFMjAxMzIFMjAxMzEFMjAxMjIFMjAxMjEFMjAxMTIUKwMNZ2dnZ2dnZ2dnZ2dnZ2RkAggPEGRkFgFmZAICD2QWAmYPZBYCZg8PFgIeBFRleHQFMTIwMTctMjAxOOWtpuW5tOesrDHlrabmnJ%2FnmoTogIPor5XooajkuI3lrZjlnKjvvIFkZAIDD2QWAmYPZBYGZg8PFgIfAGVkZAICDw8WAh8AZWRkAgQPDxYCHwBlZGQCBA9kFgJmD2QWAmYPPCsACwEADxYCHgdWaXNpYmxlaGRkZC74a7y14FQ9u95U4X%2BZFk%2BC6jss&ddlYearTerm=20162&btnSelect=%E6%9F%A5%E8%AF%A2&RdbCourse=%E4%B8%AA%E4%BA%BA%E8%80%83%E8%AF%95%E8%A1%A8&__EVENTVALIDATION=%2FwEWEgLmo53ZCgLekp65DQKA%2BtHTAQKP%2BqX0CAKA%2BqX0CAKP%2BokJAoD6iQkCj%2FqdogsCgPqdogsCj%2FrhxgICgPrhxgICj%2Fr1mwoCgPr1mwoCj%2FrZvAUC2sfb1QYCuaHTqAgCj%2FnpnQ4CwZTn4whWHkuO6LHUmnWxc9LhgAqJGND3xA%3D%3D");
            string html = d.ToString();
            Regex songti = new Regex("<font face=\"宋体\" color=\"Black\">");
            html = songti.Replace(html, "");
            Regex zhiti = new Regex("</font>");
            html = zhiti.Replace(html, "");
            //缺个正则
            return html;
        }
        public JWCHttpHelper(string url):
            base(url)
        {

        }
        private Bitmap HttpGetImage()
        {
            response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            Bitmap bitmap = (Bitmap)Image.FromStream(stream);
            return bitmap;
        }
        public static void Login(string studentnum, string pwd)
        {
            if (studentnum.Length != 8)
            {
                ErrorMsg = "学号位数不对";
                return;
            }
            if (Regex.IsMatch(pwd, "^[0-9]*$"))
            {
                ErrorMsg = "您的密码过于简单，请登录教务处信息网更改后再绑定";
                return;
            }
            JWCHttpHelper a = new JWCHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
            a.HttpGet();
            JWCHttpHelper b = new JWCHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/Base/VerifyCode.aspx");
            Bitmap input = b.HttpGetImage();
            IdentificatImage id = new IdentificatImage(input);
            string vc = id.result;
            JWCHttpHelper c = new JWCHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
            c.HttpPost("__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE2OTIxNDU0MTMPZBYCAgEPZBYCAgcPDxYCHgRUZXh0BVDlrabnlJ%2FliJ3lp4vlr4bnoIHkuLrouqvku73or4Hlj7flkI7lha3kvY3jgILlr4bnoIHplb%2FluqbkuI3otoXov4cxMOS4quWtl%2BespuOAgmRkZDanEMgmeoYOigCgOHJXPnTdIOtq&TxtStudentid=" + studentnum + "&TxtPassword=" + pwd + "&txt_verifyCode=" + vc + "&BtnLogin=%E7%99%BB%E5%BD%95&__EVENTVALIDATION=%2FwEWBQKMjOWyBAKf8ICgBwLVqbaRCwLW2qK1CALi44eGDA67X3bLsDOxfx3HDe98WpJ8%2Bncw");
            string html = c.ToString();
            if (JWCHttpHelper.regexsuccess.IsMatch(html))
            {
                IsLogin = true;
                return ;
            }
            else if (JWCHttpHelper.regexpwdfail.IsMatch(html))
            {
                ErrorMsg = "您现在设置的教务处密码：" + pwd + "，不正确。请重新输入jwc+您的教务处密码来解锁此功能，如jwc123456";
                return;
            }
            else if (JWCHttpHelper.regexvcfail.IsMatch(html))
            {
                Login(studentnum, pwd);
            }
            ErrorMsg = "unknownfail";
            return;
        }
    }
    class Letter
    {
        public static string[] dic = { "0", "2", "4", "6", "8", "b", "d", "f", "h", "j", "n", "p", "r", "t", "v", "x", "z", "L" };
        public static List<int[,]> map = new List<int[,]>();
        public static int Length { get { return dic.Length; } }
        public static void InitLetter()
        {
            int[,] l0 ={{0,0,1,1,1,1,1,1,1,1,0,0}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{1,1,1,0,0,0,0,0,0,1,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,1,0,0,0,0,0,0,1,1,1}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{0,0,1,1,1,1,1,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] l2 ={{0,0,1,1,0,0,0,0,0,0,1,1}
,{0,1,1,1,0,0,0,0,0,1,1,1}
,{1,1,1,0,0,0,0,0,1,1,1,1}
,{1,1,0,0,0,0,0,1,1,0,1,1}
,{1,1,0,0,0,0,1,1,1,0,1,1}
,{1,1,0,0,0,1,1,1,0,0,1,1}
,{0,1,1,1,1,1,1,0,0,0,1,1}
,{0,0,1,1,1,0,0,0,0,0,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] l4 ={{0,0,0,0,0,0,0,1,1,1,0,0}
,{0,0,0,0,0,0,1,1,1,1,0,0}
,{0,0,0,0,1,1,1,0,1,1,0,0}
,{0,0,0,1,1,1,0,0,1,1,0,0}
,{0,1,1,1,0,0,0,0,1,1,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] l6 ={{0,0,0,1,1,1,1,1,1,0,0,0}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{1,1,1,0,0,1,1,0,0,1,1,1}
,{1,1,0,0,1,1,0,0,0,0,1,1}
,{1,1,0,0,1,1,1,0,0,0,1,1}
,{1,1,0,0,1,1,1,0,0,0,1,1}
,{1,1,1,0,0,1,1,1,1,1,1,0}
,{0,1,1,0,0,0,1,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] l8 ={{0,0,1,1,1,1,0,1,1,1,0,0}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{0,0,1,1,1,1,0,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lb ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,0,0,0,1,1,0,0,0,1,1}
,{1,1,1,1,1,1,1,1,0,0,1,1}
,{0,1,1,1,1,0,1,1,1,1,1,0}
,{0,0,0,0,0,0,0,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] ld ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{0,1,1,0,0,0,0,0,0,1,1,0}
,{0,1,1,1,1,1,1,1,1,1,1,0}
,{0,0,0,1,1,1,1,1,1,0,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lf ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lh ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lj ={{0,0,0,0,0,0,0,0,1,1,0,0}
,{0,0,0,0,0,0,0,0,1,1,1,0}
,{0,0,0,0,0,0,0,0,0,1,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,0}
,{1,1,1,1,1,1,1,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] ln ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,1,1,1,0,0,0,0,0,0,0,0}
,{0,0,1,1,1,0,0,0,0,0,0,0}
,{0,0,0,0,1,1,1,0,0,0,0,0}
,{0,0,0,0,0,1,1,1,0,0,0,0}
,{0,0,0,0,0,0,0,1,1,1,0,0}
,{0,0,0,0,0,0,0,0,1,1,1,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lp ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,1,0,1,1,1,0,0,0,0,0}
,{0,1,1,1,1,1,0,0,0,0,0,0}
,{0,0,1,1,1,0,0,0,0,0,0,0}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lr ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,0,0,0,0,0}
,{1,1,0,0,0,1,1,1,0,0,0,0}
,{1,1,0,0,0,1,1,1,1,0,0,0}
,{1,1,1,1,1,1,0,1,1,1,1,0}
,{0,1,1,1,1,1,0,0,1,1,1,1}
,{0,0,1,1,1,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lt ={{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lv ={{1,1,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,0,0,0,0,0,0,0,0}
,{0,0,1,1,1,1,1,0,0,0,0,0}
,{0,0,0,0,1,1,1,1,1,0,0,0}
,{0,0,0,0,0,0,0,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,0,1,1,1}
,{0,0,0,0,0,0,0,1,1,1,1,1}
,{0,0,0,0,1,1,1,1,1,0,0,0}
,{0,0,1,1,1,1,1,0,0,0,0,0}
,{1,1,1,1,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lx ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{1,1,1,0,0,0,0,0,0,1,1,1}
,{0,1,1,1,1,0,0,1,1,1,1,0}
,{0,0,0,1,1,1,1,1,1,0,0,0}
,{0,0,0,0,1,1,1,1,0,0,0,0}
,{0,0,0,1,1,1,1,1,1,0,0,0}
,{0,1,1,1,1,0,0,1,1,1,1,0}
,{1,1,1,0,0,0,0,0,0,1,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            int[,] lz ={{0,0,0,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,1,1,1}
,{1,1,0,0,0,0,0,1,1,1,1,1}
,{1,1,0,0,0,0,1,1,1,0,1,1}
,{1,1,0,0,1,1,1,1,0,0,1,1}
,{1,1,0,1,1,1,0,0,0,0,1,1}
,{1,1,1,1,1,0,0,0,0,0,1,1}
,{1,1,1,0,0,0,0,0,0,0,1,1}
,{1,1,0,0,0,0,0,0,0,0,1,1}
};
            int[,] lL ={{0,0,0,0,0,0,0,0,0,0,0,0}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{1,1,1,1,1,1,1,1,1,1,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,1,1}
,{0,0,0,0,0,0,0,0,0,0,0,0}
};
            map.Add(l0);
            map.Add(l2);
            map.Add(l4);
            map.Add(l6);
            map.Add(l8);
            map.Add(lb);
            map.Add(ld);
            map.Add(lf);
            map.Add(lh);
            map.Add(lj);
            map.Add(ln);
            map.Add(lp);
            map.Add(lr);
            map.Add(lt);
            map.Add(lv);
            map.Add(lx);
            map.Add(lz);
            map.Add(lL);
        }
        private Letter()
        {

        }
        public static int getwidth(int index)
        {
            return map[index].GetLength(0);
        }
        public static int getheight(int index)
        {
            return map[index].GetLength(1);
        }
        public static int getlength()
        {
            return dic.Length;
        }
    }
    class IdentificatImage
    {
        public Bitmap input;
        public string result;
        public int[,] map;
        public IdentificatImage(Bitmap thisinput)
        {
            if (Letter.map.Count == 0)
            {
                Letter.InitLetter();
            }
            Bitmap noindeximg = thisinput.Clone(
                new Rectangle(0, 0, thisinput.Width, thisinput.Height),
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            input = noindeximg;
            result = identimage();
        }
        private string identimage()
        {
            string result = "";
            map = new int[50, 17];
            binaryzation();
            howmanyqi(5, 5, map);
            int[] score = new int[Letter.Length];
            judger(5, ref score, map);
            int max = -99990; int index = 0;
            for (int k = 0; k < score.Length; k++)
            {
                if (score[k] > max)
                {
                    index = k; max = score[k];
                }
            }
            result = result + Letter.dic[index];
            int now = 5 + Letter.map[index].GetLength(0);
            judger(now, ref score, map);
            max = -99990; index = 0;
            for (int k = 0; k < score.Length; k++)
            {
                if (score[k] > max)
                {
                    index = k; max = score[k];
                }
            }
            result = result + Letter.dic[index];
            now = now + Letter.getwidth(index);
            judger(now, ref score, map);
            max = -99990; index = 0;
            for (int k = 0; k < score.Length; k++)
            {
                if (score[k] > max)
                {
                    index = k; max = score[k];
                }
            }
            result = result + Letter.dic[index];
            now = now + Letter.getwidth(index);
            judger(now, ref score, map);
            max = -99990; index = 0;
            for (int k = 0; k < score.Length; k++)
            {
                if (score[k] > max)
                {
                    index = k; max = score[k];
                }
            }
            result = result + Letter.dic[index];
            return result;
        }
        private void howmanyqi(int x, int y, int[,] map)
        {
            if (x >= 49)
            {
                howmanyqi(5, y + 1, map);
                return;
            }
            if (y >= 17)
            {
                return;
            }
            if (map[x, y] == 0 && getgraylevel(input.GetPixel(x, y)) == 0)
            {
                map[x, y] = 1;
                int now = 0;
                turntoblack(x, y, ref now, map);
                if (now <= 4)
                {
                    clear(x, y, map);
                }
            }
            howmanyqi(x + 1, y, map);
        }
        private void turntoblack(int x, int y, ref int now, int[,] map)
        {
            if (x >= 49 || x <= 4)
            {
                return;
            }
            if (y >= 17 || y <= 4)
            {
                return;
            }
            if (map[x, y] == 2)
            {
                return;
            }
            if (getgraylevel(input.GetPixel(x, y)) == 0)
            {
                now++;
                map[x, y] = 2;
                turntoblack(x + 1, y, ref now, map);
                turntoblack(x - 1, y, ref now, map);
                turntoblack(x, y + 1, ref now, map);
                turntoblack(x, y - 1, ref now, map);
            }
        }
        private void clear(int x, int y, int[,] map)
        {
            if (x >= 49 || x <= 4)
            {
                return;
            }
            if (y >= 17 || y <= 4)
            {
                return;
            }
            if (getgraylevel(input.GetPixel(x, y)) == 0)
            {
                map[x, y] = 0;
                Color newColor = Color.FromArgb(255, 255, 255);
                input.SetPixel(x, y, newColor);
                clear(x + 1, y, map);
                clear(x - 1, y, map);
                clear(x, y + 1, map);
                clear(x, y - 1, map);
            }
        }
        private void binaryzation()
        {
            for (int x = 5; x < map.GetLength(0); x++)
            {
                for (int y = 5; y < 17; y++)
                {
                    Color pixelColor = input.GetPixel(x, y);
                    if (getgraylevel(pixelColor) == 0)
                    {
                        Color newColor = Color.FromArgb(255, 255, 255);
                        input.SetPixel(x, y, newColor);
                    }
                    else if (getgraylevel(pixelColor) <= 764)
                    {
                        Color newColor = Color.FromArgb(0, 0, 0);
                        input.SetPixel(x, y, newColor);
                    }
                    else
                    {
                        Color newColor = Color.FromArgb(255, 255, 255);
                        input.SetPixel(x, y, newColor);
                    }
                }
            }
        }
        private static int getgraylevel(Color input)
        {
            return input.R + input.G + input.B;
        }
        private static void judger(int xbegin, ref int[] score, int[,] imagemap)
        {
            score = new int[Letter.Length];
            for (int i = 0; i < Letter.Length; i++)
            {
                for (int x = xbegin; x < xbegin + Letter.getwidth(i); x++)
                {
                    if (x >= 50)
                    {
                        break;
                    }
                    for (int y = 0; y < Letter.getheight(i); y++)
                    {
                        if (imagemap[x, y + 5] == 2 && Letter.map[i][x - xbegin, y] == 1)
                        {
                            score[i]++;
                        }
                        else if (imagemap[x, y + 5] == 0 && Letter.map[i][x - xbegin, y] == 0)
                        {

                        }
                        else if (imagemap[x, y + 5] == 2 && Letter.map[i][x - xbegin, y] == 0)
                        {
                            score[i] -= 10;
                        }
                        else
                        {
                            score[i] -= 10;
                        }
                    }
                }
            }
        }
    }
    public struct ClassTableob
    {
        public List<List<Classob>> ClassTable;
        public DateTime TermBegin;
        public ClassTableob(string html,DateTime begin)
        {
            ClassTable = new List<List<Classob>>();
            TermBegin = begin;
            if (ClassTable.Count == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    List<Classob> list = new List<Classob>();
                    ClassTable.Add(list);
                }
            }
            Regex regex = new Regex("<td[^>]*>(?<class>[^<]*)</td><td[^>]*>\\d+</td><td[^>]*>(?<teacher>[^<]*)</td><td[^>]*><font size=1>(?<date>[^<]*)</td><td [^>]*><font size=1>(?<room>[^<]*)</td><td[^>]*>[^<]*</td><td[^>]*>[^<]*</td><td[^>]*>[^<]*</td>");
            MatchCollection mc = regex.Matches(html);
            foreach (Match m in mc)
            {
                GroupCollection gc = m.Groups;
                string teacher = gc["teacher"].Value;
                string classname = gc["class"].Value;
                string date = gc["date"].Value;
                string room = gc["room"].Value;
                Classob now = new Classob(teacher, classname, date, room);
                ClassTable[now.weekcode].Add(now);
            }
            sort();
        }
        private void sort()
        {
            foreach (List<Classob> list in ClassTable)
            {
                if (list.Count <= 1)
                    continue;
                sortlist(list, 0, list.Count - 1);
            }
        }
        private void sortlist(List<Classob> list, int begin, int end)
        {
            Classob temp;
            if (end <= begin)
                return;
            int a = begin; int b = end;
            while (b != a)
            {
                while (Classob.isAearlier(list[a], list[b]) && b != a)
                {
                    a++;
                }
                temp = list[a];
                list[a] = list[b];
                list[b] = temp;
                while (Classob.isAearlier(list[a], list[b]) && b != a)
                {
                    b--;
                }
                temp = list[a];
                list[a] = list[b];
                list[b] = temp;
            }
            sortlist(list, begin, a - 1);
            sortlist(list, b + 1, end);
        }
        private int getweeknum(DateTime today)
        {
            int days = (today - this.TermBegin).Days;
            return days / 7 + 1;
        }
        private static int getweekcode(DateTime today)
        {
            return (int)today.DayOfWeek;
        }
        public JsonResult GetJson()
        {
            JsonResult json = new JsonResult();
            json.Data = this;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
        public List<Classob> GetClassToday(DateTime today)
        {
            List<Classob> result = new List<Classob>();
            int weekcode = getweekcode(today);
            int weeknum = getweeknum(today);
            foreach (Classob clas in ClassTable[weekcode])
            {
                if (clas.isToday(weeknum))
                    result.Add(clas);
            }
            return result;
        }
        public string GetStringToday(DateTime today)
        {
            string result = "";
            List<Classob> todayclasses = GetClassToday(today);
            for (int i = 0; i < todayclasses.Count; i++)
            {
                //最终呈现给用户的字符处理好看一点
                result += todayclasses[i].classname;
            }
            if (result == "")
                return "今天并没有安排课程！";
            return result;
        }
    }
    public struct Classob
    {
        public string teacher;
        public string classname;
        public int timebegin;
        public int timeend;
        public int datebegin;
        public int dateend;
        public string weekday;
        public int weekcode;
        public string room ;
        public string quanorsuang;
        public bool isshuang;
        public bool isdan;
        public Classob(string thisteacher, string thisclassname, string thisdate, string thisroom)
        {
            isdan = true;isshuang = true;
            teacher = thisteacher;
            classname = thisclassname;
            room = thisroom;
            Regex regex = new Regex("(?<weekday>[^\\s]*)\\s+(?<timebegin>\\d+)-(?<timeend>\\d+)节\\s+(?<datebegin>\\d+)-(?<dateend>\\d+)(?<quanorshuang>.*)$");
            GroupCollection gc = regex.Match(thisdate).Groups;
            timebegin = Convert.ToInt32(gc["timebegin"].Value);
            timeend = Convert.ToInt32(gc["timeend"].Value);
            datebegin = Convert.ToInt32(gc["datebegin"].Value);
            dateend = Convert.ToInt32(gc["dateend"].Value);
            weekday = gc["weekday"].Value;
            quanorsuang = gc["quanorshuang"].Value;
            switch (quanorsuang)
            {
                case "双周":
                    isdan = false;
                    break;
                case "单周":
                    isshuang = false;
                    break;
                default:
                    break;
            }
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
                case "周六":
                    weekcode = 6;
                    break;
                default:
                    weekcode = 0;
                    break;
            }
        }
        public static bool isAearlier(Classob a, Classob b)
        {
            return (a.timebegin <= b.timebegin) ? true : false;
        }
        public bool isToday(int weeknum)
        {
            if (weeknum % 2 == 1 && isdan)
            {
                return true;
            }
            else if (weeknum % 2 == 0 && isshuang)
            {
                return true;
            }
            return false;
        }
    }
}