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
    public class LogOutController : Controller
    {
        // GET: Login

        LoginModel model = new LoginModel();
        DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        public ActionResult Index()
        {
            return View();
        }
       
    }
}