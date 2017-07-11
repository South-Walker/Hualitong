using System.Text;
using System.Net;
using System.IO;
using System.Drawing;

namespace WeChatMVC.Models
{
    class MyHttpHelper
    {
        static CookieContainer cookiecontainer = new CookieContainer();
        static CookieCollection cookiecollection = new CookieCollection();
        HttpWebRequest request;
        HttpWebResponse response;
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
        public Bitmap HttpGetImage()
        {
            response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            Bitmap bitmap = (Bitmap)Image.FromStream(stream);
            return bitmap;
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
    }
}