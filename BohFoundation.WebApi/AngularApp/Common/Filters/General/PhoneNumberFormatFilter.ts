module BohFoundation.Common.Filters.General {
    export class PhoneNumberFormatFilter {
        constructor() {
            return ((phoneNumber: number) => {

                if (phoneNumber != undefined) {

                    var phoneNumberString = phoneNumber.toString();

                    for (var i = phoneNumberString.length; i < 10; i++) {
                        phoneNumberString = phoneNumberString + "x";
                    }

                    var areaCode = phoneNumberString.slice(0, 3);
                    var middleThree = phoneNumberString.slice(3, 6);
                    var finalFour = phoneNumberString.slice(6);

                    return "(" + areaCode + ")" + middleThree + "-" + finalFour;
                } else {
                    return "(xxx)xxx-xxxx";
                }
            });
        }
    }    
} 

module BohFoundation.Main {
    Register.Common.filter("phoneNumberFormat", Common.Filters.General.PhoneNumberFormatFilter);
}