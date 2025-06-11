using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyCode.NetCoreBoilerplate.Core.Models;

[Table("employees", Schema = "employees")]
public class Employee
{
    [Key]
    public Guid Id { get; set; }

    public DateTime BirthDate { get; set; } 

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(1)]
    public string Gender { get; set; } = "M"; 

    public Guid? DeptId { get; set; }

    [ForeignKey("DeptId")]
    [InverseProperty("Employees")]
    public virtual Department? Department { get; set; }

    [InverseProperty("Manger")]
    public virtual ICollection<Department> LeadingDepartments { get; set; } = new HashSet<Department>();
}
