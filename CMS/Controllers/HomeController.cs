﻿using CMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W1400.DataAccess.DTO;
using W1400.DataAccess.Factory;
using W1400.Utility;
using W1400.Utility.Security;

namespace CMS.Controllers
{
    public class HomeController : Controller
    {
        private UserFunction Permission { get { return ((UserFunction)Session[SessionsManager.SESSION_PERMISSION]); } }
        private CommonLib CommonLib = new CommonLib();

        public ActionResult Index()
        {
            return View();
        }

        #region Quản trị chức năng hệ thống
        public ActionResult ManagerFunction()
        {
            UserValidation userValidate = new UserValidation();
            var _ListFunction = new List<Functions>();
            var lst = new List<TreeFunction>();
            if (!userValidate.IsSigned())
            {
                var url = Config.UrlRoot + "Account/FormLogin";
                return new RedirectResult(url, true);
            }
            var checkPermission = userValidate.CheckPermissionUser("ManagerFunction", userValidate.UserId, userValidate.IsAdministrator);
            if (!userValidate.IsAdministrator || !checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }


            _ListFunction = AbstractDAOFactory.Instance().CreateFunctionDAO().SelectAllFunctionID(-1, string.Empty, -1, -1);

            var leve0 = _ListFunction.FindAll(c => c.ParentID == 0);
            foreach (var t in leve0)
            {
                var data = new TreeFunction
                {
                    FuntionId = t.FunctionID,
                    ParentId = t.ParentID,
                    text = t.FunctionName,
                    icon = t.CssIcon,
                    IsBtnGrant = false
                };
                if (!string.IsNullOrEmpty(t.Url))
                {
                    data.IsBtnGrant = true;
                }
                lst.Add(data);
                var chils = CommonLib.GetListTreeFunction(t.FunctionID, _ListFunction);
                data.nodes = chils;
            }

            ViewBag.ListFunctionJson = JsonConvert.SerializeObject(lst);
            ViewBag.IsAdmin = userValidate.IsAdministrator;
            return View();
        }

        public ActionResult GetFunctionInfo(int? id)
        {
            var result = new ModelFunctionDetail
            {
                FunctionDetail = new Functions(),
                ListFunction = new List<Functions>()
            };
            var userValidate = new UserValidation();
            if (!userValidate.IsSigned())
            {
                var url = Config.UrlRoot + "Account/FormLogin";
                return new RedirectResult(url, true);
            }
            if (!userValidate.IsAdministrator || Permission == null || Permission.ActionName != "ManagerFunction" || (id == 0 && !Permission.IsInsert) || (id > 0 && !Permission.IsUpdate))
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            else
            {
                int Id = id == null ? 0 : (int)id;
                result.ListFunction = AbstractDAOFactory.Instance().CreateFunctionDAO().SelectAllFunctionID(-1, string.Empty, 1, -1);
                if (Id > 0)
                {
                    result.FunctionDetail = AbstractDAOFactory.Instance().CreateFunctionDAO().GetFunctionByFunctionID(Id);
                    if (result.FunctionDetail == null)
                        result.FunctionDetail = new Functions();
                }
            }
            ViewBag.IsAdmin = userValidate.IsAdministrator;
            return PartialView(result);
        }

        public ActionResult FunctionOrder()
        {
            var data = new List<Functions>();
            var userValidate = new UserValidation();
            if (!userValidate.IsSigned())
            {
                var url = Config.UrlRoot + "Account/FormLogin";
                return new RedirectResult(url, true);
            }
            if (!userValidate.IsAdministrator || Permission == null || Permission.ActionName != "ManagerFunction" || !Permission.IsUpdate)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            else
            {
                data = AbstractDAOFactory.Instance().CreateFunctionDAO().SelectAllFunctionID(-1, string.Empty, 1, -1);
            }
            ViewBag.IsAdmin = userValidate.IsAdministrator;
            return View(data);
        }

        public static string GetChildFunction(int FatherID, List<Functions> ListFunction)
        {
            var function = ListFunction.Find(f => f.FunctionID == FatherID);
            var listChirl = ListFunction.FindAll(f => f.ParentID == FatherID);
            listChirl.Sort((f1, f2) => f1.Order.CompareTo(f2.Order));

            var script = "<li class=\"dd-item\" data-id=\"" + FatherID + "\"><div class=\"dd-handle\"><i class=\"" + function.CssIcon + "\" style=\"margin-right:7px\"></i>" + function.FunctionName + "</div>";

            if (listChirl.Count <= 0)
            {
                script += "</li>";
                return script;
            }
            script += "<ol class=\"dd-list\">";
            foreach (var t in listChirl)
            {
                script += GetChildFunction(t.FunctionID, ListFunction);
            }
            script += "</ol>";
            script += "</li>";
            return script;
        }

