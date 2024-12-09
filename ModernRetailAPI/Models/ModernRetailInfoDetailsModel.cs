#region======================================Revision History=========================================================
//Written By : Debashis Talukder On 09/12/2024
//Purpose: Modern Retail Info Details.Row: 3
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
}