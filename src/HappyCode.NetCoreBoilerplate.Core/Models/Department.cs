using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyCode.NetCoreBoilerplate.Core.Models;

[Table("departments", Schema = "employees")]
public class Department
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(40)]
    public string DeptName { get; set; }

    public Guid MangerId { get; set; }

    [ForeignKey("MangerId")]
    [InverseProperty("LeadingDepartments")]
    public virtual Employee Manger { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
}
