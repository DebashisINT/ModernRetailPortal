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
using UtilityLayer;
using DataAccessLayer;
using System.Web.Services;
using BusinessLogicLayer;
using System.Web.UI.WebControls;
using DevExpress.XtraRichEdit.Import.Html;

namespace ModernRetail.Controllers
{
    public class UserConfigurationController : Controller
    {
        // GET: UserConfiguration
        UserMasterModel objdata = null;
        Int64 DetailsID = 0;
        string UserName = string.Empty;
        public UserConfigurationController()
        {
            objdata = new UserMasterModel();
        }

        public ActionResult UserMasterList()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/UserMasterList", "UserConfiguration");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            objdata.user_id = 0;
            TempData["user_id"] = null;

            TempData.Keep();
            return View();
        }

        public ActionResult Index()
        {
            if (TempData["user_id"] != null)
            {
                objdata.user_id = Convert.ToInt64(TempData["user_id"]);
                TempData.Keep();

            }

            if (TempData["IsView"] != null)
            {
                ViewBag.IsView = Convert.ToInt16(TempData["IsView"]);
                TempData["IsView"] = null;
                if (ViewBag.IsView == 0)
                {
                    ViewBag.PageTitle = "Modify User";
                }
                else
                {
                    ViewBag.PageTitle = "Add User";
                }

            }

            DataSet dt = new DataSet();
            dt = GetListData();

            if (dt != null)
            {
                List<BranchList> BranchList = new List<BranchList>();
                BranchList = APIHelperMethods.ToModelList<BranchList>(dt.Tables[0]);
                objdata.BranchList = BranchList;

                List<DepartmentList> DepartmentList = new List<DepartmentList>();
                DepartmentList = APIHelperMethods.ToModelList<DepartmentList>(dt.Tables[1]);
                objdata.DepartmentList = DepartmentList;

                List<DesignationList> DesignationList = new List<DesignationList>();
                DesignationList = APIHelperMethods.ToModelList<DesignationList>(dt.Tables[2]);
                objdata.DesignationList = DesignationList;

                List<GroupList> GroupList = new List<GroupList>();
                GroupList = APIHelperMethods.ToModelList<GroupList>(dt.Tables[3]);
                objdata.GroupList = GroupList;

                List<CountryList> CountryList = new List<CountryList>();
                CountryList = APIHelperMethods.ToModelList<CountryList>(dt.Tables[4]);
                objdata.CountryList = CountryList;

              

            }


            return View("~/Views/UserConfiguration/Index.cshtml", objdata);
        }


        public DataSet GetListData()
        {
            DataSet dt = new DataSet();

            ProcedureExecute proc = new ProcedureExecute("Prc_MR_UserAccountData");
            proc.AddPara("@ACTION", "GETDETAILS");
            dt = proc.GetDataSet();
            return dt;
        }


        public ActionResult PartialGridList(UserMasterModel model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/UserMasterList", "UserConfiguration");
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;

                CommonBL cbl = new CommonBL();
                string DefaultBranchInLogin = cbl.GetSystemSettingsResult("IsActivateEmployeeBranchHierarchy");
                ViewBag.ActivateEmployeeBranchHierarchy = DefaultBranchInLogin;

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
                sqlcmd = new SqlCommand("Prc_MR_UserAccountData", sqlcon);
                sqlcmd.Parameters.AddWithValue("@ACTION", "SHOWLISTINGDATA");
                sqlcmd.Parameters.AddWithValue("@USER_ID", Userid);
                sqlcmd.Parameters.AddWithValue("@ISPAGELOAD", model.Is_PageLoad);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                return PartialView("_PartialUserMasterList", GetUserList(Is_PageLoad));

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        public IEnumerable GetUserList(string Is_PageLoad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
            string Userid = Convert.ToString(Session["MRuserid"]);
            if (Is_PageLoad != "Ispageload")
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_USERLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_USERLISTs
                        where d.SEQ == 0
                        select d;
                return q;
            }
        }


