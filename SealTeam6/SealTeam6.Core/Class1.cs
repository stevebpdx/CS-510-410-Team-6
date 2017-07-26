﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SealTeam6.Core
{
    public class Class1
    {
        private NetworkCredential credentials;
        private String host;

        public void LogIn()
        {
            Console.Write("Host: ");
            host = Console.ReadLine();
            Regex regex = new Regex("\\A([a-zA-Z0-9]|\\.){11,}\\z");
            while (true)
            {
                if (host != null)
                {
                    if (regex.IsMatch(host))
                    {
                        break;
                    }
                }
                Console.Write("Invalid host. (Valid characters: letters, numbers and periods)\n");
                Console.Write("Host: ");
                host = Console.ReadLine();
            }
            Console.Write("Username: ");
            String username = Console.ReadLine();
            Console.Write("Password: ");
            String password = "";
            char key = Console.ReadKey(true).KeyChar;
            while (key != (char)System.Environment.NewLine.ToCharArray()[0])
            {
                password += key;
                Console.Write("*");
                key = Console.ReadKey(true).KeyChar;
            }
            Console.Write("\n");
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + host + "/");
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            credentials = new NetworkCredential(username, password);
            request.Credentials = credentials;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (WebException e)
            {
                Console.Write(e.Message);
            }
        }

        public void ListRemote(String directory, bool verbose)
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
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                if (verbose)
                {
                    Console.WriteLine("Date      Time          <DIR> or Bytes Name");
                }
                Console.WriteLine(reader.ReadToEnd());
                reader.Close();
                response.Close();
            }
            catch (WebException e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
