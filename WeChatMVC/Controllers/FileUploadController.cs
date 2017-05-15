using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Index()
        {
            return View("IndexView");
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
            //没有做文件类型验证
            fileFullName = Guid.NewGuid().ToString("N") + ex;
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