        public ActionResult ExporUserList(int type)
        {
            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToXlsx(GetGridViewSettings(), GetUserList(""));
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
            settings.SettingsExport.FileName = "Userlist";

            settings.Columns.Add(x =>
            {
                x.FieldName = "USER_LOGINID";
                x.Caption = "User ID";
                x.VisibleIndex = 1;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "USER_NAME";
                x.Caption = "User Name";
                x.VisibleIndex = 2;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "DEPARTMENTNAME";
                x.Caption = "Department";
                x.VisibleIndex = 3;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "DESIGNATIONNAME";
                x.Caption = "Designation";
                x.VisibleIndex = 4;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "REPORTTONAME";
                x.Caption = "Report To";
                x.VisibleIndex = 5;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "GROUPNAME";
                x.Caption = "Group";
                x.VisibleIndex = 6;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "USER_INACTIVE";
                x.Caption = "Active";
                x.VisibleIndex = 7;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        public string CheckUniqueUserLoginId(string LoginId, string User_ID)
        {
            string UserLoginIdFount = "0";

            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_USERACCOUNTDATA");
            proc.AddPara("@ACTION", "CHECKUNIQUEUSERLOGINID");
            proc.AddPara("@LOGINID", LoginId);
            proc.AddPara("@USERID", User_ID);
            proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
            dt = proc.GetTable();
            string output = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            UserLoginIdFount = output;

            return UserLoginIdFount;

        }

        [WebMethod]
        public JsonResult SaveUser(UserMasterAddEditModel Details)
        {
            String Message = "";
            Boolean Success = false;
            string Action = "";

            if(Details.UserId > 0 && Convert.ToInt16(TempData["IsView"]) == 0)
            {
                Action = "UPDATEUSER";
            }
            else
            {
                Action = "ADDUSER";
            }

            string PartyTypeId = "";
            int k = 1;

            Encryption epasswrd = new Encryption();
            var Encryptpass = epasswrd.Encrypt(Details.Password.Trim());

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_USERACCOUNTDATA");

            proc.AddPara("@ACTION", Action);
            proc.AddPara("@USERID", Details.UserId);
            proc.AddPara("@USERNAME", Details.UserName);
            proc.AddPara("@LOGINID", Details.LoginId);
            proc.AddPara("@PASSWORD", Encryptpass);
            proc.AddPara("@BRANCH", Details.Branch);
            proc.AddPara("@DEPARTMENT", Details.Department);
            proc.AddPara("@DESIGNATION", Details.Designation);
            proc.AddPara("@REPORTTO", Details.ReportTo);
            proc.AddPara("@GROUP", Details.Group);
            proc.AddPara("@ADDRESS", Details.Address);
            proc.AddPara("@COUNTRY", Details.Country);
            proc.AddPara("@STATE", Details.State);
            proc.AddPara("@CITY", Details.City);
            proc.AddPara("@PIN", Details.PIN);
            proc.AddPara("@PHONE", Details.Phone);
            proc.AddPara("@EMAIL", Details.Email);
            proc.AddPara("@ISACTIVE", Details.IsActive);
            proc.AddPara("@USER_ID", Convert.ToInt64(Session["MRuserid"]));
            proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
            ds = proc.GetDataSet();

            string output = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            return Json(output);

        }

        public JsonResult SetMapDataByID(Int64 ID = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["user_id"] = ID;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditUser(string id)
        {
            DataSet dt = new DataSet();

            ProcedureExecute proc = new ProcedureExecute("Prc_MR_UserAccountData");
            proc.AddPara("@ACTION", "EDITUSER");
            proc.AddPara("@USERID", id);

            dt = proc.GetDataSet();

            if (dt.Tables[0].Rows.Count > 0)
            {
               
                Encryption epasswrd = new Encryption();
                string Encryptpass = epasswrd.Decrypt(dt.Tables[0].Rows[0]["USER_PASSWORD"].ToString().Trim());

                var strBRANCH = "";

                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    if (strBRANCH == "")
                    {
                        strBRANCH = Convert.ToString(dt.Tables[0].Rows[i]["BRANCH_ID"]);
                    }
                    else
                    {
                        strBRANCH = strBRANCH + "," + Convert.ToString(dt.Tables[0].Rows[i]["BRANCH_ID"]);
                    }
                    
                }

                return Json(new
                {
                    USER_ID = Convert.ToString(dt.Tables[0].Rows[0]["USER_ID"]),
                    USER_NAME = Convert.ToString(dt.Tables[0].Rows[0]["USER_NAME"]),
                    USER_LOGINID = Convert.ToString(dt.Tables[0].Rows[0]["USER_LOGINID"]),
                    USER_PASSWORD = Convert.ToString(Encryptpass),
                    USER_DEPTID = Convert.ToString(dt.Tables[0].Rows[0]["USER_DEPTID"]),
                    USER_DEGID = Convert.ToString(dt.Tables[0].Rows[0]["USER_DEGID"]),
                    USER_REPORTTO_ID = Convert.ToString(dt.Tables[0].Rows[0]["USER_REPORTTO_ID"]),
                    USER_REPORTTONAME = Convert.ToString(dt.Tables[0].Rows[0]["REPORTTONAME"]),
                    USER_GROUPID = Convert.ToString(dt.Tables[0].Rows[0]["USER_GROUPID"]),
                    USER_ADDRESS = Convert.ToString(dt.Tables[0].Rows[0]["USER_ADDRESS"]),
                    USER_COUNTRYID = Convert.ToString(dt.Tables[0].Rows[0]["USER_COUNTRYID"]),
                    USER_STATEID = Convert.ToString(dt.Tables[0].Rows[0]["USER_STATEID"]),
                    USER_CITYID = Convert.ToString(dt.Tables[0].Rows[0]["USER_CITYID"]),
                    USER_PINID = Convert.ToString(dt.Tables[0].Rows[0]["USER_PINID"]),
                    USER_PHONE = Convert.ToString(dt.Tables[0].Rows[0]["USER_PHONE"]),
                    USER_EMAIL = Convert.ToString(dt.Tables[0].Rows[0]["USER_EMAIL"]),
                    ISACTIVE = Convert.ToString(dt.Tables[0].Rows[0]["USER_ISACTIVE"]),
                    BRANCH_LIST = strBRANCH

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { name = "" }, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetStateList(string user_id)
        {

            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_USERACCOUNTDATA");
            proc.AddPara("@ACTION", "GETSTATELIST");
            proc.AddPara("@USERID", user_id);
            dt = proc.GetTable();

            return Json(APIHelperMethods.ToModelList<StateList>(dt));

        }

        [WebMethod]
        public string GetStateListSubmit(string user_id, List<string> Statelist)
        {
            string StateId = "";
            int i = 1;

            if (Statelist != null && Statelist.Count > 0)
            {
                foreach (string item in Statelist)
                {
                    if (item == "0")
                    {
                        StateId = "0";
                        break;
                    }
                    else
                    {
                        if (i > 1)
                            StateId = StateId + "," + item;
                        else
                            StateId = item;
                        i++;
                    }
                }

            }


            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_USERACCOUNTDATA");
            proc.AddPara("@ACTION", "STATELISTSUBMIT");
            proc.AddPara("@USERID", user_id);
            proc.AddPara("@STATELIST", StateId);
            proc.AddPara("@USER_ID", Convert.ToString(Session["MRuserid"]));
            dt = proc.GetTable();

            return "Success";

        }


        public JsonResult GetBranchList(string user_id)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_USERACCOUNTDATA");
            proc.AddPara("@ACTION", "GETBRANCHLIST");
            proc.AddPara("@USERID", user_id);
            dt = proc.GetTable();

            return Json(APIHelperMethods.ToModelList<BranchMapList>(dt));
        }

        [WebMethod]
        public string GetBranchListSubmit(string user_id, List<string> Branchlist)
        {
            Employee_BL objEmploye = new Employee_BL();
            string BranchId = "";
            int i = 1;

            if (Branchlist != null && Branchlist.Count > 0)
            {
                foreach (string item in Branchlist)
                {
                    if (item == "0")
                    {
                        BranchId = "0";
                        break;
                    }
                    else
                    {
                        if (i > 1)
                            BranchId = BranchId + "," + item;
                        else
                            BranchId = item;
                        i++;
                    }
                }

            }

            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_USERACCOUNTDATA");
            proc.AddPara("@ACTION", "BRANCHLISTSUBMIT");
            proc.AddPara("@USERID", user_id);
            proc.AddPara("@BRANCHLIST", BranchId);
            proc.AddPara("@USER_ID", Convert.ToString(Session["MRuserid"]));
            dt = proc.GetTable();

            return "Success";
        }


        public JsonResult DeleteUser(string user_id)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_USERACCOUNTDATA");
            proc.AddPara("@ACTION", "DELETEUSER");
            proc.AddPara("@USERID", user_id);
            proc.AddPara("@USER_ID", Convert.ToString(Session["MRuserid"]));
            proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
            dt = proc.GetTable();
            string output = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            return Json(output, JsonRequestBehavior.AllowGet);
        }

    }
}