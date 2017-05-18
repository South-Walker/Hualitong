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

namespace WeChatMVC.Controllers
{
    public class WeChatController : BaseController
    {
        static UserRequest userrequest;
        // GET: WeChat
        public string Index() //回复全都是xml格式的string
        {
            if (IsFromTencent() && Request.HttpMethod == "POST")//确认是腾讯发来
            {
                userrequest = new UserRequest(Request.InputStream);
                if (userrequest.Content == "422")
                {
                    if (Have_PWD()) ;//待做，这里要返回一个url
                    else
                    {
                        string MD5 = MD5Encrypter(userrequest.FromUserName);
                        string binding = string.Format(@"http://119.23.56.207/binding?openid={0}&secret={1}", userrequest.FromUserName, MD5);
                        return userrequest.Get_Link_Reply(binding, "请先绑定你的学号与密码");
                    }
                }
                if (userrequest.FromUserName == "o3dl2wZ3YisQO8GW_bd_c-QOWGsQ" || userrequest.FromUserName == "o3dl2wXugXYxUebDprdV5_KyADP8" || userrequest.FromUserName == "o3dl2wUzmzcr7ZvZ6v7vi_I4Hffw" || userrequest.FromUserName == "o3dl2wZHdvmo1sxQaiKefLRcyr_o")
                {
                    Task printtask = new Task();
                    return userrequest.Get_Printer_Administrator_Reply(printtask);
                }
                return userrequest.Get_Reply("test");
            }
            else//不是腾讯发来的post
            {
                return "Don't touch this server,guy";
            }
        }
        
        public bool Have_PWD()//待完成，用来检验用户是否已经填写了密码
        {
            return false;
        }
        public bool IsFromTencent()
        {
            var signature = Request["signature"];
            var timestamp = Request["timestamp"];
            var nonce = Request["nonce"];
            var token = "961016";
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
    
    public class UserRequest
    {
        /*
         * 所有Get.*_Relpy的函数，都返回直接能回复的xml格式字符
         */
        public string ToUserName;
        public string FromUserName;
        public string CreateTime;
        public string MsgType;
        public string Content;
        public string MsgId;
        public string PicUrl;
        public string MediaId;
        public UserRequest(XmlDocument doc)
        {
            XmlElement xe = doc.DocumentElement;
            fillclass(xe);
        }
        public UserRequest(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            XmlElement xe = xmldoc.DocumentElement;
            fillclass(xe);
        }
        public UserRequest(Stream xmlstream)
        {
            StreamReader sr = new StreamReader(xmlstream);
            string xml = sr.ReadToEnd();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            XmlElement xe = xmldoc.DocumentElement;
            fillclass(xe);
        }
        private void fillclass(XmlElement xe)
        {
            ToUserName = xe.SelectSingleNode("ToUserName").InnerText;
            FromUserName = xe.SelectSingleNode("FromUserName").InnerText;
            CreateTime = xe.SelectSingleNode("CreateTime").InnerText;
            MsgType = xe.SelectSingleNode("MsgType").InnerText;
            if (MsgType == "text")
            {
                Content = xe.SelectSingleNode("Content").InnerText;
            }
            else if (MsgType == "image")
            {
                MediaId = xe.SelectSingleNode("MediaId").InnerText;
                PicUrl = xe.SelectSingleNode("PicUrl").InnerText;
            }
            MsgId = xe.SelectSingleNode("MsgId").InnerText;
        }
        public string Get_MJ_Reply(DateTime buyday, DateTime ticketday, string origination, string destination)
        {
            string url_origination = get_urlEncode(origination);
            string url_destination = get_urlEncode(destination);
            string url = "http://ecust.top/mj/html/index.php?buy_year=" +
                buyday.Year + "&buy_month=" +
                buyday.Month + "&buy_day=" +
                buyday.Day + "&buy_hour=" +
                buyday.Hour + "&buy_minute=" +
                buyday.Minute + "&stationary=0&origination=" +
                url_origination + "&destination=" +
                url_destination + "&ticket_year=" +
                ticketday.Year + "&ticket_month=" +
                ticketday.Month + "&ticket_day=" +
                ticketday.Day + "&ticket_hour=" +
                ticketday.Hour + "&ticket_minute=" +
                ticketday.Minute + "&ticket_year=" +
                ticketday.Year + "&ticket_month=" +
                ticketday.Month + "&ticket_day=" +
                ticketday.Day + "&ticket_hour=" +
                ticketday.Hour + "&ticket_minute=" +
                ticketday.Minute;

            return Get_Reply(Get_Link_Reply(url, "车票"));
        }
        private string get_urlEncode(string input)
        {
            return HttpUtility.UrlEncode(input);
        }
        public string Get_Printer_Administrator_Reply(Task printertask)
        {
            if (printertask.filename == "")
                return Get_Reply(printertask.errorstate);
            string server = (printertask.is_outdoor) ? "送货上门" : "自取";
            /*
            string reply = "用户上传的文件地址：" + @"http://119.23.56.207/upload/" + printertask.filename + "\r\n" +
                "打印的份数：" + printertask.num + "\r\n" +
                "一份有：" + printertask.pernum + "张\r\n" +
                "他应该付" + printertask.sum + "元\r\n" +
                "用户的联系方式：" + printertask.tele + "\r\n" +
                "用户下单时间：" + printertask.date + "\r\n" +
                "用户的地址：" + printertask.address + "\r\n" +
                "用户留言：" + printertask.msg + "\r\n" +
                "他要求：" + server;*/
            string reply = string.Format(
                "用户上传的文件地址： http://119.23.56.207/upload/{0} \r\n打印的份数：{1}\r\n一份有：{2}张\r\n他应该付：{3}元\r\n用户的联系方式：{4}\r\n用户的下单时间：{5}\r\n用户的地址：{6}\r\n用户留言：{7}\r\n他要求：{8}"
                , printertask.filename, printertask.num
                , printertask.pernum, printertask.sum
                , printertask.tele, printertask.date
                , printertask.address, printertask.msg
                , server);
            return Get_Reply(reply);
        }
        public string Get_Link_Reply(string url, string text) 
        {
            return Get_Reply("<a href=\"" + url + "\">" + text + "</a>");
        }
        public string Get_Reply(string content)
        {
            string reply = "<xml><ToUserName><![CDATA[" + FromUserName +
                "]]></ToUserName><FromUserName><![CDATA[" + ToUserName +
            "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) +
            "</CreateTime><MsgType><![CDATA[" + "text" +
            "]]></MsgType><Content><![CDATA[" + content +
            "]]></Content></xml>";
            return reply;
        }//返回最终的xml
        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}