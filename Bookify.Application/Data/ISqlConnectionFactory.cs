using System.Data;

namespace Bookify.Application.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}