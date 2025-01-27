﻿using BusinessLogicLayer.SalesmanTrack;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraExport;
using ModernRetail.Models;
using SalesmanTrack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace ModernRetail.Controllers
{
    public class WarehouseMasterController : Controller
    {
        //WarehouseMaster
        WarehouseMasterModel objdata = null;
        public WarehouseMasterController()
        {
            objdata = new WarehouseMasterModel();
        }
        UserList lstuser = new UserList();
        MasterWarehouseBL objwar = new MasterWarehouseBL();
        public ActionResult WarehouseIndex()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/WarehouseIndex", "WarehouseMaster");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            WarehouseMasterModel model = new WarehouseMasterModel();
            List<Country_List> country = new List<Country_List>();
            List<States_List> state = new List<States_List>();
            List<CityDistrictList> city = new List<CityDistrictList>();
            List<DistributerList> shop = new List<DistributerList>();

            DataSet ds = objwar.GetMasterDropdownListAll();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    country.Add(new Country_List
                    {
                        cou_id = Convert.ToString(item["cou_id"]),
                        cou_country = Convert.ToString(item["cou_country"])
                    });
                }
            }

            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    shop.Add(new DistributerList
                    {
                        Shop_Code = Convert.ToString(item["STORE_ID"]),
                        Shop_Name = Convert.ToString(item["STORE_NAME"])
                    });
                }
            }
            model.State_List = state;
            model.CityDistrict_List = city;
            model.Country_List = country;
            model.Distributer_List = shop;


            TempData["WareHouse_ID"] = null;
            TempData.Keep();

            return View(model);
        }

        public ActionResult WHAdd()
        {
            WarehouseMasterModel model = new WarehouseMasterModel();

            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/WarehouseIndex", "WarehouseMaster");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;



            if (TempData["Warehouse_ID"] != null)
            {
                model.WarehouseID = Convert.ToString(TempData["Warehouse_ID"]);
                TempData.Keep();

            }

            
            List<Country_List> country = new List<Country_List>();
            List<States_List> state = new List<States_List>();
            List<CityDistrictList> city = new List<CityDistrictList>();
            List<DistributerList> shop = new List<DistributerList>();

            DataSet ds = objwar.GetMasterDropdownListAll();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    country.Add(new Country_List
                    {
                        cou_id = Convert.ToString(item["cou_id"]),
                        cou_country = Convert.ToString(item["cou_country"])
                    });
                }
            }

            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    shop.Add(new DistributerList
                    {
                        Shop_Code = Convert.ToString(item["STORE_ID"]),
                        Shop_Name = Convert.ToString(item["STORE_NAME"])
                    });
                }
            }
            model.State_List = state;
            model.CityDistrict_List = city;
            model.Country_List = country;
            model.Distributer_List = shop;

            return View("~/Views/WarehouseMaster/WHAdd.cshtml", model);
            
        }

        [HttpPost]
        public ActionResult SatetListView(string countryID)
        {
            List<States_List> state = new List<States_List>();
            DataTable statedt = objwar.GetMasterDropdownList(countryID, "0", "STATE");
            if (statedt != null && statedt.Rows.Count > 0)
            {
                foreach (DataRow item in statedt.Rows)
                {
                    state.Add(new States_List
                    {
                        ID = Convert.ToString(item["id"]),
                        Name = Convert.ToString(item["state"])
                    });
                }
            }
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CityListView(string StateID)
        {
            List<CityDistrictList> city = new List<CityDistrictList>();
            DataTable Citydt = objwar.GetMasterDropdownList("0", StateID, "CITY");
            if (Citydt != null && Citydt.Rows.Count > 0)
            {
                foreach (DataRow item in Citydt.Rows)
                {
                    city.Add(new CityDistrictList
                    {
                        city_id = Convert.ToString(item["city_id"]),
                        city_name = Convert.ToString(item["city_name"])
                    });
                }
            }
            return Json(city, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddWarehouseMaster(String action, String WareHouse_ID, string warehouseName, string address1, String address2, String address3, String country, String State, String City, String Pin, String contactPerson, String ContactPhone, String Distributor, String defaultvalue)
        {
            if (WareHouse_ID == "")
            {
                WareHouse_ID = null;
            }
            String DistributorList= Distributor;
            string StateId = "";
          
            String msg = "";
            DataTable dt = objwar.Masterdatainsert(action, WareHouse_ID, warehouseName, address1, address2, address3, country, State, City, Pin, contactPerson, ContactPhone, DistributorList, defaultvalue, Session["MRuserid"].ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                msg = dt.Rows[0]["MSG"].ToString();              
            }
            else
            {
                msg = "please try again later.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult WarehouseMasterGrid(String Is_PageLoad)
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/WarehouseIndex", "WarehouseMaster");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            DataTable dt = objwar.MasterdataList("LIST", Session["MRuserid"].ToString());
            return PartialView("_PartialWareHouseGrid", GetWareHouse(Is_PageLoad));
        }

        public IEnumerable GetWareHouse(string Is_PageLoad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
            string Userid = Convert.ToString(Session["MRuserid"]);
            if (Is_PageLoad != "Ispageload")
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_WAREHOUSE_LISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                ModernRetailDataContext dc = new ModernRetailDataContext(connectionString);
                var q = from d in dc.MR_WAREHOUSE_LISTs
                        where d.USERID == Convert.ToInt32(Userid) && d.WAREHOUSE_ID == 0
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
        }

        [HttpPost]
        public ActionResult WareHouseView(string WareHouse_ID)
        {
            WarehouseMasterModel warehouse = new WarehouseMasterModel();
            DataTable dt = objwar.MasterdataView("VIEW", WareHouse_ID);
            if (dt != null && dt.Rows.Count > 0)
            {
                warehouse.WarehouseID = dt.Rows[0]["WAREHOUSE_ID"].ToString();
                warehouse.WarehouseName = dt.Rows[0]["WAREHOUSE_NAME"].ToString();
                warehouse.Address1 = dt.Rows[0]["ADDRESS1"].ToString();
                warehouse.Address2 = dt.Rows[0]["ADDRESS2"].ToString();
                warehouse.Address3 = dt.Rows[0]["ADDRESS3"].ToString();
                warehouse.Country = dt.Rows[0]["COUNTRY_ID"].ToString();
                warehouse.State = dt.Rows[0]["STATE_ID"].ToString();
                warehouse.CityDistrict = dt.Rows[0]["DISTRICT"].ToString();
                warehouse.Pin = dt.Rows[0]["PIN"].ToString();
                warehouse.ContactName = dt.Rows[0]["CONTACT_NAME"].ToString();
                warehouse.ContactPhone = dt.Rows[0]["CONTACT_PHONE"].ToString();
                warehouse.isDefault = dt.Rows[0]["ISDEFAULT"].ToString();
                warehouse.Distributer = dt.Rows[0]["STORE_ID"].ToString();
            }
            return Json(warehouse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WareHouseDelete(string WareHouse_ID)
        {
            string returns = "Not Deleted please try again later.";
            DataTable dt = objwar.MasterdataView("DELETE", WareHouse_ID);
            if (dt != null && dt.Rows.Count > 0)
            {
                returns = "Deleted Successfully.";
            }
            return Json(returns);
        }

        public ActionResult ExportWarehouselist(int type)
        {
            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToXlsx(GetBookingGridViewSettings(), GetWareHouse(""));               
                default:
                    break;
            }
            return null;
        }

        private GridViewSettings GetBookingGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "WareHouse Master";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "WareHouse Master";

            settings.Columns.Add(column =>
            {
                column.Caption = "Warehouse Name";
                column.FieldName = "WAREHOUSE_NAME";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Address";
                column.FieldName = "ADDRESS1";

            });
            
            settings.Columns.Add(column =>
            {
                column.Caption = "Country";
                column.FieldName = "cou_country";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "State";
                column.FieldName = "state";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "City/District";
                column.FieldName = "city_name";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Pin";
                column.FieldName = "PIN";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Contact Person Name";
                column.FieldName = "CONTACT_NAME";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Contact Person Phone";
                column.FieldName = "CONTACT_PHONE";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Distributor";
                column.FieldName = "STORE_NAME";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Created By";
                column.FieldName = "CREATED_BY";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Entered On";
                column.FieldName = "CREATE_ON";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy hh:mm:ss";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Modified By";
                column.FieldName = "UPDATED_BY";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Modified On";
                column.FieldName = "UPDATE_ON";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy hh:mm:ss";
            });

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        public ActionResult PartialWarehousePermission()
        {
            try
            {  
                return PartialView("_PartialWareHousePermission.cshtml",null);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        //public ActionResult PartialShopList()
        //{
        //    List<ShopList> Shop_list = new List<ShopList>();
        //    DataTable Shopdt = objwar.GetMasterShopList("4","SHOPLIST");
        //    if (Shopdt != null && Shopdt.Rows.Count > 0)
        //    {
        //        foreach (DataRow item in Shopdt.Rows)
        //        {
        //            Shop_list.Add(new ShopList
        //            {
        //                Shop_code = Convert.ToString(item["Shop_Code"]),
        //                Shop_name = Convert.ToString(item["Shop_Name"]),
        //                Type = Convert.ToString(item["SHOP_TYPE"]),
        //                ContactNo = Convert.ToString(item["Shop_Owner_Contact"]),
        //                //ReportTo = Convert.ToString(item["city_id"]),
        //                State = Convert.ToString(item["state"]),
        //                Address = Convert.ToString(item["SHOP_TYPE"])
        //            });
        //        }
        //    }
        //    return PartialView(Shop_list);
        //}

        public ActionResult GetShopList(string WarehouseID,string StateId)
        {
            try
            {
               
                int i = 1;
               
                List<Getmasterstock> modelshop = new List<Getmasterstock>();
                DataTable dtshop = objwar.GetShopListByparam(StateId, "ShopbyState", "4");
                modelshop = APIHelperMethods.ToModelList<Getmasterstock>(dtshop);

                List<GETSTORE> productdata = new List<GETSTORE>();
                GETSTORE dataobj = new GETSTORE();
                // DataSet output = new DataSet();
                DataTable output = objwar.MasterdataView("WAREHOUSEDETAILS", WarehouseID);
                //output = obj.EditQuestion(QUESTIONS_ID);
                if (output != null && output.Rows.Count > 0)
                {                   
                        
                        foreach (DataRow row in output.Rows)
                        {
                            dataobj = new GETSTORE();
                            dataobj.ID = Convert.ToString(row["STORE_ID"]);
                            productdata.Add(dataobj);

                        }
                        ViewBag.STORE_IDS = productdata;
                   

                }
               
                return PartialView("_ShopPartial", modelshop);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }


        public JsonResult SetMapDataByID(Int64 ID = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["WareHouse_ID"] = ID;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CHECKUNIQUETARGETDOCNUMBER(string WarehouseName, string WareHouse_ID)
        {
            var retData = 0;
            try
            {
                ProcedureExecute proc;
                using (proc = new ProcedureExecute("PRC_MR_MASTERWAREHOUSE"))
                {
                    proc.AddVarcharPara("@action", 100, "CHECKUNIQUETARGETDOCNUMBER");
                    proc.AddIntegerPara("@ReturnValue", 0, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@WAREHOUSE_NAME", 300, WarehouseName.Trim());
                    proc.AddVarcharPara("@WAREHOUSE_ID", 100, WareHouse_ID);
                    int i = proc.RunActionQuery();
                    retData = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));

                }
            }
            catch { }
            return Json(retData);
        }
    }
}