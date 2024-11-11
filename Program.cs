﻿﻿using System.ComponentModel.Design;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

var db = new DataContext();

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
  if (userChoice == "1")
  {
    DisplayBlogs();
  }
  else if (userChoice == "2")
  {
    AddBlog();
  }
  else if (userChoice == "3")
  {
    DisplayPosts(SelectBlog());
  }
  else if (userChoice == "4")
  {
    Blog? blog = SelectBlog();
    while (blog is null){
      Console.WriteLine("You must select a single blog for posting!");
      blog = SelectBlog();
    }
    WritePost(blog);
  }
  Console.WriteLine("\nPress enter to continue");
  Console.ReadLine();
  Console.Clear();
}
Console.WriteLine("Goodnye!");

logger.Info("Program ended");

//TODO SELECT BLOG FUNCTION

void DisplayPosts(Blog? blog)
{
  // Display all Blogs from the database
  IOrderedQueryable<Post> query;
  if (blog is null){
  Console.WriteLine("Displaying all posts");
    query = db.Posts.OrderBy(p => p.Title);
  }
  else{
    Console.WriteLine($"Displaying posts in {blog.Name}");
    query = db.Posts.Where(p => p.BlogId == blog.BlogId).OrderBy(p => p.Title);
  }
  foreach (Post post in query)
  {
    Console.WriteLine($"\nTitle: {post.Title}\n");
    Console.WriteLine($"Content: {post.Content}\n");
  }
}

Blog? SelectBlog()
{
  // Display all Blogs from the database
  var query = db.Blogs.OrderBy(b => b.Name);
  Console.WriteLine("Available Blogs");
  for (int i = 0; i < query.Count(); i++)
  {
    Console.WriteLine($"{i + 1} - {query.ElementAt(i).Name}");
  }
  //allows user to select a blog
  Console.Write("Select Blog (0 for all): ");
  int selection = 0;
  while (!Int32.TryParse(Console.ReadLine(), out selection) || selection < 0 || selection > query.Count())
  {
    Console.Write("Invalid selection, try again: ");
  }
  if (selection == 0) return null;
  return query.ElementAt(selection - 1);
  //returns unique ID
}

void WritePost(Blog blog){
  Console.Write("Enter title for your post: ");
  string? title = Console.ReadLine();
  while (title.IsNullOrEmpty()){
    Console.Write("Please enter a valid title: ");
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
  Console.WriteLine("Displaying Blogs");
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
    Console.Write("Invalid entry, try again: ");
    name = Console.ReadLine() ?? "";
  }

  var blog = new Blog { Name = name };
  db.AddBlog(blog);
  logger.Info("Blog added - {name}", name);
}