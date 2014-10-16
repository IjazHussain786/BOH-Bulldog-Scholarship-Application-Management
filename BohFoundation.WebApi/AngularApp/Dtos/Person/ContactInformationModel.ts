module BohFoundation.Dtos.Person {
    export class ContactInformationModel {
        constructor(
            public address?: AddressModel,
            public emailAddress?: string, 
            public phoneInformation?: PhoneInformationModel,
            public lastUpdated?: Date){ }
    }
} 