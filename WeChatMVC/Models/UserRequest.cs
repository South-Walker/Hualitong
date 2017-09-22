using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;
using WeChatMVC.Controllers;

namespace WeChatMVC.Models
{
    public class UserRequest
    {
        /*
         * 所有Get.*_Relpy的函数，都返回直接能回复的xml格式字符
         */
        public string Event = "";
        public string EventKey = "";
        public string ToUserName = "";
        public string FromUserName = "";
        public string CreateTime = "";
        public string MsgType = "";
        public string Content = "";
        public string MsgId = "";
        public string PicUrl = "";
        public string MediaId = "";
        public string studentid = null;//同上
        public string ty_pwd = null;//同上
        public static string hualitong_changestudentnum = "想要更改绑定吗？后台回复xh+学号，如xh10161000，重新绑定学号吧～";
        public static string hualitong_love = "<a href=\"http://dvomg.xiuzan001.cn/marketing/9zgn84240qZp.html\">花一分钟了解“卿卿如晤”</a>\n\n<a href=\"http://www.yingkebao.top/f/58b243fae7aea92b7a2f2cad\">点击填写表白信</a>/玫瑰\n\n<a href=\"http://koudaigou.net/q/u0ipdd\">点击搜索表白信</a>";
        public static string hualitong_helper = "<a href=\"http://msg.weixiao.qq.com/t/91fbdda8c93e3d7de8e1d526de4ab754\">校历查询</a>\n\n<a href=\"http://www.pocketuniversity.cn/index.php/Signin/index/index?media_id=gh_286321331ccb\">早起打卡</a>\n\n<a href=\"http://msg.weixiao.qq.com/t/657cc0da738ced258f154fc88e99f3f0\">计算机等级成绩</a>";
        public static string hualitong_welcome = "Hey~这里是主页君\n\\(￣︶￣*\\))有眼光的你关注了个非常有用的公众号呦 ~么么哒~\n\n 选择“四维口袋”，点击“一键课表”或者“一键成绩”可以进行初始绑定哦，以后就能一键查询成绩和当天课表啦 ~~\n\n温馨提示：因为教务处关闭了外网，9月16日后绑定的用户，请按照绑定流程输入xh加学号，按照提示输入jwc加密码。输入正确的密码后，不用管错误提示，课表和成绩功能将在一天内可以开始正常使用（每天下午七点更新数据库，请耐心等待）\n\n  由于公众号正在建设中，更多功能的推出请关注我们的后期推送";

        public UserRequest(XmlDocument doc)
        {
            XmlElement xe = doc.DocumentElement;
            fillclass(xe);
        }
        public static void Save_log(string xml)
        {
            FileStream fs = new FileStream(@"C:\Users\Administrator\Desktop\wechatlog.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(xml + "\r\n");
            sw.Flush();
            fs.Close();
        }
        private static void Save_log(XmlElement xe)//有误
        {
            string xml = xe.Value;
            Save_log(xml);
        }
        /// <summary>
        /// 返回的值中0表示用户没有绑定学号，
        /// 1表示对应的密码存在，且同时对实例参数中对应的pwd赋值,
        /// 2表示有学号记录但是没有对应的密码。
        /// </summary>
        /// <param name="passwordcode">暂时的，输入0表示查询体育状态</param>
        /// <returns></returns>
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
            try
            {
                Save_log(xml);
            }
            catch
            {

            }
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
            else if(MsgType == "event")
            {
                Event = xe.SelectSingleNode("Event").InnerText + "";
                if (Event == "CLICK")
                {
                    EventKey = xe.SelectSingleNode("EventKey").InnerText + "";
                }
                else if (Event == "subscribe")
                {

                }
                return;
            }
        }
        public bool IsSubscribe()
        {
            if (Event == "" || Event == null)
                return false;
            return Event == "subscribe";
        }
        public bool IsClick()
        {
            if (Event == "" || Event == null)
                return false;
            return Event == "CLICK";
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
        public string Get_Img(string mediaid)
        {
            string reply = "<xml><ToUserName><![CDATA[" + FromUserName +
                "]]></ToUserName><FromUserName><![CDATA[" + ToUserName +
            "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) +
            "</CreateTime><MsgType><![CDATA[" + "image" +
            "]]></MsgType><Image><MediaId><![CDATA[" + mediaid +
            "]]></MediaId></Image></xml>";
            return reply;
        }
        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}