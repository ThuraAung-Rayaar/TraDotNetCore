using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraDotNetCore.Shared
{
    public class DapperService
    {
        private readonly string _connectionString;
        public DapperService(string connection) { _connectionString = connection; }

        public List<T> Query<T>(string query, object? param = null) { 
        
            using IDbConnection connection = new SqlConnection(_connectionString);
            var queryList =  connection.Query<T>(query, param).ToList();
            return queryList;
        
        
        }
        public T QueryFirstOrDefault<T>(string query, object? param = null)
        {

            using IDbConnection connection = new SqlConnection(_connectionString);
            var queryItem = connection.QueryFirstOrDefault<T>(query, param);//.ToList();
            return queryItem;


        }

        public int Excute<T>(string query, object? param = null) {

            using IDbConnection connection = new SqlConnection(_connectionString);
            int result = connection.Execute(query, param);
            return result;
        }

    }
}
