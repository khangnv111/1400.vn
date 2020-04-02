using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Controllers.Common;
using W1400.Utility;
using System.Web.Script.Serialization;
using System.Configuration;
using W1400.DataAccess.Factory;
using W1400.DataAccess.DTO;
namespace CMS
{
    public partial class login_matrix : System.Web.UI.Page
    {
        public string UrlRoot = Config.UrlRoot;
        public string Url = string.Empty;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private UserValidation m_UserValidation = new UserValidation();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                var account = new VTCeBank.SSO.Utils.Account(Request);
                NLogLogger.LogInfo("account.AccountName: " + account.AccountName);

                if (string.IsNullOrEmpty(account.AccountName))
                {
                    // Response.Redirect(SSOMAIL.SsoHelper.URLLoginMail);
                }
                else
                {
                    var pos = account.AccountName.IndexOf("@");
                    if (pos > 0)
                    {
                        account.AccountName = account.AccountName.Substring(0, pos);
                    }
                    var m_Users = AbstractDAOFactory.Instance().CreateUsersDAO().GetByUsername(account.AccountName);
                    if (m_Users != null && m_Users.UserID > 0)
                    {
                        var Log = new UsersLog();
                        Log.FunctionID = 9999;
                        Log.ClientIP = Config.GetIP();
                        Log.UserID = m_Users.UserID;
                        Log.LogType = 1;
                        Log.FunctionName = "Đăng Nhập Hệ Thống";
                        Log.Description = "Tài khoản " + m_Users.Username + " Đăng nhập hệ thống";
                        var insertLog = AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(Log);

                        if (m_Users.Status)
                        {
                            //Session[SessionsManager.SESSION_USERID] = m_Users.UserID;
                            //Session[SessionsManager.SESSION_USERNAME] = m_Users.Username;
                            //Session["LoginType"] = 0;
                            string SessionID = Session.SessionID;
                            m_UserValidation.SignIn(m_Users.UserID, m_Users.Username, m_Users.IsAdministrator, SessionID);
                            Url = Session["Redirect_Uri"] == null ? UrlRoot : Server.UrlDecode(Session["Redirect_Uri"].ToString());
                            Response.Redirect(Url);
                        }
                        Response.Redirect("Common/ErrorPermission");
                    }
                    Response.Redirect("Common/ErrorPermission");
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
            }
            finally
            {
                Response.End();
            }
        }
    }
}