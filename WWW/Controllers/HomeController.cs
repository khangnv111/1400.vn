using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W1400.DataAccess.DTO;
using W1400.DataAccess.Factory;

namespace WWW.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            return View();
        }

       
        public ActionResult ArticleHotPartial()
        {
            int Total = 0;
            var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(5, 0, "", 0, "", "", 1, 1, 5, out Total);
            return PartialView(lst);
        }
        public ActionResult ArticlePartial()
        {

            return PartialView();
        }
        public ActionResult VideoPartial(int? menuId)
        {
            try
            {
                int MenuId = menuId == null ? 9 : (int)menuId;
                int Total = 0;
                var lst = AbstractDAOFactory.Instance().CreateArticleDAO().SP_Article_GetList_Web(4, 0, "", MenuId, "", "", -1, 1, 4, out Total);

                ViewBag.list = lst;
            }
            catch (Exception e)
            {
                W1400.Utility.NLogLogger.LogInfo("ERROR: " + e.ToString());
            }
            return PartialView();
        }
    }
}