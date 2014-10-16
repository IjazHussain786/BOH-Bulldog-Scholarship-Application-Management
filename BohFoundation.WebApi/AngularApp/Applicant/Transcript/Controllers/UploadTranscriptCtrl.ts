module BohFoundation.Applicant.Transcript.Controllers {
    export interface IUploadTranscriptCtrlScope extends ng.IScope {
        dropzoneConfig: Common.Models.DropZoneConfigModel;

        getLastUpdated() : Date;
    }

    export interface IUploadTranscriptCtrl {
        $scope:IUploadTranscriptCtrlScope;
    }
    

    export class UploadTranscriptCtrl implements IUploadTranscriptCtrl {
        private apiEndpoint = '/api/applicant/transcript';
        private typeOfFile = '.pdf';

        static $inject = ['$scope', 'UserInformationService', 'TranscriptService'];
        constructor(
            public $scope: IUploadTranscriptCtrlScope,
            private userInformationService: Common.Services.UserInformation.IUserInformationService,
            private transcriptService: Services.ITranscriptService) {

            this.checkToSeeIfItIsAlreadyUploaded();

            $scope.dropzoneConfig = new Common.Models.DropZoneConfigModel(this.apiEndpoint, this.fileName(), this.userInformationService.getUserAuthHeader(), this.typeOfFile, 1, 3);

            $scope.getLastUpdated = () => {
                return this.transcriptService.getLastUpdated();
            };
        }

        private fileName() {
            return this.userInformationService.getUserGuid() + "_transcript_" + this.userInformationService.getGraduatingYear() + this.typeOfFile;
        }

        private checkToSeeIfItIsAlreadyUploaded() {
            var resource = this.transcriptService.getWhenTranscriptWasLastUpdatedFromServer();
            resource.$promise.then(dateFromServer => {
                this.transcriptService.resolveGetLastUpdatedTranscript(new Common.Models.ServerResponseModel(dateFromServer, true));
            }, () => { this.transcriptService.resolveGetLastUpdatedTranscript(new Common.Models.ServerResponseModel(null, false)); });
        }
    }
}

module BohFoundation.Main {
    Register.Applicant.controller('UploadTranscriptCtrl', Applicant.Transcript.Controllers.UploadTranscriptCtrl)
        .config([
            '$routeProvider', (($routeProvider: ng.route.IRouteProvider) => {
                $routeProvider.when('/Applicant/Transcript', {
                    templateUrl: '/AngularApp/Applicant/Transcript/Templates/UploadTranscript.html',
                    controller: 'UploadTranscriptCtrl',
                    publicAccess: false,
                    title: 'Upload Transcript'
                });
            })]);
}