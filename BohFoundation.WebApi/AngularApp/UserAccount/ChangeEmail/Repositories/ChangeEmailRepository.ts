module BohFoundation.UserAccount.ChangeEmail.Repositories {
    'use strict';
    export interface IChangeEmailRepository {
        cancelConfirmation(string): ng.resource.IResource<any>;
        confirmEmail(verificationModel: Models.VerifyEmailModel): ng.resource.IResource<any>;
    }

    export class ChangeEmailRepository implements IChangeEmailRepository {

        static $inject = ['$resource'];

        constructor(private $resource: ng.resource.IResourceService) { }

        cancelConfirmation(verificationKey: string) {
            return this.$resource('/api/useraccount/changeemail/cancel/:verificationKey', { verificationKey: verificationKey }).save();
        }

        confirmEmail(verificationModel: Models.VerifyEmailModel) {
            return this.$resource('/api/useraccount/changeemail/confirm').save(verificationModel);
        }

    }

}

module BohFoundation.Main {
    Register.UserManagement.service("ChangeEmailRepository", UserAccount.ChangeEmail.Repositories.ChangeEmailRepository);
}