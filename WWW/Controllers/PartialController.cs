using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W1400.DataAccess.Factory;
using W1400.Utility;
namespace WWW.Controllers
{
    public class PartialController : Controller
    {
        public ActionResult ProgramPartial()
        {
            int Total=0;
            var list = AbstractDAOFactory.Instance().CreateProgramDAO().SP_Event_GetList_Web(0,"",-1,1,6,out Total);
            return PartialView(list);
        }
        public ActionResult MessageHistory()
        {
            return PartialView();
        }
        public ActionResult MessageHistoryPartial(string Keyword,int CurrentPage)
        {
            int RecordPerPage = 5;
            int Total = 0;
            DateTime start = DateTime.Now;
            var list = AbstractDAOFactory.Instance().CreateMessageDAO().SP_Get_MO(Keyword, CurrentPage, RecordPerPage, out Total);
            DateTime end = DateTime.Now;
            TimeSpan t = end - start;
            int s = t.Seconds;
            NLogLogger.LogInfo("thoi gian call db =" + s.ToString());
            ViewBag.TotalRecord = Total;
            ViewBag.currentPage = CurrentPage;

            int totalPage = 1;
            if (Total % RecordPerPage == 0)
            {
                totalPage = (int)(Total / RecordPerPage);
            }
            else
            {
                totalPage = (int)(Total / RecordPerPage + 1);
            }

            ViewBag.TotalPage = totalPage;

            return PartialView(list);
        }
        public ActionResult FeedbackPartial()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult SendSuggest(string Email, string Mobile, string Content)
        {
            int Result = 0;
            AbstractDAOFactory.Instance().CreateSuggestDAO().SP_Suggestion_Create(Email, Mobile, Content, out Result);
            return Json(new { success = Result });
        }
        public ActionResult MenuPartial()
        {
            int Total = 0;
            var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Menu_GetList("",1,1,out Total);
            return PartialView(lst);
        }
        
        public ActionResult MenuArticle(string UrlRewrite)
        {
            string MenuId="5";//Lấy cate con của Cate tin tức
            if (UrlRewrite == "van-ban-phap-ly" || UrlRewrite == "cau-hoi-thuong-gap")
                MenuId = "1";//Lấy cate con của Cate giới thiệu

            //Album / Video
            if (UrlRewrite == "album-anh" || UrlRewrite == "video" || UrlRewrite == "video-anh")
                MenuId = "9";//Lấy cate con của Cate giới thiệu

            int Total = 0;
            var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Menu_GetList(MenuId, 1, 1, out Total);
            return PartialView(lst);
        }
        public ActionResult MenuArticleHome(string UrlRewrite)
        {
            string MenuId = "5";//Lấy cate con của Cate tin tức
            if (UrlRewrite == "van-ban-phap-ly" || UrlRewrite == "cau-hoi-thuong-gap")
                MenuId = "1";//Lấy cate con của Cate giới thiệu

            //Album / Video
            if (UrlRewrite == "album-anh" || UrlRewrite == "video" || UrlRewrite == "video-anh")
                MenuId = "9";//Lấy cate con của Cate giới thiệu

            int Total = 0;
            var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Menu_GetList(MenuId, 1, 1, out Total);
            return PartialView(lst);
        }
        public ActionResult MenuFooterPartial()
        {
            int Total = 0;
            var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Menu_GetList("",1,1,out Total);
            return PartialView(lst);
        }
        
    }
}
