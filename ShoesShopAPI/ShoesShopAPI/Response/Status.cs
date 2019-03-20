using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesShopAPI.Response
{
    public class Status
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int status { get; set; }

    }
}