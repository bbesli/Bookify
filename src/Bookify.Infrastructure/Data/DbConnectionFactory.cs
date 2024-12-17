using Bookify.Application.Abstractions.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Data
{
    internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
    {
        public IDbConnection GetOpenConnection()
        {
            NpgsqlConnection connection = dataSource.OpenConnection();

            return connection;
        }
    }
}
