using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;

namespace WeChatMVC.Models
{
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
        public fortest linqdbresult = null;//数据库完成后更改，这个只在调用Have_PWD方法后才会被赋值
        public string studentid = null;//同上
        public string ty_pwd = null;//同上

        public UserRequest(XmlDocument doc)
        {
            XmlElement xe = doc.DocumentElement;
            fillclass(xe);
        }
        /// <summary>
        /// 返回的值中0表示用户没有绑定学号，
        /// 1表示对应的密码存在，且同时对实例参数中对应的pwd赋值,
        /// 2表示有学号记录但是没有对应的密码。
        /// </summary>
        /// <param name="passwordcode">暂时的，输入0表示查询体育状态</param>
        /// <returns></returns>
        public string Get_UserstateInDB(int passwordcode)//待完成，用来检验用户是否已经填写了密码,在数据库完成后看看要不要改
        {
            //这个查询在数据库正式完成后更改
            LinqToDB ltdb = new LinqToDB();
            var select = from t in ltdb.fortest
                         where t.wechatid == FromUserName
                         select t;
            if (select.Count() == 0)//表明这条记录根本不存在
                return "0";
            linqdbresult = select.ToList()[0];
            studentid = linqdbresult.studentnum;
            ty_pwd = linqdbresult.ty_password;
            //以上
            string result = "2";
            switch (passwordcode)
            {
                case 0:
                    if (ty_pwd != null)
                        result = "1";
                    break;
                case 1:

                    break;
                case 2:

                    break;
            }
            return result;
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
        private void fillclass(XmlElement xe)//实例化的最后一步
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