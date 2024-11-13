namespace DotNetBatch5.RestAPI.DataModels
{
    public class BlogDataModel
    {

        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public string BlogAuthor { get; set; }
        public bool DeleteFlag { get; set; }

    }
}