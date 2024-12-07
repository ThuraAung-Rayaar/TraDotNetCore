using DotNetBatch5.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.DependencyInjection.TRA.RestApi2.EndPoints;

public static class BlogEndPointInjection
{

    public static void UseBlogEndPoint(this IEndpointRouteBuilder app) { 
    
    app.MapGet("/Blogs", ([FromServices] AppDbContext appDb) => {

        
        var listt = appDb.TblBlogs.ToList();
        return Results.Ok(listt);
    }).WithName("GetBlogs").WithOpenApi();




        app.MapGet("/Blogs/{id}", ([FromServices] AppDbContext appDb,int id) => {

           // AppDbContext appDb = new AppDbContext();
            var item = appDb.TblBlogs.Where(x => x.BlogId == id && x.DeleteFlag == false).FirstOrDefault();
            return item is null ? Results.NotFound("Error ? NOt Found") : Results.Ok(item);
            //return Results.Ok(listt);
        }).WithName("GetBlog").WithOpenApi();

        app.MapPost("/Blogs", ([FromServices] AppDbContext appDb, TblBlog blog) => {
           // AppDbContext appDb = new AppDbContext();
            blog.DeleteFlag = false;
            appDb.Add(blog);
            appDb.SaveChanges();
            return Results.Ok("Success");


        }).WithName("PostBlog").WithOpenApi();

        app.MapPut("/Blogs/{id}", ([FromServices] AppDbContext appDb, int id, TblBlog blog) => {
           // AppDbContext appDb = new AppDbContext();

            var item = appDb.TblBlogs.AsNoTracking().Where(x => x.BlogId == id && x.DeleteFlag == false).FirstOrDefault();
            if (item is null) Results.NotFound("Error ? NOt Found");//: {

            item.BlogId = id;
            item.BlogTitle = blog.BlogTitle;
            item.BlogContent = blog.BlogContent;
            item.BlogAuthor = blog.BlogAuthor;
            item.DeleteFlag = blog.DeleteFlag;

            appDb.Entry(item).State = EntityState.Modified;

            //appDb.Add(blog);
            appDb.SaveChanges();
            return Results.Ok("Success");


        }).WithName("EditBlog").WithOpenApi();

        app.MapDelete("/Blogs/{id}", ([FromServices] AppDbContext appDb, int id) => {


            //AppDbContext appDb = new AppDbContext();
            var item = appDb.TblBlogs.AsNoTracking().Where(x => x.BlogId == id && x.DeleteFlag == false).FirstOrDefault();
            if (item is null) Results.NotFound("Error ? NOt Found");
            item.DeleteFlag = true;
            appDb.Entry(item).State = EntityState.Modified;
            appDb.SaveChanges();
            return Results.Ok("Item Deleted");
        });
        app.MapPatch("/Blogs", ([FromServices] AppDbContext appDb, int id, TblBlog blog) =>
        {
            //AppDbContext appDb = new AppDbContext();

            var item = appDb.TblBlogs.AsNoTracking().Where(x => x.BlogId == id && x.DeleteFlag == false).FirstOrDefault();
            if (item is null) Results.NotFound("Error ? NOt Found");//: {

            item.BlogId = id;
            if (!string.IsNullOrEmpty(blog.BlogTitle)) item.BlogTitle = blog.BlogTitle;
            if (!string.IsNullOrEmpty(blog.BlogAuthor)) item.BlogAuthor = blog.BlogAuthor;
            if (!string.IsNullOrEmpty(blog.BlogContent)) item.BlogContent = blog.BlogContent;

            appDb.Entry(item).State = EntityState.Modified;

            //appDb.Add(blog);
            appDb.SaveChanges();
            return Results.Ok("Success");


        }).WithName("PatchBlog").WithOpenApi();


    }





}
