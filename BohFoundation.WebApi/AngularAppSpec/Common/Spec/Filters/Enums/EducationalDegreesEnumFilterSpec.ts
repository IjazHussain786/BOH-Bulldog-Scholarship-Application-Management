module BohFoundation.Common.Spec.Filters.Enums {
    describe('EducationalDegreesEnumFilter', () => {
        beforeEach(module('BohFoundation.Common'));

        it('should return Associates in string format.', (inject((educationalDegreesEnumFilter) => {
            var result = educationalDegreesEnumFilter(Common.Enums.EducationalDegreesEnum.Associates);
            expect(result).toBe("Associates");
        })));

        it('should return HighSchool in string format.', (inject((educationalDegreesEnumFilter) => {
            var result = educationalDegreesEnumFilter(Common.Enums.EducationalDegreesEnum.HighSchool);
            expect(result).toBe("High School Degree");
        })));

        it('should return NoDegrees in string format.', (inject((educationalDegreesEnumFilter) => {
            var result = educationalDegreesEnumFilter(Common.Enums.EducationalDegreesEnum.NoDegrees);
            expect(result).toBe("Didn't Finish High School");
        })));

        it('should return Bachelors in string format.', (inject((educationalDegreesEnumFilter) => {
            var result = educationalDegreesEnumFilter(Common.Enums.EducationalDegreesEnum.Bachelors);
            expect(result).toBe("Bachelors");
        })));

        it('should return Masters in string format.', (inject((educationalDegreesEnumFilter) => {
            var result = educationalDegreesEnumFilter(Common.Enums.EducationalDegreesEnum.Masters);
            expect(result).toBe("Masters");
        })));

        it('should return Doctorate in string format.', (inject((educationalDegreesEnumFilter) => {
            var result = educationalDegreesEnumFilter(Common.Enums.EducationalDegreesEnum.Doctorate);
            expect(result).toBe("Doctorate");
        })));

    });
}