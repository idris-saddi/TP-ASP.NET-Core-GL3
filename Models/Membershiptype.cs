using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP1.Models;

[Table("membershiptype")]
public class MembershipType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public double SignUpFee { get; set; }

    public int DurationInMonths { get; set; }

    public double DiscountRate { get; set; }

    public virtual ICollection<Customer>? Customers { get; set; }
}