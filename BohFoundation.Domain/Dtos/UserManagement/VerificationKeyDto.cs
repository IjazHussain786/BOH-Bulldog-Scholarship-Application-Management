namespace BohFoundation.Domain.Dtos.UserManagement
{
    public class VerificationKeyDto
    {
        public string VerificationKey { get; set; }
        public bool Cancel { get; set; }
        public string Password { get; set; }
    }
}
