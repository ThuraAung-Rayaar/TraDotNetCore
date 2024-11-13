using DotNetBatch5.RestAPI.ViewModels;
using DotNetCoreTraining.Domain.Features.Blogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraDotNetCore.Shared;

namespace DotNetBatch5.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogServices_AdoDotNet_Controller : ControllerBase
    {
        private readonly BlogServices _blogService;
        public BlogServices_AdoDotNet_Controller() {

            _blogService = new BlogServices();
        }

        [HttpGet]
        public IActionResult GetBlogs()
        {
            List<BlogViewModel> lst = new List<BlogViewModel>();

            string query = @"SELECT [BlogId]
    ,[BlogTitle]
    ,[BlogAuthor]
    ,[BlogContent]
    ,[DeleteFlag]
    FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
           
           var serviceList = _blogService.GetBlogs(query);
           
            lst = serviceList.Select(x => new BlogViewModel { 
            Id=x.BlogId,
            Title=x.BlogTitle,
            Author = x.BlogAuthor,
            Content = x.BlogContent,
            DeleteFlag = x.DeleteFlag
            
            }).ToList();

          

            return Ok(lst);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {

            string query = $@"Update Tbl_Blog set DeleteFlag = 1 where BlogId = @ID ";
            int result = _blogService.DeleteBlog(query,id);

            return Ok((result > 0) ? "Item found and deleted" : "Item not found");
        }

        //[HttpPost]
        //public IActionResult CreateBlog(BlogViewModel model)
        //{


        //    string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";
        //    var result = _blogService.CreateBlog(query,model,
        //        new SqlParameterModel("@BlogTitle", model.Title),
        //        new SqlParameterModel("@BlogAuthor", model.Author),
        //        new SqlParameterModel("@BlogContent", model.Content)
        //        );

        //    return Ok((result > 0) ? "Item Created" : "Error");
        //}

    }
}
