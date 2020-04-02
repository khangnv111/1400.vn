using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W1400.DataAccess.DTO;
using W1400.DataAccess.Factory;
using W1400.Utility;

namespace CMS.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult Header()
        {
            string sUrl = Server.UrlEncode(HttpContext.Request.Url.ToString());
            Session["Redirect_Uri"] = sUrl;
            //Kiểm tra Authen
            var userValidate = new UserValidation();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                ViewBag.LogIn = false;
                ViewBag.UrlLogin = urlLogin;
                return PartialView();
            }
            else
            {
                var IsAdministrator = userValidate.IsAdministrator;
                var functions = new List<Functions>();
                if (IsAdministrator)
                    functions = AbstractDAOFactory.Instance().CreateFunctionDAO().SelectAllFunctionID(-1, string.Empty, 1, 1);
                else
                    functions = AbstractDAOFactory.Instance().CreateFunctionDAO().GetListFunctionByUserID(userValidate.UserId, -1);

                Session[SessionsManager.SESSION_FUNCTIONS] = functions;

                ViewBag.UserName = userValidate.UserName;
            }
            ViewBag.LogIn = true;
            ViewBag.UrlLogin = urlLogin;
            return PartialView();
        }

        public ActionResult Menu()
        {

            var functions = new List<Functions>();
            functions = (List<Functions>)Session[SessionsManager.SESSION_FUNCTIONS];
            return PartialView(functions);
        }

        public ActionResult ErrorPermission()
        {
            var urlLogin = Config.UrlRoot + "login_matrix.aspx";
            ViewBag.UrlLogin = urlLogin;
            return PartialView();
        }

        public ActionResult ErrorNotPage()
        {
            return PartialView();
        }
        public ActionResult ErrorInternalServer()
        {
            return PartialView();
        }
        public void LogOut()
        {
            UserValidation m_UserValidation = new UserValidation();
            m_UserValidation.SignOut();
            Session.Abandon();
            Session.RemoveAll();
            // Response.Redirect(SSOMAIL.SsoHelper.URLLogoutMailSSO);
        }

        public static string GetChildMenu(Functions func, List<Functions> listChild)
        {
            var ListChild = listChild.FindAll(f => f.ParentID == func.FunctionID && f.IsActive == true && f.IsDisplay == true);
            ListChild.Sort((f1, f2) => f1.Order.CompareTo(f2.Order));
            var script = "";
            if (ListChild.Count > 0)
            {
                script += "<li class=\"dropdown\">" +
                        "<a href=\"javascript:void(0);\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" role=\"button\" aria-expanded=\"false\">" +
                        "<i class=\"" + func.CssIcon + "\"></i> " + func.FunctionName + "<span class=\"caret\"></span></a>";
                script += "<ul class=\"dropdown-menu nav-third-level\" role=\"menu\">";
                foreach (var obj in ListChild)
                {
                    script += GetChildMenu(obj, listChild);
                }
                script += "</ul></li>";
            }
            else
            {
                script += "<li><a href=\"" + Config.UrlRoot + func.Url + "\"><i class=\"" + func.CssIcon + "\"></i>" + func.FunctionName + "</a></li>";
            }
            return script;
        }
    }
}