using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W1400.DataAccess.DTO
{
    public class AccessTokenInfo
    {
        public string AccessToken { get; set; }
        public decimal GameBalance { get; set; }
        public int ResponseStatus { get; set; }
        public decimal TotalGameBalance { get; set; }
        public int VcoinBalance { get; set; }
    }
}
