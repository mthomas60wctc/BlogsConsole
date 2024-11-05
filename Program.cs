﻿using System.Xml;
using System.Xml.Serialization;
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
    displayBlogs();
  }
  else if (userChoice == "2")
  {
    addBlog();
  }
  else if (userChoice == "3"){
    //TODO: VIEW POSTS
  }
  else if (userChoice == "4"){
    //TODO: MAKE A NEW POST
  }
  Console.WriteLine("Press enter to continue");
  Console.ReadLine();
  Console.Clear();
}
Console.WriteLine("Goodnye!");

logger.Info("Program ended");

//TODO SELECT BLOG FUNCTION
//TODO DISPLAY POSTS FUNCTION
//TODO READ POST FUNCTION
//TODO:WRITE POST FUNCTION

void displayBlogs()
{
  //TODO: LIST NUMBERS ALONGSIDE BLOG
  // Display all Blogs from the database
  var query = db.Blogs.OrderBy(b => b.Name);
  Console.WriteLine("Displaying Blogs");
  foreach (var item in query)
  {
    Console.WriteLine(item.Name);
  }
}

void addBlog()
{
  // Create and save a new Blog
  Console.Write("Enter a name for a new Blog: ");
  var name = Console.ReadLine();

  var blog = new Blog { Name = name };
  db.AddBlog(blog);
  logger.Info("Blog added - {name}", name);
}