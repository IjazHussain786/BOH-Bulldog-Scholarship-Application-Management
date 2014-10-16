module BohFoundation.Applicant.Essays.Controllers {
    export interface IApplicantEssayCtrlRouteParams extends ng.route.IRouteParamsService {
        essayTopicId: string;
    }

    export interface IApplicantEssayCtrlScope extends ng.IScope {
        essayTopicId: number;
        essayModel: Dtos.Applicant.Essay.EssayModel;

        originalEssay: string;

        editEssay(): void;
        modalInstance: any;

        attemptedToSave: boolean;
        successfullySaved: boolean;
    }

    export interface IApplicantEssayCtrl {
        $scope: IApplicantEssayCtrlScope;
    }

    export class ApplicantEssayCtrl implements IApplicantEssayCtrl{
        
        static $inject = ['$scope', '$routeParams', 'ApplicantEssayService', '$modal', 'cssInjector'];

        constructor(public $scope: IApplicantEssayCtrlScope,
            private $routeParams: IApplicantEssayCtrlRouteParams,
            private applicantEssayService: Services.IApplicantEssayService,
            private $modal: ng.ui.bootstrap.IModalService,
            private cssInjector: any) {

            this.initialize();

            $scope.editEssay = () => {
                this.editEssay();
            };
        }

        private initialize() {
            this.cssInjector.add("https://maxcdn.bootstrapcdn.com/font-awesome/4.1.0/css/font-awesome.min.css");
            this.$scope.essayTopicId = parseInt(this.$routeParams.essayTopicId);
            this.refreshEssayModel();
            var resource = this.applicantEssayService.getEssayFromServer(this.$scope.essayTopicId);
            resource.$promise.then(
                (data) => {
                    this.applicantEssayService.resolveGetEssayFromServer(new Common.Models.ServerResponseModel(data, true));
                    this.refreshEssayModel();
                },
                () => { this.applicantEssayService.resolveGetEssayFromServer(new Common.Models.ServerResponseModel(null, false)); });
        }

        private refreshEssayModel() {
            this.$scope.essayModel = this.applicantEssayService.getEssayModel(this.$scope.essayTopicId);
        }

        private editEssay() {
            this.$scope.originalEssay = this.$scope.essayModel.essay;
            this.openModal();
        }

        private openModal() {
            this.$scope.modalInstance = this.$modal.open({
                templateUrl: '/AngularApp/Applicant/Essays/Templates/EditEssayModal.html',
                controller: 'ApplicantEditEssayModalCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    essayModelForModal: () => this.$scope.essayModel
                }
            });

            this.$scope.modalInstance.result.then(
                dataFromModal => {
                    this.addAndPostDataFromModal(dataFromModal);
                },
                () => {
                    this.$scope.essayModel.essay = this.$scope.originalEssay;
                });
        }

        private addAndPostDataFromModal(dataFromModal: Dtos.Applicant.Essay.EssayModel) {
            this.$scope.attemptedToSave = true;
            this.$scope.essayModel = dataFromModal;
            var resource = this.applicantEssayService.postEssay(this.$scope.essayModel);
            resource.$promise.then(
                () => {
                    this.$scope.essayModel.revisionDateTime = new Date();
                    this.resolvePostEssay(true);
                }, () => {
                this.resolvePostEssay(false);
            });
        }

        private resolvePostEssay(success: boolean) {
            this.$scope.successfullySaved = success;
            this.applicantEssayService.resolvePostEssay(new Common.Models.ServerResponseModel(null, success));
        }
    }
}
module BohFoundation.Main {
    Register.Applicant.controller("ApplicantEssayCtrl", Applicant.Essays.Controllers.ApplicantEssayCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant/Essay/:essayTopicId', {
                    templateUrl: '/AngularApp/Applicant/Essays/Templates/ApplicantEssayTemplate.html',
                    controller: 'ApplicantEssayCtrl',
                    publicAccess: false,
                    title: 'Essay'
                });
            })]);
}