using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traDotNetCore.ConsoleApp.Models;

namespace traDotNetCore.ConsoleApp
{
    public class DapperExample
    {
        private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;";

        public void Read()
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                string query = "select * from Tbl_Blog where DeleteFlag = 0 ";
                var TemList = db.Query<DapperDataModel>(query).ToList();

                foreach (var item in TemList)
                {
                    Console.WriteLine($"{item.BlogId}    {item.BlogTitle}    {item.BlogAuthor}   {item.BlogContent}");
                    //Console.WriteLine(item);    


                }

            }
        }

        public void ReadOnly()
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                Console.Write("Enter Blog ID "); string id = Console.ReadLine();
                string query = "select * from Tbl_Blog where DeleteFlag = 0 and BlogId = @BlogID ";
                var item = db.Query<DapperDataModel>(query, new
                {
                    BlogID = id
                }).FirstOrDefault();

                if (item is not null)
                { Console.WriteLine($"{item.BlogId}    {item.BlogTitle}    {item.BlogAuthor}   {item.BlogContent}"); }

                else {
                    Console.WriteLine("No Item Found");
                }
                    //Console.WriteLine(item);    


                

            }
        }

        public void Create(string Title, string Author, string Content)
        {
            string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                int result = db.Execute(query, new
                {
                    BlogTitle = Title,
                    BlogAuthor = Author,
                    BlogContent = Content

                });

                Console.WriteLine(result >= 1 ? "Insertion Succcssful" : "Fail Insertion");

            }


        }

        public void Update(string Title, string Author, string Content)
        {
            Console.Write("Enter Blog ID To Update"); string id = Console.ReadLine();
            string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET [BlogTitle] = @BlogTitle
                ,[BlogAuthor] = @BlogAuthor
                ,[BlogContent] =@BlogContent
                
                 WHERE BlogId = @BlogId";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                int result = db.Execute(query, new
                {
                    BlogTitle = Title,
                    BlogAuthor = Author,
                    BlogContent = Content,
                    BlogId = id
                });

                Console.WriteLine(result >= 1 ? "Update Succcssful" : "Fail Update");

            }


        }
        public void Delete(int id)
        {
            // Console.Write("Enter Blog ID To Update"); string id = Console.ReadLine();
            string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET 
                [DeleteFlag] = 1
                 WHERE BlogId = @BlogId and DeleteFlag = 0";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                int result = db.Execute(query, new
                {
                    BlogId = id

                });

                Console.WriteLine(result >= 1 ? "Delete Succcssful" : "Fail Delete");
                Console.ReadKey();

            }


        }

    }
}
