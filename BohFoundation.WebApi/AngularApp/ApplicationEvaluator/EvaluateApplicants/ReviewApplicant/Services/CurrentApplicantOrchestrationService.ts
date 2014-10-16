module BohFoundation.ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services {
    export interface ICurrentApplicantOrchestrationService {

        setCurrentApplicantsGuid(applicantsGuid: string): void;

        getGeneralInformation(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel;
        getGeneralInformationFromServer(): ng.resource.IResource<any>;
        resolveGetGeneralInformationFromServer(serverMessage: Common.Models.ServerResponseModel): void;

        getExtracurricularActivitiesInformation(): Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel;
        getExtracurricularActivitiesFromServer(): ng.resource.IResource<any>;
        resolveGetExtracurricularActivitiesFromServer(serverMessage: Common.Models.ServerResponseModel): void;

        getEssayAndLetterOfRecommendationSummaries(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CollectionsOfEssaysAndLettersOfRecommendationModel;
        getEssayAndLetterOfRecommendationSummariesFromServer(): ng.resource.IResource<any>;
        resolveGetEssayAndLetterOfRecommendationSummariesFromServer(serverMessage: Common.Models.ServerResponseModel): void;

        getAcademicInformation(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CompletedAcademicInformationModel;
        getAcademicInformationFromServer(): ng.resource.IResource<any>;
        resolveGetAcademicInformationFromServer(serverMessage: Common.Models.ServerResponseModel): void;

        postEssayRating(essayRating: Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.EssayRatingModel): ng.resource.IResource<any>;
        resolvePostEssayRating(serverMessage: Common.Models.ServerResponseModel): void;

        postFirstImpressionRating(firstImpressionRating: Dtos.Common.GenericRatingModel): ng.resource.IResource<any>;
        postFinalOverallRating(finalOverallRating: Dtos.Common.GenericRatingModel): ng.resource.IResource<any>;
        resolvePostNonEssayRating(serverMessage: Common.Models.ServerResponseModel): void;

        getTranscriptFromServer(): ng.resource.IResource<any>;
        resolveGetTranscriptFromServer(serverResponse: Common.Models.ServerResponseModel): void;
        postTranscriptConfirmation(transcriptConfirmation: Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel): ng.resource.IResource<any>;
        resolvePostTranscriptConfirmation(serverResponse: Common.Models.ServerResponseModel): void;
        getTranscriptConfirmed(): Models.TranscriptConfirmed;

        canGotoNextRandomApplicant(): boolean;
        gotoNextRandomApplicant(): void;

        getFinalizedApplicantsFromServer(): ng.resource.IResource<any>;
        resolveGetFinalizedApplicants(serverResponse: Common.Models.ServerResponseModel): void;

        canGotoSummaryOfAllRatings(): boolean;
        gotoSummaryOfAllRatings(): void;
    }

    export class CurrentApplicantOrchestrationService implements ICurrentApplicantOrchestrationService {
        
        private applicantsGuid: string;

        static $inject = [  "CurrentApplicantOverviewInformationService", "CurrentApplicantExtracurricularActivitiesService",
                            "CurrentApplicantEssayAndLettersOfRecommendationSummariesService", "CurrentApplicantAcademicInformationService",
                            "EssayRatingService", "GeneralRatingService", "ConfirmTranscriptService",
                            "GetAllApplicantsService"];

        constructor(
            private currentApplicantOverviewInformationService: ICurrentApplicantOverviewInformationService,
            private currentApplicantExtracurricularActivitiesService: ICurrentApplicantExtracurricularActivitiesService,
            private currentApplicantEssayAndLettersOfRecommendationSummariesService: ICurrentApplicantEssayAndLettersOfRecommendationSummariesService,
            private currentApplicantAcademicInformationService: ICurrentApplicantAcademicInformationService,
            private essayRatingService: Rating.IEssayRatingService,
            private generalRatingService: Rating.IGeneralRatingService,
            private confirmTranscriptService: Transcript.IConfirmTranscriptService,
            private getAllApplicantsService: GetAllApplicants.Services.IGetAllApplicantsService) { }

        setCurrentApplicantsGuid(applicantsGuid: string): void {
            this.applicantsGuid = applicantsGuid;
        }


        getGeneralInformation(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.ApplicantsGeneralInformationModel {
            return this.currentApplicantOverviewInformationService.getGeneralInformation();
        }

        getGeneralInformationFromServer(): ng.resource.IResource<any> {
            return this.currentApplicantOverviewInformationService.getGeneralInformationFromServer(this.applicantsGuid);
        }

        resolveGetGeneralInformationFromServer(serverMessage: Common.Models.ServerResponseModel): void {
            this.currentApplicantOverviewInformationService.resolveGetGeneralInformationFromServer(serverMessage);
        }


        getExtracurricularActivitiesInformation(): Dtos.Applicant.Extracurricular.ExtracurricularActivitiesModel {
            return this.currentApplicantExtracurricularActivitiesService.getExtracurricularActivitiesInformation();
        }

        getExtracurricularActivitiesFromServer() {
            return this.currentApplicantExtracurricularActivitiesService.getExtracurricularActivitiesFromServer(this.applicantsGuid);
        }

        resolveGetExtracurricularActivitiesFromServer(serverMessage: Common.Models.ServerResponseModel): void {
            this.currentApplicantExtracurricularActivitiesService.resolveGetExtracurricularActivitiesFromServer(serverMessage);
        }


        getEssayAndLetterOfRecommendationSummaries(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CollectionsOfEssaysAndLettersOfRecommendationModel {
            return this.currentApplicantEssayAndLettersOfRecommendationSummariesService.getEssayAndLetterOfRecommendationSummaries();
        }

        getEssayAndLetterOfRecommendationSummariesFromServer(): ng.resource.IResource<any> {
            return this.currentApplicantEssayAndLettersOfRecommendationSummariesService.getEssayAndLetterOfRecommendationSummariesFromServer(this.applicantsGuid);
        }

        resolveGetEssayAndLetterOfRecommendationSummariesFromServer(serverMessage: Common.Models.ServerResponseModel): void {
            this.currentApplicantEssayAndLettersOfRecommendationSummariesService.resolveGetEssayAndLetterOfRecommendationSummariesFromServer(serverMessage);
        }


        getAcademicInformation(): Dtos.ApplicationEvaluator.EvaluatingApplicants.DisplayApplication.CompletedAcademicInformationModel {
            return this.currentApplicantAcademicInformationService.getAcademicInformation();
        }

        getAcademicInformationFromServer(): ng.resource.IResource<any> {
            return this.currentApplicantAcademicInformationService.getAcademicInformationFromServer(this.applicantsGuid);
        }

        resolveGetAcademicInformationFromServer(serverMessage: Common.Models.ServerResponseModel): void {
            this.currentApplicantAcademicInformationService.resolveGetAcademicInformationFromServer(serverMessage);
        }


        postEssayRating(essayRating: Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.EssayRatingModel): ng.resource.IResource<any> {
            essayRating.applicantsGuid = this.applicantsGuid;
            return this.essayRatingService.postEssayRating(essayRating);
        }

        resolvePostEssayRating(serverMessage: Common.Models.ServerResponseModel): void {
            this.essayRatingService.resolvePostEssayRating(serverMessage);
        }


        postFirstImpressionRating(genericRatingModel: Dtos.Common.GenericRatingModel): ng.resource.IResource<any> {
            return this.generalRatingService.postFirstImpressionRating(this.createRatingWithApplicantsGuid(genericRatingModel));
        }

        postFinalOverallRating(genericRatingModel: Dtos.Common.GenericRatingModel): ng.resource.IResource<any> {
            return this.generalRatingService.postFinalOverallRating(this.createRatingWithApplicantsGuid(genericRatingModel));
        }

        resolvePostNonEssayRating(serverMessage: Common.Models.ServerResponseModel): void {
            this.generalRatingService.resolvePostNonEssayRating(serverMessage);
        }


        getTranscriptFromServer(): ng.resource.IResource<any> {
            return this.confirmTranscriptService.getTranscriptFromServer(this.getAcademicInformation().transcriptBlobReference);
        }

        resolveGetTranscriptFromServer(serverResponse: Common.Models.ServerResponseModel): void {
            this.confirmTranscriptService.resolveGetTranscriptFromServer(serverResponse);
        }

        postTranscriptConfirmation(transcriptConfirmation: Dtos.ApplicationEvaluator.EvaluatingApplicants.ConfirmTranscriptModel): ng.resource.IResource<any> {
            transcriptConfirmation.applicantsGuid = this.applicantsGuid;
            return this.confirmTranscriptService.postTranscriptConfirmation(transcriptConfirmation);
        }

        resolvePostTranscriptConfirmation(serverResponse: Common.Models.ServerResponseModel): void {
            this.confirmTranscriptService.resolvePostTranscriptConfirmation(serverResponse);
        }

        getTranscriptConfirmed(): Models.TranscriptConfirmed {
            var completedAcademicInformation = this.getAcademicInformation();

            if (completedAcademicInformation == null) {
                return null;
            }

            var model = new Models.TranscriptConfirmed(completedAcademicInformation.transcriptMatchesDatabaseValues, null, completedAcademicInformation.transcriptDoesNotMatchDatabaseValues);

            if (!model.confirmed && !model.notCorrect) {
                model.notChecked = true;
            } else {
                model.notChecked = false;
            }

            return model;
        }


        canGotoNextRandomApplicant() {
            return this.getAllApplicantsService.canGotoNextRandomApplicant(this.applicantsGuid);
        }

        gotoNextRandomApplicant() {
            this.getAllApplicantsService.gotoRandomApplicant();
        }

        getFinalizedApplicantsFromServer(): ng.resource.IResource<any> {
            return this.getAllApplicantsService.getFinalizedApplicantsFromServer();
        }

        resolveGetFinalizedApplicants(serverResponse: Common.Models.ServerResponseModel): void {
            this.getAllApplicantsService.resolveGetFinalizedApplicants(serverResponse);
        }

        canGotoSummaryOfAllRatings(): boolean {
            return this.getAllApplicantsService.canGotoSummaryOfRatings();
        }

        gotoSummaryOfAllRatings(): void {
            this.getAllApplicantsService.gotoSummaryOfRatings();
        }

        private createRatingWithApplicantsGuid(genericRating: Dtos.Common.GenericRatingModel) {
            return new Dtos.ApplicationEvaluator.EvaluatingApplicants.RatingUpdate.RatingWithApplicantsGuidModel(genericRating, this.applicantsGuid);
        }
    }
} 

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("CurrentApplicantOrchestrationService", ApplicationEvaluator.EvaluateApplicants.ReviewApplicant.Services.CurrentApplicantOrchestrationService);
}