'use strict';
module BohFoundation.Person.ContactInformation.Services{

    export interface IContactInformationServices {

        getContactInformationModel(): Dtos.Person.ContactInformationModel;
        getContactInformation(): ng.resource.IResource<any>;
        resolveGetContactInformation(serverResponse: Common.Models.ServerResponseModel): void;

        postContactInformation(model: Dtos.Person.ContactInformationModel): ng.resource.IResource<any>;
        resolvePostContactInformation(serverResponse: Common.Models.ServerResponseModel): void;
    }

    export class ContactInformationServices implements IContactInformationServices{

        private contactInformationModel: Dtos.Person.ContactInformationModel;

        private contactInformationApiEndpoint = "/api/person/contactinformation";
        private typeOfInformation = "contact";

        static $inject = ['AuthRequiredRepository','UserInformationService', 'GenericResolver'];
        constructor(
            private authRequiredRepository: Common.Repositories.IAuthRequiredRepository,
            private userInformationService: Common.Services.UserInformation.IUserInformationService,
            private genericResolver: Common.Services.Resolvers.IGenericResolver) { }

        getContactInformationModel() {
            return this.contactInformationModel;
        }

        getContactInformation(): ng.resource.IResource<any> {
            return this.authRequiredRepository.get(this.contactInformationApiEndpoint);
        }

        resolveGetContactInformation(serverResponse: Common.Models.ServerResponseModel): void {
            var dataFromServer = this.genericResolver.genericGetResolver(this.typeOfInformation, serverResponse);
            if (serverResponse.success) {
                this.getSuccessResult(dataFromServer);
            } 
        }

        postContactInformation(model: Dtos.Person.ContactInformationModel): ng.resource.IResource<any> {
            return this.authRequiredRepository.post(model, this.contactInformationApiEndpoint);
        }

        resolvePostContactInformation(serverResponse: Common.Models.ServerResponseModel): void {
            if (serverResponse.success) {
                this.successfulPost();
            }
            this.genericResolver.genericPostResolver(this.typeOfInformation, serverResponse);
        }

        private getSuccessResult(data: any) {
            var dataFromServer : Dtos.Person.ContactInformationModel = data;
            if (dataFromServer == null) {
                this.createNewContactInformationModel();
            } else {
                this.contactInformationModel = dataFromServer;
            }
        }

        private createNewContactInformationModel() {
            var emailAddress = this.userInformationService.getUserEmail();
            var address = new Dtos.Person.AddressModel();
            this.contactInformationModel = new Dtos.Person.ContactInformationModel(address, emailAddress);
        }

        private successfulPost() {
            var now = new Date();
            this.contactInformationModel.lastUpdated = now;
            this.contactInformationModel.address.lastUpdated = now;
        }
    }
}

module BohFoundation.Main {
    Register.Person.service("ContactInformationServices", Person.ContactInformation.Services.ContactInformationServices);
}