namespace BohFoundation.Domain.Dtos.Admin
{
    public class AdminNotificationCountsDto
    {
        public int PendingApplicationEvaluators { get; set; }

        public int TotalCount { get { return PendingApplicationEvaluators; } }
    }
}
