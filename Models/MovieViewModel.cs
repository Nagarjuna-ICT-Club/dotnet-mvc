using System.ComponentModel.DataAnnotations;
namespace MvcMovie.Models;

public class MovieViewModel {
    
    public int id { get ; set;}

    public string? genre {get; set; }

    public string? Title { get; set; }

    [DataType(DataType.Date)]
    // public DateTime releaseDate { get; set;}
    public DateOnly releaseDate { get; set; }
    
}