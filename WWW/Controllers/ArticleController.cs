using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W1400.DataAccess.DTO;
using W1400.DataAccess.Factory;
using W1400.Utility;

namespace WWW.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index(string UrlRewrite, int? Page)
        {
            int curPage = 1;
            if (Page != null) curPage = (int)Page;
            int Total=0;
            var lst =new List<Article>();
            var article = new Article();
            ViewBag.CateName="";
            lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(10, 0, "", 0, UrlRewrite, "", -1, curPage, 10, out Total);
            if (lst != null && lst.Count > 0)
            {
                article = lst[0];
            }
            else
            {
                article = null;
            }
            var lst_menu = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Menu_GetByUrlRedirect(UrlRewrite);
            if (lst_menu != null && lst_menu.Count>0)
            {
                ViewBag.CateName = lst_menu[0].MenuName;
            }
            ViewBag.Article = article;
            ViewBag.UrlRedirect = UrlRewrite;
            
            return View(lst);
        }
        public ActionResult ArticleAll()
        {
            int Total = 0;
            var lst =new List<Article>();
            var article = new Article();
            lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(1, 0, "", 5, "", "", -1, 1, 10, out Total);
            if (lst != null && lst.Count > 0)
            {
                article = lst[0];
            }
            else
            {
                article = null;
            }
            ViewBag.Article = article;
            return View(lst);
        }

        public ActionResult VideoImage()
        {
            int Total1 = 0;
            //var article = new Article();
            var article = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(1, 0, "", 9, "", "", -1, 1, 1, out Total1).FirstOrDefault();
            ViewBag.Article = article;
            NoMark rw = new NoMark();
            if (article != null)
            {
                var rewrite = rw.ReWrite(article.Title, article.ArticleID);
                ViewBag.rewrite = rewrite;
            }
            
            int Total2 = 0;
            //var list_video = new List<Article>();
            var list_video = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(4, 0, "", 11, "", "", -1, 1, 4, out Total2);
            ViewBag.list_video = list_video;

            int Total3 = 0;
            //var list_album = new List<Article>();
            var list_album = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(4, 0, "", 10, "", "", -1, 1, 4, out Total3);
            ViewBag.list_album = list_album;
            
            return View();
        }

        public ActionResult ArticleAllPartial(string UrlRewrite_)
         {
            int Total = 0;
            var lst = new List<Article>();
            var article = new Article();
            if (UrlRewrite_=="tin-tuc")
            { 
                lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(10, 0, "", 5, "", "", -1, 1, 4, out Total);
            }
            else
            {
                lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(10, 0, "", 0, UrlRewrite_, "", -1, 1, 4, out Total);
            }
            return PartialView(lst);
        }
        public ActionResult ArticleHomePartial(int CateId)
        {
            int Total = 0;
            var lst = new List<Article>();
            var article = new Article();
            if (CateId == -1)
            {
                lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(10, 0, "", 5, "", "", -1, 1, 4, out Total);
            }
            else
            {
                lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(10, 0, "", CateId, "", "", -1, 1, 4, out Total);
            }
            return PartialView(lst);
        }
        public ActionResult OnlineSupport()
        {
            return View();
        }
        public ActionResult OnlineSupportPartial(int curPage)
        {
            int RecordPerPage = 8;
            int Total = 0;
            var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(1000, 0, "", 0, "ung-ho-truc-tuyen", "", -1, curPage, RecordPerPage, out Total);

            ViewBag.TotalRecord = Total;
            ViewBag.currentPage = curPage;

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
            
            return PartialView(lst);
        }

        public ActionResult ArticleOneCatePartial(string UrlRedirect, int? Page)
        {
            int RecordPerPage = 10;
            int curPage = 1;
            if (Page != null) curPage = (int)Page;

            int Total = 0;
            var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(1000, 0, "", 0, UrlRedirect, "", -1, curPage, RecordPerPage, out Total);
            
            ViewBag.TotalRecord = Total;
            ViewBag.currentPage = curPage;

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
            ViewBag.UrlRedirect = UrlRedirect;

            return PartialView(lst);
        }
        public ActionResult Detail(int id)
        {
            try
            {

                int result = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_AddView_Web(id);//Tăng view

                int Total = 0;
                var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(1, id, "", 0, "", "", -1, 1, 1, out Total);
                
                //truong hop la album anh
                if (lst[0].MenuID == 10)
                {
                    int TotalImg = 0;
                    var listimg = AbstractDAOFactory.Instance().CreateCmsDAO().SP_ArticleImage_GetList(1000, 0, id, 1, 1, 1000, out TotalImg);
                    ViewBag.listimg = listimg;
                }
                //end

                ViewBag.Id = id;
                ViewBag.Title = "";
                ViewBag.Image = "";
                ViewBag.Summary = "";
                ViewBag.Keyword = "";
                if (lst != null && lst.Count > 0)
                {
                    ViewBag.Title = lst[0].Title;
                    ViewBag.Image = lst[0].Image ?? "";
                    ViewBag.Summary = lst[0].Description;
                    ViewBag.Keyword = lst[0].Description;

                    if (!String.IsNullOrEmpty(lst[0].Detail))
                    {
                        lst[0].Detail = lst[0].Detail.Replace("<img ", "<img class=\"img-responsive\"");
                    }
                    
                    return View(lst[0]);
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
            catch (Exception ex)
            {
                NLogLogger.LogInfo("ERROR in Article/Detail = " + ex.ToString());
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult RelationArticlePartial(int ArticleId,int CurrentPage)
        {
            int RecordPerPage = 2;
            int Total = 0;
            var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetListSameMenu_Web(10, ArticleId, CurrentPage, RecordPerPage, out Total);

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
            ViewBag.ArticleId = ArticleId;
            ViewBag.TotalPage = totalPage;
            return PartialView(lst);
        }
        public ActionResult ArticleSearch(string keyword, int CurrentPage)
        {
            int RecordPerPage = 10;
            int Total = 0;
            var lst = new List<Article>();
            var article = new Article();
            lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(10000, 0, keyword, 0, "", "", -1, CurrentPage, RecordPerPage, out Total);

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

            return PartialView(lst);
        }
    }
}
