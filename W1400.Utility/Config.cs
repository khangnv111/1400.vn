﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using W1400.Utility.Security;
using System.Linq;
namespace W1400.Utility
{
    public class Config
    {
        public static string Site1400ConnectionString
        {
            get { return GetConnectionString("Site1400"); }
        }
        public static string Site1400MessConnectionString
        {
            get { return GetConnectionString("Site1400Mess"); }
        }
        
        //public static string WebIntecom
        //{
        //    get { return GetConnectionString("WebIntecom"); }
        //}
        //Dành cho email------
        public static string MailFromName { get { return GetAppsetting("MAIL.FROM_NAME"); } }
        public static string MailFromEmail { get { return GetAppsetting("MAIL.FROM_EMAIL"); } }
        public static string MailKey { get { return GetAppsetting("MAIL.KEY"); } }

        public static string SSODomain { get { return GetAppsetting("SSO_DOMAIN"); } }
        public static string DomainName { get { return GetAppsetting("DomainName"); } }

        public static string SiteName { get { return GetAppsetting("SiteName"); } }

        private static string GetConnectionString(string nameConnection)
        {
            return ConfigurationManager.ConnectionStrings[nameConnection] == null ? string.Empty : new RijndaelEnhanced("Site1400v2017", "@1B2c3D4e5F6g7H8").Decrypt(ConfigurationManager.ConnectionStrings[nameConnection].ConnectionString);
        }


        public static string GetAppsetting(string appSettingName)
        {
            return ConfigurationManager.AppSettings[appSettingName] ?? string.Empty;
        }

        /// <summary>
        /// Lấy ip
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            var ip = "";
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (ip == "")
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        public static string UrlRoot
        {
            get
            {
                var sRet = System.Web.HttpContext.Current.Request.ApplicationPath;
                if (!sRet.EndsWith("/"))
                    sRet = sRet + "/";
                return sRet;
            }
        }
        public static string ApplicationUrl
        {
            get
            {
                var url = ConfigurationManager.AppSettings["URL_ROOT"] ?? "http://" + HttpContext.Current.Request.Url.Authority +
                   HttpContext.Current.Request.ApplicationPath;
                return url.EndsWith("/") ? url : url + "/";
            }
        }

        public static string CutString(string str, int length)
        {
            if (str.Length > length)
            {
                str = str.Substring(0, length);
                str += "...";
            }
            return str;
        }

        public static string GetUniqueFileName(string strDir, string strFileName)
        {
            if (strDir != "" && !(strDir.EndsWith("/") || strDir.EndsWith("\\"))) strDir += "/";
            var strExt = Path.GetExtension(strFileName);
            strFileName = Path.GetFileNameWithoutExtension(strFileName);
            strFileName = strFileName.Replace(" ", "_");

            var fileAppend = 0;
            var mStrFileName = strFileName;

            while (Directory.GetFiles(strDir, mStrFileName + ".*").Length > 0)
            {
                fileAppend++;
                var mAppend = "_" + fileAppend;
                mStrFileName = Path.GetFileNameWithoutExtension(strFileName) + mAppend;
            }
            return mStrFileName + "_" + string.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + strExt;
        }

        public string GetPathFile(string filename)
        {
            var path = MediaPath + "/" + string.Format("{0:yyyyMM}", DateTime.Now);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + "/";
        }
        public string GetPathFile1(string system, string foldername, string filename)
        {
            var path = MediaPath + "/" + system + "/" + foldername + "/" + string.Format("{0:yyyyMM}", DateTime.Now);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + "/";
        }


        public static bool IsNumber(string strNumber)
        {
            return !new Regex("[^0-9]").IsMatch(strNumber);
            //const string MatchNumber = @"^[0-9]$";
            //return System.Text.RegularExpressions.Regex.IsMatch(strNumber, MatchNumber);
        }

        public enum CateType : int
        {
            Card = 1,
            Topup = 2
        }
        public enum CATEGORY_TYPE_TOPUP : int
        {
            TELCO = 11, //Topup Điện thoại
        }
        public static string MediaUrl
        {
            get
            {
                var mediaUrl = ConfigurationManager.AppSettings["mediaUrl"] ?? ApplicationUrl;
                return mediaUrl;
            }
        }


