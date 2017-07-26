using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SealTeam6.Core;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace SealTeam6.Console
{
    class Program
    {
        public static string PromptHost()
        {

            System.Console.Write("Host: ");
            var host = System.Console.ReadLine();
            var regex = new Regex("\\A([a-zA-Z0-9]|\\.){11,}\\z");
            while (true)
            {
                if (host != null)
                {
                    if (regex.IsMatch(host))
                    {
                        break;
                    }
                }
                System.Console.Write("Invalid host. (Valid characters: letters, numbers and periods)\n");
                System.Console.Write("Host: ");
                host = System.Console.ReadLine();
            }
            return host;
        }

        public static NetworkCredential LogIn(string host)
        {
            System.Console.Write("Username: ");
            String username = System.Console.ReadLine();
            System.Console.Write("Password: ");
            String password = "";
            char key = System.Console.ReadKey(true).KeyChar;
            while (key != (char)System.Environment.NewLine.ToCharArray()[0])
            {
                password += key;
                System.Console.Write("*");
                key = System.Console.ReadKey(true).KeyChar;
            }
            System.Console.Write("\n");
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + host + "/");
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            var credentials = new NetworkCredential(username, password);
            request.Credentials = credentials;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (WebException e)
            {
                System.Console.Write(e.Message);
            }
            return credentials;
        }

        static void Main(string[] args)
        {
            //var app = new Class1();
            var host = PromptHost();
            var credentials = LogIn(host);
            var fluentSession = new FluentFTP.FtpClient(host);
            fluentSession.Credentials = credentials;
            System.Console.WriteLine("Date\tTime\t< DIR > or Bytes Name");
            foreach(var file in fluentSession.GetListing())
            {
                System.Console.WriteLine("{0}\t{1}\t{2}", 
                    file.Name,
                    file.Created.ToString(),
                    file.Type == FluentFTP.FtpFileSystemObjectType.File ? file.Size.ToString() : "< DIR >");
            }
            System.Console.Write("Press return to continue...");
            System.Console.ReadLine();
        }
    }
}
