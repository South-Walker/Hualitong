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
        public static string GetGradePoint(string wechat_id)
        {
            using (HualitongDBDataContext db = new HualitongDBDataContext())
            {
                var hasexistdate = db.view_wechatpwds.SingleOrDefault<view_wechatpwds>(u => u.wechat_id == wechat_id);
                if (hasexistdate == null)
                {
                    AddIntoUsers(wechat_id);
                    return "请输入xh+您的学号来绑定学号，如xh10161000";
                }
                else
                {
                    string jwpwd = hasexistdate.jw_pwd;
                    string studentnum = hasexistdate.student_num;
                    if (studentnum == null)
                    {
                        return "请输入xh+您的学号来绑定学号，如xh10161000";
                    }
                    if (studentnum.Length != 8)
                    {
                        return "您输入的学号：" + studentnum + "，长度不正确";
                    }
                    if (jwpwd == null)
                    {
                        return "请输入jwc+您的教务处密码来解锁此功能，如jwc123456";
                    }
                    else
                    {
                        APIController api = new APIController();
                        return api.jwc_gradepoint(studentnum, jwpwd);
                    }
                }
            }
        }
        public static string GetGradeFromSmallTable(string wechat_id)
        {
            using (HualitongDBDataContext db = new HualitongDBDataContext())
            {
                var hasexistdate = db.view_wechatpwds.SingleOrDefault<view_wechatpwds>(u => u.wechat_id == wechat_id);
                if (hasexistdate == null)
                {
                    AddIntoUsers(wechat_id);
                    return "请输入xh+您的学号来绑定学号，如xh10161000";
                }
                else
                {
                    string jwpwd = hasexistdate.jw_pwd;
                    string studentnum = hasexistdate.student_num;
                    if (studentnum == null)
                    {
                        return "请输入xh+您的学号来绑定学号，如xh10161000";
                    }
                    if (studentnum.Length != 8)
                    {
                        return "您输入的学号：" + studentnum + "，长度不正确";
                    }
                    if (jwpwd == null)
                    {
                        return "请输入jwc+您的教务处密码来解锁此功能，如jwc123456";
                    }
                    else
                    {
                        APIController api = new APIController();
                        return api.jwc_smarttable(studentnum, jwpwd);
                    } 
                }
            }
        }
        public static string GetGradeFromBigTable(string wechat_id)
        {
            using (HualitongDBDataContext db = new HualitongDBDataContext())
            {
                var hasexistdate = db.view_wechatpwds.SingleOrDefault<view_wechatpwds>(u => u.wechat_id == wechat_id);
                if (hasexistdate == null)
                {
                    AddIntoUsers(wechat_id);
                    return "请输入xh+您的学号来绑定学号，如xh10161000";
                }
                else
                {
                    string jwpwd = hasexistdate.jw_pwd;
                    string studentnum = hasexistdate.student_num;
                    if (studentnum == null)
                    {
                        return "请输入xh+您的学号来绑定学号，如xh10161000";
                    }
                    if (studentnum.Length != 8)
                    {
                        return "您输入的学号：" + studentnum + "，长度不正确";
                    }
                    if (jwpwd == null)
                    {
                        return "请输入jwc+您的教务处密码来解锁此功能，如jwc123456";
                    }
                    else
                    {
                        APIController api = new APIController();
                        return api.jwc_largetable(studentnum, jwpwd);
                    }
                }
            }
        }
        public static string GetClassGrade(string wechat_id)
        {
            using (HualitongDBDataContext db = new HualitongDBDataContext())
            {
                var hasexistdate = db.view_wechatpwds.SingleOrDefault<view_wechatpwds>(u => u.wechat_id == wechat_id);
                if (hasexistdate == null)
                {
                    AddIntoUsers(wechat_id);
                    return "请输入xh+您的学号来绑定学号，如xh10161000";
                }
                else
                {
                    string jwpwd = hasexistdate.jw_pwd;
                    string studentnum = hasexistdate.student_num;
                    if (studentnum == null)
                    {
                        return "请输入xh+您的学号来绑定学号，如xh10161000";
                    }
                    if (studentnum.Length != 8)
                    {
                        return "您输入的学号：" + studentnum + "，长度不正确";
                    }
                    if (jwpwd == null)
                    {
                        return "请输入jwc+您的教务处密码来解锁此功能，如jwc123456";
                    }
                    else
                    {
                        APIController api = new APIController();
                        return api.jwc_classtable(studentnum, jwpwd);
                    }
                }
            }
        }
    }
}