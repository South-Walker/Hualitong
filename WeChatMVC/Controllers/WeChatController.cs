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
    public class WeChatController : Controller
    {
        static UserRequest userrequest;
        // GET: WeChat
        public string Index() //回复全都是xml格式的string
        {
            if(checkSignature())
            {
                if (Request.HttpMethod == "POST")
                {
                    userrequest = new UserRequest(Request.InputStream);
                    if (userrequest.FromUserName == "o3dl2wXugXYxUebDprdV5_KyADP8" || userrequest.FromUserName == "o3dl2wUzmzcr7ZvZ6v7vi_I4Hffw") 
                    {
                        Task printtask = new Task();
                        if (printtask.filename == "")
                            return userrequest.Get_Reply(printtask.errorstate);
                        string reply = "用户上传的文件地址：" + @"http://119.23.56.207/upload/" + printtask.filename + "\r\n" +
                            "打印的份数：" + printtask.num + "\r\n" +
                            "用户的联系方式：" + printtask.tele + "\r\n" +
                            "用户下单时间：" + printtask.date + "\r\n" +
                            "用户的地址：" + printtask.address + "\r\n" +
                            "用户留言：" + printtask.msg;

                        return userrequest.Get_Reply(reply);
                    }
                    return userrequest.Get_Reply("test");
                }
            }
            return "Don't touch this server,guy";
        }

        public bool checkSignature()
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