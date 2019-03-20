using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesShopAPI.Models
{
    public class ProductSkuViewModel
    {

        public int ID { get; set; }
        public int ProductID { get; set; }
        public Nullable<double> Size { get; set; }
        public Nullable<double> UnitPrice { get; set; }

    }
}