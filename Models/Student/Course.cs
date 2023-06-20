namespace MvcMovie.Models
{
    public class Course
    {
        public int CourseID { get; set; }

        public string? Title { get; set; }

        public int Credits { get; set; }

        #nullable disable
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}