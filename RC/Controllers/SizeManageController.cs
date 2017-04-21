using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using Common;
using Entity;

namespace RC.Controllers
{
    public class SizeManageController : Controller
    {
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


        public JsonResult GetCoatSize(int limit, int offset, string size_code,  string statu)
        {
            using (var db = SugarDao.GetInstance())
                try
                {
 
                    var total = db.Queryable<CoatSize>().Count();
                    var rows = db.Queryable<CoatSize>().Where(T=>T.NetBust.StartsWith(statu)|| T.FrontLength.StartsWith(statu)).OrderBy(it => it.ID).Skip(offset).Take(limit).ToList();
                    return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    return Json(new { total = 1, rows = "{'errmsg':,"+ex.Message+"}" }, JsonRequestBehavior.AllowGet);

                }


        }




        public object Load_List(FormCollection fm)
        {
            var list = new object();
            using (var db = SugarDao.GetInstance())
                try
                {
                    string par1 = fm["Size_Code"];

                    list = db.Queryable<CoatSize>().Where(it => it.Size_Code == par1.ObjToString()).ToList();
                }
                catch (Exception ex)
                {

                    throw;

                }

            return list;

        }
    }
}