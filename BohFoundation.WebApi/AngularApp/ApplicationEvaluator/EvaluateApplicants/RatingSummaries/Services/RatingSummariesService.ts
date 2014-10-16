module BohFoundation.ApplicationEvaluator.EvaluateApplicants.RatingSummaries.Services {
    export interface IRatingSummariesService {
        getRatingSummariesFromServer(): ng.resource.IResource<any>;
        resolveGetRatingSummariesFromServer(serverResponse: Common.Models.ServerResponseModel): void;

        getTop5Applicants(): Array<Dtos.ApplicationEvaluator.RatingSummary.TopApplicantRatingSummaryModel>;

        gotoApplicant(index: number): void;
    }

    export class RatingSummariesService implements IRatingSummariesService {

        private apiEndpoint = "/api/applicationevaluator/evaluatingapplicants/ratingSummaries";
        private typeOfData = "applicant's average ratings";
        private top5ApplicantsModel: Dtos.ApplicationEvaluator.RatingSummary.Top5ApplicantsModel;


        static $inject = ['AuthRequiredRepository', 'GenericResolver', '$location'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver,
            private $location: ng.ILocationService) { }


        getRatingSummariesFromServer(): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.apiEndpoint);
        }

        resolveGetRatingSummariesFromServer(serverResponse: Common.Models.ServerResponseModel): void {
            this.top5ApplicantsModel = this.genericResolver.genericGetResolver(this.typeOfData, serverResponse);
        }

        getTop5Applicants(): Array<Dtos.ApplicationEvaluator.RatingSummary.TopApplicantRatingSummaryModel> {
            if (this.top5ApplicantsModel == undefined) {
                return [];
            }
            return this.top5ApplicantsModel.topApplicants;
        }

        gotoApplicant(index: number): void {
            this.$location.path("/ApplicationEvaluator/ReviewApplicant/" + this.top5ApplicantsModel.topApplicants[index].applicantsGuid);
        }
    }
 }

module BohFoundation.Main {
    Register.ApplicationEvaluator.service("RatingSummariesService", ApplicationEvaluator.EvaluateApplicants.RatingSummaries.Services.RatingSummariesService);
}