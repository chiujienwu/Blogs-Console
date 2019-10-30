using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");

            string menuOption;
            var db = new BloggingContext();

            do
            {   //menu for app
                Console.WriteLine("Blogs-Consol Application");
                Console.WriteLine("========================");
                Console.WriteLine("1-Display all blogs");
                Console.WriteLine("2-Add blog");
                Console.WriteLine("3-Create post");
                Console.WriteLine("5-Exit program");
                menuOption = Console.ReadLine();

                if (menuOption == "1")
                {
                    try
                    {
                        // Display all Blogs from the database - this is LINQ
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.Write(item.BlogId + "-");
                            Console.WriteLine(item.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }

                if (menuOption == "2")
                {
                    try
                    {   // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }

                if (menuOption == "3")
                {
                    try
                    {   // Create a post for a selected blog
                        // display list of current blogs
                        // display all Blogs from the database - this is LINQ
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.Write(item.BlogId + "-");
                            Console.WriteLine(item.Name);
                        }

                        Console.Write("Enter blog id to write a post:");
                        var i = Console.ReadLine();
                        var id = System.Convert.ToInt32(i);
                        logger.Info("Blog ID seklected - {id}", id);

                        var blog = db.Blogs.FirstOrDefault(b => b.BlogId == id);
                        var name = blog.Name;

                        Console.Write("Enter a title for a new post to blog " + name + ": ");
                        var title = Console.ReadLine();

                        Console.WriteLine("Enter your post content below: ");
                        var content = Console.ReadLine();

                        var post = new Post { Title = title, Content = content, BlogId = id, Blog = blog};

                        db.AddPost(post);
                        logger.Info("Post added to BlogID {id} - Title {title}", id, title);

                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }


            } while (menuOption == "1" || menuOption == "2" || menuOption == "3");

            logger.Info("Program ended");
        }
    }
}