        public static string MediaPath
        {
            get
            {
                var sRet = ConfigurationManager.AppSettings["mediaPath"];
                if (!sRet.EndsWith("/"))
                    sRet += "/";
                return sRet;
            }
        }
        public static string ImageFileType
        {
            get
            {
                return ConfigurationManager.AppSettings["imageFileTypes"];
            }
        }

        public static string FlashFileType
        {
            get
            {
                return ConfigurationManager.AppSettings["FlashFileTypes"];
            }
        }


        public static string MediaFileType
        {
            get
            {
                return ConfigurationManager.AppSettings["mediaFileTypes"];
            }
        }

        public static void SetLanguage()
        {
            CultureInfo culture;
            var lang = HttpContext.Current.Session[SessionsManager.SESSION_LANGUAGE] != null
                        ? HttpContext.Current.Session[SessionsManager.SESSION_LANGUAGE].ToString()
                        : "vi";
            switch (lang)
            {
                case "vi":
                    culture = new CultureInfo("vi-VN");
                    break;
                case "en":
                    culture = new CultureInfo("en-US");
                    break;
                default:
                    culture = new CultureInfo("vi-VN");
                    break;
            }

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }


        public static string GetCurrentLanguage()
        {
            return HttpContext.Current.Session[SessionsManager.SESSION_LANGUAGE] != null
                         ? HttpContext.Current.Session[SessionsManager.SESSION_LANGUAGE].ToString()
                         : "vi";
        }

        public static int menuId
        {
            get
            {
                var _id = 0;
                int.TryParse(HttpContext.Current.Request["fid"], out _id);
                Config.Id = _id;
                return _id;
            }
        }

        public static int Id
        {
            get
            {
                return HttpContext.Current.Session[SessionsManager.SESSION_ID] != null
                           ? int.Parse(HttpContext.Current.Session[SessionsManager.SESSION_ID].ToString())
                           : 0;
            }
            set { HttpContext.Current.Session[SessionsManager.SESSION_ID] = value; }
        }
         
        // them moi String Values
        public static string stringValue
        {
            get
            {
                return HttpContext.Current.Session[SessionsManager.SESSION_STRINGVALUE] != null
                           ? (HttpContext.Current.Session[SessionsManager.SESSION_STRINGVALUE].ToString())
                           : "";
            }
            set { HttpContext.Current.Session[SessionsManager.SESSION_STRINGVALUE] = value; }
        }

        public static void Back()
        {
            Id = 0;
            var url = string.Format("{0}{1}/index.html", ApplicationUrl, int.Parse(HttpContext.Current.Session[SessionsManager.SESSION_HISTORY].ToString()));
            HttpContext.Current.Response.Redirect(url, false);
        }

        public static int UserId
        {
            get
            {
                return HttpContext.Current.Session[SessionsManager.SESSION_USERID] != null
                           ? int.Parse(HttpContext.Current.Session[SessionsManager.SESSION_USERID].ToString())
                           : 0;
            }
            set { HttpContext.Current.Session[SessionsManager.SESSION_USERID] = value; }
        }
        public static string UserName
        {
            get
            {
                return HttpContext.Current.Session[SessionsManager.SESSION_USERNAME] != null
                           ? HttpContext.Current.Session[SessionsManager.SESSION_USERNAME].ToString()
                           : string.Empty;
            }
            set { HttpContext.Current.Session[SessionsManager.SESSION_USERNAME] = value; }
        }


        public static void Message(Page page, string message, string title)
        {
            var scriptText = "jAlert('" + message + "', '" + title + "');";
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "validationAlert", scriptText, true);
        }

