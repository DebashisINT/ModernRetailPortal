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
using System.Data;

namespace ModernRetail.Controllers
{
    public class ProductClassCategoryController : Controller
    {
        // GET: ProductClassCategory
        ProductClassCategoryModel obj = new ProductClassCategoryModel();
        public ActionResult Index()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Index", "ProductClassCategory");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            return View();

        }
        public ActionResult PartialGridList(BrandMasterModel model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Index", "ProductClassCategory");
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
                sqlcmd = new SqlCommand("PRC_MR_PRODUCTCLASSCATEGORY", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GETLISTINGDETAILS");
                sqlcmd.Parameters.Add("@USER_ID", Userid);
                sqlcmd.Parameters.Add("@ISPAGELOAD", model.Is_PageLoad);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                return PartialView("PartialProductClassListing", GetDetailsList(Is_PageLoad));

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
                var q = from d in dc.MR_PRODUCTCLASSCATEGORYLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_PRODUCTCLASSCATEGORYLISTs
                        where d.SEQ == 0
                        select d;
                return q;
            }
        }
        public JsonResult SaveProductClassCategory(string ShortName, string id, string Name, string Description)
        {
            int output = 0;
            string Userid = Convert.ToString(Session["MRuserid"]);
            output = obj.SaveProductClassCategory(ShortName, Userid, id, Name, Description);
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditProductClassCategory(string id)
        {
            DataTable output = new DataTable();
            output = obj.EditProductClassCategory(id);

            if (output.Rows.Count > 0)
            {
                return Json(new
                {
                    ProductClass_Code = Convert.ToString(output.Rows[0]["ProductClass_Code"]),
                    ProductClass_Name = Convert.ToString(output.Rows[0]["ProductClass_Name"]),
                    ProductClass_Description = Convert.ToString(output.Rows[0]["ProductClass_Description"]),
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


        public ActionResult ExporProductClassList(int type)
        {
            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToXlsx(GetGridViewSettings(), GetDetailsList(""));
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
            settings.SettingsExport.FileName = "ProductClassCategory";

            settings.Columns.Add(x =>
            {
                x.FieldName = "ProductClass_Code";
                x.Caption = "Short Name";
                x.VisibleIndex = 1;
                x.Width = 200;

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "ProductClass_Name";
                x.Caption = "Name";
                x.VisibleIndex = 2;
                x.Width = 200;

            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "ProductClass_Description";
                x.Caption = "Description";
                x.VisibleIndex = 3;
                x.Width = 200;

            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateUser";
                x.Caption = "Created By";
                x.VisibleIndex = 4;
                x.Width = 200;
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateDate";
                x.Caption = "Created On";
                x.VisibleIndex = 5;
                x.Width = 200;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "ModifyUser";
                x.Caption = "Updated By";
                x.VisibleIndex = 6;
                x.Width = 200;
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "ModifyDate";
                x.Caption = "Updated On";
                x.VisibleIndex = 7;
                x.Width = 200;
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