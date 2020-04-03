using System;

namespace Pathogen.Models
{
    public class Message
    {
        public static int count = 0;

        public int Id { get; set; } = count++;
        public string Text { get; set; }
        public string User { get; set; }
        public DateTime TimeSent { get; set; } = DateTime.Now;
    }
}
