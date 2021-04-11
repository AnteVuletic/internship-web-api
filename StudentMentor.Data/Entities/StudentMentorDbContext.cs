using Microsoft.EntityFrameworkCore;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Data.Enums;

namespace StudentMentor.Data.Entities
{
    public class StudentMentorDbContext : DbContext
    {
        public StudentMentorDbContext(DbContextOptions<StudentMentorDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator(u => u.UserRole)
                .HasValue<Student>(UserRole.Student)
                .HasValue<Mentor>(UserRole.Mentor)
                .HasValue<Admin>(UserRole.Admin);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Mentor)
                .WithMany(m => m.Students)
                .HasForeignKey(s => s.MentorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.UserFrom)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.UserFromId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.UserTo)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.UserToId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
