using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Helpers;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;
using StudentMentor.Domain.Repositories.Interfaces;

namespace StudentMentor.Domain.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentMentorDbContext _dbContext;

        public StudentRepository(StudentMentorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResponseResult<Student> RegisterStudent(RegistrationModel model)
        {
            var password = EncryptionHelper.Hash(model.Password);
            var student = new Student
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = password
            };

            _dbContext.Add(student);
            _dbContext.SaveChanges();
            return new ResponseResult<Student>(student);
        }

        public ICollection<StudentModel> GetStudents()
        {
            var students = _dbContext.Students
                .AsNoTracking()
                .Select(s => new StudentModel
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Mentor = s.MentorId.HasValue
                        ? new UserModel
                        {
                            Id = s.Mentor.Id,
                            Email = s.Mentor.Email,
                            FirstName = s.Mentor.FirstName,
                            LastName = s.Mentor.LastName
                        }
                        : null
                })
                .ToList();

            return students;
        }

        public ResponseResult<StudentModel> GetStudent(int studentId)
        {
            var student = _dbContext.Students
                .Where(s => s.Id == studentId)
                .Select(s => new StudentModel
                {
                    Id = s.Id,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Mentor = s.MentorId.HasValue
                        ? new UserModel
                        {
                            Id = s.Mentor.Id,
                            Email = s.Mentor.Email,
                            FirstName = s.Mentor.FirstName,
                            LastName = s.Mentor.LastName
                        }
                        : null
                })
                .SingleOrDefault();

            return student is null
                ? ResponseResult<StudentModel>.Error("Not found")
                : new ResponseResult<StudentModel>(student);
        }

        public ResponseResult DeleteStudent(int studentId)
        {
            var student = _dbContext.Students.Find(studentId);

            if (student is null)
                return ResponseResult.Error("Not found");

            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();

            return ResponseResult.Ok;
        }

        public ResponseResult SetMentor(int userId, int mentorId)
        {
            var student = _dbContext.Students.Find(userId);
            student.MentorId = mentorId;
            _dbContext.SaveChanges();

            return ResponseResult.Ok;
        }

        public ResponseResult RemoveMentor(int userId)
        {
            var student = _dbContext.Students.Find(userId);
            student.MentorId = null;
            _dbContext.SaveChanges();

            return ResponseResult.Ok;
        }
    }
}
