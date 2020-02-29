using System;

namespace Pathogen.Models
{
    public class NewsItem
    {
        public string Publication { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDatetime { get; set; }

        public NewsItem(string publication, string title, string content, DateTime publishedDate)
        {
            Publication = publication;
            Title = title;
            Content = content;
            PublishDatetime = publishedDate;
        }
    }
}
