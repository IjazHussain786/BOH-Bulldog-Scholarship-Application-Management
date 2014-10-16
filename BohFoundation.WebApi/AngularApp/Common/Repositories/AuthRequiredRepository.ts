module BohFoundation.Common.Repositories {
    'use strict';
    export interface IAuthRequiredRepository {
        post(data: any, path: string): ng.resource.IResource<any>;
        get(path: string): ng.resource.IResource<any>;
        delete(path:string): ng.resource.IResource<any>;
    }

    export class AuthRequiredRepository implements IAuthRequiredRepository {
        static $inject = ['$resource', 'UserInformationService'];
        constructor(private $resource: ng.resource.IResourceService, private userInformationService: Services.UserInformation.IUserInformationService) { }

        post(data: any, path: string) {
            return this.$resource(path, {}, {
                save: {
                    method: 'POST',
                    headers: {
                        'Authorization': this.userInformationService.getUserAuthHeader()
                    }
                }
            }).save(data);
        }

        get(path: string) {
            return this.$resource(path, {}, {
                get: {
                    method: 'GET',
                    headers: {
                        'Authorization': this.userInformationService.getUserAuthHeader()
                    }
                }
            }).get();
        }

        delete(path: string): ng.resource.IResource<any> {
            return this.$resource(path, {}, {
                delete: {
                    method: 'DELETE', 
                    headers: {
                        'Authorization': this.userInformationService.getUserAuthHeader()
                    }
                }
            }).delete();
        }
    }
}

module BohFoundation.Main {
    Register.Common.service("AuthRequiredRepository", Common.Repositories.AuthRequiredRepository);
}