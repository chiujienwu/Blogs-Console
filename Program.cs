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
                Console.WriteLine("Blogs-Console Application");
                Console.WriteLine("========================");
                Console.WriteLine("1-Display all blogs");
                Console.WriteLine("2-Add blog");
                Console.WriteLine("3-Create post");
                Console.WriteLine("4-Display posts");
                Console.WriteLine("5-Exit program");
                Console.Write("Enter your selection 1-5: ");
                menuOption = Console.ReadLine();

                if (menuOption == "1")
                {
                    try
                    {
                        // Display all Blogs from the database - this is LINQ
                        logger.Info("Option {menuOption} selected", menuOption);

                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        var count = db.Blogs.Count();

                        Console.WriteLine(count + " Blogs returned");

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.Write(item.BlogId + "-");
                            Console.WriteLine(item.Name);
                        }

                        Console.WriteLine();

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

                        var name = "";

                        do
                        {
                            Console.Write("Enter a name for a new Blog: ");
                            name = Console.ReadLine();
                            if (name == "")
                            {
                                logger.Info("Blog name cannot be null");
                            }
                        } while (name == "");


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

                        // display all Blogs from the database - this is LINQ
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        // display list of current blogs
                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.Write(item.BlogId + "-");
                            Console.WriteLine(item.Name);
                        }

                        var id = 0;

                        do
                        {
                            Console.Write("Select the blog you would like to post to: ");
                            var idString = Console.ReadLine();

                            // validate BlogID is a valid integer

                            if (!Int32.TryParse(idString, out id))
                            {
                                logger.Info("Invalid ID entered {idString}", idString);
                                id = -1;
                            }

                            // check to see if selected BlogID exists

                            if (!db.Blogs.Any(b => b.BlogId == id))
                            {
                                logger.Info("Blog ID - {id} is incorrect or missing", id);
                                id = -1;
                            }

                        } while (id < 0);

                        logger.Info("Blog ID selected - {id}", id);

                        var blog = db.Blogs.FirstOrDefault(b => b.BlogId == id);
                        var name = blog.Name;

                        //post title cannot be blank

                        var title = "";

                        do
                        {
                            Console.Write("Enter the title for a new post to blog " + name + ": ");
                            title = Console.ReadLine();
                            if (title == "")
                            {
                                logger.Info("Post title name cannot be null");
                            }
                        } while (title == "");

                        Console.WriteLine("Enter your post content below: ");
                        var content = Console.ReadLine();

                        var post = new Post { Title = title, Content = content, BlogId = id, Blog = blog };

                        db.AddPost(post);
                        logger.Info("Post added to BlogID {id} - Title {title}", id, title);

                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }

                if (menuOption == "4")
                {
                    //display posts from blogs

                    //submenu options

                    Console.WriteLine("Select the blog's posts to display: ");
                    Console.WriteLine("0 - Posts from all blogs");
                    var query = db.Blogs.OrderBy(b => b.BlogId);

                    foreach (var item in query)
                    {
                        Console.Write(item.BlogId + " - ");
                        Console.WriteLine(item.Name);
                        //Console.WriteLine("{id} - Posts from all {name}", item.BlogId, item.Name);
                    }

                    var id = 0;

                    //do
                    //{
                    //    var idString = Console.ReadLine();

                    //    // validate BlogID is a valid integer

                    //if (!Int32.TryParse(idString, out id))
                    //{
                    //    logger.Info("Invalid ID entered {idString}", idString);
                    //    id = -1;
                    //}

                    //    // check to see if selected BlogID exists

                    //    if (!db.Blogs.Any(b => b.BlogId == id))
                    //    {
                    //        logger.Info("Blog ID - {id} is incorrect or missing", id);
                    //        id = -1;
                    //    }

                    //} while (id < 0);

                    var idString = Console.ReadLine();
                    if (!Int32.TryParse(idString, out id))
                    {
                        logger.Info("Invalid ID entered {idString}", idString);
                        id = -1;
                    }
                    logger.Info("Option 4 submenu selection - {id}", id);
                    logger.Info("Querying post");

                    if (id == 0)
                    {
                        //prints all posts from all blogs
                        var queryPost = db.Posts.OrderBy(b => b.BlogId);
                        foreach (var detail in queryPost)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Blog: " + detail.Blog.Name);
                            Console.WriteLine("Title: " + detail.Title);
                            Console.WriteLine("Content: " + detail.Content);
                        }
                        Console.WriteLine(queryPost.Count() + " post(s) returned");
                        Console.WriteLine();

                    } else
                    {
                        var queryBlogPost = db.Posts.Where(p => p.BlogId == id);
                        foreach (var detail in queryBlogPost)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Blog: " + detail.Blog.Name);
                            Console.WriteLine("Title: " + detail.Title);
                            Console.WriteLine("Content: " + detail.Content);
                        }
                        Console.WriteLine(queryBlogPost.Count() + " post(s) returned");
                        Console.WriteLine();
                    }


                    logger.Info("Option {option} selected", menuOption);
                }


            } while (menuOption == "1" || menuOption == "2" || menuOption == "3" || menuOption == "4");

            logger.Info("Program ended");
        }
    }
}