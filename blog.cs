using System;

namespace BlogApplication_db
{
    public class Blog
    {
        public string Desc { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }

        public string UserName { get; set; }

        public void Display()
        {
            Console.WriteLine($"Id: {ID}, Title: {Name}, Description: {Desc}");
        }
    }
}