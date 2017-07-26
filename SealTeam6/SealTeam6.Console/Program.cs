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

        public static string ListRemote(String directory, bool verbose, NetworkCredential credentials,
            string host)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + host + directory);
            if (verbose)
            {
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            }
            else
            {
                request.Method = WebRequestMethods.Ftp.ListDirectory;
            }
            request.Credentials = credentials;
            var output = new StringBuilder();
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                if (verbose)
                {
                    output.AppendLine("Date      Time          <DIR> or Bytes Name");
                }
                output.AppendLine(reader.ReadToEnd());
                reader.Close();
                response.Close();
            }
            catch (WebException e)
            {
                output.AppendLine(e.Message);
            }
            return output.ToString();
        }

        static void Main(string[] args)
        {
            var app = new Class1();
            var host = PromptHost();
            var credentials = LogIn(host);
            var output = ListRemote("/",true,credentials,host);
            System.Console.Write("Press return to continue...");
            System.Console.ReadLine();
        }
    }
}
