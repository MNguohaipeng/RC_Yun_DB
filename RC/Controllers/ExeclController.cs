using Common;
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
		// GET: Execl
		public ActionResult import_jacket()
		{



			return View();
		}

		public ActionResult import_trousers()
		{
			return View();
		}


		#region 处理Excel数据-西服
		[HttpPost]
		public ActionResult import_jacket(FormCollection fm)
		{
			DataTable table = Excel_analysis(fm);


			if (fm["import"] == "false")
			{
				return Json(new { total = 1, rows = Ret_Excel(table) }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				string errmsg;
				if (Import_Excel_jacket(table, out errmsg))
				{
					return Json(new { state = "success", msg = "" }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { state = "error", msg = errmsg }, JsonRequestBehavior.AllowGet);

				}
			}


		}

		public object Ret_Excel(DataTable table)
		{



			List<CoatSize> cslist = new List<CoatSize>();

			for (int i = 0; i < table.Rows.Count; i++)
			{
				decimal ty = 0;
				CoatSize cs = new CoatSize();
				if (decimal.TryParse(table.Rows[i]["Height"] + "", out ty))
				{
					cs.Height = ty;
				}
				else
				{
					cs.Height = 0;
				}
				cs.FrontLength = table.Rows[i]["FrontLength"] + "";
				cs.NetBust = table.Rows[i]["NetBust"] + "";

				if (decimal.TryParse(table.Rows[i]["FinishedBust"] + "", out ty))
				{
					cs.FinishedBust = ty;
				}
				else
				{
					cs.FinishedBust = 0;
				}


				if (decimal.TryParse(table.Rows[i]["InWaist"] + "", out ty))
				{
					cs.InWaist = ty;
				}
				else
				{
					cs.InWaist = 0;
				}



				if (decimal.TryParse(table.Rows[i]["FinishedHem_NoFork"] + "", out ty))
				{
					cs.FinishedHem_NoFork = ty;
				}
				else
				{
					cs.FinishedHem_NoFork = 0;
				}

				if (decimal.TryParse(table.Rows[i]["FinishedHem_SplitEnds"] + "", out ty))
				{
					cs.FinishedHem_SplitEnds = ty;
				}
				else
				{
					cs.FinishedHem_NoFork = 0;
				}
				if (decimal.TryParse(table.Rows[i]["ShoulderWidth"] + "", out ty))
				{
					cs.ShoulderWidth = ty;
				}
				else
				{
					cs.ShoulderWidth = 0;
				}

				cs.Size_Code = table.Rows[i]["Size_Code"] + "";

				cs.Sleecve_Show = table.Rows[i]["FK_Sleeve_ID"] + "";

				cslist.Add(cs);
			}

			return cslist;


		}


		public bool Import_Excel_jacket(DataTable table, out string errmsg)
		{

			using (var db = SugarDao.GetInstance())
				try
				{

					db.CommandTimeOut = 3000;//设置超时时间

					List<CoatSize> list = new List<CoatSize>();

					db.BeginTran();

					for (int i = 0; i < table.Rows.Count; i++)
					{
						#region 处理不符合要求得数据

						DataRow row = table.Rows[i];

						row.BeginEdit();

						for (int a = 0; a < table.Columns.Count; a++)
						{

							if (row[a].ToString().IndexOf("....") > 0)
							{
								row[a] = row[a].ToString().Replace("....", ".");
							}
							if (row[a].ToString().IndexOf("...") > 0)
							{
								row[a] = row[a].ToString().Replace("...", ".");
							}
							if (row[a].ToString().IndexOf("..") > 0)
							{
								row[a] = row[a].ToString().Replace("..", ".");
							}
						}

						row.EndEdit();

						#endregion

						CoatSize cs = new CoatSize();
						cs.Height = Convert.ToDecimal(table.Rows[i]["Height"]);
						cs.FrontLength = table.Rows[i]["FrontLength"] + "";
						cs.NetBust = table.Rows[i]["NetBust"] + "";
						cs.FinishedBust = Convert.ToDecimal(table.Rows[i]["FinishedBust"]);
						cs.InWaist = Convert.ToDecimal(table.Rows[i]["InWaist"]);
						cs.FinishedHem_NoFork = Convert.ToDecimal(table.Rows[i]["FinishedHem_NoFork"]);
						cs.FinishedHem_SplitEnds = Convert.ToDecimal(table.Rows[i]["FinishedHem_SplitEnds"]);
						cs.ShoulderWidth = Convert.ToDecimal(table.Rows[i]["ShoulderWidth"]);
						cs.Size_Code = table.Rows[i]["Size_Code"] + "";
						cs.Height = Convert.ToDecimal(table.Rows[i]["Height"]);
						var ruid = db.Insert(cs, true);

						string req = table.Rows[i]["FK_Sleeve_ID"] + "";

						if (req.IndexOf("      ") > 0)
						{
							req = req.Replace("      ", "^");
						}

						if (req.IndexOf("     ") > 0)
						{
							req = req.Replace("     ", "^");
						}

						if (req.IndexOf("    ") > 0)
						{
							req = req.Replace("    ", "^");
						}

						if (req.IndexOf("   ") > 0)
						{
							req = req.Replace("   ", "^");
						}

						if (req.IndexOf("  ") > 0)
						{
							req = req.Replace("  ", "^");
						}

						req = req.Replace(" ", "^");

						req = req.Replace(":", ";");

						string[] sleeve_arrey = req.Split('^');

						Sleeve see = new Sleeve();
						see.FK_CoatSize_ID = Convert.ToInt32(ruid);
						see.Code = sleeve_arrey[0].Split(';')[0];
						see.Length = Convert.ToDecimal(sleeve_arrey[0].Split(';')[1]);
						db.Insert(see);


					}

					db.CommitTran();
					errmsg = "";
					return true;

				}
				catch (Exception ex)
				{

					db.RollbackTran();//回滚事务
					errmsg = ex.Message;
					return false;

				}
		}





		public DataTable Excel_analysis(FormCollection fm)
		{
			Dictionary<string, string> callback_json = new Dictionary<string, string>();

			DataTable execltable;
			try
			{


				string url;
				string errmsg;
				HttpFileCollectionBase files = Request.Files;

				Common.Common.UpLoadFile(files[0], "File" + DateTime.Now.ToString("sshhffffff") + ".xls", "~/Data", out url, out errmsg);


				DataTable execl_table = ExcelHelper.InputFromExcel(Server.MapPath(url), "sheet1");

				string height_o = "";
				string height_t = "";
				string length_o = "";
				string length_t = "";
				#region 创建最终table
				execltable = new DataTable();
				execltable.Columns.Add("Height");
				execltable.Columns.Add("FrontLength");
				execltable.Columns.Add("NetBust");
				execltable.Columns.Add("FinishedBust");
				execltable.Columns.Add("InWaist");
				execltable.Columns.Add("FinishedHem_NoFork");
				execltable.Columns.Add("FinishedHem_SplitEnds");
				execltable.Columns.Add("ShoulderWidth");
				execltable.Columns.Add("FK_Sleeve_ID");
				execltable.Columns.Add("Size_Code");
				#endregion


				#region 处理身高数据和袖长数据
				int[] remove_arrey = new int[execl_table.Rows.Count];

				for (int i = 0; i < execl_table.Rows.Count; i++)
				{


					if (!Common.Common.IsNatural_Number(execl_table.Rows[i][0] + ""))
					{
						remove_arrey[i] = i;

						continue;
					}


					#region 处理身高数据不全
					if (!string.IsNullOrEmpty(execl_table.Rows[i]["F2"] + "") && execl_table.Rows[i]["F2"] + "" != "身高")
					{
						height_o = execl_table.Rows[i]["F2"] + "";
					}
					else
					{
						execl_table.Rows[i]["F2"] = height_o;
					}

					#endregion

					#region 处理袖长数据不全
					if (!string.IsNullOrEmpty(execl_table.Rows[i]["F10"] + ""))
					{
						length_o = execl_table.Rows[i]["F10"] + "";
					}
					else
					{
						execl_table.Rows[i]["F10"] = length_o;
					}


					#endregion


				}
				int del_index = 0;
				for (int i = 0; i < remove_arrey.Length; i++)
				{

					if (remove_arrey[i] != 0)
					{

						execl_table.Rows.RemoveAt(remove_arrey[i] - del_index);
						del_index++;
					}
				}
				execl_table.Rows.RemoveAt(0);

				for (int i = 0; i < execl_table.Rows.Count; i++)
				{

					if (!string.IsNullOrEmpty(execl_table.Rows[i]["F13"] + "") && execl_table.Rows[i]["F13"] + "" != "身高")
					{
						height_t = execl_table.Rows[i]["F13"].ToString();
					}
					else
					{
						execl_table.Rows[i]["F13"] = height_t;
					}
					if (!string.IsNullOrEmpty(execl_table.Rows[i]["F21"] + ""))
					{
						length_t = execl_table.Rows[i]["F21"] + "";
					}
					else
					{
						execl_table.Rows[i]["F21"] = length_t;
					}
				}
				#region  处理非数据部分
				#endregion
				for (int i = 0; i < execl_table.Rows.Count; i++)

				{
					#region 将数据剥离并填入datatable
					if (!string.IsNullOrEmpty(execl_table.Rows[i][0] + ""))
					{
						DataRow row01 = execltable.NewRow();
						row01["Height"] = execl_table.Rows[i]["F2"] + "";
						row01["FrontLength"] = execl_table.Rows[i]["F3"] + "";
						row01["NetBust"] = execl_table.Rows[i]["F4"] + "";
						row01["FinishedBust"] = execl_table.Rows[i]["F5"] + "";
						row01["InWaist"] = execl_table.Rows[i]["F6"] + "";
						row01["FinishedHem_NoFork"] = execl_table.Rows[i]["F7"] + "";
						row01["FinishedHem_SplitEnds"] = execl_table.Rows[i]["F8"] + "";
						row01["ShoulderWidth"] = execl_table.Rows[i]["F9"] + "";
						row01["FK_Sleeve_ID"] = execl_table.Rows[i]["F10"] + "";

						execltable.Rows.Add(row01);
					}
				}



				for (int i = 0; i < execl_table.Rows.Count; i++)
				{
					if (!string.IsNullOrEmpty(execl_table.Rows[i]["F12"] + ""))
					{
						DataRow row02 = execltable.NewRow();
						row02["Height"] = execl_table.Rows[i]["F13"] + "";
						row02["FrontLength"] = execl_table.Rows[i]["F14"] + "";
						row02["NetBust"] = execl_table.Rows[i]["F15"] + "";
						row02["FinishedBust"] = execl_table.Rows[i]["F16"] + "";
						row02["InWaist"] = execl_table.Rows[i]["F17"] + "";
						row02["FinishedHem_NoFork"] = execl_table.Rows[i]["F18"] + "";
						row02["FinishedHem_SplitEnds"] = execl_table.Rows[i]["F19"] + "";
						row02["ShoulderWidth"] = execl_table.Rows[i]["F20"] + "";
						row02["FK_Sleeve_ID"] = execl_table.Rows[i]["F21"] + "";
						//      row02["Size_Code"] = execl_table.Rows[i]["F10"] + "";
						execltable.Rows.Add(row02);
					}
				}
				#endregion

				#endregion
				//execltable

				return execltable;
			}
			catch (Exception ex)
			{
				throw;
			}


		}

		#endregion

		#region 处理Excel数据-西裤
		[HttpPost]
		public ActionResult import_trousers(FormCollection fm)
		{
			DataTable table = Excel_analysis_trousrs(fm);


			if (fm["import"] == "false")
			{
				return Json(new { total = 1, rows = Ret_Excel_trousrs(table) }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				string errmsg;
				if (Import_Excel_trousrs(table, fm, out errmsg))
				{
					return Json(new { state = "success", msg = "" }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { state = "error", msg = errmsg }, JsonRequestBehavior.AllowGet);

				}
			}


		}

		public object Ret_Excel_trousrs(DataTable table)
		{



			List<TrousersSize> cslist = new List<TrousersSize>();

			for (int i = 0; i < table.Rows.Count; i++)
			{
				decimal ty = 0;
				TrousersSize ts = new TrousersSize();

				ts.Code = table.Rows[i]["Code"] + "";

				if (decimal.TryParse(table.Rows[i]["DZ_HipLength_CP"] + "", out ty))
				{
					ts.DZ_HipLength_CP = ty;
				}
				else
				{
					ts.DZ_HipLength_CP = 0;
				}

				ts.SZ_HipLength_CP = 0;

				if (decimal.TryParse(table.Rows[i]["SZ_HipLength_CP"] + "", out ty))
				{
					ts.SZ_HipLength_CP = ty;
				}
				else
				{
					ts.SZ_HipLength_CP = 0;
				}


				if (decimal.TryParse(table.Rows[i]["Crosspiece"] + "", out ty))
				{
					ts.Crosspiece = ty;
				}
				else
				{
					ts.Crosspiece = 0;
				}



				if (decimal.TryParse(table.Rows[i]["LegWidth_UnderTheWaves"] + "", out ty))
				{
					ts.LegWidth_UnderTheWaves = ty;
				}
				else
				{
					ts.LegWidth_UnderTheWaves = 0;
				}

				if (decimal.TryParse(table.Rows[i]["FrontRise_EvenWaist"] + "", out ty))
				{
					ts.FrontRise_EvenWaist = ty;
				}
				else
				{
					ts.FrontRise_EvenWaist = 0;
				}

				if (decimal.TryParse(table.Rows[i]["AfterTheWaves_EvenWaist"] + "", out ty))
				{
					ts.AfterTheWaves_EvenWaist = ty;
				}
				else
				{
					ts.AfterTheWaves_EvenWaist = 0;
				}

				ts.NetHip = table.Rows[i]["NetHip"] + "";

				ts.CP_WaistWidth = table.Rows[i]["CP_WaistWidth"] + "";
				ts.Height = table.Rows[i]["Height"] + "";
				ts.LongPants = table.Rows[i]["LongPants"] + "";
				ts.NetWaist = table.Rows[i]["NetWaist"] + "";
		 

				cslist.Add(ts);
			}

			return cslist;


		}


		public bool Import_Excel_trousrs(DataTable table,FormCollection fm, out string errmsg)
		{

			using (var db = SugarDao.GetInstance())
				try
				{
					List<TrousersSize> tszie_list = new List<TrousersSize>();

					for (int i = 0; i < table.Rows.Count; i++)
					{
						#region 处理不符合要求得数据
						DataRow row = table.Rows[i];
						row.BeginEdit();
						for (int a = 0; a < table.Columns.Count; a++)
						{


							if (row[a].ToString().IndexOf("....") > 0)
							{
								row[a] = row[a].ToString().Replace("....", ".");
							}
							if (row[a].ToString().IndexOf("...") > 0)
							{
								row[a] = row[a].ToString().Replace("...", ".");
							}
							if (row[a].ToString().IndexOf("..") > 0)
							{
								row[a] = row[a].ToString().Replace("..", ".");
							}

							if (row[a].ToString().IndexOf("null") > 0)
							{
								row[a] = row[a].ToString().Replace("null", "0");
							}
							if (string.IsNullOrEmpty(row[a].ToString()))
							{
								row[a] = 0;
							}
						}
						row.EndEdit();
						#endregion

						#region 保存尺码




						TrousersSize tszie = new TrousersSize();
						foreach (DataRow item in table.Rows)
						{
							

							tszie.Code = item["Code"].ToString();
							decimal ty = 0;
							if (decimal.TryParse(item["DZ_HipLength_CP"].ToString(), out ty))
							{
								tszie.DZ_HipLength_CP = ty;
							}
							else
							{
								tszie.DZ_HipLength_CP = 0;
							}
 

							if (decimal.TryParse(item["SZ_HipLength_CP"].ToString(), out ty))
							{
								tszie.SZ_HipLength_CP = ty;
							}
							else
							{
								tszie.SZ_HipLength_CP = 0;
							}

							if (decimal.TryParse(item["Crosspiece"].ToString(), out ty))
							{
								tszie.Crosspiece = ty;
							}
							else
							{
								tszie.Crosspiece = 0;
							}

							if (decimal.TryParse(item["LegWidth_UnderTheWaves"].ToString(), out ty))
							{
								tszie.LegWidth_UnderTheWaves = ty;
							}
							else
							{
								tszie.LegWidth_UnderTheWaves = 0;
							}

							if (decimal.TryParse(item["FrontRise_EvenWaist"].ToString(), out ty))
							{
								tszie.FrontRise_EvenWaist = ty;
							}
							else
							{
								tszie.FrontRise_EvenWaist = 0;
							}

							if (decimal.TryParse(item["AfterTheWaves_EvenWaist"].ToString(), out ty))
							{
								tszie.AfterTheWaves_EvenWaist = ty;
							}
							else
							{
								tszie.AfterTheWaves_EvenWaist = 0;
							}

							tszie.NetHip = item["NetHip"].ToString();
							tszie.CP_WaistWidth = item["CP_WaistWidth"].ToString();
							tszie.Height = item["Height"].ToString();
							tszie.LongPants = item["LongPants"].ToString();
							tszie.NetWaist = item["NetWaist"].ToString();
							tszie.Size_Code = fm["Size_Code"].ToString();

			

						}
						tszie_list.Add(tszie);

						#endregion


					}
					db.InsertRange(tszie_list);
					db.CommitTran();
					errmsg = "";
					return true;

				}
				catch (Exception ex)
				{

					db.RollbackTran();//回滚事务
					errmsg = ex.Message;
					return false;

				}
		}





		public DataTable Excel_analysis_trousrs(FormCollection fm)
		{
			DataTable execltable = new DataTable();
			try
			{


				string url;
				string errmsg;
				HttpFileCollectionBase files = Request.Files;

				Common.Common.UpLoadFile(files[0], "File" + DateTime.Now.ToString("sshhffffff") + ".xls", "~/Data", out url, out errmsg);


				DataTable execl_table = ExcelHelper.InputFromExcel(Server.MapPath(url), "sheet1");


				#region 创建最终table
				execltable = new DataTable();
				execltable.Columns.Add("Code");
				execltable.Columns.Add("DZ_HipLength_CP");
				execltable.Columns.Add("SZ_HipLength_CP");
				execltable.Columns.Add("Crosspiece");
				execltable.Columns.Add("LegWidth_UnderTheWaves");
				execltable.Columns.Add("FrontRise_EvenWaist");
				execltable.Columns.Add("AfterTheWaves_EvenWaist");
				execltable.Columns.Add("NetHip");
				execltable.Columns.Add("CP_WaistWidth");
				execltable.Columns.Add("Height");
				execltable.Columns.Add("LongPants");
				execltable.Columns.Add("NetWaist");

				#endregion

				#region 处理数据

				int[] del_index = new int[execl_table.Rows.Count];
				for (int i = 0; i < execl_table.Rows.Count; i++)
				{
					if (Common.Common.IsChinses(execl_table.Rows[i][0].ToString().Trim()))
					{
						if (execl_table.Rows[i][0].ToString().Trim() != "代号")
						{
							del_index[i] = i;
							continue;
						}
					}



					if (string.IsNullOrEmpty(execl_table.Rows[i]["F3"].ToString()))
					{
						del_index[i] = i;

					}
					else
					{

						double jyw;
						if (double.TryParse(execl_table.Rows[i][0] + "", out jyw))
						{
							execl_table.Rows[i][0] += "~" + (jyw + 2);
						}
						double cpyw;
						if (double.TryParse(execl_table.Rows[i]["F2"] + "", out cpyw))
						{
							execl_table.Rows[i]["F2"] += "~" + (cpyw + 2);
						}
					}


				}

				int del_count = 0;
				for (int i = 0; i < del_index.Length; i++)
				{
					if (del_index[i] != 0)
					{
						execl_table.Rows.RemoveAt(del_index[i] - del_count);
						del_count++;
					}
				}
				execl_table.Rows.RemoveAt(0);





				#endregion

				#region 填充数据
				string[] code = new string[4];
				for (int i = 0; i < execl_table.Rows.Count; i++)
				{
					if (Common.Common.IsEnglish_Length_One(execl_table.Rows[i]["F3"].ToString()))
						code[0] = execl_table.Rows[i]["F3"].ToString();
					if (Common.Common.IsEnglish_Length_One(execl_table.Rows[i]["F10"].ToString()))
						code[1] = execl_table.Rows[i]["F10"].ToString();
					if (Common.Common.IsEnglish_Length_One(execl_table.Rows[i]["F17"].ToString()))
						code[2] = execl_table.Rows[i]["F17"].ToString();
					if (Common.Common.IsEnglish_Length_One(execl_table.Rows[i]["F24"].ToString()))
						code[3] = execl_table.Rows[i]["F24"].ToString();

					if (Common.Common.IsChinses(execl_table.Rows[i][0].ToString()))
						continue;

					DataRow row0 = execltable.NewRow();
					row0["Code"] = code[0];
					row0["NetWaist"] = execl_table.Rows[i][0];
					row0["CP_WaistWidth"] = execl_table.Rows[i][1];
					row0["NetHip"] = execl_table.Rows[i][2];
					row0["DZ_HipLength_CP"] = execl_table.Rows[i][3];
					row0["SZ_HipLength_CP"] = execl_table.Rows[i][4];
					row0["Crosspiece"] = execl_table.Rows[i][5];
					row0["LegWidth_UnderTheWaves"] = execl_table.Rows[i][6];
					row0["FrontRise_EvenWaist"] = execl_table.Rows[i][7];
					row0["AfterTheWaves_EvenWaist"] = execl_table.Rows[i][8];
					execltable.Rows.Add(row0);
					DataRow row1 = execltable.NewRow();
					row1["Code"] = code[1];
					row1["NetWaist"] = execl_table.Rows[i][0];
					row1["CP_WaistWidth"] = execl_table.Rows[i][1];
					row1["NetHip"] = execl_table.Rows[i][9];
					row1["DZ_HipLength_CP"] = execl_table.Rows[i][10];
					row1["SZ_HipLength_CP"] = execl_table.Rows[i][11];
					row1["Crosspiece"] = execl_table.Rows[i][12];
					row1["LegWidth_UnderTheWaves"] = execl_table.Rows[i][13];
					row1["FrontRise_EvenWaist"] = execl_table.Rows[i][14];
					row1["AfterTheWaves_EvenWaist"] = execl_table.Rows[i][15];
					execltable.Rows.Add(row1);
					DataRow row2 = execltable.NewRow();
					row2["Code"] = code[2];
					row2["NetWaist"] = execl_table.Rows[i][0];
					row2["CP_WaistWidth"] = execl_table.Rows[i][1];
					row2["NetHip"] = execl_table.Rows[i][16];
					row2["DZ_HipLength_CP"] = execl_table.Rows[i][17];
					row2["SZ_HipLength_CP"] = execl_table.Rows[i][18];
					row2["Crosspiece"] = execl_table.Rows[i][19];
					row2["LegWidth_UnderTheWaves"] = execl_table.Rows[i][20];
					row2["FrontRise_EvenWaist"] = execl_table.Rows[i][21];
					row2["AfterTheWaves_EvenWaist"] = execl_table.Rows[i][22];
					execltable.Rows.Add(row2);
					DataRow row3 = execltable.NewRow();
					row3["Code"] = code[3];
					row3["NetWaist"] = execl_table.Rows[i][0];
					row3["CP_WaistWidth"] = execl_table.Rows[i][1];
					row3["NetHip"] = execl_table.Rows[i][23];
					row3["DZ_HipLength_CP"] = execl_table.Rows[i][24];
					row3["SZ_HipLength_CP"] = execl_table.Rows[i][25];
					row3["Crosspiece"] = execl_table.Rows[i][26];
					row3["LegWidth_UnderTheWaves"] = execl_table.Rows[i][27];
					row3["FrontRise_EvenWaist"] = execl_table.Rows[i][28];
					row3["AfterTheWaves_EvenWaist"] = execl_table.Rows[i][29];
					execltable.Rows.Add(row3);
				}

				#endregion

				return execltable;

			}
			catch (Exception ex)
			{
				return null;

			}



		}

		#endregion

	}
}