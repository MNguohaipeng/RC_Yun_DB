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
			DataTable table = Excel_analysis_trousrs();

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
					else {
						json=Json(new { total = 1, rows = data }, JsonRequestBehavior.AllowGet);
					}

					break;

				case "N_XF_KZ_001"://男士西服裤子解析

					if (!N_XF_KZ_001(out errMsg, out data)) { 
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

		//男士西服裤子001解析
		public bool N_XF_KZ_001(out string errMsg, out object data)
		{
			try
			{
				errMsg = "";

				data = null;

				DataTable table = Excel_analysis_trousrs();

				List<OriginalDataTrousers> cslist = new List<OriginalDataTrousers>();

				for (int i = 0; i < table.Rows.Count; i++)
				{
					decimal ty = 0;

					OriginalDataTrousers ts = new OriginalDataTrousers();

					ts.Order_no = table.Rows[i]["order_no"] + "";

					int tyint = 0;

					if (int.TryParse(table.Rows[i]["order_xc"] + "", out tyint))
					{

						ts.Order_xc = tyint;

					}
					else
					{

						ts.Order_xc = 0;

					}

					ts.Name = table.Rows[i]["name"] + "";

					ts.Ret_code_size = table.Rows[i]["ret_code_size"] + "";

					if (int.TryParse(table.Rows[i]["number"] + "", out tyint))
					{

						ts.Number = tyint;

					}
					else
					{

						ts.Number = 0;

					}
					ts.Remarks = table.Rows[i]["remarks"] + "";

					cslist.Add(ts);
				}

				data = cslist;



				DataTable rettable = new DataTable();
				List<Entity.Ret_CodeTrousers> ct = new List<Ret_CodeTrousers>();
				for (int i = 0; i < table.Rows.Count; i++)
				{
					System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"//^\s*[\u4E00-\u9FA5\s]\s*[0-9]\s*[0-9]\s*[0-9_a-z_A-Z]\s*");
					if (reg1.IsMatch(table.Rows[i]["ret_code_size"] + "")) {
						DataRow rerow = rettable.NewRow();
						rettable.Rows.Add(rerow);
                    }
				}




				return true;

			}
			catch (Exception)
			{

				throw;
			}
		}


		//男西服上衣001解析
		public bool N_XF_SY_001(out string errMsg, out object retData)
		{
			try
			{
				errMsg = "";

				retData = null;

				HttpFileCollectionBase files = Request.Files;

				#region 表单验证

				if (files.Count <= 0)
				{

					return false;

				}

				#endregion

				string url;

				string errmsg;

				#region 上传文件

				Common.Common.UpLoadFile(files[0], "File" + DateTime.Now.ToString("sshhffffff") + ".xls", "~/Data", out url, out errmsg);

				#endregion

				#region 解析excel

				DataTable table = ExcelHelper.InputFromExcel(Server.MapPath(url), "原计划");

				#endregion

				return true;


			}
			catch (Exception)
			{

				throw;
			}

		}












		public DataTable Excel_analysis_trousrs()
		{

			try
			{

				DataTable excelTable = new DataTable();
				excelTable.Columns.Add("order_no");
				excelTable.Columns.Add("order_xc");
				excelTable.Columns.Add("name");
				excelTable.Columns.Add("ret_code_size");
				excelTable.Columns.Add("number");
				excelTable.Columns.Add("remarks");

				#region 上传excel到服务器 并处理  返回datatable
				string url;
				string errmsg;

				HttpFileCollectionBase files = Request.Files;

				Common.Common.UpLoadFile(files[0], "File" + DateTime.Now.ToString("sshhffffff") + ".xls", "~/Data", out url, out errmsg);


				DataTable table = ExcelHelper.InputFromExcel(Server.MapPath(url), "原计划");



				#endregion

				string gdbh = "";//工单编号  1
				string cplh = "";//成品料号  2
				string scsl = "";//生产数量  3
				string pm = "";//品    名  4
				string gg = ""; //规 格  5
				int index = 0;
				#region 解析返回得excel数据
				for (int i = 0; i < table.Rows.Count; i++)
				{
					#region 获取表头变量
					for (int b = 0; b < table.Columns.Count; b++)
					{
						if (gg != "")//前5个变量都取完了  关掉循环  
						{
							break;
						}


						if (!string.IsNullOrEmpty(table.Rows[i][b].ToString()))
						{

							if (index != 0)
							{
								switch (index)
								{
									case 1:
										gdbh = table.Rows[i][b].ToString();
										break;
									case 2:
										cplh = table.Rows[i][b].ToString();
										break;
									case 3:
										scsl = table.Rows[i][b].ToString();
										break;
									case 4:
										pm = table.Rows[i][b].ToString();
										break;
									case 5:
										gg = table.Rows[i][b].ToString();


										break;

								}

								index = 0;
							}



							switch (table.Rows[i][b].ToString().Trim())
							{
								case "工单编号:":
									index = 1;
									break;
								case "成品料号:":
									index = 2;
									break;
								case "生产数量:":
									index = 3;
									break;
								case "品    名:":
									index = 4;
									break;
								case "规    格:":
									index = 5;
									break;
							}



						}

					}
					#endregion

					#region 主体数据部分

					if (table.Rows[i]["F2"].ToString() == "制单人")
						break;


					if (table.Rows[i]["F2"].ToString() != "" && table.Rows[i]["F2"].ToString() != "订单编号" && table.Rows[i]["F2"].ToString() != "工单尺码汇总表")
					{
						DataRow row = excelTable.NewRow();
						row["order_no"] = table.Rows[i]["F2"].ToString();
						row["order_xc"] = table.Rows[i]["F6"].ToString();
						row["name"] = table.Rows[i]["F8"].ToString();
						row["ret_code_size"] = table.Rows[i]["F10"].ToString();
						row["number"] = table.Rows[i]["F16"].ToString();
						row["remarks"] = table.Rows[i]["F19"].ToString();
						excelTable.Rows.Add(row);

					}


					#endregion
				}



				#endregion

				return excelTable;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}


}