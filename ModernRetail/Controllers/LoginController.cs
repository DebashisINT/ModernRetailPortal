using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;
using ModernRetail.Models;
using UtilityLayer;
namespace ModernRetail.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        LoginModel model = new LoginModel();
        DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        public ActionResult Index()
        {
            return View();
        }       
        public ActionResult Logout()
        {
            return Redirect("/OMS/Signoff.aspx");
        }
        public ActionResult ChangePassword()
        {           
            return Redirect("/OMS/Management/ToolsUtilities/frmchangepassword.aspx");           
        }
        public ActionResult SubmitForm(LoginModel omodel)
        {
            Encryption epasswrd = new Encryption();
            string Encryptpass = epasswrd.Encrypt(omodel.password.Trim());
            string Validuser;
            Validuser = oDBEngine.AuthenticateUser(omodel.username, Encryptpass).ToString();
            if (Validuser == "Y")
            {
                return RedirectToAction("BranchMasterList", "BranchMaster");
            }

            else
            {
                return View();
            }
        }       
        public ActionResult DownloadAPK()
        {            
            string[,] getData;
            getData = oDBEngine.GetFieldValue("FSM_CONFIG_APKDET", "APKFILE_NAME", null, 1);
            string FileName = getData[0, 0];
            string strPath = (Convert.ToString(System.AppDomain.CurrentDomain.BaseDirectory) + "/Apk/" + FileName);
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "image/jpeg";
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");           
            response.TransmitFile(strPath);
            response.Flush();
            response.End();
            return null;
        }
    }
}