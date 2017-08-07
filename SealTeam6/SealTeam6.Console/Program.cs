using System;
using SealTeam6.Core;
using System.Text.RegularExpressions;
using System.IO;

namespace SealTeam6.Console
{
    class Program
    {
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

        public static String PromptDirectory(FluentFTP.FtpClient session, bool enforce)
        {
            String directory = PromptHelper("Directory");
            if (enforce)
            {
                while (directory == null || !session.DirectoryExists(directory))
                {
                    System.Console.WriteLine("Invalid directory.");
                    directory = PromptHelper("Directory");
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
            FluentFTP.FtpClient session = null;
            while (choice != "q")
            {
                while (session == null)
                {
                    String host = PromptHost();
                    String username = PromptString("Username", true);
                    String password = PromptPassword();
                    session = Class1.LogIn(host, username, password);
                }
                System.Console.WriteLine();
                System.Console.WriteLine("Local Operations:");
                System.Console.WriteLine("Remote Operations:");
                System.Console.WriteLine("1. Log Out");
                System.Console.WriteLine("2. List the contents of a directory");
                System.Console.WriteLine("3. Get File");
                System.Console.WriteLine("Enter q to quit the program.");
                choice = PromptString("Choice", true);
                System.Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        Class1.LogOut(session);
                        session = null;
                        break;
                    case "2":
                        String directory = PromptDirectory(session, true);
                        ListRemote(session, directory);
                        break;
                    case "3":
                        System.Console.WriteLine("Warning: If the local file already exists, then it will be overwritten.");
                        String local = PromptFile(session, false, "Local");
                        String remote = PromptFile(session, true, "Remote");
                        Class1.GetFile(session, local, remote);
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
                Class1.LogOut(session);
            }
        }
    }
}