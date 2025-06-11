using System.ComponentModel.DataAnnotations;

namespace HappyCode.NetCoreBoilerplate.Core.Dtos
{
    public class EmployeePutDto
    {

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(1)]
        [RegularExpression("^[MF]$")]
        public string? Gender { get; set; }

        public Guid? DeptId { get; set; } 


    }
}
