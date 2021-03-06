﻿using System;
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
                var hasexistuser = db.users.SingleOrDefault(u => u.wechat_id == thiswechat_id);
                if(hasexistuser == null)
                {
                    users auser = new users
                    {
                        wechat_id = thiswechat_id,
                        pwds_id = 0
                    };
                    db.users.InsertOnSubmit(auser);
                    db.SubmitChanges();
                    db.Dispose();
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
                if (hasexistdate == null)
                {
                    AddIntoUsers(wechat_id);
                    hasexistdate = db.users.Single(u => u.wechat_id == wechat_id);
                }
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
                db.Dispose();
            }
        }
        public static StudentInfo SelectUser(string wechat_id)
        {
            StudentInfo result = new StudentInfo();
            using (HualitongDBDataContext db = new HualitongDBDataContext())
            {
                var hasexistdate = db.view_wechatpwds.SingleOrDefault(u => u.wechat_id == wechat_id);
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
                result.Check();
                db.Dispose();
            }
            return result;
        }
        public static List<StudentInfo> SelectAll()
        {
            List<StudentInfo> result = new List<StudentInfo>();
            using (HualitongDBDataContext db = new HualitongDBDataContext())
            {
                var all = from a in db.view_wechatpwds
                          where a.student_num != null && a.jw_pwd != null
                          select a;
                foreach (var item in all)
                {
                    StudentInfo now = new StudentInfo();
                    now.wechatid = item.wechat_id;
                    now.pwd = HttpUtility.UrlEncode(item.jw_pwd);
                    now.studentnum = item.student_num;
                    result.Add(now);
                }
                db.Dispose();
            }
            return result;
        }
        public static string SelectFromJwc(string wechat_id, JWCHttpHelper.CrawlerDetail<string> detail)
        {
            StudentInfo userinfo = SelectUser(wechat_id);
            if (userinfo.IsSuccess)
                return APIController.CrawlerFromJwc(userinfo, detail);
            else
                return userinfo.errormessage;
        }
    }
}