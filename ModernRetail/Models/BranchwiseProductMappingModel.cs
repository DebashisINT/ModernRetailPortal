using DataAccessLayer;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class BranchwiseProductMappingModel
    {
        public Int64 PRODUCTBRANCHMAP_ID { get; set; }
        public string Is_PageLoad { get; set; }
        public string Branch_Ids { get; set; }
        public string PARENTERMP_IDS { get; set; }

        public string CHILDERMP_IDS { get; set; }

        public string PRODUCT_IDS { get; set; }
        public string HeadBranch {  get; set; }

        public Int64 HeadBranchID { get; set; }
        public List<BranchList> HeadBranchList { get; set; }
        public class GetBranchList
        {
            public Int64 branch_id { get; set; }
            public string branch_code { get; set; }           
            public string branch_description { get; set; }

        }

        public class GetParentEmployeeList
        {
            public Int64 USER_ID { get; set; }
            public Int64 USER_REPORTTO_ID { get; set; }            
            public string USER_LOGINID { get; set; }
            public string USER_NAME { get; set; }
            public string DESIGNATION { get; set; }
            

        }

        public class GetProductList
        {
            public Int64 SPRODUCTS_ID { get; set; }
            public string SPRODUCTS_NAME { get; set; }
            public string SPRODUCTS_CODE { get; set; }
            public string CLASSCODE { get; set; }
            public string BRANDNAME { get; set; }
        }

        public DataSet InsertUpdate(String action, Int64 PRODUCTBRANCHMAP_ID, string Branch_Ids, string PARENTERMP_IDS, string CHILDERMP_IDS, string PRODUCT_IDS,
        Int64 userid = 0
         )
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");

            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddBigIntegerPara("@PRODUCTBRANCHMAP_ID", PRODUCTBRANCHMAP_ID);
            proc.AddVarcharPara("@BRANCHID", 4000, Branch_Ids);
            proc.AddVarcharPara("@PRODUCTID",4000, PRODUCT_IDS);           
            proc.AddVarcharPara("@ParentEMPID", 4000, PARENTERMP_IDS);
            proc.AddVarcharPara("@ChildEMPID",4000, CHILDERMP_IDS);           
            proc.AddBigIntegerPara("@USERID", userid);
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
                using (proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING"))
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

        public int Delete(string ID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING");
            proc.AddNVarcharPara("@action", 50, "DELETE");
            proc.AddBigIntegerPara("@PRODUCTBRANCHMAP_ID", Convert.ToInt64(ID));
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }


        public DataSet FETCHBRANCHMAP(string action,Int64 ID)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            DataSet dt = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("PRC_MR_BRANCHWISEPRODUCTMAPPING"))
                {
                    proc.AddBigIntegerPara("@PRODUCTBRANCHMAP_ID",ID);
                    proc.AddVarcharPara("@ACTION", 100, action);
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
    }
}