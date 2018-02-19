using System;

namespace WebUI.ViewModels
{
    public class OrderVM
    {
        public int OrderID { get; set; }
        public int userId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}