using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DTO;
namespace W1400.DataAccess.DAO
{
    public interface IUsersDAO
    {
        int Authentication(string username, string password);
        Users SelectByUserID(int userId);
        Users GetByEmail(string email);
        Users GetByUsername(string Username);
        List<Users> GetListUsers(string Keyword, int isActive, int isGrant);
        List<Users> GetAllUserByStatus(int Status);
        int UpdateUsers(Users users);
        int DelleteUsers(int userId);
        int UpdateActiveUser(int Id);
        int ChangePassword(string UserName, string PasswordOld, string PasswordNew);

        AccountLoginWeb SP_AccountLoginWeb_GetByEmail(string Email);
         int SP_AccountLoginWeb_InSertUpdate(AccountLoginWeb acc,int Type);
    }
}
