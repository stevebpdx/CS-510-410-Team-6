using System;
using System.Net;

namespace SealTeam6.Core
{
    public class Class1
    {
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
    }
}