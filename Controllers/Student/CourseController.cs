using Microsoft.AspNetCore.Mvc;
using MvcMovie.DAL;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Controllers;

public class CourseController : Controller
{
    private readonly ILogger<CourseController> _logger;
    private readonly SchoolContext _context;

#nullable disable
    public CourseController(SchoolContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Courses.ToListAsync());
    }

}

