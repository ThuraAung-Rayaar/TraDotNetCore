using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.RestAPI.Models
{
    public class ViewModels
    {
       
        public class ToDoViewModel
        {
          
            public int ID { get; set; }
           
            public string Title { get; set; }
            
            public string? Description { get; set; }

            public string CategoryName { get; set; }

            public byte PriorityLevel { get; set; }
           
            public string Status { get; set; }

            public DateTime DueDate { get; set; } = DateTime.MinValue;

            public DateTime CreatedDate { get; set; }
           
            public DateTime? CompletedDate { get; set; } // Nullable if the task isn't completed


        }



    }
}
