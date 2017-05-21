using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace WeChatMVC.Controllers
{

    public class FileBaseController : BaseController
    {
        /// <summary>
        /// 保存文件的共用方法
        /// </summary>
        /// <param name="localPath">保存的路径</param>
        /// <param name="fileFullName">文件名</param>
        /// <param name="file">文件</param>
        /// <returns></returns>
        public bool SaveFile(string localPath, string fileFullName, HttpPostedFileBase file)
        {
            if (localPath == string.Empty || fileFullName == String.Empty || file == null)
            {
                return false;
            }

            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }

            try
            {
                file.SaveAs(Path.Combine(localPath, fileFullName));
                DirectoryInfo uploadFolder = new DirectoryInfo(localPath);
                FileInfo[] fileInfo = uploadFolder.GetFiles(fileFullName);
                bool isExists = false;
                foreach (var info in fileInfo)
                {
                    if (info.Name == fileFullName)
                    {
                        isExists = true;
                    }

                }
                return isExists;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    public class FileUploadController : FileBaseController
    {
        // GET: FileUpload
        [HttpGet]
        public ActionResult Index()
        {
            return View("IndexView");
        }
        [HttpPost]
        public ActionResult Index(FormCollection c)
        {
            try
            {
                Task task = new Task(c, TempData["filename"].ToString());
                task.Save_Xml();
                if (task.errorstate == "")
                {
                    ViewData["money"] = "按照您输入的数据，您一共需支付" + task.sum + "元";
                    SendSMS("17077706886");
                    return View("PayView");
                }
                else
                {
                    ViewData["errorstate"] = task.errorstate;
                    return View("ErrorView");
                }
            }
            catch
            {
                ViewData["errorstate"] = "没有文件上传！（当然也可能是别的错误）";
                return View("ErrorView");
            }
        }
        public ActionResult FileUp(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            string fileFullName = String.Empty;
            string localPath = string.Empty;
            #region MyRegion
            //string localPath =
            //    Assembly.GetExecutingAssembly().Location.Remove(Assembly.GetExecutingAssembly().Location.IndexOf("bin"));
            //localPath = Path.Combine(localPath, "Upload");
            #endregion
            try
            {
                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Upload");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                string thisDir = Directory.GetCurrentDirectory();
                int cutIndex = thisDir.LastIndexOf("FileUpload.Tests");
                localPath = thisDir.Substring(0, cutIndex) + "FileUpload\\Upload";
            }
            if (Request.Files.Count == 0)
            {
                return Json(new { error = true });
            }
            string ex = Path.GetExtension(file.FileName);
            ex = ex.ToLower();
            Regex re = new Regex(@"png|jpg|jpeg|docx|doc|pdf$");
            if (!re.IsMatch(ex) || !Regex.IsMatch(file.FileName, "."))  
            {
                ViewData["errorstate"] = "这个文件类型，不行哦";
            }
            fileFullName = Guid.NewGuid().ToString("N") + ex;
            TempData["filename"] = fileFullName;
            //也可以根据需要抽取到FileBaseController里面
            if (!SaveFile(localPath, fileFullName, file))
            {
                return Json(new { error = true });
            }
            else
            {

                return Json(new
                {
                    jsonrpc = "2.0",
                    id = id,
                    filePath = "/Upload/" + fileFullName
                });
            }
        }
    }
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
        public Task(FormCollection c,string fullfilename)
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