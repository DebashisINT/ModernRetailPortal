using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class SecurityRolesModel
    {
        public int grp_id { get; set; }

        public int grp_segmentId { get; set; }

        public string grp_name { get; set; }

        public int? CreateUser { get; set; }

        public int? LastModifyUser { get; set; }

        public string UserGroupRights { get; set; }

        public string mode { get; set; }

        public DataSet Edit(string ID)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            DataSet dt = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("PRC_MR_SECURITYROLESDETAILS"))
                {
                    proc.AddVarcharPara("@ID", 100, ID);
                    proc.AddVarcharPara("@ACTION", 100, "EDIT");
                    dt = proc.GetDataSet();
                    return dt;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public int Delete(string ID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_SECURITYROLESDETAILS");
            proc.AddNVarcharPara("@ACTION", 50, "DELETE");
            proc.AddNVarcharPara("@ID", 30, ID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
    }
   
}