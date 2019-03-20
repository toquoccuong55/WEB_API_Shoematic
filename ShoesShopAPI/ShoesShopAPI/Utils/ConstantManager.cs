using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ShoesShopAPI.Utils
{
    public static class ConstantManager
    {
        public const int STT_SUCCESS = 0;
        public const int STT_FAIL = 1;
        public const int STT_MISSING_PARAM = 2;
        public const int STT_UNAUTHORIZED = 3;
        public const int STATUS_SUCCESS = 200;

        public const string MES_LOGIN_SUCCESS = "Login successfully";
        public const string MES_LOGIN_FAIL = "Login fail";
        public const string MES_UPDATE_SUCCESS = "Update successfully";
        public const string MES_UPDATE_FAIL = "Update fail";
        public const string MES_GET_PRODUCT_SUCCESS = "Get Products successfully";
        public const string MES_GET_PRODUCT_FAIL = "Get Products fail";
        public const string MES_SET_ORDER_SUCCESS = "Set Orders successfully";
        public const string MES_SET_ORDER_FAIL = "Set Orders fail";

        public const string MES_GET_ORDER_SUCCESS = "Get Order History successfully";
        public const string MES_GET_ORDER_FAIL = "Get Orders History Failed";

        private const string privateKey = "SHOEMATICKEY";
        public static byte[] secretKey = Encoding.UTF8.GetBytes(privateKey);
    }
}