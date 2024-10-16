using DotNetBatch5.RestAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using traDotNetCore.ConsoleApp.Models;
using Dapper;
using System.Reflection.Metadata;
namespace DotNetBatch5.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsDapperController : ControllerBase
    {
        private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";

        [HttpGet]
        public IActionResult GetBlogs() {

            List<BlogViewModel> lst = new List<BlogViewModel>();
            using (IDbConnection connection = new SqlConnection(_connectionString)) { 
                    string query = @"SELECT [BlogId]
    ,[BlogTitle]
    ,[BlogAuthor]
    ,[BlogContent]
    ,[DeleteFlag]
    FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
                var temlist = connection.Query<DapperDataModel>(query).ToList();
                foreach (var item in temlist) {

                    lst.Add(new BlogViewModel
                    {
                        Id = item.BlogId,
                        Title = item.BlogTitle,
                        Author = item.BlogAuthor,
                        Content = item.BlogContent,
                                       });
                }


            }


            return Ok(lst);
        }



        [HttpGet("{id}")]
        public IActionResult GetBlog(int id) { 
            //BlogViewModel item = new BlogViewModel();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string query = $@"select * from [dbo].[Tbl_Blog] where BlogId = @ID";
                var item = connection.Query<DapperDataModel>(query,new {ID = id }).FirstOrDefault();

                if(item is  null ) return NotFound("No data Found");

               return Ok(new BlogViewModel { 
                    Id=id,
                   Title = item.BlogTitle,
                   Author = item.BlogAuthor,
                   Content = item.BlogContent,

               });

            }


                
        }

        [HttpPost]
        public IActionResult CreateBlog(BlogViewModel model) {
            int result;
            using (IDbConnection connection = new SqlConnection(_connectionString)) {

                string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";
                 result = connection.Execute(query, new
                {
                    BlogTitle = model.Title,
                    BlogAuthor = model.Author,
                    BlogContent = model.Content

                });
               



            }



                return Ok(result >0? "Success Creating Blog":"Fail & Error in Query or Code");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogViewModel blog) {
            int result;
            using (IDbConnection connection = new SqlConnection(_connectionString)) {

                string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET [BlogTitle] = @BlogTitle
                ,[BlogAuthor] = @BlogAuthor
                ,[BlogContent] =@BlogContent
                
                 WHERE BlogId = @BlogId";

                result = connection.Execute(query, new {
                    BlogId = id,
                    BlogTitle = blog.Title,

                    BlogAuthor = blog.Author,
                    BlogContent = blog.Content
                });

               // if (result<1)  return NotFound();


            }

            return (result > 0)?   Ok("Update Successful") : NotFound("Id not Found Error");

           // return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id) {
            int result;
            using (IDbConnection connection = new SqlConnection(_connectionString)) {

                string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET 
                [DeleteFlag] = 1
                 WHERE BlogId = @BlogId and DeleteFlag = 0";
                 result = connection.Execute(query, new
                {
                    BlogId = id

                });

            }

            return (result > 0) ? Ok("Delete SuccessFul") : NotFound("Id not Found Error");
        }
        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogViewModel model) {
            string condition = "";
            if (!string.IsNullOrEmpty(model.Title))
            {
                condition += " [BlogTitle] = @BlogTitle, ";



            }
            if (!string.IsNullOrEmpty(model.Author))
            {
                condition += " [BlogAuthor] = @BlogAuthor, ";



            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                condition += " [BlogContent] = @BlogContent, ";



            }
            if (condition.Length == 0) return NotFound("Error Handing PArameter");

            condition = condition.Substring(0, condition.Length - 2);
            int result;
            using (IDbConnection connection = new SqlConnection(_connectionString)) {

                string query = $@"UPDATE [dbo].[Tbl_Blog]
        SET {condition}
        WHERE BlogId = @ID";
                result = connection.Execute(query, new
                {
                    ID = id,
                    BlogTitle = model.Title,
                    BlogAuthor = model.Author,
                    BlogContent = model.Content


                });


            }


            return (result > 0) ? Ok("Update Successful") : NotFound("Id not Found Error");
        }
    }
}
