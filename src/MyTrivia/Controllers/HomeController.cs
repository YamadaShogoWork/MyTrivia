using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTrivia.Data;

namespace MyTrivia.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TriviaCount = await _context.Trivias.CountAsync();
        ViewBag.VideoCount = await _context.Videos.CountAsync();
        ViewBag.TagCount = await _context.Tags.CountAsync();
        ViewBag.RecentTrivias = await _context.Trivias
            .Include(t => t.TriviaTags)
                .ThenInclude(tt => tt.Tag)
            .OrderByDescending(t => t.CreatedAt)
            .Take(4)
            .ToListAsync();

        return View();
    }
}