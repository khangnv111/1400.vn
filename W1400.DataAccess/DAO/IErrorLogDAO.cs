using W1400.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W1400.DataAccess.DAO
{
    public interface IErrorLogDAO
    {
        List<ErrorLog> GetErrorLog(string fromDate, string toDate);
        int Delete(string fromDate, string toDate);
    }
}
