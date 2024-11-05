﻿using System.Xml;
using System.Xml.Serialization;
using Microsoft.IdentityModel.Tokens;
using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");


// // Create and save a new Blog
// Console.Write("Enter a name for a new Blog: ");
// var name = Console.ReadLine();

// var blog = new Blog { Name = name };

// var db = new DataContext();
// db.AddBlog(blog);
// logger.Info("Blog added - {name}", name);

// // Display all Blogs from the database
// var query = db.Blogs.OrderBy(b => b.Name);

// Console.WriteLine("All blogs in the database:");
// foreach (var item in query)
// {
//   Console.WriteLine(item.Name);
// }

string? userChoice = "";
Console.Clear();

while (true){
  Console.WriteLine("-- BLOG DATABASE MENU --");
  Console.WriteLine("1 - View existing blogs");
  Console.WriteLine("2 - Make new blog");
  Console.WriteLine("3 - View existing Posts");
  Console.WriteLine("4 - Create new Post");
  Console.Write("Select (enter to quit): ");
  userChoice = Console.ReadLine();
  if (userChoice.IsNullOrEmpty()){
    Console.WriteLine("Exiting program");
    break;
  }
  Console.WriteLine("Press enter to continue");
  Console.ReadLine();
  Console.Clear();
}
Console.WriteLine("Goodnye!");

logger.Info("Program ended");