using DataAccessLayer;
using ModernRetail.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ModernRetail.Models
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class MRService : System.Web.Services.WebService
    {

        string ConnectionString = String.Empty;
        public MRService()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["DBConnectionDefault"]);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetOnDemandReportTo(string SearchKey)
        {
            List<Employeels> listEmployee = new List<Employeels>();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("MR_FetchReportTo");
            proc.AddPara("@action", "GETONDEMANDREPORTTO");
            proc.AddPara("@userid", Convert.ToString(Session["MRuserid"]));
            proc.AddPara("@reqName", SearchKey);
            dt = proc.GetTable();

            foreach (DataRow dr in dt.Rows)
            {
                Employeels Employee = new Employeels();
                Employee.id = dr["Id"].ToString();
                Employee.Name = dr["Name"].ToString();
                listEmployee.Add(Employee);
            }
            return listEmployee;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetUserList(string SearchKey)
        {
            List<UsersModel> listUser = new List<UsersModel>();
            if (HttpContext.Current.Session["MRuserid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                ProcedureExecute proc = new ProcedureExecute("PRC_MR_UserNameSearch");
                proc.AddPara("@USER_ID", Convert.ToInt32(Session["MRuserid"]));
                proc.AddPara("@SearchKey", SearchKey);
                DataTable Shop = proc.GetTable();
                listUser = (from DataRow dr in Shop.Rows
                            select new UsersModel()
                            {
                                USER_NAME = dr["user_name"].ToString(),
                                USER_LOGINID = dr["user_loginId"].ToString(),
                                USER_ID = Convert.ToString(dr["user_id"]),
                            }).ToList();
            }

            return listUser;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEmployee(string SearchKey, string DesignationId)
        {
            List<EmployeeModel> listEmployee = new List<EmployeeModel>();
            if (HttpContext.Current.Session["MRuserid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
                proc.AddVarcharPara("@Action", 4000, "GetEmployee");
                proc.AddPara("@Designation", Convert.ToInt32(DesignationId));
                proc.AddPara("@SearchKey", SearchKey);
                dt = proc.GetTable();

                listEmployee = (from DataRow dr in dt.Rows
                                select new EmployeeModel()
                                {
                                    id = Convert.ToString(dr["USER_ID"]),
                                    Employee_Code = Convert.ToString(dr["USER_LOGINID"]),
                                    Employee_Name = Convert.ToString(dr["Employee_Name"])
                                }).ToList();
            }

            return listEmployee;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProduct(string SearchKey)
        {
            List<ProductModel> listCust = new List<ProductModel>();
            if (HttpContext.Current.Session["MRuserid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                DataTable dt = new DataTable();
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
                proc.AddVarcharPara("@Action", 4000, "GetProduct");
                proc.AddPara("@SearchKey", SearchKey);
                dt = proc.GetTable();

                //DataTable cust = oDBEngine.GetDataTable("select top 10  SPRODUCTSID,Products_Name ,Products_Description ,sProduct_MinSalePrice  from v_Product_SaleRateLock where Products_Name like '%" + SearchKey + "%'  or Products_Description  like '%" + SearchKey + "%' order by Products_Name,Products_Description");

                listCust = (from DataRow dr in dt.Rows
                            select new ProductModel()
                            {
                                id = dr["SPRODUCTSID"].ToString(),
                                Na = dr["Products_Name"].ToString(),
                                Des = Convert.ToString(dr["Products_Description"])
                            }).ToList();
            }

            return listCust;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetStoreList(string SearchKey)
        {
            List<StoreListModel> listStore = new List<StoreListModel>();
            if (HttpContext.Current.Session["MRuserid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                ProcedureExecute proc = new ProcedureExecute("PRC_MR_INSERTUPDATECURRENTSTOCK");
                proc.AddPara("@ACTION", "GETSTORELIST");
                proc.AddPara("@USER_ID", Convert.ToInt32(Session["MRuserid"]));
                proc.AddPara("@SearchKey", SearchKey);
                DataTable Store = proc.GetTable();

                listStore = (from DataRow dr in Store.Rows
                            select new StoreListModel()
                            {
                                STORE_ID = dr["STORE_ID"].ToString(),
                                STORE_NAME = dr["STORE_NAME"].ToString(),
                                STORETYPE_NAME = dr["STORETYPE_NAME"].ToString(),
                               
                            }).ToList();
            }

            return listStore;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetBranchWiseStoreList(string SearchKey,Int64 branchid)
        {
            List<StoreListModel> listStore = new List<StoreListModel>();
            if (HttpContext.Current.Session["MRuserid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
                proc.AddPara("@ACTION", "GETBRANCHWISESTORE");
                proc.AddPara("@USER_ID", Convert.ToInt32(Session["MRuserid"]));
                proc.AddPara("@BRANCH_ID", Convert.ToInt32(branchid)); 
                proc.AddPara("@SearchKey", SearchKey);
                DataTable Store = proc.GetTable();

                listStore = (from DataRow dr in Store.Rows
                             select new StoreListModel()
                             {
                                 STORE_ID = dr["STORE_ID"].ToString(),
                                 STORE_NAME = dr["STORE_NAME"].ToString(),
                                 STORETYPE_NAME = dr["STORETYPE_NAME"].ToString(),

                             }).ToList();
            }

            return listStore;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProductList(string SearchKey)
        {
            List<ProductListModel> listProduct = new List<ProductListModel>();
            if (HttpContext.Current.Session["MRuserid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_MR_INSERTUPDATECURRENTSTOCK");
                proc.AddPara("@USER_ID", Convert.ToInt32(Session["MRuserid"]));
                proc.AddPara("@SearchKey", SearchKey);
                proc.AddPara("@ACTION", "GETPRODUCTLIST");
                dt = proc.GetTable();

                listProduct = (from DataRow dr in dt.Rows
                               select new ProductListModel()
                               {
                                   sProducts_ID = Convert.ToInt32(dr["sProducts_ID"]),
                                   sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                   sProducts_Name = Convert.ToString(dr["sProducts_Name"])
                                   //sProducts_Description = Convert.ToString(dr["sProducts_Description"])
                               }).ToList();
            }

            return listProduct;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTargetLevelDetailsList(string SearchKey, string Type)
        {

            List<SalesTargetLevelDetails> list = new List<SalesTargetLevelDetails>();

            string USERID = Convert.ToString(Session["MRuserid"]);
            SearchKey = SearchKey.Replace("'", "''");
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            string Query = "";
            Query = @"   EXEC PRC_MR_TARGETASSIGN @Action='" + Type + "',@SearchKey='" + SearchKey + "',@USERID='" + USERID + "' ";

            DataTable dt = oDBEngine.GetDataTable(Query);
            if (!String.IsNullOrEmpty(Type))
            {
                list = (from DataRow dr in dt.Rows
                        select new SalesTargetLevelDetails()
                        {
                            Level_ID = Convert.ToString(dr["ID"]),
                            Level_Name = Convert.ToString(dr["NAME"]),
                            Level_Code = Convert.ToString(dr["CODE"]),


                        }).ToList();
            }
            else
            {
                list = (from DataRow dr in dt.Rows
                        select new SalesTargetLevelDetails()
                        {
                            Level_ID = Convert.ToString(dr["ID"]),
                            Level_Name = Convert.ToString(dr["NAME"]),
                            Level_Code = Convert.ToString(dr["CODE"]),


                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTargetProductDetailsList(string SearchKey)
        {

            List<ProductDetails> list = new List<ProductDetails>();


            SearchKey = SearchKey.Replace("'", "''");
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            string Query = "";
            Query = @"   EXEC PRC_MR_TARGETASSIGN @Action='GETPRODUCTLIST',@SearchKey='" + SearchKey + "' ";

            DataTable dt = oDBEngine.GetDataTable(Query);
            if (dt.Rows.Count > 0 && dt != null)
            {
                list = (from DataRow dr in dt.Rows
                        select new ProductDetails()
                        {
                            ID = Convert.ToString(dr["ID"]),
                            Name = Convert.ToString(dr["NAME"]),
                            Code = Convert.ToString(dr["CODE"]),


                        }).ToList();
            }
            else
            {
                list = (from DataRow dr in dt.Rows
                        select new ProductDetails()
                        {
                            ID = Convert.ToString(dr["ID"]),
                            Name = Convert.ToString(dr["NAME"]),
                            Code = Convert.ToString(dr["CODE"]),


                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTargetBrandDetailsList(string SearchKey)
        {

            List<BrandDetails> list = new List<BrandDetails>();
            SearchKey = SearchKey.Replace("'", "''");
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            string Query = "";
            Query = @"   EXEC PRC_MR_TARGETASSIGN @Action='GETBRANDLIST',@SearchKey='" + SearchKey + "'";

            DataTable dt = oDBEngine.GetDataTable(Query);
            if (dt.Rows.Count > 0 && dt != null)
            {
                list = (from DataRow dr in dt.Rows
                        select new BrandDetails()
                        {
                            Brand_ID = Convert.ToString(dr["ID"]),
                            Brand_Name = Convert.ToString(dr["NAME"]),

                        }).ToList();
            }
            else
            {
                list = (from DataRow dr in dt.Rows
                        select new BrandDetails()
                        {
                            Brand_ID = Convert.ToString(dr["ID"]),
                            Brand_Name = Convert.ToString(dr["NAME"]),
                        }).ToList();
            }

            return list;
        }
        public class UsersModel
        {
            public string USER_ID { get; set; }
            public string USER_NAME { get; set; }
            public string USER_LOGINID { get; set; }
        }

        public class EmployeeModel
        {
            public string id { get; set; }
            public string Employee_Name { get; set; }
            public string Employee_Code { get; set; }
        }

        public class ProductModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string Des { get; set; }
            // public string MinSalePrice { get; set; }
        }
        public class StoreListModel
        {
            public string STORE_ID { get; set; }
            public string STORE_NAME { get; set; }
            public string STORETYPE_NAME { get; set; }
        }
        public class ProductListModel
        {
            public int sProducts_ID { get; set; }
            public string sProducts_Code { get; set; }
            public string sProducts_Name { get; set; }
            //public string sProducts_Description { get; set; }
        }

        public class SalesTargetLevelDetails
        {
            public string Level_ID { get; set; }
            public string Level_Name { get; set; }
            public string Level_Code { get; set; }

        }
        public class ProductDetails
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }

        }

        public class BrandDetails
        {
            public string Brand_ID { get; set; }
            public string Brand_Name { get; set; }
        }

    }
}
