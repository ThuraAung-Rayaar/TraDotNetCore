using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ToDoListShell.ConsoleApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static ToDoList.RestAPI.Models.ViewModels;

namespace ToDoList.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListDapperAPI : ControllerBase
    {
        private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
        [HttpGet]
        public IActionResult Get_ToDoLists()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                string query = @"SELECT 
                                t.TaskID,
                                t.TaskTitle,
                                t.TaskDescription,
                                c.CategoryName,
                                t.PriorityLevel,
                                t.Status,
                                t.DueDate,
                                t.CreatedDate,
                                t.CompletedDate
                            FROM ToDoList t
                            LEFT JOIN TaskCategory c ON t.CategoryID = c.CategoryID
                            where t.DeleteFlag = 0
                    ";

                var tem = db.Query<ToDoDapperModel>(query).ToList();
                List<ToDoViewModel> ToDoList = new List<ToDoViewModel>();
                foreach (var item in tem)
                {
                    ToDoList.Add(new ToDoViewModel
                    {
                        ID = item.TaskID,
                        Title = item.TaskTitle,
                        Description = item.TaskDescription,
                        CategoryName = item.CategoryName,
                        PriorityLevel = item.PriorityLevel,
                        Status = item.Status,
                        DueDate = item.DueDate,
                        CreatedDate = item.CreatedDate

                    });


                }
                return Ok(ToDoList);
            }



        }

        [HttpGet("search/{id}")]
        public IActionResult Search_ToDoList(int id)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                string query = @"SELECT 
                                t.TaskID,
                                t.TaskTitle,
                                t.TaskDescription,
                                c.CategoryName,
                                t.PriorityLevel,
                                t.Status,
                                t.DueDate,
                                t.CreatedDate,
                                t.CompletedDate
                            FROM ToDoList t
                            LEFT JOIN TaskCategory c ON t.CategoryID = c.CategoryID
                            where t.DeleteFlag = 0 and t.TaskID = @TaskID
                    "
                ;

                var item = db.Query<ToDoDapperModel>(query, new { TaskID = id }).FirstOrDefault();
                if (item is null) return NotFound("No Item Found");
                ToDoViewModel ToDo = new ToDoViewModel()
                {
                    ID = item.TaskID,
                    Title = item.TaskTitle,
                    Description = item.TaskDescription,
                    CategoryName = item.CategoryName,
                    PriorityLevel = item.PriorityLevel,
                    Status = item.Status,
                    DueDate = item.DueDate,
                    CreatedDate = item.CreatedDate
                };






                return Ok(ToDo);
            }
        }

        [HttpGet("category/{name}")]
        public int? QueryCategoryId(string categoryName)
        {
            int? categoryId;
            string queryid = $@"Select CategoryID from TaskCategory where CategoryName = @Name";
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var item = connection.Query<ToDoDapperModel>(queryid, new { Name = categoryName }).FirstOrDefault();
                if (item is null) return null;
                 categoryId = item.CategoryID;
                


            }
            return categoryId;

        }

        [HttpPost]
        public IActionResult Create_ToDoList(ToDoViewModel item)
        {
           


            using (IDbConnection db = new SqlConnection(_connectionString))
            {
               
                string query = @"INSERT INTO ToDoList (TaskTitle, TaskDescription, CategoryID, PriorityLevel, Status, DueDate, CreatedDate)
                             VALUES (@TaskTitle, @TaskDescription, @ID, @Level, @Status, @DueDate, @CreatedDate)";
              int result=  db.Execute(query,new {

                    
                    TaskTitle = item.Title,
                    TaskDescription = item.Description,
                    ID = QueryCategoryId(item.CategoryName),
                    Level = item.PriorityLevel,
                    Status = item.Status,
                    DueDate = item.DueDate,
                    CreatedDate = item.CreatedDate
                });

                return Ok(result > 0 ? "Success Creating To DO List" : "Fail & Error in Query or Code");

            }
        }

        [HttpPut("{id}")]
        public IActionResult Update_ToDoList(int id, ToDoViewModel item) {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                string query = $@"UPDATE ToDoList 
                             SET TaskTitle = @TaskTitle, 
                                 TaskDescription = @TaskDescription, 
                                 CategoryID = @CategoryID, 
                                 PriorityLevel = @PriorityLevel, 
                                 Status = @Status, 
                                 DueDate = @DueDate 
                             WHERE TaskID = @TaskID and DeleteFlag = 0 ";
               int result =  db.Execute(query, new
                {

                    TaskID = id,
                    TaskTitle = item.Title,
                    TaskDescription = item.Description,
                    CategoryID = QueryCategoryId(item.CategoryName),
                    PriorityLevel = item.PriorityLevel,
                    Status = item.Status,
                    DueDate = item.DueDate,
                   


                });

                return (result > 0) ? Ok("Update Successful") : NotFound("Id not Found Error");
            }



        }

        [HttpPatch("{id}")]
        public IActionResult Patch_ToDoList(int id, ToDoViewModel toDoItem) {
            int? categoryId = QueryCategoryId(toDoItem.CategoryName);
            string conditions = "";
            if (!string.IsNullOrEmpty(toDoItem.Title))
            {
                conditions += " TaskTitle = @TaskTitle, ";
            }
            if (!string.IsNullOrEmpty(toDoItem.Description))
            {
                conditions += " TaskDescription = @TaskDescription, ";
            }
            if (categoryId is not null)
            {
                conditions += " CategoryID = @CategoryID, ";
            }
            if (toDoItem.PriorityLevel != 0) // Assuming 0 means no update; modify as needed based on your logic
            {
                conditions += " PriorityLevel = @PriorityLevel, ";
            }
            if (!string.IsNullOrEmpty(toDoItem.Status))
            {
                conditions += " Status = @Status, ";
            }
            if (toDoItem.DueDate != DateTime.MinValue) // Assuming MinValue means no update
            {
                conditions += " DueDate = @DueDate, ";
            }
            if (string.IsNullOrEmpty(conditions))
            {
                return BadRequest("Error PAtch Content");
            }


            conditions = conditions.Substring(0, conditions.Length - 2);
            using (IDbConnection connection = new SqlConnection(_connectionString)) {
                string query = $@"UPDATE ToDoList 
                 SET {conditions} 
                 WHERE TaskID = @TaskID and DeleteFlag = 0";
                int result = connection.Execute(query, new
                {


                    TaskTitle = toDoItem.Title,
                    TaskDescription = toDoItem.Description,
                    CategoryID = QueryCategoryId(toDoItem.CategoryName),
                    PriorityLevel = toDoItem.PriorityLevel,
                    Status = toDoItem.Status,
                    DueDate = toDoItem.DueDate,
                    
                });
                return (result > 0) ? Ok("Update Successful") : NotFound("Id not Found Error");


            }


        }


        [HttpDelete("{id}")]
        public IActionResult Delete_ToDoList(int id) {


            using (IDbConnection connection = new SqlConnection(_connectionString)) {

                string query = "update ToDoList SET DeleteFlag = 1 WHERE TaskID = @TaskID";
                int result = connection.Execute(query, new { 
                TaskID = id
                });
                return (result > 0) ? Ok("Delete SuccessFul") : NotFound("Id not Found Error");
            }
           


        }

        [HttpPost("category")]
        public IActionResult Create_newCategory(string category) {

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                string query = @"Insert into TaskCategory (CategoryName) values (@Name)";
                int result = connection.Execute(query, new
                {
                    Name = category
                });
                return Ok(result > 0 ? "Success Creating Category" : "Fail & Error in Query or Code");
            }


        }

        [HttpGet("category")]
        public IActionResult ShowCategory() {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {

                string query = @" select  [CategoryID]
      ,[CategoryName] from [dbo].[TaskCategory] ";
              var tem = connection.Query<ToDoDapperModel>(query).ToList();
                List<CategoryDataModel> categoryDatas = new List<CategoryDataModel>();
                foreach (var item in tem) {
                    categoryDatas.Add(new CategoryDataModel{
                    
                    CategoryID = Convert.ToInt32( item.CategoryID),
                    CategoryName = item.CategoryName.ToString()
                    
                    
                    });
                   

                }
                
                return Ok(categoryDatas);
            }


        }
    }
}