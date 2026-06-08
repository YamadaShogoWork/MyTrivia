using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTrivia.Data;
using MyTrivia.Models;

namespace MyTrivia.Controllers;

public class VideoController : Controller
{
    private readonly AppDbContext _context;

    public VideoController(AppDbContext context)
    {
        _context = context;
    }

    // 一覧
    public async Task<IActionResult> Index()
    {
        var videos = await _context.Videos
            .OrderBy(v => v.VideoTitle)
            .ToListAsync();

        // 各動画に紐づく雑学数をViewBagで渡す
        ViewBag.TriviaCounts = videos.ToDictionary(
            v => v.Id,
            v => _context.Trivias.Count(t => t.VideoId == v.Id)
        );

        return View(videos);
    }

    // 登録画面
    public IActionResult Create()
    {
        return View();
    }

    // 登録処理
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Video video)
    {
        if (ModelState.IsValid)
        {
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(video);
    }

    // 編集画面
    public async Task<IActionResult> Edit(int id)
    {
        var video = await _context.Videos.FindAsync(id);
        if (video == null) return NotFound();
        return View(video);
    }

    // 編集処理
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Video video)
    {
        if (id != video.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(video);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(video);
    }

    // 削除処理
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var video = await _context.Videos.FindAsync(id);
        if (video == null) return NotFound();

        // 紐づく雑学のVideoIdをNULLに
        var trivias = await _context.Trivias.Where(t => t.VideoId == id).ToListAsync();
        foreach (var trivia in trivias)
        {
            trivia.VideoId = null;
        }

        _context.Videos.Remove(video);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
