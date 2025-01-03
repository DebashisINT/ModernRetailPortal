using BusinessLogicLayer.SalesmanTrack;
using DataAccessLayer;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using DevExpress.XtraExport;
using ModernRetail.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using UtilityLayer;

namespace ModernRetail.Controllers
{
    public class ProductRatesController : Controller
    {
        // GET: ProductRates
        ProductRateModel objdata = null;
        Int64 DetailsID = 0;
        string BranchName = string.Empty;
        public ProductRatesController()
        {
            objdata = new ProductRateModel();
        }

        public ActionResult ProductRatesList()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ProductRatesList", "ProductRates");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            objdata.branch_ID = 0;
            TempData["branch_ID"] = null;

            TempData.Keep();
            return View();
        }
        public ActionResult Index()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ProductRatesList", "ProductRates");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;


            if (TempData["branch_ID"] != null)
            {
                objdata.branch_ID = Convert.ToInt64(TempData["branch_ID"]);
                TempData.Keep();
            }

            if (TempData["IsView"] != null)
            {
                ViewBag.IsView = Convert.ToInt16(TempData["IsView"]);
                TempData["IsView"] = null;
                if (ViewBag.IsView == 0)
                {
                    ViewBag.PageTitle = "Modify Branch";
                }           
                else
                {
                    ViewBag.PageTitle = "Add Branch";
                }

            }
            else
            {
                ViewBag.IsView = 0;
                ViewBag.PageTitle = "Add Question";
            }            
            return View("~/Views/ProductRates/Index.cshtml", objdata);          
        }

        public JsonResult SetMapDataByID(Int64 ID = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["branch_ID"] = ID;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success, JsonRequestBehavior.AllowGet);
        }
        public DataSet GetListData()
        {
            DataSet dt = new DataSet();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
            proc.AddPara("@ACTION", "GETDETAILS");
            dt = proc.GetDataSet();
            return dt;
        }

        public JsonResult GetState(string CountryID)
        {

            DataTable DT = new DataTable();
            if (CountryID != "")
            {
                DT = GetStateData(CountryID);
            }

            return Json(APIHelperMethods.ToModelList<BranchList>(DT));

        }
        public DataTable GetStateData(string CountryID)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
            proc.AddPara("@ACTION", "GETSTATE");
            proc.AddBigIntegerPara("@countryId", Convert.ToInt64(CountryID));
            dt = proc.GetTable();
            return dt;
        }

        public JsonResult GETCITY(string StateID)
        {

            DataTable DT = new DataTable();
            if (StateID != "")
            {
                DT = GETCITYDATA(StateID);
            }

            return Json(APIHelperMethods.ToModelList<BranchList>(DT));

        }
        public DataTable GETCITYDATA(string StateID)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
            proc.AddPara("@ACTION", "GETCITY");
            proc.AddBigIntegerPara("@state_id", Convert.ToInt64(StateID));
            dt = proc.GetTable();
            return dt;
        }


        public JsonResult GETPINZIP(string CityID)
        {

            DataTable DT = new DataTable();
            if (CityID != "")
            {
                DT = GETPINZIPDATA(CityID);
            }

            return Json(APIHelperMethods.ToModelList<BranchList>(DT));

        }
        public DataTable GETPINZIPDATA(string CityID)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
            proc.AddPara("@ACTION", "GETPINZIP");
            proc.AddBigIntegerPara("@city_id", Convert.ToInt64(CityID));
            dt = proc.GetTable();
            return dt;
        }

        public string CheckUniqueShortName(string ShortName, string branch_ID)
        {
            string ShortNameFount = "0";

            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
            proc.AddPara("@ACTION", "CHECKUNIQUESHORTNAME");
            proc.AddPara("@branch_code", ShortName);
            proc.AddPara("@BRANCH_ID", branch_ID);
            proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
            dt = proc.GetTable();
            string output = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            ShortNameFount = output;

            return ShortNameFount;

        }

        [WebMethod]
        public JsonResult SaveBranch(BranchMaster Details)
        {
            String Message = "";
            Boolean Success = false;
            DataSet dt = new DataSet();



            if (Convert.ToInt64(Details.branch_ID) > 0 && Convert.ToInt16(TempData["IsView"]) == 0)
            {
                dt = objdata.BranchEntryInsertUpdate("UPDATEBRANCH", Convert.ToInt64(Details.branch_ID), Details.ShortName, Convert.ToInt64(Details.ParentBranch), Details.BranchName, Details.Address1,
                    Convert.ToInt64(Details.Country), Convert.ToInt64(Details.State), Convert.ToInt64(Details.City), Convert.ToInt64(Details.PIN)
                       , Convert.ToInt64(Session["MRuserid"]));
            }
            else
            {
                dt = objdata.BranchEntryInsertUpdate("INSERTBRANCH", Convert.ToInt64(Details.branch_ID), Details.ShortName, Convert.ToInt64(Details.ParentBranch), Details.BranchName, Details.Address1,
                    Convert.ToInt64(Details.Country), Convert.ToInt64(Details.State), Convert.ToInt64(Details.City), Convert.ToInt64(Details.PIN)
                       , Convert.ToInt64(Session["MRuserid"]));

            }


            if (dt != null && dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dt.Tables[0].Rows)
                {
                    Success = Convert.ToBoolean(row["Success"]);
                    DetailsID = Convert.ToInt64(row["DetailsID"]);
                    BranchName = Convert.ToString(Details.BranchName);
                }
            }

            String retuenMsg = Success + "~" + DetailsID + "~" + Details.BranchName + "~" + Message;
            return Json(retuenMsg);

        }

        public ActionResult PartialGridList(BranchMaster model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/BranchMasterList", "BranchMaster");
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
                sqlcmd = new SqlCommand("PRC_MR_PRODUCTRATES", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GETLISTINGDETAILS");
                sqlcmd.Parameters.Add("@USER_ID", Userid);
                sqlcmd.Parameters.Add("@ISPAGELOAD", model.Is_PageLoad);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                return PartialView("_PartialBranchMasterList", GetBranchDetailsList(Is_PageLoad));

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        public IEnumerable GetBranchDetailsList(string Is_PageLoad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
            string Userid = Convert.ToString(Session["MRuserid"]);
            if (Is_PageLoad != "Ispageload")
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_BRANCHDETAILSLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_BRANCHDETAILSLISTs
                        where d.SEQ == 0
                        select d;
                return q;
            }
        }


        public ActionResult ExporBranchList(int type)
        {
            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToXlsx(GetGridViewSettings(), GetBranchDetailsList(""));
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
            settings.SettingsExport.FileName = "Branch";

            settings.Columns.Add(x =>
            {
                x.FieldName = "BRANCH_CODE";
                x.Caption = "Short Name";
                x.VisibleIndex = 1;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "PARENTBRANCH";
                x.Caption = "Parent Branch";
                x.VisibleIndex = 2;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "BRANCHNAME";
                x.Caption = "Branch Name";
                x.VisibleIndex = 3;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "BRANCH_ADDRESS1";
                x.Caption = "Address";
                x.VisibleIndex = 4;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "BRANCH_COUNTRY";
                x.Caption = "Country";
                x.VisibleIndex = 5;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "BRANCH_STATE";
                x.Caption = "State";
                x.VisibleIndex = 6;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "BRANCH_CITY";
                x.Caption = "City / District";
                x.VisibleIndex = 7;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "BRANCH_PIN";
                x.Caption = "PIN";
                x.VisibleIndex = 8;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateUser";
                x.Caption = "Created By";
                x.VisibleIndex = 9;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateDate";
                x.Caption = "Created On";
                x.VisibleIndex = 10;
                x.Width = 120;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(15);
                x.ColumnType = MVCxGridViewColumnType.DateEdit;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                (x.PropertiesEdit as DateEditProperties).EditFormatString = "dd-MM-yyyy";

            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "ModifyUser";
                x.Caption = "Updated By";
                x.VisibleIndex = 11;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "ModifyDate";
                x.Caption = "Updated On";
                x.VisibleIndex = 12;
                x.Width = 120;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(11);
                x.ColumnType = MVCxGridViewColumnType.DateEdit;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                (x.PropertiesEdit as DateEditProperties).EditFormatString = "dd-MM-yyyy";

            });
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }


        public JsonResult EditBranch(string id)
        {
            DataSet output = new DataSet();
            output = objdata.EditBranch(id);

            if (output.Tables[0].Rows.Count > 0)
            {
                return Json(new
                {
                    ShortName = Convert.ToString(output.Tables[0].Rows[0]["branch_code"]),
                    ParentBranch = Convert.ToString(output.Tables[0].Rows[0]["branch_parentId"]),
                    BranchName = Convert.ToString(output.Tables[0].Rows[0]["branch_description"]),
                    Address1 = Convert.ToString(output.Tables[0].Rows[0]["branch_address"]),
                    Country = Convert.ToString(output.Tables[0].Rows[0]["branch_country"]),
                    State = Convert.ToString(output.Tables[0].Rows[0]["branch_state"]),
                    City = Convert.ToString(output.Tables[0].Rows[0]["branch_city"]),
                    PIN = Convert.ToString(output.Tables[0].Rows[0]["branch_pin"]),

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
            output = objdata.DeleteBranch(ID);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranch(string branch_ID)
        {

            DataTable DT = new DataTable();
            DT = GetBranchData(branch_ID);

            return Json(APIHelperMethods.ToModelList<BranchList>(DT));

        }
        public DataTable GetBranchData(string branch_ID)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
            proc.AddPara("@ACTION", "GETBRANCH");
            proc.AddBigIntegerPara("@BRANCH_ID", Convert.ToInt64(branch_ID));
            dt = proc.GetTable();
            return dt;
        }

        public JsonResult GETDESIGNATION()
        {
            DataTable DT = new DataTable();
            DT = GETDESIGNATIONData();
            return Json(APIHelperMethods.ToModelList<DESIGNATIONLIST>(DT));
        }
        public DataTable GETDESIGNATIONData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
            proc.AddPara("@ACTION", "GETDESIGNATION");
            dt = proc.GetTable();
            return dt;
        }

       
    }
}