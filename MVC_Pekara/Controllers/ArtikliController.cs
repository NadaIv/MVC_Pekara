using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using MVC_Pekara;
using MVC_Pekara.Reports;

namespace MVC_Pekara.Controllers
{
    public class ArtikliController : Controller
    {
        private VG_DatabaseEntities db = new VG_DatabaseEntities();

		// GET: Artikli
		/*   public ActionResult Index()
		   {
			   return View(db.Artiklis.ToList());
		   }*/

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult GetArtiklis()
		{
			using (VG_DatabaseEntities db = new VG_DatabaseEntities())
			{

				var artiklis = (from a in db.Artiklis
								 join k in db.Kategorijes
								 on a.KategorijaID equals k.KategorijaID
								 select new
								 {
									 a.ArtikalID,
									 a.NazivArtikla,
									 a.Masa,
									 a.TipBrasna,
									 a.ProizvodjackaCenaBezPDV,
									 a.ProizvodjackaCenaSaPDV,
									 a.KategorijaID,
									 k.NazivKategorije

								 }).ToList();

				return Json(new { data = artiklis }, JsonRequestBehavior.AllowGet);
			}
		}


		// GET: Artikli/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artikli artikli = db.Artiklis.Find(id);
            if (artikli == null)
            {
                return HttpNotFound();
            }
            return View(artikli);
        }

        // GET: Artikli/Create
        public ActionResult Create()
        {
			ViewBag.KategorijaID = new SelectList(db.Kategorijes, "KategorijaID", "NazivKategorije");

			return View();
        }

        // POST: Artikli/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtikalID,KategorijaID,NazivArtikla,Masa,TipBrasna,ProizvodjackaCenaBezPDV,ProizvodjackaCenaSaPDV")] Artikli artikli)
        {
            if (ModelState.IsValid)
            {
                db.Artiklis.Add(artikli);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artikli);
        }

        // GET: Artikli/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artikli artikli = db.Artiklis.Find(id);
            if (artikli == null)
            {
                return HttpNotFound();
            }
			ViewBag.KategorijaID = new SelectList(db.Kategorijes, "KategorijaID", "NazivKategorije");
			return View(artikli);
        }

        // POST: Artikli/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtikalID,KategorijaID,NazivArtikla,Masa,TipBrasna,ProizvodjackaCenaBezPDV,ProizvodjackaCenaSaPDV")] Artikli artikli)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artikli).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artikli);
        }

        // GET: Artikli/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artikli artikli = db.Artiklis.Find(id);
            if (artikli == null)
            {
                return HttpNotFound();
            }
            return View(artikli);
        }

        // POST: Artikli/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artikli artikli = db.Artiklis.Find(id);
            db.Artiklis.Remove(artikli);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
		

		public ActionResult KatArt(int artikalID)
		{
			DataSetKatArt ds = new DataSetKatArt();


			var viewer = new ReportViewer();

			viewer.ProcessingMode = ProcessingMode.Local;

			var connectionString = ConfigurationManager.ConnectionStrings["VG_DatabaseConnectionString"].ConnectionString;

			SqlConnection conx = new SqlConnection(connectionString);

			SqlCommand comm = new SqlCommand("GetKatArt", conx);
			comm.CommandType = CommandType.StoredProcedure;

			SqlDataAdapter adp = new SqlDataAdapter(comm);

			comm.Parameters.Add("@artikalID", SqlDbType.Int);
			comm.Parameters["@artikalID"].Value = artikalID;
			adp.SelectCommand = comm;

			adp.Fill(ds, "GetKatArt");

			viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ReportKatArt.rdlc";
			viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetKatArt", ds.Tables[0]));

			viewer.SizeToReportContent = true;
			viewer.ZoomMode = ZoomMode.PageWidth;
			viewer.Width = Unit.Percentage(100);
			viewer.Height = Unit.Percentage(100);


			ViewBag.ReportViewer = viewer;



			return View();
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
