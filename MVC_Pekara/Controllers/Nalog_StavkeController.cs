using Microsoft.Reporting.WebForms;
using MVC_Pekara.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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
		public ActionResult Rpt_NalStBrN(int brojNaloga)
		{
			DataSetNalStBrN ds = new DataSetNalStBrN();


			var viewer = new ReportViewer();

			viewer.ProcessingMode = ProcessingMode.Local;

			var connectionString = ConfigurationManager.ConnectionStrings["VG_DatabaseConnectionString"].ConnectionString;

			SqlConnection conx = new SqlConnection(connectionString);
			
			SqlCommand comm = new SqlCommand("GetNalStBrN", conx);
			comm.CommandType = CommandType.StoredProcedure;

			SqlDataAdapter adp = new SqlDataAdapter(comm);
			
			comm.Parameters.Add("@brojNaloga", SqlDbType.Int);
			comm.Parameters["@brojNaloga"].Value = brojNaloga;
			adp.SelectCommand = comm;

			adp.Fill(ds, "GetNalStBrN");

			viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ReportNalStBrN.rdlc";
			viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetNalStBrN", ds.Tables[0]));

			viewer.SizeToReportContent = true;
			viewer.ZoomMode = ZoomMode.PageWidth;
			viewer.Width = Unit.Percentage(100);
			viewer.Height = Unit.Percentage(100);


			ViewBag.ReportViewer = viewer;



			return View();
		}

	}
}