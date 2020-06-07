using System.ComponentModel.DataAnnotations;

namespace HappyCode.NetCoreBoilerplate.Core.Dtos
{
    public class EmployeePutDto
    {
        [Required]
        public string LastName { get; set; }
    }
}
