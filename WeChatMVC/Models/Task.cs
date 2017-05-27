using System;
using System.Web.Mvc;
using System.Xml;
using System.Text.RegularExpressions;
using WeChatMVC.Controllers;

namespace WeChatMVC.Models
{
    public class Task : FileUploadController
    {
        public static string path = @"C:\Users\Administrator\Desktop\task.xml";
        public string num = "1";
        public string msg = "无";
        public string tele = "17077706886";
        public string address = "12-410";
        public string date = DateTime.Now.ToString(@"yyyyMMddHHmm");
        public string pernum = "1";
        public string filename = "";
        public string errorstate = "";
        public double sum = 0;
        public bool is_outdoor = false;
        public Task()
        {
            bool haved = false;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            XmlElement xe = xmldoc.DocumentElement;
            XmlNodeList xnl = xe.SelectNodes("task");
            XmlNode xn = null;
            for (int i = 0; i < xnl.Count; i++)
            {
                xn = xnl[i];
                if (xn.SelectSingleNode("isused").InnerText == "0")
                {
                    haved = true;
                    xn.SelectSingleNode("isused").InnerText = "1";
                    break;
                }
            }
            if (!haved)
            {
                errorstate = "当前没有未读任务！";
            }
            else
            {
                pernum = xn.SelectSingleNode("pernum").InnerText;
                num = xn.SelectSingleNode("num").InnerText;
                pernum = xn.SelectSingleNode("pernum").InnerText;
                sum = Convert.ToDouble(xn.SelectSingleNode("sum").InnerText);
                is_outdoor = Convert.ToBoolean(xn.SelectSingleNode("is_outdoor").InnerText);
                msg = xn.SelectSingleNode("msg").InnerText;
                tele = xn.SelectSingleNode("tele").InnerText;
                address = xn.SelectSingleNode("address").InnerText;
                date = xn.SelectSingleNode("date").InnerText;
                filename = xn.SelectSingleNode("filename").InnerText;

            }
            xmldoc.Save(path);
        }
        public Task(FormCollection c, string fullfilename)
        {
            Regex re = new Regex(@"^\d*$");
            if (c["myfilename"] == "")
                errorstate = "没上传文件啊，兄弟！";
            else
            {
                filename = fullfilename;
            }
            if (c["pernum"] != "")
            {
                pernum = c["pernum"];
                if (!re.IsMatch(pernum))
                {
                    errorstate = "份数与张数请填一个具体的数字。";
                }
            }
            if (c["num"] != "")
            {
                num = c["num"];
                if (!re.IsMatch(num))
                {
                    errorstate = "份数与张数请填一个具体的数字。";
                }
            }
            if (c["msg"] != "")
                msg = c["msg"];
            if (c["tele"] != "")
                tele = c["tele"];
            if (c["address"] != "")
                address = c["address"];
            if (errorstate == "")
            {
                sum = Convert.ToDouble(num) * Convert.ToDouble(pernum);
                if (c["server"] == "outdoorserver")
                {
                    sum = sum * 0.6;
                    is_outdoor = true;
                }
                else
                {
                    sum = sum * 0.3;
                    is_outdoor = false;
                }
            }
        }
        public void Save_Xml()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;
            XmlElement task = doc.CreateElement("task");
            task.AppendChild(getNode(doc, "isused", "0"));
            task.AppendChild(getNode(doc, "pernum", pernum));
            task.AppendChild(getNode(doc, "date", date));
            task.AppendChild(getNode(doc, "filename", filename));
            task.AppendChild(getNode(doc, "num", num));
            task.AppendChild(getNode(doc, "msg", msg));
            task.AppendChild(getNode(doc, "tele", tele));
            task.AppendChild(getNode(doc, "address", address));
            task.AppendChild(getNode(doc, "is_outdoor", is_outdoor.ToString()));
            task.AppendChild(getNode(doc, "sum", sum.ToString()));
            root.AppendChild(task);
            doc.Save(path);
        }
        private XmlElement getNode(XmlDocument xmldoc, string key, string value)
        {
            XmlElement xe = xmldoc.CreateElement(key);
            xe.InnerText = value;
            return xe;
        }
    }
}