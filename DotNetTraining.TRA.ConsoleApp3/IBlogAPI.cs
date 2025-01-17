using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTraining.TRA.ConsoleApp3
{
    public interface IBlogAPI
    {
        [Get("/api/blogs")]
        Task<List<BlogModel>> GetBlogs();

        [Get("/api/blogs/{id}")]
        Task<BlogModel> GetBlog(int id);

        [Patch("/api/blogs/{id}")]
        Task<string> UpdateBlog(int id,BlogModel blog);

        [Post("/api/blogs")]
        Task<BlogModel> CreateBlog( BlogModel blog);

        [Delete("/api/blogs/{id}")]
        Task<string> DeleteBlog(int id);

    }


    public interface IRootModelApi {

        [Get("/")]
        Task<string> GetAll();
    
    }


    public  class BlogModel
    {
        public int BlogId { get; set; }

        public string BlogTitle { get; set; } = null!;

        public string BlogAuthor { get; set; } = null!;

        public string BlogContent { get; set; } = null!;

        public bool? DeleteFlag { get; set; }
    }

}
