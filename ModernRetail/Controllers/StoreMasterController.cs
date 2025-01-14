using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Data.WcfLinq;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using DevExpress.XtraRichEdit.Import.Html;
using ModernRetail.Models;
using Newtonsoft.Json;
using SalesmanTrack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace ModernRetail.Controllers
{
    public class StoreMasterController : Controller
    {
        // GET: StoreMaster
        StoreMasterModel objdata = null;
        Int64 DetailsID = 0;
        string UserName = string.Empty;

        public StoreMasterController()
        {
            objdata = new StoreMasterModel();
        }

        public ActionResult Index()
        {
            StoreMasterModel dtLs = new StoreMasterModel();

            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/UserMasterList", "UserConfiguration");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            objdata.store_id = null;
            TempData["store_id"] = null;

            TempData.Keep();

            return View(dtLs);

        }

        public ActionResult StoreAddEdit()
        {
            StoreMasterModel dtLs = new StoreMasterModel();

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_STOREMASTERADDUPDATELIST");
            proc.AddPara("@ACTION", "GETLISTDATA");
            proc.AddPara("@USER_ID", Convert.ToString(Session["MRuserid"]));
            proc.AddPara("@BRANCH", Convert.ToString(Session["MRuserbranchHierarchy"]));
            ds = proc.GetDataSet();

            if (ds != null)
            {
                //List<PinList> PinList = new List<PinList>();
                //PinList = APIHelperMethods.ToModelList<PinList>(ds.Tables[0]);

                List<StoreTypeList> StoreTypeList = new List<StoreTypeList>();
                StoreTypeList = APIHelperMethods.ToModelList<StoreTypeList>(ds.Tables[1]);

                List<BranchList> BranchList = new List<BranchList>();
                BranchList = APIHelperMethods.ToModelList<BranchList>(ds.Tables[2]);

                //dtLs.PinList = PinList;
                dtLs.StoreTypeList = StoreTypeList;
                dtLs.BranchList = BranchList;

            }

            TempData["store_id"] = null;
            TempData["created_userid"] = null;

            return View(dtLs);
        }

        public JsonResult SetMapDataByID(string ID = "", Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["store_id"] = ID;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPIN()
        {
            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_STOREMASTERADDUPDATELIST");
            proc.AddPara("@ACTION", "GETPINLIST");
            DT = proc.GetTable();
            return Json(APIHelperMethods.ToModelList<PinList>(DT));
        }
        

        [HttpPost]
        public ActionResult GetCountryStateCity(string PIN)
        {
            List<CountryStateCity> CountryStateCity = new List<CountryStateCity>();
            
            DataTable dtState = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_STOREMASTERADDUPDATELIST");
            proc.AddPara("@ACTION", "GetCountryStateCityOnPin");
            proc.AddPara("@PIN", PIN);
            dtState = proc.GetTable();

            if (dtState.Rows.Count > 0)
            {
                CountryStateCity = APIHelperMethods.ToModelList<CountryStateCity>(dtState);
            }
            return Json(CountryStateCity, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public ActionResult GetState(string Country)
        //{
        //    List<StateListstore> StateLst = new List<StateListstore>();
        //    DataTable dtState = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("PRC_MR_STOREMASTERADDUPDATELIST");
        //    proc.AddPara("@ACTION", "GetStateListCountryWise");
        //    proc.AddPara("@CountryId", Country);
        //    dtState = proc.GetTable();

        //    if (dtState.Rows.Count > 0)
        //    {
        //        StateLst = APIHelperMethods.ToModelList<StateListstore>(dtState);
        //    }
        //    return Json(StateLst, JsonRequestBehavior.AllowGet);

        //}

        //[HttpPost]
        //public ActionResult GetCity(string State)
        //{
        //    List<CityListstore> CityLst = new List<CityListstore>();
        //    DataTable dtCity = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("PRC_MR_STOREMASTERADDUPDATELIST");
        //    proc.AddPara("@ACTION", "GetCityListStateWise");
        //    proc.AddPara("@stateId", State);
        //    dtCity = proc.GetTable();
        //    if (dtCity.Rows.Count > 0)
        //    {
        //        CityLst = APIHelperMethods.ToModelList<CityListstore>(dtCity);
        //    }
        //    return Json(CityLst, JsonRequestBehavior.AllowGet);

        //}


        // SAVE STORE BY API - ModernRetailAPI
        [AcceptVerbs("POST")]
        public JsonResult AddStore(StoreSavelists data)
        {
            string Userid = Convert.ToString(Session["MRuserid"]);
            string rtrnvalue = "";

            var storeid = "";

            
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_STOREMASTERADDUPDATELIST");
            if (data.store_id == null || data.store_id == "" || data.store_id == "Add")
            {
                proc.AddPara("@ACTION", "INSERTSTORE");
                storeid = Convert.ToString(Session["MRuserid"]) + "_" + DateTime.Now.Date.ToString("yyMMdd") + DateTime.Now.ToString("hhmmss");
            }
            else
            {
                proc.AddPara("@ACTION", "UPDATESTORE");
                storeid = data.store_id;
            }

            TempData["store_id"] = storeid;
            TempData["created_userid"] = data.created_userid;
            TempData.Keep();

            proc.AddPara("@STORE_ID", storeid);
            proc.AddPara("@STORE_NAME", data.store_name);
            proc.AddPara("@STORE_ADDRESS", data.store_address);
            proc.AddPara("@STORE_PINCODE", data.store_pincode);
            proc.AddPara("@STORE_BRANCH", data.store_branch);
            proc.AddPara("@STORE_STATUS", data.store_status);
            proc.AddPara("@STORE_LAT", data.store_lat);
            proc.AddPara("@STORE_LONG", data.store_long);
            proc.AddPara("@STORE_CONTACT_NAME", data.store_contact_name);
            proc.AddPara("@STORE_CONTACT_NUMBER", data.store_contact_number);
            proc.AddPara("@STORE_ALTERNET_CONTACT_NUMBER", data.store_alternet_contact_number);
            proc.AddPara("@STORE_WHATSAPP_NUMBER", data.store_whatsapp_number);
            proc.AddPara("@STORE_EMAIL", data.store_email);
            proc.AddPara("@STORE_TYPE", data.store_type);
            proc.AddPara("@STORE_SIZE_AREA", data.store_size_area);
            proc.AddPara("@STORE_STATE_ID", data.store_state_id);
            proc.AddPara("@REMARKS", data.remarks);
            proc.AddPara("@CREATE_DATE_TIME", data.create_date_time);
            proc.AddPara("@CREATED_USERID", data.created_userid);
            proc.AddPara("@CREATED_USERID_OLD", data.created_userid_old);
            proc.AddPara("@USER_ID", Userid);
            

            proc.AddVarcharPara("@RETURN_VALUE", 50, "", QueryParameterDirection.Output);
            
            int k = proc.RunActionQuery();

            rtrnvalue = Convert.ToString(proc.GetParaValue("@RETURN_VALUE"));
            return Json(rtrnvalue, JsonRequestBehavior.AllowGet);



            // SAVE BY API CALLING //
            ////////// Ensure `MRuserid` is assigned if needed
            ////////// model.user_id = Convert.ToInt64(Session["MRuserid"]);

            ////////// Get the API URL from configuration
            ////////string weburl = System.Configuration.ConfigurationManager.AppSettings["PortalStoreAdd"];
            ////////string apiUrl = weburl;

            ////////DateTime currentDateTime = DateTime.Now;
            ////////data.create_date_time = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            ////////var storeid = Convert.ToString(Session["MRuserid"]) +"_"+ DateTime.Now.Date.ToString("yyMMdd") + DateTime.Now.ToString("hhmmss");
            ////////data.store_id = storeid;

            ////////TempData["store_id"] = storeid;
            ////////TempData.Keep();

            ////////try
            ////////{
            ////////    using (HttpClient httpClient = new HttpClient())
            ////////    {
            ////////        // Wrap the data in a StoreInfoSaveInput object
            ////////        var apiInput = new StoreInfoSaveInput
            ////////        {
            ////////            user_id = Convert.ToInt64(Session["MRuserid"]), // Ensure this is populated correctly
            ////////            store_list = new List<StoreSavelists> { data }
            ////////        };

            ////////        // Serialize the data to JSON
            ////////        string json = JsonConvert.SerializeObject(apiInput);

            ////////        // Create HttpContent with proper headers
            ////////        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            ////////        // Post to the API endpoint
            ////////        var response = httpClient.PostAsync(apiUrl, content).Result;

            ////////        // Read the response content
            ////////        if (response.IsSuccessStatusCode)
            ////////        {
            ////////            string responseData = response.Content.ReadAsStringAsync().Result;
            ////////            return Json(new { status = "success", data = responseData });
            ////////        }
            ////////        else
            ////////        {
            ////////            return Json(new { status = "error", message = response.ReasonPhrase });
            ////////        }
            ////////    }
            ////////}
            ////////catch (Exception ex)
            ////////{
            ////////    // Log exception and return a failure response
            ////////    return Json(new { status = "error", message = ex.Message });
            ////////}
            // END SAVE BY API CALLING
        }

        [AcceptVerbs("POST")]
        public JsonResult AddStoreImage(AddStoreImageInputData model)
        {
            string weburl = System.Configuration.ConfigurationManager.AppSettings["PortalStoreAddImage"];
            string apiUrl = weburl;

            //model.data = Convert.ToInt64(Session["MRuserid"])+","+Convert.ToString(TempData["store_id"]);

            // Prepare the JSON data
            var jsonData = new
            {
                store_id = $"{TempData["store_id"]}",
                user_id = Convert.ToInt64($"{TempData["created_userid"]}")
            };

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonData);

            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();

            // Add JSON data
            form.Add(new StringContent(jsonString, Encoding.UTF8, "application/json"), "data");

            // Add file
            var fileContent = new StreamContent(model.store_image.InputStream);
            form.Add(fileContent, "attachments", model.store_image.FileName);

            // Post to the API
            var result = httpClient.PostAsync(apiUrl, form).Result;

            TempData["store_id"] = null;
            TempData["created_userid"] = null;
            TempData.Keep();


            return Json(result.ReasonPhrase);
        }
        // END SAVE STORE BY API - ModernRetailAPI

        public ActionResult PartialStoreMasterGridList(StoreMasterModel model)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Index", "StoreMaster");

            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanEdit = rights.CanEdit;

            string Is_PageLoad = string.Empty;
            DataTable dt = new DataTable();
            if (model.Fromdate == null)
            {
                model.Fromdate = DateTime.Now.ToString("dd-MM-yyyy");
            }

            if (model.Todate == null)
            {
                model.Todate = DateTime.Now.ToString("dd-MM-yyyy");
            }

            if (model.Is_PageLoad != "1") Is_PageLoad = "Ispageload";

            ViewData["ModelData"] = model;

            string datfrmat = model.Fromdate.Split('-')[2] + '-' + model.Fromdate.Split('-')[1] + '-' + model.Fromdate.Split('-')[0];
            string dattoat = model.Todate.Split('-')[2] + '-' + model.Todate.Split('-')[1] + '-' + model.Todate.Split('-')[0];
            string Userid = Convert.ToString(Session["MRuserid"]);

            //string state = "";
            //int i = 1;
            //if (model.StateIds != null && model.StateIds.Count > 0)
            //{
            //    foreach (string item in model.StateIds)
            //    {
            //        if (i > 1)
            //            state = state + "," + item;
            //        else
            //            state = item;
            //        i++;
            //    }
            //}

            //string empcode = "";
            //int k = 1;
            //if (model.empcode != null && model.empcode.Count > 0)
            //{
            //    foreach (string item in model.empcode)
            //    {
            //        if (k > 1)
            //            empcode = empcode + "," + item;
            //        else
            //            empcode = item;
            //        k++;
            //    }
            //}

            string AttachmentUrl = System.Configuration.ConfigurationManager.AppSettings["StoreAttachment"];

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_STOREMASTERADDUPDATELIST");
            proc.AddPara("@ACTION", "STOREMASTERLISTING");
            proc.AddPara("@FROMDATE", datfrmat);
            proc.AddPara("@TODATE", dattoat);
            proc.AddPara("@USER_ID", Userid);
            proc.AddPara("@ISPAGELOAD", model.Is_PageLoad);
            proc.AddPara("@AttachmentUrl", AttachmentUrl);
            //proc.AddPara("@STATEID", stateID);
            // proc.AddPara("@EMPID", userlist);
            // proc.AddPara("@IsReAssignedDate", IS_ReAssignedDate);
            ds = proc.GetTable();


            return PartialView("PartialStoreMasterGridList", GetDataDetails(Is_PageLoad));
        }

        public IEnumerable GetDataDetails(string Is_PageLoad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
            string Userid = Convert.ToString(Session["MRuserid"]);
            
           
            if (Is_PageLoad != "Ispageload")
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_STOREMASTERLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_STOREMASTERLISTs
                        where d.USERID == Convert.ToInt32(Userid) && d.STORE_ID == "0"
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
        }

        public JsonResult DeleteStore(string store_id)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_STOREMASTERADDUPDATELIST");
            proc.AddPara("@ACTION", "DELETEUSER");
            proc.AddPara("@STORE_ID", store_id);
            proc.AddPara("@USER_ID", Convert.ToString(Session["MRuserid"]));
            proc.AddVarcharPara("@RETURN_VALUE", 50, "", QueryParameterDirection.Output);
            dt = proc.GetTable();
            string output = Convert.ToString(proc.GetParaValue("@RETURN_VALUE"));

            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditStore(string store_id)
        {
            string AttachmentUrl = System.Configuration.ConfigurationManager.AppSettings["StoreAttachment"];

            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_STOREMASTERADDUPDATELIST");
            proc.AddPara("@ACTION", "EDITUSER");
            proc.AddPara("@STORE_ID", store_id);
            proc.AddPara("@USER_ID", Convert.ToString(Session["MRuserid"]));
            proc.AddPara("@AttachmentUrl", AttachmentUrl);
            proc.AddVarcharPara("@RETURN_VALUE", 50, "", QueryParameterDirection.Output);
            dt = proc.GetTable();


            if (dt.Rows.Count > 0)
            {
                return Json(new
                {
                   
                    store_name = Convert.ToString(dt.Rows[0]["STORE_NAME"]),
                    store_address = Convert.ToString(dt.Rows[0]["STORE_ADDRESS"]),
                    store_pincode = Convert.ToString(dt.Rows[0]["STORE_PINCODE"]),
                    store_branch = Convert.ToString(dt.Rows[0]["BRANCH_ID"]),
                    store_status = Convert.ToString(dt.Rows[0]["ISACTIVE"]),
                    store_lat = Convert.ToString(dt.Rows[0]["STORE_LAT"]),
                    store_long = Convert.ToString(dt.Rows[0]["STORE_LONG"]),
                    store_contact_name = Convert.ToString(dt.Rows[0]["STORE_CONTACT_NAME"]),
                    store_contact_number = Convert.ToString(dt.Rows[0]["STORE_CONTACT_NUMBER"]),
                    store_alternet_contact_number = Convert.ToString(dt.Rows[0]["STORE_ALTERNET_CONTACT_NUMBER"]),
                    store_whatsapp_number = Convert.ToString(dt.Rows[0]["STORE_WHATSAPP_NUMBER"]),
                    store_email = Convert.ToString(dt.Rows[0]["STORE_EMAIL"]),
                    store_type = Convert.ToString(dt.Rows[0]["STORE_TYPEID"]),
                    store_size_area = Convert.ToString(dt.Rows[0]["STORE_SIZE_AREA"]),
                    store_state_id = Convert.ToString(dt.Rows[0]["STORE_STATE_ID"]),
                    remarks = Convert.ToString(dt.Rows[0]["REMARKS"]),
                    created_userid = Convert.ToString(dt.Rows[0]["USERID"]),
                    created_userid_old = Convert.ToString(dt.Rows[0]["OLDUSERID"]),
                    created_username = Convert.ToString(dt.Rows[0]["USERNAME"]),
                    created_useridname_old = Convert.ToString(dt.Rows[0]["OLDUSERNAME"]),
                    store_pic_url = Convert.ToString(dt.Rows[0]["store_pic_url"]),
                    CountryName = Convert.ToString(dt.Rows[0]["CountryName"]),
                    StateName = Convert.ToString(dt.Rows[0]["StateName"]),
                    CityName = Convert.ToString(dt.Rows[0]["CityName"])


                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { name = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ExportStorelist(int type)
        {
            //List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            switch (type)
            {
                //case 1:
                //    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), TempData["ExportShoplist"]);
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetDataDetails(""));
                //break;
                //case 3:
                //    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), TempData["ExportShoplist"]);
                ////break;
                //case 4:
                //    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), TempData["ExportShoplist"]);
                ////break;
                //case 5:
                //    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), TempData["ExportShoplist"]);
                default:
                    break;
            }
            return null;
        }

        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Stroe List";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Store List Report";


            settings.Columns.Add(column =>
            {
                column.Caption = "Store Name";
                column.FieldName = "STORE_NAME";
                column.ExportWidth = 200;
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Branch";
                column.FieldName = "STORE_BRANCH";
                column.ExportWidth = 150;
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Address";
                column.FieldName = "STORE_ADDRESS";
                column.ExportWidth = 180;
            });
            
            settings.Columns.Add(column =>
            {
                column.Caption = "Pincode";
                column.FieldName = "STORE_PINCODE";
                column.ExportWidth = 180;
            });
            
            settings.Columns.Add(column =>
            {
                column.Caption = "Type";
                column.FieldName = "STORE_TYPE";
                column.ExportWidth = 120;
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Contact No.";
                column.FieldName = "STORE_CONTACT_NUMBER";
                column.ExportWidth = 120;
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "WhatsApp No.";
                column.FieldName = "STORE_WHATSAPP_NUMBER";
                column.ExportWidth = 100;
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Created By";
                column.FieldName = "STORE_CREATEDUSER";
                column.ExportWidth = 100;
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Created On";
                column.FieldName = "CREATEDATE";
                column.ExportWidth = 100;
            });
            
            settings.Columns.Add(column =>
            {
                column.Caption = "Active";
                column.FieldName = "STORE_ISACTIVE";
                column.ExportWidth = 100;
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