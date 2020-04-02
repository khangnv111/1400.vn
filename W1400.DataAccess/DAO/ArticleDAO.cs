using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DTO;

namespace W1400.DataAccess.DAO
{
    public interface ArticleDAO
    {
        List<Article> SP_Article_GetList_Web(int TopRow, int ArticleID, string Title, int MenuID, string UrlRedirect, string Tags, int isHot, int Page, int PageSize, out int TotalRow);
        List<Menu> SP_Menu_GetList(string MenuIDs, Int16 GetChild, Int16 Status, out int TotalRow);
        List<Article> SP_Article_GetListSameMenu_Web(int TopRow, int ArticleID, int Page, int PageSize, out int TotalRow);
        List<Menu> SP_Menu_GetByUrlRedirect(string UrlRedirect);
        int SP_Article_AddView_Web(int ArticleID);
    }
}
