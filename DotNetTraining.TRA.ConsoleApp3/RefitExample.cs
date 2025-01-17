using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTraining.TRA.ConsoleApp3;

public class RefitExample
{
    private readonly IBlogAPI _BlogAPi;//= RestService.For<IBlogAPI>("https://localhost:7186");

    public RefitExample() {
        _BlogAPi = RestService.For<IBlogAPI>("https://localhost:7186");
        //_BlogAPi = RestService.For<IBlogAPI>(port);

    }

    public async Task<List<BlogModel>> GetBlogsAsync() {

        //var BlogAPi = RestService.For<IBlogAPI>("https://localhost:7186");
        var lsst = await _BlogAPi.GetBlogs();
        foreach (var item in lsst) {

            Console.WriteLine(item.BlogId+": "+ item.BlogAuthor);
        }
        return lsst;

    }

    public async Task<BlogModel?> GetBlogAsync(int id)
    {

        //var BlogAPi = RestService.For<IBlogAPI>("https://localhost:7186");
        try
        {
            var item = await _BlogAPi.GetBlog(id); Console.WriteLine(item.BlogId + ": " + item.BlogAuthor); return item;
        }
        catch (ApiException ex) {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound) {

                Console.WriteLine("NO Data Found");
            }
            return null;
        }

          

    }

    public async Task<BlogModel?> UpdateBlogAsync(int id,string? author = "",string? title="",string? content="")
    {

       // var BlogAPi = RestService.For<IBlogAPI>("https://localhost:7186");
        var item = await _BlogAPi.GetBlog(id);
        

        if(!String.IsNullOrEmpty(author)) item.BlogAuthor = author;
        if (!String.IsNullOrEmpty(title)) item.BlogTitle = title;
        if (!String.IsNullOrEmpty(content)) item.BlogContent = content;

        var result = await _BlogAPi.UpdateBlog(id, new BlogModel { 
                        BlogId = id,
                        BlogTitle = item.BlogTitle,
                        BlogContent = item.BlogContent,
                        BlogAuthor = item.BlogAuthor
                         });


        Console.WriteLine(result);


        return item;

    }
    public async Task<BlogModel?> CreateBlogAsync( string? author = "", string? title = "", string? content = "")
    {

        
        BlogModel item = new BlogModel() {
            BlogAuthor = author,
            BlogTitle = title,
            BlogContent = content,
            DeleteFlag = false
        };

        var result = await _BlogAPi.CreateBlog(item);

      

        Console.WriteLine(result);


        return result;

    }
    public async Task<string> DeleteBlogAsync(int id)
    {

       


        var result = await _BlogAPi.DeleteBlog(id);



        Console.WriteLine(result);


        return result;

    }

}


public class RefitRootExample { 

    private readonly IRootModelApi _RootModelApi;
    public RefitRootExample(IRootModelApi rootModelApi) { 
    
    
    _RootModelApi = rootModelApi;
    }


    public async Task<List<rootModel>> GetAsync() {
        var lsst = await _RootModelApi.GetAll();
        var jsonList = JsonConvert.SerializeObject(lsst);
        var rootModelList = JsonConvert.DeserializeObject<List<rootModel>>(jsonList);

        foreach (var item in rootModelList) {

            Console.WriteLine(item.body);
        
        }
        return rootModelList;
    }

}
