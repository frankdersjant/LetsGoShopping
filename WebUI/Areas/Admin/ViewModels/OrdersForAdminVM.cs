using System;
using System.Collections.Generic;

namespace WebUI.Areas.Admin.ViewModels
{
    public class OrdersForAdminVM
    {
        public int OrderNumber { get; set; }
        public string UserName { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int>  ProductsandQTY { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}