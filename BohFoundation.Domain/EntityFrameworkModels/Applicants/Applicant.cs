using System.Collections.Generic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Extracurricular;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Family;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.EntityFrameworkModels.References;

namespace BohFoundation.Domain.EntityFrameworkModels.Applicants
{
    public class Applicant 
    {
        public int Id { get; set; }
        
        public virtual Person Person { get; set; }
        public virtual ApplicantPersonalInformation ApplicantPersonalInformation { get; set; }
        public virtual AcademicInformation AcademicInformation { get; set; }
        public virtual FamilyInformation FamilyInformation { get; set; }
        public virtual ExtracurricularActivities ExtracurricularActivities { get; set; }
        public virtual ApplicantMetadata Metadata { get; set; }

        public virtual ICollection<Essay> Essays { get; set; }
        public virtual ICollection<LetterOfRecommendation> LettersOfRecommendation { get; set; }
        public virtual ICollection<ApplicantRating> ApplicantRatings { get; set; }
    }
}