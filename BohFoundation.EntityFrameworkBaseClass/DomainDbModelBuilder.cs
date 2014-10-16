using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Academic;
using BohFoundation.Domain.EntityFrameworkModels.Applicants.Family;
using BohFoundation.Domain.EntityFrameworkModels.Common;
using BohFoundation.Domain.EntityFrameworkModels.Persons;
using BohFoundation.Domain.EntityFrameworkModels.References;

namespace BohFoundation.EntityFrameworkBaseClass
{
    public static class DomainDbModelBuilder
    {
        public static void CreateModel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applicant>()
                .HasOptional(applicant => applicant.AcademicInformation)
                .WithRequired(academicInformation => academicInformation.Applicant);

            modelBuilder.Entity<Applicant>()
                .HasOptional(applicant => applicant.ApplicantPersonalInformation)
                .WithRequired(applicantPersonalInformation => applicantPersonalInformation.Applicant);

            modelBuilder.Entity<Applicant>()
                .HasOptional(applicant => applicant.FamilyInformation)
                .WithRequired(familyInformation => familyInformation.Applicant);

            modelBuilder.Entity<Applicant>()
                .HasOptional(applicant => applicant.ExtracurricularActivities)
                .WithRequired(extraCurricular => extraCurricular.Applicant);

            modelBuilder.Entity<Applicant>()
                .HasOptional(applicant => applicant.Metadata)
                .WithRequired(metadata => metadata.Applicant);

            modelBuilder.Entity<FamilyInformation>()
                .HasOptional(familyInformation => familyInformation.W2)
                .WithRequired(w2 => w2.FamilyInformation);

            modelBuilder.Entity<Person>()
                .HasOptional(person => person.Name)
                .WithRequired(name => name.Person);

            modelBuilder.Entity<Person>()
                .HasOptional(person => person.ContactInformation)
                .WithRequired(contantInformation => contantInformation.Person);

            modelBuilder.Entity<Person>()
                .HasOptional(person => person.Applicant)
                .WithRequired(applicant => applicant.Person);

            modelBuilder.Entity<Person>()
                .HasOptional(person => person.ApplicationEvaluator)
                .WithRequired(applicationEvaluator => applicationEvaluator.Person);

            modelBuilder.Entity<Person>()
                .HasOptional(person => person.Admin)
                .WithRequired(admin => admin.Person);

            modelBuilder.Entity<Person>()
                .HasOptional(person => person.Reference)
                .WithRequired(admin => admin.Person);

            modelBuilder.Entity<Person>()
                .Property(person => person.Guid)
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] {new IndexAttribute("Index")}));

            modelBuilder.Entity<ContactInformation>()
                .HasOptional(contactInformation => contactInformation.Address)
                .WithRequired(address => address.ContantInformation);

            modelBuilder.Entity<ContactInformation>()
                .Property(contactInformation => contactInformation.EmailAddress)
                .HasMaxLength(90)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("Index") { IsUnique = true } }));

            modelBuilder.Entity<ContactInformation>()
                .HasOptional(contactInformation => contactInformation.PhoneInformation)
                .WithRequired(phoneInformation => phoneInformation.ContactInformation);

            modelBuilder.Entity<AcademicInformation>()
                .HasMany(academicInformation => academicInformation.LowGrades)
                .WithRequired(lowGrades => lowGrades.AcademicInformation);

            modelBuilder.Entity<AcademicInformation>()
                .HasOptional(academicInformation => academicInformation.ClassRank)
                .WithRequired(classRank => classRank.AcademicInformation);

            modelBuilder.Entity<AcademicInformation>()
                .HasOptional(academicInformation => academicInformation.Transcript)
                .WithRequired(transcript => transcript.AcademicInformation);

            modelBuilder.Entity<GraduatingClass>()
                .Property(graduatingClass => graduatingClass.GraduatingYear)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new[] { new IndexAttribute("Index") { IsUnique = true } }));

            modelBuilder.Entity<EssayTopic>()
                .HasMany(essayTopic => essayTopic.ForWhatGraduatingYears)
                .WithMany(graduatingClass => graduatingClass.EssayTopics)
                .Map(m => m.ToTable("EssayTopicGraduatingClasses"));

            modelBuilder.Entity<EssayTopic>()
                .Property(essayTopic => essayTopic.ConcurrencyTimestamp)
                .IsConcurrencyToken()
                .IsRowVersion();

            modelBuilder.Entity<LetterOfRecommendation>()
                .Property(letterOfRecommenation => letterOfRecommenation.GuidSentToReference)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new[] {new IndexAttribute("LetterOfRecommenationGuidIndex")}));
        }
    }
}
