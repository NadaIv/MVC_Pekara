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




	}
}