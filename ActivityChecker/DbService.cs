using Dapper;
using System.Data.SqlClient;

class DbService
{
    private readonly string connectionString;
    public DbService(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public SqlConnection DefaultConnection => new SqlConnection(connectionString);

    public int PublishScheduledArticles(System.Action<string> log)
    {
        var queryString = @"DELETE FROM AspNetUsers
                            WHERE EmailConfirmed = 0
                            AND DATEDIFF(
                            hour, EmailSendedOnRegister, GETDATE()) > 12";

        try
        {
            using (var conn = DefaultConnection)
            {
                conn.Open();
                log("Connection opened");
                var markResult = conn.Execute(queryString);
                log($"Command executed the query successfully with [{markResult}] results");
                return markResult;
            }
        }
        catch (System.Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            log($"ERROR: Connection failed with message:  {ex.Message} and stacktrace: {ex.StackTrace}");
            return 0;
        }
    }
}
