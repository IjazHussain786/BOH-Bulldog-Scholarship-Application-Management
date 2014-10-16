 module BohFoundation.Applicant.PersonalInformation.Spec.Modules {
     describe('DatePickerSettingsModel', () => {
         var datePickerSettings: Models.DatePickerSettingsModel;
         var now;

         beforeEach(() => {
             now = new Date();
             datePickerSettings = new Models.DatePickerSettingsModel();
         });

         it('should have a max date of today - 15 years.', () => {
             expect(datePickerSettings.maxDate).toEqual(new Date(now.getFullYear() - 15, now.getMonth(), now.getDate()));
         });

         it('should have a initialDate of today - 18 years.', () => {
             expect(datePickerSettings.initialDate).toEqual(new Date(now.getFullYear() - 18, now.getMonth(), now.getDate()));
         });

         it('should have a year range of 5.', () => {
             expect(datePickerSettings.yearRange).toBe(5);
         });

         it('should have a false show weeks.', () => {
             expect(datePickerSettings.showWeeks).toBeFalsy();
         });

         it('should have a false currentText setting.', () => {
             expect(datePickerSettings.currentText).toBeFalsy();
         });

         it('should have format of shortDate.', () => {
             expect(datePickerSettings.format).toBe("shortDate");
         });
     });
 }