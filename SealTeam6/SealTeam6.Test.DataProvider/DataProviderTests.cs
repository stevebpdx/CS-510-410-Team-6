using Microsoft.VisualStudio.TestTools.UnitTesting;
using SealTeam6.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealTeam6.DataProvider.Tests
{
    [TestClass()]
    public class DataProviderTests
    {
        [TestMethod()]
        public void SaveOrUpdateConnectionTest()
        {
            Connection c = new Connection();
            c.UserID = "test user";
            c.Password = "123";
            c.Uri = "ftp://server.com";
            c.Directory = "/test";

            DataProvider data = new DataProvider("dbContext");
            //DataProvider data = new DataProvider();
            data.SaveOrUpdateConnection(c);

            Assert.IsNotNull(c.ConnectionID);
            Assert.IsTrue(c.ConnectionID > 0);
        }
    }
}