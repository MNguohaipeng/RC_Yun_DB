﻿using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RC.Controllers
{
	public class ExeclController : Controller
	{
		#region load
		// GET: Execl
		[CxAttribute]
		public ActionResult import_jacket()
		{

			return View();

		}
		[CxAttribute]
		public ActionResult import_trousers()
		{
			return View();
		}

		#endregion
 

		#region 处理Excel数据-西服
		[HttpPost]
		public ActionResult import_jacket(FormCollection fm)
		{
			try
			{
				DataTable table = null;

				if (fm["size_gender"] == "男")
				{
					table = Analysis.Excel_analysis_NAN(Request.Files, fm["size_gender"]);
				}
				else
				{

					table = Analysis.Excel_analysis_NU(Request.Files, fm["size_gender"]);

				}
				if (fm["import"] == "false")
				{
					return Json(new { state = 1, msg = Comparison.Ret_Excel(table) }, JsonRequestBehavior.AllowGet);
				}
				else
				{

					string errmsg;

					if (import.Import_Excel_jacket(table, fm["size_code"], fm["size_gender"], out errmsg))
					{

						return Json(new { state = "success", msg = "" }, JsonRequestBehavior.AllowGet);

					}
					else
					{

						return Json(new { state = "error", msg = errmsg }, JsonRequestBehavior.AllowGet);

					}
				}
			}
			catch (Exception ex)
			{
				return Json(new { state = "error", msg = ex.Message }, JsonRequestBehavior.AllowGet);
			}

		}

		#endregion

		#region 处理Excel数据-西裤

		[HttpPost]
		public ActionResult import_trousers(FormCollection fm)
		{

			DataTable table = Analysis.Excel_trousrs_Code(Request.Files);

			if (fm["import"] == "false")
			{

				return Json(new { total = 1, rows = Comparison.Ret_Excel_trousrs(table) }, JsonRequestBehavior.AllowGet);

			}
			else
			{

				string errmsg;

				if (import.Import_Excel_trousrs(table, fm["Size_Code"], out errmsg))
				{

					return Json(new { state = "success", msg = "" }, JsonRequestBehavior.AllowGet);

				}
				else
				{

					return Json(new { state = "error", msg = errmsg }, JsonRequestBehavior.AllowGet);

				}
			}

		}
		#endregion

	}
}