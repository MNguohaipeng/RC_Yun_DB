using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
 public static class Comparison
	{

		//男士西服裤子001解析
		public static bool N_XF_KZ_001(HttpFileCollectionBase files, out string errMsg, out object data)
		{
			try
			{
				errMsg = "";

				data = null;

				#region  解析excel

				DataTable table = Analysis.Excel_analysis_trousrs(files);

				#endregion

				List<OriginalDataTrousers> cslist = new List<OriginalDataTrousers>();

				#region 验证并处理数据

				for (int i = 0; i < table.Rows.Count; i++)
				{

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

				#endregion

				data = cslist;

				#region 对比数据

				DataTable rettable = new DataTable();

				List<Entity.Ret_CodeTrousers> ct = new List<Ret_CodeTrousers>();

				for (int i = 0; i < table.Rows.Count; i++)
				{
					System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"//^\s*[\u4E00-\u9FA5\s]\s*[0-9]\s*[0-9]\s*[0-9_a-z_A-Z]\s*");

					if (reg1.IsMatch(table.Rows[i]["ret_code_size"] + ""))
					{

						DataRow rerow = rettable.NewRow();

						rettable.Rows.Add(rerow);

					}
				}

				#endregion


				return true;

			}
			catch (Exception)
			{

				throw;
			}
		}


		//西服尺码表解析
		public static object Ret_Excel(DataTable table)
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

		//西裤尺码表解析
		public static object Ret_Excel_trousrs(DataTable table)
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

	}
}
