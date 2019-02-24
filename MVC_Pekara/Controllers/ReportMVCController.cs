using Microsoft.Reporting.WebForms;
using MVC_Pekara.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace MVC_Pekara.Controllers
{
    public class ReportMVCController : Controller
    {
	
        // GET: ReportMVC
        public ActionResult Index()
        {
			
			return View();
        }

		

		public ActionResult Rpt_Artikli()
		{
			DataSetArtikli ds = new DataSetArtikli();
			//var dataSetArtikli = new Reports.DataSetArtikli();
			//	dataSetArtikli.Artikli.AddArtikliRow( 1, "Bio hleb",500,"500",21,25);

			var viewer = new ReportViewer();

			viewer.ProcessingMode = ProcessingMode.Local;

			var connectionString = ConfigurationManager.ConnectionStrings["VG_DatabaseConnectionString"].ConnectionString;
			SqlConnection conx = new SqlConnection(connectionString);
			SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Artikli", conx);

			adp.Fill(ds, ds.Artikli.TableName);

			viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ReportArtikli.rdlc";
			viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetArtikli", ds.Tables[0]));

			viewer.SizeToReportContent = true;
			viewer.ZoomMode = ZoomMode.PageWidth;
			viewer.Width = Unit.Percentage(100);
			viewer.Height = Unit.Percentage(100);

			ViewBag.ReportViewer = viewer;
			
			

			return View();
		}

		public ActionResult NalSt()
		{
			DataSetNalSt ds = new DataSetNalSt();

			var viewer = new ReportViewer();

			viewer.ProcessingMode = ProcessingMode.Local;

			var connectionString = ConfigurationManager.ConnectionStrings["VG_DatabaseConnectionString"].ConnectionString;
			SqlConnection conx = new SqlConnection(connectionString);
			
			SqlCommand cmd = new SqlCommand("GetNalSt",conx);
			cmd.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter adp = new SqlDataAdapter(cmd);

			adp.Fill(ds,"GetNalSt");

			viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ReportNalSt.rdlc";
			viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetNalSt", ds.Tables[0]));
		

			viewer.SizeToReportContent = true;
			viewer.ZoomMode = ZoomMode.PageWidth;
			viewer.Width = Unit.Percentage(100);
			viewer.Height = Unit.Percentage(100);

			ViewBag.ReportViewer = viewer;

			return View();
		}

		

		

		}
}