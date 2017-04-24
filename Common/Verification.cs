using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace Common
{
	public static  class Verification
	{
		//验证西服上衣格式
		public static bool Verification_XF_SY(DataTable table) {
			using (var db=SugarDao.GetInstance())
				try
				{
					//db.GetString()
					string xxx=db.GetString(@"  select  D_Value  from Dictionaries where d_Key='ExcelColumnsCount_XF_SY'   ");
				//	var vc = db.Queryable<Entity.Dictionaries>().Where(T => T.D_Key == "ExcelColumnsCount_XF_SY").ToList();
					int count = 0;
					//if (!int.TryParse(vc.D_Value,out count)) {
					//	throw new Exception("系统出错，没有配置excel验证条件。");
					//}
					if (table.Columns.Count != count) {
						throw new Exception("Excel格式出错，请检查文件");
					}

					return true;

				}
				catch (Exception ex)
				{

					throw;
				}
 
		}

	}
}
