using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesShopAPI.Utils
{
    public static class Utilities
    {
        public static string GenerateToken(string ID)
        {
            var payload = ID + ":" + DateTime.Now.Ticks.ToString();

            return JWT.Encode(payload, ConstantManager.secretKey, JwsAlgorithm.HS256);
        }

        public static string getCustomerIDFromToken(string token) {
            string key = JWT.Decode(token, ConstantManager.secretKey);
            string[] parts = key.Split(new char[] { ':'});
            return parts[0];
        }
    }
}