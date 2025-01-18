using DotNetBatch5.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogServices.Domain.Feature;

public class BlogService
{
    private readonly AppDbContext _dbContext;

    public BlogService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

  
    public List<TblBlog> GetBlogs()
    {
        var lst = _dbContext.TblBlogs.ToList();
        return lst;
    }

    
    public TblBlog? GetBlog(int id)
    {

        return _dbContext.TblBlogs.FirstOrDefault(b => b.BlogId == id);
    }

   
    public int CreateBlog(TblBlog model)
    {
        _dbContext.TblBlogs.Add(model);
       var result =  _dbContext.SaveChanges();
        return result;
    }

   
    public int DeleteBlog(int id)
    {
        var blog = _dbContext.TblBlogs.FirstOrDefault(b => b.BlogId == id);
        if (blog == null)
        {
            return 0; 
        }

        _dbContext.TblBlogs.Remove(blog);
        var result = _dbContext.SaveChanges();
        return result;
    }
     public int  UpdateBlog(int id, TblBlog blog)
    {
        // Fetch the existing blog using AsNoTracking to avoid tracking issues
        var existingBlog = _dbContext.TblBlogs
            .AsNoTracking()
            .FirstOrDefault(x => x.BlogId == id);

        // Return null if the blog is not found
        if (existingBlog is null)
        {
            return 0;
        }

        // Update the properties of the existing blog
        existingBlog.BlogTitle = blog.BlogTitle;
        existingBlog.BlogAuthor = blog.BlogAuthor;
        existingBlog.BlogContent = blog.BlogContent;
        

        // Mark the entity as modified and save changes
        _dbContext.Entry(existingBlog).State = EntityState.Modified;
        

        // Return the updated blog
        return _dbContext.SaveChanges();
    }
}