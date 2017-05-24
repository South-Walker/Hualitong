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
using WeChatMVC.Models;

namespace WeChatMVC.Controllers
{
    public class BindingController : BaseController
    {
        // GET: Binding
        [HttpGet]
        public ActionResult Index()
        {
            string openid = Request.QueryString["openid"];
            string secret = Request.QueryString["secret"];
            if (secret == MD5Encrypter(openid))
            {
                //等数据库完成。。。
                LinqToDB ltdb = new LinqToDB();
                var select = from t in ltdb.fortest
                             where t.wechatid == openid
                             select t;
                ViewData["openid"] = openid;
                ViewData["secret"] = secret;
                if (select.Count() == 0)//表明这条记录根本不存在
                    return View("BindingStudentNumView");
                fortest linqdbresult = select.First();
                if (linqdbresult.studentnum == null)
                    return View("BindingStudentNumView");
                else
                    return View("BindingElseView");
            }
            else
                return View();
        }
        [HttpGet]
        public string Else()
        {
            return Welcome();
        }
        [HttpPost]
        public string Else(FormCollection c)
        {
            string openid = c["openid"];
            string secret = c["secret"];
            string studentnum = c["studentnum"];
            if (secret == MD5Encrypter(openid))
            {
                LinqToDB ltdb = new LinqToDB();
                var select = from t in ltdb.fortest
                             where t.wechatid == openid
                             select t;
                fortest linqdbresult = select.First();
                linqdbresult.ty_password = c["typwd"];
                ltdb.SaveChanges();
                return "success";
            }
            else
            {
                return "failed";
            }
        }
        [HttpGet]
        public string Studentnum()
        {
            return "success";
        }
        [HttpPost]
        public string Studentnum(FormCollection c)
        {
            string openid = c["openid"];
            string secret = c["secret"];
            string studentnum = c["studentnum"];
            if (secret == MD5Encrypter(openid))
            {
                LinqToDB ltdb = new LinqToDB();
                var select = from t in ltdb.fortest
                             where t.wechatid == openid
                             select t;
                if (select.Count() != 0)
                {
                    fortest linqdbresult = select.First();
                    linqdbresult.studentnum = studentnum;
                }
                else
                {
                    fortest table = new fortest { wechatid = openid, studentnum = studentnum };
                    ltdb.fortest.Add(table);
                }
                ltdb.SaveChanges();
                return "success";
            }
            else
                return "failed";
        }
    }
}