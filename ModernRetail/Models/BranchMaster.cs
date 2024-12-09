using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class BranchMaster
    {
        public string Is_PageLoad { get; set; }
        public Int64 branch_ID { get; set; }
        public string ShortName { get; set; }
        public string ParentBranch { get; set; }
        public List<BranchList> BranchList { get; set; }

        public string BranchName { get; set; }

        
        public string Address1 { get; set; }

        
        public List<CountryList> CountryList { get; set; }
        public string Country { get; set; }

        public List<BranchList> StateList { get; set; }
        public string State { get; set; }

        public List<BranchList> CityList { get; set; }
        public string City { get; set; }

        public List<BranchList> AreaList { get; set; }
        public string Area { get; set; }      

        public List<BranchList> PINList { get; set; }
        public string PIN { get; set; }

        public string BranchPhone { get; set; }

        public DataSet BranchEntryInsertUpdate(String action,  Int64 branch_ID, string ShortName, Int64 ParentBranch, string BranchName, string Address1,
                    Int64 Country, Int64 State, Int64 City, Int64 PIN,
            Int64 userid = 0
           )
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHDETAILS");

            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddVarcharPara("@branch_code", 100, ShortName);            
            proc.AddVarcharPara("@BranchName", 100, BranchName);
            proc.AddBigIntegerPara("@BRANCH_ID", branch_ID);
            proc.AddBigIntegerPara("@branch_parentId", ParentBranch);
            proc.AddVarcharPara("@branch_address1", 100, Address1);
            proc.AddBigIntegerPara("@countryId", Country);
            proc.AddBigIntegerPara("@state_id", State);
            proc.AddBigIntegerPara("@city_id", City);
            proc.AddBigIntegerPara("@branch_pin", PIN);
            proc.AddBigIntegerPara("@USER_ID", userid);            
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet EditBranch(string ID)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            DataSet dt = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("PRC_MR_BRANCHDETAILS"))
                {
                    proc.AddVarcharPara("@BRANCH_ID", 100, ID);
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

        public int DeleteBranch(string ID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_LMS_QUESTIONS");
            proc.AddNVarcharPara("@action", 50, "DELETE");
            proc.AddBigIntegerPara("@BRANCH_ID",Convert.ToInt64(ID));
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
    }
    public class BranchList
    {
        public Int64 ID { get; set; }
        public String NAME { get; set; }

    }

    public class CountryList
    {
        public Int64 ID { get; set; }
        public String NAME { get; set; }

    }


}