using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Text;

namespace WeChatMVC.Models
{
    public class WeChatHttpHelper:MyHttpHelper
    {
        private const string appid = "wx9e64b20cf4937a98";
        private const string appsecert = "7d7d04064acdf0e0f5e51c7a3e0240ea";
        public static string Token = "";
        public const string TokenAPI = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        public const string MediaUploadAPI = "http://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";
        public WeChatHttpHelper(string url) :
            base(url)
        {

        }
        public WeChatHttpHelper()
        {

        }
        public static void GetToken()
        {
            string url = string.Format(TokenAPI, appid, appsecert);
            WeChatHttpHelper one = new WeChatHttpHelper(url);
            one.HttpGet();
            Regex regex = new Regex("\"access_token\":\"(?<accesstoken>[^\"]*)\"");
            Match m = regex.Match(one.ToString());
            Token = m.Groups["accesstoken"].Value;
        }
        public static string GetMediaID(Stream fs)
        {
            if (string.IsNullOrEmpty(Token))
            {
                return "令牌获取失败";
            }
            fs.Seek(0, SeekOrigin.Begin);
            string url = string.Format(MediaUploadAPI, Token, "image");
            WeChatHttpHelper one = new WeChatHttpHelper(url);

            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            string formdataTemplate = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"media\"; filename=\"0.png\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            byte[] head = Encoding.ASCII.GetBytes(formdataTemplate);
            byte[] foot = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            byte[] body = new byte[fs.Length];
            fs.Read(body, 0, body.Length);

            one.request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            one.request.ContentLength = head.Length + body.Length + foot.Length;
            one.request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            one.request.KeepAlive = true;
            one.request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";

            byte[] all = new byte[head.Length + body.Length + foot.Length];
            head.CopyTo(all, 0);
            body.CopyTo(all, head.Length);
            foot.CopyTo(all, head.Length + body.Length);
            one.HttpPost(all);
            string html = one.ToString();
            Regex regex = new Regex("\"media_id\":\"(?<id>[^\"]*)\"");
            Match m = regex.Match(html);
            return m.Groups["id"].Value;
        }
    }
}