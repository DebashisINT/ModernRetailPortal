#region======================================Revision History=========================================================
//Written By : Debashis Talukder On 09/12/2024
//Purpose: Modern Retail Info Details.Row: 3
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
