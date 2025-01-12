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

        //public Int32 Country { get; set; }
        //public Int32 State { get; set; }

        //public List<string> StateIds { get; set; }
        //public string StateID { get; set; }
        //public List<StateListstore> StateListstore { get; set; }

        //public string CountryID { get; set; }
        //public List<CountryListstore> CountryListstore { get; set; }
        public string Store_Name { get; set; }
        public string Address { get; set;}
        // public string Pin_Code {  get; set; }

        public Int32 Pin { get; set; }

        public List<string> PinIds { get; set; }
        public string PinID { get; set; }
        public List<PinList> PinList { get; set; }

        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        //public bool IsActive { get; set; }

        public string Branch { get; set; }
        public string BranchID { get; set; }
        public List<BranchList> BranchList { get; set; }
        public List<int> SelectedBranchIDs { get; set; }

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

    public class PinList
    {
        public string pin_id { get; set;}
        public string pin_code { get; set;}
    }

    public class CountryStateCity
    {
        public string CountryName { get; set;}
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string StateId { get; set;}

    }

    //public class StateListstore
    //{
    //    public string StateID { get; set; }
    //    public string StateName { get; set; }
    //}

    //public class CityListstore
    //{
    //    public string CityID { get; set; }
    //    public string CityName { get; set; }
    //}

    //public class CountryListstore
    //{
    //    public String cou_id { get; set; }
    //    public String cou_country { get; set; }
    //}

    public class StoreTypeList
    {
        public string storetype_id { get; set; }
        public string storetype_name { get; set;}
    }

    public class AddStoreInputData
    {
        public string data { get; set; }
        public HttpPostedFileBase shop_image { get; set; }
    }

    public class AddStoreImageInputData
    {
        public string data { get; set; }
        public HttpPostedFileBase store_image { get; set; }
    }

    public class StoreInfoSaveInput
    {
        public long user_id { get; set; }
        public List<StoreSavelists> store_list { get; set; }
    }

    public class StoreSavelists
    {
        public string store_id { get; set; }
        public string store_name { get; set; }
        public string store_address { get; set; }
        public string store_pincode { get; set; }
        public string store_branch { get; set; }
        public string store_status { get; set; }
        public string store_lat { get; set; }
        public string store_long { get; set; }
        public string store_contact_name { get; set; }
        public string store_contact_number { get; set; }
        public string store_alternet_contact_number { get; set; }
        public string store_whatsapp_number { get; set; }
        public string store_email { get; set; }
        public string store_type { get; set; }
        public string store_size_area { get; set; }
        public string store_state_id { get; set; }
        public string remarks { get; set; }
        public string create_date_time { get; set; }
        public string created_userid { get; set; }
        public string created_userid_old { get; set; }
        public string created_username { get; set; }
        public string created_useridname_old { get; set; }
        public string store_pic_url { get; set; }


    }

    

}