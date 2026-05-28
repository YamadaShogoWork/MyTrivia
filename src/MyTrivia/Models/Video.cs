using System.ComponentModel.DataAnnotations;

namespace MyTrivia.Models;

public class Video
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string VideoTitle { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string VideoUrl { get; set; } = string.Empty;

    public DateOnly? PublishedAt { get; set; }

    // ナビゲーションプロパティ
    public ICollection<Trivia> Trivias { get; set; } = new List<Trivia>();
}
