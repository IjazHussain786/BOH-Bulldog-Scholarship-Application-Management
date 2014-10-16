module BohFoundation.Common.Spec.Filters.General {
    describe('PhoneNumberFormatFilter', () => {
        var filter;

        beforeEach(module('BohFoundation.Common'));

        beforeEach(inject((phoneNumberFormatFilter) => {
            filter = phoneNumberFormatFilter;
        }));

        it('should return a string like this (1xx)xxx-xxxx when it is 1.', () => {
            expect(filter(1)).toBe("(1xx)xxx-xxxx");
        });

        it('should return a string like this (12x)xxx-xxxx when it is 12.', () => {
            expect(filter(12)).toBe("(12x)xxx-xxxx");
        });

        it('should return a string like this (123)xxx-xxxx when it is 123.', () => {
            expect(filter(123)).toBe("(123)xxx-xxxx");
        });

        it('should return a string like this (123)4xx-xxxx when it is 1234.', () => {
            expect(filter(1234)).toBe("(123)4xx-xxxx");
        });

        it('should return a string like this (123)45x-xxxx when it is 12345.', () => {
            expect(filter(12345)).toBe("(123)45x-xxxx");
        });

        it('should return a string like this (123)456-xxxx when it is 123456.', () => {
            expect(filter(123456)).toBe("(123)456-xxxx");
        });

        it('should return a string like this (123)456-7xxx when it is 1234567.', () => {
            expect(filter(1234567)).toBe("(123)456-7xxx");
        });

        it('should return a string like this (123)456-78xx when it is 12345678.', () => {
            expect(filter(12345678)).toBe("(123)456-78xx");
        });

        it('should return a string like this (123)456-789x when it is 123456789.', () => {
            expect(filter(123456789)).toBe("(123)456-789x");
        });

        it('should return a string like this (123)456-7890 when it is 1234567890.', () => {
            expect(filter(1234567890)).toBe("(123)456-7890");
        });

        it('should return a string like this (xxx)xxx-xxxx when it is undefined.', () => {
            expect(filter(undefined)).toBe('(xxx)xxx-xxxx');
        });
    });
}