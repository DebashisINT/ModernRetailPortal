#region======================================Revision History=========================================================
//Written By : Debashis Talukder On 09/12/2024
//Purpose: LMS Info Details.Row: 2
#endregion===================================End of Revision History==================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModernRetailAPI.Models
{
    public class MDRUserLoginModel
    {
        [Required]
        public string login_id { get; set; }
        [Required]
        public string login_password { get; set; }

        public string app_version { get; set; }
        public string device_token { get; set; }
    }
    public class MDRClassLoginOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public string user_name { get; set; }
        public string user_id { get; set; }
        public string contact_number { get; set; }
        public string email { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public string address { get; set; }
        public string profile_pic_url { get; set; }
    }
}