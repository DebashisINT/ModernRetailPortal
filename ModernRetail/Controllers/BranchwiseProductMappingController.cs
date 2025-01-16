using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using DocumentFormat.OpenXml.Office2010.Excel;
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
using UtilityLayer;
using static ModernRetail.Models.BranchwiseProductMappingModel;
using DevExpress.XtraExport;
using System.Web.Services;

namespace ModernRetail.Controllers
{
    public class BranchwiseProductMappingController : Controller
    {
        // GET: BranchwiseProductMapping
        Int64 DetailsID = 0;
        BranchwiseProductMappingModel objdata = null;
        public BranchwiseProductMappingController()
        {
            objdata = new BranchwiseProductMappingModel();
        }
        CommonBL objSystemSettings = new CommonBL();
        public ActionResult Index()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/BWPListing", "BranchwiseProductMapping");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            if (TempData["PRODUCTBRANCHMAP_ID"] != null)
            {
                objdata.PRODUCTBRANCHMAP_ID = Convert.ToInt64(TempData["PRODUCTBRANCHMAP_ID"]);
                TempData.Keep();
            }



            
            //DataTable DT = new DataTable();
            //DataTable dtBranchChild = new DataTable();
            //DT = GetBranchheadoffice(Convert.ToString(Session["MRuserbranchHierarchy"]), "HO");
            //if (DT.Rows.Count > 0)
            //{
            //    dtBranchChild = GetChildBranch(Convert.ToString(Session["MRuserbranchHierarchy"]));
            //    if (dtBranchChild.Rows.Count > 0)
            //    {
            //        DT.Rows.Add(0, "All");
            //    }
            //}

            //List<BranchList> _BranchList = new List<BranchList>();
            //_BranchList = APIHelperMethods.ToModelList<BranchList>(DT);
            //objdata.HeadBranchList = _BranchList;
            objdata.HeadBranchID = 0;

