using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Pekara;

namespace MVC_Pekara.Controllers
{
    public class KategorijeController : Controller
    {
        private VG_DatabaseEntities db = new VG_DatabaseEntities();

        // GET: Kategorije
        public ActionResult Index()
        {
            return View(db.Kategorijes.ToList());
        }

        // GET: Kategorije/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategorije kategorije = db.Kategorijes.Find(id);
            if (kategorije == null)
            {
                return HttpNotFound();
            }
            return View(kategorije);
        }

        // GET: Kategorije/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategorije/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KategorijaID,NazivKategorije")] Kategorije kategorije)
        {
            if (ModelState.IsValid)
            {
                db.Kategorijes.Add(kategorije);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kategorije);
        }

        // GET: Kategorije/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategorije kategorije = db.Kategorijes.Find(id);
            if (kategorije == null)
            {
                return HttpNotFound();
            }
            return View(kategorije);
        }

        // POST: Kategorije/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KategorijaID,NazivKategorije")] Kategorije kategorije)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kategorije).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategorije);
        }

        // GET: Kategorije/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategorije kategorije = db.Kategorijes.Find(id);
            if (kategorije == null)
            {
                return HttpNotFound();
            }
            return View(kategorije);
        }

        // POST: Kategorije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kategorije kategorije = db.Kategorijes.Find(id);
            db.Kategorijes.Remove(kategorije);
            db.SaveChanges();
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
