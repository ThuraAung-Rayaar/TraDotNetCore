using DotNetBatch5.RestAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using TraDotNetCore.Shared;

namespace TraDotNet_Service.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNetServiceController : ControllerBase
    {

        private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
        private readonly AdoDotNetService _AdoDotNetService;
        public BlogAdoDotNetServiceController()
        {

            _AdoDotNetService = new AdoDotNetService(_connectionString);
        }

        [HttpGet]
        public IActionResult GetBlog()
        {
            List<BlogViewModel> B_list = new List<BlogViewModel>();
            string query = @"SELECT [BlogId]
            ,[BlogTitle]
            ,[BlogAuthor]
            ,[BlogContent]
            ,[DeleteFlag]
            FROM [dbo].[Tbl_Blog] where DeleteFlag = 0 ";
            var BlogTable = _AdoDotNetService.Query(query);
            foreach (DataRow dr in BlogTable.Rows)
            {

                B_list.Add(new BlogViewModel
                {
                    Id = Convert.ToInt32(dr["BlogId"]),
                    Title = Convert.ToString(dr["BlogTitle"]),
                    Author = Convert.ToString(dr["BlogAuthor"]),
                    Content = Convert.ToString(dr["BlogContent"]),
                    DeleteFlag = Convert.ToBoolean(dr["DeleteFlag"]),


                });
            }

            return Ok(B_list);
        }


        [HttpGet("{id}")]
        public IActionResult GetBlogById(int id)
        {
            BlogViewModel item;// = new BlogViewModel();
            string query = @"SELECT [BlogId]
            ,[BlogTitle]
            ,[BlogAuthor]
            ,[BlogContent]
            ,[DeleteFlag]
            FROM [dbo].[Tbl_Blog] where DeleteFlag = 0  and BlogId = @ID";
            DataTable table = _AdoDotNetService.Query(query, new SqlParameterModel("@ID", id.ToString()));
            if (table.Rows.Count == 0) return NotFound();

            item = new BlogViewModel()
            {
                Id = id,
                Title = table.Rows[0]["BlogTitle"].ToString(),
                Author = table.Rows[0]["BlogAuthor"].ToString(),
                Content = table.Rows[0]["BlogContent"].ToString(),

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
        public IActionResult CreateBlog(BlogViewModel model)
        {

            string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";
            int result = _AdoDotNetService.Excute(query,
                new SqlParameterModel("@BlogTitle", model.Title),
                new SqlParameterModel("@BlogAuthor", model.Author),
                new SqlParameterModel("@BlogContent", model.Content)
                );

            return Ok((result > 0) ? "Item Created" : "Error");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogViewModel model)
        {

            string query = $@"UPDATE [dbo].[Tbl_Blog]
                SET [BlogTitle] = @BlogTitle
              ,[BlogAuthor] = @BlogAuthor
              ,[BlogContent] =@BlogContent
              ,[DeleteFlag] =@DeleteFlag
                WHERE BlogId = @ID";
            int result = _AdoDotNetService.Excute(query,
                new SqlParameterModel("@ID", id.ToString()),
               new SqlParameterModel("@BlogTitle", model.Title),
                new SqlParameterModel("@BlogAuthor", model.Author),
                new SqlParameterModel("@BlogContent", model.Content),
                 new SqlParameterModel("@DeleteFlag", model.DeleteFlag.ToString())

                );


            return (result > 0) ? Ok("Updated") : NotFound();
        }

        [HttpPatch("{id}")]

        public IActionResult PatchBlog(int id, BlogViewModel blog)
        {

            List<SqlParameterModel> sqlPara = new List<SqlParameterModel>();
            //sqlPara = new SqlParameterModel[]();

            string condition = "";
            if (!string.IsNullOrEmpty(blog.Title))
            {
                condition += " [BlogTitle] = @BlogTitle, ";
                sqlPara.Add(new SqlParameterModel("Title", blog.Title));


            }
            if (!string.IsNullOrEmpty(blog.Author))
            {
                condition += " [BlogAuthor] = @BlogAuthor, ";
                sqlPara.Add(new SqlParameterModel("Author", blog.Author));


            }
            if (!string.IsNullOrEmpty(blog.Content))
            {
                condition += " [BlogContent] = @BlogContent, ";
                sqlPara.Add(new SqlParameterModel("@BlogContent", blog.Content));


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