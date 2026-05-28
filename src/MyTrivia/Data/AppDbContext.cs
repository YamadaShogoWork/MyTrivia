using Microsoft.EntityFrameworkCore;
using MyTrivia.Models;

namespace MyTrivia.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Trivia> Trivias { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TriviaTag> TriviaTags { get; set; }
    public DbSet<Video> Videos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TriviaTagの複合主キーを設定
        modelBuilder.Entity<TriviaTag>()
            .HasKey(tt => new { tt.TriviaId, tt.TagId });

        // TriviaTag → Trivia のリレーション
        modelBuilder.Entity<TriviaTag>()
            .HasOne(tt => tt.Trivia)
            .WithMany(t => t.TriviaTags)
            .HasForeignKey(tt => tt.TriviaId);

        // TriviaTag → Tag のリレーション
        modelBuilder.Entity<TriviaTag>()
            .HasOne(tt => tt.Tag)
            .WithMany(t => t.TriviaTags)
            .HasForeignKey(tt => tt.TagId)
            .OnDelete(DeleteBehavior.Cascade); // タグ削除時にTriviaTagも削除

        // Trivia → Video のリレーション（動画削除時はVideoIdをNULLに）
        modelBuilder.Entity<Trivia>()
            .HasOne(t => t.Video)
            .WithMany(v => v.Trivias)
            .HasForeignKey(t => t.VideoId)
            .OnDelete(DeleteBehavior.SetNull);

        // TagNameにユニーク制約
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.TagName)
            .IsUnique();

        // VideoUrlにユニーク制約
        modelBuilder.Entity<Video>()
            .HasIndex(v => v.VideoUrl)
            .IsUnique();
    }
}
