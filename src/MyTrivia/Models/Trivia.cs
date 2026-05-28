using System.ComponentModel.DataAnnotations;

namespace MyTrivia.Models;

public class Trivia
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string TriviaTitle { get; set; } = string.Empty;

    [Required]
    public string TriviaBody { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Source { get; set; }

    public int? VideoId { get; set; }

    [StringLength(500)]
    public string? QuizQuestion { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ナビゲーションプロパティ
    public Video? Video { get; set; }
    public ICollection<TriviaTag> TriviaTags { get; set; } = new List<TriviaTag>();
}
