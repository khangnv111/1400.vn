using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W1400.DataAccess.DAO
{
    public interface SuggestDAO
    {
        void SP_Suggestion_Create(string Email, string Mobile, string Suggestion, out int ResponseStatus);
    }
}
