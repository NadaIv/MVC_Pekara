using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Pekara.Controllers
{
    public class Nalog_StavkeController : Controller
    {
		public ActionResult Index()
		{
			return View();
		}

		public JsonResult getArtikalKategorijas()
		{
			List<Kategorije> kategorijes = new List<Kategorije>();
			using (VG_DatabaseEntities dc = new VG_DatabaseEntities())
			{
				kategorijes = dc.Kategorijes.OrderBy(a => a.NazivKategorije).ToList();
			}
			return new JsonResult { Data = kategorijes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		public JsonResult getArtikals(int kategorijaID)
		{
			List<Artikli> artikals = new List<Artikli>();
			using (VG_DatabaseEntities dc = new VG_DatabaseEntities())
			{
				artikals = dc.Artiklis.Where(a => a.KategorijaID.Equals(kategorijaID)).OrderBy(a => a.NazivArtikla).ToList();
			}
			return new JsonResult { Data = artikals, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		[HttpPost]
		public JsonResult save(NalogProizvodnje order)
		{
			bool status = false;
			DateTime dateOrg;
			var isValidDate = DateTime.TryParseExact(order.DatumNalogaString, "mm-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out dateOrg);
			if (isValidDate)
			{
				order.DatumNaloga = dateOrg;
			}

			var isValidModel = TryUpdateModel(order);
			if (isValidModel)
			{
				using (VG_DatabaseEntities dc = new VG_DatabaseEntities())
				{
					dc.NalogProizvodnjes.Add(order);
					dc.SaveChanges();
					status = true;
				}
			}
			return new JsonResult { Data = new { status = status } };
		}
	}
}