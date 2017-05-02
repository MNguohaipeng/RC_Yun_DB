using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Common
{
	public class CxAttribute:ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{


			if (HttpContext.Current.Request.Cookies["user"] == null)
			{
				//filterContext.Result = new RedirectResult("/Home/Login");
				//HttpContext.Current.Response.Write("<script>alert('13')</script>");
				//	FormsAuthentication.RedirectToLoginPage();
				string loginUrl = $"<script>top.location.href='/Home/Login'</script>";
				filterContext.Result = new ContentResult() {
					Content = loginUrl, ContentType = "text/html",
					ContentEncoding = Encoding.UTF8 };
			}
 
			base.OnActionExecuting(filterContext);



		}


	}
}
