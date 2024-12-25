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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetOnDemandReportTo(string SearchKey)
        {
            List<Employeels> listEmployee = new List<Employeels>();
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("MR_FetchReportTo");
            proc.AddPara("@action", "GETONDEMANDREPORTTO");
            proc.AddPara("@userid", Convert.ToString(Session["userid"]));
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
    }
}
