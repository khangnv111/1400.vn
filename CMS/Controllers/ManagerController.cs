using CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using W1400.DataAccess.DTO;
using W1400.DataAccess.Factory;
using W1400.Utility;

namespace CMS.Controllers
{
    public class ManagerController : Controller
    {
        private UserFunction Permission { get { return ((UserFunction)Session[SessionsManager.SESSION_PERMISSION]); } }

        private CommonLib CommonLib = new CommonLib();

        #region Menu
        public ActionResult MenuManager()
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("MenuManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            ViewBag.UrlLogin = urlLogin;

            return View();
        }

        public ActionResult MenuGetList()
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("MenuManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            var data = new List<Menu>();
            int TotalRecord = 0;
            data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Menu_GetList("", 1, -1, out TotalRecord);

            return PartialView(data);
        }

        public ActionResult MenuDetail(int? ID)
        {
            int MenuID = ID == null ? 0 : (int)ID;
            if (MenuID < 0)
            {
                return RedirectToAction("MenuManager", "Manager");
            }

            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("MenuManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            var data = new Menu();
            if (MenuID > 0)
            {
                data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Menu_GetDetail(MenuID);
            }

            var listmenu = new List<Menu>();
            int TotalRecord = 0;
            listmenu = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Menu_GetList("0", 1, -1, out TotalRecord);
            ViewBag.listmenu = listmenu;

            ViewBag.MenuID = MenuID;
            return View(data);
        }

        [HttpPost]
        public JsonResult MenuInsertUpdate(Menu menu)
        {
            var userValidate = new UserValidation();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }

                if (Permission == null || (menu.MenuID == 0 && !Permission.IsInsert) || menu.MenuID > 0 && !Permission.IsUpdate)
                {
                    return Json(new { ResponseCode = -101, Msg = "Bạn không có quyền sử dụng chức năng này" });
                }
                if (menu.MenuID < 0)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Dữ liệu đầu vào không hợp lệ" });
                }
                if (string.IsNullOrEmpty(menu.MenuName))
                {
                    return Json(new { ResponseCode = -6001, Msg = "Tên Menu không được để trống" });
                }

                menu.CreateUser = userValidate.UserName;

