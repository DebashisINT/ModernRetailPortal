#region======================================Revision History=========================================================
//Written By : Debashis Talukder On 09/12/2024
//Purpose: Modern Retail Info Details.Row: 5,17
#endregion===================================End of Revision History==================================================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModernRetailAPI.Models;

namespace ModernRetailAPI.Controllers
{
    public class ModernRetailImageInfoController : Controller
    {
        // GET: ModernRetailImageInfo
        public JsonResult StoreImageSave(StoreImageSaveInput model)
        {
            StoreImageSaveOutput omodel = new StoreImageSaveOutput();
            string ImageName = "";
            StoreImageSaveInputDetails omedl2 = new StoreImageSaveInputDetails();
            String APIHostingPort = System.Configuration.ConfigurationManager.AppSettings["APIHostingPort"];
            string UploadFileDirectory = "~/CommonFolder";
            try
            {
                var hhhh = Newtonsoft.Json.JsonConvert.DeserializeObject<StoreImageSaveInputDetails>(model.data);

                if (!string.IsNullOrEmpty(model.data))
                {
                    ImageName = model.attachments.FileName;
                    string vPath = Path.Combine(Server.MapPath("~/CommonFolder/StoreImage"), ImageName);
                    model.attachments.SaveAs(vPath);
                }

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];

                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();

                sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                sqlcmd.Parameters.AddWithValue("@ACTION", "STOREIMAGESAVE");
                sqlcmd.Parameters.AddWithValue("@USER_ID", hhhh.user_id);
                sqlcmd.Parameters.AddWithValue("@STORE_ID", hhhh.store_id);
                sqlcmd.Parameters.AddWithValue("@PathImage", "/CommonFolder/StoreImage/" + ImageName);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Success.";
                }
                else
                {
                    omodel.status = "202";
                    omodel.message = "Image Not Save.";
                }
            }
            catch (Exception msg)
            {
                omodel.status = "204" + ImageName;
                omodel.message = msg.Message;
            }
            return Json(omodel);
        }
        public JsonResult StockImageSave(StockImageSaveInput model)
        {
            StockImageSaveOutput omodel = new StockImageSaveOutput();
            string ImageName = "";
            StockImageSaveInputDetails omedl2 = new StockImageSaveInputDetails();
            String APIHostingPort = System.Configuration.ConfigurationManager.AppSettings["APIHostingPort"];
            string UploadFileDirectory = "~/CommonFolder";
            try
            {
                var hhhh = Newtonsoft.Json.JsonConvert.DeserializeObject<StockImageSaveInputDetails>(model.data);

                if (!string.IsNullOrEmpty(model.data))
                {
                    ImageName = model.attachments.FileName;
                    string vPath = Path.Combine(Server.MapPath("~/CommonFolder/StockImage"), ImageName);
                    model.attachments.SaveAs(vPath);
                }

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationManager.AppSettings["DBConnectionDefault"];

                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();

                sqlcmd = new SqlCommand("PRC_MDRINFODETAILS", sqlcon);
                sqlcmd.Parameters.AddWithValue("@ACTION", "STOCKIMAGESAVE");
                sqlcmd.Parameters.AddWithValue("@USER_ID", hhhh.user_id);
                sqlcmd.Parameters.AddWithValue("@STOCK_ID", hhhh.stock_id);
                sqlcmd.Parameters.AddWithValue("@PathImage", "/CommonFolder/StockImage/" + ImageName);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel.status = "200";
                    omodel.message = "Success.";
                }
                else
                {
                    omodel.status = "202";
                    omodel.message = "Image Not Save.";
                }
            }
            catch (Exception msg)
            {
                omodel.status = "204" + ImageName;
                omodel.message = msg.Message;
            }
            return Json(omodel);
        }
    }
}