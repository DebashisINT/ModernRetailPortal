using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class ProductRateModel
    {
        public decimal SpecialPrice { get; set; }
        public string Employee { get; set; }
        public string Is_PageLoad { get; set; }
        public Int64 branch_ID { get; set; }

        public string Designation { get; set; }

        public string Product { get; set; }

        public Int64 ID { get; set; }

        public List<BranchList> BranchList { get; set; }

        

        public DataSet ProductRateEntryInsertUpdate(String action, Int64 ID, Int64 branch_ID, Int32 Designation, Int64 Employee, Int64 Product,
                    String SpecialPrice,
            Int64 userid = 0
           )
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");

            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddBigIntegerPara("@ID", ID);           
            proc.AddBigIntegerPara("@BRANCH_ID", branch_ID);
            proc.AddBigIntegerPara("@Designation", Designation);           
            proc.AddBigIntegerPara("@Employee", Employee);
            proc.AddBigIntegerPara("@Product", Product);
            proc.AddVarcharPara("@SpecialPrice",100, SpecialPrice);          
            proc.AddBigIntegerPara("@USER_ID", userid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet EditProductRate(string ID)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            DataSet dt = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("PRC_MR_PRODUCTRATES"))
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

        public int DeleteBranch(string ID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");
            proc.AddNVarcharPara("@action", 50, "DELETE");
            proc.AddBigIntegerPara("@ID", Convert.ToInt64(ID));
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
    }

    public class EmployeeModel
    {
        public string id { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Code { get; set; }
    }

    public class DESIGNATIONLIST
    {
        public Int64 ID { get; set; }
        public String NAME { get; set; }

    }

    public class ProductModel
    {
        public string id { get; set; }
        public string Na { get; set; }
        public string Des { get; set; }
        // public string MinSalePrice { get; set; }
    }

}