                var updateResult = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Menu_INUP_CMS(menu);
                if (updateResult > 0)
                {
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = " Sử dụng chức năng Update Menu " + userValidate.UserName,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                    return Json(new { ResponseCode = updateResult, Msg = "Cập Nhật Dữ Liệu Thành Công" });

                }
                else if (updateResult == -600)
                {
                    return Json(new { ResponseCode = -600, Msg = "Đầu vào không hợp lệ" });
                }
                else
                {
                    return Json(new { ResponseCode = -99, Msg = "Cập nhật dữ liệu không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }

        }
        #endregion

        #region Article
        public ActionResult ArticleManager(int? status, int? hot)
        {
            var userValidate = new UserValidation();
            int Status = status == null ? -1 : (int)status;
            int Hot = hot == null ? -1 : (int)hot;
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ArticleManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            DateTime FromDate = DateTime.Now.AddMonths(-5);
            DateTime ToDate = DateTime.Now.AddDays(3);
            ViewBag.Fromdate = FromDate.ToString("dd/MM/yyyy");
            ViewBag.ToDate = ToDate.ToString("dd/MM/yyyy");

            var data = new List<Menu>();
            int TotalRecord = 0;
            data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Menu_GetList("", 1, -1, out TotalRecord); //1,5,13
            ViewBag.listMenu = data;

            ViewBag.UrlLogin = urlLogin;
            ViewBag.Status = Status;
            ViewBag.IsHot = Hot;
            return View();
        }

        public ActionResult ArticleGetList(string title, int? menuId, int? hot, int? status, string fromdate, string todate, int? page)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ArticleManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            int MenuId = menuId == null ? 0 : (int)menuId;
            int IsHot = hot == null ? -1 : (int)hot;

            int Status = status == null ? -1 : (int)status;

            DateTime FromDate = DateTime.Now.AddMonths(-5);
            DateTime ToDate = DateTime.Now.AddDays(3);
            try
            {
                FromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                FromDate = DateTime.Now.AddMonths(-5);
            }

            try
            {
                todate = todate + " 23:59:59";
                ToDate = DateTime.ParseExact(todate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                ToDate = DateTime.Now.AddDays(3);
            }

            int CurPage = page == null ? 1 : (int)page;

            var data = new List<Article>();

            int TotalRecord = 0;
            data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Article_GetList_CMS(0, title, MenuId, "", IsHot, Status, FromDate, ToDate, CurPage, 10, out TotalRecord);

            ViewBag.PageSize = 10;
            ViewBag.CurrentPage = CurPage;
            ViewBag.TotalRecord = TotalRecord;
            ViewBag.IsHot = IsHot;
            ViewBag.Status = Status;
            return PartialView(data);
        }

        public ActionResult ArticleDetail(int? ID)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ArticleManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            int ArtID = ID == null ? 0 : (int)ID;
            if (ArtID < 0)
            {
                return RedirectToAction("ArticleManager", "Manager");
            }

            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;

            var data = new Article();

            int TotalRecord = 0;
            if (ArtID > 0)
            {
                data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Article_GetList_CMS(ArtID, "", 0, "", -1, -1, FromDate, ToDate, 1, 1, out TotalRecord).FirstOrDefault();
            }

            ViewBag.ArtID = ArtID;

            var listMenu = new List<Menu>();
            int TotalMenu = 0;
            listMenu = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Menu_GetList("", 1, -1, out TotalMenu); //1,5,13
            ViewBag.listMenu = listMenu;

            return View(data);
        }

        [HttpPost]
        public JsonResult ArticleInUp(Article article)
        {
            var ReturnData = new ReturnData();
            var userValidate = new UserValidation();
            ViewBag.Admin = userValidate.IsAdministrator;
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }

                if (Permission == null || Permission.ActionName != "ArticleManager" || (article.ArticleID == 0 && !Permission.IsInsert) || article.ArticleID > 0 && !Permission.IsUpdate)
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn không có quyền sử dụng chức năng này";
                    return Json(ReturnData);
                }
                if (string.IsNullOrEmpty(article.Title))
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn chưa nhập tiêu đề";
                    return Json(ReturnData);
                }
                //nhập mô tả bài viết
                if (string.IsNullOrEmpty(article.Description))
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn chưa nhập mô tả";
                    return Json(ReturnData);
                }

                if (article.MenuID == 9)
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Mời bạn chọn Ảnh hoặc Video";
                    return Json(ReturnData);
                }
                //nhập link video
                if (article.MenuID == 11 && string.IsNullOrEmpty(article.MetaData))
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn chưa nhập link video";
                    return Json(ReturnData);
                }
                //link redirect của bài ủng hộ trực tuyến
                if (article.MenuID == 13 && string.IsNullOrEmpty(article.MetaData))
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn chưa nhập link gắn kết cho bài viết Ủng hộ trực tuyến";
                    return Json(ReturnData);
                }

                if (string.IsNullOrEmpty(article.Image))
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn chưa chọn ảnh avatar nhỏ";
                    return Json(ReturnData);
                }

                if (string.IsNullOrEmpty(article.ImageDetail))
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn chưa chọn ảnh avatar lớn";
                    return Json(ReturnData);
                }

                if (article.isHot == true && string.IsNullOrEmpty(article.ImageHot))
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn chưa chọn ảnh hot";
                    return Json(ReturnData);
                }

                if (article.PublishDate == null)
                {
                    article.PublishDate = DateTime.Now;
                }

                if (String.IsNullOrEmpty(article.Author))
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn chưa nhập tên tác giả";
                    return Json(ReturnData);
                }

                if (article.MenuID == 11)
                {
                    if (!String.IsNullOrEmpty(article.MetaData))
                    {
                        //
                        string temp = "watch?v=";
                        if (article.MetaData.Contains(temp))
                        {
                            Uri myUri = new Uri(article.MetaData);
                            article.MetaData = HttpUtility.ParseQueryString(myUri.Query).Get("v");
                        }
                        else
                        {
                            article.MetaData = article.MetaData.Substring(article.MetaData.LastIndexOf('/') + 1);
                        }
                    }
                }

                if (article.BottomDesc == null) article.BottomDesc = "";

                article.CreateUser = userValidate.UserName;

                var updateResult = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Article_INUP_CMS(article);
                ReturnData.ResponseCode = updateResult;
                if (updateResult > 0)
                {

                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = " Sử dụng chức năng Update Bài viết " + userValidate.UserName,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                    ReturnData.Description = "Cập Nhật Dữ Liệu Thành Công";
                    return Json(new { ResponseCode = updateResult, Description = "Cập Nhật Dữ Liệu Thành Công", MenuId = article.MenuID });
                }
                else
                {
                    if (updateResult == -55)
                    {
                        ReturnData.Description = "Đã có 5 bài hot";
                    }
                    if (updateResult == -600)
                    {
                        ReturnData.Description = "Đầu vào không hợp lệ ";
                    }
                    else
                    {
                        ReturnData.Description = "Hệ thống bận bạn vui lòng quay lại sau";
                    }
                    return Json(ReturnData);
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống bận bạn vui lòng quay lại sau";
                return Json(ReturnData);
            }

        }

        [HttpPost]
        public JsonResult ArtUpdateStatus(int ArtId, int Status)
        {
            var userValidate = new UserValidation();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }

                if (Permission == null || !Permission.IsInsert || !Permission.IsUpdate)
                {
                    return Json(new { ResponseCode = -101, Msg = "Bạn không có quyền sử dụng chức năng này" });
                }
                if (Status < 2 || Status > 4 || ArtId <= 0)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Dữ liệu đầu vào không hợp lệ" });
                }

                var updateResult = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Article_UpdateStatus_CMS(ArtId, Status, userValidate.UserName);
                if (updateResult > 0)
                {
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = " Sử dụng chức năng Update bài viết " + userValidate.UserName,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                    return Json(new { ResponseCode = updateResult, Msg = "Cập Nhật Dữ Liệu Thành Công" });

                }
                else if (updateResult == -600)
                {
                    return Json(new { ResponseCode = -600, Msg = "Đầu vào không hợp lệ" });
                }
                else
                {
                    return Json(new { ResponseCode = -99, Msg = "Cập nhật dữ liệu không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }

        }

        [HttpPost]
        public JsonResult ArtUpdateOrderHot(int ArtId, int type)
        {
            var userValidate = new UserValidation();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }

                if (Permission == null || !Permission.IsInsert || !Permission.IsUpdate)
                {
                    return Json(new { ResponseCode = -101, Msg = "Bạn không có quyền sử dụng chức năng này" });
                }
                if (type < 0 || type > 1 || ArtId <= 0)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Dữ liệu đầu vào không hợp lệ" });
                }

                var updateResult = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Article_UpdateHotOrder_CMS(ArtId, type, userValidate.UserName);
                if (updateResult > 0)
                {
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = " Sử dụng chức năng Update vị trí bài viết hot",
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                    return Json(new { ResponseCode = updateResult, Msg = "Cập Nhật Dữ Liệu Thành Công" });

                }
                else if (updateResult == -600)
                {
                    return Json(new { ResponseCode = -600, Msg = "Đầu vào không hợp lệ" });
                }
                else
                {
                    return Json(new { ResponseCode = -99, Msg = "Cập nhật dữ liệu không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }

        }
        #endregion

        #region Ảnh của Album
        //danh sach anh
        public ActionResult ImgManager(int? ArtId)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ImgManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            int ArticleId = ArtId == null ? 0 : (int)ArtId;
            if (ArticleId <= 0) return RedirectToAction("ArticleManager", "Manager");

            ViewBag.ArticleId = ArticleId;
            ViewBag.UrlLogin = urlLogin;

            return View();
        }

        public ActionResult ImgGetList(int ArtId, int? toprow, int? status, int? page)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ArticleManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            int TopRow = toprow == null ? 100 : (int)toprow;
            if (TopRow < 0) TopRow = 0;
            int Status = status == null ? -1 : (int)status;
            if (Status < -1 || Status > 1) Status = -1;
            int CurPage = page == null ? 1 : (int)page;

            var data = new List<ArticleImage>();

            int TotalRecord = 0;
            data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_ArticleImage_GetList(TopRow, 0, ArtId, Status, CurPage, 20, out TotalRecord);

            ViewBag.PageSize = 20;
            ViewBag.CurrentPage = CurPage;
            ViewBag.TotalRecord = TotalRecord;
            ViewBag.TopRow = TopRow;
            ViewBag.ArtId = ArtId;
            ViewBag.Status = Status;
            return PartialView(data);
        }

        //them moi anh vao album
        public ActionResult ImgAddNew(int ArtId)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            //var checkPermission = userValidate.CheckPermissionUser("ImgAddNew", userValidate.UserId, userValidate.IsAdministrator);
            //if (!checkPermission || Permission == null)
            //{
            //    return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            //}

            ViewBag.ArtId = ArtId;
            ViewBag.UrlLogin = urlLogin;

            return View();
        }

        [HttpPost]
        public JsonResult AlbumAddImage(int artId, string file_img)
        {
            var userValidate = new UserValidation();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }

                if (artId < 0)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Dữ liệu đầu vào không hợp lệ" });
                }

                if (string.IsNullOrEmpty(file_img))
                {
                    return Json(new { ResponseCode = -6001, Msg = "Đầu vào không hợp lệ" });
                }

                var updateResult = AbstractDAOFactory.Instance().CreateCmsDAO().SP_ArticleImage_Add_CMS(artId, file_img, userValidate.UserName);

                if (updateResult > 0)
                {
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = " Sử dụng chức năng thêm ảnh vào Album " + userValidate.UserName,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                    return Json(new { ResponseCode = updateResult, Msg = "Cập Nhật Dữ Liệu Thành Công" });

                }
                else if (updateResult == -50)
                {
                    return Json(new { ResponseCode = -50, Msg = "Album không tồn tại" });
                }
                else
                {
                    return Json(new { ResponseCode = -99, Msg = "Cập nhật dữ liệu không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }

        }

        //Cập nhật ảnh trong bài viết
        public ActionResult ImgUpdate(int ArtId, int ImgId)//
        {
            var data = new ArticleImage();
            try
            {
                int TotalRecord = 0;
                data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_ArticleImage_GetList(1, ImgId, ArtId, -1, 1, 1, out TotalRecord).FirstOrDefault();
            }
            catch (Exception e)
            {
                NLogLogger.LogInfo("Error: " + e.ToString());
            }
            return PartialView(data);
        }

        [HttpPost]
        public JsonResult ImageSaveUpdate(int ImgId, string file_img, int Status, string des)
        {
            var userValidate = new UserValidation();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }

                if (ImgId < 0)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Dữ liệu đầu vào không hợp lệ" });
                }

                if (string.IsNullOrEmpty(file_img) && Status == 1)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Bạn chưa chọn ảnh" });
                }

                if (Status != 0 && Status != 1)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Dữ liệu đầu vào không hợp lệ" });
                }

                var updateResult = AbstractDAOFactory.Instance().CreateCmsDAO().SP_ArticleImage_Update_CMS(ImgId, file_img, Status, userValidate.UserName, des);

                if (updateResult > 0)
                {
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = " Sử dụng chức năng sửa ảnh trong Album " + userValidate.UserName,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                    return Json(new { ResponseCode = updateResult, Msg = "Cập Nhật Dữ Liệu Thành Công" });

                }
                else
                {
                    return Json(new { ResponseCode = -99, Msg = "Cập nhật dữ liệu thất bại" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }

        }
        #endregion

        #region Chuong trinh
        public ActionResult EventManager()
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("EventManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            DateTime FromDate = DateTime.Now.AddMonths(-3);
            DateTime ToDate = DateTime.Now.AddDays(3);
            ViewBag.FromDate = FromDate.ToString("dd/MM/yyyy");
            ViewBag.ToDate = ToDate.ToString("dd/MM/yyyy");

            ViewBag.UrlLogin = urlLogin;

            return View();
        }

        public ActionResult EventGetList(string eventName, int? status, string fromdate, string todate, int? page)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("EventManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            if (String.IsNullOrEmpty(eventName)) eventName = "";

            int Status = status == null ? -1 : (int)status;

            DateTime FromDate = DateTime.Now.AddMonths(-3);
            DateTime ToDate = DateTime.Now.AddDays(3);
            try
            {
                FromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                FromDate = DateTime.Now.AddMonths(-3);
            }

            try
            {
                todate = todate + " 23:59:59";
                ToDate = DateTime.ParseExact(todate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                ToDate = DateTime.Now.AddDays(3);
            }

            int CurPage = page == null ? 1 : (int)page;

            var data = new List<Event>();
            int TotalRecord = 0;
            data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Event_GetList_CMS(0, eventName, -1, -1, Status, FromDate, ToDate, CurPage, 20, out TotalRecord);

            ViewBag.PageSize = 20;
            ViewBag.CurrentPage = CurPage;
            ViewBag.TotalRecord = TotalRecord;
            ViewBag.Status = Status;
            return PartialView(data);
        }

        public ActionResult EventInertUpdate(int? ID)
        {
            int EventId = ID == null ? 0 : (int)ID;
            if (EventId < 0)
            {
                return RedirectToAction("EventManager", "Manager");
            }

            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }

            var data = new Event();
            var listSMS = new List<Event>();
            int TotalRecord = 0, totalSMS = 0;
            if (EventId > 0)
            {
                data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Event_GetList_CMS(EventId, "", -1, -1, -1, DateTime.Now, DateTime.Now, 1, 1, out TotalRecord).FirstOrDefault();

                listSMS = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Event_GetDetail_CMS(EventId, 0, 1, 100, out totalSMS);
                ViewBag.listSMS = listSMS;

            }
            ViewBag.totalSMS = totalSMS;
            ViewBag.EventId = EventId;
            return View(data);
        }

        [HttpPost]
        public JsonResult EventInUpFunction(Event eventx)
        {
            var userValidate = new UserValidation();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }

                if (Permission == null || (eventx.EventID == 0 && !Permission.IsInsert) || eventx.EventID > 0 && !Permission.IsUpdate)
                {
                    return Json(new { ResponseCode = -101, Msg = "Bạn không có quyền sử dụng chức năng này" });
                }
                if (eventx.EventID < 0)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Dữ liệu đầu vào không hợp lệ" });
                }
                if (string.IsNullOrEmpty(eventx.EventName))
                {
                    return Json(new { ResponseCode = -6001, Msg = "Tên Chương trình không được để trống" });
                }
                if (string.IsNullOrEmpty(eventx.Porpose))
                {
                    return Json(new { ResponseCode = -6001, Msg = "Bạn chưa nhập mục đích triển khai chương trình" });
                }
                if (string.IsNullOrEmpty(eventx.Image))
                {
                    return Json(new { ResponseCode = -6001, Msg = "Bạn chưa chọn ảnh đại diện của chương trình" });
                }

                if (string.IsNullOrEmpty(eventx.HostUnit))
                {
                    return Json(new { ResponseCode = -6001, Msg = "Bạn chưa nhập tên Đơn vị chủ trì" });
                }

                if (string.IsNullOrEmpty(eventx.CoordUnit))
                {
                    return Json(new { ResponseCode = -6001, Msg = "Bạn chưa nhập tên Đơn vị phối hợp" });
                }

                if (string.IsNullOrEmpty(eventx.LegalFile))
                {
                    return Json(new { ResponseCode = -6001, Msg = "Bạn chưa chọn file văn bản pháp lý" });
                }

                if (eventx.StartDate >= eventx.EndDate)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc" });
                }

                eventx.CreateUser = userValidate.UserName;

                var updateResult = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Event_INUP_CMS(eventx);
                if (updateResult > 0)
                {
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = " Sử dụng chức năng Update Chương trình " + userValidate.UserName,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                    return Json(new { ResponseCode = updateResult, Msg = "Cập Nhật Dữ Liệu Thành Công" });

                }
                else if (updateResult == -600)
                {
                    return Json(new { ResponseCode = -600, Msg = "Đầu vào không hợp lệ" });
                }
                else
                {
                    return Json(new { ResponseCode = -99, Msg = "Cập nhật dữ liệu không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }

        }

        [HttpPost]
        public JsonResult EventRemove(Event eventx)
        {
            var userValidate = new UserValidation();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }

                if (Permission == null || (eventx.EventID == 0 && !Permission.IsInsert) || (eventx.EventID > 0 && !Permission.IsUpdate) || (eventx.EventID > 0 && !Permission.IsDelete))
                {
                    return Json(new { ResponseCode = -101, Msg = "Bạn không có quyền sử dụng chức năng này" });
                }
                if (eventx.EventID < 0)
                {
                    return Json(new { ResponseCode = -6001, Msg = "Dữ liệu đầu vào không hợp lệ" });
                }
                
                eventx.CreateUser = userValidate.UserName;
                eventx.Status = 0;

                var updateResult = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Event_Updatestatus_CMS(eventx);
                if (updateResult > 0)
                {
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = " Sử dụng chức năng Update Chương trình ",
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                    return Json(new { ResponseCode = updateResult, Msg = "Cập Nhật Dữ Liệu Thành Công" });

                }
                else if (updateResult == -600)
                {
                    return Json(new { ResponseCode = -600, Msg = "Đầu vào không hợp lệ" });
                }
                else
                {
                    return Json(new { ResponseCode = -99, Msg = "Cập nhật dữ liệu không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }

        }

        //Danh sách chương trình đang thực hiện
        public ActionResult EventRunning()
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("EventRunning", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            ViewBag.UrlLogin = urlLogin;

            return View();
        }
        public ActionResult EventListRunning(string eventName, int? page)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("EventRunning", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            if (String.IsNullOrEmpty(eventName)) eventName = "";

            
            int CurPage = page == null ? 1 : (int)page;

            int TotalRecord = 0;
            var data = AbstractDAOFactory.Instance().CreateProgramDAO().SP_Event_GetList_Web(0, eventName, -1, CurPage, 20, out TotalRecord);
            ViewBag.ListEvent = data;

            ViewBag.PageSize = 20;
            ViewBag.CurrentPage = CurPage;
            ViewBag.TotalRecord = TotalRecord;
            return PartialView();
        }

        //Danh sách chương trình đã thực hiện
        public ActionResult EventDone()
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("EventDone", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            ViewBag.UrlLogin = urlLogin;

            return View();
        }

        public ActionResult EventListDone(string eventName, int? year, int? page)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("EventDone", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            if (String.IsNullOrEmpty(eventName)) eventName = ""; 
            int CurPage = page == null ? 1 : (int)page;
            int YearEvent = year == null ? Convert.ToInt32(DateTime.Now.Year) : (int)year;

            int TotalRecord = 0;
            var data = AbstractDAOFactory.Instance().CreateProgramDAO().SP_Event_OldGetList_Web(0, eventName, -1, YearEvent, CurPage, 10, out TotalRecord);
            ViewBag.ListEvent = data;

            ViewBag.PageSize = 10;
            ViewBag.CurrentPage = CurPage;
            ViewBag.TotalRecord = TotalRecord;
            ViewBag.YearEvent = YearEvent;

            return PartialView();
        }

        //bao cao doanh thu
        public ActionResult ReportEventManager()
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ReportEventManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            DateTime FromDate = DateTime.Now.AddMonths(-3);
            DateTime ToDate = DateTime.Now.AddDays(3);
            ViewBag.FromDate = FromDate.ToString("dd/MM/yyyy");
            ViewBag.ToDate = ToDate.ToString("dd/MM/yyyy");

            var listEvent = new List<Event>();
            int TotalRecord = 0;
            listEvent = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Event_GetList_CMS(0, "", -1, -1, -1, DateTime.Now.AddYears(-3), DateTime.Now.AddDays(3), 1, 1000, out TotalRecord);

            ViewBag.listEvent = listEvent;
            ViewBag.UrlLogin = urlLogin;

            return View();
        }

        public ActionResult ReportEventGetList(int? eventId, string fromdate, string todate)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ReportEventManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            int EventId = eventId == null ? 0 : (int)eventId;

            DateTime FromDate = DateTime.Now.AddMonths(-3);
            DateTime ToDate = DateTime.Now.AddDays(3);
            try
            {
                FromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                FromDate = DateTime.Now.AddMonths(-3);
            }

            try
            {
                todate = todate + " 23:59:59";
                ToDate = DateTime.ParseExact(todate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                ToDate = DateTime.Now.AddDays(3);
            }

            var data = new List<EventReport>();
            int TotalRecord = 0;
            data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Event_GetReport_CMS(EventId, FromDate, ToDate, out TotalRecord);

            ViewBag.TotalRecord = TotalRecord;
            return PartialView(data);
        }
        #endregion

        #region gop y
        public ActionResult SuggestManager()
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("SuggestManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            DateTime FromDate = DateTime.Now.AddMonths(-3);
            DateTime ToDate = DateTime.Now.AddDays(3);
            ViewBag.FromDate = FromDate.ToString("dd/MM/yyyy");
            ViewBag.ToDate = ToDate.ToString("dd/MM/yyyy");

            ViewBag.UrlLogin = urlLogin;

            return View();
        }

        public ActionResult SuggestGetList(string mobile, string mail, int? status, string fromdate, string todate, int? page)
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("SuggestManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            if (String.IsNullOrEmpty(mobile)) mobile = "";
            if (String.IsNullOrEmpty(mail)) mail = "";

            int Status = status == null ? 0 : (int)status;

            DateTime FromDate = DateTime.Now.AddMonths(-3);
            DateTime ToDate = DateTime.Now.AddDays(3);
            try
            {
                FromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                FromDate = DateTime.Now.AddMonths(-3);
            }

            try
            {
                todate = todate + " 23:59:59";
                ToDate = DateTime.ParseExact(todate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                ToDate = DateTime.Now.AddDays(3);
            }

            int CurPage = page == null ? 1 : (int)page;

            var data = new List<Suggest>();
            int TotalRecord = 0;
            data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Suggestion_List_CMS(0, mobile, mail, FromDate, ToDate, Status, CurPage, 10, out TotalRecord);

            ViewBag.PageSize = 10;
            ViewBag.CurrentPage = CurPage;
            ViewBag.TotalRecord = TotalRecord;
            ViewBag.Status = Status;
            return PartialView(data);
        }

        public ActionResult SuggestiReply(int? ID)
        {
            int SugId = ID == null ? 0 : (int)ID;
            if (SugId <= 0)
            {
                return RedirectToAction("SuggestManager", "Manager");
            }

            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("SuggestManager", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            ViewBag.EmailSend = userValidate.UserName + "@vtc.vn";

            var data = new Suggest();
            int TotalRecord = 0;

            data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Suggestion_List_CMS(SugId, "", "", DateTime.Now, DateTime.Now, 0, 1, 1, out TotalRecord).FirstOrDefault();


            ViewBag.SugId = SugId;
            return View(data);
        }

        [HttpPost]
        public JsonResult SuggestSendMail(int suggetid, string from_email, string password, string recive_mail, string cc_email, string subject, string mailcontent)
        {
            try
            {

                var userValidate = new UserValidation();

                var urlLogin = Config.UrlRoot + "Account/FormLogin";
                if (!userValidate.IsSigned())
                {
                    return Json(new { Response = -9, msg = "Mời bạn đăng nhập lại", url = urlLogin });
                }

                if (suggetid <= 0)
                {
                    return Json(new { Response = -1, msg = "Đầu vào không hợp lệ" });
                }

                if (String.IsNullOrEmpty(from_email))
                {
                    return Json(new { Response = -1, msg = "Bạn chưa nhập Email người gửi" });
                }

                if (!from_email.Contains("@vtc.vn"))
                {
                    return Json(new { Response = -1, msg = "Email người gửi phải là vtc mail" });
                }

                if (String.IsNullOrEmpty(password))
                {
                    return Json(new { Response = -1, msg = "Bạn chưa nhập mật khẩu Email" });
                }

                if (String.IsNullOrEmpty(recive_mail))
                {
                    return Json(new { Response = -1, msg = "Bạn chưa nhập Email người nhận" });
                }

                Regex rx = new Regex(@"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                if (!rx.IsMatch(from_email.Trim()))
                {
                    return Json(new { Response = -1, msg = "Email người gửi không tồn tại" });
                }
                if (!rx.IsMatch(recive_mail.Trim()))
                {
                    return Json(new { Response = -1, msg = "Email người nhận không tồn tại" });
                }

                string[] EmailCC = cc_email.Split(';');
                //for (int i = 0; i < EmailCC.Length; i++)
                //{
                //    if (!rx.IsMatch(EmailCC[i].Trim()))
                //    {
                //        return Json(new { Response = -1, msg = "Email người đồng gửi không tồn tại" });
                //    }
                //}

                var data = new Suggest();
                int TotalRecord = 0; 
                data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Suggestion_List_CMS(suggetid, "", "", DateTime.Now, DateTime.Now, 0, 1, 1, out TotalRecord).FirstOrDefault();

                if (data.Status != 1)
                {
                    return Json(new { Response = -1, msg = "Thư này đã được trả lời hoặc không được trả lời" });
                }

                //if (String.IsNullOrEmpty(subject))
                //{
                //    return Json(new { Response = -1, msg = "Bạn chưa nhập tiêu đề Email" });
                //}
                string[] name_email = from_email.Split('@');
                CMS.Controllers.Common.POP3Auth pop = new CMS.Controllers.Common.POP3Auth();
                int nRet = pop.CheckAuth(name_email[0].ToString(), password) == "OK" ? 1 : -1;
                if (nRet == -1)
                {
                    return Json(new { Response = 0, msg = "Địa chỉ email hoặc password của bạn không đúng" });
                }

                NetworkCredential loginInfo = new NetworkCredential();
                loginInfo.UserName = from_email;
                loginInfo.Password = password;
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(from_email.Trim());
                msg.Subject = subject == null ? "" : subject;
                msg.To.Add(recive_mail.Trim());

                for (int i = 0; i < EmailCC.Length; i++)
                {
                    if (!String.IsNullOrEmpty(EmailCC[i].Trim()))
                    {
                        msg.CC.Add(EmailCC[i].Trim());
                    } 
                }
                
                msg.Body = mailcontent;

                //attach file
                //System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(file_path);
                //msg.Attachments.Add(attachment);

                msg.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.mail.vtc.vn", 465);
                //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = loginInfo;
                try
                {
                    client.Send(msg);
                }
                catch
                {
                    return Json(new { Response = -2, msg = "Email hoặc mật khẩu không đúng" });
                }

                var result = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Suggestion_Answer_CMS(suggetid, mailcontent, userValidate.UserName, 2);

                NLogLogger.LogInfo("Send Mail OK: " + recive_mail.ToString());

                return Json(new { Response = 1, msg = "Thành công" });
            }
            catch (Exception e)
            {
                NLogLogger.LogInfo("Send To Email " + recive_mail + " Error: " + e.ToString());
                return Json(new { Response = -99, msg = "Hệ thống bận" });
            }

        }

        public ActionResult SuggestViewAnswer(int? ID)
        {
            int SugId = ID == null ? 0 : (int)ID;
            if (SugId <= 0)
            {
                return RedirectToAction("SuggestManager", "Manager");
            }

            var data = new Suggest();
            int TotalRecord = 0;

            data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Suggestion_List_CMS(SugId, "", "", DateTime.Now, DateTime.Now, 0, 1, 1, out TotalRecord).FirstOrDefault();

            ViewBag.AnswerContent = data.Answer;

            return PartialView();
        }

        public JsonResult SuggestNotAns(int suggetid)
        {
            try
            {

                var userValidate = new UserValidation();

                var urlLogin = Config.UrlRoot + "Account/FormLogin";
                if (!userValidate.IsSigned())
                {
                    return Json(new { Response = -9, msg = "Mời bạn đăng nhập lại", url = urlLogin });
                }

                if (suggetid <= 0)
                {
                    return Json(new { Response = -1, msg = "Đầu vào không hợp lệ" });
                }

                var data = new Suggest();
                int TotalRecord = 0;
                data = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Suggestion_List_CMS(suggetid, "", "", DateTime.Now, DateTime.Now, 0, 1, 1, out TotalRecord).FirstOrDefault();

                if (data.Status == 2)
                {
                    return Json(new { Response = -1, msg = "Thư này đã được trả lời" });
                }


                var result = AbstractDAOFactory.Instance().CreateCmsDAO().SP_Suggestion_Answer_CMS(suggetid, null, userValidate.UserName, 3);
                if (result == 1)
                {
                    return Json(new { Response = 1, msg = "Thành công" });
                }
                else if(result == -600)
                {
                    return Json(new { Response = -600, msg = "Đầu vào không hợp lệ" });
                }
                return Json(new { Response = -99, msg = "Hệ thống bận" });
            }
            catch (Exception e)
            {
                NLogLogger.LogInfo("Error: " + e.ToString());
                return Json(new { Response = -99, msg = "Hệ thống bận" });
            }

        }
        #endregion

        #region bao cao chuong trinh
        #endregion
    }
}