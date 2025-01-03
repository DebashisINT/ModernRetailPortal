﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class CurrentStockImportLogModel
    {
        public String Branch { get; set; }
        public String ShopName { get; set; }
        public String Code { get; set; }
        public String ContactNumber { get; set; }
        public String Shoptype { get; set; }
        public String CurrentStockDate { get; set; }
        public String ProductCode { get; set; }
        public String ProductName { get; set; }
        public String Quantity { get; set; }
        public String ImportStatus { get; set; }
        public String ImportMsg { get; set; }
        public String ImportDate { get; set; }
        public String CreateUser { get; set; }

        public String StoreId { get; set; }

        public String ProductId { get; set; }



        public int StockBranchId { get; set; }
        public List<StockBranchList> StockBranchList { get; set; }

    }

    public class StockBranchList
    {
        public String branch_id { get; set; }
        public String branch_code { get; set; }
        public String branch_description { get; set; }
    }

    public class AddCurrentStockData
    {
        public string Is_PageLoad { get; set; }
        public String Action { get; set; }
        public String StockId { get; set; }
        public String BranchID { get; set; }
        public String ShopCode { get; set; }
        public String ProductID { get; set; }
        public String CurrentStockDate { get; set; }
        public String Quantity { get; set; }

        public String ShopName { get; set; }
        public String ProductName { get; set; }


    }
}