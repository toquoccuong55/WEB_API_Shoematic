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
    public class ProductImagesController : Controller
    {
        private ShoematicContext db = new ShoematicContext();
        
        public ActionResult IndexByProductID(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productImages = db.ProductImages.Include(p => p.Product).Where(p => p.ProductID == id);
            if (productImages == null)
            {
                return HttpNotFound();
            }
            productImages = productImages.OrderBy(p => p.Product.Name);
            ViewBag.ProductID = id;
            return View(productImages.ToList());
        }

        // GET: ProductImages/Create
        public ActionResult Create(int? id)
        {
            var product = db.Products.Where(p => p.ID == id);
            ViewBag.ProductID = new SelectList(product, "ID", "Name");
            return View();
        }

        // POST: ProductImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,PicURL")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                db.ProductImages.Add(productImage);
                db.SaveChanges();
                return RedirectToAction("IndexByProductID", "ProductImages",new { id = productImage.ProductID});
            }

            var product = db.Products.Where(p => p.ID == productImage.ProductID);
            ViewBag.ProductID = new SelectList(product, "ID", "Name", productImage.ProductID);
            return View(productImage);
        }

        // GET: ProductImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            var product = db.Products.Where(p => p.ID == productImage.ProductID);
            ViewBag.ProductID = new SelectList(product, "ID", "Name", productImage.ProductID);
            return View(productImage);
        }

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,PicURL")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexByProductID", "ProductImages", new { id = productImage.ProductID });
            }
            var product = db.Products.Where(p => p.ID == productImage.ProductID);
            ViewBag.ProductID = new SelectList(product, "ID", "Name", productImage.ProductID);
            return View(productImage);
        }

        // GET: ProductImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductImage productImage = db.ProductImages.Find(id);
            db.ProductImages.Remove(productImage);
            db.SaveChanges();
            return RedirectToAction("IndexByProductID", "ProductImages", new { id = productImage.ProductID});
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
