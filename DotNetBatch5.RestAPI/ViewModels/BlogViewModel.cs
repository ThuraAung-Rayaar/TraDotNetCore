﻿namespace DotNetBatch5.RestAPI.ViewModels
{
    public class BlogViewModel
    {

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public bool DeleteFlag { get; set; }

    }
}