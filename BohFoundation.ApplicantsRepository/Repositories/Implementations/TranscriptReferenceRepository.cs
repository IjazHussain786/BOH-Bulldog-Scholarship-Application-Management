using System;
using System.Linq;
using AutoMapper;
using BohFoundation.ApplicantsRepository.Repositories.Interfaces;
using BohFoundation.Domain.Dtos.Applicant.Academic;
using BohFoundation.Domain.Dtos.Common;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Utilities.Context.Interfaces;

namespace BohFoundation.ApplicantsRepository.Repositories.Implementations
{
    public class TranscriptReferenceRepository : ApplicantsRepositoryBase, ITranscriptReferenceRepository
    {
        public TranscriptReferenceRepository(string dbConnection, IClaimsInformationGetters claimsInformationGetters) : base(dbConnection, claimsInformationGetters)
        {
            Mapper.CreateMap<TranscriptBlobReferenceDto, TranscriptBlobReference>();
        }

        public void UpsertTranscriptReference(TranscriptBlobReferenceDto transcriptBlobReferenceDto)
        {
            var blobReference = Mapper.Map<TranscriptBlobReference>(transcriptBlobReferenceDto);
            blobReference.LastUpdated = DateTime.UtcNow;

            using (var context = GetApplicantsDbContext())
            {
                var personFromDb = context.People.First(person => person.Guid == ApplicantGuid);
                var academicInformation = personFromDb.Applicant.AcademicInformation;
                if (academicInformation == null)
                {
                    personFromDb.Applicant.AcademicInformation = new AcademicInformation {Transcript = blobReference};
                }
                else
                {
                    if (academicInformation.Transcript == null)
                    {
                        academicInformation.Transcript = blobReference;
                    }
                    else
                    {
                        academicInformation.Transcript.LastUpdated = blobReference.LastUpdated;
                        academicInformation.Transcript.ReferenceToTranscriptPdf = blobReference.ReferenceToTranscriptPdf;
                        academicInformation.Transcript.BlobContainerName = blobReference.BlobContainerName;
                    }
                }
                context.SaveChanges();
            }
        }

        public LastUpdatedDto LastUpdatedTranscript()
        {
            LastUpdatedDto lastUpdated;

            using (var context = GetApplicantsDbContext())
            {
                lastUpdated =
                    context.People.Where(people => people.Guid == ApplicantGuid)
                        .Select(person =>new LastUpdatedDto{ 
                            LastUpdated = person.Applicant.AcademicInformation.Transcript.LastUpdated})
                        .First();
            }

            return lastUpdated;
        }
    }
}
