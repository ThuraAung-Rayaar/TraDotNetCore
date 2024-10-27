using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ToDoList.Database.Models;

public partial class CategoryDataModel
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;
    //[JsonIgnore]
    // public virtual ICollection<ToDoDataModel> ToDoLists { get; set; } = new List<ToDoDataModel>();
}
