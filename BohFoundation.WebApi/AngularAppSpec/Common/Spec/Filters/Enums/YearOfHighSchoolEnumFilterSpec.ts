module BohFoundation.Common.Spec.Filters.Enums {
    describe('YearOfHighSchoolEnum', () => {
        beforeEach(module('BohFoundation.Common'));

        it('should return freshman in string format.', (inject((yearOfHighSchoolEnumFilter) => {
            var result = yearOfHighSchoolEnumFilter(Common.Enums.YearOfHighSchool.Freshman);
            expect(result).toBe("Freshman");
        })));

        it('should return Sophomore in string format.', (inject((yearOfHighSchoolEnumFilter) => {
            var result = yearOfHighSchoolEnumFilter(Common.Enums.YearOfHighSchool.Sophomore);
            expect(result).toBe("Sophomore");
        })));

        it('should return Junior in string format.', (inject((yearOfHighSchoolEnumFilter) => {
            var result = yearOfHighSchoolEnumFilter(Common.Enums.YearOfHighSchool.Junior);
            expect(result).toBe("Junior");
        })));

        it('should return Senior in string format.', (inject((yearOfHighSchoolEnumFilter) => {
            var result = yearOfHighSchoolEnumFilter(Common.Enums.YearOfHighSchool.Senior);
            expect(result).toBe("Senior");
        })));
    });
} 