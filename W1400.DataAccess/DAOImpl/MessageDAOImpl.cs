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
    public class MessageDAOImpl : MessageDAO
    {
        public List<DBMessage> SP_Get_MO(string Keyword, int CurrPage, int RecordPerPage, out int TotalRecord)
        {
            TotalRecord = 0;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@Keyword", Keyword);
                pars[1] = new SqlParameter("@CurrPage", CurrPage);
                pars[2] = new SqlParameter("@RecordPerPage", RecordPerPage);
                pars[3] = new SqlParameter("@TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400MessConnectionString).GetListSP<DBMessage>("SP_Get_MO", pars);
                if (list != null || list.Count >= 0)
                {
                    TotalRecord = Convert.ToInt32(pars[3].Value);
                }
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<DBMessage>();
            }
        }
    }
}
