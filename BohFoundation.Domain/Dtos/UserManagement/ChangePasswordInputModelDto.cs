namespace BohFoundation.Domain.Dtos.UserManagement
{
    public class ChangePasswordInputModelDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}