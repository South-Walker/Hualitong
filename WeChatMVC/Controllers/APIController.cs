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
        public delegate string CrawlerDetail();
        public static CrawlerDetail largetable = new CrawlerDetail(jwc_largetable);
        public static CrawlerDetail smalltable = new CrawlerDetail(jwc_smarttable);
        public static CrawlerDetail gradepoint = new CrawlerDetail(jwc_gradepoint);
        public static CrawlerDetail classtable = new CrawlerDetail(jwc_classtable);
        public static CrawlerDetail examtable = new CrawlerDetail(jwc_examtable);
        private const string log_success = "success";
        private const string log_fail_pwd = "pwd";
        private const string log_fail_vc = "vc";
        private const string log_fail_xh = "xh";
        public static string jwc_largetable()
        {
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
            result = result + "平均绩点："+ jidian.Match(html).Groups["jidian"].Value;
            return result;
        }
        public static string jwc_smarttable()
        {
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
            result = result + "以上为本学期成绩";
            return result;
        }
        public static string jwc_gradepoint()
        {
            MyHttpHelper e = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_BigScoreTableDetail.aspx?key=0");
            e.HttpGet();
            string html = e.ToString();
            Regex jidian = new Regex("平均学分绩点:(?<jidian>\\d+(\\.\\d+)?)");
            return jidian.Match(html).Groups["jidian"].Value;
        }
        public static string jwc_classtable()
        {
            // 功能待启用
            return "今天并没有安排课程，享受你的暑假吧！";
            MyHttpHelper d = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/E_SelectCourse/ScInFormation/syllabus.aspx");
            d.HttpPost("__VIEWSTATE=%2FwEPDwUKLTg3NzgzODIwNw9kFgICAQ9kFgICAw8QDxYGHg1EYXRhVGV4dEZpZWxkBQhZZWFyVGVybR4ORGF0YVZhbHVlRmllbGQFAnNtHgtfIURhdGFCb3VuZGdkEBUCBTIwMTcxBTIwMTYyFQIJ5LiL5a2m5pyfCeacrOWtpuacnxQrAwJnZ2RkZNO%2Fri3X13dLfsVR9NFAAfI1ATzP&selyeartermflag=%E4%B8%8B%E5%AD%A6%E6%9C%9F&bttn_search=%E6%9F%A5%E8%AF%A2&__EVENTVALIDATION=%2FwEWBAKX%2B67KDQKukO%2FqDwLJpuDqDwK1man8CYWGxTfqcteijecSaCWqU1U3a0ll");
            string html = d.ToString();
            Regex songti = new Regex("<font face=\"宋体\" color=\"Black\">");
            html = songti.Replace(html, "");
            Regex zhiti = new Regex("</font>");
            html = zhiti.Replace(html, "");
            ClassTableob classtable = new ClassTableob(html);
            //启用时日期改为当前时间
            return classtable.GetStringToday(new DateTime(2017, 9, 18));
        }
        public static string jwc_examtable()
        {
            MyHttpHelper d = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_TestTableDetail.aspx?key=0");
            d.HttpPost("__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwULLTE2NTU5MjUyNDUPZBYCAgEPZBYCAgEPZBYIAgEPZBYCZg9kFgQCAQ8QZBAVDQnor7fpgInmi6kFMjAxNzEFMjAxNjIFMjAxNjEFMjAxNTIFMjAxNTEFMjAxNDIFMjAxNDEFMjAxMzIFMjAxMzEFMjAxMjIFMjAxMjEFMjAxMTIVDQnor7fpgInmi6kFMjAxNzEFMjAxNjIFMjAxNjEFMjAxNTIFMjAxNTEFMjAxNDIFMjAxNDEFMjAxMzIFMjAxMzEFMjAxMjIFMjAxMjEFMjAxMTIUKwMNZ2dnZ2dnZ2dnZ2dnZ2RkAggPEGRkFgFmZAICD2QWAmYPZBYCZg8PFgIeBFRleHQFMTIwMTctMjAxOOWtpuW5tOesrDHlrabmnJ%2FnmoTogIPor5XooajkuI3lrZjlnKjvvIFkZAIDD2QWAmYPZBYGZg8PFgIfAGVkZAICDw8WAh8AZWRkAgQPDxYCHwBlZGQCBA9kFgJmD2QWAmYPPCsACwEADxYCHgdWaXNpYmxlaGRkZC74a7y14FQ9u95U4X%2BZFk%2BC6jss&ddlYearTerm=20162&btnSelect=%E6%9F%A5%E8%AF%A2&RdbCourse=%E4%B8%AA%E4%BA%BA%E8%80%83%E8%AF%95%E8%A1%A8&__EVENTVALIDATION=%2FwEWEgLmo53ZCgLekp65DQKA%2BtHTAQKP%2BqX0CAKA%2BqX0CAKP%2BokJAoD6iQkCj%2FqdogsCgPqdogsCj%2FrhxgICgPrhxgICj%2Fr1mwoCgPr1mwoCj%2FrZvAUC2sfb1QYCuaHTqAgCj%2FnpnQ4CwZTn4whWHkuO6LHUmnWxc9LhgAqJGND3xA%3D%3D");
            string html = d.ToString();
            Regex songti = new Regex("<font face=\"宋体\" color=\"Black\">");
            html = songti.Replace(html, "");
            Regex zhiti = new Regex("</font>");
            html = zhiti.Replace(html, "");
            //缺个正则
            return html;
        }
        public static string CrawlerFromJwc(StudentInfo userinfo, CrawlerDetail detail)
        {
            string studentnum = userinfo.studentnum;
            string pwd = userinfo.pwd;
            if (studentnum.Length != 8)
            {
                return log_fail_xh;
            }
            if (Regex.IsMatch(pwd, "^[0-9]*$"))
            {
                return "您的密码过于简单，请登录教务处信息网更改后再绑定";
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
                return detail();
            }
            else if (MyHttpHelper.regexpwdfail.IsMatch(html))
            {
                return "您现在设置的教务处密码：" + pwd + "，不正确。请重新输入jwc+您的教务处密码来解锁此功能，如jwc123456";
            }
            else if (MyHttpHelper.regexvcfail.IsMatch(html))
            {
                return CrawlerFromJwc(userinfo, detail);
            }
            return "unknownfail";
        }
    }
}