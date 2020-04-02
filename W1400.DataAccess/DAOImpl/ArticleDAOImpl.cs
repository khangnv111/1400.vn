using DBHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DAO;
using W1400.DataAccess.DTO;
using W1400.Utility;

namespace W1400.DataAccess.DAOImpl
{
    public class ArticleDAOImpl : ArticleDAO
    {
        public List<Article> SP_Article_GetList_Web(int TopRow, int ArticleID, string Title, int MenuID,string UrlRedirect, string Tags, int isHot, int Page, int PageSize, out int TotalRow)
        {
            TotalRow = 0;
            try
            {
                var pars = new SqlParameter[10];
                pars[0] = new SqlParameter("@TopRow", TopRow);
                pars[1] = new SqlParameter("@ArticleID", ArticleID );
                pars[2] = new SqlParameter("@Title", Title );
                pars[3] = new SqlParameter("@MenuID", MenuID );
                pars[4] = new SqlParameter("@UrlRedirect", UrlRedirect);
                pars[5] = new SqlParameter("@Tags", Tags);
                if (isHot==-1)
                pars[6] = new SqlParameter("@isHot", DBNull.Value);
                else if (isHot==1)
                pars[6] = new SqlParameter("@isHot", true);
                else if (isHot == 0)
                pars[6] = new SqlParameter("@isHot", false);
                pars[7] = new SqlParameter("@Page", Page);
                pars[8] = new SqlParameter("@PageSize", PageSize);
                pars[9] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Article>("SP_Article_GetList_Web", pars);
                if (list != null || list.Count >= 0)
                {
                    TotalRow = Convert.ToInt32(pars[9].Value);
                }
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<Article>();
            }
        }
        public List<Menu> SP_Menu_GetList(string MenuIDs, Int16 GetChild, Int16 Status, out int TotalRow)
        {
            TotalRow = 0;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@MenuIDs", MenuIDs);
                pars[1] = new SqlParameter("@GetChild", GetChild);
                pars[2] = new SqlParameter("@Status", Status);
                pars[3] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Menu>("SP_Menu_GetList", pars);
                if (list != null || list.Count >= 0)
                {
                    TotalRow = Convert.ToInt32(pars[3].Value);
                }
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<Menu>();
            }
        }
        public List<Article> SP_Article_GetListSameMenu_Web(int TopRow, int ArticleID, int Page, int PageSize, out int TotalRow)
        {
            TotalRow = 0;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@TopRow", TopRow);
                pars[1] = new SqlParameter("@ArticleID", ArticleID );               
                pars[2] = new SqlParameter("@Page", Page);
                pars[3] = new SqlParameter("@PageSize", PageSize);
                pars[4] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Article>("SP_Article_GetListSameMenu_Web", pars);
                if (list != null || list.Count >= 0)
                {
                    TotalRow = Convert.ToInt32(pars[4].Value);
                }
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new List<Article>();
            }
        }
        public List<Menu> SP_Menu_GetByUrlRedirect(string UrlRedirect)
        {
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@UrlRedirect", UrlRedirect);
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Menu>("SP_Menu_GetByUrlRedirect", pars);
                if (list != null || list.Count >= 0)
                {
                    return list;
                }
                else
                { 
                    return null;
                }
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return null;
            }
        }
        public int SP_Article_AddView_Web(int ArticleID)
        {
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@ArticleID", ArticleID);
                pars[1] = new SqlParameter("@ResponseStatus", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Menu>("SP_Article_AddView_Web", pars);
                return Convert.ToInt32(pars[1].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return  -99;
            }
        }
    }
}
