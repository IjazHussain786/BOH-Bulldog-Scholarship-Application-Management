using System;
using System.Text;
using BohFoundation.Domain.Dtos.Email;
using BohFoundation.MembershipProvider.UserManagement.Admin.Implementation;
using BohFoundation.TestHelpers;
using BohFoundation.Utilities.Context.Interfaces;
using BohFoundation.Utilities.Context.Interfaces.Context;
using BohFoundation.Utilities.Email.Interfaces.Email;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.MembershipProvider.Tests.UnitTests.UserManagement.Admin
{
    [TestClass]
    public class InviteApplicationEvaluatorTests
    {
        private InviteApplicationEvaluator _inviteApplicationEvaluator;
        private IClaimsInformationGetters _claimsInformationGetters;
        private IEmailService _emailService;
        private IHttpContextInformationGetters _httpContextGetters;

        private string _senderFullName = Sender + TestHelpersCommonFields.FullName;
        private string _urlToSend = BaseUrl + "/D49E4466995642C1887C591BB87FDE74/" + RecipientsEmail + "/" + RecipientFirstName + "/" + RecipientLastName;

        private const string Sender = "Sender";
        private const string Recipient = "Recipient";
        private const string SendersEmail = Sender + TestHelpersCommonFields.Email;
        private const string RecipientsEmail = Recipient + TestHelpersCommonFields.Email;
        private const string RecipientFirstName = Recipient + TestHelpersCommonFields.FirstName;
        private const string RecipientLastName = Recipient + TestHelpersCommonFields.LastName;
        private const string BaseUrl = "url";

        private const string Subject = "Invitation to Evaluate Bulldog Scholarship Applications";
        private const string Explaination =
            "Please click the link below and enter a new password. After you have done that call me to finalize confirm your account.";
        
        [TestInitialize]
        public void Initialize()
        {
            _emailService = A.Fake<IEmailService>();
            _claimsInformationGetters = A.Fake<IClaimsInformationGetters>();
            _httpContextGetters = A.Fake<IHttpContextInformationGetters>();
            _inviteApplicationEvaluator = new InviteApplicationEvaluator(_emailService, _claimsInformationGetters, _httpContextGetters);

            A.CallTo(() => _claimsInformationGetters.IsAdmin()).Returns(true);
        }

        [TestMethod]
        public void InviteApplicationEvaluator_Should_Call_ClaimsInformationGetters_IsAdmin()
        {
            CallSendApplicationEvaluatorInvitation();
            A.CallTo(()=>_claimsInformationGetters.IsAdmin()).MustHaveHappened();
        }

        [TestMethod, ExpectedException(typeof(UnauthorizedAccessException))]
        public void InviteApplicationEvaluator_Should_Not_Call_EmailService_IfNotAdmin()
        {
            A.CallTo(() => _claimsInformationGetters.IsAdmin()).Returns(false);
            CallSendApplicationEvaluatorInvitation();
            A.CallTo(() => _emailService.SendEmailToOneUser(A<SendEmailDtoWithSubjectBodyAndSender>.Ignored)).MustNotHaveHappened();
        }

        [TestMethod]
        public void InviteApplicationEvaluator_Should_Call_ClaimsInformationGetters_GetFullName()
        {
            CallSendApplicationEvaluatorInvitation();
            A.CallTo(() => _claimsInformationGetters.GetUsersFullName()).MustHaveHappened(Repeated.Exactly.Times(2));
        }

        [TestMethod]
        public void InviteApplicationEvaluator_Should_Call_ClaimsInformationGetters_GetEmailAddress()
        {
            CallSendApplicationEvaluatorInvitation();
            A.CallTo(() => _claimsInformationGetters.GetUsersEmail()).MustHaveHappened();
        }

        [TestMethod]
        public void InviteApplicationEvaluator_Should_Call_HttpContextGetters_GetBaseUrl()
        {
            CallSendApplicationEvaluatorInvitation();
            A.CallTo(() => _httpContextGetters.GetRequestHttpBaseUrl()).MustHaveHappened();
        }

        [TestMethod]
        public void InviteApplicationEvaluator_Should_Call_EmailService_WithBuiltEmail()
        {
            A.CallTo(() => _claimsInformationGetters.GetUsersEmail()).Returns(SendersEmail);
            A.CallTo(() => _claimsInformationGetters.GetUsersFullName()).Returns(_senderFullName);
            A.CallTo(() => _httpContextGetters.GetRequestHttpBaseUrl()).Returns(BaseUrl);

            CallSendApplicationEvaluatorInvitation();
            A.CallTo(() => _emailService.SendEmailToOneUser(A<SendEmailDtoWithSubjectBodyAndSender>.That.Matches(x => 
                x.Body == BodyOfMessage() &&
                x.SendersEmail == SendersEmail && 
                x.SendersFullName == _senderFullName && 
                x.RecipientEmailAddress == RecipientsEmail &&
                x.RecipientFirstName == RecipientFirstName &&
                x.Subject == Subject))).MustHaveHappened();
        }

        private string BodyOfMessage()
        {
            var salutation = string.Format("{0} {1},", RecipientFirstName, RecipientLastName);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(salutation);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(Explaination);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(_urlToSend);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(_senderFullName);
            
            return stringBuilder.ToString();
        }

        private void CallSendApplicationEvaluatorInvitation()
        {
            _inviteApplicationEvaluator.SendApplicationEvaluatorInvitation(new SendEmailContactDto{RecipientEmailAddress = RecipientsEmail, RecipientFirstName = RecipientFirstName, RecipientLastName = RecipientLastName});
        }
    }
}
