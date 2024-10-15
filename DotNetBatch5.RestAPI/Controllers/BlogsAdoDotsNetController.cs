using DotNetBatch5.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetBatch5.RestAPI.DataModels;
using DotNetBatch5.RestAPI.ViewModels;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace DotNetBatch5.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsAdoDotsNetController : ControllerBase
    {

       private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
        [HttpGet]
        public IActionResult GetBlogs()
        {
            List<BlogViewModel> lst = new List<BlogViewModel>();

            SqlConnection connection = new SqlConnection(_connectionString);
            string query = @"SELECT [BlogId]
    ,[BlogTitle]
    ,[BlogAuthor]
    ,[BlogContent]
    ,[DeleteFlag]
    FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                lst.Add(new BlogViewModel {
                    Id = Convert.ToInt32(reader["BlogId"]),
                Title = reader["BlogTitle"].ToString(),
                Author = reader["BlogAuthor"].ToString(),
                Content = reader["BlogContent"].ToString(),
                DeleteFlag = Convert.ToBoolean( reader["DeleteFlag"] )});
            }

            connection.Close();

            return Ok(lst);
        }


        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            BlogViewModel item = new BlogViewModel();
            SqlConnection connection = new SqlConnection(_connectionString);
            string query = $@"select * from [dbo].[Tbl_Blog] where BlogId = @ID";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                item = new BlogViewModel
                {
                    Id = Convert.ToInt32(reader["BlogId"]),
                    Title = reader["BlogTitle"].ToString(),
                    Author = reader["BlogAuthor"].ToString(),
                    Content = reader["BlogContent"].ToString(),
                    DeleteFlag = Convert.ToBoolean(reader["DeleteFlag"])
                };

            }
            connection.Close();

            return Ok(item);
        }
        [HttpPost]
        public IActionResult CreateBlogs(BlogViewModel blog)
        {

            BlogViewModel lst = new BlogViewModel();
            SqlConnection connection = new SqlConnection(_connectionString);
            string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BlogTitle", blog.Title);
            command.Parameters.AddWithValue("@BlogAuthor", blog.Author);
            command.Parameters.AddWithValue("@BlogContent", blog.Content);
           // command.Parameters.AddWithValue("@DeleteFlag", blog.DeleteFlag);
          // int ii = command.ExecuteNonQuery();


            return Ok(blog);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateBlogs(int id, BlogViewModel blog)
        {
            BlogViewModel lst = new BlogViewModel();
            SqlConnection connection = new SqlConnection(_connectionString);

            string query = $@"select * from [dbo].[Tbl_Blog] where BlogId =@ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            connection.Open();
            
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows) return NotFound("No data Found");
            //reader.Read();
            connection.Close();

           string query2 = $@"UPDATE [dbo].[Tbl_Blog]
        SET [BlogTitle] = @BlogTitle
      ,[BlogAuthor] = @BlogAuthor
      ,[BlogContent] =@BlogContent
      ,[DeleteFlag] =@DeleteFlag
        WHERE BlogId = @ID";

            command = new SqlCommand(query2, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@BlogTitle", blog.Title);
            command.Parameters.AddWithValue("@BlogAuthor", blog.Author);
            command.Parameters.AddWithValue("@BlogContent", blog.Content);
            command.Parameters.AddWithValue("@DeleteFlag", blog.DeleteFlag);
            connection.Open();
           int ii = command.ExecuteNonQuery();
            connection.Close();

            return Ok($"Row effected: {ii}");
        }


        [HttpPatch("{id}")]
        public IActionResult PatchBlogs(int id, BlogViewModel blog)
        {
            

            SqlConnection connection = new SqlConnection(_connectionString);
            string con = "";
            if (!string.IsNullOrEmpty(blog.Title))
            {
                con += " [BlogTitle] = @BlogTitle, ";
              
            }
            if (!string.IsNullOrEmpty(blog.Author))
            {
                con += " [BlogAuthor] = @BlogAuthor, ";
               
            }
            if (!string.IsNullOrEmpty(blog.Content))
            {
                con += " [BlogContent] = @BlogContent, ";
               
            }
            if (con.Length == 0) { return NotFound("Error Handling Query / Invalid parameter"); }
            con = con.Substring(0, con.Length - 2);
            string query = $@"Update [dbo].[Tbl_Blog] set {con} WHERE BlogId = @ID";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            if (!string.IsNullOrEmpty(blog.Title)) {
               
                command.Parameters.AddWithValue("@BlogTitle", blog.Title);
                 }
            if (!string.IsNullOrEmpty(blog.Author))
            {
             
                command.Parameters.AddWithValue("@BlogAuthor", blog.Author);
            }
            if (!string.IsNullOrEmpty(blog.Content))
            {
               
                command.Parameters.AddWithValue("@BlogContent", blog.Content);
            }
           
            
          
            
           
           int re = command.ExecuteNonQuery();
            connection.Close();



            return Ok(re>0?"Success update":"Fail");
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBlogs(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            string query = $@"Update Tbl_Blog set DeleteFlag = 1 where BlogId = @ID ";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            connection.Open();
            int ii = command.ExecuteNonQuery();
            connection.Close();
            string re = (ii>0)?"Item found and deleted" : "Item not found";

            return Ok(re);
        }

    }
}
