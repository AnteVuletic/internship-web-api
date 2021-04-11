using System.Collections.Generic;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;

namespace StudentMentor.Domain.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        ResponseResult<Student> RegisterStudent(RegistrationModel model);
        ICollection<StudentModel> GetStudents();
        ResponseResult<StudentModel> GetStudent(int studentId);
        ResponseResult DeleteStudent(int studentId);
        ResponseResult SetMentor(int userId, int mentorId);
        ResponseResult RemoveMentor(int userId);
    }
}
