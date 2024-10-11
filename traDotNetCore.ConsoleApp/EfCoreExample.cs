using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traDotNetCore.ConsoleApp.Models;

namespace traDotNetCore.ConsoleApp
{
    public class EfCoreExample
    {

        public void Read() {  
        AppDbContext dbContent = new AppDbContext();
        var lst = dbContent.Blogs.Where(x=> x.DeleteFlag == false).ToList();
            foreach (var item in lst)
            {
                Console.WriteLine($"{ item.BlogId}    { item.BlogTitle}    {item.BlogAuthor}   {item.BlogContent}");
                 


            }

        }


        public void Create(string Title, string author, string content) {

            AppDbContext dbContent = new AppDbContext();
            BlogDataModel blog = new BlogDataModel() { 
            BlogAuthor = author,
            BlogContent = content,
            BlogTitle = Title,
            DeleteFlag = false
            
            };
            dbContent.Blogs.Add(blog);


            var result = dbContent.SaveChanges();
            Console.WriteLine(result ==1 ? "Successful":"Unsuccessful");

        }



        public void Edit(int id)
        {

            AppDbContext dbContent = new AppDbContext();
            var item = dbContent.Blogs.FirstOrDefault(x=>x.BlogId == id);
            if (item is null)
            {
                Console.WriteLine("No Data Found");
                return;

            }
            Console.WriteLine($"{item.BlogId}    {item.BlogTitle}    {item.BlogAuthor}   {item.BlogContent}");
            /*BlogDataModel blog = new BlogDataModel()
            {
                BlogAuthor = author,
                BlogContent = content,
                BlogTitle = Title,
                DeleteFlag = false

            };
            dbContent.Blogs.Add(blog);*/


           

        }



        public void Update(int id,string Title, string author, string content)
        {
            //Edit(id); 
           // AppDbContent dbContent = new AppDbContent();
            AppDbContext dbContent = new AppDbContext();
            var item = dbContent.Blogs.AsNoTracking()
                .FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                Console.WriteLine("No Data Found");
                return;

            }
            if (!string.IsNullOrEmpty(author)) item.BlogAuthor = author;
            if (!string.IsNullOrEmpty(content)) item.BlogContent = content;
            if (!string.IsNullOrEmpty(Title)) item.BlogTitle = Title;
            dbContent.Entry(item).State = EntityState.Modified;
            int result = dbContent.SaveChanges();
            Console.WriteLine(result == 1 ? "Update Successful" : "Update Unsuccessful");



        }

        public void Delete(int id)
        {
            AppDbContext dbContent = new AppDbContext();
            var item = dbContent.Blogs.FirstOrDefault(y => y.BlogId == id );
            if (item is null)
            {
                Console.WriteLine("No Data Found");
                return;

            }
            dbContent.Entry(item).State= EntityState.Deleted;
            var result = dbContent.SaveChanges();
            Console.WriteLine(result == 1 ? "Delete Successful" : "Delete Unsuccessful");




        }


    }
}
