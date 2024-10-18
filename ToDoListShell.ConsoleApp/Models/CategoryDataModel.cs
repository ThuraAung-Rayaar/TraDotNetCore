using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListShell.ConsoleApp.Models
{
    [Table("TaskCategory")]
    public class CategoryDataModel
    {
        [Column("CategoryID")]
        public int CategoryID { get; set; }
        [Column("CategoryName")]
        public string CategoryName { get; set; }


    }
}
