using System;
using BohFoundation.Domain.EntityFrameworkModels.Admins;
using BohFoundation.Domain.EntityFrameworkModels.Applicants;
using BohFoundation.Domain.EntityFrameworkModels.ApplicationEvaluators;
using BohFoundation.Domain.EntityFrameworkModels.References;

namespace BohFoundation.Domain.EntityFrameworkModels.Persons
{
    public class Person
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Name Name { get; set; }
        public virtual ContactInformation ContactInformation { get; set; }
        public virtual Applicant Applicant { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual ApplicationEvaluator ApplicationEvaluator { get; set; }
        public virtual Reference Reference { get; set; }
    }
}