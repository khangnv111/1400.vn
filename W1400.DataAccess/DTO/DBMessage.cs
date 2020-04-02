using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W1400.DataAccess.DTO
{
    public class DBMessage
    {
        public int STT { get; set; }
        public int MT_ID { get; set; }
        public int MO_ID { get; set; }
        public string UserID { get; set; }
        public string ServiceID { get; set; }
        public string CommandCode { get; set; }
        public string Message { get; set; }
        public Int32 MsgType { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public Int32 IsCDR { get; set; }
    }
}
