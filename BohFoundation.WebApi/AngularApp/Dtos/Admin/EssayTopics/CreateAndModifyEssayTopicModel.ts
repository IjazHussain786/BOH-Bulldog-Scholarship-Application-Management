module BohFoundation.Dtos.Admin.EssayTopics {
    export class CreateAndModifyEssayTopicModel {
        constructor(
            public titleOfEssay?: string,
            public essayPrompt?: string,
            public id?: number){}
    }
} 