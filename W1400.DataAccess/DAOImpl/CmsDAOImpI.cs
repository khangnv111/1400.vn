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
    public class CmsDAOImpI : CmsDAO
    {
        #region  Menu
        public List<Menu> SP_Menu_GetList(string MenuIDs, int GetChild, int Status, out int TotalRow)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@MenuIDs", MenuIDs);
                pars[1] = new SqlParameter("@GetChild", GetChild);
                pars[2] = new SqlParameter("@Status", Status);
                pars[3] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Menu>("SP_Menu_GetList", pars);
                TotalRow = Convert.ToInt32(pars[3].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                TotalRow = 0;
                return new List<Menu>();
            }

        }

        public Menu SP_Menu_GetDetail(int MenuID)
        {
            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@MenuID", MenuID);
                var list = new DBHelper(Config.Site1400ConnectionString).GetInstanceSP<Menu>("SP_Menu_GetDetail", pars);
                
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return new Menu();
            }

        }

        public int SP_Menu_INUP_CMS(Menu menu)
        {
            try
            {
                var pars = new SqlParameter[9];
                pars[0] = new SqlParameter("@MenuID", menu.MenuID);
                pars[1] = new SqlParameter("@MenuName", menu.MenuName);
                pars[2] = new SqlParameter("@MenuDesc", menu.MenuDesc);
                pars[3] = new SqlParameter("@FatherID", menu.FatherID);
                pars[4] = new SqlParameter("@Url", menu.Url);
                pars[5] = new SqlParameter("@UrlRedirect", menu.UrlRedirect);
                pars[6] = new SqlParameter("@Status", menu.Status);
                pars[7] = new SqlParameter("@CreateUser", menu.CreateUser);
                pars[8] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_Menu_INUP_CMS", pars);
                return Convert.ToInt32(pars[8].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -99;
            }
        }
        #endregion

        #region Article
        //get list
        public List<Article> SP_Article_GetList_CMS(int ArticleID, string Title, int MenuID, string Tags, int isHot, int Status, DateTime FromDate, DateTime ToDate, int Page, int PageSize, out int TotalRow)
        {
            try
            {
                var pars = new SqlParameter[11];
                pars[0] = new SqlParameter("@ArticleID", ArticleID);
                pars[1] = new SqlParameter("@Title", Title);
                pars[2] = new SqlParameter("@MenuID", MenuID);
                pars[3] = new SqlParameter("@Tags", Tags);
                if (isHot == -1)
                {
                    pars[4] = new SqlParameter("@isHot", DBNull.Value);
                }
                else if(isHot == 0)
                {
                    pars[4] = new SqlParameter("@isHot", false);
                }
                else
                {
                    pars[4] = new SqlParameter("@isHot", true);
                } 
                pars[5] = new SqlParameter("@Status", Status); 
                pars[6] = new SqlParameter("@FromDate", FromDate);
                pars[7] = new SqlParameter("@ToDate", ToDate); 
                pars[8] = new SqlParameter("@Page", Page);
                pars[9] = new SqlParameter("@PageSize", PageSize);
                pars[10] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Article>("SP_Article_GetList_CMS", pars);
                TotalRow = Convert.ToInt32(pars[10].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                TotalRow = 0;
                return new List<Article>();
            }

        }

        public int SP_Article_INUP_CMS(Article article)
        {
            try
            {
                var pars = new SqlParameter[16];
                pars[0] = new SqlParameter("@ArticleID", article.ArticleID);
                pars[1] = new SqlParameter("@Title", article.Title);
                pars[2] = new SqlParameter("@MenuID", article.MenuID);
                pars[3] = new SqlParameter("@Description", article.Description);
                pars[4] = new SqlParameter("@Detail", article.Detail);
                pars[5] = new SqlParameter("@Image", article.Image);
                pars[6] = new SqlParameter("@ImageDetail", article.ImageDetail);
                pars[7] = new SqlParameter("@ImageHot", article.ImageHot);
                pars[8] = new SqlParameter("@MetaData", article.MetaData);
                pars[9] = new SqlParameter("@BottomDesc", article.BottomDesc);
                pars[10] = new SqlParameter("@Tags", article.Tags);
                pars[11] = new SqlParameter("@PublishDate", article.PublishDate);
                pars[12] = new SqlParameter("@isHot", article.isHot);
                pars[13] = new SqlParameter("@Author", article.Author);
                pars[14] = new SqlParameter("@CreateUser", article.CreateUser);
                pars[15] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_Article_INUP_CMS", pars);
                return Convert.ToInt32(pars[15].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -99;
            }
        }

        public int SP_Article_UpdateStatus_CMS(int ArticleID, int Status, string CreateUser)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@ArticleID", ArticleID);
                pars[1] = new SqlParameter("@Status", Status);
                pars[2] = new SqlParameter("@CreateUser", CreateUser);
                pars[3] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_Article_UpdateStatus_CMS", pars);
                return Convert.ToInt32(pars[3].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -99;
            }
        }

        public int SP_Article_UpdateHotOrder_CMS(int ArticleID, int Type, string CreateUser)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@ArticleID", ArticleID);
                pars[1] = new SqlParameter("@Type", Type);
                pars[2] = new SqlParameter("@CreateUser", CreateUser);
                pars[3] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_Article_UpdateHotOrder_CMS", pars);
                return Convert.ToInt32(pars[3].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -99;
            }
        }
        #endregion

        #region List anh trong Album
        public List<ArticleImage> SP_ArticleImage_GetList(int TopRow, int ImageID, int ArticleID, int Status, int Page, int PageSize, out int TotalRow)
        {
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@TopRow", TopRow);
                pars[1] = new SqlParameter("@ImageID", ImageID);
                pars[2] = new SqlParameter("@ArticleID", ArticleID);
                pars[3] = new SqlParameter("@Status", Status);
                pars[4] = new SqlParameter("@Page", Page);
                pars[5] = new SqlParameter("@PageSize", PageSize);
                pars[6] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<ArticleImage>("SP_ArticleImage_GetList", pars);
                TotalRow = Convert.ToInt32(pars[6].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                TotalRow = 0;
                return new List<ArticleImage>();
            }

        }

        public int SP_ArticleImage_Add_CMS(int ArticleID, string FilePath, string CreateUser)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@ArticleID", ArticleID);
                pars[1] = new SqlParameter("@FilePath", FilePath);
                pars[2] = new SqlParameter("@CreateUser", CreateUser);
                pars[3] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_ArticleImage_Add_CMS", pars);
                return Convert.ToInt32(pars[3].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -99;
            }
        }

        public int SP_ArticleImage_Update_CMS(int ImageID, string FilePath, int Status, string CreateUser, string Description)
        {
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@ImageID", ImageID);
                pars[1] = new SqlParameter("@FilePath", FilePath);
                pars[2] = new SqlParameter("@Description", Description);
                pars[3] = new SqlParameter("@Status", Status);
                pars[4] = new SqlParameter("@CreateUser", CreateUser);
                pars[5] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_ArticleImage_Update_CMS", pars);
                return Convert.ToInt32(pars[5].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                return -99;
            }
        }
        #endregion

        #region chuong trinh
        public List<Event> SP_Event_GetList_CMS(int EventID, string EventName, int IsDone, int IsSetted, int Status, DateTime FromDate, DateTime ToDate, int Page, int PageSize, out int TotalRow)
        {
            try
            {
                var pars = new SqlParameter[10];
                pars[0] = new SqlParameter("@EventID", EventID);
                pars[1] = new SqlParameter("@EventName", EventName);
                if (IsDone == -1)
                {
                    pars[2] = new SqlParameter("@IsDone", DBNull.Value);
                }
                else
                {
                    pars[2] = new SqlParameter("@IsDone", IsDone);
                }
                if (IsSetted == -1)
                {
                    pars[3] = new SqlParameter("@IsSetted", DBNull.Value);
                }
                else
                {
                    pars[3] = new SqlParameter("@IsSetted", IsSetted);
                }
                pars[4] = new SqlParameter("@Status", Status);
                pars[5] = new SqlParameter("@FromDate", FromDate);
                pars[6] = new SqlParameter("@ToDate", ToDate);
                pars[7] = new SqlParameter("@Page", Page);
                pars[8] = new SqlParameter("@PageSize", PageSize);
                pars[9] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Event>("SP_Event_GetList_CMS", pars);
                TotalRow = Convert.ToInt32(pars[9].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                TotalRow = 0;
                return new List<Event>();
            }

        }

        public int SP_Event_INUP_CMS(Event x)
        {
            try
            {
                var pars = new SqlParameter[19];
                pars[0] = new SqlParameter("@EventID", x.EventID);
                pars[1] = new SqlParameter("@EventName", x.EventName);
                pars[2] = new SqlParameter("@EventDetail", x.EventDetail);
                pars[3] = new SqlParameter("@StartDate", x.StartDate);
                pars[4] = new SqlParameter("@EndDate", x.EndDate);
                pars[5] = new SqlParameter("@Porpose", x.Porpose);
                pars[6] = new SqlParameter("@HostUnit", x.HostUnit);
                pars[7] = new SqlParameter("@CoordUnit", x.CoordUnit);
                pars[8] = new SqlParameter("@LegalDoc", x.LegalDoc);
                pars[9] = new SqlParameter("@LegalFile", x.LegalFile);
                pars[10] = new SqlParameter("@Revenue", x.Revenue);
                pars[11] = new SqlParameter("@Result", x.Result);
                pars[12] = new SqlParameter("@Image", x.Image);
                pars[13] = new SqlParameter("@Detail", x.Detail);
                pars[14] = new SqlParameter("@IsDone", x.IsDone);
                pars[15] = new SqlParameter("@IsSetted", x.IsSetted);
                pars[16] = new SqlParameter("@Status", x.Status);
                pars[17] = new SqlParameter("@CreateUser", x.CreateUser);
                pars[18] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_Event_INUP_CMS", pars);
                return Convert.ToInt32(pars[18].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Error: " + ex.ToString());
                return -99;
            }
        }
        public int SP_Event_Updatestatus_CMS(Event x)
        {
            try
            {
                var pars = new SqlParameter[19];
                pars[0] = new SqlParameter("@EventID", x.EventID);
                pars[1] = new SqlParameter("@EventName", DBNull.Value);
                pars[2] = new SqlParameter("@EventDetail", DBNull.Value);
                pars[3] = new SqlParameter("@StartDate", DBNull.Value);
                pars[4] = new SqlParameter("@EndDate", DBNull.Value);
                pars[5] = new SqlParameter("@Porpose", DBNull.Value);
                pars[6] = new SqlParameter("@HostUnit", DBNull.Value);
                pars[7] = new SqlParameter("@CoordUnit", DBNull.Value);
                pars[8] = new SqlParameter("@LegalDoc", DBNull.Value);
                pars[9] = new SqlParameter("@LegalFile", DBNull.Value);
                pars[10] = new SqlParameter("@Revenue", DBNull.Value);
                pars[11] = new SqlParameter("@Result", DBNull.Value);
                pars[12] = new SqlParameter("@Image", DBNull.Value);
                pars[13] = new SqlParameter("@Detail", DBNull.Value);
                pars[14] = new SqlParameter("@IsDone", DBNull.Value);
                pars[15] = new SqlParameter("@IsSetted", DBNull.Value);
                pars[16] = new SqlParameter("@Status", x.Status);
                pars[17] = new SqlParameter("@CreateUser", x.CreateUser);
                pars[18] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_Event_INUP_CMS", pars);
                return Convert.ToInt32(pars[18].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Error: " + ex.ToString());
                return -99;
            }
        }
        public List<Event> SP_Event_GetDetail_CMS(int EventID, int ServiceID, int Page, int PageSize, out int TotalRow)
        {
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@EventID", EventID);
                pars[1] = new SqlParameter("@ServiceID", ServiceID);
                pars[2] = new SqlParameter("@Page", Page);
                pars[3] = new SqlParameter("@PageSize", PageSize);
                pars[4] = new SqlParameter("@TotalRow", DbType.Int32) { Direction = ParameterDirection.Output };

                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Event>("SP_Event_GetDetail_CMS", pars);
                TotalRow = Convert.ToInt32(pars[4].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Error: " + ex.ToString());
                TotalRow = 0;
                return new List<Event>();
            }
        }

        public List<EventReport> SP_Event_GetReport_CMS(int EventID, DateTime FromDate, DateTime ToDate, out int TotalRow)
        {
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@EventID", EventID);
                pars[1] = new SqlParameter("@FromDate", FromDate);
                pars[2] = new SqlParameter("@ToDate", ToDate);
                pars[3] = new SqlParameter("@TotalRow", DbType.Int32) { Direction = ParameterDirection.Output };

                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<EventReport>("SP_Event_GetReport_CMS", pars);
                TotalRow = Convert.ToInt32(pars[3].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Error: " + ex.ToString());
                TotalRow = 0;
                return new List<EventReport>();
            }
        }
        #endregion

        #region gop y
        public List<Suggest> SP_Suggestion_List_CMS(int SuggestionID, string Mobile, string Email, DateTime? FromDate, DateTime? ToDate, int Status, int Page, int PageSize, out int TotalRow)
        {
            try
            {
                var pars = new SqlParameter[9];
                pars[0] = new SqlParameter("@SuggestionID", SuggestionID);
                pars[1] = new SqlParameter("@Mobile", Mobile);
                pars[2] = new SqlParameter("@Email", Email);
                pars[3] = new SqlParameter("@Status", Status);
                pars[4] = new SqlParameter("@FromDate", FromDate);
                pars[5] = new SqlParameter("@ToDate", ToDate);
                pars[6] = new SqlParameter("@Page", Page);
                pars[7] = new SqlParameter("@PageSize", PageSize);
                pars[8] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<Suggest>("SP_Suggestion_List_CMS", pars);
                TotalRow = Convert.ToInt32(pars[8].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                TotalRow = 0;
                return new List<Suggest>();
            }

        }

        public int SP_Suggestion_Answer_CMS(int SuggestionID, string Answer, string CreateUser, int Status)
        {
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@SuggestionID", SuggestionID);
                pars[1] = new SqlParameter("@Answer", Answer);
                pars[2] = new SqlParameter("@CreateUser", CreateUser);
                pars[3] = new SqlParameter("@Status", Status);
                pars[4] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_Suggestion_Answer_CMS", pars);
                return Convert.ToInt32(pars[4].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Error: " + ex.ToString());
                return -99;
            }
        }
        #endregion

        #region Bao cao chuong trinh
        public List<EventReport> SP_EventReport_GetList_CMS(int LogID, int EventID, string Vender, DateTime? FromDate, DateTime? ToDate, int Page, int PageSize, out int TotalRow)
        {
            try
            {
                var pars = new SqlParameter[8];
                pars[0] = new SqlParameter("@LogID", LogID);
                pars[1] = new SqlParameter("@EventID", EventID);
                pars[2] = new SqlParameter("@Vender", Vender);
                pars[3] = new SqlParameter("@FromDate", FromDate);
                pars[4] = new SqlParameter("@ToDate", ToDate);
                pars[5] = new SqlParameter("@Page", Page);
                pars[6] = new SqlParameter("@PageSize", PageSize);
                pars[7] = new SqlParameter("@TotalRow", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var list = new DBHelper(Config.Site1400ConnectionString).GetListSP<EventReport>("SP_EventReport_GetList_CMS", pars);
                TotalRow = Convert.ToInt32(pars[7].Value);
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo(ex.ToString());
                TotalRow = 0;
                return new List<EventReport>();
            }

        }

        public int SP_EventReport_INUP_CMS(EventReport x)
        {
            try
            {
                var pars = new SqlParameter[17];
                pars[0] = new SqlParameter("@LogID", x.LogID);
                pars[1] = new SqlParameter("@EventID", x.EventID);
                pars[2] = new SqlParameter("@Vender", x.Vender);
                pars[3] = new SqlParameter("@DataControl", x.DataControl);
                pars[4] = new SqlParameter("@Pay1400", x.Pay1400);
                pars[5] = new SqlParameter("@PayCharit", x.PayCharit);
                pars[6] = new SqlParameter("@Note", x.Note);
                pars[7] = new SqlParameter("@DataReviewTime", x.DataReviewTime);
                pars[8] = new SqlParameter("@Pay14ReviewTime", x.Pay14ReviewTime);
                pars[9] = new SqlParameter("@PayCharReviewTime", x.PayCharReviewTime);
                pars[10] = new SqlParameter("@TimeAnswer", x.TimeAnswer);
                pars[11] = new SqlParameter("@TimeSignRecord", x.TimeSignRecord);
                pars[12] = new SqlParameter("@TimehsPay", x.TimehsPay);
                pars[13] = new SqlParameter("@TimeTelcoPay", x.TimeTelcoPay);
                pars[14] = new SqlParameter("@TimePay", x.TimePay);
                pars[15] = new SqlParameter("@CreateUser", x.CreateUser);
                pars[16] = new SqlParameter("@ResponseStatus", DbType.Int32) { Direction = ParameterDirection.Output };

                new DBHelper(Config.Site1400ConnectionString).ExecuteNonQuerySP("SP_EventReport_INUP_CMS", pars);
                return Convert.ToInt32(pars[16].Value);
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("Error: " + ex.ToString());
                return -99;
            }
        }
        #endregion
    }
}
