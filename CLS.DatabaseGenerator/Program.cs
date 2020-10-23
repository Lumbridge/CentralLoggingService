using System;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using CLS.Infrastructure.Helpers;

namespace CLS.DatabaseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["cls"].ToString();
            RunSqlScript(connectionString, "./Scripts/CreateCLSTables.sql");
            RunSqlScript(connectionString, "./Scripts/PopulateStaticCLSData.sql");
            Console.WriteLine("Done running scripts...");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void RunSqlScript(string connectionString, string scriptPath)
        {
            var script = string.Empty;

            try
            {
                script = File.ReadAllText(scriptPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetExceptionMessages());
            }

            using (var conn = new SqlConnection(connectionString))
            {
                var server = new Server(new ServerConnection(conn));

                try
                {
                    server.ConnectionContext.ExecuteNonQuery(script);
                    Console.WriteLine($"Successfully executed {scriptPath}...");
                }
                catch (ExecutionFailureException ex)
                {
                    var innerEx = ex.InnerException;
                    if (innerEx is SqlException)
                    {
                        var sqlEx = ex.InnerException as SqlException;
                        if (sqlEx?.State == 6 && sqlEx.Class == 16)
                        {
                            Console.WriteLine("One or more tables already exist in the target database, skipping script...");
                        }
                    }
                    else
                    {
                        Console.WriteLine(ex.GetExceptionMessages());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetExceptionMessages());
                }
            }
        }
    }
}
