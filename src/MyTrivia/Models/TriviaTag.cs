namespace MyTrivia.Models;

public class TriviaTag
{
    public int TriviaId { get; set; }
    public int TagId { get; set; }

    // ナビゲーションプロパティ
    public Trivia Trivia { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
