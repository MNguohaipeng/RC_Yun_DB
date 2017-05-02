using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using Common;
using Entity;
using System.Data;

namespace RC.Controllers
{
	public class SizeManageController : Controller
	{
		[CxAttribute]
		// GET: SizeManage
		public ActionResult List(FormCollection fm)
		{

			try
			{

				if (string.IsNullOrEmpty(fm["Size_Code"]))
				{
					ViewBag.state = "1";
				}
				else
				{
					var list = Load_List(fm);

					if (list != null)
					{

						ViewBag.List = list;

						ViewBag.state = "0";

					}
					else
					{

						ViewBag.state = "1";

					}
				}
			}
			catch (Exception ex)
			{
				ViewBag.Message = ex.Message;
			}
			return View();
		}


		public JsonResult GetCoatSize(int limit, int offset, string size_code, string statu)
		{
			using (var db = SugarDao.GetInstance())
				try
				{
					if (string.IsNullOrEmpty(statu)) {
						statu = "";
                    }
					var total = db.Queryable<XF_SY_NAN_CodeSize>().Where(T => T.Size_Code == size_code && (T.NetBust.Contains(statu) || T.FrontLength.Contains(statu))).Count();
					var rows = db.Queryable<XF_SY_NAN_CodeSize>().Where(T =>T.Size_Code==size_code && (T.NetBust.Contains(statu) || T.FrontLength.Contains(statu))).OrderBy(it => it.ID).Skip(offset).Take(limit).ToList();
					return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
				}
				catch (Exception ex)
				{

					return Json(new { total = 1, rows = "{'errmsg':," + ex.Message + "}" }, JsonRequestBehavior.AllowGet);

				}

		}

		//加载尺码编号
		public object Query_Code(FormCollection fm)
		{
			var list = new object();

			DataTable size_code_table = null;

			using (var db = SugarDao.GetInstance())
				try
				{
					List<String> codelist = new List<String>();

					if (!string.IsNullOrEmpty(fm["gender"]))
					{

						if (fm["gender"] == "男")
						{

							codelist = db.GetList<String>("select Size_Code from XF_SY_NAN_CodeSize group by Size_Code").ToList();

						}
						else
						{
							size_code_table = db.GetDataTable("select Size_Code from XF_SY_NU_CodeSize group by Size_Code");
						}
					}
					else
					{
						throw new Exception("请选择适用性别。");
					}

					return Json(new { msg = codelist }, JsonRequestBehavior.AllowGet);
				}
				catch (Exception ex)
				{

					throw;

				}

		}

		//加载列表
		public object Load_List(FormCollection fm)
		{
			var list = new object();

			using (var db = SugarDao.GetInstance())
				try
				{

					string par1 = fm["Size_Code"];




					list = db.Queryable<XF_SY_NAN_CodeSize>().Where(it => it.Size_Code == par1.ObjToString()).ToList();

				}
				catch (Exception ex)
				{

					throw;

				}

			return list;

		}


		public ActionResult SizeCheckList(FormCollection fm)
		{

			using (var db=SugarDao.GetInstance())
				try
				{
					List<SizeCheck> list = new List<SizeCheck>();

					list = db.Queryable<SizeCheck>().ToList();
					ViewBag.list = list;

				}
				catch (Exception)
				{

 
					throw;
				}

			return View();

		}
	}

}