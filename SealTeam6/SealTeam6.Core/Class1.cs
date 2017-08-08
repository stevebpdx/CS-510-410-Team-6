﻿using System;
using System.IO;
using System.Net;

namespace SealTeam6.Core
{
    public class Class1
    {
        public static void GetFile(FluentFTP.FtpClient session, String local, String remote)
        {
            try
            {
                session.DownloadFile(local, remote);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public static FluentFTP.FtpClient LogIn(String host, String username, String password)
        {
            NetworkCredential credentials = new NetworkCredential(username, password);
            var session = new FluentFTP.FtpClient(host);
            session.Credentials = credentials;
            try
            {
                session.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return null;
            }
            Console.WriteLine("Connection established.");
            return session;
        }

        public static void LogOut(FluentFTP.FtpClient session)
        {
            if (session != null && session.IsConnected)
            {
                session.Disconnect();
                Console.WriteLine("Connection terminated.");
            }
        }

        public static void RenameLocal(String file, String new_name)
        {
            try
            {
                FileInfo file_info = new FileInfo(file);
                FileInfo new_info = new FileInfo(new_name);
                File.Move(file, (file_info.Directory.FullName + "\\" + new_info.Name));
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}