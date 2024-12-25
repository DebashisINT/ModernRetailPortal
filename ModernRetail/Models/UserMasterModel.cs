using DevExpress.Map.Kml.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class UserMasterModel
    {
        public Int64 user_id {  get; set; }
        public string Is_PageLoad { get; set; }
        public string UserName { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Branch { get; set; }
        public string BranchID { get; set; }
        public List<BranchList> BranchList { get; set; }
        public List<int> SelectedBranchIDs { get; set; }

        public string Department { get; set; }
        public string DeparmentID { get; set; }
        public List<DepartmentList> DepartmentList { get; set; }

        public string Designation { get; set; }
        public string DesignationID { get; set; }
        public List<DesignationList> DesignationList { get; set; }

        public string ReportTo { get; set; }
        public string ReportToCode { get; set; }

        public string Group { get; set; }
        public string GroupID { get; set; }
        public List<GroupList> GroupList {  get; set; }
        public string Address { get; set; }

        public string Country { get; set; }
        public string CountryID { get; set; }
        public List<CountryList> CountryList { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public bool IsActive { get; set; }
    }

    public class Employeels
    {
        public string id { get; set; }
        public string Name { get; set; }
    }
    public class DepartmentList
    {
        public Int64 ID { get; set; }
        public String NAME { get; set; }

    }

    public class DesignationList
    {
        public Int64 ID { get; set; }
        public String NAME { get; set; }

    }

    public class GroupList
    {
        public Int64 ID { get; set; }
        public String NAME { get; set; }

    }

    public class AttendHomeLocList
    {
        public Int64 ID { get; set; }
        public String NAME { get; set; }

    }
    public class PartyTypeList
    {
        public Int64 ID { get; set; }
        public String NAME { get; set; }

    }

    public class UserMasterAddEditModel
    {
        public Int64 user_id { get; set; }
        public Int64 UserId {  get; set; }
        public string UserName { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Branch { get; set; }
        public List<BranchList> BranchList { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ReportTo { get; set; }
        public string Group { get; set; }
        public string Address { get; set; }

        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PIN { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }

}