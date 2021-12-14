using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPOSTWebUILibrary.DataAccess
{
    public class SqlNewsRepository
    {
        public static IConfigurationRoot _config { get; set; }

        public SqlNewsRepository(){ }

        private static string getConnectionString(string connName = "DefaultConnection")
        {
            _config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            var connString = _config.GetConnectionString(connName);
            return _config.GetConnectionString(connName);
        }

        public async Task<IEnumerable<T>> LoadData<T>(string sql)
        {
            using(IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return await cnn.QueryAsync<T>(sql);
            }
        }


        public async Task<IEnumerable<T>> LoadData<T>(string sql, DynamicParameters parameters)
        {
            using(IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return await cnn.QueryAsync<T>(sql,parameters);
            }
        }

        public async Task<int> SaveData(string sql)
        {
            using(IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return await cnn.ExecuteAsync(sql);
            }
        }

        public async Task<int> SaveData(string sql, DynamicParameters parameters)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return await cnn.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<IEnumerable<T>> ExecStoredProcedure<T>(string storedProcedureName, DynamicParameters param)
        {
            using (IDbConnection cnn = new SqlConnection(getConnectionString()))
            {
                return await cnn.QueryAsync<T>(storedProcedureName, param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
