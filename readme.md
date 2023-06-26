# Table of Contents

- [Introduction to Dotnet](#introduction-to-dotnet)
- [Installation](#installation)
- [MVC Architecture](#mvc-architecture)
- [How MVC Works](#how-mvc-works)
- [Folder Structure of Dotnet MVC App](#folder-structure-of-dotnet-mvc-app)
- [Basic Crud Example](#basic-crud-example)
- [Entity Framework Core](#entity-framework-core)
- [Entity Framework Core Installation](#entity-framework-core-installation)
- [Entity Framework Core Model](#entity-framework-core-model)
  - [Creating the database Context](#creating-the-database-context)
    - [Register the Context](#register-the-context)
  - [Migrations](#migrations)
  - [Create Your Initial Migration](#create-your-initial-migration)

## Introduction to Dotnet

On Dotnet MVC, CRUD operations are carried out.One of the Entity Framework examples was created using [Microsoft's](https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/?view=aspnetcore-7.0) official documentation.

## Installation

To create a dotnet MVC app at first you have to install dotnet cli and dotnet sdk or dotnet runtime in your system.We'll see the installation process on linux.You can see the installation process from [here](https://learn.microsoft.com/en-us/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website)

You can install dotnet in following ways:

- Manual Installation
- Scripted Installation

We will be using script installation for this process

You can download the script with wget:

```
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
```

Before running this script, you'll need to grant permission for this script to run as an executable:

```
sudo chmod +x ./dotnet-install.sh
```

The script defaults to installing the latest [long term support (LTS)](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) SDK version, which is .NET 6. To install the latest release, which may not be an (LTS) version, use the --version latest parameter.

```
./dotnet-install.sh --version latest
```

To install .NET Runtime instead of the SDK, use the --runtime parameter.

```
./dotnet-install.sh --version latest --runtime aspnetcore
```

You can install a specific major version with the --channel parameter to indicate the specific version. The following command installs .NET 7.0 SDK.

```
./dotnet-install.sh --channel 7.0
```

## MVC Architecture

The Model-View-Controller (MVC) framework is an architectural/design pattern that separates an application into three main logical components Model, View, and Controller.

![alt text](https://res.cloudinary.com/codewithsudeep/image/upload/v1687752549/Content%20for%20github/Screenshot_from_2023-06-26_09-52-58_d3kfhw.png)

## How MVC Works

First, the browser sends a request to the Controller. Then, the Controller interacts with the Model to send and receive data. The Controller then interacts with the View to render the data.

## Folder Structure of Dotnet MVC App

![folder structure of mvc app](https://res.cloudinary.com/codewithsudeep/image/upload/v1687754358/Content%20for%20github/Screenshot_from_2023-06-26_10-24-08_l5sd2e.png)

A controller is responsible for controlling the way that a user interacts with an MVC application. A controller contains the flow control logic for an ASP.NET MVC application. A controller determines what response to send back to a user when a user makes a browser request.

An MVC model contains all of your application logic that is not contained in a view or a controller. The model should contain all of your application business logic, validation logic, and database access logic. For example, if you are using the Microsoft Entity Framework to access your database, then you would create your Entity Framework classes (your .edmx file) in the Models folder.

A view contains the HTML markup and content that is sent to the browser. A view is the equivalent of a page when working with an ASP.NET MVC application.

## Basic Crud Example

For this example we'll be using Mysql.For using Mysql Database we'll be using MysqlConnector,let's install that using dotnet cli

```
dotnet add package MySqlConnector --version 2.2.6
```

We define our crud logic in our controller as it defines actions for our http requests.

At first let's define our data in our model

<details>
<summary open>Model</summary>

```
using System.ComponentModel.DataAnnotations;
namespace MvcMovie.Models;

public class MovieViewModel {

    public int id { get ; set;}

    public string? genre {get; set; }

    public string? Title { get; set; }

    [DataType(DataType.Date)]
    public DateOnly releaseDate { get; set; }

}

```

</details>

For this purpose let's create a generic list of our movie model in our controller. For full code you can see Controller > MovieController.cs

```

    private List<MovieViewModel> movies = new List<MovieViewModel>();

```

Let's define our controller for listing our movies list in Homepage

<details>
<summary>View Controller</summary>

```

    public IActionResult Index()
    {
        using var connection = new MySqlConnection("Server=localhost;User ID=root;password=7227;Database=movies;ConvertZeroDatetime=True");
        connection.Open();

        string query = "Select * from movies";
        using var command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            MovieViewModel movie = new MovieViewModel();
            movie.id = Convert.ToInt32(reader["id"]);
            movie.Title = reader["title"].ToString();
            movie.genre = Convert.ToString(reader["genre"]);
            movie.releaseDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["release_year"]));
            movies.Add(movie);
        }
        return View(movies);
    }

```

</details>

Let's define our View for Movies Home in Views > Movie > Index.cshtml

<details>
<summary>View Page</summary>

```

@{
ViewData["Title"] = "Movie Page";
}

<div class="text-center">
  <h1 class="display-4">Welcome to Movie Page</h1>
  <a asp-controller="Movie" asp-action="NewMovie" >Create a new Movie</a>
  <table>
    <thead>
      <tr>
        <th>Id</th>
        <th>Genre</th>
        <th>Title</th>
        <th>ReleaseDate</th>
      </tr>
    </thead>
    <tbody>
      @foreach (MovieViewModel movie in Model)
      {
        <tr class="flex gap-5">
          <td>@movie.id</td>
          <td>@movie.genre</td>
          <td>@movie.Title</td>
          <td>@movie.releaseDate</td>
          <td style="diplay:flex;gap:5px;">
            <a
              asp-action="Detail"
              asp-route-id="@movie.id"
              class="btn btn-primary"
              >View</a
            >
            <a
              asp-action="Edit"
              asp-route-id="@movie.id"
              class="btn btn-primary"
              >Edit</a
            >
            <a asp-action="Delete" asp-route-id="@movie.id" class="btn btn-danger"
              >Delete</a
            >
          </td>
        </tr>
      }
    </tbody>
  </table>
</div>
```

</details>

Let's define our controller for detail page for each movie:

<details>
<summary>Detail Controller</summary>

```
    public IActionResult Detail(int id)
    {
        using var connection = new MySqlConnection("Server=localhost;User ID=root;password=7227;Database=movies;ConvertZeroDatetime=True;");
        connection.Open();

        string query = $"select * from movies where id = {id} ";
        using var command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();
        MovieViewModel movie = new MovieViewModel();
        while (reader.Read())
        {
            movie.id = Convert.ToInt32(reader["id"]);
            movie.genre = reader["genre"].ToString();
            movie.Title = reader["Title"].ToString();
            movie.releaseDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["release_year"]));
        }
        return View(movie);
    }
```

</details>

Our view for detail page:

<details>
<summary>Detail View Page</summary>

```
@model MvcMovie.Models.MovieViewModel;

<div class="card">
    <div class="card-header">
        Movie Details
    </div>
    <div class="card-body">
        <h4>ID :  @Model.id </h4>
        <h4>Genre: @Model.genre </h4>
        <h4>Title: @Model.Title </h4>
        <h4>Release Date: @Model.releaseDate </h4>
    </div>
</div>
```

</details>

Let's define our controller for creating a new movie:

<details>
<summary>New Movie Controller</summary>

```
    public IActionResult NewMovie()
    {
        return View();
    }

```

```
    [HttpPost]
    public IActionResult NewMovie(MovieViewModel movie)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Not a valid model");
        }

        using var connection = new MySqlConnection("Server=localhost;User ID=root;password=7227;Database=movies;ConvertZeroDatetime=True");
        connection.Open();

        string query = $"INSERT into movies (id,genre,title,release_year) VALUES({movie.id},'{movie.genre}','{movie.Title}',STR_TO_DATE('{movie.releaseDate}','%m/%d/%Y'))";
        // string query = $"INSERT into movies (id,genre,title,release_year) VALUES({movie.id},'{movie.genre}','{movie.Title}','{movie.releaseDate}' )";

        using var command = new MySqlCommand(query, connection);
        int reader = command.ExecuteNonQuery();
        ViewData["Movie"] = "Done";
        return Redirect("/Movie");
    }
```

</details>

Let's create our view for new movie :

<details>
<summary>New Movie View Page</summary>

```
@{
@using MvcMovie.Models;
@model MovieViewModel;

}

<div class="" style="diplay:flex;flex-direction:column;gap:10px;">
    <p>@ViewData["movie"]</p>
    <h2>Create Movie</h2>
    <form method="post" asp-action="NewMovie" asp-controller="Movie" style="display:flex;flex-direction:column;gap:10px;" >
        <label asp-for="id">Id:</label>
        <input type="text" asp-for="id">
        <span asp-validation-for="id"></span>

        <label for="genre">Genre:</label>
        <input type="text" asp-for="genre">
        <span asp-validation-for="genre"></span>


        <label for="Title">Title:</label>
        <input type="text" asp-for="Title">
        <span asp-validation-for="Title"></span>


        <label for="releaseDate">Release Date</label>
        <input type="date" asp-for="releaseDate">
        <span asp-validation-for="releaseDate"></span>


        <input type="submit" value="Create Movie" class="btn btn-success">

    </form>
</div>
```

</details>

Let's create our controller for updating/Editing a movie

<details>
<summary>Update/Edit Controller</summary>

```
    public IActionResult Edit(int id)
    {
        using var connection = new MySqlConnection("Server=localhost;User ID=root;password=7227;Database=movies;ConvertZeroDatetime=True");
        connection.Open();

        string query = $"select * from movies where id = {id}";
        using var command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();
        MovieViewModel movie = new MovieViewModel();
        while (reader.Read())
        {
            movie.id = Convert.ToInt32(reader["id"]);
            movie.genre = Convert.ToString(reader["genre"]);
            movie.Title = Convert.ToString(reader["Title"]);
            movie.releaseDate = DateOnly.FromDateTime(Convert.ToDateTime((reader["release_year"])));
        }
        return View(movie);
    }
```

```
    [HttpPost]
    public IActionResult Edit(MovieViewModel movie)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("model doesnot exist");
        }

        using var connection = new MySqlConnection("Server=localhost;User ID=root;password=7227;Database=movies;ConvertZeroDatetime=True");
        connection.Open();

        string query = $"UPDATE movies SET genre = '{movie.genre}', title = '{movie.Title}', release_year = STR_TO_DATE('{movie.releaseDate}','%m/%d/%Y')";
        using var command = new MySqlCommand(query, connection);
        int reader = command.ExecuteNonQuery();
        ViewData["movies"] = "Updated";
        return View();

}

```

</details>

Let's define our view for updating a movie:

<details>
<summary>Update View</summary>

```
@{
@using MvcMovie.Models;
@model MovieViewModel;

if(ViewData.ContainsKey("movies")){
    <div class="alert alert-success alert-dismissible">
        @ViewData["movies"];
    </div>
}
}

<div class="" style="diplay:flex;flex-direction:column;gap:10px;">
    <h2>Create Movie</h2>
    <form method="post" asp-action="Edit" asp-controller="Movie" style="display:flex;flex-direction:column;gap:10px;" >
        <label asp-for="id">Id:</label>
        <input type="text" asp-for="id">
        <span asp-validation-for="id"></span>

        <label for="genre">Genre:</label>
        <input type="text" asp-for="genre">
        <span asp-validation-for="genre"></span>


        <label for="Title">Title:</label>
        <input type="text" asp-for="Title">
        <span asp-validation-for="Title"></span>


        <label for="releaseDate">Release Date</label>
        <input type="date" asp-for="releaseDate">
        <span asp-validation-for="releaseDate"></span>


        <input type="submit" value="Update Movie" class="btn btn-success">

    </form>
</div>
```

</details>

Let's define our Controller for deleting a movie :

<details>
<summary>Delete Controller</summary>

```
    public IActionResult Delete(MovieViewModel movie)
    {
        using var connection = new MySqlConnection("Server=localhost;User ID=root;password=7227;Database=movies");
        connection.Open();

        string query = $"DELETE from movies where id = {movie.id} ";
        using var command = new MySqlCommand(query, connection);
        int reader = command.ExecuteNonQuery();
        return Redirect("/Movie");
    }
```

</details>

## Entity Framework Core

Entity Framework (EF) Core is a lightweight, extensible, open source and cross-platform version of the popular Entity Framework data access technology.

EF Core can serve as an object-relational mapper (O/RM), which:

- Enables .NET developers to work with a database using .NET objects.
- Eliminates the need for most of the data-access code that typically needs to be written.

For more details you can see [here](https://learn.microsoft.com/en-us/ef/core/)

## Entity Framework Core Installation

We'll need **EntityFrameworkCore** **EntityFrameworkCore.Design** **Mysql.Data,Mysql.EntityFrameworkCore** to work with entity framework and mysql using entity framework.

```
dotnet add package Microsoft.EntityFrameworkCore
```

```
dotnet add package Microsoft.EntityFrameworkCore.Design
```

```
dotnet add package MySql.Data
```

```
dotnet add package Mysql.EntityFrameworkCore
```

# Entity Framework Core Model

With EF Core, data access is performed using a model. A model is made up of entity classes and a context object that represents a session with the database. The context object allows querying and saving data.

Let's create a model for our products data:

```
namespace MvcMovie.Models;

public class ProductModel {
    public int id { get; set; }

    public string? title { get; set;}

    public double price { get; set; }
}

```

## Creating the database Context

The main class that coordinates EF functionality for a given data model is the DbContext database context class. This class is created by deriving from the **Microsoft.EntityFrameworkCore.DbContext class**. The DbContext derived class specifies which entities are included in the data model. Some EF behaviors can be customized. In this project, the class is named _ProductContext_.

We are going to create a folder name DAL(Data accress Layer) where we are going to create our context to coordinate with the data model using entity framework core.

<details>
<summary>ProductContext Class</summary>

```
// It is the main class that coordinates entity framework functionality for a given data model
// DAL - Data Access Layer

using MvcMovie.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcMovie.DAL
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options){

        }
        #nullable disable
        public DbSet<ProductModel> Products  { get; set; }


        // this method prevents table name from being plualized eg:- products
        // this is a personal perference method so you can omit or include this method

        // protected override void OnModelCreating(DbModelBuilder modelBuilder)
        // {
        //     modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        // }
    }
}

```

</details>

The preceding code creates a DbSet property for each entity set which typically corresponds to database table.

### Register the Context

We register our context in program.cs in using [dependency injection](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-7.0).Services are registered using dependency injection in our startup file which is _program.cs_ in _dotnetcore 6_ and prior to that that is _startup.cs_.

```
string connectionString = builder.Configuration.GetConnectionString("Default")!;

builder.Services.AddTransient<MySqlConnection>(_ =>
    new MySqlConnection(connectionString));

builder.Services.AddDbContext<ProductContext>(options => options.UseMySQL(connectionString));
```

## Migrations

Migrations helps keep the database in sync with the data models in production.

When you develop a new application, your data model changes frequently, and each time the model changes, it gets out of sync with the database. You started these tutorials by configuring the Entity Framework to create the database if it doesn't exist. Then each time you change the data model -- add, remove, or change entity classes or change your DbContext class -- you can delete the database and EF creates a new one that matches the model, and seeds it with test data.

When the application is running in production it's usually storing data that you want to keep, and you don't want to lose everything each time you make a change such as adding a new column. The EF Core Migrations feature solves this problem by enabling EF to update the database schema instead of creating a new database.

**We are going to use EF Core tools to create migrations and update our databse**

We can install globally or locally

```
dotnet tool install --global dotnet-ef
```

## Create Your Initial Migration

```
dotnet ef migrations add InitialCreate
```

You can learn further more from [here](https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/?view=aspnetcore-7.0)
