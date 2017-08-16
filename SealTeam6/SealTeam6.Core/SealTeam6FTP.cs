using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FluentFTP;
using System.Text.RegularExpressions;

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

        public static void GetFile(FtpClient session, String local, String remote)
        {
            try
            {
                session.DownloadFile(local, remote);
            }
            catch (FtpCommandException e)
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

        public static void GetFiles(FtpClient session, String directory, List<String> files)
        {
            try
            {
                session.DownloadFiles(directory, files);
                return;
            }
            catch (FtpCommandException e)
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

        public static FtpClient LogIn(String host, String username, String password)
        {
            NetworkCredential credentials = new NetworkCredential(username, password);
            var session = new FtpClient(host);
            session.Credentials = credentials;
            try
            {
                session.Connect();
            }
            catch (FtpCommandException e)
            {
                
                Console.WriteLine("Failed to log in: " + e.Message);
                return null;
            }
            Console.WriteLine("Connection established.");
            return session;
        }

        public static void LogOut(FtpClient session)
        {
            if (session != null && session.IsConnected)
            {
                session.Disconnect();
                Console.WriteLine("Connection terminated.");
            }
        }

        public static void ChangePerms(FtpClient session, String file, int to_set)
		{
            try
            {
                FileInfo file_info = new FileInfo(file);
                session.SetFilePermissions(file_info.Directory.FullName + "\\" + file, to_set);
                return;
            }
            catch (FtpCommandException e)
            {
                Console.WriteLine("CHMOD failed: " + e.Message);
                PromptResume(() => ChangePerms(session, file, to_set));
            }
        }

        public static void CreateDir(FtpClient session, String to_create)
        {
            try
            {
                // I had to add this regex because the CreateDirectory method chokes on directories located in the root.
                Regex regex = new Regex("\\A/[^/]+\\z");
                if (regex.IsMatch(to_create))
                {
                    FtpReply reply;
                    if (session.DirectoryExists(to_create))
                    {
                        return;
                    }
                    else if (!(reply = session.Execute("MKD " + to_create)).Success)
                    {
                        throw new FtpCommandException(reply);
                    }
                }
                else
                {
                    session.CreateDirectory(to_create);
                }
            }
            catch (FtpCommandException e)
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

        public static void RenameRemote(FtpClient session, String file, String new_name)
        {
            try
            {
                session.Rename(file, new_name);
            }
            catch (FtpCommandException e)
            {
                Console.WriteLine("Rename Failed: " + e.Message);
                PromptResume(() => RenameRemote(session, file, new_name));
            }
        }
    }
}