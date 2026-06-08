using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTrivia.Data;
using MyTrivia.Models;

namespace MyTrivia.Controllers;

public class TagController : Controller
{
    private readonly AppDbContext _context;

    public TagController(AppDbContext context)
    {
        _context = context;
    }

    // 一覧
    public async Task<IActionResult> Index()
    {
        var tags = await _context.Tags
            .OrderBy(t => t.TagName)
            .ToListAsync();

        ViewBag.TriviaCounts = tags.ToDictionary(
            t => t.Id,
            t => _context.TriviaTags.Count(tt => tt.TagId == t.Id)
        );

        return View(tags);
    }

    // 登録画面
    public IActionResult Create()
    {
        return View();
    }

    // 登録処理
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tag tag)
    {
        if (ModelState.IsValid)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tag);
    }

    // 編集画面
    public async Task<IActionResult> Edit(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null) return NotFound();

        ViewBag.TriviaCount = await _context.TriviaTags.CountAsync(tt => tt.TagId == id);
        return View(tag);
    }

    // 編集処理
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tag tag)
    {
        if (id != tag.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tag);
    }

    // 削除処理
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var tag = await _context.Tags
            .Include(t => t.TriviaTags)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tag == null) return NotFound();

        _context.TriviaTags.RemoveRange(tag.TriviaTags);
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}