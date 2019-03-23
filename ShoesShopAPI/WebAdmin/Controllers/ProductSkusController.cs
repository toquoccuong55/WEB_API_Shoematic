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

        public ActionResult IndexByProductID(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productSkus = db.ProductSkus.Include(p => p.Product).Where(p => p.ProductID == id);
            if (productSkus == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = id;
            return View(productSkus.ToList());
        }

        // GET: ProductSkus/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Where(p => p.ID == id);
            ViewBag.ProductID = new SelectList(product, "ID", "Name");
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
                return RedirectToAction("IndexByProductID", "ProductSkus", new { id = productSku.ProductID });
            }
            var product = db.Products.Where(p => p.ID == productSku.ProductID);
            ViewBag.ProductID = new SelectList(product, "ID", "Name", productSku.ProductID);
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
            var product = db.Products.Where(p => p.ID == productSku.ProductID);
            ViewBag.ProductID = new SelectList(product, "ID", "Name", productSku.ProductID);
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
                return RedirectToAction("IndexByProductID", "ProductSkus", new { id = productSku.ProductID });
            }
            var product = db.Products.Where(p => p.ID == productSku.ProductID);
            ViewBag.ProductID = new SelectList(product, "ID", "Name", productSku.ProductID);
            return View(productSku);
        }

        // GET: ProductSkus/Delete/5
        public ActionResult Delete(int? id)
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
            return View(productSku);
        }

        // POST: ProductSkus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSku productSku = db.ProductSkus.Find(id);
            db.ProductSkus.Remove(productSku);
            db.SaveChanges();
            return RedirectToAction("IndexByProductID", "ProductSkus", new { id = productSku.ProductID });
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
