using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DatabaseService.Entities;
using Newtonsoft.Json.Linq;

namespace WebAdmin.Controllers
{
    public class OrdersController : Controller
    {
        private ShoematicContext db = new ShoematicContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer);

            orders = orders.OrderByDescending(o => o.CreatedDate);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            double totalAmount = 0;
            foreach (var item in order.OrderDetails)
            {
                totalAmount += item.Quantity * item.UnitPrice;
            }

            ViewBag.TotalAmount = totalAmount;

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.Status = Utils.ConstantManager.ORDER_STATUS_CONFIRMED;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            //send notification for android app
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers["Authorization"] = "key=AAAAEjoDnxI:APA91bFPPbDKjaEjOMAmdJ-fEM4e8AJJZte5Hsp1oR2O-y3JXvGVfhKtzBYume1jtTOmHSLm9uPKDZzTBwoDVbcV1uZ4lIc7Ig5yfubj6YXWv8uRPFrPT7R7ubSwAZwFwcx1Zl5Obpk_";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string body = "{\n" +
                            "  \"to\": \n" +
                            "    \"/topics/SHOEMATICS\"\n" +
                            "  ,\n" +
                            "  \"data\": {\n" +
                            "    \"title\": \"Đơn hàng đã thanh toán\",\n" +
                            "    \"message\": \"Nhấn vào để xem chi tiết\"\n" +
                            "  },\n" +
                            "  \"notification\": {\n" +
                            "    \"title\": \"Đơn hàng đã thanh toán\",\n" +
                            "    \"text\": \"Nhấn vào để xem chi tiết\"\n" +
                            "  }\n" +
                            "}";

                streamWriter.Write(body);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string messageID = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var parsedObject = JObject.Parse(result);

                messageID = parsedObject["message_id"].ToString();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
