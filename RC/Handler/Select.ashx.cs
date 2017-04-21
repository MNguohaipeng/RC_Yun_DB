using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace RC.Handal
{
    /// <summary>
    /// Select 的摘要说明
    /// </summary>
    public class Select : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string json = "";
            string errmsg = "";
            try
            {
			

				string action = context.Request.Form["action"];//获取执行方式


				switch (action)
                {
                    case "ExcelType":

                        string data = "";
						if (ExcelType(out data, out errmsg))
							json = data;

//							json = "{status:0,msg:" + data + "}";
                        else
                            throw new Exception(errmsg);
                        break;

                    default:
                        json = "{status:1,msg:'没有对应的action'}";
                        throw new Exception("没有对应的action");
                 
                }

            }
            catch (Exception ex)
            {
                json = "{status:1,msg:'"+ ex.Message+ "'}";
            }

            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        private bool ExcelType(out string data,out string errmsg)
        {
            using (var db = Common.SugarDao.GetInstance())
                try
                {
                    errmsg = "";
                    data = "";

					List<Entity.Dictionaries> list = db.Queryable<Entity.Dictionaries>().Where(t => t.D_Key == "ExcelType").ToList();

					  data = "[";
           
					foreach (Entity.Dictionaries item in list)
					{
						data += "{\"values\":\""+ item.D_Value+"\"}," ;
                    }
					data=data.TrimEnd(',');
					data += "]";
                    return true;
 
                }
                catch (Exception ex)
                {
                    data = "";
                    errmsg = ex.Message;
                    return false;
                }

        }



    }
}