using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HappyCode.NetCoreBoilerplate.Core.Dtos
{
    public class DepartmentDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EmployeeDto? Manager { get; set; }
        public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }

    public class DepartmentPostDto
    {
        [Required]
        [StringLength(40)]
        public string Name { get; set; } = "";
        
        [Required]
        public Guid ManagerId { get; set; }
    }

    public class DepartmentPutDto
    {
        [StringLength(40)]
        public string Name { get; set; }
        
        public Guid? ManagerId { get; set; }
    }
}
