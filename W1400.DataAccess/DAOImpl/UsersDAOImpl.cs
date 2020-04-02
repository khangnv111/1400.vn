using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using W1400.DataAccess.DAO;
using W1400.DataAccess.DTO;
using W1400.Utility;
using DBHelpers;

namespace W1400.DataAccess.DAOImpl
{
    public class UsersDAOImpl : IUsersDAO
    {

        /// <summary>
        /// Xác thực người dùng
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="isSucess"></param>
        /// <returns></returns>
        public int Authentication(string username, string password)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_Username", username);
                pars[1] = new SqlParameter("@_Password", password);
                pars[2] = new SqlParameter("@_ClientIP", System.Web.HttpContext.Current.Request.UserHostAddress);
                pars[3] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("sp_User_Authenticate", pars);

                return Convert.ToInt32(pars[3].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Error: " + ex.ToString());
                return -99;
            }
        }


        /// <summary>
        /// Get User theo UserID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Users SelectByUserID(int userId)
        {
            try
            {

                return new DBHelper(Config.Site1400ConnectionString).GetInstanceSP<Users>("SP_User_GetByCondition",
                                                                                                 new SqlParameter("@_UserID", userId));

            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new Users();
            }
        }



        /// <summary>
        /// Get User theo email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Users GetByEmail(string email)
        {
            try
            {
                return new DBHelper(Config.Site1400ConnectionString).GetInstanceSP<Users>("SP_User_GetByCondition",
                                                                                                 new SqlParameter("@_Email", email));
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new Users();
            }
        }
        /// <summary>
        /// Get User theo Username
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public Users GetByUsername(string Username)
        {
            try
            {
                return new DBHelper(Config.Site1400ConnectionString).GetInstanceSP<Users>("SP_User_GetByCondition",
                                                                                                 new SqlParameter("@_Username", Username));
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new Users();
            }
        }



        /// <summary>
        /// Get list<Users>theo điều kiện, có phân trang
        /// </summary>
        /// <param name="departmentID"></param>
        /// <param name="groupID"></param>
        /// <param name="isAcitve"></param>
        /// <param name="email"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Users> GetListUsers(string Keyword, int isActive, int isGrant)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_Status", isActive);
                pars[1] = new SqlParameter("@_Keyword", Keyword);
                pars[2] = new SqlParameter("@_IsGrant", isGrant != 1 ? DBNull.Value : (object)isGrant);
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Users>("SP_User_GetPage", pars);
                if (list == null || list.Count <= 0)
                    return new List<Users>();
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<Users>();
            }
        }
        /// <summary>
        /// Lấy tất cả user theo trạng thái
        /// </summary>
        /// <param name="Status"> -1 : tất cả, 0 : unactive, 1:active</param>
        /// <returns></returns>
        public List<Users> GetAllUserByStatus(int Status)
        {
            try
            {
                var list = new DBHelper(Config.Site1400ConnectionString)
                    .GetListSP<Users>("SP_User_GetAll_ByStatus", new SqlParameter("@_Status", Status));
                if (list == null || list.Count <= 0)
                    return new List<Users>();
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<Users>();
            }
        }

        /// <summary>
        /// Update thông tin User
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public int UpdateUsers(Users users)
        {
            try
            {
                var pars = new SqlParameter[9];
                pars[0] = new SqlParameter("@_UserID", (users.UserID < 0) ? 0 : users.UserID);
                pars[1] = new SqlParameter("@_Username", string.IsNullOrEmpty(users.Username) ? DBNull.Value : (object)users.Username);
                pars[2] = new SqlParameter("@_Email", string.IsNullOrEmpty(users.Email) ? DBNull.Value : (object)users.Email);
                pars[3] = new SqlParameter("@_FullName", string.IsNullOrEmpty(users.FullName) ? DBNull.Value : (object)users.FullName);
                pars[4] = new SqlParameter("@_Password", string.IsNullOrEmpty(users.Password) ? DBNull.Value : (object)users.Password);
                pars[5] = new SqlParameter("@_IsActive", users.Status);
                pars[6] = new SqlParameter("@_IsAdministrator", users.IsAdministrator);
                pars[7] = new SqlParameter("@_CreatedUser", string.IsNullOrEmpty(users.CreatedUser) ? DBNull.Value : (object)users.CreatedUser);
                pars[8] = new SqlParameter("@_ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_User_InsertUpdate", pars);
                return Convert.ToInt32(pars[8].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -99;
            }
        }



        /// <summary>
        /// Xóa thông tin một user theo UserID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DelleteUsers(int userId)
        {
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_UserID", userId);
                pars[1] = new SqlParameter("@_ResponseCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_User_Delete", pars);
                return Convert.ToInt32(pars[1].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -99;
            }
        }


        public int UpdateActiveUser(int Id)
        {
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_UserID", Id);
                pars[1] = new SqlParameter("@_ResponseCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_User_UpdateActive", pars);
                return Convert.ToInt32(pars[1].Value);
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return -99;
            }
        }

        public int ChangePassword(string UserName, string PasswordOld, string PasswordNew)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_UserName", UserName);
                pars[1] = new SqlParameter("@_PasswordOld", PasswordOld);
                pars[2] = new SqlParameter("@_PasswordNew", PasswordNew);
                pars[3] = new SqlParameter("@_ResponseCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_User_ChangePassword", pars);
                return Convert.ToInt32(pars[3].Value);
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return -99;
            }
        }

        public AccountLoginWeb SP_AccountLoginWeb_GetByEmail(string Email)
        {
            try
            {
                var acc = new DBHelper(Config.Site1400ConnectionString).GetInstanceSP<AccountLoginWeb>("SP_AccountLoginWeb_GetByEmail", new SqlParameter("@Email", Email));
                return acc;
            }
            catch (Exception e)
            {
                NLogLogger.LogInfo("SP_AccountLoginWeb_GetByEmail Exception : " + e.ToString());
                return new AccountLoginWeb();
            }
        }
        public int SP_AccountLoginWeb_InSertUpdate(AccountLoginWeb acc,int Type)
        {
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@Type", Type);
                pars[1] = new SqlParameter("@AccountID", acc.AccountID);
                pars[2] = new SqlParameter("@Email", acc.Email);
                pars[3] = new SqlParameter("@Namedisplay", acc.Namedisplay);
                pars[4] = new SqlParameter("@LastLoginTime", acc.LastLoginTime);
                pars[5] = new SqlParameter("@ErrorCode", DbType.Int32) { Direction = ParameterDirection.Output };
                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_AccountLoginWeb_InSertUpdate", pars);
                return Convert.ToInt32(pars[5].Value);
            }
            catch (Exception e)
            {
                NLogLogger.LogInfo("SP_AccountLoginWeb_InSertUpdate Exception : " + e.ToString());
                return -191;
            }
        }
    }
}
