using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using ToDoListShell.ConsoleApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static ToDoList.RestAPI.Models.ViewModels;

namespace ToDoList.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListADODotNetController : ControllerBase
    {
        private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
        [HttpGet]
        public IActionResult Get_ToDoLists()
        {
            List<ToDoViewModel> toDoList = new List<ToDoViewModel>();
            SqlConnection connection = new SqlConnection(_connectionString);
            
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

                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    toDoList.Add(new ToDoViewModel
                    {
                        ID = Convert.ToInt32(reader["TaskID"]),
                        Title = reader["TaskTitle"].ToString(),
                        Description = reader["TaskDescription"].ToString(),
                        CategoryName = reader["CategoryName"].ToString(),
                        PriorityLevel = Convert.ToByte(reader["PriorityLevel"]),
                        Status = reader["Status"].ToString(),
                        DueDate = Convert.ToDateTime(reader["DueDate"]),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        CompletedDate = reader["CompletedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CompletedDate"])
                    });
                }
            connection.Close();

            return Ok(toDoList);
        }

        [HttpGet("search/{id}")]
        public IActionResult Search_ToDoList(int id)
        {
            ToDoViewModel toDoItem = new ToDoViewModel();
            SqlConnection connection = new SqlConnection(_connectionString);
            
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
                            WHERE t.TaskID = @TaskID and t.DeleteFlag = 0";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TaskID", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();


                toDoItem.ID = Convert.ToInt32(reader["TaskID"]);
                toDoItem.Title = reader["TaskTitle"].ToString();
                toDoItem.Description = reader["TaskDescription"].ToString();
                toDoItem.CategoryName = reader["CategoryName"].ToString();
                toDoItem.PriorityLevel = Convert.ToByte(reader["PriorityLevel"]);
                toDoItem.Status = reader["Status"].ToString();
                toDoItem.DueDate = Convert.ToDateTime(reader["DueDate"]);
                toDoItem.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                toDoItem.CompletedDate = reader["CompletedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CompletedDate"]);
                return Ok(toDoItem);
                
                }
                else { return NotFound("No Item Found/ Error"); }
            

           

           
        }
        [HttpGet("category/{name}")]
        public int? QueryCategoryId(string categoryName) {
            int? categoryId = null;
            SqlConnection connection = new SqlConnection(_connectionString);
            string queryid = $@"Select CategoryID from TaskCategory where CategoryName = @Name";
            SqlCommand getIdCmd = new SqlCommand(queryid, connection);
            getIdCmd.Parameters.AddWithValue("@Name", categoryName);
            connection.Open();
            SqlDataReader reader = getIdCmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                categoryId = Convert.ToInt32(reader["CategoryID"]);



            }
           // else categoryId = DBNull.Value;
            connection.Close();

            return categoryId;
        }

        [HttpPost]
        public IActionResult Create_ToDoList(ToDoViewModel toDoItem)
        {

            /* SqlConnection connection = new SqlConnection(_connectionString);

                 int categoryId = 0;
                 string queryid = $@"Select CategoryID from TaskCategory where CategoryName = @Name";
                 SqlCommand getIdCmd = new SqlCommand(queryid,connection);
                 getIdCmd.Parameters.AddWithValue("@Name",toDoItem.CategoryName);
                 connection.Open();
                 SqlDataReader reader = getIdCmd.ExecuteReader();
                 if (reader.HasRows) { 
                     reader.Read();
                     categoryId = Convert.ToInt32( reader["CategoryID"]);



                 }
                 connection.Close();*/


            int? categoryId = QueryCategoryId(toDoItem.CategoryName);
            SqlConnection connection = new SqlConnection(_connectionString);
            string query = @"INSERT INTO ToDoList (TaskTitle, TaskDescription, CategoryID, PriorityLevel, Status, DueDate, CreatedDate)
                             VALUES (@TaskTitle, @TaskDescription, @CategoryID, @PriorityLevel, @Status, @DueDate, @CreatedDate)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TaskTitle", toDoItem.Title);
                cmd.Parameters.AddWithValue("@TaskDescription", toDoItem.Description);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId is null? DBNull.Value:categoryId); // Handle nullable CategoryID if necessary
                cmd.Parameters.AddWithValue("@PriorityLevel", toDoItem.PriorityLevel);
                cmd.Parameters.AddWithValue("@Status", toDoItem.Status);
                cmd.Parameters.AddWithValue("@DueDate", toDoItem.DueDate);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now); // Assuming it's created now

                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                connection.Close();



            return Ok(rowsAffected > 0 ? "Task Created" :
                "A problem happened while handling your request.");
        }


        [HttpPut("{id}")]
        public IActionResult Update_ToDoList(int id,ToDoViewModel toDoItem)
        {

            int? categoryId= QueryCategoryId(toDoItem.CategoryName);
            SqlConnection connection = new SqlConnection(_connectionString);
                string query = $@"UPDATE ToDoList 
                             SET TaskTitle = @TaskTitle, 
                                 TaskDescription = @TaskDescription, 
                                 CategoryID = @CategoryID, 
                                 PriorityLevel = @PriorityLevel, 
                                 Status = @Status, 
                                 DueDate = @DueDate 
                             WHERE TaskID = @TaskID and DeleteFlag = 0 " ;

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TaskID", id);
                cmd.Parameters.AddWithValue("@TaskTitle", toDoItem.Title);
                cmd.Parameters.AddWithValue("@TaskDescription", toDoItem.Description);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId is null ? DBNull.Value : categoryId); // Handle nullable CategoryID
                cmd.Parameters.AddWithValue("@PriorityLevel", toDoItem.PriorityLevel);
                cmd.Parameters.AddWithValue("@Status", toDoItem.Status);
                cmd.Parameters.AddWithValue("@DueDate", toDoItem.DueDate);

                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                connection.Close();
            return (rowsAffected == 0)?NotFound("Task NOt Found"):Ok("Updated");
            
        }

        [HttpPatch("{id}")]
        public IActionResult Patch_ToDoList(int id, ToDoViewModel toDoItem)
        {
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
            

          
            

            string query = $@"UPDATE ToDoList 
                 SET {conditions} 
                 WHERE TaskID = @TaskID and DeleteFlag = 0";


            SqlConnection connection = new SqlConnection(_connectionString);
            
                
                SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskID", id);
            if (!string.IsNullOrEmpty(toDoItem.Title))
            {
                command.Parameters.AddWithValue("@TaskTitle", toDoItem.Title);
            }
            if (!string.IsNullOrEmpty(toDoItem.Description))
            {
                command.Parameters.AddWithValue("@TaskDescription", toDoItem.Description);
            }
            if (categoryId is not null)
            {
                command.Parameters.AddWithValue("@CategoryID", categoryId is null ? DBNull.Value : categoryId);
            }
            if (toDoItem.PriorityLevel != 0)
            {
                command.Parameters.AddWithValue("@PriorityLevel", toDoItem.PriorityLevel);
            }
            if (!string.IsNullOrEmpty(toDoItem.Status))
            {
                command.Parameters.AddWithValue("@Status", toDoItem.Status);
            }
            if (toDoItem.DueDate != DateTime.MinValue)
            {
                command.Parameters.AddWithValue("@DueDate", toDoItem.DueDate);
            }
            //command.Parameters.AddWithValue("@CategoryID", categoryId is null ? DBNull.Value : categoryId);
                

                connection.Open();
               int result = command.ExecuteNonQuery();
                connection.Close();

            return Ok(result>0 ? "Updated":"Error Updating / ID not found");
        }


        [HttpDelete("{id}")]
        public IActionResult Delete_ToDoList(int id)
            {
            SqlConnection connection = new SqlConnection(_connectionString);
                
                    string query = "update ToDoList SET DeleteFlag = 1 WHERE TaskID = @TaskID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@TaskID", id);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                connection.Close();
             return (rowsAffected == 0)?
                NotFound("Task NOt Found"):
                Ok("DELETED");


        }

        [HttpPost("category")]
        public IActionResult Create_newCategory(string category) {
            SqlConnection connection = new SqlConnection(_connectionString);
            string query = @"Insert into TaskCategory (CategoryName) values (@Name)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", category);
            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close ();

            return Ok();
        }
        [HttpGet("category")]
        public IActionResult ShowCategory()
        {   List<CategoryDataModel> categoryDatas = new List<CategoryDataModel> ();
            SqlConnection connection = new SqlConnection(_connectionString);
            string query = @" select  [CategoryID]
      ,[CategoryName] from [dbo].[TaskCategory] ";
            SqlCommand command = new SqlCommand(query, connection);
          

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                categoryDatas.Add(
                    new CategoryDataModel {
                   CategoryID = Convert.ToInt32( reader["CategoryID"]),
                   CategoryName = reader["CategoryName"].ToString()
                    
                    }

                    );



            }
            connection.Close();

            return Ok(categoryDatas);
        }
    }
}
