
namespace MvcMovie.Models
{
    public class Student
    {
        // by default EF interprets ID or classnameID as Primary Key
        public int ID { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }

#nullable disable
        public ICollection<Enrollment> Enrollments { get; set; }

    }

}