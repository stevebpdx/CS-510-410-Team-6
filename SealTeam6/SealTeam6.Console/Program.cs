using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SealTeam6.Core;

namespace SealTeam6.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Class1();
            app.LogIn();
            app.ListRemote("/",true);
        }
    }
}
