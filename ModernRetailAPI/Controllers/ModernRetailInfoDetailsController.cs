#region======================================Revision History=========================================================
//Written By : Debashis Talukder On 09/12/2024
//Purpose: Modern Retail Info Details.Row: 3,4,6,7,11,12,13,14,15,16,17,18,19
#endregion===================================End of Revision History==================================================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ModernRetailAPI.Models;
using BusinessLogicLayer;
using System.Web;

namespace ModernRetailAPI.Controllers
{
    public class ModernRetailInfoDetailsController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage StoreTypeLists(StoreTypeListInput model)
        {
            StoreTypeListOutput omodel = new StoreTypeListOutput();
            List<StoretypelistOutput> STview = new List<StoretypelistOutput>();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "STORETYPELISTS");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Successfully Get List.";
                        STview = APIHelperMethods.ToModelList<StoretypelistOutput>(ds.Tables[0]);
                        omodel.store_type_list = STview;
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "No Data Found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage StoreInfoSave(StoreInfoSaveInput model)
        {
            StoreInfoSaveOutput omodel = new StoreInfoSaveOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    List<StoreSavelists> omodel2 = new List<StoreSavelists>();
                    foreach (var s2 in model.store_list)
                    {
                        omodel2.Add(new StoreSavelists()
                        {
                            store_id = s2.store_id,
                            branch_id = s2.branch_id,
                            store_name = s2.store_name,
                            store_address = s2.store_address,
                            store_pincode = s2.store_pincode,
                            store_lat = s2.store_lat,
                            store_long = s2.store_long,
                            store_contact_name = s2.store_contact_name,
                            store_contact_number = s2.store_contact_number,
                            store_alternet_contact_number=s2.store_alternet_contact_number,
                            store_whatsapp_number = s2.store_whatsapp_number,
                            store_email = s2.store_email,
                            store_type=s2.store_type,
                            store_size_area = s2.store_size_area,
                            store_state_id = s2.store_state_id,
                            remarks = s2.remarks,
                            create_date_time = s2.create_date_time
                        });
                    }

                    string JsonXML = XmlConversion.ConvertToXml(omodel2, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "STOREINFOSAVE");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);
                    sqlcmd.Parameters.AddWithValue("@JsonXML", JsonXML);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0 && Convert.ToInt64(dt.Rows[0][0]) == model.user_id)
                    {
                        omodel.status = "200";
                        omodel.message = "Information added successfully.";
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "Data not Saved.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage StoreInfoFetchLists(StoreInfoFetchListsInput model)
        {
            StoreInfoFetchListsOutput omodel = new StoreInfoFetchListsOutput();
            List<StoreFetchlists> STview = new List<StoreFetchlists>();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    string AttachmentUrl = System.Configuration.ConfigurationManager.AppSettings["StoreAttachment"];
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "STOREINFOFETCH");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);
                    sqlcmd.Parameters.AddWithValue("@AttachmentUrl", AttachmentUrl);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Successfully Get List.";
                        STview = APIHelperMethods.ToModelList<StoreFetchlists>(ds.Tables[0]);
                        omodel.user_id = model.user_id;
                        omodel.store_list = STview;
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "No Data Found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage StoreInfoEdit(StoreInfoEditInput model)
        {
            StoreInfoEditOutput omodel = new StoreInfoEditOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    List<StoreEditlists> omodel2 = new List<StoreEditlists>();
                    foreach (var s2 in model.store_list)
                    {
                        omodel2.Add(new StoreEditlists()
                        {
                            store_id = s2.store_id,
                            branch_id = s2.branch_id,
                            store_name = s2.store_name,
                            store_address = s2.store_address,
                            store_pincode = s2.store_pincode,
                            store_lat = s2.store_lat,
                            store_long = s2.store_long,
                            store_contact_name = s2.store_contact_name,
                            store_contact_number = s2.store_contact_number,
                            store_alternet_contact_number=s2.store_alternet_contact_number,
                            store_whatsapp_number = s2.store_whatsapp_number,
                            store_email = s2.store_email,
                            store_type = s2.store_type,
                            store_size_area = s2.store_size_area,
                            store_state_id = s2.store_state_id,
                            remarks = s2.remarks,
                            edit_date_time = s2.edit_date_time
                        });
                    }

                    string JsonXML = XmlConversion.ConvertToXml(omodel2, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "STOREINFOEDIT");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);
                    sqlcmd.Parameters.AddWithValue("@JsonXML", JsonXML);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Information modified successfully.";
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "Data not Saved.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage PinCityStateLists(PinCityStateListInput model)
        {
            PinCityStateListOutput omodel = new PinCityStateListOutput();
            List<PinCityStlistOutput> PCSview = new List<PinCityStlistOutput>();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "PINCITYSTATEMASTER");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Successfully Get List.";
                        PCSview = APIHelperMethods.ToModelList<PinCityStlistOutput>(ds.Tables[0]);
                        omodel.pin_state_list = PCSview;
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "No Data Found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage ProductDetailLists(ProductDetailListInput model)
        {
            ProductDetailListOutput omodel = new ProductDetailListOutput();
            List<ProductlistOutput> PTview = new List<ProductlistOutput>();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    string AttachmentUrl = System.Configuration.ConfigurationManager.AppSettings["ProductImages"];
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "PRODUCTLISTS");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);
                    sqlcmd.Parameters.AddWithValue("@AttachmentUrl", AttachmentUrl);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Successfully Get List.";
                        PTview = APIHelperMethods.ToModelList<ProductlistOutput>(ds.Tables[0]);
                        omodel.product_list = PTview;
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "No Data Found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage ProductRateLists(ProductRateListInput model)
        {
            ProductRateListOutput omodel = new ProductRateListOutput();
            List<ProdRatelistOutput> PRview = new List<ProdRatelistOutput>();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "PRODUCTRATELISTS");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Successfully Get List.";
                        PRview = APIHelperMethods.ToModelList<ProdRatelistOutput>(ds.Tables[0]);
                        omodel.product_rate_list = PRview;
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "No Data Found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage ProductUomLists(ProductUomListInput model)
        {
            ProductUomListOutput omodel = new ProductUomListOutput();
            List<ProdUomlistOutput> Uview = new List<ProdUomlistOutput>();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "PRODUCTUOMLIST");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Successfully Get List.";
                        Uview = APIHelperMethods.ToModelList<ProdUomlistOutput>(ds.Tables[0]);
                        omodel.uom_list = Uview;
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "No Data Found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage UserBranchLists(UserBranchListInput model)
        {
            UserBranchListOutput omodel = new UserBranchListOutput();
            List<UserBranlistOutput> UBview = new List<UserBranlistOutput>();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "USERBRANCHLIST");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);
                    //sqlcmd.Parameters.AddWithValue("@BRANCHHIERARCHY", Convert.ToString(HttpContext.Current.Session["MRuserbranchHierarchy"]));

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Successfully Get List.";
                        UBview = APIHelperMethods.ToModelList<UserBranlistOutput>(ds.Tables[0]);
                        omodel.branch_list = UBview;
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "No Data Found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage StockInfoSave(StockInfoSaveInput model)
        {
            StockInfoSaveOutput omodel = new StockInfoSaveOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    List<StockSavelists> omodel2 = new List<StockSavelists>();
                    foreach (var s2 in model.product_list)
                    {
                        omodel2.Add(new StockSavelists()
                        {                            
                            stock_id = s2.stock_id,
                            product_dtls_id = s2.product_dtls_id,
                            product_id =s2.product_id,
                            qty=s2.qty,
                            uom_id=s2.uom_id,
                            mfg_date=s2.mfg_date,
                            expire_date=s2.expire_date
                        });
                    }

                    string JsonXML = XmlConversion.ConvertToXml(omodel2, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "STOCKINFOSAVE");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);
                    sqlcmd.Parameters.AddWithValue("@STOCK_ID", model.stock_id);
                    sqlcmd.Parameters.AddWithValue("@STOCK_CREATEDATE", model.save_date_time);
                    sqlcmd.Parameters.AddWithValue("@STORE_ID", model.store_id);
                    sqlcmd.Parameters.AddWithValue("@REMARKS", model.remarks);
                    sqlcmd.Parameters.AddWithValue("@JsonXML", JsonXML);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0 && Convert.ToInt64(dt.Rows[0][0]) == model.user_id)
                    {
                        omodel.status = "200";
                        omodel.message = "Information added successfully.";
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "Data not Saved.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage StockInfoFetchLists(StockInfoFetchListsInput model)
        {
            StockInfoFetchListsOutput omodel = new StockInfoFetchListsOutput();
            List<StocklistOutput> Stkview = new List<StocklistOutput>();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "STOCKINFOFETCH");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            List<StockProductlists> Poview = new List<StockProductlists>();
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["stock_id"]) == Convert.ToString(ds.Tables[1].Rows[j]["stock_id"]))
                                {
                                    Poview.Add(new StockProductlists()
                                    {
                                        stock_id = Convert.ToString(ds.Tables[1].Rows[j]["stock_id"]),
                                        product_dtls_id = Convert.ToInt64(ds.Tables[1].Rows[j]["product_dtls_id"]),
                                        product_id = Convert.ToInt64(ds.Tables[1].Rows[j]["product_id"]),
                                        qty = Convert.ToDecimal(ds.Tables[1].Rows[j]["qty"]),
                                        uom_id = Convert.ToInt64(ds.Tables[1].Rows[j]["uom_id"]),
                                        uom = Convert.ToString(ds.Tables[1].Rows[j]["uom"]),
                                        mfg_date = Convert.ToString(ds.Tables[1].Rows[j]["mfg_date"]),
                                        expire_date = Convert.ToString(ds.Tables[1].Rows[j]["expire_date"])
                                    });
                                }
                            }

                            Stkview.Add(new StocklistOutput()
                            {
                                stock_id = Convert.ToString(ds.Tables[0].Rows[i]["stock_id"]),
                                save_date_time = Convert.ToString(ds.Tables[0].Rows[i]["save_date_time"]),
                                store_id = Convert.ToString(ds.Tables[0].Rows[i]["store_id"]),
                                remarks = Convert.ToString(ds.Tables[0].Rows[i]["remarks"]),
                                product_list = Poview
                            });
                        }

                        omodel.status = "200";
                        omodel.message = "Successfully Get List.";
                        omodel.user_id = model.user_id;
                        omodel.stock_list = Stkview;
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "No Data Found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage OrderInfoSave(OrderInfoSaveInput model)
        {
            OrderInfoSaveSaveOutput omodel = new OrderInfoSaveSaveOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    List<OrderProductLists> omodel2 = new List<OrderProductLists>();
                    foreach (var s2 in model.order_details_list)
                    {
                        omodel2.Add(new OrderProductLists()
                        {
                            order_id = s2.order_id,
                            product_id = s2.product_id,
                            qty = s2.qty,
                            rate = s2.rate
                        });
                    }

                    string JsonXML = XmlConversion.ConvertToXml(omodel2, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "ORDERINFOSAVE");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);
                    sqlcmd.Parameters.AddWithValue("@STORE_ID", model.store_id);
                    sqlcmd.Parameters.AddWithValue("@ORDER_ID", model.order_id);
                    sqlcmd.Parameters.AddWithValue("@ORDER_DATETIME", model.order_date_time);
                    sqlcmd.Parameters.AddWithValue("@ORDER_AMOUNT", model.order_amount);
                    sqlcmd.Parameters.AddWithValue("@ORDER_STATUS", model.order_status);
                    sqlcmd.Parameters.AddWithValue("@REMARKS", model.remarks);
                    sqlcmd.Parameters.AddWithValue("@JsonXML", JsonXML);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0][0]) == model.order_id)
                    {
                        omodel.status = "200";
                        omodel.message = "Order Saved Successfully.";
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "Data not Saved.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage OrderInfoFetchLists(OrderInfoFetchListsInput model)
        {
            OrderInfoFetchListsOutput omodel = new OrderInfoFetchListsOutput();
            List<OrderlistOutput> Ordview = new List<OrderlistOutput>();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    DataSet ds = new DataSet();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "ORDERINFOFETCH");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(ds);
                    sqlcon.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            List<OrderProductlists> Poview = new List<OrderProductlists>();
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[i]["order_id"]) == Convert.ToString(ds.Tables[1].Rows[j]["order_id"]))
                                {
                                    Poview.Add(new OrderProductlists()
                                    {
                                        order_id = Convert.ToString(ds.Tables[1].Rows[j]["order_id"]),
                                        product_id = Convert.ToInt64(ds.Tables[1].Rows[j]["product_id"]),
                                        qty = Convert.ToDecimal(ds.Tables[1].Rows[j]["qty"]),
                                        rate = Convert.ToDecimal(ds.Tables[1].Rows[j]["rate"])
                                    });
                                }
                            }

                            Ordview.Add(new OrderlistOutput()
                            {
                                store_id = Convert.ToString(ds.Tables[0].Rows[i]["store_id"]),
                                order_id = Convert.ToString(ds.Tables[0].Rows[i]["order_id"]),
                                order_date_time = Convert.ToString(ds.Tables[0].Rows[i]["order_date_time"]),
                                order_amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["order_amount"]),
                                order_status = Convert.ToString(ds.Tables[0].Rows[i]["order_status"]),
                                remarks = Convert.ToString(ds.Tables[0].Rows[i]["remarks"]),
                                order_details_list = Poview
                            });
                        }

                        omodel.status = "200";
                        omodel.message = "Successfully Get List.";
                        omodel.user_id = model.user_id;
                        omodel.order_list = Ordview;
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "No Data Found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage OrderWithProductDetailEdit(OrderInfoEditInput model)
        {
            OrderInfoEditOutput omodel = new OrderInfoEditOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    List<OrderProductEditLists> omodel2 = new List<OrderProductEditLists>();
                    foreach (var s2 in model.order_details_list)
                    {
                        omodel2.Add(new OrderProductEditLists()
                        {
                            order_id = s2.order_id,
                            product_id = s2.product_id,
                            qty = s2.qty,
                            rate = s2.rate
                        });
                    }

                    string JsonXML = XmlConversion.ConvertToXml(omodel2, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "ORDEREDIT");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);
                    sqlcmd.Parameters.AddWithValue("@STORE_ID", model.store_id);
                    sqlcmd.Parameters.AddWithValue("@ORDER_ID", model.order_id);
                    sqlcmd.Parameters.AddWithValue("@ORDER_DATETIME", model.order_date_time);
                    sqlcmd.Parameters.AddWithValue("@ORDER_AMOUNT", model.order_amount);
                    sqlcmd.Parameters.AddWithValue("@ORDER_STATUS", model.order_status);
                    sqlcmd.Parameters.AddWithValue("@REMARKS", model.remarks);
                    sqlcmd.Parameters.AddWithValue("@JsonXML", JsonXML);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0][0]) == model.order_id)
                    {
                        omodel.status = "200";
                        omodel.message = "Order Saved Successfully.";
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "Data not Saved.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage OrderWithProductDetailDelete(OrderWithProductDetailDeleteInput model)
        {
            OrderWithProductDetailDeleteOutput omodel = new OrderWithProductDetailDeleteOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    omodel.status = "213";
                    omodel.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
                }
                else
                {
                    List<DeleteOrderProductLists> omodel2 = new List<DeleteOrderProductLists>();
                    foreach (var s2 in model.order_delete_list)
                    {
                        omodel2.Add(new DeleteOrderProductLists()
                        {
                            order_id = s2.order_id
                        });
                    }

                    string JsonXML = XmlConversion.ConvertToXml(omodel2, 0);

                    DataTable dt = new DataTable();
                    String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                    sqlcmd.Parameters.AddWithValue("@ACTION", "ORDERDELETE");
                    sqlcmd.Parameters.AddWithValue("@USER_ID", model.user_id);
                    sqlcmd.Parameters.AddWithValue("@JsonXML", JsonXML);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();
                    if (dt.Rows.Count > 0)
                    {
                        omodel.status = "200";
                        omodel.message = "Order Deleted Successfully.";
                    }
                    else
                    {
                        omodel.status = "205";
                        omodel.message = "Data not found.";
                    }
                    var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                    return message;
                }
            }
            catch (Exception ex)
            {
                omodel.status = "204";
                omodel.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }
    }
}
