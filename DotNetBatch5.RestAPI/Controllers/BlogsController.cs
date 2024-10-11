using DotNetBatch5.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetBatch5.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
       private readonly AppDbContext _dbContext = new AppDbContext();


        [HttpGet]
        public IActionResult GetBlogs() {
            var lst = _dbContext.TblBlogs.AsNoTracking().Where(x=>x.DeleteFlag == false)
                .ToList();

            return Ok(lst);
        }


        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            var item = _dbContext.TblBlogs.AsNoTracking().FirstOrDefault(x=>x.BlogId == id);
            if (item is null) { 
            
            return NotFound();
            }

            return Ok(item);
        }
        [HttpPost]
        public IActionResult CreateBlogs(TblBlog blog) { 
            _dbContext.TblBlogs.Add(blog);
            _dbContext.SaveChanges();

            
            return Ok(blog); }


        [HttpPut("{id}")]
        public IActionResult UpdateBlogs(int id, TblBlog blog) { 
            var item = _dbContext.TblBlogs.AsNoTracking().FirstOrDefault(x=> x.BlogId == id);
            if (item is null) { return NotFound(); }

            item.BlogTitle = blog.BlogTitle;
            item.BlogAuthor = blog.BlogAuthor;
            item.BlogContent = blog.BlogContent;
            _dbContext.Entry(item).State = EntityState.Modified;
            _dbContext.SaveChanges();

            
            return Ok(); }


        [HttpPatch("{id}")]
        public IActionResult PatchBlogs(int id, TblBlog blog) {
            var item = _dbContext.TblBlogs.AsNoTracking().FirstOrDefault(x => x.BlogId == id);
            if (item is null) { return NotFound(); }

            if (!string.IsNullOrEmpty(blog.BlogTitle)) { item.BlogTitle = blog.BlogTitle; }
            if (!string.IsNullOrEmpty(blog.BlogAuthor)) {  item.BlogAuthor=blog.BlogAuthor;}
            if (!string.IsNullOrEmpty(blog.BlogContent)) {  item.BlogContent = blog.BlogContent;}

            _dbContext.Entry(item).State= EntityState.Modified;
            _dbContext.SaveChanges();

            return Ok(item); }


        [HttpDelete("{id}")]
        public IActionResult DeleteBlogs(int id) {


            var item = _dbContext.TblBlogs.AsNoTracking().FirstOrDefault(x => x.BlogId == id);
            if (item is null) { return NotFound(); }
            item.DeleteFlag = true;

            _dbContext.Entry(item).State = EntityState.Modified;
            //_dbContext.Entry(item).State = EntityState.Deleted;
            _dbContext.SaveChanges();


            return Ok();}






    }
}
