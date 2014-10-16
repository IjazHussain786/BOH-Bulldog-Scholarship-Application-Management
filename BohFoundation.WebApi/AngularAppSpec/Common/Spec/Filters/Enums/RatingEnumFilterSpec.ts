module BohFoundation.Common.Spec.Filters.Enums {
    describe('RatingEnumFilter', () => {
        beforeEach(module('BohFoundation.Common'));

        it('should return F in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.F);
            expect(result).toBe("F");
        })));

        it('should return C- in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.CMinus);
            expect(result).toBe("C-");
        })));

        it('should return C in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.C);
            expect(result).toBe("C");
        })));

        it('should return C+ in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.CPlus);
            expect(result).toBe("C+");
        })));

        it('should return B- in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.BMinus);
            expect(result).toBe("B-");
        })));

        it('should return B in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.B);
            expect(result).toBe("B");
        })));

        it('should return B+ in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.BPlus);
            expect(result).toBe("B+");
        })));

        it('should return A- in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.AMinus);
            expect(result).toBe("A-");
        })));

        it('should return A in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.A);
            expect(result).toBe("A");
        })));

        it('should return A+ in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(Common.Enums.RatingEnum.APlus);
            expect(result).toBe("A+");
        })));

        it('should return Not Rated in string format.', (inject((ratingEnumFilter) => {
            var result = ratingEnumFilter(null);
            expect(result).toBe("Not Rated");
        })));
    });
} 