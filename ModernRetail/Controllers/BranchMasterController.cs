using BusinessLogicLayer.SalesmanTrack;
using DataAccessLayer;
using DevExpress.XtraExport;
using ModernRetail.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using UtilityLayer;

namespace ModernRetail.Controllers
{
    public class BranchMasterController : Controller
    {
        // GET: BranchMaster
        BranchMaster objdata = null;
        Int64 DetailsID = 0;
        string BranchName = string.Empty;
        public BranchMasterController()
        {
            objdata = new BranchMaster();
        }
        public ActionResult Index()
        {


            DataSet dt = new DataSet();
            dt = GetListData();

            if (dt != null)
            {
                List<BranchList> BranchList = new List<BranchList>();
                BranchList = APIHelperMethods.ToModelList<BranchList>(dt.Tables[0]);
                objdata.BranchList = BranchList;

                

                List<CountryList> Country_List = new List<CountryList>();
                Country_List = APIHelperMethods.ToModelList<CountryList>(dt.Tables[1]);
                objdata.CountryList = Country_List;

            }

            return View(objdata);
        }
        public DataSet GetListData()
        {
            DataSet dt = new DataSet();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHDETAILS");
            proc.AddPara("@ACTION", "GETDETAILS");
            dt = proc.GetDataSet();
            return dt;
        }
        
        public JsonResult GetState(string CountryID)
        {

            DataTable DT = new DataTable();
            if (CountryID != "")
            {
                DT = GetStateData(CountryID);
            }

            return Json(APIHelperMethods.ToModelList<BranchList>(DT));

        }
        public  DataTable GetStateData(string CountryID)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHDETAILS");
            proc.AddPara("@ACTION", "GETSTATE");
            proc.AddBigIntegerPara("@countryId", Convert.ToInt64(CountryID));
            dt = proc.GetTable();
            return dt;
        }

        public JsonResult GETCITY(string StateID)
        {

            DataTable DT = new DataTable();
            if (StateID != "")
            {
                DT = GETCITYDATA(StateID);
            }

            return Json(APIHelperMethods.ToModelList<BranchList>(DT));

        }
        public DataTable GETCITYDATA(string StateID)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHDETAILS");
            proc.AddPara("@ACTION", "GETCITY");
            proc.AddBigIntegerPara("@state_id", Convert.ToInt64(StateID));
            dt = proc.GetTable();
            return dt;
        }


        public JsonResult GETPINZIP(string CityID)
        {

            DataTable DT = new DataTable();
            if (CityID != "")
            {
                DT = GETPINZIPDATA(CityID);
            }

            return Json(APIHelperMethods.ToModelList<BranchList>(DT));

        }
        public DataTable GETPINZIPDATA(string CityID)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHDETAILS");
            proc.AddPara("@ACTION", "GETPINZIP");
            proc.AddBigIntegerPara("@city_id", Convert.ToInt64(CityID));
            dt = proc.GetTable();
            return dt;
        }

        [WebMethod]
        public JsonResult SaveBranch(BranchMaster Details)
        {
            String Message = "";
            Boolean Success = false;
            DataSet dt = new DataSet();
            


            if (Convert.ToInt64(Details.branch_ID) > 0 && Convert.ToInt16(TempData["IsView"]) == 0)
            {
                dt = objdata.BranchEntryInsertUpdate("UPDATEBRANCH", Convert.ToInt64(Details.branch_ID), Details.ShortName, Convert.ToInt64(Details.ParentBranch), Details.BranchName, Details.Address1,
                    Convert.ToInt64(Details.Country), Convert.ToInt64(Details.State), Convert.ToInt64(Details.City), Convert.ToInt64(Details.PIN)
                       , Convert.ToInt64(Session["userid"]));
            }
            else
            {
                dt = objdata.BranchEntryInsertUpdate("INSERTBRANCH", Convert.ToInt64(Details.branch_ID), Details.ShortName, Convert.ToInt64(Details.ParentBranch), Details.BranchName, Details.Address1,
                    Convert.ToInt64(Details.Country), Convert.ToInt64(Details.State), Convert.ToInt64(Details.City), Convert.ToInt64(Details.PIN)
                       , Convert.ToInt64(Session["userid"]));

            }


            if (dt != null && dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dt.Tables[0].Rows)
                {
                    Success = Convert.ToBoolean(row["Success"]);
                    DetailsID = Convert.ToInt64(row["DetailsID"]);
                    BranchName = Convert.ToString(Details.BranchName);
                }
            }
            
            String retuenMsg = Success + "~" + DetailsID + "~" + Details.BranchName + "~" + Message;
            return Json(retuenMsg);

        }


    }


}