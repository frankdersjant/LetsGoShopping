using System;
using System.Collections.Generic;

namespace WebUI.ViewModels
{
    public class OrdersForUserVM
    {
        public int OrderNumber { get; set; }
        public decimal Total { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, int> ProductsandQTY { get; set; }
    }
}