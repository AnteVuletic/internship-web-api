using System;

namespace StudentMentor.Data.Entities.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? UserFromId { get; set; }
        public User UserFrom { get; set; }

        public int? UserToId { get; set; }
        public User UserTo { get; set; }

        public DateTime MessageCreatedAt { get; set; }
    }
}
