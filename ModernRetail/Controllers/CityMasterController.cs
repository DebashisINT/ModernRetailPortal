using DevExpress.Web.Mvc;
using DevExpress.Web;
using ModernRetail.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;
using System.Data;

namespace ModernRetail.Controllers
{
    public class CityMasterController : Controller
    {
        // GET: CityMaster
        CityMasterModel obj = new CityMasterModel();
        public ActionResult Index()
        {

            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Index", "CityMaster");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            CityMasterModel Dtls = new CityMasterModel();
            DataTable ds = Dtls.GetCountry();

            List<COUNTRYLIST> COUNTRYLIST = new List<COUNTRYLIST>();
            COUNTRYLIST = APIHelperMethods.ToModelList<COUNTRYLIST>(ds);
            Dtls.COUNTRYLIST = COUNTRYLIST;


            return View(Dtls);
        }
        public ActionResult PartialGridList(StateMasterModel model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Index", "CityMaster");
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;

                string Is_PageLoad = string.Empty;
                DataTable dt = new DataTable();

                if (model.Is_PageLoad != "1")
                    Is_PageLoad = "Ispageload";

                ViewData["ModelData"] = model;
                string Userid = Convert.ToString(Session["MRuserid"]);

                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_MR_CITYMASTER", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GETLISTINGDETAILS");
                sqlcmd.Parameters.Add("@USER_ID", Userid);
                sqlcmd.Parameters.Add("@ISPAGELOAD", model.Is_PageLoad);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                return PartialView("_PartialCityMasterList", GetDetailsList(Is_PageLoad));
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        public IEnumerable GetDetailsList(string Is_PageLoad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
            string Userid = Convert.ToString(Session["MRuserid"]);
            if (Is_PageLoad != "Ispageload")
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_CITYMASTERLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_CITYMASTERLISTs
                        where d.SEQ == 0
                        select d;
                return q;
            }
        }
        public JsonResult SaveCity(Int64 Country, string id, Int64 State, string City, string CityLocationLat, string CityLocationLong)
        {
            int output = 0;
            string Userid = Convert.ToString(Session["MRuserid"]);
            output = obj.SaveCity(State, City, CityLocationLat, CityLocationLong, Userid, id);
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditCity(string id)
        {
            DataTable output = new DataTable();
            output = obj.EditCity(id);

            if (output.Rows.Count > 0)
            {
                return Json(new
                {
                    Country = Convert.ToString(output.Rows[0]["cou_id"]),
                    State = Convert.ToString(output.Rows[0]["state_id"]),
                    CITY_NAME = Convert.ToString(output.Rows[0]["city_name"]),
                    City_Lat = Convert.ToString(output.Rows[0]["City_Lat"]),
                    City_Long = Convert.ToString(output.Rows[0]["City_Long"])
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { name = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Delete(string ID)
        {
            int output = 0;
            output = obj.Delete(ID);
            return Json(output, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExporList(int type)
        {
            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToXlsx(GetGridViewSettings(), GetDetailsList(""));
                //case 2:
                //    return GridViewExtension.ExportToPdf(GetCategoryGridViewSettings(), LGetCountryDetailsList(""));
                //case 3:
                //    return GridViewExtension.ExportToXls(GetCategoryGridViewSettings(), LGetCountryDetailsList(""));
                //case 4:
                //    return GridViewExtension.ExportToRtf(GetCategoryGridViewSettings(), LGetCountryDetailsList(""));
                //case 5:
                //    return GridViewExtension.ExportToCsv(GetCategoryGridViewSettings(), LGetCountryDetailsList(""));
                default:
                    break;
            }
            return null;
        }

        private GridViewSettings GetGridViewSettings()
        {


            var settings = new GridViewSettings();
            settings.Name = "PartialGridList";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "State";

            settings.Columns.Add(x =>
            {
                x.FieldName = "STATENAME";
                x.Caption = "State";
                x.VisibleIndex = 1;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "STATECODE";
                x.Caption = "State Code";
                x.VisibleIndex = 2;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "COUNTRYNAME";
                x.Caption = "Country";
                x.VisibleIndex = 3;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(15);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CREATEUSER";
                x.Caption = "Created By";
                x.VisibleIndex = 4;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CREATEDATE";
                x.Caption = "Created On";
                x.VisibleIndex = 5;
                x.Width = 120;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(15);
                x.ColumnType = MVCxGridViewColumnType.DateEdit;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";

            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "MODIFYUSER";
                x.Caption = "Updated By";
                x.VisibleIndex = 6;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "MODIFYDATE";
                x.Caption = "Updated On";
                x.VisibleIndex = 7;
                x.Width = 120;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(16);
                x.ColumnType = MVCxGridViewColumnType.DateEdit;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";

            });

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }
    }
}