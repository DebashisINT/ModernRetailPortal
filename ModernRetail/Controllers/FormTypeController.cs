//*****************************************************************************************************************************
//Written by Sanchita - on 27/01/2025 - for Module Form Type.Mantis : 0027902
//* ****************************************************************************************************************************

using DevExpress.Web.Mvc;
using DevExpress.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModernRetail.Models;
using System.Data;
using UtilityLayer;
using DataAccessLayer;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ModernRetail.Controllers
{
    public class FormTypeController : Controller
    {
        // GET: FormType
        FormTypeModel obj = new FormTypeModel();

        public ActionResult Index()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Index", "FormType");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            FormTypeModel Dtls = new FormTypeModel();

            DataTable dt = GetSecGroupList();

            List<SECGRPLIST> SECGRPLIST = new List<SECGRPLIST>();
            SECGRPLIST = APIHelperMethods.ToModelList<SECGRPLIST>(dt);
            Dtls.SECGRPLIST = SECGRPLIST;

            Dtls.grp_id = 0;
            Dtls.IsActive = true;

            return View(Dtls);
        }

        public DataTable GetSecGroupList()
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            DataTable dt = new DataTable();
            try
            {
                using (proc = new ProcedureExecute("PRC_MR_FORMTYPEMASTER"))
                {
                    proc.AddVarcharPara("@ACTION", 100, "GETSECGROUPLIST");
                    dt = proc.GetTable();
                    return dt;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public ActionResult PartialGridList(FormTypeModel model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Index", "FormType");
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

                String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_MR_FORMTYPEMASTER", sqlcon);
                sqlcmd.Parameters.AddWithValue("@ACTION", "GETLISTINGDETAILS");
                sqlcmd.Parameters.AddWithValue("@USER_ID", Userid);
                sqlcmd.Parameters.AddWithValue("@ISPAGELOAD", model.Is_PageLoad);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                return PartialView("_PartialFormTypeList", GetDetailsList(Is_PageLoad));

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
                var q = from d in dc.MR_FORMTYPELISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_FORMTYPELISTs
                        where d.SEQ == 0
                        select d;
                return q;
            }
        }

        public JsonResult SaveFormType(string SurveyDesc, string Remarks, string SecGroup, string IsActive, string FormTypeId)
        {
            //int output = 0;
            string Userid = Convert.ToString(Session["MRuserid"]);

            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
            SqlCommand sqlcmd = new SqlCommand();
            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("PRC_MR_FORMTYPEMASTER", sqlcon);
            sqlcmd.Parameters.AddWithValue("@FORMTYPE_ID", FormTypeId);
            if (FormTypeId == "0")
                sqlcmd.Parameters.AddWithValue("@ACTION", "ADDFORMTYPE");
            else
                sqlcmd.Parameters.AddWithValue("@ACTION", "UPDATEFORMTYPE");
            
            sqlcmd.Parameters.AddWithValue("@USER_ID", Userid);
            sqlcmd.Parameters.AddWithValue("@FORMTYPE_SURVEYDESC", SurveyDesc);
            sqlcmd.Parameters.AddWithValue("@FORMTYPE_REMARKS", Remarks);
            sqlcmd.Parameters.AddWithValue("@FORMTYPE_SECGROUPID", SecGroup);
            sqlcmd.Parameters.AddWithValue("@FORMTYPE_ISACTIVE", IsActive);
            SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
            output.Direction = ParameterDirection.Output;
            sqlcmd.Parameters.Add(output);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            sqlcon.Close();

            Int32 ReturnValue = Convert.ToInt32(output.Value);

            sqlcmd.Dispose();
            sqlcmd.Dispose();


            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditFormType(string id)
        {
            DataTable output = new DataTable();
            output = GetFormTypeData(id);

            if (output.Rows.Count > 0)
            {
                var Is_active = 0;
                if (Convert.ToString(output.Rows[0]["FORMTYPE_ISACTIVE"]) == "True")
                {
                    Is_active = 1;
                }

                return Json(new
                {
                    FORMTYPE_SURVEYDESC = Convert.ToString(output.Rows[0]["FORMTYPE_SURVEYDESC"]),
                    FORMTYPE_REMARKS = Convert.ToString(output.Rows[0]["FORMTYPE_REMARKS"]),
                    FORMTYPE_SECGROUPID = Convert.ToString(output.Rows[0]["FORMTYPE_SECGROUPID"]),
                    FORMTYPE_ISACTIVE = Is_active,

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { name = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        public DataTable GetFormTypeData(string ID)
        {
            ProcedureExecute proc;
            DataTable dt = new DataTable();
            try
            {
                using (proc = new ProcedureExecute("PRC_MR_FORMTYPEMASTER"))
                {
                    proc.AddVarcharPara("@FORMTYPE_ID", 100, ID);
                    proc.AddVarcharPara("@ACTION", 100, "EDITFORMTYPE");
                    dt = proc.GetTable();
                    return dt;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public JsonResult Delete(string ID)
        {
            int output = 0;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_FORMTYPEMASTER");
            proc.AddNVarcharPara("@action", 50, "DELETEFORMTYPE");
            proc.AddNVarcharPara("@FORMTYPE_ID", 30, ID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            output = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));


            return Json(rtrnvalue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportFormTypeList(int type)
        {
            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToXlsx(GetCategoryGridViewSettings(), GetDetailsList(""));
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

        private GridViewSettings GetCategoryGridViewSettings()
        {


            var settings = new GridViewSettings();
            settings.Name = "PartialGridList";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "FORMTYPE";

            settings.Columns.Add(x =>
            {
                x.FieldName = "FORMTYPE_SURVEYDESC";
                x.Caption = " ";
                x.VisibleIndex = 1;
                x.Width = 200;
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "FORMTYPE_REMARKS";
                x.Caption = "Remarks";
                x.VisibleIndex = 2;
                x.Width = 200;
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "FORMTYPE_SECGROUPNAME";
                x.Caption = "Security Group Name";
                x.VisibleIndex = 3;
                x.Width = 200;
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "FORMTYPE_ISACTIVE";
                x.Caption = "Active?";
                x.VisibleIndex = 4;
                x.Width = 200;
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "CREATEDBY";
                x.Caption = "Created By";
                x.VisibleIndex = 5;
                x.Width = 200;
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CREATEDON";
                x.Caption = "Created On";
                x.VisibleIndex = 6;
                x.Width = 200;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
            });
            //settings.Columns.Add(x =>
            //{
            //    x.FieldName = "MODIFIEDBY";
            //    x.Caption = "Updated By";
            //    x.VisibleIndex = 7;
            //    x.Width = 200;
            //});
            //settings.Columns.Add(x =>
            //{
            //    x.FieldName = "MODIFIEDON";
            //    x.Caption = "Updated On";
            //    x.VisibleIndex = 8;
            //    x.Width = 200;
            //    x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
            //});
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }


    }
}