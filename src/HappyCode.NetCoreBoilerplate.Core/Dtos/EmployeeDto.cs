using System;

namespace HappyCode.NetCoreBoilerplate.Core.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public DateTime BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
}
