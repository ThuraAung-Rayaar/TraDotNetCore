namespace DotNetCoreTraining.Domain.DataModels;

public class BlogDataModel
{
    public static BlogDataModel GetDefault()
    {
        return new BlogDataModel
        {
            BlogId = 0,
            BlogTitle = string.Empty,
            BlogContent = string.Empty,
            BlogAuthor = string.Empty,
            DeleteFlag = false
        };
    }
    public int BlogId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
    public string BlogAuthor { get; set; }
    public bool DeleteFlag { get; set; }

}
