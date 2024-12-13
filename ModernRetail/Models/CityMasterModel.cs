using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using DevExpress.Map.Kml.Model;
using DevExpress.PivotGrid.OLAP;

namespace ModernRetail.Models
{
    public class CityMasterModel
    {
        public Int64 Country { get; set; }

        public Int64 State { get; set; }

        public string City { get; set; }

        public string CityLocationLat { get; set; }

        public string CityLocationLong { get; set; }


        public List<COUNTRYLIST> COUNTRYLIST { get; set; }
        public List<STATELIST> STATELIST { get; set; }
        //
        public string Is_PageLoad { get; set; }
        public int SaveCity(Int64 STATEID, string CITY_NAME, string City_Lat, string City_Long, string Userid, string ID = "0")
        {
            ProcedureExecute proc;
            try
            {
                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_MR_CITYMASTER", sqlcon);
                if (ID == "0")
                    sqlcmd.Parameters.AddWithValue("@ACTION", "ADD");
                else
                    sqlcmd.Parameters.AddWithValue("@ACTION", "UPDATE");

                sqlcmd.Parameters.AddWithValue("@STATEID", STATEID);
                sqlcmd.Parameters.AddWithValue("@CITY_NAME", CITY_NAME);
                sqlcmd.Parameters.AddWithValue("@City_Lat", City_Lat);
                sqlcmd.Parameters.AddWithValue("@City_Long", City_Long);
                sqlcmd.Parameters.AddWithValue("@USER_ID", Userid);
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

        public int Delete(string ID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_CITYMASTER");
            proc.AddNVarcharPara("@action", 50, "DELETE");
            proc.AddNVarcharPara("@ID", 30, ID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public DataTable EditCity(string ID)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            DataTable dt = new DataTable();
            try
            {
                using (proc = new ProcedureExecute("PRC_MR_CITYMASTER"))
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
                using (proc = new ProcedureExecute("PRC_MR_CITYMASTER"))
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

    public class STATELIST
    {
        public Int64 ID { get; set; }
        public string NAME { get; set; }
    }
}