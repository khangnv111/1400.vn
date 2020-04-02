using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DTO;

namespace W1400.DataAccess.DAO
{
    public interface MessageDAO
    {
        List<DBMessage> SP_Get_MO(string Keyword, int CurrPage, int RecordPerPage, out int TotalRecord);
    }
}
