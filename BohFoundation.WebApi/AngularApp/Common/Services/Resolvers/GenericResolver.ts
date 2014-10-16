module BohFoundation.Common.Services.Resolvers {
    export interface IGenericResolver {
        genericPostResolver(typeOfData: string, serverResponse: Models.ServerResponseModel): void;
        genericGetResolver(typeOfData: string, serverResponse: Models.ServerResponseModel): any;
    }

    export class GenericResolver implements IGenericResolver {
        
        static $inject = ['AlertHelperService'];

        constructor(
            private alertHelperService: Services.UiServices.IAlertHelperServices) {}

        genericPostResolver(typeOfData: string, serverResponse: Models.ServerResponseModel) {
            if (serverResponse.success) {
                this.alertHelperService.addGenericInformationPostSuccess();
            } else {
                this.alertHelperService.addGenericInformationPostError(typeOfData);
            }
        }

        genericGetResolver(typeOfData: string, serverResponse: Models.ServerResponseModel): any {
            if (serverResponse.success) {
                return serverResponse.dataFromServer.data;
            }
            this.alertHelperService.addGenericInformationGetError(typeOfData);
            return null;
        }
    }
} 

module BohFoundation.Main {
    Register.Common.service("GenericResolver", Common.Services.Resolvers.GenericResolver);
}