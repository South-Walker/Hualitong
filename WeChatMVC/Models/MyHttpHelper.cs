using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WeChatMVC.Models
{
    public class MyHttpHelper
    {
        static CookieContainer cookiecontainer = new CookieContainer();
        static CookieCollection cookiecollection = new CookieCollection();
        protected HttpWebRequest request;
        protected HttpWebResponse response;
        string html = string.Empty;
        public MyHttpHelper(string url)
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = MyHttpHelper.cookiecontainer;
            cookiecontainer.Add(request.RequestUri, MyHttpHelper.cookiecollection);
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            request.Accept = "*/*;";
            request.UserAgent = "Mozilla/5.0";
            request.ContentType = "application/x-www-form-urlencoded";
        }
        public void HttpGet()
        {
            GetResponse();
        }
        public void HttpPost(string postcontent)
        {
            request.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(postcontent);
            request.ContentLength = bytes.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            GetResponse();
        }
        private void GetResponse()
        {
            response = (HttpWebResponse)request.GetResponse();
            ReadHtml();
            EndCookie();
        }
        private void ReadHtml()
        {
            StreamReader sr = new StreamReader(response.GetResponseStream());
            html = sr.ReadToEnd();
        }
        private void EndCookie()
        {
            cookiecollection = response.Cookies;
        }
        public override string ToString()
        {
            return html;
        }
        public static void ClearCookies()
        {
            cookiecollection = new CookieCollection();
            cookiecontainer = new CookieContainer();
        }
    }
}