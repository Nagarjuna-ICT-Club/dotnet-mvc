## Introduction to Dotnet

On Dotnet MVC, CRUD operations are carried out.One of the Entity Framework examples was created using [Microsoft's](https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/?view=aspnetcore-7.0) official documentation.

## We'll go through

1. What is MVC architecture ?
2. How MVC Works ?
3. Folder Structure for MVC Pattern
4. Basic CRUD Example without Entity Framework
5. What is Entity Framework ?
6. Create Data Model
7. Create the database Context
8. Register the database Context
9. Create Controller and Views
10. Asynchronous Code

## Table of Contents

- [Introduction to Dotnet](#introduction-to-dotnet)
  - [We'll go through](#well-go-through)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
  - [MVC Architecture](#mvc-architecture)
  - [How MVC Works](#how-mvc-works)

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

For this purpose let's create a generic list of our movie model in our controller. For full code you can see Controller > MovieController.cs

```
    private List<MovieViewModel> movies = new List<MovieViewModel>();
```

Let's define our controller for listing our movies list in Homepage

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

Let's define our View for Movies Home in Views > Movie > Index.cshtml

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
          <div style="diplay:flex;gap:5px;">
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
          </div>
        </tr>
      }
    </tbody>
  </table>
</div>

```
 
