using System.Collections.Generic;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.References;
using BohFoundation.Domain.Dtos.Email;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces.Helpers;
using BohFoundation.Utilities.Email.Interfaces.Email;

namespace BohFoundation.MiddleTier.ApplicantsOrchestration.Implementations
{
    public class ApplicantReferenceOrchestration : IApplicantReferenceOrchestration
    {
        private readonly IApplicantsReferencesRepository _applicantsReferencesRepository;
        private readonly IEmailService _emailService;
        private readonly ICreateEmailBodyForApplicantReferenceRequest _createEmailBody;

        public ApplicantReferenceOrchestration(IApplicantsReferencesRepository applicantsReferencesRepository, IEmailService emailService, ICreateEmailBodyForApplicantReferenceRequest createEmailBody)
        {
            _applicantsReferencesRepository = applicantsReferencesRepository;
            _emailService = emailService;
            _createEmailBody = createEmailBody;
        }

        public ICollection<ReferenceDto> GetReferences()
        {
            return _applicantsReferencesRepository.GetReferences();
        }

        public void AddReference(ApplicantReferenceInputDto referenceInputDto)
        {
            var dtoForEf = _createEmailBody.CreateBody(referenceInputDto);
            
            _applicantsReferencesRepository.AddReference(dtoForEf);
            
            var objectForEmailService = new SendEmailWithSubjectAndBodyDto
            {
                RecipientEmailAddress = dtoForEf.ReferenceEmail,
                Body = dtoForEf.MessageParagraph,
                Subject = "Reference Request For The Bulldog Scholarship"
            };

            _emailService.SendEmailToOneUser(objectForEmailService);
        }
    }
}
