using DataAccessLayer;
using DevExpress.XtraRichEdit.Import.Html;
using ModernRetail.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace ModernRetail.Controllers
{
    public class StoreMasterController : Controller
    {
        // GET: StoreMaster
        public ActionResult Index()
        {
            StoreMasterModel dtLs = new StoreMasterModel();

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("MR_STOREMASTERADDUPDATELIST");
            proc.AddPara("@ACTION", "GETLISTDATA");
            ds = proc.GetDataSet();

            if (ds != null)
            {
                List<CountryListstore> CountryListstore = new List<CountryListstore>();
                CountryListstore = APIHelperMethods.ToModelList<CountryListstore>(ds.Tables[0]);

                List<StateListstore> StateListstore = new List<StateListstore>();
                StateListstore = APIHelperMethods.ToModelList<StateListstore>(ds.Tables[1]);

                List<StoreTypeList> StoreTypeList = new List<StoreTypeList>();
                StoreTypeList = APIHelperMethods.ToModelList<StoreTypeList>(ds.Tables[2]);

                dtLs.StateListstore = StateListstore;
                dtLs.CountryListstore = CountryListstore;
                dtLs.StoreTypeList = StoreTypeList;

            }

            return View(dtLs);
        }


        [HttpPost]
        public ActionResult GetState(string Country)
        {
            List<StateListstore> StateLst = new List<StateListstore>();
            DataTable dtState = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("MR_STOREMASTERADDUPDATELIST");
            proc.AddPara("@ACTION", "GetStateListCountryWise");
            proc.AddPara("@CountryId", Country);
            dtState = proc.GetTable();

            if (dtState.Rows.Count > 0)
            {
                StateLst = APIHelperMethods.ToModelList<StateListstore>(dtState);
            }
            return Json(StateLst, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetCity(string State)
        {
            List<CityListstore> CityLst = new List<CityListstore>();
            DataTable dtCity = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("MR_STOREMASTERADDUPDATELIST");
            proc.AddPara("@ACTION", "GetCityListStateWise");
            proc.AddPara("@stateId", State);
            dtCity = proc.GetTable();
            if (dtCity.Rows.Count > 0)
            {
                CityLst = APIHelperMethods.ToModelList<CityListstore>(dtCity);
            }
            return Json(CityLst, JsonRequestBehavior.AllowGet);

        }
    }
}