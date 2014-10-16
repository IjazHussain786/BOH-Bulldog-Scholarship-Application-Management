module BohFoundation.Dtos.Admin.References {
    export class GetLetterOfRecommendationGuidModel {
        constructor(
            public applicantsEmailAddress?: string,
            public referencesEmailAddress?: string) { }
    }
}   