        [HttpPost]
        public JsonResult SaveDataFunction(Functions function)
        {
            var ReturnData = new ReturnData();
            var userValidate = new UserValidation();
            if (!userValidate.IsSigned())
            {
                var url = Config.UrlRoot + "Account/FormLogin";
                var urlLogout = string.Format("{0}?act=out", url);
                Response.Redirect(urlLogout, true);
            }
            if (!userValidate.IsAdministrator || Permission == null || Permission.ActionName != "ManagerFunction" || (function.FunctionID == 0 && !Permission.IsInsert) || (function.FunctionID > 0 && !Permission.IsUpdate))
            {
                ReturnData.ResponseCode = -101;
                ReturnData.Description = "Bạn không có quyền sử dụng chức năng này";
                return Json(ReturnData);
            }
            if (string.IsNullOrEmpty(function.FunctionName))
            {
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Bạn chưa nhập tên chức năng";
                return Json(ReturnData);
            }
            function.Url = string.IsNullOrEmpty(function.Url) ? string.Empty : function.Url;
            try
            {
                var result = AbstractDAOFactory.Instance().CreateFunctionDAO().InsertUpdateFunction(function);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {
                    string Description = "";
                    if (function.FunctionID > 0)
                    {
                        ReturnData.Description = "Tài khoản : " + userValidate.UserName + " Cập nhật";
                        Description = "cập nhật chức năng :" + function.FunctionName + " (Hệ Thống)";
                    }
                    else
                    {
                        ReturnData.Description = "Tài khoản : " + userValidate.UserName + " Thêm mới";
                        Description = "thêm mới chức năng :" + function.FunctionName + " (Hệ Thống)";
                    }
                    //Ghi log

                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = Description,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                }
                else switch (result)
                    {
                        case -70: ReturnData.Description = "Tên chức năng đã tồn tại"; break;
                        case -71: ReturnData.Description = "Đường dẫn đã tồn tại"; break;
                        case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }

        [HttpPost]
        public JsonResult SaveOrderFunction(List<NestedOrder> listOrder)
        {
            var ReturnData = new ReturnData();
            var userValidate = new UserValidation();
            if (!userValidate.IsSigned())
            {
                var url = Config.UrlRoot + "Account/FormLogin";
                var urlLogout = string.Format("{0}?act=out", url);
                Response.Redirect(urlLogout, true);
            }
            if (!userValidate.IsAdministrator || Permission == null || Permission.ActionName != "ManagerFunction" || !Permission.IsUpdate)
            {
                ReturnData.ResponseCode = -101;
                ReturnData.Description = "Bạn không có quyền sử dụng chức năng này";
                return Json(ReturnData);
            }
            if (listOrder.Count <= 0)
            {
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "không tồn tại danh sách chức năng sắp xếp mới";
                return Json(ReturnData);
            }

            try
            {
                foreach (var t in listOrder)
                {
                    AbstractDAOFactory.Instance().CreateFunctionDAO().UpdateOrder(t.Id, t.FatherID, t.Order);
                }

                ReturnData.ResponseCode = 1;
                ReturnData.Description = "Sắp xếp Thành Công Chức Năng";
                //Ghi log

                AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                {
                    FunctionID = Permission.FunctionID,
                    FunctionName = Permission.FunctionName,
                    Description = "Sắp xếp chức năng (hệ thống)",
                    UserID = userValidate.UserId,
                    ClientIP = userValidate.ClientIP
                });
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
            }
            return Json(ReturnData);
        }

        [HttpPost]
        public JsonResult DeleteFunction(int id)
        {
            var ReturnData = new ReturnData();
            try
            {
                var userValidate = new UserValidation();
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }
                if (!userValidate.IsAdministrator || Permission == null || Permission.ActionName != "ManagerFunction" || !Permission.IsDelete)
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn không có quyền sử dụng chức năng này";
                    return Json(ReturnData);
                }
                if (id > 0)
                {
                    var Function = AbstractDAOFactory.Instance().CreateFunctionDAO().GetFunctionByFunctionID(id);
                    if (Function == null)
                    {
                        ReturnData.ResponseCode = -102;
                        ReturnData.Description = "chức năng xóa không tồn tại !";
                        return Json(ReturnData);
                    }
                    var result = AbstractDAOFactory.Instance().CreateFunctionDAO().DelleteFunction(id);
                    ReturnData.ResponseCode = result;
                    if (result >= 0)
                    {
                        ReturnData.Description = "Xóa chức năng Thành Công";
                        //Ghi log
                        AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                        {
                            FunctionID = Permission.FunctionID,
                            FunctionName = Permission.FunctionName,
                            Description = "Xóa chức năng " + Function.FunctionName + " (Hệ thống)",
                            UserID = userValidate.UserId,
                            ClientIP = userValidate.ClientIP
                        });
                    }
                    else switch (result)
                        {
                            case -24: ReturnData.Description = "Chức năng đang trong trạng thái hoạt động. Hãy tắt trạng thái hoạt động của hệ thống trước khi xóa !"; break;
                            case -25: ReturnData.Description = "Chức năng không tồn tại trong hệ thống"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định chức năng cần xóa";
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }
        #endregion

        #region Quản trị người dùng
        public ActionResult ManagerUser()
        {
            var userValidate = new UserValidation();

            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ManagerUser", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            ViewBag.IsAdmin = userValidate.IsAdministrator;
            ViewBag.UrlLogin = urlLogin;

            //r,b,c !=0
            string r = "";
            string e = "";
            string d = "";
            string b = "";
            string l = "";
            string u = "";
            string c = "";
            string o = "";
            string A = "";
            string B = "";
            string C = "";
            string A1 = "";
            string A2 = "";
            int count = 0;
            for (int a1 = 1; a1 <= 9; a1++)
            {
                r = a1.ToString();
                for (int a2 = 0; a2 <= 9; a2++)
                {
                    if (a1 != a2)
                    {
                        e = a2.ToString();
                        for (int a3 = 0; a3 <= 9; a3++)
                        {
                            if (a1 != a2 && a2 != a3)
                            {
                                d = a3.ToString();
                                for (int a4 = 1; a4 <= 9; a4++)
                                {
                                    if (a1 != a2 && a2 != a3 && a3 != a4)
                                    {
                                        b = a4.ToString();
                                        for (int a5 = 0; a5 <= 9; a5++)
                                        {
                                            if (a1 != a2 && a2 != a3 && a3 != a4 && a4 != a5)
                                            {
                                                l = a5.ToString();
                                                for (int a6 = 0; a6 <= 9; a6++)
                                                {
                                                    if (a1 != a2 && a2 != a3 && a3 != a4 && a4 != a5 && a5 != a6)
                                                    {
                                                        u = a6.ToString();
                                                        for (int a7 = 1; a7 <= 9; a7++)
                                                        {
                                                            if (a1 != a2 && a2 != a3 && a3 != a4 && a4 != a5 && a5 != a6 && a6 != a7)
                                                            {
                                                                c = a7.ToString();
                                                                for (int a8 = 0; a8 <= 9; a8++)
                                                                {
                                                                    if (a1 != a2 && a2 != a3 && a3 != a4 && a4 != a5 && a5 != a6 && a6 != a7 && a8 != a7)
                                                                    {
                                                                        o = a8.ToString();
                                                                        A = r + e + d;
                                                                        B = b + l + u + e;
                                                                        C = c + o + l + o + r;
                                                                        if(Convert.ToInt32(A) + Convert.ToInt32(B) == Convert.ToInt32(C))
                                                                        {
                                                                             A1 += r + "," + e + "," + d + "," + b + "," + l + "," + u + "," + e + "," + c + "," + o + "," + l + "," + r + "==========";
                                                                             A2 += A + "+" + B + "=" + C + "==========";
                                                                             count++;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            ViewBag.A1 = A1;
            ViewBag.A2 = A2;
            ViewBag.Count = count;
            return View(Permission);
        }

        public ActionResult ListUsers(int? isActive)
        {
            var email = String.Empty;
            var data = new List<Users>();
            var userValidate = new UserValidation();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            ViewBag.IsAdmin = userValidate.IsAdministrator;
            var checkPermission = userValidate.CheckPermissionUser("ManagerUser", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            string Email = string.IsNullOrEmpty(email) ? string.Empty : email;
            int IsActive = isActive == null ? -1 : (int)isActive;
            int IsGrant = -1;
            if (!userValidate.IsAdministrator)
            {
                IsGrant = 1;
            }

            data = AbstractDAOFactory.Instance().CreateUsersDAO().GetListUsers(Email, IsActive, IsGrant);
            if (!userValidate.IsAdministrator)
            {
                var m_user = AbstractDAOFactory.Instance().CreateUsersDAO().SelectByUserID(userValidate.UserId);
            }
            ViewBag.UserId = userValidate.UserId;
            ViewBag.UrlLogin = urlLogin;

            return PartialView(data);
        }

        public ActionResult GetUserInfo(int? id)
        {
            var userValidate = new UserValidation();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            int UserId = id == null ? 0 : (int)id;
            var User = new Users();
            var checkPermission = userValidate.CheckPermissionUser("ManagerUser", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            else if (UserId > 0)
            {
                User = AbstractDAOFactory.Instance().CreateUsersDAO().SelectByUserID(UserId);
            }
            ViewBag.IsAdmin = new UserValidation().IsAdministrator;
            ViewBag.UrlLogin = urlLogin;
            return PartialView(User);
        }

        public ActionResult GetGrantUser(int id)
        {
            var userValidate = new UserValidation();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }

            var checkPermission = userValidate.CheckPermissionUser("GetGrantUser", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null || !Permission.IsGrant)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            var listFunction = new List<FunctionsGrant>();
            var _ListFunction = new List<Functions>();
            var listtree = new List<TreeDataFunction>();
            var userGrant = AbstractDAOFactory.Instance().CreateUsersDAO().SelectByUserID(id);
            ViewBag.User = userGrant.Username + " (" + userGrant.FullName + ")";
            ViewBag.IsAdmin = userValidate.IsAdministrator ? 1 : 0;
            ViewBag.UserId = id;
            int FunctionSystem = (int)Enums.FunctionId.System;
            //Lấy quyền hiện tại của người cần cấp
            var userfunction = AbstractDAOFactory.Instance().CreateUserRoleDAO().UserFunction_GetByUserID(id);
            if (userValidate.IsAdministrator)
            {
                _ListFunction = AbstractDAOFactory.Instance().CreateFunctionDAO().SelectAllFunctionID(-1, string.Empty, -1, -1);
                foreach (var f in _ListFunction)
                {
                    listFunction.Add(new FunctionsGrant
                    {
                        FunctionID = f.FunctionID,
                        ParentID = f.ParentID,
                        FunctionName = f.FunctionName,
                        FatherName = f.FatherName,
                        IsReport = f.IsReport,
                        CssIcon = f.CssIcon
                    });
                }
            }
            //Là user mod -> lọc tất cả các chức năng báo cáo của mod 
            else if (Permission.IsGrant)
            {
                var checkMod = userfunction.FindAll(uf => uf.IsGrant == true);
                if (checkMod.Count > 0)
                {
                    return View(listtree);
                }
                var userMod = AbstractDAOFactory.Instance().CreateUsersDAO().SelectByUserID(userValidate.UserId);

                //Lấy danh sách chức năng mà user có quyền mod
                var lst = AbstractDAOFactory.Instance().CreateFunctionDAO().GetListFunctionByUserID(userValidate.UserId, 1);
                foreach (var f in lst)
                {
                    if (f.FunctionID != FunctionSystem && f.ParentID != FunctionSystem)
                        listFunction.Add(new FunctionsGrant
                        {
                            FunctionID = f.FunctionID,
                            ParentID = f.ParentID,
                            FunctionName = f.FunctionName,
                            FatherName = f.FatherName,
                            IsReport = f.IsReport,
                            CssIcon = f.CssIcon
                        });
                }
            }

            if (listFunction != null && listFunction.Count > 0)
            {
                var level0 = new List<FunctionsGrant>();
                level0 = listFunction.FindAll(c => c.ParentID == 0);
                foreach (var t in level0)
                {
                    var data = new TreeDataFunction
                    {
                        FunctionID = t.FunctionID,
                        FunctionName = t.FunctionName,
                        CssIcon = t.CssIcon,
                        ParentID = t.ParentID,
                        IsReport = t.IsReport,
                        IsView = false
                    };
                    if (userfunction != null && userfunction.Count > 0)
                    {
                        var fct = userfunction.Find(uf => uf.FunctionID == t.FunctionID);
                        if (fct != null)
                        {
                            data.IsView = true;
                            data.IsInsert = fct.IsInsert;
                            data.IsUpdate = fct.IsUpdate;
                            data.IsDelete = fct.IsDelete;
                            data.IsGrant = fct.IsGrant;
                        }
                    }
                    listtree.Add(data);
                    var chils = this.GetListTreeFunction(t.FunctionID, listFunction, userfunction);
                    data.Childrent = chils;
                }
            }

            return View(listtree);
        }

        public ActionResult ListUserFunction(int FunctionID)
        {
            var userValidate = new UserValidation();
            var ListUserFunction = new List<UserFunction>();
            var ListUserRole = new List<UserFunction>();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
                return new RedirectResult(urlLogin);

            if (Permission == null)
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);

            var m_function = AbstractDAOFactory.Instance()
                .CreateFunctionDAO().GetFunctionByFunctionID(FunctionID);
            if (m_function == null || m_function.FunctionID <= 0)
                return PartialView(ListUserFunction);
            var m_permission = new UserFunction();

            //Nếu user là admin -> full quyền
            if (userValidate.IsAdministrator)
            {
                m_permission.FunctionID = FunctionID;
                m_permission.UserID = userValidate.UserId;
                m_permission.IsGrant = true;
                m_permission.IsInsert = true;
                m_permission.IsUpdate = true;
                m_permission.IsDelete = true;
            }
            else
            {
                //Lấy quyền của user khi truy cập chức năng
                var roleFuncID = AbstractDAOFactory.Instance().CreateUserRoleDAO().
                    CheckPermission(userValidate.UserId, m_function.ActionName);
                if (roleFuncID == null || roleFuncID.FunctionID == 0)
                    return PartialView(ListUserFunction);

                //Kiểm tra có quyền truy cập chức năng phân quyền người dùng ko ?
                m_permission = AbstractDAOFactory.Instance().CreateUserRoleDAO().
                    CheckPermission(userValidate.UserId, "GetGrantUser");
                if (m_permission == null || m_permission.FunctionID == 0)
                    return PartialView(ListUserFunction);
            }
            int IsGrant = 2; //Lấy toàn bộ User có quyền
            //Lấy tất cả User đang hoạt động -> chỉ phân quyền cho User đang hoạt động
            var ListAllUser = AbstractDAOFactory.Instance().CreateUsersDAO().GetAllUserByStatus(1);
            //Lọc những tk không phải là admin
            var ListUserFilter = ListAllUser.FindAll(us => us.IsAdministrator == false);
            //Lấy tất cả User có quyền tương ứng vs chức năng hiện tại
            ListUserRole = AbstractDAOFactory.Instance().CreateUserRoleDAO().GetListUser_ByRoleFunctionID(FunctionID, IsGrant);

            if (userValidate.IsAdministrator)
            {
                foreach (var us in ListUserFilter)
                {
                    var m_userfunction = new UserFunction
                    {
                        UserID = us.UserID,
                        UserName = us.Username,
                        FunctionID = FunctionID
                    };
                    //Tìm user có quyền
                    var findUser = ListUserRole.Find(ur => ur.UserID == us.UserID);
                    if (findUser != null && findUser.UserID > 0)
                    {
                        m_userfunction.IsRead = true; //xác định có quyền
                        m_userfunction.IsGrant = findUser.IsGrant;
                        m_userfunction.IsInsert = findUser.IsInsert;
                        m_userfunction.IsUpdate = findUser.IsUpdate;
                        m_userfunction.IsDelete = findUser.IsDelete;
                    }
                    ListUserFunction.Add(m_userfunction);
                }
            }
            else if (m_permission.IsGrant && !userValidate.IsAdministrator)
            {
                foreach (var us in ListUserFilter)
                {
                    var m_userfunction = new UserFunction
                    {
                        UserID = us.UserID,
                        UserName = us.Username,
                        FunctionID = FunctionID
                    };
                    //Tìm user có quyền
                    var findUser = ListUserRole.Find(ur => ur.UserID == us.UserID);
                    if (findUser != null && findUser.UserID > 0)
                    {
                        if (!findUser.IsGrant)
                        {
                            m_userfunction.IsRead = true; //xác định có quyền
                            m_userfunction.IsGrant = findUser.IsGrant;
                            m_userfunction.IsInsert = findUser.IsInsert;
                            m_userfunction.IsUpdate = findUser.IsUpdate;
                            m_userfunction.IsDelete = findUser.IsDelete;
                        }
                        else
                            continue;
                    }
                    ListUserFunction.Add(m_userfunction);
                }
            }

            ViewBag.FunctionID = FunctionID;
            ViewBag.permission = m_permission;
            ViewBag.FunctionName = m_function.FunctionName;
            ViewBag.IsAdmin = userValidate.IsAdministrator;
            return PartialView(ListUserFunction);
        }

        [HttpPost]
        public JsonResult SaveGrantForListUser(List<UserFunction> listUserFunction)
        {
            var ReturnData = new ReturnData();
            var userValidate = new UserValidation();
            var m_permission = new UserFunction();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }
                //Nếu user ko là admin 
                if (!userValidate.IsAdministrator)
                {
                    //Kiểm tra có quyền truy cập chức năng phân quyền người dùng ko ?
                    m_permission = AbstractDAOFactory.Instance().CreateUserRoleDAO().
                         CheckPermission(userValidate.UserId, "GetGrantUser");
                    if (m_permission == null || m_permission.FunctionID == 0 || !Permission.IsInsert || !Permission.IsUpdate || !Permission.IsDelete)
                    {
                        ReturnData.ResponseCode = -101;
                        ReturnData.Description = "Bạn không có quyền sử dụng chức năng này";
                        return Json(ReturnData);
                    }
                }

                if (listUserFunction == null || listUserFunction.Count == 0)
                {
                    ReturnData.ResponseCode = -7001;
                    ReturnData.Description = "Bạn chưa chọn quyền cho user";
                    return Json(ReturnData);
                }

                var m_function = AbstractDAOFactory.Instance().CreateFunctionDAO().GetFunctionByFunctionID(listUserFunction[0].FunctionID);
                if (m_function == null || m_function.FunctionID <= 0)
                {
                    ReturnData.ResponseCode = -7002;
                    ReturnData.Description = "Chức năng phân quyền ko tồn tại!";
                    return Json(ReturnData);
                }
                string ListRole = string.Empty;

                foreach (var userfunction in listUserFunction)
                {
                    if (userfunction.IsRead)
                        ListRole += userfunction.UserID + "," + userfunction.IsInsert + "," + userfunction.IsUpdate + "," + userfunction.IsDelete + ";";
                    else
                    {
                        AbstractDAOFactory.Instance().CreateUserRoleDAO().UserFunctionDelete(userfunction.UserID, userfunction.FunctionID);
                    }
                }
                if (string.IsNullOrEmpty(ListRole))
                {
                    ReturnData.ResponseCode = 1;
                    ReturnData.Description = "Phân quyền thành công";
                    return Json(ReturnData);
                }

                if (!string.IsNullOrEmpty(ListRole))
                {
                    ListRole = ListRole.Substring(0, ListRole.Length - 1);
                }
                var Result = AbstractDAOFactory.Instance().CreateUserRoleDAO().UserFunctionInsertListByFunctionID(listUserFunction[0].FunctionID, ListRole, userValidate.UserId);
                if (Result >= 0)
                {
                    ReturnData.Description = "Phân quyền Thành Công";
                    //Ghi log
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = "phân quyền chức năng " + m_function.FunctionName + " với danh sách TK (" + ListRole + ")",
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                }
                else if (Result == -101)
                    ReturnData.Description = "Chức năng không tồn tại";
                else if (Result == -600)
                    ReturnData.Description = "Tham số truyền vào không hợp lệ";
                else
                    ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau !";

                ReturnData.ResponseCode = Result;

                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }

        public List<TreeDataFunction> GetListTreeFunction(int parentId, List<FunctionsGrant> roots, List<UserFunction> lt1)
        {
            var tmp = new List<TreeDataFunction>();
            var levesub = roots.FindAll(c => c.ParentID == parentId);
            if (levesub.Count <= 0) return tmp;
            foreach (var t in levesub)
            {
                var data = new TreeDataFunction
                {
                    FunctionID = t.FunctionID,
                    ParentID = t.ParentID,
                    FunctionName = t.FunctionName,
                    CssIcon = t.CssIcon,
                    IsReport = t.IsReport,
                    IsView = false
                };
                if (lt1 != null && lt1.Count > 0)
                {
                    var fct = lt1.Find(uf => uf.FunctionID == t.FunctionID);
                    if (fct != null)
                    {
                        data.IsView = true;
                        data.IsInsert = fct.IsInsert;
                        data.IsUpdate = fct.IsUpdate;
                        data.IsDelete = fct.IsDelete;
                        data.IsGrant = fct.IsGrant;
                    }
                }
                tmp.Add(data);
                var childrens = this.GetListTreeFunction(t.FunctionID, roots, lt1);
                data.Childrent = childrens;
            }

            return tmp;
        }

        public static string GetChildUserFunctionGrant(List<TreeDataFunction> ListChildFunction)
        {
            var m_user = new UserValidation();
            var script = string.Empty;
            if (ListChildFunction != null && ListChildFunction.Count > 0)
            {
                foreach (var usergrant in ListChildFunction)
                {
                    if (usergrant.ParentID > 0)
                    {

                        if (usergrant.Childrent.Count == 0)
                        {
                            if (m_user.IsAdministrator || !usergrant.IsGrant)
                            {
                                script += "<tr class=\"treegrid-" + usergrant.FunctionID + " treegrid-parent-" + usergrant.ParentID + "\"><td><i class=\"" + usergrant.CssIcon + "\"></i>" +
                                        usergrant.FunctionName + "</td>";

                                if (m_user.IsAdministrator)
                                {
                                    script += "<td valign=\"middle\" align=\"center\"><input class=\"IsGrant CheckGrant\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsGrant == true) ? "checked" : "") + " /></td>";
                                }
                                script += "<td valign=\"middle\" align=\"center\"><input class=\"IsView CheckView\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsView == true) ? "checked" : "") + " /></td>" +
                                "<td valign=\"middle\" align=\"center\"><input class=\"IsInsert CheckInsert\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsInsert == true) ? "checked" : "") + " /></td>" +
                                "<td valign=\"middle\" align=\"center\"><input class=\"IsUpdate CheckUpdate\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsUpdate == true) ? "checked" : "") + " /></td>" +
                                "<td valign=\"middle\" align=\"center\"><input class=\"IsDelete CheckDelete\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsDelete == true) ? "checked" : "") + " /></td>";
                                script += "</tr>";
                            }

                        }
                        else
                        {
                            script += "<tr class=\"treegrid-" + usergrant.FunctionID + " treegrid-parent-" + usergrant.ParentID + "\"><td><i class=\"" + usergrant.CssIcon + "\"></i>" +
                                        usergrant.FunctionName + "</td>";
                            if (m_user.IsAdministrator)
                            {
                                script += "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllGrant CheckGrant\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsGrant == true) ? "checked" : "") + " /></td>";
                            }
                            script += "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllView CheckView\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsView == true) ? "checked" : "") + " /></td>" +
                            "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllInsert CheckInsert\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsInsert == true) ? "checked" : "") + " /></td>" +
                            "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllUpdate CheckUpdate\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsUpdate == true) ? "checked" : "") + "/></td>" +
                            "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllDelete CheckDelete\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsDelete == true) ? "checked" : "") + "/></td>";
                            script += "</tr>";

                        }

                    }
                    else
                    {
                        script += "<tr class=\"treegrid-" + usergrant.FunctionID + "\"><td><i class=\"" + usergrant.CssIcon + "\"></i>" +
                        usergrant.FunctionName + "</td>";

                        if (m_user.IsAdministrator)
                        {
                            script += "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllGrant CheckGrant\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsGrant == true) ? "checked" : "") + "/></td>";
                        }
                        script += "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllView CheckView\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsView == true) ? "checked" : "") + "/></td>" +
                        "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllInsert CheckInsert\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsInsert == true) ? "checked" : "") + "/></td>" +
                        "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllUpdate CheckUpdate\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsUpdate == true) ? "checked" : "") + "/></td>" +
                        "<td valign=\"middle\" align=\"center\"><input class=\"CheckAllDelete CheckDelete\" id=\"" + usergrant.FunctionID + "\" type=checkbox " + ((usergrant.IsDelete == true) ? "checked" : "") + "/></td>";
                    }

                    if (usergrant.Childrent.Count > 0)
                        script += GetChildUserFunctionGrant(usergrant.Childrent);
                }
                return script;
            }
            else
                return script;
        }

        public ActionResult ChangePassword()
        {
            var userValidate = new UserValidation();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }

            return PartialView();
        }

        [HttpPost]
        public JsonResult SaveDataUser(Users users)
        {
            var ReturnData = new ReturnData();
            var userValidate = new UserValidation();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }
                if (Permission == null || !userValidate.IsAdministrator || (users.UserID == 0 && !Permission.IsInsert) || users.UserID > 0 && !Permission.IsUpdate)
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn không có quyền sử dụng chức năng này";
                    return Json(ReturnData);
                }
                if (string.IsNullOrEmpty(users.Username))
                {
                    ReturnData.ResponseCode = -6001;
                    ReturnData.Description = "Bạn chưa nhập tên tài khoản";
                    return Json(ReturnData);
                }
                if (string.IsNullOrEmpty(users.Email) || !users.Email.Contains("@"))
                {
                    ReturnData.ResponseCode = -6002;
                    ReturnData.Description = "Email chưa nhập hoặc sai định dạng";
                    return Json(ReturnData);
                }
                if (userValidate.UserId == users.UserID)
                {
                    if (userValidate.UserName != users.Username)
                    {
                        ReturnData.ResponseCode = -6002;
                        ReturnData.Description = "Bạn không được sửa tên tài khoản của chính mình";
                        return Json(ReturnData);
                    }

                    if (userValidate.IsAdministrator != users.IsAdministrator)
                    {
                        ReturnData.ResponseCode = -6002;
                        ReturnData.Description = "Bạn không được sửa admin của chính mình";
                        return Json(ReturnData);
                    }

                    if (users.Status == false)
                    {
                        ReturnData.ResponseCode = -6002;
                        ReturnData.Description = "Bạn không được sửa trạng thái của chính mình";
                        return Json(ReturnData);
                    }
                }

                users.FullName = string.IsNullOrEmpty(users.FullName) ? string.Empty : users.FullName;
                users.Password = string.IsNullOrEmpty(users.Password) ? string.Empty : Encrypt.Md5(users.Password);
                users.CreatedUser = userValidate.UserName;
                var result = AbstractDAOFactory.Instance().CreateUsersDAO().UpdateUsers(users);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {
                    ReturnData.Description = users.UserID > 0 ? "Cập nhật Thành Công" : "Thêm mới Thành Công";
                    //Ghi log
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = ReturnData.Description + " tài khoản " + users.Username,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                }
                else switch (result)
                    {
                        case -51: ReturnData.Description = "Tài khoản đã tồn tại"; break;
                        case -52: ReturnData.Description = "Email đã tồn tại"; break;
                        case -600: ReturnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }
        [HttpPost]
        public JsonResult SaveGrantUser(List<AddGrantFunction> ListAddGrant, List<DeleteGrantFunction> ListDelGrant, int UserId)
        {
            var ReturnData = new ReturnData();
            var userValidate = new UserValidation();
            try
            {
                var Result = -99;
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }
                if (Permission == null)
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn không có quyền sử dụng chức năng này";
                    return Json(ReturnData);
                }

                var m_user = AbstractDAOFactory.Instance().CreateUsersDAO().SelectByUserID(UserId);
                if (m_user == null || m_user.UserID <= 0)
                {
                    ReturnData.ResponseCode = -102;
                    ReturnData.Description = "Tài khoản phân quyền không tồn tại";
                    return Json(ReturnData);
                }
                string ListRole = string.Empty;
                string ListDel = string.Empty;
                if (ListDelGrant != null && ListDelGrant.Count > 0)
                {
                    foreach (var Grant in ListDelGrant)
                    {
                        ListDel += Grant.FunctionId + ",";
                    }
                }
                if (ListAddGrant != null && ListAddGrant.Count > 0)
                {
                    foreach (var Grant in ListAddGrant)
                    {
                        ListRole += Grant.FunctionId + "," + Grant.IsGrant + "," + Grant.IsInsert + "," + Grant.IsUpdate + "," + Grant.IsDelete + ";";
                    }
                }

                //IF quyền được thêm không rỗng
                if (!string.IsNullOrEmpty(ListRole))
                {
                    if (!userValidate.IsAdministrator && Permission.IsGrant)
                    {
                        if (!string.IsNullOrEmpty(ListDel))
                        {
                            ListDel = ListDel.Substring(0, ListDel.Length - 1);
                            Result = AbstractDAOFactory.Instance().CreateUserRoleDAO().UserFunctionDeleteList(UserId, ListDel);
                        }
                    }
                    ListRole = ListRole.Substring(0, ListRole.Length - 1);
                    Result = AbstractDAOFactory.Instance().CreateUserRoleDAO().UserFunctionInsertList(UserId, ListRole, userValidate.UserId, userValidate.IsAdministrator);
                }
                else
                {
                    if (!userValidate.IsAdministrator && Permission.IsGrant)
                    {
                        if (!string.IsNullOrEmpty(ListDel))
                        {
                            ListDel = ListDel.Substring(0, ListDel.Length - 1);
                            Result = AbstractDAOFactory.Instance().CreateUserRoleDAO().UserFunctionDeleteList(UserId, ListDel);
                        }
                    }
                    else if (userValidate.IsAdministrator)
                    {
                        Result = AbstractDAOFactory.Instance().CreateUserRoleDAO().UserFunctionDeleteAll(UserId);
                    }
                }

                if (Result >= 0)
                {
                    var role = string.Empty;
                    if (!string.IsNullOrEmpty(ListRole))
                    {
                        ReturnData.Description = "Phân quyền Thành Công";
                        role = ListRole;
                    }
                    else
                    {
                        ReturnData.Description = "Xóa quyền Thành Công";
                        role = ListDel;
                    }

                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = Permission.FunctionID,
                        FunctionName = Permission.FunctionName,
                        Description = ReturnData.Description + " tài khoản : " + m_user.Username,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                }
                else switch (Result)
                    {
                        case -50:
                            ReturnData.Description = "Tài khoản không tồn tại";
                            break;
                        case -600:
                            ReturnData.Description = "Tham số truyền vào không hợp lệ";
                            break;
                        default:
                            ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                            break;
                    }
                ReturnData.ResponseCode = Result;

                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }

        [HttpPost]
        public JsonResult ChangePass(string PasswordOld, string PasswordNew)
        {
            var ReturnData = new ReturnData();
            var userValidate = new UserValidation();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);

                    ReturnData.ResponseCode = -7001;
                    ReturnData.Description = "Hãy đăng nhập hệ thống để sử dụng chức năng này";
                    return Json(ReturnData);
                }
                if (string.IsNullOrEmpty(PasswordOld) || string.IsNullOrEmpty(PasswordNew))
                {
                    ReturnData.ResponseCode = -7002;
                    ReturnData.Description = "Mật khẩu cũ và mật khẩu mới không được phép bỏ trống";
                    return Json(ReturnData);
                }
                if (string.CompareOrdinal(PasswordOld, PasswordNew) == 0)
                {
                    ReturnData.ResponseCode = -7003;
                    ReturnData.Description = "Mật khẩu cũ và mật khẩu mới không được phép trùng nhau";
                    return Json(ReturnData);
                }
                PasswordOld = Encrypt.Md5(PasswordOld);
                PasswordNew = Encrypt.Md5(PasswordNew);
                var Result = AbstractDAOFactory.Instance().CreateUsersDAO().ChangePassword(userValidate.UserName, PasswordOld, PasswordNew);
                if (Result >= 0)
                {
                    ReturnData.Description = "Đổi mật khẩu thành công";
                    AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                    {
                        FunctionID = 999,
                        FunctionName = "Đổi mật khẩu",
                        Description = ReturnData.Description,
                        UserID = userValidate.UserId,
                        ClientIP = userValidate.ClientIP
                    });
                }
                else switch (Result)
                    {
                        case -1: ReturnData.Description = "Tài khoản không tồn tại"; break;
                        case -2: ReturnData.Description = "Tài khoản bị block"; break;
                        case -3: ReturnData.Description = "Mật khẩu cũ không đúng"; break;
                        case -600: ReturnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }

                ReturnData.ResponseCode = Result;

                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }

        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            var ReturnData = new ReturnData();
            try
            {
                var userValidate = new UserValidation();
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }
                if (Permission == null || !userValidate.IsAdministrator || Permission.FunctionID != (int)Enums.FunctionId.User || !Permission.IsDelete)
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn không có quyền sử dụng chức năng này";
                    return Json(ReturnData);
                }
                if (id > 0)
                {
                    var m_user = AbstractDAOFactory.Instance().CreateUsersDAO().SelectByUserID(id);
                    if (m_user == null || m_user.UserID <= 0)
                    {
                        ReturnData.ResponseCode = -101;
                        ReturnData.Description = "Không tồn tại tài khoản cần xóa";
                        return Json(ReturnData);
                    }
                    if (userValidate.UserId == id)
                    {
                        ReturnData.ResponseCode = -101;
                        ReturnData.Description = "Bạn không thể xóa tài khoản của chính mình";
                        return Json(ReturnData);
                    }

                    var result = AbstractDAOFactory.Instance().CreateUsersDAO().DelleteUsers(id);
                    if (result >= 0)
                    {
                        ReturnData.Description = "Xóa user Thành Công";
                        //Ghi log
                        AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                        {
                            FunctionID = 9999,
                            FunctionName = "Xóa user",
                            Description = "Xóa thành công tài khoản " + m_user.Username,
                            UserID = userValidate.UserId,
                            ClientIP = userValidate.ClientIP
                        });
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định user cần xóa";
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }

        [HttpPost]
        public JsonResult UpdateActiveUser(int id)
        {
            var userValidate = new UserValidation();
            var ReturnData = new ReturnData();
            try
            {
                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }
                if (Permission == null || !userValidate.IsAdministrator || !Permission.IsUpdate)
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Bạn không có quyền sử dụng chức năng này";
                    return Json(ReturnData);
                }
                if (id >= 0)
                {
                    if (userValidate.UserId == id)
                    {
                        ReturnData.ResponseCode = -101;
                        ReturnData.Description = "Bạn không được tự bỏ kích hoạt chính mình";
                        return Json(ReturnData);
                    }
                    var m_user = AbstractDAOFactory.Instance().CreateUsersDAO().SelectByUserID(id);
                    if (m_user == null || m_user.UserID <= 0)
                    {
                        ReturnData.ResponseCode = -101;
                        ReturnData.Description = "Không tồn tại tài khoản cần xóa";
                        return Json(ReturnData);
                    }
                    var result = AbstractDAOFactory.Instance().CreateUsersDAO().UpdateActiveUser(id);
                    if (result >= 0)
                    {
                        //Ghi log
                        AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                        {
                            FunctionID = Permission.FunctionID,
                            FunctionName = Permission.FunctionName,
                            Description = "Cập nhật trạng thái tài khoản " + m_user.Username + " (Hệ Thống)",
                            UserID = userValidate.UserId,
                            ClientIP = userValidate.ClientIP
                        });
                        ReturnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định user cần active";
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }

        #endregion

        #region Quản trị Lỗi Hệ Thống
        public ActionResult ManagerErrorLog()
        {
            var userValidate = new UserValidation();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ManagerErrorLog", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            if (Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            var DateNow = DateTime.Now;
            var fromDate = DateNow.AddDays(-1);
            var toDate = DateNow;

            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return View(Permission);
        }
        public ActionResult ListErrorLog(string FromDate, string ToDate)
        {
            var userValidate = new UserValidation();
            List<ErrorLog> data = new List<ErrorLog>();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ManagerErrorLog", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            if (Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }

            var toDate = DateTime.Now;
            var fromDate = toDate.AddDays(-1);
            if (FromDate != null && ToDate != null)
            {
                fromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ToDate = ToDate + " 23:59:59";
                toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            string newFromDate = fromDate.ToString("MM/dd/yyyy");
            string newToDate = toDate.ToString("MM/dd/yyyy HH:mm:ss");

            data = AbstractDAOFactory.Instance().CreateErrorLogDAO().GetErrorLog(newFromDate, newToDate);

            return PartialView(data);

        }
        [HttpPost]
        public JsonResult DelDataErroLog(string fromdate, string todate)
        {
            var ReturnData = new ReturnData();
            try
            {
                var userValidate = new UserValidation();

                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }
                if (!userValidate.IsAdministrator)
                {
                    ReturnData.ResponseCode = -100;
                    ReturnData.Description = "Bạn không có quyền";
                    return Json(ReturnData);
                }
                if (string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(todate))
                {
                    ReturnData.ResponseCode = -600;
                    ReturnData.Description = "dữ liệu đầu vào không hợp lệ";
                    return Json(ReturnData);
                }

                DateTime FromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                todate = todate + " 23:59:59";
                DateTime ToDate = DateTime.ParseExact(todate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ReturnData.ResponseCode = -600;
                    ReturnData.Description = "dữ liệu đầu vào không hợp lệ";
                    return Json(ReturnData);
                }
                string newFromDate = FromDate.ToString("MM/dd/yyyy");
                string newToDate = ToDate.ToString("MM/dd/yyyy HH:mm:ss");
                if (Permission.IsDelete && Permission.FunctionID == (int)Enums.FunctionId.ErrorSystem)
                {
                    var status = AbstractDAOFactory.Instance().CreateErrorLogDAO().Delete(newFromDate, newToDate);
                    if (status >= 0)
                    {
                        //Ghi log
                        AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                        {
                            FunctionID = Permission.FunctionID,
                            FunctionName = Permission.FunctionName,
                            Description = "Xóa Log Lỗi hệ thống :  " + fromdate + "-" + todate,//Convert.ToInt32(e.CommandArgument),//txtFolderName.Text,
                            UserID = userValidate.UserId,
                            ClientIP = userValidate.ClientIP
                        });
                        ReturnData.ResponseCode = 1;
                        ReturnData.Description = "Xóa thành công";
                        return Json(ReturnData);
                    }
                    else
                    {
                        ReturnData.ResponseCode = -2;
                        ReturnData.Description = "Đã có lỗi xảy ra trong quá trình xóa dữ liệu !";
                        return Json(ReturnData);
                    }
                }
                ReturnData.ResponseCode = -3;
                ReturnData.Description = "Không có quyền xóa";
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thông đang bận !";
                return Json(ReturnData);
            }

        }
        #endregion

        #region Quản trị Log người dùng
        public ActionResult ManagerUserLog()
        {
            var userValidate = new UserValidation();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            var listFunction = new List<Functions>();
            var listUser = new List<Users>();
            if (!userValidate.IsSigned())
            {
                return new RedirectResult(urlLogin);
            }
            var checkPermission = userValidate.CheckPermissionUser("ManagerUserLog", userValidate.UserId, userValidate.IsAdministrator);
            if (!checkPermission || Permission == null)
            {
                return new RedirectResult(Config.UrlRoot + "common/errorpermission", true);
            }
            listFunction = AbstractDAOFactory.Instance().CreateFunctionDAO().SelectAllFunctionID(-1, "", -1, -1);
            listFunction = listFunction.FindAll(lf => lf.ActionName != "" && lf.ActionName != null);
            listUser = AbstractDAOFactory.Instance().CreateUsersDAO().GetAllUserByStatus(-1);
            var DateNow = DateTime.Now;
            var fromDate = DateNow.AddDays(-1);
            var toDate = DateNow;
            ViewBag.ListUser = listUser;
            ViewBag.ListFunction = listFunction;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            ViewBag.IsAdmin = userValidate.IsAdministrator;
            ViewBag.UrlLogin = urlLogin;
            return View();
        }
        public ActionResult ListUserLog(string FromDate, string ToDate, int FunctionId, int UserId)
        {
            var Role = 1;
            List<UsersLog> data = new List<UsersLog>();
            var userValidate = new UserValidation();
            var urlLogin = Config.UrlRoot + "Account/FormLogin";
            ViewBag.UrlLogin = urlLogin;
            if (!userValidate.IsSigned())
            {
                Role = -1;
                ViewBag.Role = Role;
                return PartialView(data);
            }
            if (Permission == null || Permission.ActionName != "ManagerUserLog" || !userValidate.IsAdministrator)
            {
                Role = -2;
                ViewBag.Role = Role;
                return PartialView(data);
            }

            var toDate = DateTime.Now;
            var fromDate = toDate.AddDays(-1);
            if (FromDate != null && ToDate != null)
            {
                fromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                ToDate = ToDate + " 23:59:59";
                toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            if (fromDate.CompareTo(toDate) > 0)
            {
                return PartialView(data);
            }

            var totalDay = (toDate - fromDate).Days;
            if (totalDay > 30)
            {
                return PartialView(data);
            }
            string newFromDate = fromDate.ToString("MM/dd/yyyy");
            string newToDate = toDate.ToString("MM/dd/yyyy HH:mm:ss");
            data = AbstractDAOFactory.Instance().CreateUsersLogDAO().GetListUsersLog(newFromDate, newToDate, UserId, FunctionId);
            ViewBag.Role = Role;
            return PartialView(data);

        }
        [HttpPost]
        public JsonResult DelDataUserLog(string fromdate, string todate, int FunctionId, int UserId)
        {
            var ReturnData = new ReturnData();
            try
            {
                var userValidate = new UserValidation();

                if (!userValidate.IsSigned())
                {
                    var url = Config.UrlRoot + "Account/FormLogin";
                    var urlLogout = string.Format("{0}?act=out", url);
                    Response.Redirect(urlLogout, true);
                }
                if (string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(todate))
                {
                    ReturnData.ResponseCode = -600;
                    ReturnData.Description = "dữ liệu đầu vào không hợp lệ";
                    return Json(ReturnData);
                }

                DateTime FromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                todate = todate + " 23:59:59";
                DateTime ToDate = DateTime.ParseExact(todate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                if (DateTime.Compare(FromDate, ToDate) > 0)
                {
                    ReturnData.ResponseCode = -600;
                    ReturnData.Description = "dữ liệu đầu vào không hợp lệ";
                    return Json(ReturnData);
                }
                string newFromDate = FromDate.ToString("MM/dd/yyyy");
                string newToDate = ToDate.ToString("MM/dd/yyyy HH:mm:ss");
                if (Permission.IsDelete && Permission.FunctionID == (int)Enums.FunctionId.UserLog)
                {
                    var status = AbstractDAOFactory.Instance().CreateUsersLogDAO().DeleteUsersLog(newFromDate, newToDate, UserId, FunctionId);
                    if (status > 0)
                    {
                        //Ghi log
                        AbstractDAOFactory.Instance().CreateUsersLogDAO().InsertUsersLog(new UsersLog
                        {
                            FunctionID = Permission.FunctionID,
                            FunctionName = Permission.FunctionName,
                            Description = "Xóa LogUser :  " + fromdate + "-" + todate,
                            UserID = userValidate.UserId,
                            ClientIP = userValidate.ClientIP
                        });
                        ReturnData.ResponseCode = 1;
                        ReturnData.Description = "Xóa thành công";
                        return Json(ReturnData);
                    }
                    else
                    {
                        ReturnData.ResponseCode = -2;
                        ReturnData.Description = "Đã có lỗi xảy ra trong quá trình xóa dữ liệu !";
                        return Json(ReturnData);
                    }
                }
                ReturnData.ResponseCode = -3;
                ReturnData.Description = "Không có quyền xóa";
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thông đang bận !";
                return Json(ReturnData);
            }

        }
        #endregion
    }
}