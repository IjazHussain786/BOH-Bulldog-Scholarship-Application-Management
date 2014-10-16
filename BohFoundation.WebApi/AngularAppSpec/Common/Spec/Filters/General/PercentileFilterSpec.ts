 module BohFoundation.Common.Spec.Filters.General {
     describe('PercentileFilter', () => {
         var filter;

         beforeEach(module('BohFoundation.Common'));

         beforeEach(inject((percentileFilter) => {
             filter = percentileFilter;
         }));

         it("should return 98th percentile.", () => {
             expect(filter(.98)).toBe("98th percentile");
         });

         it("should return 31st percentile.", () => {
             expect(filter(.31)).toBe("31st percentile");
         });

         it("should return 42nd percentile.", () => {
             expect(filter(.42)).toBe("42nd percentile");
         });

         it("should return 11th percentile.", () => {
             expect(filter(.11)).toBe("11th percentile");
         });

         it("should return 12th percentile.", () => {
             expect(filter(.12)).toBe("12th percentile");
         });

         it("should return 2nd percentile.", () => {
             expect(filter(.02)).toBe("02nd percentile");
         });

         it("should return 3rd percentile.", () => {
             expect(filter(.03)).toBe("3rd percentile");
         });
     });
 }