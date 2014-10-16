 module BohFoundation.Common.Spec.Filters.Enums {
     describe('TimeOfDayEnumFilter', () => {
         beforeEach(module('BohFoundation.Common'));

         it('should return Morning in string format.', (inject((timeOfDayEnumFilter) => {
             var result = timeOfDayEnumFilter(Common.Enums.TimeOfDayEnum.Morning);
             expect(result).toBe("Morning");
         })));

         it('should return Afternoon in string format.', (inject((timeOfDayEnumFilter) => {
             var result = timeOfDayEnumFilter(Common.Enums.TimeOfDayEnum.Afternoon);
             expect(result).toBe("Afternoon");
         })));

         it('should return Noon in string format.', (inject((timeOfDayEnumFilter) => {
             var result = timeOfDayEnumFilter(Common.Enums.TimeOfDayEnum.Noon);
             expect(result).toBe("Noon");
         })));

         it('should return Evening in string format.', (inject((timeOfDayEnumFilter) => {
             var result = timeOfDayEnumFilter(Common.Enums.TimeOfDayEnum.Evening);
             expect(result).toBe("Evening");
         })));

         it('should return Anytime in string format.', (inject((timeOfDayEnumFilter) => {
             var result = timeOfDayEnumFilter(Common.Enums.TimeOfDayEnum.Anytime);
             expect(result).toBe("Anytime");
         })));
     });
 }