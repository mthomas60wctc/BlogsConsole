﻿using Microsoft.IdentityModel.Tokens;
using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
Logger logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

DataContext db = new();

string? userChoice = "";
Console.Clear();

while (true)
{
  Console.WriteLine("-- BLOG DATABASE MENU --");
  Console.WriteLine("1 - View existing blogs");
  Console.WriteLine("2 - Make new blog");
  Console.WriteLine("3 - View existing Posts");
  Console.WriteLine("4 - Create new Post");
  Console.Write("Select (enter to quit): ");
  userChoice = Console.ReadLine();
  if (userChoice.IsNullOrEmpty())
  {
    Console.WriteLine("Exiting program");
    break;
  }
  Console.Clear();
  if (userChoice == "1")
  {
    Console.WriteLine("-- DISPLAY BLOGS --");
    DisplayBlogs();
  }
  else if (userChoice == "2")
  {
    Console.WriteLine("-- ADD BLOG --");
    AddBlog();
  }
  else if (userChoice == "3")
  {
    Console.WriteLine("-- DISPLAY POSTS --");
    DisplayPosts(SelectBlog());
  }
  else if (userChoice == "4")
  {
    Console.WriteLine("-- ADD POST --");
    Blog? blog = SelectBlog();
    if (blog is not null)
    {
      WritePost(blog);
    }
  }
  Console.WriteLine("\nPress enter to continue");
  Console.ReadLine();
  Console.Clear();
}
Console.WriteLine("Goodnye!");

logger.Info("Program ended");


//FUNCTIONS
void DisplayPosts(Blog? blog)
{
  // Display all Blogs from the database
  IOrderedQueryable<Post> query;
  string currentBlog = "";
  if (blog is null)
  {
    Console.WriteLine("Displaying all posts");
    query = db.Posts.OrderBy(p => p.Title).OrderBy(p => p.Blog.Name);
  }
  else
  {
    query = db.Posts.Where(p => p.BlogId == blog.BlogId).OrderBy(p => p.Title);
  }
  foreach (Post post in query)
  {
    if (currentBlog != post.Blog?.Name)
    {
      currentBlog = post.Blog?.Name ?? "";
      Console.WriteLine($"\nDisplaying posts in {currentBlog}");
    }
    Console.WriteLine($"\n\tTitle: {post.Title}\n");
    Console.WriteLine($"\tContent: {post.Content}\n");
  }
}

Blog? SelectBlog()
{
  // Display all Blogs from the database
  var query = db.Blogs.OrderBy(b => b.Name);
  Console.WriteLine($"{query.Count()} available Blogs");
  if (query.Count() == 0)
  {
    logger.Error("There msut be at least 1 blog to make a post");
    return null;
  }
  for (int i = 0; i < query.Count(); i++)
  {
    Console.WriteLine($"{i + 1} - {query.ElementAt(i).Name}");
  }
  //allows user to select a blog
  Console.Write("Select Blog (0 for all): ");
  int selection = 0;
  while (!Int32.TryParse(Console.ReadLine(), out selection) || selection < 0 || selection > query.Count())
  {
    logger.Error($"Selection must be between 0 and {query.Count()} (inculsive)");
    Console.Write("Select Blog (0 for all): ");
  }
  if (selection == 0) return null;
  return query.ElementAt(selection - 1);
  //returns null if user selected nothing, otherwise returns a blog object
}

void WritePost(Blog blog)
{
  Console.Write($"Enter title for your post in {blog.Name}: ");
  string? title = Console.ReadLine();
  while (title.IsNullOrEmpty())
  {
    logger.Error("Title cannot be empty");
    Console.Write($"Enter title for your post in {blog.Name}: ");
    title = Console.ReadLine();
  }
  Console.Write($"Enter content for post '{title}': ");
  string? content = Console.ReadLine();
  Post post = new()
  {
    Title = title,
    Content = content,
    BlogId = blog.BlogId,
    Blog = blog
  };
  db.AddPost(post);
  logger.Info($"Post added to {blog.Name} - {title}");
}

void DisplayBlogs()
{
  // Display all Blogs from the database
  var query = db.Blogs.OrderBy(b => b.Name);
  Console.WriteLine($"Displaying {query.Count()} Blogs");
  foreach (Blog blog in query)
  {
    Console.WriteLine(blog.Name);
  }
}

void AddBlog()
{
  // Create and save a new Blog
  Console.Write("Enter a name for a new Blog: ");
  string name;
  name = Console.ReadLine() ?? "";
  while (name.IsNullOrEmpty())
  {
    logger.Error("Post title cannot be empty");
    Console.Write("Enter a name for a new Blog: ");
    name = Console.ReadLine() ?? "";
  }

  Blog blog = new Blog { Name = name };
  db.AddBlog(blog);
  logger.Info($"Blog added - {name}");
}