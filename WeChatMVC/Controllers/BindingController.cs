using System.Linq;
using System.Data;
using System.Web.Mvc;
using WeChatMVC.Models;

namespace WeChatMVC.Controllers
{
    public class BindingController : BaseController
    {
        // GET: Binding
        [HttpGet]
        public ActionResult Else()
        {
            string openid = Request.QueryString["openid"];
            string secret = Request.QueryString["secret"];
            ViewData["openid"] = openid;
            ViewData["secret"] = secret;
            if (secret == MD5Encrypter(openid, "2"))
            {
                return View("BindingElseView");
            }
            return View("null");
        }
        [HttpPost]
        public string Else(FormCollection c)
        {
            string openid = c["openid"];
            string secret = c["secret"];
            if (secret == MD5Encrypter(openid, "2")) 
            {
                LinqToDB ltdb = new LinqToDB();
                var select = from t in ltdb.fortest
                             where t.wechatid == openid
                             select t;
                fortest linqdbresult = select.First();
                linqdbresult.ty_password = c["typwd"];
                ltdb.SaveChanges();
            }
            return "string";
        }
        [HttpGet]
        public ActionResult Studentnum()
        {
            string openid = Request.QueryString["openid"];
            string secret = Request.QueryString["secret"];
            ViewData["openid"] = openid;
            ViewData["secret"] = secret;
            if (secret == MD5Encrypter(openid, "0")) 
            {
                return View("BindingStudentNumView");
            }
            return View("null");
        }
        [HttpPost]
        public string Studentnum(FormCollection c)
        {
            string openid = c["openid"];
            string secret = c["secret"];
            string studentnum = c["studentnum"];
            if (secret == MD5Encrypter(openid, "0"))
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
            }
            return "string";
        }
    }
}