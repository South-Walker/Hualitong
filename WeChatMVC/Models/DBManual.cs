using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChatMVC.Models;
using System.Web.Mvc;
using WeChatMVC.Controllers;

namespace WeChatMVC.Models
{
    public class DBManual
    {
        public static string xhmes = "xh";
        public static string jwcmes = "jwc";
        public static void AddIntoUsers(string thiswechat_id)
        {
            using (HualitongDBDataContext db = new HualitongDBDataContext())
            {
                var hasexistuser = db.users.SingleOrDefault<users>(u => u.wechat_id == thiswechat_id);
                if(hasexistuser == null)
                {
                    users auser = new users
                    {
                        wechat_id = thiswechat_id,
                        pwds_id = 0
                    };
                    db.users.InsertOnSubmit(auser);
                    db.SubmitChanges();
                }
            }
        }
        public static void AddIntoView_Wechatpwds(string pwd, string wechat_id, string whitchpwd)
        {
            if (pwd == null || pwd.Length == 0)
            {
                return;
            }
            using (HualitongDBDataContext db = new HualitongDBDataContext())
            {
                var hasexistdate = db.users.Single(u => u.wechat_id == wechat_id);
                var pwdid = hasexistdate.pwds_id;
                if (pwdid == 0)
                {
                    pwds newpwd = new pwds
                    {
                    };
                    db.pwds.InsertOnSubmit(newpwd);
                    db.SubmitChanges();
                    pwdid = (from apwd in db.pwds select apwd.pwds_id).Max();
                    hasexistdate.pwds_id = pwdid;
                }
                var pwdt = db.pwds.Single(p => p.pwds_id == pwdid);
                if (whitchpwd == xhmes)
                {
                    pwdt.student_num = pwd;
                }
                else if (whitchpwd == jwcmes)
                {
                    pwdt.jw_pwd = pwd;
                }
                db.SubmitChanges();
            }
        }
        private static StudentInfo selectuser(string wechat_id)
        {
            StudentInfo result = new StudentInfo();
            using (HualitongDBDataContext db = new HualitongDBDataContext())
            {
                var hasexistdate = db.view_wechatpwds.SingleOrDefault<view_wechatpwds>(u => u.wechat_id == wechat_id);
                if (hasexistdate == null)
                {
                    AddIntoUsers(wechat_id);
                    result.hasexistdate = false;
                }
                else
                {
                    result.pwd = HttpUtility.UrlEncode(hasexistdate.jw_pwd);
                    result.studentnum = hasexistdate.student_num;
                }
            }
            result.Check();
            return result;
        }
        public static string SelectFromJwc(string wechat_id, APIController.CrawlerDetail detail)
        {
            StudentInfo userinfo = selectuser(wechat_id);
            if (userinfo.errormessage == "")
                return userinfo.errormessage;
            return APIController.CrawlerFromJwc(userinfo, detail);
        }
    }
}