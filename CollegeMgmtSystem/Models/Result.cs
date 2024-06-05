namespace CollegeMgmtSystem.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int Sessional1 { get; set; }
        public int Sessional2 { get; set; }
        public int Sessional3 { get; set; }
        public int External {  get; set; }
        public int Semester { get; set; } 

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; } 

        public int StudentId { get; set; }
        public Student? Student { get; set; }
    }
}
