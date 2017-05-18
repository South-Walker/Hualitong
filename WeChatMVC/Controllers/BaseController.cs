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
    public class BaseController : Controller
    {
        // GET: Base
        public static string MD5Encrypter(string input)
        {
            string token = "E58D8EE79086E9809AE5BEAEE4BFA1";
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytesinput = Encoding.UTF8.GetBytes(input + token);
            byte[] bytesoutput = md5.ComputeHash(bytesinput);

            string output = "";
            for (int i = 0; i < bytesoutput.Length; i++)
            {
                output += bytesoutput[i].ToString("x2");
            }
            return output;
        } //通用MD5加密
    }
}