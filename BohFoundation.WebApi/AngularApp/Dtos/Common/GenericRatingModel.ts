module BohFoundation.Dtos.Common {
    export class GenericRatingModel {
        constructor(
            public ratingEnum?: BohFoundation.Common.Enums.RatingEnum,
            public explanation?: string 
            ) {
        }
    }
}  