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

                ProcedureExecute proc = new ProcedureExecute("PRC_UserNameSearch");
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

        public class UsersModel
        {
            public string USER_ID { get; set; }
            public string USER_NAME { get; set; }
            public string USER_LOGINID { get; set; }
        }

    }
}
