using BlogServices.Domain.Feature;
using Microsoft.AspNetCore.Mvc;

namespace Tra.DotNetBatch5.MvcApp.Controllers.Ajex;


[Route("ajax/blog")]
public class BlogControllerAjax : Controller
{
    private readonly BlogService _blogService;

    public BlogControllerAjax(BlogService blogService)
    {
        _blogService = blogService;
    }

    [ActionName("index")]
    public IActionResult BlogList()
    {
        var blogList = _blogService.GetBlogs();
        return View("BlogList", blogList);
    }

   

}



