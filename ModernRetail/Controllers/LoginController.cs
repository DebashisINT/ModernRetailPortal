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

        //public ActionResult Index()
        //{
        //    return View();
        //}
        
        public ActionResult Login()
        {
            ViewBag.ApplicationVersion = oDBEngine.GetApplicationVersion();
            ViewBag.ValidateMessage = "";

            if (Session["LMSuserid"] != null)
            {

                if (Request.Cookies["ERPACTIVEURL"] != null && Convert.ToString(Request.Cookies["ERPACTIVEURL"].Value) == "1")
                {
                    Response.Redirect("/OMS/MultiTabError.aspx", true);
                }

                Session["DeveloperRedirect"] = "Yes";

                HttpCookie ERPACTIVEURL = new HttpCookie("ERPACTIVEURL");
                ERPACTIVEURL.Value = "1";
                Response.Cookies.Add(ERPACTIVEURL);

            }

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
            ViewBag.ValidateMessage = "";

            if ((omodel.username is null || omodel.username == "") && (omodel.password is null || omodel.password == ""))
            {
                ViewBag.ValidateMessage = "Please enter User Name and Password";
                return View("Login");
            }
            else if (omodel.username is null || omodel.username == "")
            {
                ViewBag.ValidateMessage = "Please enter User Name";
                return View("Login");
            }
            else if (omodel.password is null || omodel.password == "")
            {
                ViewBag.ValidateMessage = "Please enter Password";
                return View("Login");
            }

            else
            {
                Encryption epasswrd = new Encryption();
                string Encryptpass = epasswrd.Encrypt(omodel.password.Trim());
                string Validuser;

                Validuser = oDBEngine.AuthenticateUser(omodel.username, Encryptpass).ToString();
                //Validuser = oDBEngine.AuthenticateUserMR(omodel.username, Encryptpass).ToString();
                if (Validuser == "Y")
                {
                    HttpCookie cookie = new HttpCookie("sKeyMR");
                    cookie.Value = omodel.username;
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);

                    HttpCookie ERPACTIVEURL = new HttpCookie("ERPACTIVEURL");
                    ERPACTIVEURL.Value = "1";
                    Response.Cookies.Add(ERPACTIVEURL);


                    return RedirectToAction("Index", "Dashboard");
                }

                else
                {
                    ViewBag.ValidateMessage = Validuser;
                    return View("Login");
                }
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