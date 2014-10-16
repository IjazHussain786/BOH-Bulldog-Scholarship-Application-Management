module BohFoundation.Common.Spec.Filters.General {
    describe('StripAllNonNumbersFilter', () => {
        var filter;

        beforeEach(module('BohFoundation.Common'));

        beforeEach(inject((stripAllNonNumbersFilter) => {
            filter = stripAllNonNumbersFilter;
        }));

        it('should return null if input is undefined.', () => {
            expect(filter(undefined)).toBeNull();
        });

        it('should return 1 with a string like this (1xx)xxx-xxxx.', () => {
            expect(filter("(1xx)xxx-xxxx")).toBe(1);
        });

        it('should return 1234567890 with a string like this (123)456-7890.', () => {
            expect(filter("(123)456-7890")).toBe(1234567890);
        });
    });
}