namespace Tra.DotNetBatch5.MvcApp.Models;

public class BlogRequestModel
{
    public int BlogId { get; set; } = 0;
    public string BlogTitle { get; set; } = null!;

    public string BlogAuthor { get; set; } = null!;

    public string BlogContent { get; set; } = null!;
}
