module BohFoundation.UserAccount.ResetPassword.Repositories {
    'use strict';
    export interface IResetPasswordRepository {
        requestPasswordResetKey(resetPasswordThruEmailModel: Models.ResetPasswordThruEmailModel): ng.resource.IResource<any>;
        changePasswordFromResetKey(changePasswordFromResetKeyModel: Models.ChangePasswordFromResetKeyModel): ng.resource.IResource<any>;
    }

    export class ResetPasswordRepository implements IResetPasswordRepository {
        static $inject = ['$resource'];
        constructor(public $resource: ng.resource.IResourceService) { }

        requestPasswordResetKey(resetPasswordThruEmailModel: Models.ResetPasswordThruEmailModel) {
            return this.$resource('/api/useraccount/changepassword/request').save(resetPasswordThruEmailModel);
        }

        changePasswordFromResetKey(changePasswordFromResetKeyModel: Models.ChangePasswordFromResetKeyModel) {
            return this.$resource("/api/useraccount/changepassword/withkey").save(changePasswordFromResetKeyModel);
        }
    } 
} 

module BohFoundation.Main {
    Register.UserManagement.service("ResetPasswordRepository", UserAccount.ResetPassword.Repositories.ResetPasswordRepository);
}