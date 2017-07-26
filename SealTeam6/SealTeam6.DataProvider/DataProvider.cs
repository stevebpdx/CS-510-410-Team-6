using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealTeam6.DataProvider
{
    public class DataProvider
    {
        public string ConnectStringName;

        public DataProvider()
        { }

        public DataProvider(string connectStringName)
        {
            this.ConnectStringName = connectStringName;
        }

        public void SaveOrUpdateConnection(Connection connection)
        {
            using (var db = new SealTeam6Entities())
            {
                var result = db.Connections.FirstOrDefault(c => c.UserID == connection.UserID && c.Uri == connection.Uri);
                if (result != null)
                {
                    result.UserID = connection.UserID;
                    result.Uri = connection.Uri;
                    result.Password = connection.Password;
                    result.Directory = connection.Directory;

                    db.SaveChanges();
                }
                else
                {
                    db.Connections.Add(connection);
                    db.SaveChanges();
                }
            }
        }



    }
}
