using DBHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DTO;
using W1400.Utility;

namespace W1400.DataAccess.DAO
{
    public interface ProgramDAO
    {
        List<Program> SP_Event_GetList_Web(int EventID, string EventName, int IsSetted, int Page, int PageSize, out int TotalRow);
        List<ProgramDetail> SP_Event_GetDetail_Web(int EventID, out int TotalRow);
        List<ProgramReport> SP_Event_GetReport_Web(int EventID, DateTime FromDate, DateTime ToDate, out int TotalRow);
        List<Program> SP_Event_OldGetList_Web(int EventID, string EventName, int IsSetted, int Year, int Page, int PageSize, out int TotalRow);
    }
}
