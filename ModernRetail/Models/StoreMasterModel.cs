using DevExpress.Xpo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class StoreMasterModel
    {
        public string StoreTypeId { get; set; }
        public string Is_PageLoad { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }

        public Int32 Country { get; set; }
        public Int32 State { get; set; }

        public List<string> StateIds { get; set; }
        public string StateID { get; set; }
        public List<StateListstore> StateListstore { get; set; }

        public string CountryID { get; set; }
        public List<CountryListstore> CountryListstore { get; set; }
        public string Store_Name { get; set; }
        public string Address { get; set;}
        public string Pin_Code {  get; set; }

        public string StoreType { get; set;}
        public List<StoreTypeList> StoreTypeList { get; set; }

        public string Area { get; set;}
        public string location_lat { get; set;}
        public string location_long { get; set;}
        public string Remarks { get; set;}
        public string AssignedTo { get; set;}
        public string NewUserid { get; set;}
        public string OldUserid { get; set;}
        public string contact_name { get; set;}
        public string contact_no { get; set;}
        public string Alternate_Contact { get; set;}
        public string Whatsapp_Number { get; set;}
        public string owner_email { get; set;}
    }

    public class StateListstore
    {
        public string StateID { get; set; }
        public string StateName { get; set; }
    }

    public class CityListstore
    {
        public string CityID { get; set; }
        public string CityName { get; set; }
    }

    public class CountryListstore
    {
        public String cou_id { get; set; }
        public String cou_country { get; set; }
    }

    public class StoreTypeList
    {
        public string storetype_id { get; set; }
        public string storetype_name { get; set;}
    }
}