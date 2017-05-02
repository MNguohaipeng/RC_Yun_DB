using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlSugar;
using System.Web.Caching;
using System.Collections;
using Common;

namespace RC.Controllers
{
	public class HomeController : Controller
	{
		// GET: Home
		[CxAttribute]
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Login()
		{

			return View();
		}


		[HttpPost]
		public object Login(FormCollection fm)
		{
			string msg = "";
			string start = "";

			using (var db = Common.SugarDao.GetInstance())

				try
				{

					#region 表单验证
					if (string.IsNullOrEmpty(fm["username"]))
					{
						start = "error";
						msg = "请输入用户名。";

					}

					if (string.IsNullOrEmpty(fm["password"]))
					{
						start = "error";
						msg = "请输入密码。";

					}
					#endregion


					Encryption en = new Encryption();

					string jmpw = en.DesEncrypt(fm["password"], "123");

					int userCount = db.GetInt(string.Format("select count(ID) from [User] where user_name='{0}' and [password]='{1}'", fm["username"], jmpw));

					if (userCount == 1)
					{
						start = "success";
						#region 写入缓存
						HttpCookie cookie = new HttpCookie("user");
						cookie.Value = fm["username"];
						Response.Cookies.Add(cookie);
						#endregion

					}
					else if (userCount >= 2)
					{
						msg = "有多个匹配用户，请联系管理员";
						start = "error";
					}
					else if (userCount <= 0)
					{
						msg = "用户名或密码错误。";
						start = "error";
					}
					else
					{
						start = "error";
					}

					return Json(new { Start = start, Msg = msg }, JsonRequestBehavior.AllowGet);

				}
				catch (Exception)
				{

					throw;
				}

		}

	}
}