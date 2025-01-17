
using BusinessLogicLayer;
using BusinessLogicLayer.SalesmanTrack;
//using ClosedXML.Excel;
using DataAccessLayer;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraSpreadsheet.Forms;
//using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
//using ModernRetail.Models;
using ModernRetail.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UtilityLayer;
using Excel = Microsoft.Office.Interop.Excel;
using SalesmanTrack;
using DevExpress.Data.Mask;
using System.Reflection.Emit;


namespace ModernRetail.Controllers
{
    public class BeatPlanController : Controller
    {
        // GET: BeatPlan
        public ActionResult Index()
        {
            return View();
        }
    }
}