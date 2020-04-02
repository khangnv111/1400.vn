using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W1400.Utility
{
    public class NoMark
    {
        public string VietnameseNoMark(string Str)
        {
            if (!string.IsNullOrEmpty(Str))
            {
                Str = Str.Trim();
                Str = Str.Replace("á", "a");
                Str = Str.Replace("à", "a");
                Str = Str.Replace("ạ", "a");
                Str = Str.Replace("ả", "a");
                Str = Str.Replace("ã", "a");

                Str = Str.Replace("Á", "A");
                Str = Str.Replace("À", "A");
                Str = Str.Replace("Ạ", "A");
                Str = Str.Replace("Ả", "A");
                Str = Str.Replace("Ã", "A");

                Str = Str.Replace("ă", "a");
                Str = Str.Replace("ắ", "a");
                Str = Str.Replace("ằ", "a");
                Str = Str.Replace("ặ", "a");
                Str = Str.Replace("ẳ", "a");
                Str = Str.Replace("ẵ", "a");

                Str = Str.Replace("Ă", "A");
                Str = Str.Replace("Ắ", "A");
                Str = Str.Replace("Ằ", "A");
                Str = Str.Replace("Ặ", "A");
                Str = Str.Replace("Ẳ", "A");
                Str = Str.Replace("Ẵ", "A");

                Str = Str.Replace("â", "a");
                Str = Str.Replace("ấ", "a");
                Str = Str.Replace("ầ", "a");
                Str = Str.Replace("ậ", "a");
                Str = Str.Replace("ẩ", "a");
                Str = Str.Replace("ẫ", "a");

                Str = Str.Replace("Â", "A");
                Str = Str.Replace("Ấ", "A");
                Str = Str.Replace("Ầ", "A");
                Str = Str.Replace("Ậ", "A");
                Str = Str.Replace("Ẩ", "A");
                Str = Str.Replace("Ẫ", "A");

                Str = Str.Replace("đ", "d");
                Str = Str.Replace("Đ", "D");

                Str = Str.Replace("é", "e");
                Str = Str.Replace("è", "e");
                Str = Str.Replace("ẻ", "e");
                Str = Str.Replace("ẹ", "e");
                Str = Str.Replace("ẽ", "e");

                Str = Str.Replace("É", "E");
                Str = Str.Replace("È", "E");
                Str = Str.Replace("Ẻ", "E");
                Str = Str.Replace("Ẹ", "E");
                Str = Str.Replace("Ẽ", "E");

                Str = Str.Replace("ê", "e");
                Str = Str.Replace("ế", "e");
                Str = Str.Replace("ề", "e");
                Str = Str.Replace("ể", "e");
                Str = Str.Replace("ệ", "e");
                Str = Str.Replace("ễ", "e");

                Str = Str.Replace("Ê", "E");
                Str = Str.Replace("Ế", "E");
                Str = Str.Replace("Ề", "E");
                Str = Str.Replace("Ể", "E");
                Str = Str.Replace("Ệ", "E");
                Str = Str.Replace("Ễ", "E");

                Str = Str.Replace("í", "i");
                Str = Str.Replace("ì", "i");
                Str = Str.Replace("ỉ", "i");
                Str = Str.Replace("ị", "i");
                Str = Str.Replace("ĩ", "i");

                Str = Str.Replace("Í", "I");
                Str = Str.Replace("Ì", "I");
                Str = Str.Replace("Ỉ", "I");
                Str = Str.Replace("Ị", "I");
                Str = Str.Replace("Ĩ", "I");

                Str = Str.Replace("ó", "o");
                Str = Str.Replace("ò", "o");
                Str = Str.Replace("ỏ", "o");
                Str = Str.Replace("ọ", "o");
                Str = Str.Replace("õ", "o");

                Str = Str.Replace("Ó", "O");
                Str = Str.Replace("Ò", "O");
                Str = Str.Replace("Ỏ", "O");
                Str = Str.Replace("Ọ", "O");
                Str = Str.Replace("Õ", "O");

                Str = Str.Replace("ô", "o");
                Str = Str.Replace("ố", "o");
                Str = Str.Replace("ồ", "o");
                Str = Str.Replace("ổ", "o");
                Str = Str.Replace("ộ", "o");
                Str = Str.Replace("ỗ", "o");

                Str = Str.Replace("Ô", "O");
                Str = Str.Replace("Ố", "O");
                Str = Str.Replace("Ồ", "O");
                Str = Str.Replace("Ổ", "O");
                Str = Str.Replace("Ộ", "O");
                Str = Str.Replace("Ỗ", "O");

                Str = Str.Replace("ơ", "o");
                Str = Str.Replace("ớ", "o");
                Str = Str.Replace("ờ", "o");
                Str = Str.Replace("ở", "o");
                Str = Str.Replace("ợ", "o");
                Str = Str.Replace("ỡ", "o");

                Str = Str.Replace("Ơ", "O");
                Str = Str.Replace("Ớ", "O");
                Str = Str.Replace("Ờ", "O");
                Str = Str.Replace("Ở", "O");
                Str = Str.Replace("Ọ", "O");
                Str = Str.Replace("Ỡ", "O");

                Str = Str.Replace("ú", "u");
                Str = Str.Replace("ù", "u");
                Str = Str.Replace("ủ", "u");
                Str = Str.Replace("ụ", "u");
                Str = Str.Replace("ũ", "u");

                Str = Str.Replace("Ú", "U");
                Str = Str.Replace("Ù", "U");
                Str = Str.Replace("Ủ", "U");
                Str = Str.Replace("Ụ", "U");
                Str = Str.Replace("Ũ", "U");

                Str = Str.Replace("ư", "u");
                Str = Str.Replace("ứ", "u");
                Str = Str.Replace("ừ", "u");
                Str = Str.Replace("ử", "u");
                Str = Str.Replace("ự", "u");
                Str = Str.Replace("ữ", "u");

                Str = Str.Replace("Ư", "U");
                Str = Str.Replace("Ứ", "U");
                Str = Str.Replace("Ừ", "U");
                Str = Str.Replace("Ử", "U");
                Str = Str.Replace("Ự", "U");
                Str = Str.Replace("Ữ", "U");

                Str = Str.Replace("ý", "y");
                Str = Str.Replace("ỳ", "y");
                Str = Str.Replace("ỷ", "y");
                Str = Str.Replace("ỵ", "y");
                Str = Str.Replace("ỹ", "y");

                Str = Str.Replace("Ý", "Y");
                Str = Str.Replace("Ỳ", "Y");
                Str = Str.Replace("Ỷ", "Y");
                Str = Str.Replace("Ỵ", "Y");
                Str = Str.Replace("Ỹ", "Y");

                //-------------------------
                Str = Str.Replace(";", "-");
                Str = Str.Replace("?", "-");
                Str = Str.Replace("[", "-");
                Str = Str.Replace("]", "-");
                Str = Str.Replace("(", "-");
                Str = Str.Replace(")", "-");
                Str = Str.Replace("}", "-");
                Str = Str.Replace("{", "-");
                Str = Str.Replace(":", "-");
                Str = Str.Replace('"', '-');
                Str = Str.Replace("/", "-");
                Str = Str.Replace(">", "-");
                Str = Str.Replace("<", "-");
                Str = Str.Replace("#", "-");
                Str = Str.Replace("%", "-");
                Str = Str.Replace("+", "-");
                Str = Str.Replace("*", "-");
                Str = Str.Replace("=", "-");
                Str = Str.Replace("~", "-");
                Str = Str.Replace("`", "-");
                Str = Str.Replace("^", "-");
                Str = Str.Replace("&", "-");
                Str = Str.Replace("|", "-");
                Str = Str.Replace("'", "-");
                Str = Str.Replace("“", "-");
                Str = Str.Replace("”", "-");
                Str = Str.Replace("”", "-");
                Str = Str.Replace("‘", "-");
                Str = Str.Replace("’", "-");
                Str = Str.Replace(",", "-");
                Str = Str.Replace(".", "");
                Str = Str.Replace(" ", "-");
                Str = Str.Replace("_", "-");
                Str = Str.Replace("---", "-");
                Str = Str.Replace("--", "-");

            }
            return Str;
        }

        //  replace các ký tự đặc biệt
        public string ReplaceSymbol(string Str) {
            if (!string.IsNullOrEmpty(Str))
            {
                Str = Str.Trim(); 
                Str = Str.Replace(";", "-");
                Str = Str.Replace("?", "-");
                Str = Str.Replace("[", "-");
                Str = Str.Replace("]", "-");
                Str = Str.Replace("(", "-");
                Str = Str.Replace(")", "-");
                Str = Str.Replace("}", "-");
                Str = Str.Replace("{", "-");
                Str = Str.Replace(":", "-");
                Str = Str.Replace('"', '-');
                Str = Str.Replace("/", "-");
                Str = Str.Replace(">", "-");
                Str = Str.Replace("<", "-");
                Str = Str.Replace("#", "-");
                Str = Str.Replace("%", "-");
                Str = Str.Replace("+", "-");
                Str = Str.Replace("*", "-");
                Str = Str.Replace("=", "-");
                Str = Str.Replace("~", "-");
                Str = Str.Replace("`", "-");
                Str = Str.Replace("^", "-");
                Str = Str.Replace("&", "-");
                Str = Str.Replace("|", "-");
                Str = Str.Replace("'", "-");
                Str = Str.Replace(",", "-");
                Str = Str.Replace("_", "-");
                Str = Str.Replace("---", "-");
                Str = Str.Replace("--", "-");
            }
            return Str;
        }
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string ReWrite(string Str,int Id)
        {
            NoMark rw = new NoMark();
            Str = rw.VietnameseNoMark(Str.ToLower());
            Str = rw.VietnameseNoMark(Str);
            if (Str.StartsWith("-"))
            {
                Str = Str.Substring(1);
            }

            if (Str.EndsWith("-"))
            {
                Str = Str.Substring(0, Str.Length - 1);
            }
            Str =System.Configuration.ConfigurationManager.AppSettings["URL_ROOT"] + Str + "-post" + Id.ToString();
            return Str;
        }
        public string ReWriteProgramPending(string Str, int Id)
        {
            NoMark rw = new NoMark();
            Str = rw.VietnameseNoMark(Str.ToLower());
            Str = rw.VietnameseNoMark(Str);
            if (Str.StartsWith("-"))
            {
                Str = Str.Substring(1);
            }

            if (Str.EndsWith("-"))
            {
                Str = Str.Substring(0, Str.Length - 1);
            }
            Str = System.Configuration.ConfigurationManager.AppSettings["URL_ROOT"] + Str + "-pen" + Id.ToString();
            return Str;
        }
        public string ReWriteProgramPass(string Str, int Id)
        {
            NoMark rw = new NoMark();
            Str = rw.VietnameseNoMark(Str.ToLower());
            Str = rw.VietnameseNoMark(Str);
            if (Str.StartsWith("-"))
            {
                Str = Str.Substring(1);
            }

            if (Str.EndsWith("-"))
            {
                Str = Str.Substring(0, Str.Length - 1);
            }
            Str = System.Configuration.ConfigurationManager.AppSettings["URL_ROOT"] + Str + "-pass" + Id.ToString();
            return Str;
        }
    }
}
