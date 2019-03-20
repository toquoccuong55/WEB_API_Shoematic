using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesShopAPI.Response
{
    public class JSONFormat<T>
    {
        public Status status;
        public T data;
    }
}