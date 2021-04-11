using System.Collections.Generic;
using StudentMentor.Data.Enums;

namespace StudentMentor.Data.Entities.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }

        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
    }
}
