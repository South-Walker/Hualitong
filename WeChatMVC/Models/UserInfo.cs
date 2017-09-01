using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatMVC.Models
{
    public class StudentInfo
    {
        public string studentnum { get; set; }
        public string pwd { get; set; }
        public bool hasexistdate = true;
        public string errormessage = "";
        public bool IsSuccess = false;
        public StudentInfo(string thisstudentnum,string thispwd)
        {
            studentnum = thisstudentnum;
            pwd = thispwd;
        }
        public StudentInfo()
        {

        }
        public void Check()
        {
            if (studentnum == null)
            {
                errormessage = "请输入xh+您的学号来绑定学号，如xh10161000";
            }
            if (studentnum.Length != 8)
            {
                errormessage = "您输入的学号：" + studentnum + "，长度不正确";
            }
            if (pwd == null)
            {
                errormessage = "请输入jwc+您的教务处密码来解锁此功能，如jwc123456";
            }
            IsSuccess = true;
        }
    }
}