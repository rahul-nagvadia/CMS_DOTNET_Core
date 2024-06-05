using Microsoft.AspNetCore.Identity;

namespace CollegeMgmtSystem.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string StuName { get; set; }
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? MobileNumber { get; set; }
        public int? Semester { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}
