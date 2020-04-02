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
    public class ErrorLogDAOImpl : IErrorLogDAO
    {
        /// <summary>
        /// Lấy danh sách ErrorLog từ ngày x đến ngày y
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pagenumber"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public List<ErrorLog> GetErrorLog(string fromDate, string toDate)
        {
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@FromDate", fromDate);
                pars[1] = new SqlParameter("@ToDate", toDate);
                return new DBHelper(Config.Site1400ConnectionString).GetListSP<ErrorLog>("SP_ErrorLog_GetPage", pars);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<ErrorLog>();
            }

        }


        /// <summary>
        /// Xóa danh sách ErrorLog từ ngày x đến ngày y
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public int Delete(string fromDate, string toDate)
        {
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@FromDate", fromDate);
                pars[1] = new SqlParameter("@ToDate", toDate);
                pars[2] = new SqlParameter("@ResponseCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_ErrorLog_Delete", pars);
                return Convert.ToInt32(pars[2].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -99;
            }
        }

    }
}
