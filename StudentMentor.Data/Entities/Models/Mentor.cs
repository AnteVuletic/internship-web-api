using System.Collections.Generic;

namespace StudentMentor.Data.Entities.Models
{
    public class Mentor : User
    {
        public ICollection<Student> Students { get; set; }
    }
}
