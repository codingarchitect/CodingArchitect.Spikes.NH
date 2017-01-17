using NHibernate.Connection;
using SqlLocalDb;
using System.Data;

namespace CodingArchitect.Spikes.NH.Persistance
{
    public class SqlLocalDbConnectionProvider : DriverConnectionProvider
    {
        public override IDbConnection GetConnection()
        {
            LocalDatabase database = SqlLocalDbInstanceProvider.Instance;
            var connection = database.GetConnection();
            return connection;
        }
    }
}
