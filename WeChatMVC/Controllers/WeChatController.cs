using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Drawing;
using WeChatMVC.Models;

namespace WeChatMVC.Controllers
{
    public class WeChatController : BaseController
    {
        static UserRequest userrequest;
        // GET: WeChat
        public string Index() //回复全都是xml格式的string
        {
            if (IsFromTencent("961016") && Request.HttpMethod == "POST")//确认是腾讯发来,debug前会在前面加上感叹号
            {
                userrequest = new UserRequest(Request.InputStream);
                if (userrequest.Content == "422")
                {
                    string state_pwd = userrequest.Get_UserstateInDB(0);
                    if (state_pwd == "1")  //待做，这里要返回一个url
                    {
                        return userrequest.Get_Reply("不好意思体育网站崩了");
                    }
                    else
                    {
                        string MD5 = MD5Encrypter(userrequest.FromUserName, state_pwd);
                        if (state_pwd == "2")
                        {
                            string binding = string.Format(@"http://119.23.56.207/binding/else?openid={0}&secret={1}", userrequest.FromUserName, MD5);
                            return userrequest.Get_Link_Reply(binding, "请先绑定你的密码，网址当天有效");
                        }
                        else if (state_pwd == "0")
                        {
                            string binding = string.Format(@"http://119.23.56.207/binding/studentnum?openid={0}&secret={1}", userrequest.FromUserName, MD5);
                            return userrequest.Get_Link_Reply(binding, "请先绑定你的学号，网址当天有效");
                        }
                    }
                }
                else if (userrequest.Content.Substring(0, 3) == "423") 
                {
                    string studentid = userrequest.Content.Substring(3, 8);
                    string password = userrequest.Content.Substring(11);
                    MyHttpHelper a = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
                    a.HttpGet();
                    MyHttpHelper b = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/Base/VerifyCode.aspx");
                    Bitmap input = b.HttpGetImage();
                    IdentificatImage id = new IdentificatImage(input);
                    string vc = id.result;
                    MyHttpHelper c = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_StudentQueryLogin.aspx");
                    c.HttpPost("__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE2OTIxNDU0MTMPZBYCAgEPZBYCAgcPDxYCHgRUZXh0BVDlrabnlJ%2FliJ3lp4vlr4bnoIHkuLrouqvku73or4Hlj7flkI7lha3kvY3jgILlr4bnoIHplb%2FluqbkuI3otoXov4cxMOS4quWtl%2BespuOAgmRkZDanEMgmeoYOigCgOHJXPnTdIOtq&TxtStudentid=" + studentid + "&TxtPassword=" + password + "&txt_verifyCode=" + vc + "&BtnLogin=%E7%99%BB%E5%BD%95&__EVENTVALIDATION=%2FwEWBQKMjOWyBAKf8ICgBwLVqbaRCwLW2qK1CALi44eGDA67X3bLsDOxfx3HDe98WpJ8%2Bncw");
                    MyHttpHelper d = new MyHttpHelper("http://inquiry.ecust.edu.cn/ecustedu/K_StudentQuery/K_BigScoreTableDetail.aspx?key=0");
                    d.HttpGet();
                    string html = d.ToString();
                    Regex regex = new Regex("<td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\"" +
                        " valign=\"middle\"><font face=\"[^\"]*\">(?<kemu>[^<]*)</font></td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font>" +
                        "</td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\" valign=\"middle\">" +
                        "<font face=\"[^\"]*\">(?<fengshu>\\d+)</font></td><td align=\"center\" valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td><td align=\"center\" " +
                        "valign=\"middle\"><font face=\"[^\"]*\">[^<]*</font></td>"
                        ); string result = "";
                    MatchCollection mc = regex.Matches(html);
                    foreach (Match item in mc)
                    {
                        GroupCollection gc = item.Groups;
                        result = result + gc["kemu"].Value + ":" + gc["fengshu"].Value + "\n";
                    }
                    return userrequest.Get_Reply(result);
                }
                #region print
                if (userrequest.FromUserName == "o3dl2wZ3YisQO8GW_bd_c-QOWGsQ" || userrequest.FromUserName == "o3dl2wXugXYxUebDprdV5_KyADP8" || userrequest.FromUserName == "o3dl2wUzmzcr7ZvZ6v7vi_I4Hffw" || userrequest.FromUserName == "o3dl2wZHdvmo1sxQaiKefLRcyr_o")
                {
                    Task printtask = new Task();
                    return userrequest.Get_Printer_Administrator_Reply(printtask);
                }
                #endregion
                return userrequest.Get_Reply("test");
            }
            else if (IsFromTencent("666888"))
            {
                userrequest = new UserRequest(Request.InputStream);
                return userrequest.Get_Reply("hello world");
            }
            else//不是腾讯发来的post
            {
                return "Don't touch this server,guy";
            }
        }

        public bool IsFromTencent(string thistoken)
        {
            var signature = Request["signature"];
            var timestamp = Request["timestamp"];
            var nonce = Request["nonce"];
            var token = thistoken;
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }//信息来源是腾讯才会返回true
    }
}