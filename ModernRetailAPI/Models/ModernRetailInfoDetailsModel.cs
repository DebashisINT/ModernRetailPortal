#region======================================Revision History=========================================================
//Written By : Debashis Talukder On 09/12/2024
//Purpose: Modern Retail Info Details.Row: 3,4,5,6,7,11,12,13
#endregion===================================End of Revision History==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernRetailAPI.Models
{
    public class StoreTypeListInput
    {
        public long user_id { get; set; }
    }
    public class StoreTypeListOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<StoretypelistOutput> store_type_list { get; set; }
    }
    public class StoretypelistOutput
    {
        public long type_id { get; set; }
        public string type_name { get; set; }
    }
    public class StoreInfoSaveInput
    {
        public long user_id { get; set; }
        public List<StoreSavelists> store_list { get; set; }
    }
    public class StoreSavelists
    {
        public string store_id { get; set; }
        public long branch_id { get; set; }
        public string store_name { get; set; }
        public string store_address { get; set; }
        public string store_pincode { get; set; }
        public string store_lat { get; set; }
        public string store_long { get; set; }
        public string store_contact_name { get; set; }
        public string store_contact_number { get; set; }
        public string store_alternet_contact_number { get; set; }
        public string store_whatsapp_number { get; set; }
        public string store_email { get; set; }
        public long store_type { get; set; }
        public string store_size_area { get; set; }
        public int store_state_id { get; set; }
        public string remarks { get; set; }
        public string create_date_time { get; set; }
    }
    public class StoreInfoSaveOutput
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    public class StoreImageSaveInput
    {
        public string data { get; set; }
        public HttpPostedFileBase attachments { get; set; }
    }
    public class StoreImageSaveInputDetails
    {
        public long user_id { get; set; }
        public string store_id { get; set; }
    }
    public class StoreImageSaveOutput
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    public class StoreInfoFetchListsInput
    {
        public long user_id { get; set; }
    }
    public class StoreInfoFetchListsOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public long user_id { get; set; }
        public List<StoreFetchlists> store_list { get; set; }
    }
    public class StoreFetchlists
    {
        public string store_id { get; set; }
        public long branch_id { get; set; }
        public string store_name { get; set; }
        public string store_address { get; set; }
        public string store_pincode { get; set; }
        public string store_lat { get; set; }
        public string store_long { get; set; }
        public string store_contact_name { get; set; }
        public string store_contact_number { get; set; }
        public string store_alternet_contact_number { get; set; }
        public string store_whatsapp_number { get; set; }
        public string store_email { get; set; }
        public long store_type { get; set; }
        public string store_size_area { get; set; }
        public int store_state_id { get; set; }
        public string remarks { get; set; }
        public string create_date_time { get; set; }
        public string store_pic_url { get;set; }
    }
    public class StoreInfoEditInput
    {
        public long user_id { get; set; }
        public List<StoreEditlists> store_list { get; set; }
    }
    public class StoreEditlists
    {
        public string store_id { get; set; }
        public long branch_id { get; set; }
        public string store_name { get; set; }
        public string store_address { get; set; }
        public string store_pincode { get; set; }
        public string store_lat { get; set; }
        public string store_long { get; set; }
        public string store_contact_name { get; set; }
        public string store_contact_number { get; set; }
        public string store_alternet_contact_number { get; set; }
        public string store_whatsapp_number { get; set; }
        public string store_email { get; set; }
        public long store_type { get; set; }
        public string store_size_area { get; set; }
        public int store_state_id { get; set; }
        public string remarks { get; set; }
        public string edit_date_time { get; set; }
    }
    public class StoreInfoEditOutput
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    public class PinCityStateListInput
    {
        public long user_id { get; set; }
    }
    public class PinCityStateListOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<PinCityStlistOutput> pin_state_list { get; set; }
    }
    public class PinCityStlistOutput
    {
        public int pin_id { get; set; }
        public string pincode { get; set; }
        public int city_id { get; set; }
        public string city_name { get; set; }
        public int state_id { get; set; }
        public string state_name { get; set; }
    }
    public class ProductDetailListInput
    {
        public long user_id { get; set; }
    }
    public class ProductDetailListOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<ProductlistOutput> product_list { get; set; }
    }
    public class ProductlistOutput
    {
        public long product_id { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public int brand_id { get; set; }
        public string brand_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int watt_id { get; set; }
        public string watt_name { get; set; }
        public decimal product_mrp { get; set; }
        public string UOM { get; set; }
        public string product_pic_url { get; set; }
    }

    public class ProductRateListInput
    {
        public long user_id { get; set; }
    }
    public class ProductRateListOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<ProdRatelistOutput> product_rate_list { get; set; }
    }
    public class ProdRatelistOutput
    {
        public long product_id { get; set; }
        public int state_id { get; set; }
        public decimal rate { get; set; }
    }
    public class ProductUomListInput
    {
        public long user_id { get; set; }
    }
    public class ProductUomListOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<ProdUomlistOutput> uom_list { get; set; }
    }
    public class ProdUomlistOutput
    {
        public long product_id { get; set; }
        public long uom_id { get; set; }
        public string uom_name { get; set; }
    }
    public class UserBranchListInput
    {
        public long user_id { get; set; }
    }
    public class UserBranchListOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<UserBranlistOutput> branch_list { get; set; }
    }
    public class UserBranlistOutput
    {
        public long branch_id { get; set; }
        public string branch_name { get; set; }
    }
    public class StockInfoSaveInput
    {
        public long user_id { get; set; }
        public string stock_id { get; set; }
        public string save_date_time { get; set; }
        public string store_id { get; set; }
        public List<StockSavelists> product_list { get; set; }
    }
    public class StockSavelists
    {
        public string stock_id { get; set; }
        public long product_id { get; set; }
        public decimal qty { get; set; }
        public long uom { get; set; }
        public string mfg_date { get; set; }
        public string expire_date { get; set; }
    }
    public class StockInfoSaveOutput
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    public class StockInfoFetchListsInput
    {
        public long user_id { get; set; }
    }
    public class StockInfoFetchListsOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public long user_id { get; set; }
        public List<StocklistOutput> stock_list { get; set; }        
    }

    public class StocklistOutput
    {        
        public string stock_id { get; set; }
        public string save_date_time { get; set; }
        public string store_id { get; set; }
        public List<StockProductlists> product_list { get; set; }
    }
    public class StockProductlists
    {
        public string stock_id { get; set; }
        public long product_id { get; set; }
        public decimal qty { get; set; }
        public long uom { get; set; }
        public string mfg_date { get; set; }
        public string expire_date { get; set; }
    }
}