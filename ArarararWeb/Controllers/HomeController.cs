using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ArarararWeb.Models;
using ArarararWeb.DataModels;
using Microsoft.EntityFrameworkCore;

namespace ArarararWeb.Controllers;

public class HomeController : Controller
{
    private readonly MyDBContext _dbContext;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, MyDBContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
  
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult GetImage(int id)
    {
        var fileItem = _dbContext.Files.FirstOrDefault(f => f.Id == id);

        if (fileItem != null)
        {
            return File(fileItem.Content, "image/jpeg"); // Или "image/png" в зависимости от формата файла
        }

        return NotFound();
    }
}

