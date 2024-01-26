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

        private user loggedInUser;

        public void DisplayAllBlog()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM blogs WHERE UserName = @UserName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", loggedInUser.Username);

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
                                    Desc = reader.GetString(2),
                                    UserName = reader.GetString(3)
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
                string query = "SELECT * FROM posts WHERE UserName = @UserName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", loggedInUser.Username);

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
                                    Content = reader.GetString(4),
                                    UserName = reader.GetString(5)
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


        public void CreateNewBlog()
        {
            Console.WriteLine("Enter Title of blog: ");
            string title = Console.ReadLine();

            Console.WriteLine("Enter blog description: ");
            string desc = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO blogs (Name, Descr, UserName) VALUES (@Name, @Desc, @UserName)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", title);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.Parameters.AddWithValue("@UserName", loggedInUser.Username);

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

            DateTime date = DateTime.Now;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO posts (BlogID, Date, Title, Content, UserName) " +
                               "VALUES (@BlogID, @Date, @Title, @Content, @UserName)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BlogID", blogId);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Content", content);
                    command.Parameters.AddWithValue("@UserName", loggedInUser.Username);

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
                string query = "DELETE FROM posts WHERE ID = @PostID AND BlogID = @BlogID AND UserName = @UserName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PostID", postId);
                    command.Parameters.AddWithValue("@BlogID", blogId);
                    command.Parameters.AddWithValue("@UserName", loggedInUser.Username);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Post Deleted Successfully");
                    }
                    else
                    {
                        Console.WriteLine("Post not found or not associated with the specified blog or user.");
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
                string query = "SELECT * FROM blogs WHERE ID = @BlogID AND UserName = @UserName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BlogID", blogId);
                    command.Parameters.AddWithValue("@UserName", loggedInUser.Username);

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


        // USER AUTHENTICAtiON


        public bool AuthenticateUser(string identifier, bool isEmail = false)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string columnName = isEmail ? "Email" : "Username";
                string query = $"SELECT * FROM users WHERE {columnName} = @Identifier";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Identifier", identifier);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            loggedInUser = new user
                            {
                                ID = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Email = reader.GetString(2),
                                Phone = reader.GetString(3),
                                FirstName = reader.GetString(4),
                                LastName = reader.GetString(5)
                            };
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public user RegisterUser(user newUser)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO users (Username, Email, Phone, FirstName, LastName) " +
                                "VALUES (@Username, @Email, @Phone, @FirstName, @LastName)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", newUser.Username);
                    command.Parameters.AddWithValue("@Email", newUser.Email);
                    command.Parameters.AddWithValue("@Phone", newUser.Phone);
                    command.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", newUser.LastName);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Registration successful, return the user object
                        return newUser;
                    }
                    else
                    {
                        // Registration failed, return null or handle accordingly
                        return null;
                    }
                }
            }
        }

        public user LoggedInUser
        {
            get { return loggedInUser; }
        }
    }
}

