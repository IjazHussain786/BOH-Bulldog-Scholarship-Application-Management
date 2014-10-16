namespace BohFoundation.Domain.Dtos.Admin
{
    public class ConfirmApplicationEvaluatorDto
    {
        public string EmailAddress { get; set; }
        public bool Confirm { get; set; }
        public bool CreateAdmin { get; set; }
    }
}
