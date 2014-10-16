module BohFoundation.ApplicationEvaluator.EvaluateApplicants.GetAllApplicants.Controllers {
    export interface IDisplayAllFinalizedApplicantsDirectiveCtrl {
        $scope: IDisplayAllFinalizedApplicantsDirectiveCtrlScope
    }


    export interface IDisplayAllFinalizedApplicantsDirectiveCtrlScope extends ng.IScope {
        getApplicantSummaries(): Array<Dtos.ApplicationEvaluator.EvaluatingApplicants.ShowAllApplicants.ApplicantSummaryModel>;
        getGraduatingYear(): number;
        gotoApplicant(index: number): void;

        gotoRandomApplicant(): void;
        canGotoSummaryOfRatings(): boolean;
        gotoSummaryOfRatings(): void;
    }

    export class DisplayAllFinalizedApplicantsDirectiveCtrl implements IDisplayAllFinalizedApplicantsDirectiveCtrl {
        
        static $inject = ['$scope','GetAllApplicantsService'];
        constructor(
            public $scope: IDisplayAllFinalizedApplicantsDirectiveCtrlScope,
            private getAllApplicantsService: Services.IGetAllApplicantsService) {

            this.initalize();

            $scope.getGraduatingYear = () => {
                return this.getAllApplicantsService.getClassYear();
            };

            $scope.getApplicantSummaries = () => {
                return this.getAllApplicantsService.getListOfApplicantSummaries();
            }

            $scope.gotoApplicant = (index: number) => {
                this.getAllApplicantsService.gotoApplicant(index);
            };

            $scope.gotoRandomApplicant = () => {
                this.getAllApplicantsService.gotoRandomApplicant();
            };

            $scope.gotoSummaryOfRatings = () => {
                this.getAllApplicantsService.gotoSummaryOfRatings();
            };

            $scope.canGotoSummaryOfRatings = () => {
                return this.getAllApplicantsService.canGotoSummaryOfRatings();
            };
        }

        private initalize() {
            var resource = this.getAllApplicantsService.getFinalizedApplicantsFromServer();
            resource.$promise.then(data => {
                this.resolve(data); 
            },() => {
                this.resolve(null);
            });
        }

        private resolve(data: any) {
            var success = false;
            if (data != null) {
                success = true;
            }
            this.getAllApplicantsService.resolveGetFinalizedApplicants(new Common.Models.ServerResponseModel(data, success));
        }
    }
    
} 

module BohFoundation.Main {
    Register.ApplicationEvaluator.controller("DisplayAllFinalizedApplicantsDirectiveCtrl", ApplicationEvaluator.EvaluateApplicants.GetAllApplicants.Controllers.DisplayAllFinalizedApplicantsDirectiveCtrl);
}