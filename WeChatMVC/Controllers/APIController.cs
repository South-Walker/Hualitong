﻿using System.Text.RegularExpressions;
using WeChatMVC.Models;
using System.Web.Mvc;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace WeChatMVC.Controllers
{
    public class APIController : BaseController
    {
        // GET: API
        public static string CrawlerFromJwc(string studentnum, string pwd, JWCHttpHelper.CrawlerDetail<string> detail)
        {
            JWCHttpHelper.Login(studentnum, pwd);
            if (JWCHttpHelper.IsLogin)
            {
                string answer = detail();
                JWCHttpHelper.ClearCookies();
                return answer;
            }
            else
            {
                JWCHttpHelper.ClearCookies();
                return JWCHttpHelper.ErrorMsg;
            }
        }
        public static string CrawlerFromJwc(StudentInfo userinfo, JWCHttpHelper.CrawlerDetail<string> detail)
        {
            return CrawlerFromJwc(userinfo.studentnum, userinfo.pwd, detail);
        }
    }
}