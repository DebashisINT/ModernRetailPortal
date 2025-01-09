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
using System.Data.OleDb;
using System.IO;

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

            objdata.ID = 0;
            TempData["ID"] = null;

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


            if (TempData["ID"] != null)
            {
                objdata.ID = Convert.ToInt64(TempData["ID"]);
                TempData.Keep();
            }

            //if (TempData["IsView"] != null)
            //{
            //    ViewBag.IsView = Convert.ToInt16(TempData["IsView"]);
            //    TempData["IsView"] = null;
            //    if (ViewBag.IsView == 0)
            //    {
            //        ViewBag.PageTitle = "Modify Branch";
            //    }           
            //    else
            //    {
            //        ViewBag.PageTitle = "Add Branch";
            //    }

            //}
            //else
            //{
            //    ViewBag.IsView = 0;
            //    ViewBag.PageTitle = "Add Question";
            //}            
            return View("~/Views/ProductRates/Index.cshtml", objdata);          
        }

        public JsonResult SetMapDataByID(Int64 ID = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["ID"] = ID;
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

        

        [WebMethod]
        public JsonResult SaveProductRates(ProductRateModel Details)
        {
            String Message = "";
            Boolean Success = false;
            DataSet dt = new DataSet();



            if (Convert.ToInt64(Details.ID) > 0 && Convert.ToInt16(TempData["IsView"]) == 0)
            {
                dt = objdata.ProductRateEntryInsertUpdate("UPDATEPRODUCTRATE", Convert.ToInt64(Details.ID),Convert.ToInt64(Details.branch_ID), Convert.ToInt32(Details.Designation), Convert.ToInt64(Details.Employee), Convert.ToInt64(Details.Product),
                    Convert.ToString(Details.SpecialPrice), Convert.ToInt64(Session["MRuserid"]));

            }
            else
            {
                dt = objdata.ProductRateEntryInsertUpdate("INSERTPRODUCTRATE", Convert.ToInt64(Details.ID), Convert.ToInt64(Details.branch_ID), Convert.ToInt32(Details.Designation), Convert.ToInt64(Details.Employee), Convert.ToInt64(Details.Product),
                    Convert.ToString(Details.SpecialPrice), Convert.ToInt64(Session["MRuserid"]));

            }


            if (dt != null && dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dt.Tables[0].Rows)
                {
                    Success = Convert.ToBoolean(row["Success"]);
                    DetailsID = Convert.ToInt64(row["DetailsID"]);
                   // BranchName = Convert.ToString(Details.BranchName);
                }
            }

            String retuenMsg = Success + "~" + DetailsID + "~" + "~" + Message;
            return Json(retuenMsg);

        }

        public ActionResult PartialGridList(ProductRateModel model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ProductRatesList", "ProductRates");
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
                return PartialView("PartialProductRatesList", GetDetailsList(Is_PageLoad));

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
                var q = from d in dc.MR_PRODUCTRATESDETAILSLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_PRODUCTRATESDETAILSLISTs
                        where d.SEQ == 0
                        select d;
                return q;
            }
        }


        public ActionResult ExportList(int type)
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
            settings.SettingsExport.FileName = "ProductRate";

            settings.Columns.Add(x =>
            {
                x.FieldName = "BRANCH";
                x.Caption = "Branch";
                x.VisibleIndex = 1;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "EMP_NAME";
                x.Caption = "Employee";
                x.VisibleIndex = 2;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "PRODUCT_CODE";
                x.Caption = "Item Code";
                x.VisibleIndex = 3;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "PRODUCT_NAME";
                x.Caption = "Item Name";
                x.VisibleIndex = 4;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "STORE_RATE";
                x.Caption = "Special Price";
                x.VisibleIndex = 5;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);

            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateUser";
                x.Caption = "Created By";
                x.VisibleIndex = 9;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateDate";
                x.Caption = "Created On";
                x.VisibleIndex = 10;
                x.Width = 120;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
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
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "ModifyDate";
                x.Caption = "Updated On";
                x.VisibleIndex = 12;
                x.Width = 120;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
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


        public JsonResult EditProductRate(string id)
        {
            DataSet output = new DataSet();
            output = objdata.EditProductRate(id);

            if (output.Tables[0].Rows.Count > 0)
            {
                return Json(new
                {
                    PRODUCT_ID = Convert.ToString(output.Tables[0].Rows[0]["PRODUCT_ID"]),
                    sProducts_Name = Convert.ToString(output.Tables[0].Rows[0]["sProducts_Name"]),
                    BRANCH_ID = Convert.ToString(output.Tables[0].Rows[0]["BRANCH_ID"]),
                    DESIGID = Convert.ToString(output.Tables[0].Rows[0]["DESIGID"]),
                    USER_ID = Convert.ToString(output.Tables[0].Rows[0]["USER_ID"]),
                    USER_NAME = Convert.ToString(output.Tables[0].Rows[0]["USER_NAME"]),
                    STORE_RATE = Convert.ToString(output.Tables[0].Rows[0]["STORE_RATE"]),
                

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
           // proc.AddBigIntegerPara("@BRANCH_ID", Convert.ToInt64(branch_ID));
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

        public ActionResult DownloadFormat()
        {
            string FileName = "ProductRatesList.xlsx";
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "image/jpeg";
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
            response.TransmitFile(Server.MapPath("~/Commonfolder/ProductRatesList.xlsx"));
            response.Flush();
            response.End();

            return null;
        }       

        public ActionResult ImportExcel()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        String extension = Path.GetExtension(fname);
                        fname = DateTime.Now.Ticks.ToString() + extension;
                        fname = Path.Combine(Server.MapPath("~/Temporary/"), fname);
                        file.SaveAs(fname);
                        Import_To_Grid(fname, extension, file);
                    }
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public Int32 Import_To_Grid(string FilePath, string Extension, HttpPostedFileBase file)
        {
            Boolean Success = false;
            Int32 HasLog = 0;

            if (file.FileName.Trim() != "")
            {
                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    DataTable dt = new DataTable();
                    string conString = string.Empty;
                    conString = ConfigurationManager.AppSettings["ExcelConString"];
                    conString = string.Format(conString, FilePath);
                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = "Sheet1"; //ī;

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dt);
                        }
                        excel_con.Close();
                    }

                    // }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                       
                        DataTable dtExcelData = new DataTable();
                        dtExcelData.Columns.Add("BRANCH", typeof(string));
                        dtExcelData.Columns.Add("ITEM CODE", typeof(string));
                        dtExcelData.Columns.Add("ITEM NAME", typeof(string));
                        dtExcelData.Columns.Add("SPECIAL PRICE", typeof(string));
                        dtExcelData.Columns.Add("USER LOGIN ID", typeof(string));
                      
                        foreach (DataRow row in dt.Rows)
                        {
                            if (Convert.ToString(row["ITEM CODE"]) != "" && Convert.ToString(row["ITEM NAME"]) != "")
                            {

                                if (Convert.ToString(row["SPECIAL PRICE"]) == "")
                                    row["SPECIAL PRICE"] = "0.00";

                                

                                dtExcelData.Rows.Add(Convert.ToString(row["BRANCH"]), Convert.ToString(row["ITEM CODE"]),
                                                Convert.ToString(row["ITEM NAME"]),
                                                Convert.ToString(row["SPECIAL PRICE"]), Convert.ToString(row["USER LOGIN ID"])
                                               );
                            }

                        }

                        try
                        {
                            TempData["ProductRateImportLog"] = dtExcelData;
                            TempData.Keep();

                            DataTable dtCmb = new DataTable();
                            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
                            proc.AddPara("@ACTION", "IMPORTPRODUCTRATE");
                            proc.AddPara("@IMPORT_TABLE", dtExcelData);
                            proc.AddPara("@USER_ID", Convert.ToInt32(Session["MRuserid"]));
                            dtCmb = proc.GetTable();

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return HasLog;
        }
        [HttpPost]
        public JsonResult ProductImportLog(string Fromdt, String ToDate)
        {
            string output_msg = string.Empty;
            try
            {
                string datfrmat = Fromdt.Split('-')[2] + '-' + Fromdt.Split('-')[1] + '-' + Fromdt.Split('-')[0];
                string dattoat = ToDate.Split('-')[2] + '-' + ToDate.Split('-')[1] + '-' + ToDate.Split('-')[0];

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
                proc.AddPara("@ACTION", "GETPRODUCTIMPORTLOG");
                proc.AddPara("@FromDate", datfrmat);
                proc.AddPara("@ToDate", dattoat);
                dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    TempData["ProductRateImportLog"] = dt;
                    TempData.Keep();
                    output_msg = "True";
                }
                else
                {
                    output_msg = "Log not found.";
                }
            }
            catch (Exception ex)
            {
                output_msg = "Please try again later";
            }
            return Json(output_msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ImportLog()
        {
            List<ProductRateImportLog> list = new List<ProductRateImportLog>();
            DataTable dt = new DataTable();
            try
            {
                dt = (DataTable)TempData["ProductRateImportLog"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    ProductRateImportLog data = null;
                    foreach (DataRow row in dt.Rows)
                    {
                        data = new ProductRateImportLog();
                        data.BRANCH = Convert.ToString(row["BRANCH"]);
                        data.ItemCode = Convert.ToString(row["ItemCode"]);
                        data.ItemName = Convert.ToString(row["ItemName"]);                    
                       
                        data.SPECIALPRICE = Convert.ToDecimal(row["SPECIALPRICE"]);
                        data.USERLOGINID = Convert.ToString(row["USERLOGINID"]);                      
                        data.ImportStatus = Convert.ToString(row["ImportStatus"]);
                        data.ImportMsg = Convert.ToString(row["ImportMsg"]);
                        data.ImportDate = Convert.ToDateTime(row["ImportDate"]);
                        data.UpdatedBy = Convert.ToString(row["UpdatedBy"]);

                        list.Add(data);
                    }
                    TempData["ProductRateImportLog"] = dt;
                }
            }
            catch (Exception ex)
            {

            }
            TempData.Keep();
            return PartialView(list);
        }

    }
}