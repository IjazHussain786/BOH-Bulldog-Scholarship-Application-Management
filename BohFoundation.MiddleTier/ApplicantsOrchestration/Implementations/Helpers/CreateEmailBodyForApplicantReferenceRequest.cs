using System.Text;
using AutoMapper;
using BohFoundation.Domain.Dtos.Applicant.References;
using BohFoundation.MiddleTier.ApplicantsOrchestration.Interfaces.Helpers;
using BohFoundation.Utilities.Context.Interfaces.Context;
using BohFoundation.Utilities.Utilities.Interfaces;

namespace BohFoundation.MiddleTier.ApplicantsOrchestration.Implementations.Helpers
{
    public class CreateEmailBodyForApplicantReferenceRequest : ICreateEmailBodyForApplicantReferenceRequest
    {
        private readonly IRandomObjectGenerator _objectGenerator;
        private readonly IHttpContextInformationGetters _httpContextInformation;

        public CreateEmailBodyForApplicantReferenceRequest(IRandomObjectGenerator objectGenerator, IHttpContextInformationGetters httpContextInformation)
        {
            _objectGenerator = objectGenerator;
            _httpContextInformation = httpContextInformation;
            Mapper.CreateMap<ApplicantReferenceInputDto, ApplicantReferenceForEntityFrameworkDto>();
        }

        public ApplicantReferenceForEntityFrameworkDto CreateBody(ApplicantReferenceInputDto applicantReferenceInputDto)
        {
            var efDto = Mapper.Map<ApplicantReferenceForEntityFrameworkDto>(applicantReferenceInputDto);

            efDto.GuidLink = _objectGenerator.GenerateNewGuid();
            efDto.MessageParagraph = BuildString(efDto);

            return efDto;
        }

        private string BuildString(ApplicantReferenceForEntityFrameworkDto message)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(message.Salutation);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(message.MessageParagraph);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(message.Signature);
            stringBuilder.AppendLine();
            stringBuilder.Append(CreateLinkLine(message));
            return stringBuilder.ToString();
        }

        private string CreateLinkLine(ApplicantReferenceForEntityFrameworkDto message)
        {
            var baseUrl = _httpContextInformation.GetRequestHttpBaseUrl();
            return "Please click the following link to fill out the recommendation: " + baseUrl + "/Reference/LetterOfRecommendation/Anon/" + message.GuidLink;
        }
    }
}
