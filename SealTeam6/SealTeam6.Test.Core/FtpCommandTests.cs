using Microsoft.VisualStudio.TestTools.UnitTesting;
using SealTeam6.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SealTeam6.Core.Tests
{
    [TestClass()]
    public class SealTeam6ClientTests
    {
        string server = "spinach.dankmeme.com";
        string user = "stevebpdx";
        string password = "cs410510";

        [TestMethod()]
        public void LogInTest()
        {
            var session = Class1.LogIn(server, user, password);

            Assert.IsTrue(session.IsConnected);
        }

        [TestMethod()]
        public void LogOutTest()
        {
            var session = Class1.LogIn(server, user, password);
            Class1.LogOut(session);

            Assert.IsFalse(session.IsConnected);
        }

        [TestMethod()]
        public void GetFileTest()
        {
            string localpath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\test1.txt";
            string remotepath = @"/files/test1.txt";
            var session = Class1.LogIn(server, user, password);

            // Setup
            try { File.Delete(localpath); } catch { } // swallow exeption
            try { session.DeleteFile(remotepath); } catch { } // swallow exeption
            File.WriteAllText(localpath, "test1");
            session.UploadFile(localpath, remotepath);
            try { File.Delete(localpath); } catch { } // swallow exeption

            Core.Class1.GetFile(session, localpath, remotepath);

            Int64 fileSizeInBytes = new FileInfo(localpath).Length;
            Assert.IsTrue(fileSizeInBytes == 5);
        }

        [TestMethod()]
        public void GetFilesTest()
        {
            // Steve Braich, 16 Aug 2017
            // The following code is just using the brute force method of creating 3 test files 
            // uploading them to the FTP server, deleting them from local, downloading them, 
            // then testing that they successfully downloaded.
            // It's ugly and this particular test is my full responsibility.

            string localdir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string localpath1 = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\test1.txt";
            string localpath2 = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\test2.txt";
            string localpath3 = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\test3.txt";
            string remotepath1 = @"/files/test1.txt";
            string remotepath2 = @"/files/test2.txt";
            string remotepath3 = @"/files/test3.txt";
            List<String> files = new List<String>(new String[] { remotepath1, remotepath2, remotepath3 });
            var session = Class1.LogIn(server, user, password);

            // Setup
            try { File.Delete(localpath1); } catch { } // swallow exeption
            try { File.Delete(localpath2); } catch { } // swallow exeption
            try { File.Delete(localpath3); } catch { } // swallow exeption
            try { session.DeleteFile(remotepath1); } catch { } // swallow exeption
            try { session.DeleteFile(remotepath2); } catch { } // swallow exeption
            try { session.DeleteFile(remotepath3); } catch { } // swallow exeption
            File.WriteAllText(localpath1, "test1");
            File.WriteAllText(localpath2, "test2");
            File.WriteAllText(localpath3, "test3");
            session.UploadFile(localpath1, remotepath1);
            session.UploadFile(localpath2, remotepath2);
            session.UploadFile(localpath3, remotepath3);
            try { File.Delete(localpath1); } catch { } // swallow exeption
            try { File.Delete(localpath2); } catch { } // swallow exeption
            try { File.Delete(localpath3); } catch { } // swallow exeption

            Core.Class1.GetFiles(session, localdir, files);

            Int64 fileSizeInBytes1 = new FileInfo(localpath1).Length;
            Int64 fileSizeInBytes2 = new FileInfo(localpath2).Length;
            Int64 fileSizeInBytes3 = new FileInfo(localpath3).Length;
            Assert.IsTrue(fileSizeInBytes1 == 5);
            Assert.IsTrue(fileSizeInBytes2 == 5);
            Assert.IsTrue(fileSizeInBytes3 == 5);
        }

        [TestMethod()]
        public void RenameRemoteTest()
        {
            string localpath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\old.txt";
            string remotepath = @"/files/old.txt";
            string renamepath = @"/files/new.txt";
            var session = Class1.LogIn(server, user, password);

            // Setup
            try { File.Delete(localpath); } catch { } // swallow exeption
            try { session.DeleteFile(remotepath); } catch { } // swallow exeption
            try { session.DeleteFile(renamepath); } catch { } // swallow exeption
            File.WriteAllText(localpath, "rename");
            session.UploadFile(localpath, remotepath);
            try { File.Delete(localpath); } catch { } // swallow exeption

            Core.Class1.RenameRemote(session, remotepath, renamepath);
            Assert.IsTrue(session.FileExists(renamepath));
        }

        [TestMethod()]
        public void RenameLocalTest()
        {
            string localpath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\old.txt";
            string renamepath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\new.txt";
            var session = Class1.LogIn(server, user, password);

            // Setup
            try { File.Delete(localpath); } catch { } // swallow exeption
            try { File.Delete(renamepath); } catch { } // swallow exeption
            File.WriteAllText(localpath, "rename");

            Core.Class1.RenameLocal(localpath, renamepath);
            Assert.IsTrue(File.Exists(renamepath));
        }
    }
}