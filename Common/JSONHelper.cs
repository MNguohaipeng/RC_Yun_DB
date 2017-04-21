using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 
using System.Text.RegularExpressions;
using System.Reflection;
using Newtonsoft.Json.Converters;

namespace Common
{
    public class JSONHelper
    { 
        public static DataTable JsonToDataTable(string strJson)
        {
            if (!string.IsNullOrEmpty(strJson))
            {
                if (strJson[0] != '[')
                    strJson = "[" + strJson;
                if (strJson[strJson.Length - 1] != ']')
                    strJson = strJson + "]";

                DataTable tb = JsonConvert.DeserializeObject(strJson, typeof(DataTable)) as DataTable;

                return tb;
            }
            else
                return new DataTable();
        }
         
        public static DataSet JsonToDataSet(string strJson)
        { 
            if (!string.IsNullOrEmpty(strJson))
            {
                DataSet ds = new DataSet();

                JObject ob = JObject.Parse(strJson); 
                JToken jt = ob["tables"];

                if (jt.Count() > 0)
                { 
                    int i = 0;
                    while (true)
                    {
                        DataTable dt = JsonToDataTable(jt[i]["rows"].ToString());

                        dt.TableName = jt[i]["title"].ToString();

                        ds.Tables.Add(dt);

                        if (jt[i].Next == null)
                            break;
                        i++;
                    }
                }
                 
                return ds;
            }
            else
                return new DataSet();
        }


        public static string DataSetToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + DataTableToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }

        public static string DataTableToJson(DataTable dt)
        {
            try
            {
                return DataTableToJson(dt, dt.Rows.Count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        
        /// <summary>
        /// 添加总行数集合, 用于easyUI DataGrid绑定数据用
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="totalCount">总行数</param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt, int totalCount)
        {
            return DataTableToJson(dt, totalCount, ""); 
        }

        /// <summary>
        /// 添加总行数集合, 用于easyUI DataGrid绑定数据用
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt, int totalCount, string footer )
        {
            try
            {
                StringBuilder strJson = new StringBuilder();

                strJson.Append("{\"total\":");
                strJson.Append(totalCount.ToString());
                strJson.Append(",\"rows\":");
                strJson.Append("[");
                foreach (DataRow dr in dt.Rows)
                {
                    strJson.Append(" {");
                    foreach (DataColumn item in dt.Columns)
                    {
                        strJson.Append("\"" + ConvertHepler.ConvertJson(item.ColumnName) + "\":");

                        if (Equals(dr[item.ColumnName], DBNull.Value))
                            strJson.Append("null");
                        else if (string.IsNullOrEmpty(dr[item.ColumnName].ToString()))
                            strJson.Append("\"\"");
                        else
                            strJson.Append("\"" + ConvertHepler.ConvertJson(dr[item.ColumnName].ToString()) + "\"");
                        strJson.Append(",");
                    }
                    strJson = new StringBuilder(strJson.ToString().TrimEnd(','));
                    strJson.Append("},");
                }

                return strJson.ToString().TrimEnd(',') + "]" + footer  + "}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string DictionaryToJson(Dictionary<string, string> list)
        {
            StringBuilder strJson = new StringBuilder();
            strJson.Append("{");
            foreach (KeyValuePair<string, string> item in list)
            {
                if (item.Value != null)
                { 
                    if (item.Value.Length > 0)
                    {
                        if (item.Value[0] == '[' || item.Value[0] == '{')
                        {
                            strJson.Append("\"" + item.Key + "\":" + item.Value + ",");
                            continue;
                        }
                    }
                }
                strJson.Append("\"" + item.Key + "\":" + "\"" +  ConvertHepler.ConvertJson(item.Value) + "\",");
            }
            strJson = new StringBuilder(strJson.ToString().TrimEnd(','));
            strJson.Append("}");
            return strJson.ToString();
        }

        public static string ListToJson<T>(List<T> _list) where T : new()
        {
            StringBuilder strJson = new StringBuilder();

            strJson.Append("{\"total\":");
            strJson.Append(_list.Count.ToString());
            strJson.Append(",\"rows\":");
            strJson.Append("[");

            Type type = typeof(T);

            foreach (T _t in _list)
            {
                strJson.Append(" {");
                foreach (PropertyInfo pi in type.GetProperties())
                { 
                    object _obj = pi.GetValue(_t, null);
                    string _val = "";
                    if (_obj == null)
                        _val = "";
                    else if (_obj.GetType() == typeof(DateTime))
                        _val = Convert.ToDateTime(_obj).ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        _val = _obj.ToString();

                    strJson.Append("\"" + pi.Name + "\":");
                    strJson.Append("\"" + ConvertHepler.ConvertJson(_val) + "\"");
                    strJson.Append(","); 
                }
                strJson = new StringBuilder(strJson.ToString().TrimEnd(','));
                strJson.Append("},");
            }

            return strJson.ToString().TrimEnd(',') + "]}";
        }

        public static string ModuleToJson<T>(T _mol) where T : new()
        {
            StringBuilder strJson = new StringBuilder();
             
            strJson.Append("{");

            Type type = typeof(T);
             
            foreach (PropertyInfo pi in type.GetProperties())
            {
                object _obj = pi.GetValue(_mol, null);
                string _val = "";
                if (_obj == null)
                    _val = "";
                else if (_obj.GetType() == typeof(DateTime))
                    _val = Convert.ToDateTime(_obj).ToString("yyyy-MM-dd HH:mm:ss");
                else
                    _val = _obj.ToString();

                strJson.Append("\"" + pi.Name + "\":");
                strJson.Append("\"" + _val + "\"");
                strJson.Append(",");
            }
            strJson = new StringBuilder(strJson.ToString().TrimEnd(','));
             
            return strJson.ToString() + "}";
        }

        public string GetAllCategory(DataTable dt)
        {
            string result = ""; 
            result = JsonConvert.SerializeObject(dt, new DataTableConverter());
            return result;
        } 
    }
}
