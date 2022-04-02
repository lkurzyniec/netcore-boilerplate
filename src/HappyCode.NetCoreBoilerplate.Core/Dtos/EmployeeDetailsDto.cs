namespace HappyCode.NetCoreBoilerplate.Core.Dtos
{
    public class EmployeeDetailsDto
    {
        public int Id { get; set; }
        public DateTime BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DepartmentDto Department { get; set; }
    }
}
