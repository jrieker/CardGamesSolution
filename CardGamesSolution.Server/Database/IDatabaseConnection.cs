using Microsoft.Data.SqlClient;

namespace CardGamesSolution.Server.Database
{
    public interface IDatabaseConnection
    {
        SqlConnection GetConnection();
    }
}