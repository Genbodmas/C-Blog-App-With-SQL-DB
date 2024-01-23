using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BlogApplication_db
{

    public class BlogManager
    {
        private const string ConnectionString = "Data Source=GENERALBODMAS\\SQLEXPRESS;Initial Catalog=Blog_App;Integrated Security=True";

        public void DisplayAllBlog()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM blogs";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("All Blogs: ");
                            while (reader.Read())
                            {
                                var blog = new Blog
                                {
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Desc = reader.GetString(2)
                                };
                                blog.Display();
                            }
                        }
                        else
                        {
                            Console.WriteLine("No blogs found");
                        }
                    }
                }
            }
        }

        public void DisplayAllPost()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM posts";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("All posts: ");
                            while (reader.Read())
                            {
                                var post = new Post
                                {
                                    ID = reader.GetInt32(0),
                                    BlogID = reader.GetInt32(1),
                                    Date = reader.GetDateTime(2),
                                    Title = reader.GetString(3),
                                    Content = reader.GetString(4)
                                };
                                post.Display();
                            }
                        }
                        else
                        {
                            Console.WriteLine("No posts found");
                        }
                    }
                }
            }
        }

        // Other methods for creating, updating, and deleting blogs and posts from the database.

        public void CreateNewBlog()
        {
            Console.WriteLine("Enter Title of blog: ");
            string title = Console.ReadLine();

            Console.WriteLine("Enter blog description: ");
            string desc = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO blogs (Name, Descr) VALUES (@Name, @Desc)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", title);
                    command.Parameters.AddWithValue("@Desc", desc);

                    command.ExecuteNonQuery();

                    Console.WriteLine("Blog created Successfully");
                }
            }
        }

        public void CreateNewPost()
        {
            Console.WriteLine("Enter the BlogId the post belongs to");
            int blogId;
            if (!int.TryParse(Console.ReadLine(), out blogId))
            {
                Console.WriteLine("Invalid input for BlogId. Post creation failed.");
                return;
            }

            if (!BlogExists(blogId))
            {
                Console.WriteLine($"Blog with ID {blogId} not found. Post creation failed.");
                return;
            }

            Console.WriteLine("Enter Title of post: ");
            string title = Console.ReadLine();

            Console.WriteLine("Enter post Content: ");
            string content = Console.ReadLine();

            Console.WriteLine("Enter Date of post (yyyy-mm-dd): ");
            string dateInput = Console.ReadLine();
            DateTime date;
            if (!DateTime.TryParse(dateInput, out date))
            {
                Console.WriteLine("Invalid input for Date. Post creation failed.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO posts (BlogID, Date, Title, Content) VALUES (@BlogID, @Date, @Title, @Content)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BlogID", blogId);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Content", content);

                    command.ExecuteNonQuery();

                    Console.WriteLine("Post created Successfully");
                }
            }
        }

        public void DeletePost()
        {
            Console.WriteLine("Select the BlogID to delete from");
            int blogId;
            if (!int.TryParse(Console.ReadLine(), out blogId))
            {
                Console.WriteLine("Invalid input for BlogId. Try Again.");
                return;
            }

            if (!BlogExists(blogId))
            {
                Console.WriteLine($"Blog with ID {blogId} not found. Post deletion failed.");
                return;
            }

            Console.WriteLine($"Select the Id of the post to delete from blog {blogId} ");
            int postId;
            if (!int.TryParse(Console.ReadLine(), out postId))
            {
                Console.WriteLine("Invalid input for PostID. Try Again.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM posts WHERE ID = @PostID AND BlogID = @BlogID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PostID", postId);
                    command.Parameters.AddWithValue("@BlogID", blogId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Post Deleted Successfully");
                    }
                    else
                    {
                        Console.WriteLine("Post not found or not associated with the specified blog.");
                    }
                }
            }
        }

        public void ManageBlog()
        {
            Console.WriteLine("Enter ID of Blog to Manage");
            int blogId;

            if (!int.TryParse(Console.ReadLine(), out blogId))
            {
                Console.WriteLine("Invalid input for BlogId. Try Again.");
                return;
            }

            if (!BlogExists(blogId))
            {
                Console.WriteLine("Blog not Found");
                return;
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM blogs WHERE ID = @BlogID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BlogID", blogId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            Console.WriteLine($"Id: {reader.GetInt32(0)}, Title: {reader.GetString(1)}, Description: {reader.GetString(2)}");
                            Console.WriteLine();
                            Console.WriteLine("Select Action:");
                            Console.WriteLine("1: Update Title");
                            Console.WriteLine("2: Update Description");
                            Console.WriteLine("3: Delete Blog");
                            Console.WriteLine();
                            Console.WriteLine("Enter your choice");
                            string choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "1":
                                    Console.WriteLine("Enter new Title: ");
                                    string newTitle = Console.ReadLine();
                                    UpdateBlogTitle(blogId, newTitle);
                                    Console.WriteLine("Blog Title Updated Successfully");
                                    break;
                                case "2":
                                    Console.WriteLine("Enter new Description: ");
                                    string newDesc = Console.ReadLine();
                                    UpdateBlogDescription(blogId, newDesc);
                                    Console.WriteLine("Blog Description Updated Successfully");
                                    break;
                                case "3":
                                    Console.WriteLine($"Are you sure you want to delete Blog {reader.GetString(1)}");
                                    Console.WriteLine("Type 'DELETE' to confirm");
                                    string finalChoice = Console.ReadLine();
                                    if (finalChoice == "DELETE")
                                    {
                                        DeleteBlog(blogId);
                                        Console.WriteLine("Blog deleted Successfully");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Blog deletion Cancelled");
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Invalid Option");
                                    break;
                            }
                        }
                    }
                }
            }
        }


        // HELPER CLASSES

        private bool BlogExists(int blogId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM blogs WHERE ID = @BlogID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BlogID", blogId);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        private void UpdateBlogTitle(int blogId, string newTitle)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE blogs SET Name = @NewTitle WHERE ID = @BlogID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewTitle", newTitle);
                    command.Parameters.AddWithValue("@BlogID", blogId);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateBlogDescription(int blogId, string newDesc)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE blogs SET Descr = @NewDesc WHERE ID = @BlogID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewDesc", newDesc);
                    command.Parameters.AddWithValue("@BlogID", blogId);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteBlog(int blogId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM blogs WHERE ID = @BlogID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BlogID", blogId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

