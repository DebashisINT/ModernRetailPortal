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
        //public string Employee { get; set; }
        public string Is_PageLoad { get; set; }

        public string Product_Ids { get; set; }
        public Int64 branch_ID { get; set; }

        //public string Designation { get; set; }

        public string Product { get; set; }

        public Int64 ID { get; set; }

        public List<BranchList> BranchList { get; set; }

        public String StoreName { get; set; }

        public String StoreId { get; set; }

        public DataSet ProductRateEntryInsertUpdate(String action, Int64 ID, Int64 branch_ID, string StoreId, Int64 Product,
                    String SpecialPrice,
            Int64 userid = 0
           )
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_PRODUCTRATES");

            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddBigIntegerPara("@ID", ID);           
            proc.AddBigIntegerPara("@BRANCH_ID", branch_ID);
            proc.AddVarcharPara("@StoreId",500, StoreId);
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

   
    public class DESIGNATIONLIST
    {
        public Int64 ID { get; set; }
        public String NAME { get; set; }

    }
    public class ProductRateImportLog
    {
        public string BRANCH { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }     
       
        public decimal SPECIALPRICE { get; set; }
       
        public string USERLOGINID { get; set; }
       

        public string ImportStatus { get; set; }
        public string ImportMsg { get; set; }

        public DateTime ImportDate { get; set; }
        public string UpdatedBy { get; set; }

    }


}