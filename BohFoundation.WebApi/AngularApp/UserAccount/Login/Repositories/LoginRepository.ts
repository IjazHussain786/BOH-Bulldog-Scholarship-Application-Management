module BohFoundation.UserAccount.Login.Repositories {
    'use strict';
    export interface ILoginRepository {
        getToken(loginModel: Models.LoginModel): ng.resource.IResource<any>;
    }

    export class LoginRepository implements ILoginRepository {

        static $inject = ['$resource'];

        constructor(private $resource: ng.resource.IResourceService) { }

        getToken(loginModel: Models.LoginModel) {
            var request = this.urlEncode(loginModel);
            return this.resourceCall(request);
        }

        private urlEncode(loginModel: Models.LoginModel) {
            return 'grant_type=password&username=' + loginModel.email + '&password=' + loginModel.password;
        }

        private resourceCall(request: string) {
            return this.$resource('/token', {}, {
                save: {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }
            }).save(request);
        }
    }     
} 

module BohFoundation.Main {
    Register.UserManagement.service("LoginRepository", BohFoundation.UserAccount.Login.Repositories.LoginRepository);
}