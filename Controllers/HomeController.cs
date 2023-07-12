using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        ViewBag.AllDishes = _context.Dishes.OrderByDescending(d => d.CreatedAt).ToList();
        return View();
    }

    [HttpGet("dishes/new")]
    public IActionResult DishForm()
    {
        return View();
    }

    [HttpPost("dishes/create")]
    public IActionResult NewDish(Dish newDish)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newDish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            return View("DishForm");
        }
    }

    [HttpGet("dishes/{id}")]
    public IActionResult DishView(int id)
    {
        Dish? OneDish = _context.Dishes.FirstOrDefault(d => d.DishId == id);
        return View(OneDish);
    }

    [HttpGet("dishes/{id}/edit")]
    public IActionResult DishEdit(int id)
    {
        Dish? OneDish = _context.Dishes.FirstOrDefault(d => d.DishId == id);
        return View(OneDish);
    }

    [HttpPost("dishes/{DishId}/update")]
    public IActionResult DishUpdate(Dish UpdatedDish, int DishId)
    {
        Dish? DishToUpdate = _context.Dishes.FirstOrDefault(d => d.DishId == DishId);
        if (DishToUpdate != null)
        {

            if (ModelState.IsValid)
            {
                DishToUpdate.Chef = UpdatedDish.Chef;
                DishToUpdate.Name = UpdatedDish.Name;
                DishToUpdate.Calories = UpdatedDish.Calories;
                DishToUpdate.Tastiness = UpdatedDish.Tastiness;
                DishToUpdate.Description = UpdatedDish.Description;
                DishToUpdate.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("DishEdit", DishToUpdate);
            }
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost("dishes/{DishId}/destroy")]
    public IActionResult DeleteDish(int DishId)
    {
        Dish? DeleteDish = _context.Dishes.SingleOrDefault(d => d.DishId == DishId);
        _context.Dishes.Remove(DeleteDish);
        _context.SaveChanges();
        return RedirectToAction("Index");
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
}
