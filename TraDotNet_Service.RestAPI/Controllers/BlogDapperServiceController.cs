using Dapper;
using DotNetBatch5.RestAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using traDotNetCore.ConsoleApp.Models;
using TraDotNetCore.Shared;

namespace TraDotNet_Service.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDapperServiceController : ControllerBase
    {

        private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
        private readonly DapperService _DapperService;
        public BlogDapperServiceController()
        {

            _DapperService = new DapperService(_connectionString);
        }




        [HttpGet]
        public IActionResult Get()
        {
            List<BlogViewModel> list = new List<BlogViewModel>();
            string query = @"SELECT [BlogId]
            ,[BlogTitle]
            ,[BlogAuthor]
            ,[BlogContent]
            ,[DeleteFlag]
            FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
            var temList = _DapperService.Query<DapperDataModel>(query);
            foreach (var item in temList)
            {

                list.Add(
                    new BlogViewModel
                    {

                        Id = item.BlogId,
                        Author = item.BlogAuthor,
                        Content = item.BlogContent,
                        Title = item.BlogTitle


                    });
            }



            return Ok(list);
        }




        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            //BlogViewModel item = new BlogViewModel();


            string query = $@"select * from [dbo].[Tbl_Blog] where BlogId = @ID";
            var item = _DapperService.QueryFirstOrDefault<DapperDataModel>(query, new { ID = id });

            if (item is null) return NotFound("No data Found");

            return Ok(new BlogViewModel
            {
                Id = id,
                Title = item.BlogTitle,
                Author = item.BlogAuthor,
                Content = item.BlogContent,

            });





        }


        [HttpPost]
        public IActionResult CreateBlog(BlogViewModel model)
        {
            //int result;


            string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";
            int result = _DapperService.Excute<DapperDataModel>(query,
               new
               {
                   BlogTitle = model.Title,
                   BlogAuthor = model.Author,
                   BlogContent = model.Content

               }

                );









            return Ok(result > 0 ? "Success Creating Blog" : "Fail & Error in Query or Code");
        }


        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogViewModel model)
        {
            int result;


            string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET [BlogTitle] = @BlogTitle
                ,[BlogAuthor] = @BlogAuthor
                ,[BlogContent] =@BlogContent
                
                 WHERE BlogId = @BlogId";

            result = _DapperService.Excute<DapperDataModel>(query,
           new
           {
               BlogId = model.Id,
               BlogTitle = model.Title,
               BlogAuthor = model.Author,
               BlogContent = model.Content

           }

            );

            // if (result<1)  return NotFound();




            return (result > 0) ? Ok("Update Successful") : NotFound("Id not Found Error");

            // return Ok();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            int result;


            string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET 
                [DeleteFlag] = 1
                 WHERE BlogId = @BlogId and DeleteFlag = 0";
            result = _DapperService.Excute<DapperDataModel>(query, new { BlogId = id });



            return (result > 0) ? Ok("Delete SuccessFul") : NotFound("Id not Found Error");
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogViewModel model)
        {
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

            string query = $@"UPDATE [dbo].[Tbl_Blog]
        SET {condition} , [DeleteFlag] = 0
        WHERE BlogId = @ID";
            result = _DapperService.Excute<DapperDataModel>(query, new
            {
                ID = id,
                BlogTitle = model.Title,
                BlogAuthor = model.Author,
                BlogContent = model.Content


            });





            return (result > 0) ? Ok("Update Successful") : NotFound("Id not Found Error");
        }


    }
}