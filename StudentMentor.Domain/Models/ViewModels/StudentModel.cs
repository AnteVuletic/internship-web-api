namespace StudentMentor.Domain.Models.ViewModels
{
    public class StudentModel : UserModel
    {
        public UserModel Mentor { get; set; }
    }
}
