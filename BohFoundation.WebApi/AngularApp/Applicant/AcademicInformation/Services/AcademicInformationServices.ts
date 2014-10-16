module BohFoundation.Applicant.AcademicInformation.Services {
    export interface IAcademicInformationServices {
        getAcademicInformation(): ng.resource.IResource<any>;
        resolveGetAcademicInformation(result: Common.Models.ServerResponseModel): void;

        postAcademicInformation(model: Dtos.Applicant.Academic.AcademicInformationModel): ng.resource.IResource<any>;
        resolvePostAcademicInformation(result: Common.Models.ServerResponseModel): void;

        getApplicantAcademicInformationModel(): Dtos.Applicant.Academic.AcademicInformationModel;
    }

    export class AcademicInformationServices implements IAcademicInformationServices {
        
        private applicantAcademicInformation: Dtos.Applicant.Academic.AcademicInformationModel;

        private typeOfData = "academic";
        private academicInformationApiEndpoint = "/api/applicant/academicinformation";

        static $inject = ['AuthRequiredRepository', 'GenericResolver'];

        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        getApplicantAcademicInformationModel() {
            return this.applicantAcademicInformation;
        }

        getAcademicInformation(): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.academicInformationApiEndpoint);
        }

        resolveGetAcademicInformation(result: Common.Models.ServerResponseModel): void {
            var dataFromServer = this.genericResolver.genericGetResolver(this.typeOfData, result);
            if (result.success) {
                this.getSuccessResult(dataFromServer);
            }
        }

        postAcademicInformation(model: Dtos.Applicant.Academic.AcademicInformationModel) {
            return this.authRequiredRepository.post(model, this.academicInformationApiEndpoint);
        }

        resolvePostAcademicInformation(result: Common.Models.ServerResponseModel): void {
            if (result.success) {
                this.applicantAcademicInformation.lastUpdated = new Date();
            }
            this.genericResolver.genericPostResolver(this.typeOfData, result);
        }

        private getSuccessResult(dataFromServer: any) {
            if (dataFromServer == null) {
                var classRank = new Dtos.Applicant.Academic.ClassRankModel();
                this.applicantAcademicInformation = new Dtos.Applicant.Academic.AcademicInformationModel(classRank);
            } else {
                var classRank2 = new Dtos.Applicant.Academic.ClassRankModel(dataFromServer.classRank.classNumericalRank, dataFromServer.classRank.graduatingClassSize, dataFromServer.classRank.classPercentile, dataFromServer.classRank.lastUpdated);
                this.applicantAcademicInformation = new Dtos.Applicant.Academic.AcademicInformationModel(classRank2, dataFromServer.gpa, dataFromServer.careerChoice, dataFromServer.probableNextSchool, dataFromServer.lastUpdated);
            }
        }
    }
} 

module BohFoundation.Main {
    Register.Applicant.service("AcademicInformationServices", Applicant.AcademicInformation.Services.AcademicInformationServices);
}