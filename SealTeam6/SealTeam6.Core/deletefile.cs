using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SealTeam6.Core
{
    public class class2
    {
        public static bool delete_file(Uri file_to_delete)
        {

            if (file_to_delete.Scheme != Uri.UriSchemeFtp)
            {
                Console.Write("Not a valid file representation");
                return false;

            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(file_to_delete);
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            }

            catch(WebException m)
            {

                Console.Write(m.message);

            }

        }







    }
}
