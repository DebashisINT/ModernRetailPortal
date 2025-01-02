using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class WarehouseMasterModel
    {
        public String WarehouseID { get; set; }
        public String WarehouseName { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String Address3 { get; set; }
        public String Country { get; set; }
        public String State { get; set; }
        public String CityDistrict { get; set; }
        public String Pin { get; set; }
        public String ContactName { get; set; }
        public String ContactPhone { get; set; }
        public String Distributer { get; set; }
        public String isDefault { get; set; }

        public List<string> shopId { get; set; }

        public List<Country_List> Country_List { get; set; }
        public List<States_List> State_List { get; set; }
        public List<CityDistrictList> CityDistrict_List { get; set; }
        public List<DistributerList> Distributer_List { get; set; }
    }

    public class EmployeeListModel
    {
        public List<string> StateId { get; set; }
        public List<string> desgid { get; set; }
        public List<string> shopId { get; set; }

        public List<string> CityId { get; set; }

        public List<string> AreaId { get; set; }

        public List<string> CountryId { get; set; }

        public List<string> DeptId { get; set; }

        public string userId { get; set; }
    }

    public class Getmasterstock
    {
        public string ID { get; set; }
        public string Name { get; set; }

    }

    public class Country_List
    {
        public String cou_id { get; set; }
        public String cou_country { get; set; }
    }

    public class States_List
    {
        public String ID { get; set; }
        public String Name { get; set; }
    }

    public class CityDistrictList
    {
        public String city_id { get; set; }
        public String city_name { get; set; }
    }

    public class DistributerList
    {
        public String Shop_Code { get; set; }
        public String Shop_Name { get; set; }
    }

    public class ShopList
    {
        public String Shop_code { get; set; }
        public String Shop_name { get; set; }
        public String Type { get; set; }
        public String ContactNo { get; set; }
        public String ReportTo { get; set; }
        public String State { get; set; }
        public String Address { get; set; }
    }
}
