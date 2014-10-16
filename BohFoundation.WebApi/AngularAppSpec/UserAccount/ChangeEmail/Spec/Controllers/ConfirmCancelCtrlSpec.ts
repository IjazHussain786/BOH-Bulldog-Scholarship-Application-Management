module BohFoundation.UserAccount.ChangeEmail.Spec.Controllers {
    describe('ConfirmCancelCtrl', () => {

        var $routeParams, scope: ChangeEmail.Controllers.IConfirmCancelCtrlScope;
        var ctrl: ChangeEmail.Controllers.IConfirmCancelCtrl;
        var mockRepo;
        var confirmationCode = 'confimationCode-asdfafweva32';
        var q: ng.IQService;
        var deferred, resource;
        var promise: ng.IPromise<any>;
        var errorMessage = "Drat! There was an error canceling email verification.";
        var successMessage = "Great! You have successfully cancelled the email verification.";
        var returnedErrorData;
        var alertHelpers;

        beforeEach(inject(($rootScope, $q) => {
            scope = $rootScope;

            $routeParams = sinon.stub({
                confirmationCode: confirmationCode
            });

            mockRepo = sinon.stub({
                cancelConfirmation: () => { }
            });

            alertHelpers = sinon.stub({
                alerts: [],
                addWarningAlert: () => { },
                addSuccessAlert: () => { },
                addDangerAlert: () => { },
                removeAlert: () => { }
            });

            q = $q;
            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };
            returnedErrorData = { data: { message: errorMessage } };

            mockRepo.cancelConfirmation.returns(resource);

            ctrl = new ChangeEmail.Controllers.ConfirmCancelCtrl($routeParams, scope, mockRepo, alertHelpers);
        }));

        it('should take call the cancelConfirmation method with the confimationCode.', () => {
            expect(mockRepo.cancelConfirmation.calledWithExactly(confirmationCode)).toBeTruthy();
        });

        it('should create an error alert when there is an error.', () => {
            deferred.reject();

            expect(alertHelpers.addDangerAlert.calledWith(errorMessage)).toBeFalsy();

            scope.$apply();

            expect(alertHelpers.addDangerAlert.calledWith(errorMessage)).toBeTruthy();
        });

        it('should create a success alert when all goes smoothly.', () => {
            deferred.resolve();

            expect(alertHelpers.addSuccessAlert.calledWith(successMessage)).toBeFalsy();

            scope.$apply();

            expect(alertHelpers.addSuccessAlert.calledWith(successMessage)).toBeTruthy();
        });

    });
} 