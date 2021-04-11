using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Web.Infrastructure;

namespace StudentMentor.Web.Controllers
{
    [Authorize(Policy = Policies.Admin)]
    public class AdminController : ApiController
    {
        private readonly IStudentRepository _studentRepository;

        public AdminController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet(nameof(GetStudents))]
        public ActionResult<ICollection<StudentModel>> GetStudents()
        {
            var students = _studentRepository.GetStudents();
            return Ok(students);
        }

        [HttpDelete(nameof(DeleteStudent))]
        public ActionResult DeleteStudent([FromQuery] int studentId)
        {
            var response = _studentRepository.DeleteStudent(studentId);
            if (response.IsError)
                return BadRequest(response.Message);

            return Ok();
        }
    }
}
