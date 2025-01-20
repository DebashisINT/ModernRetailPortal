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
    public class MasterPopulateController : Controller
    {
        MasterPolulate lstuser = new MasterPolulate();

        public ActionResult GetChildBranchList(string Hoid)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            try
            {
                List<GetBranch> modelbranch = new List<GetBranch>();
                DataTable ComponentTable = new DataTable();
        
                if (Hoid == null || Hoid == "")
                {
                    Hoid = Session["Hoid"].ToString();
                }
                if (Hoid != "0")
                {
                    Session["Hoid"] = Hoid;
                    ComponentTable = lstuser.GetBranch(Convert.ToString(Session["userbranchHierarchy"]), Hoid);
                }
                else
                {
                    Session["Hoid"] = Hoid;
                    ComponentTable = oDBEngine.GetDataTable("select distinct * from (select branch_id as BRANCH_ID,branch_description as CODE,branch_code from MR_MASTER_BRANCH a where a.branch_id in (1,119)  union all select branch_id as BRANCH_ID,branch_description as CODE,branch_code from MR_MASTER_BRANCH b where b.branch_parentId in (1,119)) a order by CODE");
                }
                modelbranch = APIHelperMethods.ToModelList<GetBranch>(ComponentTable);
                return PartialView("~/Views/SearchingInputs/_HeadBranchPartial.cshtml", modelbranch);

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult checkSessionLogout()
        {
            SessionLogoutCheck ret = new SessionLogoutCheck();

            if (Session["userid"] == null)
            {
                ret.SessionLoddedOut = "1";
            }
            else
            {
                ret.SessionLoddedOut = "0";
            }

            return Json(ret, JsonRequestBehavior.AllowGet);

        }
        
    }
}