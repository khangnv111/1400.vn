using System;

namespace W1400.DataAccess.DTO
{
    public class Users
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public bool IsAdministrator { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUser { get; set; }
        public bool Status { get; set; }

    }
    //Quyền chức năng
    public class UserFunction
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int FunctionID { get; set; }
        public string FunctionName { get; set; }
        public string ActionName { get; set; }
        public int FatherID { get; set; }
        public string FatherName { get; set; }
        public bool IsGrant { get; set; }
        public bool IsInsert { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public int CreatedUserID { get; set; }
        public bool IsRead { get; set; }
    }
    public class AccountLoginWeb
    {
        public long AccountID { get; set; }
        public string Email { get; set; }
        public string Namedisplay { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}
