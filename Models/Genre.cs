using System.ComponentModel.DataAnnotations.Schema;

namespace TP1.Models;

public class Genre
{
    public Guid Id { get; set; }

    [Column("GenreName")]
    public string Name { get; set; } = "";

    public virtual ICollection<Movie>? Movies { get; set; }
}
