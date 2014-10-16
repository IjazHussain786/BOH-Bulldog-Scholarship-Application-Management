module BohFoundation.Dtos.Person {
    export class AddressModel {
        constructor(
            public streetAddress1?: string,
            public city?: string,
            public state?: string,
            public zipCode?: number,
            public lastUpdated?: Date,
            public streetAddress2?: string) { }
    }
} 