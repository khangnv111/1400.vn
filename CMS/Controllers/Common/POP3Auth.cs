using System;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Summary description for POP3Auth
/// </summary>

namespace CMS.Controllers.Common
{
    public class POP3Auth : TcpClient
    {
        public string CheckAuth(string username, string password)
        {
            string message;
            string response;
            Connect("smtp.mail.vtc.vn", 110);
            response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                return response;
            }
            message = "USER " + username + "\r\n";
            Write(message);
            response = Response();

            if (response.Substring(0, 3) != "+OK")
            {
                return response;
            }
            message = "PASS " + password + "\r\n";
            Write(message);
            response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                return response;
            }
            message = "QUIT\r\n";
            Write(message);
            return "OK";
        }

        private string Response()
        {
            var enc = new ASCIIEncoding();
            var serverbuff = new Byte[1024];
            NetworkStream stream = GetStream();
            int count = 0;
            while (true)
            {
                var buff = new Byte[2];
                int bytes = stream.Read(buff, 0, 1);
                if (bytes == 1)
                {
                    serverbuff[count] = buff[0];
                    count++;
                    if (buff[0] == '\n')
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            string retval = enc.GetString(serverbuff, 0, count);
            return retval;
        }

        private void Write(string message)
        {
            var en = new ASCIIEncoding();
            var WriteBuffer = new byte[1024];
            WriteBuffer = en.GetBytes(message);
            NetworkStream stream = GetStream();
            stream.Write(WriteBuffer, 0, WriteBuffer.Length);
        }
    }
}