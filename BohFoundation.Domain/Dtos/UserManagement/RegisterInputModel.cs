namespace BohFoundation.Domain.Dtos.UserManagement
{
    public class RegisterInputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int GraduatingYear { get; set; }
        public string Password { get; set; }
    }
}
