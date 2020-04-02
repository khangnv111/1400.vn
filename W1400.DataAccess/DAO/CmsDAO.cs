using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DTO;

namespace W1400.DataAccess.DAO
{
    public interface CmsDAO
    {
        //Menu
        List<Menu> SP_Menu_GetList(string MenuIDs, int GetChild, int Status, out int TotalRow);
        Menu SP_Menu_GetDetail(int MenuID);
        int SP_Menu_INUP_CMS(Menu menu);

        //Article
        List<Article> SP_Article_GetList_CMS(int ArticleID, string Title, int MenuID, string Tags, int isHot, int Status, DateTime FromDate, DateTime ToDate, int Page, int PageSize, out int TotalRow); 
        int SP_Article_INUP_CMS(Article article); 
        int SP_Article_UpdateStatus_CMS(int ArticleID, int Status, string CreateUser);
        int SP_Article_UpdateHotOrder_CMS(int ArticleID, int Type, string CreateUser);
        
        //list anh trong album
        List<ArticleImage> SP_ArticleImage_GetList(int TopRow, int ImageID, int ArticleID, int Status, int Page, int PageSize, out int TotalRow);
        int SP_ArticleImage_Add_CMS(int ArticleID, string FilePath, string CreateUser);
        int SP_ArticleImage_Update_CMS(int ImageID, string FilePath, int Status, string CreateUser, string Description);

        //chuong trinh
        List<Event> SP_Event_GetList_CMS(int EventID, string EventName, int IsDone, int IsSetted, int Status, DateTime FromDate, DateTime ToDate, int Page, int PageSize, out int TotalRow); 
        int SP_Event_INUP_CMS(Event x);
        List<Event> SP_Event_GetDetail_CMS(int EventID, int ServiceID, int Page, int PageSize, out int TotalRow);
        List<EventReport> SP_Event_GetReport_CMS(int EventID, DateTime FromDate, DateTime ToDate, out int TotalRow);

        int SP_Event_Updatestatus_CMS(Event x);

        //goi y
        List<Suggest> SP_Suggestion_List_CMS(int SuggestionID, string Mobile, string Email, DateTime? FromDate, DateTime? ToDate, int Status, int Page, int PageSize, out int TotalRow);
        int SP_Suggestion_Answer_CMS(int SuggestionID, string Answer, string CreateUser, int Status);

        //bao cao chuong trinh
        List<EventReport> SP_EventReport_GetList_CMS(int LogID, int EventID, string Vender, DateTime? FromDate, DateTime? ToDate, int Page, int PageSize, out int TotalRow);
        int SP_EventReport_INUP_CMS(EventReport x);
    }
}
