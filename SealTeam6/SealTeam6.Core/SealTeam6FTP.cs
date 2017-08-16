using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SealTeam6.Core
{
    public static class SealTeam6FTP
    {
        public static void PromptResume(Action resumption, string message = "Try again?")
        {
            System.Console.Write(message);
            System.Console.Write(" (y/n): ");
            var userResponse = System.Console.ReadLine();
            if (userResponse.ToLower() != "y")
            {
                if (userResponse.ToLower() == "n") return;
                else
                {
                    System.Console.WriteLine("Please enter y or n");
                    PromptResume(resumption, message);
                }
            }
            else resumption();
        }

        public static void GetFile(FluentFTP.FtpClient session, String local, String remote)
        {
            try
            {
                session.DownloadFile(local, remote);
            }
            catch (FluentFTP.FtpCommandException e)
            {
                Console.WriteLine("Exception: " + e.Message);
                PromptResume(() => GetFile(session, local, remote));
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("Exception: " + e.Message);
                PromptResume(() => GetFile(session, local, remote));
            }
        }

        public static void GetFiles(FluentFTP.FtpClient session, String directory, List<String> files)
        {
            try
            {
                session.DownloadFiles(directory, files);
                return;
            }
            catch (FluentFTP.FtpCommandException e)
            {
                Console.WriteLine("Could not download files: " + e.Message);
                PromptResume(() => GetFiles(session, directory, files));
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("Could not write downloaded files to disk: " + e.Message);
                PromptResume(() => GetFiles(session, directory, files));
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
            catch (FluentFTP.FtpCommandException e)
            {
                
                Console.WriteLine("Failed to log in: " + e.Message);
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
        public static void ChangePerms(FluentFTP.FtpClient session, String file, int to_set)
		{
            try 
            {
                FileInfo file_info = new FileInfo(file);
                session.SetFilePermissions(file_info.Directory.FullName + "\\" + file, to_set);
                return;
            }
            catch (FluentFTP.FtpCommandException e)
            {
                Console.WriteLine("CHMOD failed: " + e.Message);
                PromptResume(() => ChangePerms(session, file, to_set));
            }
        }

        public static void CreateDir(FluentFTP.FtpClient session, String to_create){
            try 
            {
                session.CreateDirectory(to_create);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("Could not create directory: " + e.Message);
                PromptResume(() => CreateDir(session, to_create));
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
            catch (System.IO.IOException e)
            {
                Console.WriteLine("Rename Failed: " + e.Message);
                PromptResume(() => RenameLocal(file, new_name));
            }
        }

        public static void RenameRemote(FluentFTP.FtpClient session, String file, String new_name)
        {
            try
            {
                session.Rename(file, new_name);
            }
            catch (FluentFTP.FtpCommandException e)
            {
                Console.WriteLine("Rename Failed: " + e.Message);
                PromptResume(() => RenameRemote(session, file, new_name));
            }
        }
    }
}