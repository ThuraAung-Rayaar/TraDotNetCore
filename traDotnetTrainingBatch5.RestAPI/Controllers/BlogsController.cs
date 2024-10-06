using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace traDotnetTrainingBatch5.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBlogs()
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateBlogs()
        {
            return Ok();
        }

        [HttpPatch]
        public IActionResult PatchBlogs()
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult CreateBlogs()
        {
            return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteBlogs()
        {
            return Ok();
        }
    }
}
