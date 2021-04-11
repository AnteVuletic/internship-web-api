namespace StudentMentor.Data.Entities.Models
{
    public class Student : User
    {
        public int? MentorId { get; set; }
        public Mentor Mentor { get; set; }
    }
}
