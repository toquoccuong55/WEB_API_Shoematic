using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesShopAPI.Models
{
    public class OrderDetailViewModel
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public string imageURL { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
    }
}