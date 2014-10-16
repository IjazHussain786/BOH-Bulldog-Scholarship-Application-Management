module BohFoundation.Common.Filters.General {
    export class PercentileFilter {
        constructor() {
            return ((decimal: number) => {

                if (decimal == .11) {
                    return "11th percentile";
                }

                if (decimal == .12) {
                    return "12th percentile";
                }

                if (decimal == .03) {
                    return "3rd percentile";
                }

                var inPercentString = (decimal * 100).toString();

                if (inPercentString.length == 1) {
                    inPercentString = 0 + inPercentString;
                }

                var secondDigit = inPercentString.charAt(1);

                switch (secondDigit) {
                    case "1":
                        inPercentString = inPercentString + "st";
                        break;
                    case "2":
                        inPercentString = inPercentString + "nd";
                        break;
                    default:
                        inPercentString = inPercentString + "th";
                }

                return inPercentString + " percentile";
            });
        }
    }
 }
 module BohFoundation.Main {
     Register.Common.filter("percentile", Common.Filters.General.PercentileFilter);
 }