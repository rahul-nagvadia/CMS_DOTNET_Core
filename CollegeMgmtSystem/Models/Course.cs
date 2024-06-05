using System.ComponentModel.DataAnnotations;

namespace CollegeMgmtSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int Semester { get; set; }
        public int Credit { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; } = null;
    }
}
