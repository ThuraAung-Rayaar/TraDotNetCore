using DotNetBatch5.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using TraDotNetCore.Shared;

namespace DotNetCore.DependencyInjection.TRA.RestApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsAdoDotNetInjection : ControllerBase
    {

       
        private readonly AdoDotNetService _AdoDotNetService;

        public BlogsAdoDotNetInjection(IConfiguration config)
        {
            _AdoDotNetService = new AdoDotNetService(config.GetConnectionString("DbConnection")!);
        }
        [HttpGet]
        public IActionResult GetBlog()
        {
            List<TblBlog> B_list = new List<TblBlog>();
            string query = @"SELECT [BlogId]
            ,[BlogTitle]
            ,[BlogAuthor]
            ,[BlogContent]
            ,[DeleteFlag]
            FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
            var BlogTable = _AdoDotNetService.Query(query);
            foreach (DataRow dr in BlogTable.Rows)
            {

                B_list.Add(new TblBlog
                {
                    BlogId = Convert.ToInt32(dr["BlogId"]),
                    BlogTitle = Convert.ToString(dr["BlogTitle"]),
                    BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
                    BlogContent = Convert.ToString(dr["BlogContent"]),
                    DeleteFlag = Convert.ToBoolean(dr["DeleteFlag"]),


                });
            }

            return Ok(B_list);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlogById(int id)
        {
            TblBlog item;// = new BlogViewModel();
            string query = @"SELECT [BlogId]
            ,[BlogTitle]
            ,[BlogAuthor]
            ,[BlogContent]
            ,[DeleteFlag]
            FROM [dbo].[Tbl_Blog] where DeleteFlag = 0  and BlogId = @ID";
            DataTable table = _AdoDotNetService.Query(query, new SqlParameterModel("@ID", id.ToString()));
            if (table.Rows.Count == 0) return NotFound();

            item = new TblBlog()
            {
                BlogId = id,
                BlogTitle = table.Rows[0]["BlogTitle"].ToString(),
                BlogAuthor = table.Rows[0]["BlogAuthor"].ToString(),
                BlogContent = table.Rows[0]["BlogContent"].ToString(),

                DeleteFlag = Convert.ToBoolean(table.Rows[0]["DeleteFlag"])

            };

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {

            string query = $@"Update Tbl_Blog set DeleteFlag = 1 where BlogId = @ID ";
            int result = _AdoDotNetService.Excute(query, new SqlParameterModel("@ID", id.ToString()));

            return Ok((result > 0) ? "Item found and deleted" : "Item not found");
        }

        [HttpPost]
        public IActionResult CreateBlog(TblBlog model)
        {

            string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";
            int result = _AdoDotNetService.Excute(query,
                new SqlParameterModel("@BlogTitle", model.BlogTitle),
                new SqlParameterModel("@BlogAuthor", model.BlogAuthor),
                new SqlParameterModel("@BlogContent", model.BlogContent)
                );

            return Ok((result > 0) ? "Item Created" : "Error");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, TblBlog model)
        {

            string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET [BlogTitle] = @BlogTitle
              ,[BlogAuthor] = @BlogAuthor
              ,[BlogContent] =@BlogContent
              ,[DeleteFlag] =@DeleteFlag
                WHERE BlogId = @ID";
            int result = _AdoDotNetService.Excute(query,
                new SqlParameterModel("@ID", id.ToString()),
               new SqlParameterModel("@BlogTitle", model.BlogTitle),
                new SqlParameterModel("@BlogAuthor", model.BlogAuthor),
                new SqlParameterModel("@BlogContent", model.BlogContent),
                 new SqlParameterModel("@DeleteFlag", model.DeleteFlag.ToString())

                );


            return (result > 0) ? Ok("Updated") : NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, TblBlog blog)
        {

            List<SqlParameterModel> sqlPara = new List<SqlParameterModel>();
            //sqlPara = new SqlParameterModel[]();

            string condition = "";
            if (!string.IsNullOrEmpty(blog.BlogTitle))
            {
                condition += " [BlogTitle] = @BlogTitle, ";
                sqlPara.Add(new SqlParameterModel("Title", blog.BlogTitle));


            }
            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            {
                condition += " [BlogAuthor] = @BlogAuthor, ";
                sqlPara.Add(new SqlParameterModel("Author", blog.BlogAuthor));


            }
            if (!string.IsNullOrEmpty(blog.BlogContent))
            {
                condition += " [BlogContent] = @BlogContent, ";
                sqlPara.Add(new SqlParameterModel("@BlogContent", blog.BlogContent));


            }
            if (condition.Length == 0) return NotFound("Error Handing PArameter");

            condition = condition.Substring(0, condition.Length - 2);
            string query = $@"UPDATE [dbo].[Tbl_Blog]
        SET {condition}
        WHERE BlogId = @ID";

            sqlPara.Add(new SqlParameterModel("@ID", id.ToString()));

            int result = _AdoDotNetService.Excute(query,
                   sqlPara.ToArray()

                    );


            return (result > 0) ? Ok("Updated") : NotFound();
        }


    }
}
