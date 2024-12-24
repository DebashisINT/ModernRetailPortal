#region======================================Revision History=========================================================
//Written By : Debashis Talukder On 09/12/2024
//Purpose: Modern Retail Info Details.Row: 3,4,6,7,11
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
    }
}
