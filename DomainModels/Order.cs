using System;

namespace DomainModels
{
    public class Order
    {
        public int OrderID { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PayPalReference { get; set; }
    }
}
