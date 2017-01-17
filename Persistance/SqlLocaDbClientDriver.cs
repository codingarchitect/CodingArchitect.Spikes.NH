using NHibernate.Driver;
using SqlLocalDb;
using System;
using System.Data;

namespace CodingArchitect.Spikes.NH.Persistance
{
    public sealed class SqlLocalDbClientDriver : DriverBase
    {
        private readonly LocalDatabase database = SqlLocalDbInstanceProvider.Instance;
        private IDbConnection connection;
        public override IDbConnection CreateConnection()
        {
            connection = database.GetConnection();
            connection.Close();
            Console.WriteLine(connection.State);
            return connection;
        }

        public override IDbCommand CreateCommand()
        {
            connection = database.GetConnection();
            return connection.CreateCommand();
        }

        public override bool UseNamedPrefixInSql
        {
            get { return true; }
        }
        public override bool UseNamedPrefixInParameter
        {
            get { return true; }
        }
        public override string NamedPrefix
        {
            get { return "@"; }
        }
    }
}
