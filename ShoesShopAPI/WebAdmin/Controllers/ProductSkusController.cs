using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseService.Entities;

namespace WebAdmin.Controllers
{
    public class ProductSkusController : Controller
    {
        private ShoematicContext db = new ShoematicContext();

        // GET: ProductSkus
        public ActionResult Index()
        {
            var productSkus = db.ProductSkus.Include(p => p.Product);
            productSkus = productSkus.OrderBy(p => p.Product.Name);
            return View(productSkus.ToList());
        }
        
        // GET: ProductSkus/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            return View();
        }

        // POST: ProductSkus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,Size,UnitPrice,Active")] ProductSku productSku)
        {
            if (ModelState.IsValid)
            {
                db.ProductSkus.Add(productSku);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productSku.ProductID);
            return View(productSku);
        }

        // GET: ProductSkus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSku productSku = db.ProductSkus.Find(id);
            if (productSku == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productSku.ProductID);
            return View(productSku);
        }

        // POST: ProductSkus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,Size,UnitPrice,Active")] ProductSku productSku)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productSku).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productSku.ProductID);
            return View(productSku);
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
