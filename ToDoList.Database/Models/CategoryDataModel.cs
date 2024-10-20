using System;
using System.Collections.Generic;

namespace ToDoList.Database.Models;

public partial class CategoryDataModel
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<ToDoDataModel> ToDoLists { get; set; } = new List<ToDoDataModel>();
}
