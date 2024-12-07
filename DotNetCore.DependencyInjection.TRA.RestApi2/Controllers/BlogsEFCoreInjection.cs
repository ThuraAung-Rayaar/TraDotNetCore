using DotNetBatch5.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.DependencyInjection.TRA.RestApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsEFCoreInjection : ControllerBase
    {
        private readonly AppDbContext db;

        public BlogsEFCoreInjection(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetBlogs()
        {

            var blogList = db.TblBlogs.Where(x => x.DeleteFlag == false).ToList();


            return Ok(blogList);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {





           
            var item = db.TblBlogs.Where(x => x.BlogId == id && x.DeleteFlag == false).FirstOrDefault();
            if (item is null) return BadRequest("No ID Found");
           

            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBlog(TblBlog blog)
        {
            AppDbContext dbContext = new AppDbContext();
           
            db.Add(blog);
            int re = db.SaveChanges();


            return Ok(re > 0 ? "Success Adding new Blog" : "Fail Some Error");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {

           
            //var re = dbContext.TblBlogs.Update()
            var item = db.TblBlogs.Where(x => x.BlogId == id).AsNoTracking().FirstOrDefault();
            if (item is null) return BadRequest("No Item Found");
            item.DeleteFlag = true;
            db.Entry(item).State = EntityState.Modified;
            int re = db.SaveChanges();

            return Ok("Item Deleted");
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateBlog(int id, TblBlog blog)
        {
            AppDbContext dbContext = new AppDbContext();
            var item = dbContext.TblBlogs.Where(x => x.BlogId == id).AsNoTracking().FirstOrDefault();
            if (item is null) return BadRequest("No Item Found");
            // item.BlogId = blog.Id;
            if (!string.IsNullOrEmpty(blog.BlogTitle)) item.BlogTitle = blog.BlogTitle;
            if (!string.IsNullOrEmpty(blog.BlogAuthor)) item.BlogAuthor = blog.BlogAuthor;
            if (!string.IsNullOrEmpty(blog.BlogContent)) item.BlogContent = blog.BlogContent;
            dbContext.Entry(item).State = EntityState.Modified;
            dbContext.SaveChanges();

            return Ok(blog);
        }

        //[HttpPatch("{id}")]
        //public IActionResult PatchBlog(int id, TblBlog blog)
        //{


        //    AppDbContext dbContext = new AppDbContext();
        //    var item = dbContext.TblBlogs.AsNoTracking().Where(x => x.BlogId == id).FirstOrDefault();
        //    if (item is null) return NotFound("No ID Found");
        //    if (!string.IsNullOrEmpty(blog.Title))
        //    {

        //        item.BlogTitle = blog.Title;


        //    }
        //    if (!string.IsNullOrEmpty(blog.Author))
        //    {
        //        item.BlogAuthor = blog.Author;



        //    }
        //    if (!string.IsNullOrEmpty(blog.Content))
        //    {
        //        item.BlogContent = blog.Content;



        //    }
        //    dbContext.Entry(item).State = EntityState.Modified;
        //    dbContext.SaveChanges();
        //    return Ok("Succcessful Update");
        //}
    }
}
