module BohFoundation.Dtos.Reference.Anonymous {
    export class InformationForReferenceFormModel {
        constructor(
            public applicantsRelationshipToYou?: string,
            public applicantsName?: Person.NameModel,
            public referencesName?: Person.NameModel,
            public phoneNumberLastUpdated?: Date,
            public letterOfRecommendationAlreadyRecieved?: boolean
            ) { }
    }
}