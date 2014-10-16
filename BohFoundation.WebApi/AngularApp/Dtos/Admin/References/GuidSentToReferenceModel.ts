module BohFoundation.Dtos.Admin.References {
    export class GuidSentToReferenceModel {
        constructor(
            public guidSentToReference: string,
            public alreadyUpdated: boolean,
            public errorMessage: string) { }
    }
}  