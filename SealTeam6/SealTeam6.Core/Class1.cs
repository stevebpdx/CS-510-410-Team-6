﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SealTeam6.Core
{
    public class Class1
    {
        public static void LogIn()
        {
            Console.Write("Host: ");
            String host = Console.ReadLine();
            Regex regex = new Regex("\\A([a-zA-Z0-9]|\\.){11,}\\z");
            if (host == null)
            {
                Console.Write("Error: Host Null");
                return;
            }
            while (!regex.IsMatch(host))
            {
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
            Console.Write(password);
            Console.ReadLine();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + host + "/");
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(username, password);
            try
            {
                FtpWebResponse directoryListResponse = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
