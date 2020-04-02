using DBHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DAO;
using W1400.Utility;

namespace W1400.DataAccess.DAOImpl
{
    public class SuggestDAOImpl : SuggestDAO
    {
        public void SP_Suggestion_Create(string Email, string Mobile, string Suggestion, out int ResponseStatus)
        {
            
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@Email", Email);
                pars[1] = new SqlParameter("@Mobile", Mobile);
                pars[2] = new SqlParameter("@Suggestion", Suggestion);
                pars[3] = new SqlParameter("@ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_Suggestion_Create", pars);
                ResponseStatus = Convert.ToInt32(pars[3].Value);   
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                ResponseStatus = -99;
            }
        }
    }
}
