using BohFoundation.Domain.Enums;

namespace BohFoundation.Domain.Dtos.Common
{
    public class GenericRatingDto
    {
        public RatingEnum? RatingEnum { get; set; }
        public string Explanation { get; set; }
    }
}
