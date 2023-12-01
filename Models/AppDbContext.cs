using Microsoft.EntityFrameworkCore;

namespace TP1.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<MembershipType> MembershipTypes { get; set; }

    public DbSet<Movie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        string genresJson = File.ReadAllText("data/Genres.json");
        List<Genre>? genres = System.Text.Json.JsonSerializer.Deserialize<List<Genre>>(genresJson);

        if (genres == null)
        {
            return;
        }

        foreach (var genre in genres)
        {
            modelBuilder
                .Entity<Genre>()
                .HasData(genre);
        }

        string membershipTypesJson = File.ReadAllText("data/MembershipTypes.json");
        List<MembershipType>? membershipTypes = System.Text.Json.JsonSerializer.Deserialize<List<MembershipType>>(membershipTypesJson);

        if (membershipTypes == null)
        {
            return;
        }

        foreach (var membershipType in membershipTypes)
        {
            modelBuilder
                .Entity<MembershipType>()
                .HasData(membershipType);
        }
    }
}
