module BohFoundation.Common.Filters.Enums {
    export class YearOfHighSchoolEnumFilter {
        constructor() {
            return ((itemNumber: Common.Enums.YearOfHighSchool) => {
                switch (itemNumber) {
                case Common.Enums.YearOfHighSchool.Freshman:
                    return "Freshman";
                case Common.Enums.YearOfHighSchool.Sophomore:
                    return "Sophomore";
                case Common.Enums.YearOfHighSchool.Junior:
                    return "Junior";
                case Common.Enums.YearOfHighSchool.Senior:
                    return "Senior";
                    default:
                        return "Error";
                }
            });
        }
    }
}

module BohFoundation.Main {
    Register.Common.filter("yearOfHighSchoolEnum", Common.Filters.Enums.YearOfHighSchoolEnumFilter);
}