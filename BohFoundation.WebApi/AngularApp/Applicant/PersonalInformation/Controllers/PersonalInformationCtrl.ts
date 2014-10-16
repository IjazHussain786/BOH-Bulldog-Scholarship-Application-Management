module BohFoundation.Applicant.PersonalInformation.Controllers {
    export interface IPersonalInformationCtrlScope extends ng.IScope {
        processing: boolean;

        fullName: string;
        applicantPersonalInformation: Dtos.Applicant.PersonalInformation.ApplicantPersonalInformationModel;

        postApplicantPersonalInformation(form: ng.IFormController): void;

        datePickerSettings: Models.DatePickerSettingsModel;
    }

    export interface IPersonalInformationCtrl {
        $scope: IPersonalInformationCtrlScope
    }

    export class PersonalInformationCtrl implements IPersonalInformationCtrl {
        static $inject = ['$scope','PersonalInformationService'];
        constructor(public $scope: IPersonalInformationCtrlScope,
            private personalInformationService: Services.IPersonalInformationService) {

            this.$scope.datePickerSettings = new Models.DatePickerSettingsModel();

            this.getPersonalInformationFromServer();

            $scope.postApplicantPersonalInformation = (form: ng.IFormController) => {
                this.postApplicantPersonalInformation(form);
            };

        }

        private getPersonalInformationFromServer() {
            this.flipProcessing();
            var resource = this.personalInformationService.getPersonalInformation();
            resource.$promise.then(
                (data) => {
                    this.resolveGetPendingInformation(new Common.Models.ServerResponseModel(data, true));
                    this.flipProcessing();
                    this.setApplicationPersonalInformationOnScope();
                },
                () => {
                    this.resolveGetPendingInformation(new Common.Models.ServerResponseModel(null, false));
                });
        }

        private resolveGetPendingInformation(serverResponse: Common.Models.ServerResponseModel) {
            this.personalInformationService.resolveGetPersonalInformation(serverResponse);
        }

        private flipProcessing() {
            this.$scope.processing = !this.$scope.processing;
        }

        private setApplicationPersonalInformationOnScope() {
            this.$scope.fullName = this.personalInformationService.fullName;
            this.$scope.applicantPersonalInformation = this.personalInformationService.applicantPersonalInformation;
        }

        private postApplicantPersonalInformation(form: ng.IFormController): void {
            if (form.$valid && !this.$scope.processing && this.validateBirthdate()) {
                this.flipProcessing();
                var resource = this.personalInformationService.postPersonalInformation(this.$scope.applicantPersonalInformation);
                this.resolvePost(resource);
            }
        }

        private resolvePost(resource: ng.resource.IResource<any>) {
            resource.$promise.then(
                () => { this.callResolvePostService(true); },
                () => { this.callResolvePostService(false); });
        }

        private callResolvePostService(success: boolean) {
            this.personalInformationService.resolvePostPersonalInformation(new Common.Models.ServerResponseModel(null, success));
            this.flipProcessing();
            if (success) {
                this.$scope.applicantPersonalInformation = this.personalInformationService.applicantPersonalInformation;
            }
        }

        private validateBirthdate(): boolean {
            var birthdate = this.$scope.applicantPersonalInformation.birthdate;
            if (birthdate == undefined) {
                return false;
            } else {
                if (isNaN(birthdate.valueOf())) {
                    return false;
                } else {
                    return true;
                }
            }
        }
    }
} 

module BohFoundation.Main {
    Register.Applicant.controller('PersonalInformationCtrl', Applicant.PersonalInformation.Controllers.PersonalInformationCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant/PersonalInformation', {
                    templateUrl: '/AngularApp/Applicant/PersonalInformation/Templates/PersonalInformation.html',
                    controller: 'PersonalInformationCtrl',
                    publicAccess: false,
                    title: 'Personal Information'
                });
            })]);
}