            return View(objdata);
        }
        public ActionResult BWPListing()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/BWPListing", "BranchwiseProductMapping");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            TempData["PRODUCTBRANCHMAP_ID"] = null;
            TempData.Keep();
            return View();
        }
        public JsonResult GetHeadBranch()
        {

            DataTable DT = new DataTable();
            DataTable dtBranchChild = new DataTable();
            DT = GetBranchheadoffice(Convert.ToString(Session["MRuserbranchHierarchy"]), "HO");
            if (DT.Rows.Count > 0)
            {
                dtBranchChild = GetChildBranch(Convert.ToString(Session["MRuserbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    DT.Rows.Add(0,"All");                    
                }
            }

            return Json(APIHelperMethods.ToModelList<BranchList>(DT));

        }            
        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
            SqlConnection Connection = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            Connection.Dispose();
            return dt;
        }
        public DataTable GetBranchheadoffice(string CHILDBRANCH, string Action)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Get_AllbranchHO");
            proc.AddPara("@CHILDBRANCH", CHILDBRANCH);
            proc.AddPara("@Ation", Action);
            ds = proc.GetTable();
            return ds;
        }
        public ActionResult GetBranchListJson(string headBranch)
        {
            try
            {
                //string IsActivateEmployeeBranchHierarchy = objSystemSettings.GetSystemSettingsResult("IsActivateEmployeeBranchHierarchy");
                //ViewBag.IsActivateEmployeeBranchHierarchy = IsActivateEmployeeBranchHierarchy;

                DataTable dtbranch = new DataTable();
                List<GetBranchList> modelbranch = new List<GetBranchList>();
                //if (headBranch != null)
                //{
                //    if (headBranch != "0")
                //    {
                //        dtbranch = GetBranch(Convert.ToString(Session["userbranchHierarchy"]), headBranch);
                //    }
                //    else if (IsActivateEmployeeBranchHierarchy == "0")
                //    {
                //        ProcedureExecute proc = new ProcedureExecute("PRC_FSMBRANCHWISEPRODUCTMAPPING");
                //        proc.AddVarcharPara("@Action", 50, "FETCHBRANCHS");
                //        proc.AddIntegerPara("@USERID", Convert.ToInt32(Session["MRuserid"]));
                //        dtbranch = proc.GetTable();
                //    }
                //    else
                //    {
                //        dtbranch = GetBranchData();
                //    }
                //}
                //else
                //{
                //    dtbranch = GetBranchData();
                //}

                dtbranch = GetBranchData();
                modelbranch = APIHelperMethods.ToModelList<GetBranchList>(dtbranch);
                return Json(modelbranch, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult GetBranchList(BranchwiseProductMappingModel values)
        {
            try
            {
                string IsActivateEmployeeBranchHierarchy = objSystemSettings.GetSystemSettingsResult("IsActivateEmployeeBranchHierarchy");
                ViewBag.IsActivateEmployeeBranchHierarchy = IsActivateEmployeeBranchHierarchy;


                DataTable dtbranch=new DataTable();
                List<GetBranchList> modelbranch = new List<GetBranchList>();

                //if(values.HeadBranch!=null)
                //{
                //    if (values.HeadBranch != "0")
                //    {
                //        dtbranch = GetBranch(Convert.ToString(Session["userbranchHierarchy"]), values.HeadBranch);
                //    }
                //    else if (IsActivateEmployeeBranchHierarchy == "0")
                //    {
                //        ProcedureExecute proc = new ProcedureExecute("PRC_FSMBRANCHWISEPRODUCTMAPPING");
                //        proc.AddVarcharPara("@Action", 50, "FETCHBRANCHS");
                //        proc.AddIntegerPara("@USERID", Convert.ToInt32(Session["MRuserid"]));
                //        dtbranch = proc.GetTable();
                //    }
                //    else
                //    {
                //        dtbranch = GetBranchData();
                //    }
                //}
                
                //else
                //{
                    //dtbranch = GetBranchData();
                //}
                dtbranch = GetBranchData();
                modelbranch = APIHelperMethods.ToModelList<GetBranchList>(dtbranch);



                List<GetBranchList> productdata = new List<GetBranchList>();
                GetBranchList dataobj = new GetBranchList();
               
                DataSet output = objdata.FETCHBRANCHMAP("FETCHBRANCHMAP", values.PRODUCTBRANCHMAP_ID);
              
                if (output != null && output.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in output.Tables[0].Rows)
                    {
                        dataobj = new GetBranchList();
                        dataobj.branch_id = Convert.ToInt64(row["branch_id"]);
                        productdata.Add(dataobj);

                    }
                    ViewBag.branch_id = productdata;


                }

                return PartialView("PartialBranch", modelbranch);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        public DataTable GetBranchData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
            proc.AddPara("@ACTION", "GETBRANCH");           
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();            
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }
        public ActionResult GetParentEmployeeList(BranchwiseProductMappingModel values)
        {
            try
            {
                //DataTable dt = new DataTable();
                List<GETSTORELIST> model = new List<GETSTORELIST>();
                DataTable ComponentTable = new DataTable();

                if (values.Branch_Ids != null)
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                    proc.AddVarcharPara("@Action", 50, "GETSTORE");
                    proc.AddVarcharPara("@BRANCHID", 4000, values.Branch_Ids);
                    ComponentTable = proc.GetTable();
                }
                else if (values.PRODUCTBRANCHMAP_ID != 0)
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                    proc.AddVarcharPara("@Action", 50, "GETSTOREEDITMODE");
                    proc.AddBigIntegerPara("@PRODUCTBRANCHMAP_ID",values.PRODUCTBRANCHMAP_ID);
                    ComponentTable = proc.GetTable();
                }



                model = APIHelperMethods.ToModelList<GETSTORELIST>(ComponentTable);



                List<GETSTORELIST> productdata = new List<GETSTORELIST>();
                GETSTORELIST dataobj = new GETSTORELIST();

                DataSet output = objdata.FETCHBRANCHMAP("FETCHBRANCHMAP", values.PRODUCTBRANCHMAP_ID);

                if (output != null && output.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in output.Tables[0].Rows)
                    {
                        dataobj = new GETSTORELIST();
                        dataobj.STORE_ID = Convert.ToString(row["STORE_ID"]);
                        productdata.Add(dataobj);

                    }
                    ViewBag.PARENTEMP_USERID = productdata;


                }

                return PartialView("PartialParentEmployee", model);
            }
            catch
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult GetParentEmployeeListJson(string Branch_Ids)
        {
            try
            {
                List<GetParentEmployeeList> model = new List<GetParentEmployeeList>();
                DataTable ComponentTable = new DataTable();

                if (Branch_Ids != null)
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                    proc.AddVarcharPara("@Action", 50, "FETCHPARENTEMPLOYEE");
                    proc.AddVarcharPara("@BRANCHID", 4000, Branch_Ids);
                    ComponentTable = proc.GetTable();
                }
                model = APIHelperMethods.ToModelList<GetParentEmployeeList>(ComponentTable);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult GetChildEmployeeList(BranchwiseProductMappingModel values)
        {
            try
            {
                DataTable dt = new DataTable();
                List<GetParentEmployeeList> model = new List<GetParentEmployeeList>();
                DataTable ComponentTable = new DataTable();


                 if (values.PRODUCTBRANCHMAP_ID != 0)
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                    proc.AddVarcharPara("@Action", 50, "FETCHCHILDEMPLOYEEEDITMODE");
                    proc.AddBigIntegerPara("@PRODUCTBRANCHMAP_ID", values.PRODUCTBRANCHMAP_ID);
                    ComponentTable = proc.GetTable();
                }
                else if (values.PARENTERMP_IDS ==null)
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                    proc.AddVarcharPara("@Action", 50, "FETCHCHILDEMPLOYEE");
                    proc.AddVarcharPara("@BRANCHID", 4000, values.Branch_Ids);
                    proc.AddVarcharPara("@ParentEMPID", 4000, values.PARENTERMP_IDS);
                    ComponentTable = proc.GetTable();                   

                }
                else if (values.Branch_Ids != null)
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                    proc.AddVarcharPara("@Action", 50, "FETCHCHILDEMPLOYEE");
                    proc.AddVarcharPara("@BRANCHID", 4000, values.Branch_Ids);
                    proc.AddVarcharPara("@ParentEMPID", 4000, values.PARENTERMP_IDS);
                    ComponentTable = proc.GetTable();                    
                }
                

                model = APIHelperMethods.ToModelList<GetParentEmployeeList>(ComponentTable);



                List<GetParentEmployeeList> productdata = new List<GetParentEmployeeList>();
                GetParentEmployeeList dataobj = new GetParentEmployeeList();

                DataSet output = objdata.FETCHBRANCHMAP("FETCHBRANCHMAP", values.PRODUCTBRANCHMAP_ID);

                if (output != null && output.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in output.Tables[0].Rows)
                    {
                        dataobj = new GetParentEmployeeList();
                        dataobj.USER_ID = Convert.ToInt64(row["CHILDEMP_USERID"]);
                        productdata.Add(dataobj);

                    }
                    ViewBag.CHILDEMP_USERID = productdata;
                }

                return PartialView("PartialChildEmployee", model);
            }
            catch
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult GetChildEmployeeListJson(string PARENTERMP_IDS, string Branch_Ids)
        {
            try
            {
                DataTable dt = new DataTable();
                List<GetParentEmployeeList> model = new List<GetParentEmployeeList>();
                DataTable ComponentTable = new DataTable();

                if (PARENTERMP_IDS == "")
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                    proc.AddVarcharPara("@Action", 50, "FETCHCHILDEMPLOYEE");
                    proc.AddVarcharPara("@BRANCHID", 4000, Branch_Ids);
                    proc.AddVarcharPara("@ParentEMPID", 4000, PARENTERMP_IDS);
                    ComponentTable = proc.GetTable();

                }
                else
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                    proc.AddVarcharPara("@Action", 50, "FETCHCHILDEMPLOYEE");
                    proc.AddVarcharPara("@BRANCHID", 4000, Branch_Ids);
                    proc.AddVarcharPara("@ParentEMPID", 4000, PARENTERMP_IDS);
                    ComponentTable = proc.GetTable();
                }

                model = APIHelperMethods.ToModelList<GetParentEmployeeList>(ComponentTable);

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult GetProductList(BranchwiseProductMappingModel values)
        {
            try
            {
                DataTable dt = new DataTable();
                List<GetProductList> model = new List<GetProductList>();
                DataTable ComponentTable = new DataTable();
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                proc.AddVarcharPara("@Action", 50, "FETCHPRODUCTS");
                ComponentTable = proc.GetTable();              

                model = APIHelperMethods.ToModelList<GetProductList>(ComponentTable);





                List<GetProductList> productdata = new List<GetProductList>();
                GetProductList dataobj = new GetProductList();
                DataSet output = objdata.FETCHBRANCHMAP("FETCHBRANCHMAP", values.PRODUCTBRANCHMAP_ID);

                if (output != null && output.Tables[1].Rows.Count > 0)
                {

                    foreach (DataRow row in output.Tables[1].Rows)
                    {
                        dataobj = new GetProductList();
                        dataobj.SPRODUCTS_ID = Convert.ToInt64(row["PRODUCT_ID"]);
                        productdata.Add(dataobj);

                    }
                    ViewBag.PRODUCTS_ID = productdata;
                }

                return PartialView("PartialProduct", model);
            }
            catch
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult GetProductListJson()
        {
            try
            {
                DataTable dt = new DataTable();
                List<GetProductList> model = new List<GetProductList>();
                DataTable ComponentTable = new DataTable();
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
                proc.AddVarcharPara("@Action", 50, "FETCHPRODUCTS");
                ComponentTable = proc.GetTable();

                model = APIHelperMethods.ToModelList<GetProductList>(ComponentTable);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public JsonResult SetMapDataByID(Int64 ID = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["PRODUCTBRANCHMAP_ID"] = ID;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PartialGridList(BranchwiseProductMappingModel model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/BWPListing", "BranchwiseProductMapping");
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
                sqlcmd = new SqlCommand("PRC_MR_BRANCHWISEPRODUCTMAPPING", sqlcon);
                sqlcmd.Parameters.AddWithValue("@ACTION", "GETLISTINGDETAILS");
                sqlcmd.Parameters.AddWithValue("@UserId", Userid);
                sqlcmd.Parameters.AddWithValue("@ISPAGELOAD", model.Is_PageLoad);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                return PartialView("PartialBWPMListing", GetDetailsList(Is_PageLoad));

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
                var q = from d in dc.MR_PRODUCTBRANCHMAPLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_PRODUCTBRANCHMAPLISTs
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
            settings.SettingsExport.FileName = "BranchwiseProductMapping";

            settings.Columns.Add(x =>
            {
                x.FieldName = "BRANCHNAME";
                x.Caption = "Branch";
                x.VisibleIndex = 1;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "PARENTEMP_NAME";
                x.Caption = "Store";
                x.VisibleIndex = 2;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);

            });
            //settings.Columns.Add(x =>
            //{
            //    x.FieldName = "CHILDEMP_NAME";
            //    x.Caption = "Child Employee";
            //    x.VisibleIndex = 3;
            //    x.ColumnType = MVCxGridViewColumnType.TextBox;
            //    x.Width = System.Web.UI.WebControls.Unit.Pixel(250);

            //});
            settings.Columns.Add(x =>
            {
                x.FieldName = "PRODUCT_CODE";
                x.Caption = "Item Code";
                x.VisibleIndex = 4;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "PRODUCTS_NAME";
                x.Caption = "Item Name";
                x.VisibleIndex = 5;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);

            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateUser";
                x.Caption = "Created By";
                x.VisibleIndex = 6;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateDate";
                x.Caption = "Created On";
                x.VisibleIndex = 7;
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
                x.VisibleIndex = 8;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "ModifyDate";
                x.Caption = "Updated On";
                x.VisibleIndex = 9;
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

        [WebMethod]
        public JsonResult SaveBranchProductMap(BranchwiseProductMappingModel Details)
        {
            String Message = "";
            Boolean Success = false;
            DataSet dt = new DataSet();

            if (Convert.ToInt64(Details.PRODUCTBRANCHMAP_ID) > 0 && Convert.ToInt16(TempData["IsView"]) == 0)
            {
                dt = objdata.InsertUpdate("EDIT", Convert.ToInt64(Details.PRODUCTBRANCHMAP_ID), Details.Branch_Ids, Details.PARENTERMP_IDS, Details.CHILDERMP_IDS, Details.PRODUCT_IDS
               , Convert.ToInt64(Session["MRuserid"]));
            }
            else
            {
                dt = objdata.InsertUpdate("Add", Convert.ToInt64(Details.PRODUCTBRANCHMAP_ID), Details.Branch_Ids, Details.PARENTERMP_IDS, Details.CHILDERMP_IDS, Details.PRODUCT_IDS
                , Convert.ToInt64(Session["MRuserid"]));
            }


            if (dt != null && dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dt.Tables[0].Rows)
                {
                    Success = Convert.ToBoolean(row["Success"]);
                    DetailsID = Convert.ToInt64(row["DetailsID"]);
                    
                }
            }

            String returnMsg = Success + "~" + DetailsID  + "~" + Message;
            
            return Json(returnMsg);

        }

        public JsonResult Delete(string ID)
        {
            int output = 0;
            output = objdata.Delete(ID);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

    }
}