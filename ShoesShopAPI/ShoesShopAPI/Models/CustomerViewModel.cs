using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesShopAPI.Models
{
    public class CustomerViewModel
    {
        public string access_token { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public bool first_login { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }
}