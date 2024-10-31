using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraDotNetCore.Shared;

namespace traDotNetCore.ConsoleApp
{
    public class AdoDotNetExample2
    {

        private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
        private readonly AdoDotNetService _dotNetService;

        public AdoDotNetExample2() { 
        
        
        _dotNetService = new AdoDotNetService(_connectionString);
        }


        public void Read() {
            string query = @"SELECT [BlogId]
    ,[BlogTitle]
    ,[BlogAuthor]
    ,[BlogContent]
    ,[DeleteFlag]
    FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
            DataTable dt = _dotNetService.Query(query);

            foreach (DataRow row in dt.Rows) {
                Console.Write(row["BlogId"]+"\t");
                Console.Write(row["BlogTitle"] + "\t");
                Console.Write(row["BlogAuthor" ] + "\t");
                Console.Write(row["BlogContent" ] + "\t");
                Console.WriteLine(row["DeleteFlag"]);

            }

        }

        public void CreateBlog()
        {

            string title = "", author = "", content = "";

            Console.Write("ENter New Title or Enter key for null ");

            title = Console.ReadLine();

            Console.Write("ENter New Author or Enter key for null ");

            author = Console.ReadLine();

            Console.Write("ENter New BlogContent or Enter key for null ");

            content = Console.ReadLine();

            string flag = "0";
            string queryInsert = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,@DeleteFlag)";
            int Result = _dotNetService.Excute(queryInsert,
                new SqlParameterModel ("@BlogTitle",title ),
                new SqlParameterModel { Name = "@BlogAuthor",Value = author },
                 new SqlParameterModel { Name = "@BlogContent", Value = content },
                 new SqlParameterModel { Name = "@DeleteFlag", Value = flag }

                );
            Console.WriteLine(Result > 0 ? "New Item Added Successfuly" : "No Item Added / Error query");
           // Console.WriteLine("New Item Added Successfuly");



        }

        public string ReadOne() {
            Console.WriteLine("Enter Blog ID");
            string id = Console.ReadLine();

                    string query = @"SELECT [BlogId]
            ,[BlogTitle]
            ,[BlogAuthor]
            ,[BlogContent]
            ,[DeleteFlag]
            FROM [dbo].[Tbl_Blog] where DeleteFlag = 0  and BlogId = @ID";

            DataTable dt = _dotNetService.Query(query,new SqlParameterModel ("@ID",id));
            if (dt.Rows.Count < 1) Console.WriteLine("No Item Found");
          else
            {
                Console.Write(dt.Rows[0]["BlogId"] + "\t");
                Console.Write(dt.Rows[0]["BlogTitle"] + "\t");
                Console.Write(dt.Rows[0]["BlogAuthor"] + "\t");
                Console.Write(dt.Rows[0]["BlogContent"] + "\t");
                Console.WriteLine(dt.Rows[0]["DeleteFlag"]);

            }
            return id;
        }

        public void EditBlog() {
            string id = ReadOne();
            string title = "", author = "", content = "";

            // Retrieve the existing data from the database
            string queryRead = "SELECT * FROM [dbo].[Tbl_Blog] WHERE BlogId = @BlogId";
            DataTable resultTable = _dotNetService.Query(queryRead, new SqlParameterModel("@BlogId", id ));

            if (resultTable.Rows.Count > 0)
            {
                DataRow row = resultTable.Rows[0];
                title = row["BlogTitle"].ToString();
                author = row["BlogAuthor"].ToString();
                content = row["BlogContent"].ToString();
            }

            // Prompt the user for new values, or use the old ones if no new input is given
            string temp;

            Console.Write("Enter New Title or press Enter to keep the current title: ");
            temp = Console.ReadLine();
            title = string.IsNullOrWhiteSpace(temp) ? title : temp;

            Console.Write("Enter New Author or press Enter to keep the current author: ");
            temp = Console.ReadLine();
            author = string.IsNullOrWhiteSpace(temp) ? author : temp;

            Console.Write("Enter New Blog Content or press Enter to keep the current content: ");
            temp = Console.ReadLine();
            content = string.IsNullOrWhiteSpace(temp) ? content : temp;

            // Default flag value
            string flag = "0";

            // Update the record in the database using the Excute() method
            string updateQuery = @"
        UPDATE [dbo].[Tbl_Blog]
        SET [BlogTitle] = @BlogTitle,
            [BlogAuthor] = @BlogAuthor,
            [BlogContent] = @BlogContent,
            [DeleteFlag] = @DeleteFlag
        WHERE BlogId = @BlogId";

            int rowsAffected = _dotNetService.Excute(updateQuery,
                new SqlParameterModel { Name = "@BlogTitle", Value = title },
                new SqlParameterModel { Name = "@BlogAuthor", Value = author },
                new SqlParameterModel { Name = "@BlogContent", Value = content },
                new SqlParameterModel { Name = "@DeleteFlag", Value = flag },
                new SqlParameterModel { Name = "@BlogId", Value = id }
            );

            if (rowsAffected > 0)
            {
                Console.WriteLine("Update successful.");
            }
            else
            {
                Console.WriteLine("No records were updated.");
            }

           
          


        }
        public void DeleteData()
        {
            string id = ReadOne();

            
            string query = "UPDATE Tbl_Blog SET DeleteFlag = 1 WHERE BlogId = @BlogId";

           
            int result = _dotNetService.Excute(query, new SqlParameterModel("@BlogId", id ));

           
            Console.WriteLine((result >= 1) ? "Delete Successfully" : "Failed to Delete");

           
        }
        public void InsertData()
        {
            string title, author, content;

           
            Console.Write("Enter New Title or press Enter to leave it null: ");
            title = Console.ReadLine();

            Console.Write("Enter New Author or press Enter to leave it null: ");
            author = Console.ReadLine();

            Console.Write("Enter New BlogContent or press Enter to leave it null: ");
            content = Console.ReadLine();

            
            string flag = "0";

            
            string queryInsert = @"
        INSERT INTO Tbl_Blog (BlogTitle, BlogAuthor, BlogContent, DeleteFlag) 
        VALUES (@BlogTitle, @BlogAuthor, @BlogContent, @DeleteFlag)";

           
            int result = _dotNetService.Excute(queryInsert,
                new SqlParameterModel("@BlogTitle", title),
                new SqlParameterModel("@BlogAuthor", author),
                new SqlParameterModel("@BlogContent", content),
                new SqlParameterModel("@DeleteFlag", flag)
            );

            
            Console.WriteLine((result >= 1) ? "Insert Successfully" : "Insertion Failed");

           
        }


    }
}
