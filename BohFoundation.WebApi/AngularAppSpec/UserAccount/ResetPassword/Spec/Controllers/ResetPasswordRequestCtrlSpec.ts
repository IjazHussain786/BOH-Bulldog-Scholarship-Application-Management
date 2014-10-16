 module BohFoundation.UserAccount.ResetPassword.Spec.Controllers {
    describe('ResetPasswordRequestCtrl', ()=> {

        var resetPasswordRepository;
        var scope: ResetPassword.Controllers.IResetPasswordRequestCtrlScope;
        var email = "email@email.com";
        var q: ng.IQService, deferred, promise: ng.IPromise<any>, resource, returnedErrorData;
        var errorMessage = "errorMessage";
        var alertHelpers;
        var resetPasswordThruEmailModel: Models.ResetPasswordThruEmailModel;
        var form;
        
        beforeEach(inject(($rootScope, $q)=> {
            scope = $rootScope;

            form = sinon.stub({});

            resetPasswordRepository = sinon.stub({
                requestPasswordResetKey:()=> {}
            });

            alertHelpers = sinon.stub({
                addDangerAlert: () => { },
                addSuccessAlert: () => {}  
            });

            q = $q;
            deferred = q.defer();
            promise = deferred.promise;
            resource = { $promise: promise };
            returnedErrorData = { data: { message: errorMessage } };

            resetPasswordRepository.requestPasswordResetKey.returns(resource);

            resetPasswordThruEmailModel = new Models.ResetPasswordThruEmailModel(email);

            new ResetPassword.Controllers.ResetPasswordRequestCtrl(scope, resetPasswordRepository, alertHelpers);
        }));

        describe('requestResetKey with valid form and not processing', () => {
            beforeEach(() => {
                form.$valid = true;
                scope.processing = false;
            });

            it('should call requestPasswordResetKey with whatever thing you feed into it.', ()=> {
                expect(resetPasswordRepository.requestPasswordResetKey.calledOnce).toBeFalsy();
                requestKey();
                expect(resetPasswordRepository.requestPasswordResetKey.calledOnce).toBeTruthy();
            });

            it('should change processing to true.', ()=> {
                expect(scope.processing).toBeFalsy();
                requestKey();
                expect(scope.processing).toBeTruthy();
            });

            it('should let the user know when there are errors.', ()=> {
                requestKey();
                deferred.reject(returnedErrorData);

                expect(alertHelpers.addDangerAlert.calledWith(errorMessage)).toBeFalsy();
                scope.$apply();
                expect(alertHelpers.addDangerAlert.calledWith(errorMessage)).toBeTruthy();
            });

            it('should let the user know when there is a success.', () => {
                requestKey();
                deferred.resolve();

                expect(alertHelpers.addSuccessAlert.calledWith('You will recieve a message in your inbox soon with your temporary key.')).toBeFalsy();
                scope.$apply();
                expect(alertHelpers.addSuccessAlert.calledWith('You will recieve a message in your inbox soon with your temporary key.')).toBeTruthy();
            });

            it('should flip the processing bool when there is a failure.', ()=> {
                requestKey();
                deferred.reject(returnedErrorData);
                expect(scope.processing).toBeTruthy();
                scope.$apply();
                expect(scope.processing).toBeFalsy();
            });

            it('should not flip the processing bool when there is a success.', ()=> {
                requestKey();
                deferred.resolve();
                expect(scope.processing).toBeTruthy();
                scope.$apply();
                expect(scope.processing).toBeTruthy();
            });
        });

        describe('requestResetKey', () => {
            describe('invalid form', () => {
                beforeEach(() => {
                    form.$valid = false;
                    requestKey();
                });

                it('should maintain a false processing flag.', () => {
                    expect(scope.processing).toBeFalsy();
                });

                it('should not call resetPasswordRepo.', () => {
                    sinon.assert.notCalled(resetPasswordRepository.requestPasswordResetKey);
                });
            });

            describe('processing is true', () => {
                beforeEach(() => {
                    scope.processing = true;
                    form.$valid = true;
                    requestKey();
                });

                it('should not call resetPasswordRepo.', () => {
                    sinon.assert.notCalled(resetPasswordRepository.requestPasswordResetKey);
                });
            });
        });

        function requestKey() {
            scope.requestResetKey(form);
        }

    });
} 