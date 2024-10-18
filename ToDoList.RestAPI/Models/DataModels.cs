using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.RestAPI.Models
{
    public class DataModels
    {

        [Table("ToDoList")]
        public class ToDoDataModel
        {
            [Column("TaskID")]
            public int TaskID { get; set; }
            [Column("TaskTitle")]
            public string TaskTitle { get; set; }
            [Column("TaskDescription")]
            public string? TaskDescription { get; set; }
            [Column("CategoryID")]
            public int? CategoryID { get; set; }
            [Column("PriorityLevel")]
            public byte PriorityLevel { get; set; }
            [Column("Status")]
            public string Status { get; set; }
            [Column("DueDate")]
            public DateTime DueDate { get; set; }
            [Column("CreatedDate")]
            public DateTime CreatedDate { get; set; }
            [Column("CompletedDate")]
            public DateTime? CompletedDate { get; set; } // Nullable if the task isn't completed
            public bool DeleteFlag { get; set; }

        }

        [Table("TaskCategory")]
        public class CategoryDataModel
        {
            [Column("CategoryID")]
            public int CategoryID { get; set; }
            [Column("CategoryName")]
            public string CategoryName { get; set; }


        }


    }
}
