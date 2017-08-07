using System;
using SealTeam6.Core;
using System.Text.RegularExpressions;

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
            System.Console.Write("Directory: ");
            String directory = System.Console.ReadLine();
            if (enforce)
            {
                while (directory == null || !session.DirectoryExists(directory))
                {
                    System.Console.WriteLine("Invalid directory.");
                    System.Console.Write("Directory: ");
                    directory = System.Console.ReadLine();
                }
            }
            return directory;
        }

        public static String PromptFile(FluentFTP.FtpClient session, bool enforce)
        {
            System.Console.Write("File: ");
            String file = System.Console.ReadLine();
            if (enforce)
            {
                while (file == null || !session.FileExists(file))
                {
                    System.Console.WriteLine("Invalid file.");
                    System.Console.Write("File: ");
                    file = System.Console.ReadLine();
                }
            }
            return file;
        }

        public static String PromptHost()
        {
            System.Console.Write("Host: ");
            String host = System.Console.ReadLine();
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
                System.Console.Write("Host: ");
                host = System.Console.ReadLine();
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
            System.Console.Write(name + ": ");
            String response = System.Console.ReadLine();
            if (enforce)
            {
                while (response == null || response == "")
                {
                    System.Console.WriteLine("Invalid response.");
                    System.Console.Write(name + ": ");
                    response = System.Console.ReadLine();
                }
            }
            return response;
        }

        static void Main(string[] args)
        {
            String host = PromptHost();
            String username = PromptString("Username", true);
            String password = PromptPassword();
            var session = Class1.LogIn(host, username, password);
            String directory = PromptDirectory(session, true);
            ListRemote(session, directory);
            Class1.LogOut(session);
            System.Console.ReadLine();
        }
    }
}