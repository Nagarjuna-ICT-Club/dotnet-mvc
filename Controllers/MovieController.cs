using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using MySqlConnector;

namespace MvcMovie.Controllers;

public class MovieController : Controller
{
    private readonly ILogger<MovieController> _logger;

    public MovieController(ILogger<MovieController> logger)
    {
        _logger = logger;
    }

    private List<MovieViewModel> movies = new List<MovieViewModel>();

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

    public IActionResult NewMovie()
    {
        return View();
    }

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

    public IActionResult Delete(MovieViewModel movie)
    {
        using var connection = new MySqlConnection("Server=localhost;User ID=root;password=7227;Database=movies");
        connection.Open();

        string query = $"DELETE from movies where id = {movie.id} ";
        using var command = new MySqlCommand(query, connection);
        int reader = command.ExecuteNonQuery();
        return Redirect("/Movie");
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
