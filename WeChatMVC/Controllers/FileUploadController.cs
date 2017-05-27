using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using WeChatMVC.Models;

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

            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
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
}