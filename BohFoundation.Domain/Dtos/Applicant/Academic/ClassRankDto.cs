using System;

namespace BohFoundation.Domain.Dtos.Applicant.Academic
{
    public class ClassRankDto
    {
        public int ClassNumericalRank { get; set; }
        public int GraduatingClassSize { get; set; }

        public DateTime LastUpdated { get; set; }

        public double ClassPercentile { get { return Math.Round(1 - (double)ClassNumericalRank / GraduatingClassSize, 2); } }
    }
}