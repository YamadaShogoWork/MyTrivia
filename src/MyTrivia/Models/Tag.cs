using System.ComponentModel.DataAnnotations;

namespace MyTrivia.Models;

public class Tag
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string TagName { get; set; } = string.Empty;

    // ナビゲーションプロパティ
    public ICollection<TriviaTag> TriviaTags { get; set; } = new List<TriviaTag>();
}
