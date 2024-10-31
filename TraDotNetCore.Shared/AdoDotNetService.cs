using System.Data;
using System.Data.SqlClient;

namespace TraDotNetCore.Shared
{
    public class AdoDotNetService
    {
        private readonly string _connectionString;

        public AdoDotNetService(string connection) { _connectionString = connection; }


        

        public DataTable Query(string query, params SqlParameterModel[] parameterModels  ) { 
        
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            if (parameterModels is not null)
            {
                foreach (var model in parameterModels)
                {

                    command.Parameters.AddWithValue(model.Name, model.Value);

                }
            }
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            
            connection.Close();
            return dt;     
        
        }


        public int Excute(string query, params SqlParameterModel[] parameterModels)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            if (parameterModels is not null)
            {
                foreach (var model in parameterModels)
                {

                    command.Parameters.AddWithValue(model.Name, model.Value);

                }
            }
            connection.Open();
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }


    }


    public class SqlParameterModel {


        public SqlParameterModel() { }
       public string Name { get; set; }
       public string Value { get; set; }
    
    public SqlParameterModel(string name, string val) {
        Name = name; Value = val; 
        }
    
    }

}