        /// <summary>
        /// JAlert(Page, 'Title','Messenger',function('url'){document.location=url;})
        /// </summary>
        /// <param name="page"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="method"></param>
        public static void Message(Page page, string message, string title, string method)
        {
            var scriptText = "jAlert('" + message + "', '" + title + "'," + method + ");";
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "validationAlert", scriptText, true);
        }

        public static void Redirect(int controlid, int value)
        {
            Id = value;
            var url = string.Format("{0}load-control/function/{1}", ApplicationUrl, controlid);
            HttpContext.Current.Response.Redirect(url, false);
            return;
        }
        
        // Them Moi
        public static void RedirectString(int controlid, string Value)
        {
            stringValue = Value;
            var url = string.Format("{0}{1}/index.html", ApplicationUrl, controlid);
            HttpContext.Current.Response.Redirect(url, false);
            return;
        }

        public static void Dowload(Page page, string url)
        {
            var script = "Download('" + url + "');";
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "validationAlert", script, true);
        }

        public static void Download(string filepath, string filename)
        {
            Stream iStream = null;
            var buffer = new Byte[100000];
            try
            {
                iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var dataToRead = iStream.Length;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                while (dataToRead > 0)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        var length = iStream.Read(buffer, 0, 100000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        buffer = new Byte[100000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                File.Delete(filepath);
                NLogLogger.Info(ex.ToString());
                HttpContext.Current.Response.Write("Error :  " + ex.Message);
                Back();
            }
            finally
            {
                if (iStream != null)
                {
                    iStream.Close();
                }
                HttpContext.Current.Response.Close();
                File.Delete(filepath);
            }

        }



       
        public static void MergeRows(GridView radGrid, int colPos)
        {
            for (var i = radGrid.Rows.Count - 1; i > 0; i--)
            {
                var cell = radGrid.Rows[i].Cells[colPos];
                var cellPre = radGrid.Rows[i - 1].Cells[colPos];
                var lit = (cell.Controls[0] as DataBoundLiteralControl);
                var litPre = (cellPre.Controls[0] as DataBoundLiteralControl);
                if (lit != null)
                    if (litPre != null)
                        if (lit.Text == litPre.Text)
                        {
                            cellPre.RowSpan = cell.RowSpan < 2 ? 2 : cell.RowSpan + 1;
                            cell.Visible = false;
                        }
            }
        }


        public static DataTable ConvertTo<T>(IList<T> lst)
        {
            //create DataTable Structure
            var tbl = CreateTable<T>();
            var entType = typeof(T);

            var properties = TypeDescriptor.GetProperties(entType);
            //get the list item and add into the list
            foreach (var item in lst)
            {
                var row = tbl.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                tbl.Rows.Add(row);
            }

            return tbl;
        }

        private static DataTable CreateTable<T>()
        {
            //T –> ClassName
            var entType = typeof(T);
            //set the datatable name as class name
            var tbl = new DataTable(entType.Name);
            //get the property list
            var properties = TypeDescriptor.GetProperties(entType);
            foreach (PropertyDescriptor prop in properties)
            {
                //add property as column
                try
                {
                    tbl.Columns.Add(prop.Name, prop.PropertyType);
                }
                catch
                {
                    if (prop.PropertyType.ToString().ToLower().Contains("datetime"))
                    {
                        tbl.Columns.Add(prop.Name, typeof(DateTime));
                        continue;

                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("boolean"))
                    {
                        tbl.Columns.Add(prop.Name, typeof(bool));
                        continue;
                    }
                    if (prop.PropertyType.ToString().ToLower().Contains("int"))
                    {
                        tbl.Columns.Add(prop.Name, typeof(int));
                        continue;
                    }
                }
            }
            return tbl;
        }

        public static String UCS2Convert(String sContent)
        {
            sContent = sContent.Trim().ToLower();
            var sUTF8Lower = "a|á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ|đ|e|é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ|i|í|ì|ỉ|ĩ|ị|o|ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ|u|ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự|y|ý|ỳ|ỷ|ỹ|ỵ";

            var sUCS2Lower = "a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|d|e|e|e|e|e|e|e|e|e|e|e|e|i|i|i|i|i|i|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|u|u|u|u|u|u|u|u|u|u|u|u|y|y|y|y|y|y";

            var aUTF8Lower = sUTF8Lower.Split(new[] { '|' });

            var aUCS2Lower = sUCS2Lower.Split(new[] { '|' });

            var l = aUTF8Lower.GetUpperBound(0);

            for (var i = 1; i < l; i++)
            {
                sContent = sContent.Replace(aUTF8Lower[i], aUCS2Lower[i]);
            }
            sContent = sContent.Replace(" ", "-");
            var list = new List<string> { "/", "?", "&", ":", "#", "*", "\"", "@", "%", "$", "!", "~" };
            sContent = list.Aggregate(sContent, (current, str) => current.Replace(str, "-"));

            const string filter = ".-_[]0123456789abcdefghijklmnopqrstuvwxyz";
            var s = "";
            l = sContent.Length;
            for (var i = 0; i < l; i++)
            {
                if (filter.IndexOf(sContent[i]) >= 0)
                {
                    s = s + sContent[i];
                }
            }

            return s;
        }

    }


}
