module BohFoundation.Applicant.LowGrades.Services {
    export interface ILowGradesService {
        getLowGrades(): ng.resource.IResource<any>;
        resolveGetLowGrades(serverResponse: Common.Models.ServerResponseModel): void;

        postLowGrades(): ng.resource.IResource<any>;
        resolvePostLowGrades(serverResponse: Common.Models.ServerResponseModel): void;

        getLowGradesArray(): Array<Dtos.Applicant.Academic.LowGradeModel>;
        getLowGradeNotificationInformationModel(): Dtos.Applicant.Notifications.ILowGradeNotificationInformationModel;
        getArraySameAsServer(): boolean;

        modifyLowGrade(index: number, newLowGrade: Dtos.Applicant.Academic.LowGradeModel): void;
        addLowGrade(newLowGrade: Dtos.Applicant.Academic.LowGradeModel): void;
        deleteLowGrade(index: number): void;
    }

    export class LowGradesService implements ILowGradesService {

        private lowGradesArray: Array<Dtos.Applicant.Academic.LowGradeModel>;
        private lowGradeNotificationsInformationModel: Dtos.Applicant.Notifications.ILowGradeNotificationInformationModel;
        private arraySameAsServer: boolean;

        private typeOfData = "low grade";
        private lowGradesApiEndpoint = "/api/applicant/lowgrades";

        static $inject = ['AuthRequiredRepository', 'GenericResolver'];

        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        getLowGrades(): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.lowGradesApiEndpoint);
        }

        resolveGetLowGrades(serverResponse: Common.Models.ServerResponseModel): void {
            var lowGradesWithGpa: Dtos.Applicant.Academic.LowGradesWithGpaModel = this.genericResolver.genericGetResolver(this.typeOfData, serverResponse);
            if (serverResponse.success) {
                this.setArrayWithDataFromServer(lowGradesWithGpa);
                if (lowGradesWithGpa.lowGrades.length > 0) {
                    this.setLowGradeNotificationsInformationModel(lowGradesWithGpa.gpa, lowGradesWithGpa.lowGrades[0].lastUpdated);
                } else {
                    this.setLowGradeNotificationsInformationModel(lowGradesWithGpa.gpa);
                }
                this.setArrayAsSameStatus(true);
            } 
        }

        postLowGrades(): ng.resource.IResource<any> {
            return this.authRequiredRepository.post(this.lowGradesArray, this.lowGradesApiEndpoint);
        }

        resolvePostLowGrades(serverResponse: Common.Models.ServerResponseModel): void {
            if (serverResponse.success) {
                this.lowGradeNotificationsInformationModel.lastUpdatedLowGrade = new Date();
                this.setArrayAsSameStatus(true);
            }
            this.genericResolver.genericPostResolver(this.typeOfData, serverResponse);
        }

        getLowGradesArray(): Array<Dtos.Applicant.Academic.LowGradeModel> {
            return this.lowGradesArray;
        }

        getArraySameAsServer(): boolean {
            return this.arraySameAsServer;
        }

        getLowGradeNotificationInformationModel(): Dtos.Applicant.Notifications.ILowGradeNotificationInformationModel {
            if (this.lowGradeNotificationsInformationModel != undefined) {
                return this.lowGradeNotificationsInformationModel;
            } else {
                return new Dtos.Applicant.Notifications.LowGradeNotificationInformationModel(0, 0);
            }
        }

        modifyLowGrade(index: number, newLowGrade: Dtos.Applicant.Academic.LowGradeModel): void {
            this.lowGradesArray[index] = newLowGrade;
            this.setArrayAsSameStatus(false);
        }

        addLowGrade(newLowGrade: Dtos.Applicant.Academic.LowGradeModel): void {
            this.lowGradesArray.push(newLowGrade);
            this.setArrayAsSameStatus(false);
            this.setLowGradeNotificationsInformationModel(this.lowGradeNotificationsInformationModel.gpa, new Date());
        }

        deleteLowGrade(index: number): void {
            this.lowGradesArray.splice(index, 1);
            this.setArrayAsSameStatus(false);
            this.setLowGradeNotificationsInformationModel(this.lowGradeNotificationsInformationModel.gpa, new Date());
        }

        private setArrayWithDataFromServer(lowGradesWithGpaModel: Dtos.Applicant.Academic.LowGradesWithGpaModel) {
            this.lowGradesArray = lowGradesWithGpaModel.lowGrades;
        }

        private setLowGradeNotificationsInformationModel(gpa: number, dateLastUpdated?: Date) {
            if (this.lowGradesArray.length == 0) {
                this.lowGradeNotificationsInformationModel = new Dtos.Applicant.Notifications.LowGradeNotificationInformationModel(this.lowGradesArray.length, gpa);
            } else {
                this.lowGradeNotificationsInformationModel = new Dtos.Applicant.Notifications.LowGradeNotificationInformationModel(this.lowGradesArray.length, gpa, dateLastUpdated);
            }
        }

        private setArrayAsSameStatus(isSameAsServer: boolean) {
            this.arraySameAsServer = isSameAsServer;
        }
    }
} 

module BohFoundation.Main {
    Register.Applicant.service("LowGradesService", Applicant.LowGrades.Services.LowGradesService);
}