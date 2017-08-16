using System;
using SealTeam6.Core;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace SealTeam6.Console
{
    class Program
    {
        public static void GetFiles(FluentFTP.FtpClient session)
        {
            System.Console.WriteLine("Warning: If the local file already exists, then it will be overwritten.");
            int count = PromptInt("File Count");
            if (count == 1)
            {
                String local = PromptFile(session, false, "Local");
                String remote = PromptFile(session, true, "Remote");
                SealTeam6FTP.GetFile(session, local, remote);
            }
            else if (count > 1)
            {
                String directory = PromptDirectory(session, true, "Local");
                List<String> files = new List<String>();
                for (int i = 0; i < count; ++i)
                {
                    files.Add(PromptFile(session, true, "Remote"));
                }
                SealTeam6FTP.GetFiles(session, directory, files);
            }
            else
            {
                System.Console.WriteLine("Invalid count.");
            }
        }

        public static void ListLocal(String directory)
        {
            String[] list = Directory.GetFileSystemEntries(directory);
            System.Console.WriteLine("Date       Time     <DIR> or Bytes Name");
            foreach (String item in list)
            {
                FileInfo info = new FileInfo(item);
                System.Console.Write(info.LastWriteTime.GetDateTimeFormats()[56] + " ");
                if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    System.Console.Write("<DIR>          ");
                }
                else
                {
                    String size = info.Length.ToString();
                    String spaces = new String(' ', (14 - size.Length));
                    System.Console.Write(spaces + size + " ");
                }
                System.Console.WriteLine(info.Name);
            }
        }

        public static void ListRemote(FluentFTP.FtpClient session, String directory)
        {
            FluentFTP.FtpListItem[] list = session.GetListing(directory);
            System.Console.WriteLine("Date       Time     <DIR> or Bytes Name");
            foreach (var item in list)
            {
                System.Console.Write(item.Modified.GetDateTimeFormats()[56] + " ");
                if (item.Type == FluentFTP.FtpFileSystemObjectType.Directory)
                {
                    System.Console.Write("<DIR>          ");
                }
                else
                {
                    String size = item.Size.ToString();
                    String spaces = new String(' ', (14 - size.Length));
                    System.Console.Write(spaces + size + " ");
                }
                System.Console.WriteLine(item.Name);
            }
        }

        public static String PromptDirectory(FluentFTP.FtpClient session, bool enforce, String system)
        {
            String directory = PromptHelper(system + " Directory");
            if (enforce)
            {
                if (system == "Local")
                {
                    while (directory == null || !Directory.Exists(directory))
                    {
                        System.Console.WriteLine("Invalid directory.");
                        directory = PromptHelper(system + " Directory");
                    }
                }
                else
                {
                    while (directory == null || !session.DirectoryExists(directory))
                    {
                        System.Console.WriteLine("Invalid directory.");
                        directory = PromptHelper(system + " Directory");
                    }
                }
            }
            else
            {
                while (directory == null || directory == "")
                {
                    System.Console.WriteLine("Invalid directory.");
                    directory = PromptHelper(system + " Directory");
                }
            }
            return directory;
        }

        public static String PromptFile(FluentFTP.FtpClient session, bool enforce, String system)
        {
            String file = PromptHelper(system + " File");
            if (enforce)
            {
                if (system == "Local")
                {
                    while (file == null || !File.Exists(file))
                    {
                        System.Console.WriteLine("Invalid file.");
                        file = PromptHelper(system + " File");
                    }
                }
                else
                {
                    while (file == null || !session.FileExists(file))
                    {
                        System.Console.WriteLine("Invalid file.");
                        file = PromptHelper(system + " File");
                    }
                }
            }
            else
            {
                while (file == null || file == "")
                {
                    System.Console.WriteLine("Invalid file.");
                    file = PromptHelper(system + " File");
                }
            }
            return file;
        }

        private static String PromptHelper(String name)
        {
            System.Console.Write(name + ": ");
            return System.Console.ReadLine();
        }

        public static String PromptHost()
        {
            String host = PromptHelper("Host");
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
                System.Console.WriteLine("Invalid host. (Valid characters: letters, numbers and periods)");
                host = PromptHelper("Host");
            }
            return host;
        }

        public static int PromptInt(String name)
        {
            String response = PromptHelper(name);
            int result;
            while (!Int32.TryParse(response, out result))
            {
                System.Console.WriteLine("Invalid integer.");
                response = PromptHelper(name);
            }
            return result;
        }

        public static String PromptPassword()
        {
            System.Console.Write("Password: ");
            String password = "";
            char key = System.Console.ReadKey(true).KeyChar;
            while (key != (char)System.Environment.NewLine.ToCharArray()[0])
            {
                password += key;
                System.Console.Write("*");
                key = System.Console.ReadKey(true).KeyChar;
            }
            System.Console.WriteLine();
            return password;
        }

        public static String PromptString(String name, bool enforce)
        {
            String response = PromptHelper(name);
            if (enforce)
            {
                while (response == null || response == "")
                {
                    System.Console.WriteLine("Invalid response.");
                    response = PromptHelper(name);
                }
            }
            return response;
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Welcome to Team Six's command line FTP client.");
            System.Console.WriteLine("Contributors: Steve Braich, Devan Cakebread, Victor Ochia, Patrick Overton and");
            System.Console.WriteLine("Barend Venter");
            System.Console.WriteLine();
            String choice = "";

            //Allocate registers for tracking user input with the state machine
            String directory;
            String file;
            String new_name;

            FluentFTP.FtpClient session = null;
            while (choice != "q")
            {
                while (session == null)
                {
                    String host = PromptHost();
                    String username = PromptString("Username", true);
                    String password = PromptPassword();
                    session = SealTeam6FTP.LogIn(host, username, password);
                }
                System.Console.WriteLine();
                System.Console.WriteLine("Local Operations:");
                System.Console.WriteLine("1. List the contents of a directory");
                System.Console.WriteLine("2. Rename File");
                System.Console.WriteLine("Remote Operations:");
                System.Console.WriteLine("3. Log Out");
                System.Console.WriteLine("4. List the contents of a directory");
                System.Console.WriteLine("5. Get File(s)");
                System.Console.WriteLine("6. Rename a File");
                System.Console.WriteLine("7. Change Permissions on a File");
                System.Console.WriteLine("8. Create Directory");
                System.Console.WriteLine("Enter q to quit the program.");
                choice = PromptString("Choice", true);
                System.Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        directory = PromptDirectory(session, true, "Local");
                        ListLocal(directory);
                        break;
                    case "2":
                        System.Console.WriteLine("Note: This operation cannot be used to move files.");
                        file = PromptFile(session, true, "Local");
                        new_name = PromptString("New Name", true);
                        SealTeam6FTP.RenameLocal(file, new_name);
                        break;
                    case "3":
                        SealTeam6FTP.LogOut(session);
                        session = null;
                        break;
                    case "4":
                        directory = PromptDirectory(session, true, "Remote");
                        ListRemote(session, directory);
                        break;
                    case "5":
                        GetFiles(session);
                        break;
                    case "6":
                        file = PromptFile(session, true, "Remote");
                        new_name = PromptString("New Name", true);
                        SealTeam6FTP.RenameRemote(session, file, new_name);
                        break;
                    case "7":
                        file = PromptFile(session, true, "Remote");
                        int to_Set = PromptInt("Permissions to set (ex. 777):");
                        SealTeam6FTP.ChangePerms(session, file, to_Set);
                        break;
                    case "8":
                        directory = PromptString("Path to create:", true);
                        SealTeam6FTP.CreateDir(session, directory);
                        break;
                    case "q":
                        break;
                    default:
                        System.Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            if (session != null)
            {
                SealTeam6FTP.LogOut(session);
            }
        }
    }
}