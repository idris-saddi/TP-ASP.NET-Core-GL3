using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP1.Models;

public class Movie
{
	public Guid Id { get; set; }

	public string Name { get; set; } = "";

	public DateTime Released { get; set; } = DateTime.Now;

	public DateTime Added { get; set; } = DateTime.Now;

	public string? Poster { get; set; }

	[NotMapped]
	public IFormFile? File { get; set; }

	[Column("genre_id")]
	public Guid? GenreId { get; set; }

	public virtual Genre? Genre { get; set; }

	public virtual ICollection<Customer>? Customers { get; set; }
}
