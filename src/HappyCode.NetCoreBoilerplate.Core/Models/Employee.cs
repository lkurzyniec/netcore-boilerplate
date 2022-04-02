using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyCode.NetCoreBoilerplate.Core.Models
{
    [Table("employees", Schema = "employees")]
    public class Employee
    {
        [Key]
        [Column("emp_no", TypeName = "int(11)")]
        public int EmpNo { get; set; }

        [Column("birth_date", TypeName = "date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Column("first_name")]
        [StringLength(14)]
        public string FirstName { get; set; }

        [Required]
        [Column("last_name")]
        [StringLength(16)]
        public string LastName { get; set; }

        [Required]
        [Column("gender", TypeName = "enum('M','F')")]
        public string Gender { get; set; }

        [Column("dept_no", TypeName = "char(4)")]
        public string DeptNo { get; set; }


        [ForeignKey("DeptNo")]
        [InverseProperty("Employees")]
        public virtual Department Department { get; set; }

        [InverseProperty("Manger")]
        public virtual ICollection<Department> LeadingDepartments { get; set; } = new HashSet<Department>();
    }
}
