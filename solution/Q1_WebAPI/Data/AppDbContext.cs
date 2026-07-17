using Microsoft.EntityFrameworkCore;
using Q1_WebAPI.Models;

namespace Q1_WebAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Director> Directors => Set<Director>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Producer> Producers => Set<Producer>();
    public DbSet<Star> Stars => Set<Star>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<MovieGenre> MovieGenres => Set<MovieGenre>();
    public DbSet<MovieStar> MovieStars => Set<MovieStar>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.ToTable("Movie_Genre");
            entity.HasKey(e => new { e.MovieId, e.GenreId });
        });

        modelBuilder.Entity<MovieStar>(entity =>
        {
            entity.ToTable("Movie_Star");
            entity.HasKey(e => new { e.MovieId, e.StarId });
        });

        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Director)
            .WithMany(d => d.Movies)
            .HasForeignKey(m => m.DirectorId)
            .IsRequired(false);

        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Producer)
            .WithMany()
            .HasForeignKey(m => m.ProducerId)
            .IsRequired(false);

        base.OnModelCreating(modelBuilder);
    }
}
