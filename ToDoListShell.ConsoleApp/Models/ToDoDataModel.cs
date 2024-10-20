using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListShell.ConsoleApp.Models
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

        [Column("DeleteFlag")]
        public bool DeleteFlag { get; set; }

    }
    public class ToDoDapperModel
    {
        public int TaskID { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public string? CategoryName { get; set; }
        public int? CategoryID { get; set; }
        public byte PriorityLevel { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; } // Nullable if the task isn't completed


    }




}
