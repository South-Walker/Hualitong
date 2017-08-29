﻿using System;
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
using System.Drawing;
using WeChatMVC.Models;

namespace WeChatMVC.Controllers
{
    public class WeChatController : BaseController
    {
        static UserRequest userrequest;
        // GET: WeChat
        public string Index() //回复全都是xml格式的string
        {
            //测试语句
            //return APIController.CrawlerFromJwc("10150111", "***ak96101", APIController.jwc_classtable);
            if (IsFromTencent("961016") && Request.HttpMethod == "GET")
            {
                return Request["echostr"];
            }
            if (IsFromTencent("961016") && Request.HttpMethod == "POST") //确认是腾讯发来,debug前会在前面加上感叹号
            {
                try
                {
                    #region wechatpost
                    userrequest = new UserRequest(Request.InputStream);
                    if (userrequest.IsClick())
                    {
                        switch (userrequest.EventKey)
                        {
                            case "hualitong_love":
                                return userrequest.Get_Reply(UserRequest.hualitong_love);
                            case "hualitong_helper":
                                return userrequest.Get_Reply(UserRequest.hualitong_helper);
                            case "hualitong_changestudentnum":
                                return userrequest.Get_Reply(UserRequest.hualitong_changestudentnum);
                            case "hualitong_grade":
                                return userrequest.Get_Reply(DBManual.SelectFromJwc(userrequest.FromUserName, APIController.smalltable));
                            case "hualitong_classtable":
                                return userrequest.Get_Reply(DBManual.SelectFromJwc(userrequest.FromUserName, APIController.classtable));
                            case "hualitong_pj":
                                return userrequest.Get_Reply("现在还没到评教时间哦～");
                            case "hualitong_more":
                                return userrequest.Get_Reply("查询成绩大表请回复：成绩大表\n查询绩点请回复：绩点\n查询完整课表请回复：完整课表");
                            default:
                                return userrequest.Get_Reply("功能还在开发中，敬请期待~");
                        }
                    }
                    else if (userrequest.IsSubscribe())
                    {
                        DBManual.AddIntoUsers(userrequest.FromUserName);
                        return userrequest.Get_Reply(UserRequest.hualitong_welcome);
                    }
                    else
                    {
                        string message = userrequest.Content;

                        if (message == "成绩大表")
                        {
                            return userrequest.Get_Reply(DBManual.SelectFromJwc(userrequest.FromUserName, APIController.largetable));
                        }
                        else if (message == "绩点")
                        {
                            return userrequest.Get_Reply(DBManual.SelectFromJwc(userrequest.FromUserName, APIController.gradepoint));
                        }
                        else if (message == "完整课表")
                        {
                            return userrequest.Get_Reply("内部测试中，即将开放!");
                        }
                        else if (message.Length > DBManual.xhmes.Length && message.Substring(0, DBManual.xhmes.Length) == DBManual.xhmes)
                        {
                            DBManual.AddIntoView_Wechatpwds(message.Substring(DBManual.xhmes.Length), userrequest.FromUserName, DBManual.xhmes);
                            return userrequest.Get_Reply("学号已修改，现在您绑定的学号为：" + message.Substring(DBManual.xhmes.Length));
                        }
                        else if (message.Length > DBManual.jwcmes.Length && message.Substring(0, DBManual.jwcmes.Length) == DBManual.jwcmes)
                        {
                            DBManual.AddIntoView_Wechatpwds(message.Substring(DBManual.jwcmes.Length), userrequest.FromUserName, DBManual.jwcmes);
                            return userrequest.Get_Reply("教务处密码已修改，现在您绑定的教务处密码为：" + message.Substring(DBManual.jwcmes.Length) + "，为防止密码泄露，请及时删除此条消息");
                        }
                    }
                    #region print
                    if (userrequest.FromUserName == "o3dl2wZ3YisQO8GW_bd_c-QOWGsQ" || userrequest.FromUserName == "o3dl2wXugXYxUebDprdV5_KyADP8" || userrequest.FromUserName == "o3dl2wUzmzcr7ZvZ6v7vi_I4Hffw" || userrequest.FromUserName == "o3dl2wZHdvmo1sxQaiKefLRcyr_o")
                    {
                        Task printtask = new Task();
                        return userrequest.Get_Printer_Administrator_Reply(printtask);
                    }
                    #endregion
                    return "success";
                    #endregion}
                }
                catch
                {
                    return "success";
                }
            }
            else//不是腾讯发来的post
            {
                return "Do not touch this server,guy!";
            }
        }

        public bool IsFromTencent(string thistoken)
        {
            var signature = Request["signature"];
            var timestamp = Request["timestamp"];
            var nonce = Request["nonce"];
            var token = thistoken;
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }//信息来源是腾讯才会返回true
    }
};