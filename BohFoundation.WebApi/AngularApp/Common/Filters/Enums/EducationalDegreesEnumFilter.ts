module BohFoundation.Common.Filters.Enums {
    export class EducationalDegreesEnumFilter {
        constructor() {
            return ((itemNumber: Common.Enums.EducationalDegreesEnum) => {
                switch (itemNumber) {
                    case Common.Enums.EducationalDegreesEnum.Associates:
                        return "Associates";
                    case Common.Enums.EducationalDegreesEnum.HighSchool:
                        return "High School Degree";
                    case Common.Enums.EducationalDegreesEnum.NoDegrees:
                        return "Didn't Finish High School";
                    case Common.Enums.EducationalDegreesEnum.Bachelors:
                        return "Bachelors";
                    case Common.Enums.EducationalDegreesEnum.Masters:
                        return "Masters";
                    case Common.Enums.EducationalDegreesEnum.Doctorate:
                        return "Doctorate";
                    default:
                        return "ERROR";
                }
            });
        }
    }
}

module BohFoundation.Main {
    Register.Common.filter("educationalDegreesEnum", Common.Filters.Enums.EducationalDegreesEnumFilter);
}