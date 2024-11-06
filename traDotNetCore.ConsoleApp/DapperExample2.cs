using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traDotNetCore.ConsoleApp.Models;
using TraDotNetCore.Shared;
namespace traDotNetCore.ConsoleApp
{
    public class DapperExample2
    {
        private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
        private readonly DapperService _dapperService;
            public DapperExample2() { 
            _dapperService = new DapperService(_connectionString);
        
        }


        public void ReadBlog() { 
        
       string query = "select * from Tbl_Blog where DeleteFlag = 0 ";

            var TemList = _dapperService.Query<DapperDataModel>(query);
            foreach (var item in TemList)
            {
                Console.WriteLine($"{item.BlogId}    {item.BlogTitle}    {item.BlogAuthor}   {item.BlogContent}");
                //Console.WriteLine(item);    


            }


        }



        public void ReadOnly()
        {

            
                Console.Write("Enter Blog ID "); string id = Console.ReadLine();
                string query = "select * from Tbl_Blog where DeleteFlag = 0 and BlogId = @BlogID ";
                var item = _dapperService.QueryFirstOrDefault<DapperDataModel>(query,new {BlogID = id});

                if (item is not null)
                { Console.WriteLine($"{item.BlogId}    {item.BlogTitle}    {item.BlogAuthor}   {item.BlogContent}"); }

                else
                {
                    Console.WriteLine("No Item Found");
                }
                //Console.WriteLine(item);    




            
        }
        public void Create(string Title, string Author, string Content)
        {
            string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";


            int result = _dapperService.Excute<DapperDataModel>(query,
                new {
                    BlogTitle = Title,
                    BlogAuthor = Author,
                    BlogContent = Content

                }
                
                );

                Console.WriteLine(result >= 1 ? "Insertion Succcssful" : "Fail Insertion");
            
            


        }

        public void Update(string Title, string Author, string Content)
        {
            Console.Write("Enter Blog ID To Update"); string id = Console.ReadLine();
            string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET [BlogTitle] = @BlogTitle
                ,[BlogAuthor] = @BlogAuthor
                ,[BlogContent] =@BlogContent
                
                 WHERE BlogId = @BlogId";


            int result = _dapperService.Excute<DapperDataModel>(query,
                new
                {
                    BlogId = id,
                    BlogTitle = Title,
                    BlogAuthor = Author,
                    BlogContent = Content

                }
                
                );

                Console.WriteLine(result >= 1 ? "Update Succcssful" : "Fail Update");

            


        }

        public void DeleteItem() {
            Console.WriteLine("Enter Id to DELETE");
            string id = Console.ReadLine();
            string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET 
                [DeleteFlag] = 1
                 WHERE BlogId = @BlogId and DeleteFlag = 0";

            int result = _dapperService.Excute<DapperDataModel>(query, new { BlogId = id });


        }



    }
}
