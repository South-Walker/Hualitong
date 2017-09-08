using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace WeChatMVC.Models
{
    public class StudentInfo
    {
        public string studentnum { get; set; }
        public string pwd { get; set; }
        public bool hasexistdate = true;
        public string errormessage = "";
        public bool IsSuccess = true;
        public StudentInfo(string thisstudentnum,string thispwd)
        {
            studentnum = thisstudentnum;
            pwd = thispwd;
        }
        public StudentInfo()
        {
            studentnum = string.Empty;
            pwd = string.Empty;
        }
        public void Check()
        {
            if (string.IsNullOrEmpty(studentnum))
            {
                IsSuccess = false;
                this.errormessage = "请输入xh+您的学号来绑定学号，如xh10161000";
            }
            else if (studentnum.Length != 8)
            {
                IsSuccess = false;
                errormessage = "您输入的学号：" + studentnum + "，长度不正确";
            }
            else if (string.IsNullOrEmpty(pwd))
            {
                IsSuccess = false;
                errormessage = "请输入jwc+您的教务处密码来解锁此功能，如jwc123456";
            }
            else if (Regex.IsMatch(pwd, "^[0-9]*$"))
            {
                IsSuccess = false;
                errormessage = "您的密码过于简单，请登录教务处信息网更改后再绑定";
            }
        }
    }
}