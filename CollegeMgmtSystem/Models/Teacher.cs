using Microsoft.AspNetCore.Identity;

namespace CollegeMgmtSystem.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string TeacherName { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        public IdentityUser? User { get; set; }
        public string? UserId { get; set; }
    }
}
