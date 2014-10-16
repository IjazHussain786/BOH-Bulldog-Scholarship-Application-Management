module BohFoundation.Dtos.Person {
    export class PhoneInformationModel {
        constructor(
            public phoneNumber?: number,
            public bestTimeToContactByPhone?: BohFoundation.Common.Enums.TimeOfDayEnum,
            public lastUpdated?: Date) {} 
    }
}