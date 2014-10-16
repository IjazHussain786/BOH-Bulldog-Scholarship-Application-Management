module BohFoundation.ApplicationEvaluator.EvaluateApplicants.GetAllApplicants.Services {
    export interface IGetAllApplicantsService {
        getFinalizedApplicantsFromServer(): ng.resource.IResource<any>;
        resolveGetFinalizedApplicants(serverResponse: Common.Models.ServerResponseModel): void;

        getListOfApplicantSummaries(): Array<Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.ApplicantSummaryModel>;
        getPercentRated(): number;
        getClassYear(): number;
        getNumberOfApplicantsNotYetRated(): number;
        getAllFinalizedApplicantsForAGraduatingYearModel(): Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel;
    
        gotoApplicant(applicantsIndex: number): void;
        gotoRandomApplicant(): void;

        canGotoNextRandomApplicant(currentGuid: string): boolean;
        
        canGotoSummaryOfRatings(): boolean;
        gotoSummaryOfRatings(): void;
    }

    export class GetAllApplicantsService implements IGetAllApplicantsService {
        private apiEndpoint = "/api/applicationevaluator/evaluatingapplicants/getlistofapplicants";
        private typeOfData = "finalized applicants";
        private allFinalizedApplicantForAGraduatingYearModel: Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel;

        static $inject = ['AuthRequiredRepository', 'GenericResolver', '$location'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver,
            private $location: ng.ILocationService) { }

        getListOfApplicantSummaries(): Array<Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.ApplicantSummaryModel> {
            if (!this.modelDefined()) {
                return [];
            }
            return this.allFinalizedApplicantForAGraduatingYearModel.applicantSummaries;
        }

        getAllFinalizedApplicantsForAGraduatingYearModel(): Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.AllFinalizedApplicantsForAGraduatingYearModel {
            return this.allFinalizedApplicantForAGraduatingYearModel;
        }

        getPercentRated(): number {
            if (!this.modelDefined()) {
                return 0;
            }
            return this.allFinalizedApplicantForAGraduatingYearModel.percentRated;
        }

        getClassYear(): number {
            if (!this.modelDefined()) {
                return 0;
            }
            return this.allFinalizedApplicantForAGraduatingYearModel.graduatingYear;
        }
        
        getNumberOfApplicantsNotYetRated() {
            if (!this.modelDefined()) {
                return 0;
            }
            return this.allFinalizedApplicantForAGraduatingYearModel.numberOfApplicantsNotYetRated;
        } 

        getFinalizedApplicantsFromServer(): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.apiEndpoint);
        }

        resolveGetFinalizedApplicants(serverResponse: Common.Models.ServerResponseModel): void {
            this.allFinalizedApplicantForAGraduatingYearModel = this.genericResolver.genericGetResolver(this.typeOfData, serverResponse);
        }

        gotoRandomApplicant() {
            this.gotoApplicantWithGuid(this.allFinalizedApplicantForAGraduatingYearModel.nextRandomApplicantForReview);
        }

        gotoApplicant(applicantsIndex: number): void {
            this.gotoApplicantWithGuid(this.allFinalizedApplicantForAGraduatingYearModel.applicantSummaries[applicantsIndex].applicantGuid);
        }

        canGotoNextRandomApplicant(currentGuid: string): boolean {
            if (!this.modelDefined() || this.canGotoSummaryOfRatings()) {
                return false;
            }
            return !(this.allFinalizedApplicantForAGraduatingYearModel.nextRandomApplicantForReview == currentGuid);
        }

        canGotoSummaryOfRatings(): boolean {
            if (this.modelDefined()) {
                if (this.allFinalizedApplicantForAGraduatingYearModel.nextRandomApplicantForReview == "00000000-0000-0000-0000-000000000000") {
                    return true;
                }
            }
            return false;
        }

        gotoSummaryOfRatings(): void {
            this.$location.path("/ApplicationEvaluator/RatingsSummaries");
        }

        private gotoApplicantWithGuid(applicantsGuid: string) {
            this.$location.path("/ApplicationEvaluator/ReviewApplicant/" + applicantsGuid);
        }

        private modelDefined() {
            return this.allFinalizedApplicantForAGraduatingYearModel != undefined;
        }
    }
} 

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("GetAllApplicantsService", ApplicationEvaluator.EvaluateApplicants.GetAllApplicants.Services.GetAllApplicantsService);
}