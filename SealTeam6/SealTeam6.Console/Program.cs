using System;
using SealTeam6.Core;
using System.Text.RegularExpressions;

namespace SealTeam6.Console
{
    class Program
    {
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
            Class1.LogOut(session);
            System.Console.ReadLine();
        }
    }
}