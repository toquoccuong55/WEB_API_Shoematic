using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesShopAPI.Models
{
    public class OrderViewModel
    {
        public string AccessToken { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentType { get; set; }
        public string Note { get; set; }
        public int OrderId { get; set; }
        public string OrderTime { get; set; }
        public int Status { get; set; } 
        public double TotalAmount { get; set; }

        public List<OrderDetailViewModel> OrderDetailList { get; set; }
    }
}