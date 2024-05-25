using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyCode.NetCoreBoilerplate.Core.Models
{
    [Table("departments", Schema = "employees")]
    public class Department
    {
        [Key]
        [Column("dept_no", TypeName = "char(4)")]
        public string DeptNo { get; set; }

        [Required]
        [Column("dept_name")]
        [StringLength(40)]
        public string DeptName { get; set; }

        [Column("manger_no", TypeName = "int(11)")]
        public int MangerNo { get; set; }


        [ForeignKey("MangerNo")]
        [InverseProperty("LeadingDepartments")]
        public virtual Employee Manger { get; set; }

        [InverseProperty("Department")]
        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
