﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernRetailAPI.Models
{
    public class MDRUserLoginModel
    {
    }
    public class MDRUserClass
    {
        public string user_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string success { get; set; }
        public string address { get; set; }
        public int country { get; set; }
        public int state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }

    }
    public class MDRClassLoginOutput
    {
        public string status { get; set; }
        public string message { get; set; }
        public string session_token { get; set; }

        public MDRUserClass user_details { get; set; }

        //public UserClasscounting user_count { get; set; }
        public List<StateListLogin> state_list { get; set; }
    }
}