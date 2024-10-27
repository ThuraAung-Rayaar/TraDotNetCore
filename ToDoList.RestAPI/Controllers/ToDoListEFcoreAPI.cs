using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using ToDoList.Database.Models;

using static ToDoList.RestAPI.Models.ViewModels;

namespace ToDoList.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListEFcoreAPI : ControllerBase
    {

        [HttpGet]
        public IActionResult Get_ToDoLists()
        {   AppDbContext dbContext = new AppDbContext();
           
            List<ToDoViewModel> toDOList = dbContext.ToDoLists.Where(x=> x.DeleteFlag == false).Select(x=> new ToDoViewModel { 
            
                ID = x.TaskId,
                Title = x.TaskTitle,
                Description = x.TaskDescription,
                CategoryName = dbContext.TaskCategories.Where(y=>y.CategoryId == x.CategoryId).Select(y=> y.CategoryName).FirstOrDefault() ,
                PriorityLevel = (byte)x.PriorityLevel,
                Status = x.Status,
                DueDate = (DateTime)x.DueDate,
                CreatedDate = (DateTime)x.CreatedDate


            }) .ToList();
           

            return Ok(toDOList);
        }
        [HttpGet("search/{id}")]
        public IActionResult Search_ToDoList(int id) {
            AppDbContext dbContext = new AppDbContext();
           var item = dbContext.ToDoLists.Where(x =>x.TaskId==id & x.DeleteFlag == false).Select(x => new ToDoViewModel
            {

                ID = x.TaskId,
                Title = x.TaskTitle,
                Description = x.TaskDescription,
                CategoryName = dbContext.TaskCategories.Where(y => y.CategoryId == x.CategoryId).Select(y => y.CategoryName).FirstOrDefault(),
                PriorityLevel = (byte)x.PriorityLevel,
                Status = x.Status,
                DueDate = (DateTime)x.DueDate,
                CreatedDate = (DateTime)x.CreatedDate


            }).FirstOrDefault();
            if (item is null) return BadRequest("No ID Found");
            ToDoViewModel toDO = item as ToDoViewModel;
            return Ok(toDO);
        }

        [HttpPost]
        public IActionResult Create_ToDoList(ToDoViewModel toDoItem) {
           AppDbContext dbContext = new AppDbContext();

            var item = dbContext.TaskCategories.Where(y => y.CategoryName == toDoItem.CategoryName).Select(y => y).FirstOrDefault();
            int? ID = item is null ? null:item.CategoryId;
          ToDoDataModel newItem = new ToDoDataModel() { 
            
            TaskDescription = toDoItem.Description,
            TaskTitle = toDoItem.Title,
            CategoryId = ID ,
            PriorityLevel = toDoItem.PriorityLevel,
            Status = toDoItem.Status,
            DueDate = toDoItem.DueDate,
            CreatedDate= toDoItem.CreatedDate,
            DeleteFlag = false
            
            
            };
            // Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Database.Models.ToDoList> entityEntry = dbContext.ToDoLists.Add(newItem);
            dbContext.Add(newItem);
            dbContext.SaveChanges();
            return Ok("New Item Added");
        }

        [HttpPut("{id}")]
        public IActionResult Update_ToDoList(int id, ToDoViewModel toDoItem) {
            AppDbContext dbContext = new AppDbContext();
            int? ID = (dbContext.TaskCategories.Where(y => y.CategoryName == toDoItem.CategoryName).Select(y => y).FirstOrDefault())?.CategoryId;
            ToDoDataModel dataitem = dbContext.ToDoLists.AsNoTracking().Where(x=> x.TaskId == id & x.DeleteFlag== false).FirstOrDefault();
            if(dataitem is null) return NotFound("No Data Found");

           
           
                dataitem.TaskTitle = toDoItem.Title;
            

            
                dataitem.TaskDescription = toDoItem.Description;
            

           
                dataitem.CategoryId =ID; 
            

          
               dataitem.PriorityLevel = toDoItem.PriorityLevel;
            

            
                dataitem.Status = toDoItem.Status;
            

            
                dataitem.DueDate = toDoItem.DueDate;
            
            dbContext.Entry(dataitem).State = EntityState.Modified;
            dbContext.SaveChanges();
            // item.Title = toDoItem.Title;

            return Ok(dataitem);
        }
        [HttpPatch("{id}")]
        public IActionResult Patch_ToDoList(int id, ToDoViewModel toDoItem) {

            AppDbContext dbContext = new AppDbContext();
            // var item = dbContext.TaskCategories.Where(y => y.CategoryName == toDoItem.CategoryName).Select(y => y).FirstOrDefault();
            var dataitem = dbContext.ToDoLists.AsNoTracking().Where(x => x.TaskId == id && x.DeleteFlag == false).FirstOrDefault();
            if (dataitem is null) return NotFound("No Data Found");
            if (!string.IsNullOrEmpty(toDoItem.Title))
            {
                dataitem.TaskTitle = toDoItem.Title;
            }

            if (!string.IsNullOrEmpty(toDoItem.Description))
            {
                dataitem.TaskDescription = toDoItem.Description;
            }

            if (!string.IsNullOrEmpty(toDoItem.CategoryName))
            {
                var category = dbContext.TaskCategories
                .Where(y => y.CategoryName == toDoItem.CategoryName)
                .Select(y => y)
                .FirstOrDefault();

                if (category != null)
                {
                    dataitem.CategoryId = category.CategoryId;
                }
            }

            if (toDoItem.PriorityLevel != 0)
            {
                dataitem.PriorityLevel = toDoItem.PriorityLevel;
            }

            if (!string.IsNullOrEmpty(toDoItem.Status))
            {
                dataitem.Status = toDoItem.Status;
            }

            if (toDoItem.DueDate != DateTime.MinValue)
            {
                dataitem.DueDate = toDoItem.DueDate;
            }
            dbContext.Attach(dataitem);
            dbContext.Entry(dataitem).State = EntityState.Modified;
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete_ToDoList(int id) {

            AppDbContext dbContext = new AppDbContext();
            ToDoDataModel item = dbContext.ToDoLists.AsNoTracking().Where(x=> x.TaskId == id & x.DeleteFlag == false).FirstOrDefault();
            if (item is null) return NotFound("No Data Found");
            item.DeleteFlag = true;
            dbContext.Entry(item).State = EntityState.Modified;
            dbContext.SaveChanges();


            return Ok("Successful");
        }

        [HttpPost("category")]
        public IActionResult Create_newCategory(string category) {
            AppDbContext dbContext = new AppDbContext();
            dbContext.TaskCategories.Add(
                new CategoryDataModel
                {

                    CategoryName = category
                }
                );
            dbContext.SaveChanges();
            return Ok("New Category Added");
        }

        [HttpGet("category")]
        public IActionResult ShowCategory() { 
        List<CategoryDataModel> CAtegoryList = new List<CategoryDataModel>();
            AppDbContext dbContext = new AppDbContext();
            CAtegoryList.AddRange( dbContext.TaskCategories.ToList() );
            return Ok(CAtegoryList);
        
        }

    }
}
