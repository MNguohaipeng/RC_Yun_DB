using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class ConvertHepler
    {
        public static string ConvertJson(string _json)
        {
            return ConvertJson(_json, "tojson");
        }

        public static string ConvertJson(string _json, string type = "tojson")
        {
            if (string.IsNullOrEmpty(_json))
                return "";

            Dictionary<string, string> split = new Dictionary<string, string>();
            split.Add("\\", "\\\\");
            split.Add("\"", "\\\"");
            
            if (type == "tojson")
            {
                split.Add("\t", " ");   
                split.Add("\r", "\\r");
                split.Add("\n", "\\n");
            } 

            foreach (KeyValuePair<string, string> item in split)
            {
                switch (type)
                {
                    // 从string 转 json
                    case "tojson":
                        _json = _json.Replace(item.Key, item.Value);
                        break;
                    // 从json 转 字符串
                    case "tostr":
                        _json = _json.Replace(item.Value, item.Key);
                        break;
                }
            }
            return _json;
        }


        public static string ConvertHtml(string _html)
        { 
            // &符号转义必须放在第一个, 因为转义符带有&符号
            return _html
                .Replace("&", "&amp;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;") 
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
               ;
        }


        public static string ConvertSQL(string _val)
        {
            // 处理拼接字符串时 ' 引发的问题
            return _val.Replace("'", "''");
        }

    }
}
