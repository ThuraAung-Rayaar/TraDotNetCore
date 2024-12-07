using DotNetBatch5.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraDotNetCore.Shared;

namespace DotNetCore.DependencyInjection.TRA.RestApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDapperInjection : ControllerBase
    {
       
        private readonly DapperService _DapperService;
        public BlogDapperInjection(IConfiguration config)
        {
            _DapperService = new DapperService(config.GetConnectionString("DbConnection")!);
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<TblBlog> list = new List<TblBlog>();
            string query = @"SELECT [BlogId]
            ,[BlogTitle]
            ,[BlogAuthor]
            ,[BlogContent]
            ,[DeleteFlag]
            FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
            var temList = _DapperService.Query<TblBlog>(query);
            foreach (var item in temList)
            {

                list.Add(
                    new TblBlog
                    {

                        BlogId = item.BlogId,
                        BlogAuthor = item.BlogAuthor,
                        BlogContent = item.BlogContent,
                        BlogTitle = item.BlogTitle


                    });
            }



            return Ok(list);
        }




        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            //BlogViewModel item = new BlogViewModel();


            string query = $@"select * from [dbo].[Tbl_Blog] where BlogId = @ID";
            var item = _DapperService.QueryFirstOrDefault<TblBlog>(query, new { ID = id });

            if (item is null) return NotFound("No data Found");

            return Ok(new TblBlog
            {
                BlogId = item.BlogId,
                BlogAuthor = item.BlogAuthor,
                BlogContent = item.BlogContent,
                BlogTitle = item.BlogTitle

            });





        }


        [HttpPost]
        public IActionResult CreateBlog(TblBlog model)
        {
            //int result;


            string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";
            int result = _DapperService.Excute<TblBlog>(query,
               new
               {
                  
                
                BlogAuthor = model.BlogAuthor,
                BlogContent = model.BlogContent,
                BlogTitle = model.BlogTitle


               }

                );









            return Ok(result > 0 ? "Success Creating Blog" : "Fail & Error in Query or Code");
        }


        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, TblBlog model)
        {
            int result;


            string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET [BlogTitle] = @BlogTitle
                ,[BlogAuthor] = @BlogAuthor
                ,[BlogContent] =@BlogContent
                
                 WHERE BlogId = @BlogId";

            result = _DapperService.Excute<TblBlog>(query,
           new
           {
               BlogId = model.BlogId,

               BlogAuthor = model.BlogAuthor,
               BlogContent = model.BlogContent,
               BlogTitle = model.BlogTitle

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
            result = _DapperService.Excute<TblBlog>(query, new { BlogId = id });



            return (result > 0) ? Ok("Delete SuccessFul") : NotFound("Id not Found Error");
        }



    }
}
