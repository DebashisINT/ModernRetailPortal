using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class BeatPlanModel
    {
        public string AssignedTo { get; set; }
        public string AssignedEMPCode { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string date_AssignedFrom { get; set; }
        public string date_AssignedTo { get; set; }
        public string Beat_Old { get; set; }
        public string Area_Old { get; set; }
        public string Route_Old { get; set; }
        public string Remarks { get; set; }
        public string Empcode { get; set; }
        public string BranchId { get; set; }
        public string Is_PageLoad { get; set; }


        public string Beat { get; set; }
        public string BeatId { get; set; }
        public List<clsBeat> BeatList { get; set; }

        public string Area { get; set; }
        public string AreaId { get; set; }
        public List<clsArea> AreaList { get; set; }

        public string Route { get; set; }
        public string RouteId { get; set; }
        public List<clsRoute> RouteList { get; set; }
    }

    public class clsBeat
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class clsArea
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class clsRoute
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class saveBeatPlan
    {
        public string Mode { get; set; }
        public string PLAN_ID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Plan { get; set; }
        public string EmpCntId { get; set; }
        public string BeatId { get; set; }
        public string RouteId { get; set; }
        public string AreaId { get; set; }
        public string BeatNameOld { get; set; }
        public string RouteNameOld { get; set; }
        public string AreaNameOld { get; set; }
        public string Remarks { get; set; }
        public string EMPNAME { get; set; }

    }

    public class EmployeeMasterReport
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fromdate { get; set; }



        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Todate { get; set; }


        public List<string> BranchId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<string> desgid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public List<string> empcode { get; set; }

        public List<GetDesignation> designation { get; set; }

        public List<GetAllEmployee> employee { get; set; }

        public List<GetBranch> modelbranch = new List<GetBranch>();

        public string is_pageload { get; set; }

    }

    public class GetDesignation
    {
        public string desgid { get; set; }

        public string designame { get; set; }
    }

    public class GetAllEmployee
    {

        public string empcode { get; set; }

        public string empname { get; set; }
    }

    public class GetBranch
    {
        public Int64 BRANCH_ID { get; set; }
        public string CODE { get; set; }
    }
    public class GetUsersStates
    {
        public string ID { get; set; }
        public string StateName { get; set; }
    }


}