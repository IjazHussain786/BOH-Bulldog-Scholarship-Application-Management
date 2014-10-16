module BohFoundation.Common.Filters.Enums {
    export class RatingEnumFilter {
        constructor() {
            return ((itemNumber: Common.Enums.RatingEnum) => {
                switch (itemNumber) {
                    case Common.Enums.RatingEnum.F:
                        return "F";
                    case Common.Enums.RatingEnum.CMinus:
                        return "C-";
                    case Common.Enums.RatingEnum.C:
                        return "C";
                    case Common.Enums.RatingEnum.CPlus:
                        return "C+";
                    case Common.Enums.RatingEnum.BMinus:
                        return "B-";
                    case Common.Enums.RatingEnum.B:
                        return "B";
                    case Common.Enums.RatingEnum.BPlus:
                        return "B+";
                    case Common.Enums.RatingEnum.AMinus:
                        return "A-";
                    case Common.Enums.RatingEnum.A:
                        return "A";
                    case Common.Enums.RatingEnum.APlus:
                        return "A+";
                    default:
                        return "Not Rated";
                }
            });
        }
    }
}

module BohFoundation.Main {
    Register.Common.filter("ratingEnum", Common.Filters.Enums.RatingEnumFilter);
} 