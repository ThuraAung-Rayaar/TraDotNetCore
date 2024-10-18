// See https://aka.ms/new-console-template for more information
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using ToDoListShell.ConsoleApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
Console.WriteLine("Hello, World!");

Console.WriteLine(
@"TO DO LIST
Add catgory
Show list
edit category
delete category
2.search/group by Category
3.search by title
4.sort by date
5.sort by completed/created date
6.ascending and decending
7.sort by priority level
8.sort by status
");

string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";

void ShowList() {
    List<ToDoDataModel> ToDOlist = new List<ToDoDataModel>();
    SqlConnection connection = new SqlConnection(_connectionString);
    string query = @"SELECT [TaskID]
      ,[TaskTitle]
      ,[TaskDescription]
      ,[CategoryID]
      ,[PriorityLevel]
      ,[Status]
      ,[DueDate]
      ,[CreatedDate]
      ,[CompletedDate]
  FROM [dbo].[ToDoList]";
    SqlCommand cmd = new SqlCommand(query, connection);
    connection.Open();
    SqlDataReader reader = cmd.ExecuteReader();

    while (reader.Read()) {
        ToDOlist.Add(
            new ToDoDataModel
            {
                TaskID = Convert.ToInt32(reader["TaskID"]),
                TaskTitle = reader["TaskTitle"].ToString(),
                TaskDescription = reader["TaskDescription"].ToString(),
                CategoryID =  reader["CategoryID"] == DBNull.Value ? null : Convert.ToInt32(reader["CategoryID"]),
                PriorityLevel = Convert.ToByte(reader["PriorityLevel"]),
                Status = reader["Status"].ToString(),
                DueDate = Convert.ToDateTime(reader["DueDate"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                CompletedDate = reader["CompletedDate"] == DBNull.Value ? null :Convert.ToDateTime(reader["CompletedDate"]) // Handles nullable CompletedDate




            }


            );



    
    }
    connection.Close();
    foreach (var item in ToDOlist)
    {
        Console.WriteLine(String.Format(
            "| {0,-7} | {1,-16} | {2,-11} | {3,-13} | {4,-12} | {5,-15} |",
            item.TaskID,
            item.TaskTitle.Length > 15 ? item.TaskTitle.Substring(0, 15) + "..." : item.TaskTitle, // Truncate long titles
            item.PriorityLevel,
            item.Status,
            item.DueDate.ToShortDateString(),
            item.CompletedDate.HasValue ? item.CompletedDate.Value.ToShortDateString() : "Not Completed"
        ));
    }
    Console.WriteLine("-----------------------------------------------------------------------------------------------------");



}





void SearchByTitle(string title) {
    List<ToDoDataModel> ToDOlist = new List<ToDoDataModel>();
    SqlConnection connection = new SqlConnection(_connectionString);
    string query = $@"SELECT [TaskID]
      ,[TaskTitle]
      ,[TaskDescription]
      ,[CategoryID]
      ,[PriorityLevel]
      ,[Status]
      ,[DueDate]
      ,[CreatedDate]
      ,[CompletedDate]
  FROM [dbo].[ToDoList] Where [TaskTitle] like @Title +'%'";


    SqlCommand cmd = new SqlCommand(query, connection);
    cmd.Parameters.AddWithValue("@Title", title);
    connection.Open();

    SqlDataReader reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        ToDOlist.Add(
            new ToDoDataModel
            {
                TaskID = Convert.ToInt32(reader["TaskID"]),
                TaskTitle = reader["TaskTitle"].ToString(),
                TaskDescription = reader["TaskDescription"].ToString(),
                CategoryID = reader["CategoryID"] == DBNull.Value ? null : Convert.ToInt32(reader["CategoryID"]),
                PriorityLevel = Convert.ToByte(reader["PriorityLevel"]),
                Status = reader["Status"].ToString(),
                DueDate = Convert.ToDateTime(reader["DueDate"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                CompletedDate = reader["CompletedDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["CompletedDate"]) 




            });
    }
    connection.Close();
    
    foreach (var item in ToDOlist)
    {
        Console.WriteLine(String.Format(
            "| {0,-7} | {1,-16} | {2,-11} | {3,-13} | {4,-12} | {5,-15} |",
            item.TaskID,
            item.TaskTitle.Length > 15 ? item.TaskTitle.Substring(0, 15) + "..." : item.TaskTitle, // Truncate long titles
            item.PriorityLevel,
            item.Status,
            item.DueDate.ToShortDateString(),
            item.CompletedDate.HasValue ? item.CompletedDate.Value.ToShortDateString() : "Not Completed"
        ));
    }
    Console.WriteLine("-----------------------------------------------------------------------------------------------------");


}


void GetList ()
{
    using (IDbConnection db = new SqlConnection(_connectionString)) {

        string query = @"SELECT [TaskID]
      ,[TaskTitle]
      ,[TaskDescription]
      ,[CategoryID]
      ,[PriorityLevel]
      ,[Status]
      ,[DueDate]
      ,[CreatedDate]
      ,[CompletedDate]
  FROM [dbo].[ToDoList]";
        var tem = db.Query<ToDoDapperModel>(query).ToList();
        foreach (var item in tem)
        {
            Console.WriteLine(String.Format(
                "| {0,-7} | {1,-16} | {2,-11} | {3,-13} | {4,-12} | {5,-15} |",
                item.TaskID,
                item.TaskTitle.Length > 15 ? item.TaskTitle.Substring(0, 15) + "..." : item.TaskTitle, // Truncate long titles
                item.PriorityLevel,
                item.Status,
                item.DueDate.ToShortDateString(),
                item.CompletedDate.HasValue ? item.CompletedDate.Value.ToShortDateString() : "Not Completed"
            ));
        }

    }


}

void readList() { 

    AppDbContext db = new AppDbContext();
    var tem = db.ToDoLists.ToList();
    foreach (var item in tem)
    {
        Console.WriteLine(String.Format(
            "| {0,-7} | {1,-16} | {2,-11} | {3,-13} | {4,-12} | {5,-15} |",
            item.TaskID,
            item.TaskTitle.Length > 15 ? item.TaskTitle.Substring(0, 15) + "..." : item.TaskTitle, // Truncate long titles
            item.PriorityLevel,
            item.Status,
            item.DueDate.ToShortDateString(),
            item.CompletedDate.HasValue ? item.CompletedDate.Value.ToShortDateString() : "Not Completed"
        ));
    }

}


SearchByTitle("Plan Vacation");
//ShowList();
GetList();
readList();