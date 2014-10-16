 module BohFoundation.Person.ContactInformation.Spec.Controllers {
     describe('ContactInformationInputCtrl', () => {
         var scope, contactInformationServices, q, modal, fakeModal;
         var deferred, promise, resource;
         var deferred2, promise2, resource2;
         var data = { somestuff: 12390 };
         var contactInfo1 = { son: 10 };
         var contactInfo2 = { foalk: 100 };

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

             contactInformationServices = sinon.stub({
                 getContactInformationModel: () => {},
                 getContactInformation: () => { },
                 resolveGetContactInformation: () => { },
                 postContactInformation: () => { },
                 resolvePostContactInformation: () => { }
             });

             contactInformationServices.getContactInformation.returns(resource);
             contactInformationServices.postContactInformation.returns(resource2);
             contactInformationServices.getContactInformationModel.returns(contactInfo1);

             new ContactInformation.Controllers.ContactInformationInputCtrl(scope, contactInformationServices, modal);
         }));

         describe('construction', () => {
             it('should call getContactInformation.', () => {
                 sinon.assert.calledOnce(contactInformationServices.getContactInformation);
             });

             it('should set processing as true.', () => {
                 expect(scope.processing).toBeTruthy();
             });

             describe('failure', () => {
                 beforeEach(() => {
                     deferred.reject();
                     scope.$apply();
                 });

                 it('should call resolve with a false success.', () => {
                     sinon.assert.calledWith(contactInformationServices.resolveGetContactInformation, new Common.Models.ServerResponseModel(null, false));
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
                     sinon.assert.calledWith(contactInformationServices.resolveGetContactInformation, new Common.Models.ServerResponseModel(data, true));
                 });

                 it('should flip the processing bit to false.', () => {
                     expect(scope.processing).toBeFalsy();
                 });
             });
         });

         describe('getContactInformation', () => {
             it('should return whatever the service returns.', () => {
                 expect(scope.getContactInformationModel()).toBe(contactInfo1);
             });
         });

         describe('editContactInformation', () => {
             beforeEach(() => {
                 deferred.resolve(data);
                 scope.$apply();
             });

             beforeEach(() => {
                 fakeModal = new TestHelpers.FakeModal().createFakeModel();

                 modal.open.returns(fakeModal);
             });

             describe('processing is true', () => {
                 beforeEach(() => {
                     scope.processing = true;
                     scope.editContactInformation();
                 });

                 it('should not call modal open.', () => {
                     sinon.assert.notCalled(modal.open);
                 });
             });

             describe('processing is false', () => {
                 beforeEach(() => {
                     scope.editContactInformation();
                 });

                 it('should open the modal.', () => {
                     sinon.assert.calledOnce(modal.open);
                 });

                 it('should put whatever is in the service for a model on scope.', () => {
                     expect(scope.contactInformationModelForModal).toBe(contactInfo1);
                 });

                 describe('ok modal', () => {
                     beforeEach(() => {
                         scope.modalInstance.close(contactInfo2);
                     });

                     describe('postContactInfo', () => {
                         describe('post', () => {
                             it('should call postContactInformation with model from modal.', () => {
                                 sinon.assert.calledWith(contactInformationServices.postContactInformation, contactInfo2);
                             });

                             it('should flip processing to true.', () => {
                                 expect(scope.processing).toBeTruthy();
                             });
                         });

                         describe('resolve', () => {
                             describe('happy path', () => {
                                 beforeEach(() => {
                                     deferred2.resolve();
                                     scope.$apply();
                                 });

                                 it('should call resolvePost.', () => {
                                     sinon.assert.calledWith(contactInformationServices.resolvePostContactInformation, new Common.Models.ServerResponseModel(null, true));
                                 });

                                 it('should call getContactInfo again.', () => {
                                     sinon.assert.calledTwice(contactInformationServices.getContactInformation);
                                 });

                                 it('should set successfullySave to true.', () => {
                                     expect(scope.successfullySaved).toBeTruthy();
                                 });
                             });

                             describe('failure', () => {
                                 beforeEach(() => {
                                     deferred2.reject();
                                     scope.$apply();
                                 });

                                 it('should call resolvePost.', () => {
                                     sinon.assert.calledWith(contactInformationServices.resolvePostContactInformation, new Common.Models.ServerResponseModel(null, false));
                                 });

                                 it('should flip processing.', () => {
                                     expect(scope.processing).toBeFalsy();
                                 });

                                 it('should not call getContactInfo again.', () => {
                                     sinon.assert.calledOnce(contactInformationServices.getContactInformation);
                                 });
                             });
                         });
                     });
                 });

                 describe('cancel operation', () => {
                     beforeEach(() => {
                         scope.modalInstance.dismiss();
                     });

                     it('should not call postContactInformation.', () => {
                         sinon.assert.notCalled(contactInformationServices.postContactInformation);
                     });
                     
                     it('should not call getContactInfo again.', () => {
                         sinon.assert.calledOnce(contactInformationServices.getContactInformation);
                     });
                 });
             });
         });
     });
 }