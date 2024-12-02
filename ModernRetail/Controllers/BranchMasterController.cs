using javax.jws;
using Microsoft.AspNetCore.Mvc;
using ModernRetail.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ModernRetail.Controllers
{
    public class BranchMasterController : Controller
    {
        BranchMasterModel objdata = null;
        Int32 DetailsID = 0;
       
        private IConfiguration Configuration;
        public BranchMasterController(IConfiguration _configuration)
        {
            Configuration = _configuration;
           
        }
        private string GetConnectionString()
        {
            return this.Configuration.GetConnectionString("DefaultConnection");
        }
        public IActionResult Index()
        {
            GetConnectionString();
            return View();
        }
        [HttpPost]
        public JsonResult SaveBranch(BranchMasterModel Details)
        {
            String Message = "";
            Boolean Success = false;
            DataSet dt = new DataSet();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                
                using (SqlCommand cmd = new SqlCommand("PRC_BRANCHMASTERDETAILS", con))
                {
                    cmd.Parameters.AddWithValue("@ACTION", "ADD");
                    cmd.Parameters.AddWithValue("@BranchName", Details.BranchName);
                    cmd.Parameters.AddWithValue("@BRANCH_ID", Details.BranchID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dt);
                    cmd.Dispose();
                    con.Dispose();

                    if (dt != null && dt.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Tables[0].Rows)
                        {
                            Success = Convert.ToBoolean(row["Success"]);
                            DetailsID = Convert.ToInt32(row["DetailsID"]);                            
                        }
                    }

                }
            }           

            String retuenMsg = Success + "~" + DetailsID  + "~" + Message;
            return Json(retuenMsg);

        }

        //public ActionResult PartialGridList(BranchMasterModel model)
        //{
        //    try
        //    {
        //        //EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/LMSCategory/Index");
        //        //ViewBag.CanAdd = rights.CanAdd;
        //        //ViewBag.CanView = rights.CanView;
        //        //ViewBag.CanExport = rights.CanExport;
        //        //ViewBag.CanEdit = rights.CanEdit;
        //        //ViewBag.CanDelete = rights.CanDelete;

        //        string Is_PageLoad = string.Empty;
        //        DataTable dt = new DataTable();

        //        //if (model.Is_PageLoad != "1")
        //        //    Is_PageLoad = "Ispageload";

        //        ViewData["ModelData"] = model;
        //       // string Userid = Convert.ToString(Session["userid"]);

        //       // String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                
                
        //        SqlCommand sqlcmd = new SqlCommand();
        //        SqlConnection sqlcon = new SqlConnection(con);
        //        sqlcon.Open();
        //        sqlcmd = new SqlCommand("PRC_LMS_CATEGORYMASTER", sqlcon);
        //        sqlcmd.Parameters.Add("@ACTION", "GETLISTINGDETAILS");
        //        sqlcmd.Parameters.Add("@USER_ID", Userid);
        //        sqlcmd.Parameters.Add("@ISPAGELOAD", model.Is_PageLoad);
        //        sqlcmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
        //        da.Fill(dt);
        //        sqlcon.Close();
        //        return PartialView("PartialCategorylisting", LGetCountryDetailsList(Is_PageLoad));

        //    }
        //    catch
        //    {
        //        return RedirectToAction("Logout", "Login", new { Area = "" });
        //    }
        //}

        //public IEnumerable LGetCountryDetailsList(string Is_PageLoad)
        //{
        //    //string connectionString = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
        //    string Userid = Convert.ToString(SessionOptions["userid"]);
        //    if (Is_PageLoad != "Ispageload")
        //    {
        //        LMSMasterDataContext dc = new LMSMasterDataContext(connectionString);
        //        var q = from d in dc.LMS_CATEGORYMASTERLISTs
        //                where d.USERID == Convert.ToInt32(Userid)
        //                orderby d.SEQ ascending
        //                select d;
        //        return q;
        //    }
        //    else
        //    {
        //        LMSMasterDataContext dc = new LMSMasterDataContext(connectionString);
        //        var q = from d in dc.LMS_CATEGORYMASTERLISTs
        //                where d.SEQ == 0
        //                select d;
        //        return q;
        //    }
        //}
    }
}
