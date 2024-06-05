namespace CollegeMgmtSystem.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DeptName { get; set; }
        public int DeptDuration { get; set; }
        public int DeptFees { get; set; }

        public ICollection<Course>? Courses { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<Teacher>? Teachers { get; set; }
    }
}
