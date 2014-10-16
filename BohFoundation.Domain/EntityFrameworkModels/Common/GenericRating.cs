using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.EntityFrameworkModels.Common
{
    public class GenericRating
    {
        public int Id { get; set; }
        public RatingEnum? RatingEnum { get; set; }
        public string Explanation { get; set; }
    }
}
