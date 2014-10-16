namespace BohFoundation.Domain.Dtos.UserManagement
{
    public class ChangePasswordFromResetKeyDto
    {
        public string NewPassword { get; set; }
        public string Key { get; set; }
    }
}
