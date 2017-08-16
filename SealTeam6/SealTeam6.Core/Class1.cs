using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SealTeam6.Core
{
    public class Class1
    {
           
        public static bool ChangeFileName(FluentFTP.FtpClient fluentSession){


            System.Console.WriteLine("Please provide file name you would like to change:");
            string to_change = System.Console.ReadLine();

            if (!FileExists(fluentSession, to_change))
                return false;


            System.Console.WriteLine("Please enter what you would like to change your file name to:");
            string new_name = System.Console.ReadLine();

            fluentSession.Rename(to_change, new_name);

            return true;

        }
        

        public static bool DirectoryExists(FluentFTP.FtpClient fluentSession, string file_name)
        {

            try
            {
                fluentSession.DirectoryExists(file_name);
                return true;
            }

            catch (WebException e)
            {
                System.Console.WriteLine();
                System.Console.Write(e.Message);
            }


            return false;
        }


		public static bool FileExists(FluentFTP.FtpClient fluentSession, string file_name)
		{
            
			try
			{
				fluentSession.FileExists(file_name);
				return true;
			}

			catch (WebException e)
			{

				System.Console.WriteLine();
				System.Console.Write(e.Message);
			}

			return false;
		}


        public static bool DeletesDirectory(FluentFTP.FtpClient fluentSession)
        {
            
			System.Console.WriteLine("Please provide name of Directory you would like to delete");
			string directory_name = System.Console.ReadLine();

            //function to check if directory exists
            DirectoryExists(fluentSession, directory_name);

			try
			{
                fluentSession.DeleteDirectory(directory_name);
				return true;
			}

			catch (WebException e)
			{

				System.Console.WriteLine();
				System.Console.Write(e.Message);
			}


            return false;

		}

		public static bool DeletesFile(FluentFTP.FtpClient fluentSession)
		{

			System.Console.WriteLine("Please provide name of file you would like to delete");
			var file_name = System.Console.ReadLine();

            FileExists(fluentSession, file_name);

            try
            {
                fluentSession.DeleteFile(file_name);
                return true;
            }

            catch(WebException e){
                
				System.Console.WriteLine();
				System.Console.Write(e.Message);

            }

            return false;
		}
        
        
        
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

        public static void GetFiles(FluentFTP.FtpClient session, String directory, List<String> files)
        {
            try
            {
                session.DownloadFiles(directory, files);
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

        public static void RenameRemote(FluentFTP.FtpClient session, String file, String new_name)
        {
            try
            {
                session.Rename(file, new_name);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}
