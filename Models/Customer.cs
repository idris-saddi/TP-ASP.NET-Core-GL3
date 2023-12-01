using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace TP1.Models;

public class Customer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    [Column("membershiptype")]
    public Guid? MembershipTypeId { get; set; }

    public virtual MembershipType? MembershipType { get; set; }

    public virtual ICollection<Movie>? Movies { get; set; }
}
