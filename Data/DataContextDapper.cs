using System.Data;
using Dapper;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace HelloWorld.Data
{
    public class DataContextDapper
    {
        private IConfiguration _config;
        private string _connectionString = "";

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<T> LoadData<T>(string sqlCommand)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Query<T>(sqlCommand);
        }

        public T LoadDataSingle<T>(string sqlCommand)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.QuerySingle<T>(sqlCommand);
        }

        public bool ExecuteSql(string sqlCommand)
        {
            return ExecuteSqlWithRowCount(sqlCommand) > 0;
        }

        public int ExecuteSqlWithRowCount(string sqlCommand)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sqlCommand);
        }
    }
}