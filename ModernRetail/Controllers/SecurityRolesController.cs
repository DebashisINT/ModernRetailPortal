using BusinessLogicLayer;
using BusinessLogicLayer.MenuBLS;
using BusinessLogicLayer.UserGroupsBLS;
using EntityLayer.CommonELS;
using EntityLayer.MenuHelperELS;
using EntityLayer.UserGroupsEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModernRetail.Models;
using DataAccessLayer;
using System.Data;
using DevExpress.XtraExport;
using UtilityLayer;
using System.Web.Services;
using DevExpress.Web;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;
using DevExpress.Web.Mvc;
namespace ModernRetail.Controllers
{
    public class SecurityRolesController : Controller
    {
        UserGroupBL userGroupBL = new UserGroupBL();
        MenuBL menuBl = new MenuBL();
        SecurityRolesModel objdata = new SecurityRolesModel();
        // GET: SecurityRoles

        public ActionResult SecurityRolesList()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/SecurityRolesList", "SecurityRoles");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            objdata.grp_id = 0;
            TempData["grp_id"] = null;

            TempData.Keep();
            return View();
        }


        public ActionResult GroupMember()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/SecurityRolesList", "SecurityRoles");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            //objdata.grp_id = 0;
            //TempData["grp_id"] = null;

            //TempData.Keep();
            return View();
        }
        
        public ActionResult Index()
        {
            if (TempData["grp_id"] != null)
            {
                objdata.grp_id = Convert.ToInt32(TempData["grp_id"]);
                TempData.Keep();

                 objdata.UserGroupRights = GetSetGroupAccessValues(Convert.ToInt32(objdata.grp_id));

            }
            return View(objdata);
        }       
        private string GetSetGroupAccessValues(int GroupId)
        {
            string UserGroupRightsString = "";
            List<TranAccessByGroupModel> accessList = userGroupBL.GetTranAccessByGroup(GroupId);

            if (accessList != null && accessList.Count() > 0)
            {
                
                foreach (TranAccessByGroupModel model in accessList)
                {
                    string TempString = "";                   
                    if (model.CanAdd || model.CanEdit || model.CanDelete || model.CanView || model.CanIndustry || model.CanCreateActivity ||
                        model.CanContactPerson || model.CanHistory || model.CanAddUpdateDocuments || model.CanMembers || model.CanOpeningAddUpdate ||
                        model.CanAssetDetails || model.CanExport || model.CanPrint || model.CanBudget || model.CanAssignbranch || model.Cancancelassignmnt ||
                        model.CanReassign || model.CanClose || model.CanCancel || model.CanAssign || model.CanBulkUpdate ||
                        model.CanReassignedAreaRouteBeat || model.CanReassignedAreaRouteBeatLog || model.CanReassignedBeatParty || model.CanReassignedBeatPartyLog
                        
                        || model.CanMassDelete || model.CanMassDeleteDownloadImport
                        
                        || model.CanAttendanceLeaveClear
                        
                        || model.CanInvoice || model.CanReadyToDispatch || model.CanDispatch || model.CanDeliver
                        
                        )
                  
                    {
                        if (model.CanAdd)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|1";
                            }
                            else
                            {
                                TempString += "1";
                            }
                        }



                        if (model.CanView)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|2";
                            }
                            else
                            {
                                TempString += "2";
                            }
                        }

                        if (model.CanEdit)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|3";
                            }
                            else
                            {
                                TempString += "3";
                            }
                        }

                        if (model.CanDelete)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|4";
                            }
                            else
                            {
                                TempString += "4";
                            }
                        }


                        if (model.CanIndustry)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|6";
                            }
                            else
                            {
                                TempString += "6";
                            }
                        }

                        if (model.CanCreateActivity)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|5";
                            }
                            else
                            {
                                TempString += "5";
                            }
                        }

                        if (model.CanContactPerson)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|7";
                            }
                            else
                            {
                                TempString += "7";
                            }
                        }

                        if (model.CanHistory)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|8";
                            }
                            else
                            {
                                TempString += "8";
                            }
                        }
                        if (model.CanAddUpdateDocuments)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|9";
                            }
                            else
                            {
                                TempString += "9";
                            }
                        }
                        if (model.CanMembers)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|10";
                            }
                            else
                            {
                                TempString += "10";
                            }
                        }
                        if (model.CanOpeningAddUpdate)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|11";
                            }
                            else
                            {
                                TempString += "11";
                            }
                        }
                        if (model.CanAssetDetails)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|12";
                            }
                            else
                            {
                                TempString += "12";
                            }
                        }
                        if (model.CanExport)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|13";
                            }
                            else
                            {
                                TempString += "13";
                            }
                        }
                        if (model.CanPrint)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|14";
                            }
                            else
                            {
                                TempString += "14";
                            }
                        }

                        if (model.CanBudget)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|15";
                            }
                            else
                            {
                                TempString += "15";
                            }
                        }

                        if (model.CanAssignbranch)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|16";
                            }
                            else
                            {
                                TempString += "16";
                            }
                        }

                        if (model.Cancancelassignmnt)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|17";
                            }
                            else
                            {
                                TempString += "17";
                            }
                        }
                        if (model.CanReassign)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|18";
                            }
                            else
                            {
                                TempString += "18";
                            }
                        }
                        if (model.CanClose)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|19";
                            }
                            else
                            {
                                TempString += "19";
                            }
                        }
                        if (model.CanSpecialEdit)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|20";
                            }
                            else
                            {
                                TempString += "20";
                            }
                        }
                        if (model.CanCancel)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|21";
                            }
                            else
                            {
                                TempString += "21";
                            }
                        }
                       
                        if (model.CanAssign)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|22";
                            }
                            else
                            {
                                TempString += "22";
                            }
                        }
                      
                        if (model.CanBulkUpdate)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|25";
                            }
                            else
                            {
                                TempString += "25";
                            }
                        }
                      
                        if (model.CanReassignedBeatParty)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|26";
                            }
                            else
                            {
                                TempString += "26";
                            }
                        }
                        if (model.CanReassignedBeatPartyLog)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|27";
                            }
                            else
                            {
                                TempString += "27";
                            }
                        }
                        if (model.CanReassignedAreaRouteBeat)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|28";
                            }
                            else
                            {
                                TempString += "28";
                            }
                        }
                        if (model.CanReassignedAreaRouteBeatLog)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|29";
                            }
                            else
                            {
                                TempString += "29";
                            }
                        }                      
                        if (model.CanMassDelete)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|30";
                            }
                            else
                            {
                                TempString += "30";
                            }
                        }
                        if (model.CanMassDeleteDownloadImport)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|31";
                            }
                            else
                            {
                                TempString += "31";
                            }
                        }                       
                        if (model.CanAttendanceLeaveClear)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|32";
                            }
                            else
                            {
                                TempString += "32";
                            }
                        }                    
                        if (model.CanInvoice)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|33";
                            }
                            else
                            {
                                TempString += "33";
                            }
                        }
                        if (model.CanReadyToDispatch)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|34";
                            }
                            else
                            {
                                TempString += "34";
                            }
                        }
                        if (model.CanDispatch)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|35";
                            }
                            else
                            {
                                TempString += "35";
                            }
                        }
                        if (model.CanDeliver)
                        {
                            if (!string.IsNullOrWhiteSpace(TempString))
                            {
                                TempString += "|36";
                            }
                            else
                            {
                                TempString += "36";
                            }
                        }                       
                        TempString = model.MenuId + "^" + TempString;
                    }

                    if (!string.IsNullOrWhiteSpace(TempString))
                    {
                        if (!string.IsNullOrWhiteSpace(UserGroupRightsString))
                        {
                            UserGroupRightsString += "_" + TempString;
                        }
                        else
                        {
                            UserGroupRightsString = TempString;
                        }
                    }
                }

               // GroupUserRights.Value = UserGroupRightsString;
            }

            return UserGroupRightsString;
        }

        [WebMethod]
        public JsonResult GenerateMenus()
        {
            List<MenuEL> AllMenu = menuBl.GetAllMenus(1);
            List<RightEL> rights = menuBl.GetAllRights();
            string MenuTreeString = "<ul id=\"ulMenuTree\">";
            if (AllMenu != null && AllMenu.Count() > 0)
            {
                List<MenuEL> ParentMenus = AllMenu.Where(t => t.mun_parentId == 0).ToList();

                foreach (MenuEL pMenus in ParentMenus.ToList())
                {
                    List<MenuEL> level1Menus = AllMenu.Where(t => t.mun_parentId == pMenus.mnu_id).ToList();

                    MenuTreeString += "<li id=\"0\">";
                    MenuTreeString += "<span>" + pMenus.mnu_menuName + "</span>";
                    if (level1Menus != null && level1Menus.Count() > 0)
                    {
                        MenuTreeString += "<ul>";
                        foreach (MenuEL lvl1 in level1Menus)
                        {
                            List<MenuEL> level2Menus = AllMenu.Where(t => t.mun_parentId == lvl1.mnu_id).ToList();

                            bool stat = !string.IsNullOrWhiteSpace(lvl1.mnu_menuLink) ? true : false;

                            MenuTreeString += "<li id=\"" + ((stat) ? lvl1.mnu_id : 0) + "\">";
                            MenuTreeString += "<span><div style=\"float:left\">" + lvl1.mnu_menuName + "</div>";

                            if (stat)
                            {

                                List<RightEL> allowedRights = menuBl.GetRights(lvl1.RightsToCheck, rights);

                                if (allowedRights == null || allowedRights.Count() <= 0)
                                {
                                    allowedRights = new List<RightEL>();
                                }
                               
                                MenuTreeString += "<span style=\"position:relative;left:16px;\">";
                                foreach (var item in rights)
                                {
                                    if (allowedRights.Where(t => t.Id == item.Id).Count() > 0)
                                    {

                                        MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"" + item.Id + "\" data-menuid=\"" + lvl1.mnu_id + "\" />&nbsp;" + item.Rights + "&nbsp;";

                                    }
                                    else
                                    {

                                        MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"" + item.Id + "\" data-menuid=\"" + lvl1.mnu_id + "\"   style=\"visibility:hidden\"/><label style=\"visibility:hidden\">&nbsp;" + item.Rights + "&nbsp;</label>";

                                    }
                                }
                                MenuTreeString += "</span>";
                                
                            }

                            MenuTreeString += "</span>";

                            if (level2Menus != null && level2Menus.Count() > 0)
                            {
                                MenuTreeString += "<ul>";
                                foreach (MenuEL lvl2 in level2Menus)
                                {
                                    List<RightEL> allowedRights = menuBl.GetRights(lvl2.RightsToCheck, rights);

                                    if (allowedRights == null || allowedRights.Count() <= 0)
                                    {
                                        allowedRights = new List<RightEL>();
                                    }
                                    MenuTreeString += "<li id=\"" + lvl2.mnu_id + "\">";

                                    MenuTreeString += "<span><div style=\"float:left\">" + lvl2.mnu_menuName + "</div>";

                                  
                                    MenuTreeString += "<span >";

                                    foreach (var item in rights)
                                    {
                                        if (allowedRights.Where(t => t.Id == item.Id).Count() > 0)
                                        {
                                            MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"" + item.Id + "\" data-menuid=\"" + lvl2.mnu_id + "\" />&nbsp;" + item.Rights + "&nbsp;";

                                        }
                                        else
                                        {
                                            MenuTreeString += "<input type=\"checkbox\" class=\"chckRights\" data-id=\"" + item.Id + "\" data-menuid=\"" + lvl2.mnu_id + "\"   style=\"display: none\"/><label style=\"display: none\">&nbsp;" + item.Rights + "&nbsp;</label>";
                                          
                                        }
                                    }                                    
                                    MenuTreeString += "</span>";
                                    MenuTreeString += "</span>";
                                    MenuTreeString += "</li>";
                                }
                                MenuTreeString += "</ul>";
                            }

                            MenuTreeString += "</li>";
                        }
                        MenuTreeString += "</ul>";
                    }
                    MenuTreeString += "</li>";
                }

                MenuTreeString += "</ul>";              
            }
            return Json(MenuTreeString);
        }

        [WebMethod]
        public JsonResult SaveSecurityRoles(SecurityRolesModel Details)
        {
            String Message = "";
            Boolean Success = false;
            DataSet dt = new DataSet();


            int? userId = null;

            if (Session["MRuserid"] != null)
            {
                try
                {
                    userId = Convert.ToInt32(Session["MRuserid"]);
                }
                catch
                {
                    userId = null;
                }
            }

            UserGroupSaveModel saveModel = new UserGroupSaveModel();
            saveModel.grp_name = Details.grp_name;
            saveModel.grp_segmentId = 1;
            saveModel.UserGroupRights = Details.UserGroupRights;
            saveModel.CreateUser = userId;
            saveModel.LastModifyUser = userId;

            if (Details.grp_id != null & Details.grp_id != 0)
            {
                try
                {
                    saveModel.grp_id = Convert.ToInt32(Details.grp_id);
                    saveModel.mode = PROC_USP_UserGroups_Modes.UPDATE.ToString();
                }
                catch
                {
                    saveModel.grp_id = 0;
                    saveModel.mode = PROC_USP_UserGroups_Modes.INSERT.ToString();
                }
            }
            else
            {
                saveModel.mode = PROC_USP_UserGroups_Modes.INSERT.ToString();
            }


            CommonResult stat = userGroupBL.SaveUserGroupData(saveModel);

          

            String retuenMsg = stat.IsSuccess + "~" + stat.AddonData + "~"  + "~" + stat.Message;
            return Json(retuenMsg);

        }
        public JsonResult SetMapDataByID(Int64 ID = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["grp_id"] = ID;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string ID)
        {
            int output = 0;
            output = objdata.Delete(ID);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartialGridList(BranchMaster model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/SecurityRolesList", "SecurityRoles");
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
                sqlcmd = new SqlCommand("PRC_MR_SECURITYROLESDETAILS", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GETLISTINGDETAILS");
                sqlcmd.Parameters.Add("@USER_ID", Userid);
                sqlcmd.Parameters.Add("@ISPAGELOAD", model.Is_PageLoad);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                return PartialView("_PartialSecurityRolesListing", GetDetailsList(Is_PageLoad));

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
                var q = from d in dc.MR_SECURITYROLESLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_SECURITYROLESLISTs
                        where d.SEQ == 0
                        select d;
                return q;
            }
        }

        public JsonResult Edit(string id)
        {
            DataSet output = new DataSet();
            output = objdata.Edit(id);
            if (output.Tables[0].Rows.Count > 0)
            {
                return Json(new
                {
                    grp_name = Convert.ToString(output.Tables[0].Rows[0]["grp_name"]),
                    grp_id = Convert.ToInt32(output.Tables[0].Rows[0]["grp_id"])
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { name = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExportList(int type)
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
            settings.SettingsExport.FileName = "SecurityRoles";

            settings.Columns.Add(x =>
            {
                x.FieldName = "GRP_NAME";
                x.Caption = "Group Name";
                x.VisibleIndex = 1;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateUser";
                x.Caption = "Created By";
                x.VisibleIndex = 2;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CreateDate";
                x.Caption = "Created On";
                x.VisibleIndex = 3;
                x.Width = 120;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(15);
                x.ColumnType = MVCxGridViewColumnType.DateEdit;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
               

            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "ModifyUser";
                x.Caption = "Updated By";
                x.VisibleIndex = 4;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(20);
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "ModifyDate";
                x.Caption = "Updated On";
                x.VisibleIndex = 5;
                x.Width = 120;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(11);
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

        public ActionResult PartialGroupMemberGridList(SecurityRolesModel model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/SecurityRolesList", "SecurityRoles");
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;

                string Is_GMPageLoad = string.Empty;
                DataTable dt = new DataTable();

                if (model.Is_GMPageLoad != "1")
                    Is_GMPageLoad = "Ispageload";

                ViewData["ModelData"] = model;
                string Userid = Convert.ToString(Session["MRuserid"]);

                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_MR_SECURITYROLESDETAILS", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GETGROUPMEMBER");
                sqlcmd.Parameters.Add("@USER_ID", Userid);
                sqlcmd.Parameters.Add("@ISPAGELOAD", model.Is_GMPageLoad);
                sqlcmd.Parameters.Add("@ID", model.grp_id);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                return PartialView("_PartialGroupMember", GetGroupMemberDetailsList(Is_GMPageLoad));

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        public IEnumerable GetGroupMemberDetailsList(string Is_PageLoad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
            string Userid = Convert.ToString(Session["MRuserid"]);
            if (Is_PageLoad != "Ispageload")
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_GROUPMEMBERLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_GROUPMEMBERLISTs
                        where d.SEQ == 0
                        select d;
                return q;
            }
        }

        public ActionResult ExporGMList(int type)
        {
            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToXlsx(GetGMGridViewSettings(), GetGroupMemberDetailsList(""));               
                default:
                    break;
            }
            return null;
        }

        private GridViewSettings GetGMGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "PartialGroupMemberGridList";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "GroupMember";

            settings.Columns.Add(x =>
            {
                x.FieldName = "USER_NAME";
                x.Caption = "Name";
                x.VisibleIndex = 1;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(25);
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "DESIGNATION";
                x.Caption = "Designation";
                x.VisibleIndex = 2;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(25);
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "GRSTATUS";
                x.Caption = "User Status";
                x.VisibleIndex = 3;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(25);
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "ONLINESTATUS";
                x.Caption = "Online Status";
                x.VisibleIndex = 4;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Width = System.Web.UI.WebControls.Unit.Percentage(25);
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