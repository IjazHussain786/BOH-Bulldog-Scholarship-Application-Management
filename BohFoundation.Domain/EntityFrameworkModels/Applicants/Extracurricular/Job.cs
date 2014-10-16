namespace BohFoundation.Domain.EntityFrameworkModels.Applicants.Extracurricular
{
    public class Job
    {
        public int Id { get; set; }
        public string Employer { get; set; }
        public string Position { get; set; }
        public string ShortSummaryOfWorkResponsibilities { get; set; }

        public virtual ExtracurricularActivities ExtracurricularActivities { get; set; }
    }
}