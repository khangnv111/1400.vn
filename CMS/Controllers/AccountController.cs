using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W1400.DataAccess.DTO;
using W1400.DataAccess.Factory;
using W1400.Utility;
using W1400.Utility.Security;

namespace CMS.Controllers
{
    public class AccountController : Controller
    {
        private UserValidation m_UserValidation = new UserValidation();
        public ActionResult FormLogin(string act)
        {
            if (!string.IsNullOrEmpty(act) && act == "out")
            {
                UserValidation m_UserValidation = new UserValidation();
                m_UserValidation.SignOut();
                Session.Abandon();
                Session.RemoveAll();
                Response.Redirect("~/", true);
            }
            return View();
        }

        [HttpPost]
        public JsonResult Login(string Username, string Password)
        {
            try
            {
                NLogLogger.LogInfo("Login-->Username:" + Username);
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                    return Json(new { success = false, statusCode = -1, msg = "Dữ liệu không được bỏ trống" });
                var password = Encrypt.Md5(Password.Trim());

                int checkLogin = AbstractDAOFactory.Instance().CreateUsersDAO().Authentication(Username.Trim(), password);
                if (checkLogin == -49)
                {
                    return Json(new { success = false, statusCode = -102, msg = "Tài khoản của bạn đã bị block" });
                }
                else if (checkLogin == -50)
                {
                    return Json(new { success = false, statusCode = -102, msg = "Tài khoản của bạn chưa được cấp quyền" });
                }
                else if (checkLogin == -53)
                {
                    return Json(new { success = false, statusCode = -102, msg = "Mật khẩu không chính xác" });
                }
                else if (checkLogin > 0)
                {
                    var m_Users = AbstractDAOFactory.Instance().CreateUsersDAO().GetByUsername(Username);
                    if (m_Users != null && m_Users.UserID > 0)
                    {
                        var Log = new UsersLog();
                        Log.ClientIP = Config.GetIP();
                        Log.FunctionID = 9999;
                        Log.UserID = m_Users.UserID;
                        Log.LogType = 1;
                        Log.FunctionName = "Đăng Nhập Hệ Thống";
                        Log.Description = "Tài khoản " + m_Users.Username + " Đăng nhập hệ thống";
                        var insertLog = AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(Log);

                        if (m_Users.Status)
                        {
                            Session["LoginType"] = 1;
                            string SessionID = Session.SessionID;
                            m_UserValidation.SignIn(m_Users.UserID, m_Users.Username, m_Users.IsAdministrator, SessionID);
                            var UrlRedirect = Config.UrlRoot; // Session["Redirect_Uri"] == null ? Config.UrlRoot : Server.UrlDecode(Session["Redirect_Uri"].ToString());
                            NLogLogger.LogInfo("UrlRedirect : " + UrlRedirect);
                            return Json(new { success = true, statusCode = 1, msg = "Đăng Nhập Thành Công", url = UrlRedirect });
                        }
                        return Json(new { success = false, statusCode = -102, msg = "Tài khoản của bạn đã bị block" });
                    }

                }
                return Json(new { success = false, statusCode = -1, msg = "Username hoặc Password không đúng" });
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { success = false, statusCode = -99, msg = "Hệ thống bận vui lòng quay lại sau" });
            }
        }
    }
}