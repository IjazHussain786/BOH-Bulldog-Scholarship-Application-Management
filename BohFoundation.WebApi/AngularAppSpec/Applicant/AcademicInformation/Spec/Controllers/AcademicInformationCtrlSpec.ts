 module BohFoundation.Applicant.AcademicInformation.Spec.Controller {
     describe('AcademicInformationCtrl', () => {
         var scope, academicInformationServices, q;
         var deferred, promise, resource;
         var deferred2, promise2, resource2;
         var data = { somestuff: 12390 };
         var academicInfo1 = { son: 10 };
         var academicInfo2 = { foalk: 100 };
         var modal;

         beforeEach(inject(($rootScope, $q) => {
             q = $q;
             scope = $rootScope;

             deferred = q.defer();
             promise = deferred.promise;
             resource = { $promise: promise };

             deferred2 = q.defer();
             promise2 = deferred2.promise;
             resource2 = { $promise: promise2 };

             modal = new TestHelpers.FakeModal().createModalStub();

             academicInformationServices = sinon.stub({
                 getApplicantAcademicInformationModel: () => {},
                 getAcademicInformation: () => { },
                 resolveGetAcademicInformation: () => { },
                 postAcademicInformation: () => { },
                 resolvePostAcademicInformation: () => { }
             });

             academicInformationServices.getAcademicInformation.returns(resource);
             academicInformationServices.postAcademicInformation.returns(resource2);
             academicInformationServices.getApplicantAcademicInformationModel.returns(academicInfo1);

             new AcademicInformation.Controllers.AcademicInformationCtrl(scope, academicInformationServices, modal);
         }));

         describe('construction', () => {
             it('should call getAcademicInformation.', () => {
                 sinon.assert.calledOnce(academicInformationServices.getAcademicInformation);
             });

             it('should set processing to true.', () => {
                 expect(scope.processing).toBeTruthy();
             });

             describe('failure', () => {
                 beforeEach(() => {
                     deferred.reject();
                     scope.$apply();
                 });

                 it('should call resolve with a false success.', () => {
                     sinon.assert.calledWith(academicInformationServices.resolveGetAcademicInformation, new Common.Models.ServerResponseModel(null, false));
                 });

                 it('should maintain a true processing.', () => {
                     expect(scope.processing).toBeTruthy();
                 });
             });

             describe('success', () => {
                 beforeEach(() => {
                     deferred.resolve(data);
                     scope.$apply();
                 });

                 it('should call resolve with a true success.', () => {
                     sinon.assert.calledWith(academicInformationServices.resolveGetAcademicInformation, new Common.Models.ServerResponseModel(data, true));
                 });

                 it('should flip the processing bit to false.', () => {
                     expect(scope.processing).toBeFalsy();
                 });
             });
         });

         describe('getAcademicInformationModel', () => {
             it('should return whatever is on the service.', () => {
                 expect(scope.getAcademicInformationModel()).toBe(academicInfo1);
             });
         });

         describe('successful construction', () => {
             beforeEach(() => {
                 deferred.resolve(data);
                 scope.$apply();
             });

             describe('editAcademicInformationModel', () => {
                 var fakeModal;

                 beforeEach(() => {
                     fakeModal = new TestHelpers.FakeModal().createFakeModel();

                     modal.open.returns(fakeModal);
                 });

                 describe('processing is true', () => {
                     beforeEach(() => {
                         scope.processing = true;
                         scope.editAcademicInformationModel();
                     });

                     it('should not call modal open.', () => {
                         sinon.assert.notCalled(modal.open);
                     });
                 });

                 describe('processing is false', () => {
                     beforeEach(() => {
                         scope.processing = false;
                         scope.editAcademicInformationModel();
                     });

                     it('should open the modal.', () => {
                         sinon.assert.calledOnce(modal.open);
                     });

                     it('should put whatever is in the service for a model on scope.', () => {
                         expect(scope.academicInformationModelForModal).toBe(academicInfo1);
                     });

                     describe('cancel modal', () => {
                         beforeEach(() => {
                             scope.modalInstance.dismiss();
                         });

                         it('should not call postContactInformation.', () => {
                             sinon.assert.notCalled(academicInformationServices.postAcademicInformation);
                         });

                         it('should not call getContactInfo again.', () => {
                             sinon.assert.calledOnce(academicInformationServices.getAcademicInformation);
                         });

                         it('should set successfullySaved to true.', () => {
                             expect(scope.successfullySaved).toBeFalsy();
                         });
                     });

                     describe('ok modal', () => {
                         beforeEach(() => {
                             scope.modalInstance.close(academicInfo2);
                         });

                         it('should set processing to true.', () => {
                             expect(scope.processing).toBeTruthy();
                         });

                         it('should call postContactInformation.', () => {
                             sinon.assert.calledWith(academicInformationServices.postAcademicInformation, academicInfo2);
                         });

                         describe('success', () => {
                             beforeEach(() => {
                                 deferred2.resolve();
                                 scope.$apply();
                             });

                             it('should call resolvePost.', () => {
                                 sinon.assert.calledWith(academicInformationServices.resolvePostAcademicInformation, new Common.Models.ServerResponseModel(null, true));
                             });

                             it('should call getContactInfo again.', () => {
                                 sinon.assert.calledTwice(academicInformationServices.getAcademicInformation);
                             });

                             it('should set successfullySaved to true.', () => {
                                 expect(scope.successfullySaved).toBeTruthy();
                             });
                         });

                         describe('failure', () => {
                             beforeEach(() => {
                                 deferred2.reject();
                                 scope.$apply();
                             });

                             it('should call resolvePost.', () => {
                                 sinon.assert.calledWith(academicInformationServices.resolvePostAcademicInformation, new Common.Models.ServerResponseModel(null, false));
                             });

                             it('should flip processing.', () => {
                                 expect(scope.processing).toBeFalsy();
                             });
                         });
                     });
                 });
             });
         });
     });
 }