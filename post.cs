using System;

namespace BlogApplication_db
{
    public class Post
    {
        public int BlogID { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }

        public string UserName { get; set; }

        public void Display()
        {
            Console.WriteLine($"Id: {ID}, Blog ID: {BlogID}, Post Date: {Date}, Post Title: {Title}, Content: {Content}");
        }
    }
}