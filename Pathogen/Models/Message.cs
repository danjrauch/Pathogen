using System;

namespace Pathogen.Models
{
    public class Message
    {
        public string Text { get; set; }
        public string User { get; set; }
        public DateTime TimeSent { get; set; } = DateTime.Now;
    }
}
