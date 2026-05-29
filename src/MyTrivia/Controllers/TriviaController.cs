using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTrivia.Data;
using MyTrivia.Models;

namespace MyTrivia.Controllers;

public class TriviaController : Controller
{
    private readonly AppDbContext _context;

    public TriviaController(AppDbContext context)
    {
        _context = context;
    }

    // 一覧
    public async Task<IActionResult> Index(int? tagId, string? keyword)
    {
        var query = _context.Trivias
            .Include(t => t.Video)
            .Include(t => t.TriviaTags)
                .ThenInclude(tt => tt.Tag)
            .AsQueryable();

        // タグ絞込
        if (tagId.HasValue)
        {
            query = query.Where(t => t.TriviaTags.Any(tt => tt.TagId == tagId));
        }

        // キーワード検索
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(t =>
                t.TriviaTitle.Contains(keyword) ||
                t.TriviaBody.Contains(keyword));
        }

        var trivias = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        var tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();

        ViewBag.Tags = tags;
        ViewBag.SelectedTagId = tagId;
        ViewBag.Keyword = keyword;

        return View(trivias);
    }

    // 詳細
    public async Task<IActionResult> Detail(int id)
    {
        var trivia = await _context.Trivias
            .Include(t => t.Video)
            .Include(t => t.TriviaTags)
                .ThenInclude(tt => tt.Tag)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (trivia == null) return NotFound();

        return View(trivia);
    }

    // 登録画面
    public async Task<IActionResult> Create()
    {
        ViewBag.Tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
        ViewBag.Videos = await _context.Videos.OrderBy(v => v.VideoTitle).ToListAsync();
        return View();
    }

    // 登録処理
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Trivia trivia, int[] selectedTagIds)
    {
        if (ModelState.IsValid)
        {
            trivia.CreatedAt = DateTime.Now;
            _context.Trivias.Add(trivia);
            await _context.SaveChangesAsync();

            // タグの紐づけ
            foreach (var tagId in selectedTagIds)
            {
                _context.TriviaTags.Add(new TriviaTag
                {
                    TriviaId = trivia.Id,
                    TagId = tagId
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
        ViewBag.Videos = await _context.Videos.OrderBy(v => v.VideoTitle).ToListAsync();
        return View(trivia);
    }

    // 編集画面
    public async Task<IActionResult> Edit(int id)
    {
        var trivia = await _context.Trivias
            .Include(t => t.TriviaTags)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (trivia == null) return NotFound();

        ViewBag.Tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
        ViewBag.Videos = await _context.Videos.OrderBy(v => v.VideoTitle).ToListAsync();
        ViewBag.SelectedTagIds = trivia.TriviaTags.Select(tt => tt.TagId).ToList();
        return View(trivia);
    }

    // 編集処理
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Trivia trivia, int[] selectedTagIds)
    {
        if (id != trivia.Id) return NotFound();

        if (ModelState.IsValid)
        {
            // 既存タグを削除して付け直す
            var existingTags = _context.TriviaTags.Where(tt => tt.TriviaId == id);
            _context.TriviaTags.RemoveRange(existingTags);

            foreach (var tagId in selectedTagIds)
            {
                _context.TriviaTags.Add(new TriviaTag
                {
                    TriviaId = id,
                    TagId = tagId
                });
            }

            _context.Update(trivia);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Detail), new { id });
        }

        ViewBag.Tags = await _context.Tags.OrderBy(t => t.TagName).ToListAsync();
        ViewBag.Videos = await _context.Videos.OrderBy(v => v.VideoTitle).ToListAsync();
        ViewBag.SelectedTagIds = selectedTagIds.ToList();
        return View(trivia);
    }

    // 削除処理
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var trivia = await _context.Trivias
            .Include(t => t.TriviaTags)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (trivia == null) return NotFound();

        _context.TriviaTags.RemoveRange(trivia.TriviaTags);
        _context.Trivias.Remove(trivia);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
