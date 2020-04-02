using DBHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DAO;
using W1400.DataAccess.DTO;
using W1400.Utility;

namespace W1400.DataAccess.DAOImpl
{
    public class ProgramDAOImpl : ProgramDAO
    {
        public List<Program> SP_Event_GetList_Web(int EventID, string EventName, int IsSetted, int Page, int PageSize, out int TotalRow)
        {
            TotalRow = 0;
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@EventID", EventID);
                pars[1] = new SqlParameter("@EventName", EventName);
                if (IsSetted == -1)
                    pars[2] = new SqlParameter("@IsSetted", DBNull.Value);
                else if (IsSetted == 1)
                    pars[2] = new SqlParameter("@IsSetted", true);
                pars[3] = new SqlParameter("@Page", Page);
                pars[4] = new SqlParameter("@PageSize", PageSize);
                pars[5] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Program>("SP_Event_GetList_Web", pars);
                if (list != null || list.Count >= 0)
                {
                    TotalRow = Convert.ToInt32(pars[5].Value);
                }
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<Program>();
            }
        }
        public List<ProgramDetail> SP_Event_GetDetail_Web(int EventID, out int TotalRow)
        {
            TotalRow = 0;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@EventID", EventID);
                pars[1] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<ProgramDetail>("SP_Event_GetDetail_Web", pars);
                if (list != null || list.Count >= 0)
                {
                    TotalRow = Convert.ToInt32(pars[1].Value);
                }
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<ProgramDetail>();
            }
        }
        public List<ProgramReport> SP_Event_GetReport_Web(int EventID,DateTime FromDate,DateTime ToDate, out int TotalRow)
        {
            TotalRow = 0;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@EventID", EventID);
                pars[1] = new SqlParameter("@FromDate", FromDate);
                pars[2] = new SqlParameter("@ToDate", ToDate);
                pars[3] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<ProgramReport>("SP_Event_GetReport_Web", pars);
                if (list != null || list.Count >= 0)
                {
                    TotalRow = Convert.ToInt32(pars[3].Value);
                }
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<ProgramReport>();
            }
        }
        public List<Program> SP_Event_OldGetList_Web(int EventID, string EventName, int IsSetted, int Year, int Page, int PageSize, out int TotalRow)
        {
            TotalRow = 0;
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@EventID", EventID);
                pars[1] = new SqlParameter("@EventName", EventName);
                if (IsSetted == -1)
                    pars[2] = new SqlParameter("@IsSetted", DBNull.Value);
                else if (IsSetted == 1)
                    pars[2] = new SqlParameter("@IsSetted", true);
                pars[3] = new SqlParameter("@Year", Year);
                pars[4] = new SqlParameter("@Page", Page);
                pars[5] = new SqlParameter("@PageSize", PageSize);
                pars[6] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Program>("SP_Event_OldGetList_Web", pars);
                if (list != null || list.Count >= 0)
                {
                    TotalRow = Convert.ToInt32(pars[6].Value);
                }
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<Program>();
            }
        }
        
    }
}
