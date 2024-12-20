using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class StateMasterModel
    {
        public Int64 Country { get; set; }

        public string State { get; set; }

        public string StateCode { get; set; }
        public List<COUNTRYLIST> COUNTRYLIST { get; set; }
        //
        public string Is_PageLoad { get; set; }
        public int SaveState(string COUNTRYID, string user, string ID = "0", string State = "", string StateCode = "0")
        {
            ProcedureExecute proc;
            try
            {
                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_MR_STATEMASTER", sqlcon);
                if (ID == "0")
                    sqlcmd.Parameters.AddWithValue("@ACTION", "ADD");
                else
                    sqlcmd.Parameters.AddWithValue("@ACTION", "UPDATE");
                sqlcmd.Parameters.AddWithValue("@STATENAME", State);
                sqlcmd.Parameters.AddWithValue("@STATECODE", StateCode);
                sqlcmd.Parameters.AddWithValue("@COUNTRYID", COUNTRYID);
                sqlcmd.Parameters.AddWithValue("@USER_ID", user);               
                sqlcmd.Parameters.AddWithValue("@ID", ID);

                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                sqlcmd.Parameters.Add(output);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                Int32 ReturnValue = Convert.ToInt32(output.Value);
                sqlcmd.Dispose();
                sqlcmd.Dispose();
                return ReturnValue;

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

        public int DeleteState(string ID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_STATEMASTER");
            proc.AddNVarcharPara("@action", 50, "DELETE");
            proc.AddNVarcharPara("@ID", 30, ID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public DataTable EditState(string ID)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            DataTable dt = new DataTable();
            try
            {
                using (proc = new ProcedureExecute("PRC_MR_STATEMASTER"))
                {
                    proc.AddVarcharPara("@ID", 100, ID);
                    proc.AddVarcharPara("@ACTION", 100, "EDIT");
                    dt = proc.GetTable();
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


        public DataTable GetCountry()
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            DataTable dt = new DataTable();
            try
            {
                using (proc = new ProcedureExecute("PRC_MR_STATEMASTER"))
                {
                    proc.AddVarcharPara("@ACTION", 100, "GetCountry");
                    dt = proc.GetTable();
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
    }
    public class COUNTRYLIST
    {
        public Int64 ID { get; set; }
        public string NAME { get; set; }
    }
}