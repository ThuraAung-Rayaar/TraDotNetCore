using BlogServices.Domain.Feature;
using DotNetBatch5.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Tra.DotNetBatch5.MvcApp.Models;

namespace Tra.DotNetBatch5.MvcApp.Controllers.Blog;

public class BlogController : Controller
{
    public readonly BlogService _service;

    public BlogController(BlogService service)
    {
        _service = service;
    }

    [ActionName("Create")]
    public IActionResult BlogCreate()
    {
        return View("BlogCreate");
    }




    [HttpGet]//default
    public IActionResult Index()
    {

        var blogList = _service.GetBlogs();
        if (blogList.Count == 0 || blogList is null)
        {
            return View(null);
        }
        return View(blogList);
    }



    [HttpPost]
    [ActionName("Save")]
    public IActionResult Save(BlogRequestModel blogModel)
    {
        try
        {
            var blog = new TblBlog()
            {
                BlogTitle = blogModel.BlogTitle,
                BlogAuthor = blogModel.BlogAuthor,
                BlogContent = blogModel.BlogContent
            };



            _service.CreateBlog(blog);

            TempData["Message"] = "Blog Created Successfully";
            TempData["MessageType"] = "success";
            TempData["IsSuccess"] = true;


        }

        catch(Exception ex) {
            TempData["Message"] =ex.ToString();
            TempData["MessageType"] = "Blog Creation Failed";
            TempData["IsSuccess"] = false;
        }
        return RedirectToAction("Index");
    }

   
    [ActionName("Delete")]
    public IActionResult DeleteBlog(int id) {

        try {
            var result = _service.DeleteBlog(id);
            if (result == 0) throw new Exception("Blog not found");

            TempData["Message"] = "Blog deletion Successful";
            TempData["MessageType"] = "success";
            TempData["IsSuccess"] = true;
            
        }
        catch (Exception ex) {

            TempData["Message"] = ex.ToString();
            TempData["MessageType"] = "Blog deletion Failed";
            TempData["IsSuccess"] = false;
        }
        return RedirectToAction("Index");
    }

    [ActionName("Edit")]
    public IActionResult EditBlog(int id)
    {

        var blog = _service.GetBlog(id);
        if (blog == null)
        {
            return RedirectToAction("Index");
        }


        return View("EditBlog", blog);
    }


    [HttpPost]
    [ActionName("Update")]
    public IActionResult BlogUpdate( BlogRequestModel requestModel)
    {
        try
        {
            var blog = new TblBlog()
            {
                BlogId = requestModel.BlogId,
                BlogTitle = requestModel.BlogTitle,
                BlogAuthor = requestModel.BlogAuthor,
                BlogContent = requestModel.BlogContent
            };
         int result=   _service.UpdateBlog(requestModel.BlogId, blog);
            if(result == 0) throw new Exception("Blog not found");

            TempData["IsSuccess"] = true;
            TempData["Message"] = "Blog Updated Successfully";
        }
        catch (Exception ex)
        {
            TempData["IsSuccess"] = false;
            TempData["Message"] = ex.ToString();
        }

        return RedirectToAction("Index");
    }

}



