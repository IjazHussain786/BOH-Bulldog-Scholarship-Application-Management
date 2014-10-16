 module BohFoundation.Common.Filters.Enums {
     export class TimeOfDayEnumFilter {
         constructor() {
             return ((itemNumber: Common.Enums.TimeOfDayEnum) => {
                 switch (itemNumber) {
                     case Common.Enums.TimeOfDayEnum.Morning:
                         return "Morning";
                     case Common.Enums.TimeOfDayEnum.Noon:
                         return "Noon";
                     case Common.Enums.TimeOfDayEnum.Afternoon:
                         return "Afternoon";
                     case Common.Enums.TimeOfDayEnum.Evening:
                         return "Evening";
                     case Common.Enums.TimeOfDayEnum.Anytime:
                         return "Anytime";
                     default:
                         return "ERROR";
                 }
             });
         }
     }
 }

 module BohFoundation.Main {
     Register.Common.filter("timeOfDayEnum", Common.Filters.Enums.TimeOfDayEnumFilter);
 }