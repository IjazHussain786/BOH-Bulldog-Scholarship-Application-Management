 module BohFoundation.Common.Spec.Services.Utilities {
     describe('CreateYearArrayService', () => {

         var service;
         var result;
         var thisYear;

         beforeEach(() => {
             service = new Common.Services.Utilities.CreateYearArrayService();
             result = service.getThisYearPlusFourArray();
             thisYear = new Date().getFullYear();
         });

         it('should return an array of 5 items', () => {
             expect(result.length).toBe(5);
         });

         it('should have the correct years in the correct indexes.', () => {
             for (var i = 0; i < result.length; i++) {
                 expect(result[i]).toBe(thisYear + i);
             }
         });
     });
 }