using System.Net.Mail;
using BohFoundation.Domain.Dtos.Email;
using BohFoundation.TestHelpers;
using BohFoundation.Utilities.Email.Implementation;
using BohFoundation.Utilities.Email.Interfaces.Email.Helpers;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BohFoundation.Utilities.Tests.Utilities.Email
{
    [TestClass]
    public class Office365EmailServiceTests
    {
        private ISendEmailFromOffice365 _sendGridClient;
        private Office365EmailService _sendGridService;
        private const string Body = "body";
        private const string Subject = "subject";

        [TestInitialize]
        public void Initialize()
        {
            TestHelpersCommonFields.InitializeFields();

            _sendGridClient = A.Fake<ISendEmailFromOffice365>();

            _sendGridService = new Office365EmailService(_sendGridClient);
        }

        //[TestMethod]
        //public void Office365EmailService_SendEmailToOneUser_WithSender_Should_CreateTheMessage_AndSendIt()
        //{
        //    var sendEmailDto = new SendEmailDtoWithSubjectBodyAndSender
        //    {
        //        Body = Body,
        //        RecipientEmailAddress = TestHelpersCommonFields.Email,
        //        Subject = Subject,
        //        RecipientLastName = TestHelpersCommonFields.LastName,
        //        RecipientFirstName = TestHelpersCommonFields.FirstName,
        //        SendersEmail = TestHelpersCommonFields.Email + "m",
        //        SendersFullName = TestHelpersCommonFields.FullName + 2
        //    };

        //    _sendGridService.SendEmailToOneUser(sendEmailDto);

        //    A.CallTo(() => _sendGridClient.SendMessage(A<MailMessage>.That.Matches(x => 
        //        x.From.Address == TestHelpersCommonFields.Email + "m" && 
        //        x.From.DisplayName == TestHelpersCommonFields.FullName + 2 &&
        //        x.Subject == Subject &&
        //        x.Body == Body &&
        //        x.To[0].Address == TestHelpersCommonFields.Email &&
        //        x.To[0].DisplayName == TestHelpersCommonFields.FullName
        //        ))).MustHaveHappened();
        //}

        [TestMethod]
        public void Office365EmailService_SendEmailToOneUser_WithOutSender_Should_CreateTheMessage_AndSendIt()
        {
            var sendEmailDto = new SendEmailWithSubjectAndBodyDto
            {
                Body = Body,
                RecipientEmailAddress = TestHelpersCommonFields.Email,
                Subject = Subject,
                RecipientLastName = TestHelpersCommonFields.LastName,
                RecipientFirstName = TestHelpersCommonFields.FirstName
            };

            _sendGridService.SendEmailToOneUser(sendEmailDto);

            A.CallTo(() => _sendGridClient.SendMessage(A<MailMessage>.That.Matches(x =>
                x.From.Address == "Robot@BOHFoundation.org" &&
                x.From.DisplayName == "BOHFoundation Mail Robot" &&
                x.Subject == Subject &&
                x.Body == Body &&
                x.To[0].Address == TestHelpersCommonFields.Email &&
                x.To[0].DisplayName == TestHelpersCommonFields.FullName
                ))).MustHaveHappened();
        }
    }
}
