module BohFoundation.Common.Filters.General {
    export class StripAllNonNumbersFilter {
        constructor() {
            return ((formattedString: string) => {

                if (formattedString != undefined && formattedString.length > 0) {
                    var numberString = formattedString.replace(/\D/g, '');

                    return parseInt(numberString);
                }
                return null;
            });
        }
    }
} 


module BohFoundation.Main {
    Register.Common.filter("stripAllNonNumbers", Common.Filters.General.StripAllNonNumbersFilter);
}