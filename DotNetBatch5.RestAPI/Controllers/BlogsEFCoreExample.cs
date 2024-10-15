using DotNetBatch5.Database.Models;
using DotNetBatch5.RestAPI.DataModels;
using DotNetBatch5.RestAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DotNetBatch5.RestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogsEFCoreExample : ControllerBase
{
    [HttpGet]
    public IActionResult GetBlogs() {

         List<BlogViewModel> blogList = new List<BlogViewModel>();

        AppDbContext db = new AppDbContext();
       blogList = db.TblBlogs.Where(x=> x.DeleteFlag == false).Select(x => new BlogViewModel { 
        Id = x.BlogId,
        Title= x.BlogTitle,
        Content = x.BlogContent,
        Author = x.BlogAuthor
       
       }).ToList();
        
        
        return Ok(blogList); }

    [HttpGet("{id}")]
    public IActionResult GetBlog(int id)
    {

        

       
      
            AppDbContext db = new AppDbContext();
        var re = db.TblBlogs.Where(x => x.BlogId == id && x.DeleteFlag == false).FirstOrDefault();
        BlogViewModel blogItem = new BlogViewModel() { 
        Id = id,
        Author=re.BlogAuthor, Title=re.BlogTitle,Content=re.BlogContent
        };

        return Ok(blogItem);
    }

    [HttpPost]
    public IActionResult CreateBlog(BlogViewModel blog) {
        AppDbContext dbContext = new AppDbContext();
        TblBlog model = new TblBlog() { 
            BlogId = blog.Id,
            BlogAuthor = blog.Author,
            BlogContent = blog.Content,
            BlogTitle = blog.Title,
            DeleteFlag = false
        
        };
        dbContext.Add(model);
       int re = dbContext.SaveChanges();


        return Ok(re>0?"Success Adding new Blog":"Fail Some Error");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBlog(int id) {

        AppDbContext dbContext = new AppDbContext();
        //var re = dbContext.TblBlogs.Update()
        var item = dbContext.TblBlogs.Where(x=> x.BlogId == id).AsNoTracking().FirstOrDefault();
        if (item is null) return BadRequest("No Item Found");
        item.DeleteFlag = true;
        dbContext.Entry(item).State = EntityState.Modified;
        int re = dbContext.SaveChanges();

        return Ok("Item Deleted");
    }
    [HttpPut("{id}")]
    public IActionResult UpdateBlog(int id,BlogViewModel blog)
    {
        AppDbContext dbContext= new AppDbContext();
        var item = dbContext.TblBlogs.Where(x=> x.BlogId==id).AsNoTracking().FirstOrDefault();
        if (item is null) return BadRequest("No Item Found");
       // item.BlogId = blog.Id;
       if(!string.IsNullOrEmpty(blog.Title)) item.BlogTitle = blog.Title;
        if (!string.IsNullOrEmpty(blog.Author)) item.BlogAuthor = blog.Author;
        if (!string.IsNullOrEmpty(blog.Content)) item.BlogContent = blog.Content;
        dbContext.Entry(item).State= EntityState.Modified;
        dbContext.SaveChanges();

        return Ok(blog);
    }
}
