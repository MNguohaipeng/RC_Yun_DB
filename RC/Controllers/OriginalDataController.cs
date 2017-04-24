using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RC.Controllers
{
	public class OriginalDataController : Controller
	{
		// GET: OriginalData
		public ActionResult embalmed()
		{
			return View();
		}


		[HttpPost]
		public object embalmed(FormCollection fm)
		{

			string errMsg;

			object data;

			object json = "";

			switch (fm["action"])
			{
				case "N_XF_SY_001"://男士西服上衣解析

					if (!N_XF_SY_001(out errMsg, out data))
					{

						throw new Exception(errMsg);

					}
					else
					{

						json = Json(new { total = 1, rows = data }, JsonRequestBehavior.AllowGet);

					}

					break;

				case "N_XF_KZ_001"://男士西服裤子解析

					if (!Comparison.N_XF_KZ_001(Request.Files, out errMsg, out data))
					{

						throw new Exception(errMsg);

					}
					else
					{

						json = Json(new { total = 1, rows = data }, JsonRequestBehavior.AllowGet);

					}

					break;

				default:

					break;
			}

			return json;

		}



		//男西服上衣001解析
		public bool N_XF_SY_001(out string errmsg, out object retData)
		{
			try
			{
				errmsg = "";

				retData = null;

				HttpFileCollectionBase files = Request.Files;

				#region 表单验证

				if (files.Count <= 0)
				{
					errmsg = "请上传要处理的文件。";
					return false;

				}

				#endregion

				string url;


				#region 上传文件

				Common.Common.UpLoadFile(files[0], "File" + DateTime.Now.ToString("sshhffffff") + ".xls", "~/Data", out url, out errmsg);

				#endregion

				#region 解析excel

				DataTable table = ExcelHelper.InputFromExcel(Server.MapPath(url), "原计划");

				#endregion

				return true;


			}
			catch (Exception ex)
			{
				errmsg = ex.Message;
				retData = null;
				return false;
			}

		}


	}


}