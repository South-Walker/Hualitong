using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace wechatpost
{
    class Program
    {
        static void Main(string[] args)
        {
            //wx9e64b20cf4937a98
            //7d7d04064acdf0e0f5e51c7a3e0240ea
            //https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx9e64b20cf4937a98&secret=7d7d04064acdf0e0f5e51c7a3e0240ea
                 string post = "{\"button\":[{ \"name\":\"拾光TEN\", \"sub_button\":[{\"type\":\"click\",\"name\":\"卿卿如唔\",\"key\":\"hualitong_love\"},{\"type\":\"view\",\"name\":\"我要卖室友\",\"url\":\"http://koudaigou.net/f/58e25d0abb7c7c57ac3b76e1/\"},{\"type\":\"view\",\"name\":\"双创协会\",\"url\":\"http://m.ecustcxcy.icoc.me\"}]},{ \"name\":\"四维口袋\", \"sub_button\":[{\"type\":\"click\",\"name\":\"更改绑定\",\"key\":\"hualitong_changestudentnum\"},{\"type\":\"click\",\"name\":\"今明课表\",\"key\":\"hualitong_classtable\"},{\"type\":\"click\",\"name\":\"一键成绩\",\"key\":\"hualitong_grade\"},{\"type\":\"click\",\"name\":\"一键评教\",\"key\":\"hualitong_pj\"},{\"type\":\"click\",\"name\":\"更多功能\",\"key\":\"hualitong_more\"}]},{ \"name\":\"关于我们\", \"sub_button\":[{\"type\":\"view\",\"name\":\"四六级管家\",\"url\":\"http://msg.weixiao.qq.com/t/d03a96c84480a4cf1e33d4ec385d47fb\"},{\"type\":\"view\",\"name\":\"华理夜市\",\"url\":\"http://wx.quanzijishi.com/circle/o0es947g45u\"},{\"type\":\"click\",\"name\":\"实用工具\",\"key\":\"hualitong_helper\"},{\"type\":\"view\",\"name\":\"APP下载\",\"url\":\"http://dvomg.xiuzan001.cn/marketing/9e3x7o0z37nr.html\"},{\"type\":\"view\",\"name\":\"加入我们\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx7605be91175e1d69&redirect_uri=http%3A%2F%2Fh5.ixiuzan.cn%2Fbridge%2Findex.html%3FbackUrl%3Dhttp%253A%252F%252Fdvomg.xiuzan001.cn%252Fbaoming%252FplGx7j3omgZQ.html&response_type=code&scope=snsapi_userinfo&state=869971&connect_redirect=1#wechat_redirect\"}]}]}";
               string acc = "SMfIpiZv9bBU4-Zl8hHqL1Wn9bvSqDjuKfNL2DbBzZ9lDnLbuOYnfoZQeJ8jzsJYjdrmIaNqa-6PKoSC1eU8uYbHA6vcpRpPBm0GNumO8cXPrUwOBNOmbBKVi8MeaRtoRFCfAEADRO";
             string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + acc;
             MyHttpHelper a = new MyHttpHelper(url);
            a.HttpPost(post);
        }
    }

    class MyHttpHelper
    {
        static CookieContainer cookiecontainer = new CookieContainer();
        static CookieCollection cookiecollection = new CookieCollection();
        public HttpWebRequest request;
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
