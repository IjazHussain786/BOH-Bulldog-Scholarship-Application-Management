module BohFoundation.Dtos.Admin.EssayTopics {
    export class EssayTopicModel {
        constructor(
            public id: number,
            public titleOfEssay: string,
            public essayPrompt: string,
            public revisionDateTime: Date,
            public forWhatGraduatingYears: Array<number>,
            public lastRevisionAuthor: Person.NameModel) { }
    }
} 