using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace traDotNetCore.ConsoleApp
{
    public class adoDotnetExample
    {
        string connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;";
        public void readTable() {
            
            Console.WriteLine("Connecting String: " + connectionString);
            SqlConnection connection = new SqlConnection(connectionString);
            Console.WriteLine("Opening sql Connection");
            connection.Open();
            Console.WriteLine("Sql Connection OPened");

            string query = @"SELECT [BlogId]
    ,[BlogTitle]
    ,[BlogAuthor]
    ,[BlogContent]
    ,[DeleteFlag]
    FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
            SqlCommand cmd = new SqlCommand(query, connection);

            //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //adapter.Fill(dt);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                Console.Write(reader["BlogId"] + "\t");
                Console.Write(reader["BlogTitle"] + "\t");
                Console.Write(reader["BlogAuthor"] + "\t");
                Console.Write(reader["BlogContent"] + "\t");
                Console.WriteLine(reader["DeleteFlag"] + "\t");


            }


            /*foreach (DataRow vr in dt.Rows)
            {
                Console.WriteLine(vr["BlogId"]);
                Console.WriteLine(vr["BlogTitle"]);
                Console.WriteLine(vr["BlogAuthor"]);
                Console.WriteLine(vr["BlogContent"]);
                Console.WriteLine(vr["DeleteFlag"]);
            }*/


            Console.WriteLine("Closing sql Connection");
            connection.Close();
            Console.WriteLine("Sql Connection closed");

         //   Console.ReadKey();


            /*foreach (DataRow vr in dt.Rows)
            {
                Console.Write(vr["BlogId"]+"\t");
                Console.Write(vr["BlogTitle"+"\t"]);
                Console.Write(vr["BlogAuthor"+"\t"]);
                Console.Write(vr["BlogContent"+"\t"]);
                Console.Write(vr["DeleteFlag"+"\t"]);
            }*/

        }

        public string ReadOnly() {

            Console.Write("Select ID:");
            string id = Console.ReadLine();
           // string title = "", author = "", content = "";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string queryRead = $@"select * from [dbo].[Tbl_Blog] where BlogId = @ID";

            SqlCommand cmdRead = new SqlCommand(queryRead, connection);
            cmdRead.Parameters.AddWithValue("@ID", id);
            SqlDataReader reader = cmdRead.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                Console.Write(reader["BlogTitle"] + "\t");
                Console.Write(reader["BlogAuthor"] + "\t");
                Console.Write(reader["BlogContent"] + "\n");
                
            }

            Console.WriteLine("Closing sql Connection");
            connection.Close();
            return id;
           // Console.ReadKey();
        }


        public void DeleteData() {

            string id = ReadOnly();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $@"Update Tbl_Blog set DeleteFlag = 1 where BlogId = {id} ";
            SqlCommand cmdDelete = new SqlCommand(query,connection);
            cmdDelete.Parameters.AddWithValue("@ID", id);
            int result = cmdDelete.ExecuteNonQuery();
            Console.WriteLine((result >= 1) ? "Delete SuccessFully" : "Fail to Delete");
            connection.Close();
            Console.ReadKey();



        }

        public void InsertData() {
            string title = "", author = "", content = "";
            //string temp;
            Console.Write("ENter New Title or Enter key for null ");
           /* temp = Console.ReadLine();
            title = string.Equals(temp, "") ? title : temp;*/
            title = Console.ReadLine();

            Console.Write("ENter New Author or Enter key for null ");
            /*temp = Console.ReadLine();
            author = string.Equals(temp, "") ? author : temp;*/
             author = Console.ReadLine();

            Console.Write("ENter New BlogContent or Enter key for null ");
            /*temp = Console.ReadLine();
            content = string.Equals(temp, "") ? content : temp;*/
             content = Console.ReadLine();

            string flag = "0";


            
            //Console.WriteLine("Connecting String: " + connectionString);
            SqlConnection connection = new SqlConnection(connectionString);
            // Console.WriteLine("Opening sql Connection");
            connection.Open();
          
            string queryInsert = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,@DeleteFlag)";
            SqlCommand cmdInsert = new SqlCommand(queryInsert, connection);
            cmdInsert.Parameters.AddWithValue("@BlogTitle", title);
            cmdInsert.Parameters.AddWithValue("@BlogAuthor", author);
            cmdInsert.Parameters.AddWithValue("@BlogContent", content);
            cmdInsert.Parameters.AddWithValue("@DeleteFlag", flag);
            /*SqlDataAdapter adapter = new SqlDataAdapter(cmdInsert);
            DataTable dt = new DataTable();
            adapter.Fill(dt);*/

            int result = cmdInsert.ExecuteNonQuery();


            Console.WriteLine((result  >= 1)? "Insert SuccessFully":"Fail INsertion");


            connection.Close();
            Console.ReadKey();



        }


        public void Update() {

           // Console.Write("Select ID to Update :");
            string id = ReadOnly();// from above function to get one item
            string title = "", author = "", content = "";
           // Console.WriteLine("Connecting String: " + connectionString);
            SqlConnection connection = new SqlConnection(connectionString);
           // Console.WriteLine("Opening sql Connection");
            connection.Open();
           // Console.WriteLine("Sql Connection OPened");

            string queryRead = $@"select * from [dbo].[Tbl_Blog] where BlogId ='{id}'";

            SqlCommand cmdRead = new SqlCommand(queryRead,connection);
            SqlDataReader reader = cmdRead.ExecuteReader();
           
            if (reader.HasRows) {
                reader.Read();
                title = reader["BlogTitle"].ToString();
                author = reader["BlogAuthor"].ToString();
                content = reader["BlogContent"].ToString();

            }

            //Console.WriteLine("Closing sql Connection");
            connection.Close();
            //Console.WriteLine("Sql Connection closed");


            string temp;

            Console.Write("ENter New Title or Enter key for default ");
            temp = Console.ReadLine();
            title = string.Equals(temp,"")?title: temp;

            Console.Write("ENter New Author or Enter key for default ");
            temp = Console.ReadLine();
            author = string.Equals(temp, "") ? author : temp;
           // author = Console.ReadLine();

            Console.Write("ENter New BlogContent or Enter key for default ");
            temp = Console.ReadLine();
            content = string.Equals(temp, "") ? content : temp;
           // content = Console.ReadLine();

            string flag = "0";

            Console.WriteLine("Connecting String: " + connectionString);
           // SqlConnection connection = new SqlConnection(connectionString);
            Console.WriteLine("Opening sql Connection");
            connection.Open();
            Console.WriteLine("Sql Connection OPened");

            string query = $@"UPDATE [dbo].[Tbl_Blog]
        SET [BlogTitle] = @BlogTitle
      ,[BlogAuthor] = @BlogAuthor
      ,[BlogContent] =@BlogContent
      ,[DeleteFlag] =@DeleteFlag
        WHERE BlogId = '{id}'";

            SqlCommand cmdUpdate = new SqlCommand(query, connection);
            cmdUpdate.Parameters.AddWithValue("@BlogTitle", title);
            cmdUpdate.Parameters.AddWithValue("@BlogAuthor", author);
            cmdUpdate.Parameters.AddWithValue("@BlogContent", content);
            cmdUpdate.Parameters.AddWithValue("@DeleteFlag", flag);
            /*DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmdUpdate);
            adapter.Fill(dt);*/
            cmdUpdate.ExecuteNonQuery();

            //Console.WriteLine("Closing sql Connection");
            connection.Close();
           // Console.WriteLine("Sql Connection closed");
           Console.ReadKey();
            readTable();

        }

    }
}
