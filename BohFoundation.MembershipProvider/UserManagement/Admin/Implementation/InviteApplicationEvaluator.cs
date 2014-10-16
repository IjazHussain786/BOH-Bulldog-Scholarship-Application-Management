using System;
using System.Text;
using BohFoundation.Domain.Dtos.Email;
using BohFoundation.MembershipProvider.UserManagement.Admin.Interfaces;
using BohFoundation.Utilities.Context.Interfaces;
using BohFoundation.Utilities.Context.Interfaces.Context;
using BohFoundation.Utilities.Email.Interfaces.Email;

namespace BohFoundation.MembershipProvider.UserManagement.Admin.Implementation
{
    public class InviteApplicationEvaluator : IInviteApplicationEvaluator
    {
        private readonly IEmailService _emailService;
        private readonly IClaimsInformationGetters _claimsInformationGetters;
        private readonly IHttpContextInformationGetters _httpContextInformation;

        private SendEmailDtoWithSubjectBodyAndSender SendEmailToDto { get; set; }
        
        public InviteApplicationEvaluator(IEmailService emailService, IClaimsInformationGetters claimsInformationGetters, IHttpContextInformationGetters httpContextInformation)
        {
            _emailService = emailService;
            _claimsInformationGetters = claimsInformationGetters;
            _httpContextInformation = httpContextInformation;

            AutoMapper.Mapper.CreateMap<SendEmailContactDto, SendEmailDtoWithSubjectBodyAndSender>();
        }

        public void SendApplicationEvaluatorInvitation(SendEmailContactDto sendEmailToDto)
        {
            if (!_claimsInformationGetters.IsAdmin()) throw new UnauthorizedAccessException();

            SendEmailToDto = AutoMapper.Mapper.Map<SendEmailDtoWithSubjectBodyAndSender>(sendEmailToDto);;

            CreateMessage();

            _emailService.SendEmailToOneUser(SendEmailToDto);
        }

        private void CreateMessage()
        {
            SendEmailToDto.Body = CreateBodyMessage();
            SendEmailToDto.SendersEmail = _claimsInformationGetters.GetUsersEmail();
            SendEmailToDto.SendersFullName = _claimsInformationGetters.GetUsersFullName();
            SendEmailToDto.Subject = SubjectOfEmail;
        }

        private const string SubjectOfEmail = "Invitation to Evaluate Bulldog Scholarship Applications";

        private string CreateBodyMessage()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(CreateSalutation());
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Explaination);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(GetUrlToSend());
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Signature());
            return stringBuilder.ToString();
        }

        private const string Explaination =
            "Please click the link below and enter a new password. After you have done that call me to finalize confirm your account.";
 
        private string Signature()
        {
            return _claimsInformationGetters.GetUsersFullName();
        }

        private string GetUrlToSend()
        {
            var baseUrl = _httpContextInformation.GetRequestHttpBaseUrl();

            return baseUrl + "/" + UrlEnding + "/" + SendEmailToDto.RecipientEmailAddress + "/" + SendEmailToDto.RecipientFirstName + "/" + SendEmailToDto.RecipientLastName;
        }

        private const string UrlEnding = "D49E4466995642C1887C591BB87FDE74";

        private string CreateSalutation()
        {
            return SendEmailToDto.RecipientFullName + ",";
        }
    }
}
