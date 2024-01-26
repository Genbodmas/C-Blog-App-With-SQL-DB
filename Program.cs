using System;

namespace BlogApplication_db
{
    class Program
    {
        static void Main(string[] args)
        {
            var blogManager = new BlogManager();
            bool loggedIn = false;
            user currentUser = null;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("WELCOME TO BLOG MANAGER");

                if (!loggedIn)
                {
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.WriteLine("3: Exit");
                }
                else
                {
                    Console.WriteLine($"Welcome, {blogManager.LoggedInUser.FirstName}!");
                    Console.WriteLine("4: Display all Blogs");
                    Console.WriteLine("5: Display all Posts");
                    Console.WriteLine("6: Create new Blog");
                    Console.WriteLine("7: Create new Post");
                    Console.WriteLine("8: Delete a Post");
                    Console.WriteLine("9: Manage Blog");
                    Console.WriteLine("10: Logout");
                }

                Console.WriteLine();

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (!loggedIn)
                        {
                            // Login
                            Console.WriteLine("Enter Username to login: ");
                            string username = Console.ReadLine();

                            if (blogManager.AuthenticateUser(username))
                            {
                                currentUser = blogManager.LoggedInUser;
                                Console.WriteLine($"Welcome back, {currentUser.FirstName}!");
                                loggedIn = true;
                            }
                            else
                            {
                                Console.WriteLine($"Username '{username}' not found.");

                                // Ask for Email if Username didn't work
                                Console.WriteLine("Try Entering Email to login: ");
                                string email = Console.ReadLine();

                                if (blogManager.AuthenticateUser(email, true))
                                {
                                    currentUser = blogManager.LoggedInUser;
                                    Console.WriteLine($"Welcome back, {currentUser.FirstName}!");
                                    loggedIn = true;
                                }
                                else
                                {
                                    Console.WriteLine($"User with email '{email}' not found.");

                                    // Prompt user to register
                                    Console.WriteLine("User not found. Do you want to register? (yes/no)");
                                    string registerChoice = Console.ReadLine().ToLower();

                                    if (registerChoice == "yes")
                                    {
                                        // Create a new user and register
                                        user newUser = new user();
                                        Console.WriteLine("Enter Username: ");
                                        newUser.Username = Console.ReadLine();
                                        Console.WriteLine("Enter Email: ");
                                        newUser.Email = Console.ReadLine();
                                        Console.WriteLine("Enter Phone: ");
                                        newUser.Phone = Console.ReadLine();
                                        Console.WriteLine("Enter First Name: ");
                                        newUser.FirstName = Console.ReadLine();
                                        Console.WriteLine("Enter Last Name: ");
                                        newUser.LastName = Console.ReadLine();

                                        if (blogManager.RegisterUser(newUser) != null)
                                        {
                                            Console.WriteLine("Registration successful. Please log in.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Registration failed.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Login canceled.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Already logged in. Logout to login as a different user.");
                        }
                        break;

                    case "2":
                        if (!loggedIn)
                        {
                            // Register
                            Console.WriteLine("Enter Username: ");
                            string regUsername = Console.ReadLine();
                            Console.WriteLine("Enter Email: ");
                            string regEmail = Console.ReadLine();
                            Console.WriteLine("Enter Phone: ");
                            string regPhone = Console.ReadLine();
                            Console.WriteLine("Enter First Name: ");
                            string regFirstName = Console.ReadLine();
                            Console.WriteLine("Enter Last Name: ");
                            string regLastName = Console.ReadLine();

                            user newUser = new user
                            {
                                Username = regUsername,
                                Email = regEmail,
                                Phone = regPhone,
                                FirstName = regFirstName,
                                LastName = regLastName
                            };

                            if (blogManager.RegisterUser(newUser) != null)
                            {
                                Console.WriteLine("Registration successful. Please log in.");
                            }
                            else
                            {
                                Console.WriteLine("Registration failed.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Already logged in. Logout to register as a different user.");
                        }
                        break;

                    case "3":
                        Environment.Exit(0);
                        break;

                    case "4":
                        blogManager.DisplayAllBlog();
                        break;

                    case "5":
                        blogManager.DisplayAllPost();
                        break;

                    case "6":
                        blogManager.CreateNewBlog();
                        break;

                    case "7":
                        blogManager.CreateNewPost();
                        break;

                    case "8":
                        blogManager.DeletePost();
                        break;

                    case "9":
                        blogManager.ManageBlog();
                        break;

                    case "10":
                        if (loggedIn)
                        {
                            // Logout
                            loggedIn = false;
                            currentUser = null;
                            Console.WriteLine("Logout successful.");
                        }
                        else
                        {
                            Console.WriteLine("Not logged in.");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid Command, Please try again");
                        break;
                }
            }
        }
    }
}
