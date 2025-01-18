using BlogServices.Domain.Feature;
using DotNetBatch5.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Tra.DotNetBatch5.MvcApp.Models;

namespace Tra.DotNetBatch5.MvcApp.Controllers.Ajax;

public class BlogAjaxController : Controller
{
    private readonly BlogService _blogService;

    public BlogAjaxController(BlogService blogService)
    {
        _blogService = blogService;
    }


    [ActionName("index")]
    public IActionResult BlogList()
    {
        var blogList = _blogService.GetBlogs();
        return View("BlogList");
    }

    [ActionName("List")]
    public IActionResult getList()
    {

        var blogList = _blogService.GetBlogs();
        return Json(blogList);
    }

    
    [ActionName("Create")]
    public IActionResult CreateBlog() { 
    
    return View("BlogCreate");
    }
    [HttpPost]
    [ActionName("Save")]
    public IActionResult Save(BlogRequestModel blogModel)
    {
        MessageModel messageModel;
        try
        {
            var blog = new TblBlog()
            {
                BlogTitle = blogModel.BlogTitle,
                BlogAuthor = blogModel.BlogAuthor,
                BlogContent = blogModel.BlogContent
            };



            _blogService.CreateBlog(blog);

            TempData["Message"] = "Blog Created Successfully";
            TempData["MessageType"] = "success";
            TempData["IsSuccess"] = true;
            messageModel = new MessageModel(true, "Blog Created Successfully", "success");

        }

        catch (Exception ex)
        {
            TempData["Message"] = ex.ToString();
            TempData["MessageType"] = "Failure";
            TempData["IsSuccess"] = false;
            messageModel = new MessageModel(false, ex.ToString(), "error");
        }


        
        return Json(messageModel);
    }


    [ActionName("Edit")]
    public IActionResult BlogEdit(int id)
    {
        var blog = _blogService.GetBlog(id);
        BlogRequestModel blogRequestModel = new BlogRequestModel
        {
            BlogId = blog.BlogId,
            BlogAuthor = blog.BlogAuthor,
            BlogContent = blog.BlogContent,
            BlogTitle = blog.BlogTitle
        };
        return View("EditBlog", blogRequestModel);
    }

    [HttpPost]
    [ActionName("Update")]
    public IActionResult BlogUpdate(int id, BlogRequestModel requestModel)
    {
        MessageModel model;
        try
        {
            _blogService.UpdateBlog(id, new TblBlog
            {
                BlogAuthor = requestModel.BlogAuthor,
                BlogContent = requestModel.BlogContent,
                BlogTitle = requestModel.BlogTitle
            });

            TempData["IsSuccess"] = true;
            TempData["Message"] = "Blog Updated Successfully";
            TempData["MessageType"] = "success";
            model = new MessageModel(true, "Blog Updated Successfully", "success");
        }
        catch (Exception ex)
        {
            TempData["IsSuccess"] = false;
            TempData["Message"] = ex.ToString();
            TempData["MessageType"] = "Failure";

            model = new MessageModel(false, ex.ToString(), "Failure");
        }

        return Json(model);
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult BlogDelete(BlogRequestModel requestModel)
    {
        MessageModel model;
        try
        {
            _blogService.DeleteBlog(requestModel.BlogId);
            model = new MessageModel(true, "Blog Deleted Successfully", "success");
        }
        catch (Exception ex)
        {
            TempData["IsSuccess"] = false;
            TempData["Message"] = ex.ToString();
            TempData["MessageType"] = "Failure";
            model = new MessageModel(false, ex.ToString(), "Failure");
        }

        return Json(model);
    }




}
public class MessageModel
{
    public MessageModel()
    {
    }
    public MessageModel(bool isSuccess, string message, string messageType)
    {
        IsSuccess = isSuccess;
        Message = message;
        MessageType = messageType;

    }

    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string MessageType { get; set; }
}