using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W1400.DataAccess.DTO
{
    public class Program
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Porpose { get; set; }
        public string HostUnit { get; set; }
        public string CoordUnit { get; set; }
        public string LegalDoc { get; set; }
        public string LegalFile { get; set; }
        public string Revenue { get; set; }
        public string Result { get; set; }
        public string Image { get; set; }
        public string Detail { get; set; }
        public bool IsDone { get; set; }
        public bool IsSetted { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
    public class ProgramDetail
    {
        public long STT { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string Commandcode { get; set; }
        public int ServiceID { get; set; }
        public int Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Order { get; set; }        
    }
    public class ProgramReport
    {
        
        public int EventID { get; set; }
        public int SMSC_ID { get; set; }
        public string SMSHost { get; set; }
        public string CommandCode { get; set; }
        public string ServiceID { get; set; }
        public int Price { get; set; }
        public int Total_SMS { get; set; }
    }
}
