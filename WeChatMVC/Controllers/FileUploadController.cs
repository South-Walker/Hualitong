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

    public class FileBaseController : Controller
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
        public string Index(FormCollection c)
        {
            Task test = new Task(c, TempData["filename"].ToString());
            test.Save_Xml();
            if (test.errorstate == "")
                return "订单上传成功！";
            else
                return test.errorstate;
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
                string thisDir = System.IO.Directory.GetCurrentDirectory();
                int cutIndex = thisDir.LastIndexOf("FileUpload.Tests");
                localPath = thisDir.Substring(0, cutIndex) + "FileUpload\\Upload";
            }
            if (Request.Files.Count == 0)
            {
                return Json(new { error = true });
            }
            string ex = Path.GetExtension(file.FileName);
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
    class Task : FileUploadController
    {
        public static string path = @"C:\Users\Administrator\Desktop\task.xml";
        public string num = "1";
        public string msg = "无";
        public string tele = "17077706886";
        public string address = "12-410";
        public string date = DateTime.Now.ToString(@"MMddyyyyHHmm");
        public string filename;
        public string errorstate = "";
        public Task(FormCollection c,string fullfilename)
        {
            if (c["myfilename"] == "")
                errorstate = "没上传文件啊，兄弟！";
            else
            {
                filename = fullfilename;
            }
            if (c["num"] != "")
                num = c["num"];
            if (c["msg"] != "")
                msg = c["msg"];
            if (c["tele"] != "")
                tele = c["tele"];
            if (c["address"] != "")
                address = c["address"];
        }
        public void Save_Xml()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;
            XmlElement task = doc.CreateElement("task");

            task.AppendChild(getNode(doc, "date", date));
            task.AppendChild(getNode(doc, "filename", filename));
            task.AppendChild(getNode(doc, "num", num));
            task.AppendChild(getNode(doc, "msg", msg));
            task.AppendChild(getNode(doc, "tele", tele));
            task.AppendChild(getNode(doc, "address", address));
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