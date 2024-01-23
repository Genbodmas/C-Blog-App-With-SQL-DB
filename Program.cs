using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication_db
{
    class Program   
    {
        static void Main(string[] args)
        {
            var blogManager = new BlogManager();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("WELCOME TO SOFTWORKS BLOG MANAGER, WHAT WOULD YOU LIKE TO DO?");
                Console.WriteLine();
                Console.WriteLine("1: Display all Blogs");
                Console.WriteLine("2: Display all Post");
                Console.WriteLine("3: Create new Blog");
                Console.WriteLine("4: Create new post");
                Console.WriteLine("5: Delete a post");
                Console.WriteLine("6: Manage blog");
                Console.WriteLine("7: Exit");
                Console.WriteLine();

                Console.Write("Enter your choice");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        blogManager.DisplayAllBlog();
                        break;
                    case "2":
                        blogManager.DisplayAllPost();
                        break;
                    case "3":
                        blogManager.CreateNewBlog(); 
                        break;
                    case "4":
                        blogManager.CreateNewPost();
                        break;
                    case "5":
                        blogManager.DeletePost();
                        break;
                    case "6":
                        blogManager.ManageBlog();
                        break;
                    case "7":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid Command, Please try again");
                        break;
                }
            }
        }
    }
}
