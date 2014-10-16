 module BohFoundation.Applicant.PersonalInformation.Spec.Services {
     describe('PersonalInformationService', () => {
         var applicantPersonalInformationApiEndpoint = "/api/applicant/personalinformation";
         var service: PersonalInformation.Services.IPersonalInformationService;
         var authRepo, genericResolver, result, errorResponse, userInformationService;
         var fullname = "firstname";
         var birthday = new Date(1984, 7, 6);
         var lastUpdated = new Date(2014, 6, 28, 7, 6, 5, 2);
         var graduatingYear = 1345;
         var typeOfInformation = "personal";
         var successResponse;

         beforeEach(() => {
             errorResponse = new Common.Models.ServerResponseModel(null, false);
             successResponse = new Common.Models.ServerResponseModel(null, true);

             var commonStubs: TestHelpers.ICommonStubs = new TestHelpers.CommonStubs();

             authRepo = commonStubs.createAuthRepo();

             genericResolver = commonStubs.createGenericResolver();

             userInformationService = commonStubs.getUserInformationServiceStub();
             userInformationService.getFullName.returns(fullname);

             service = new PersonalInformation.Services.PersonalInformationService(authRepo, userInformationService, genericResolver);
         });


         describe('construction', () => {
             it('should call userInformationService for fullName.', () => {
                 sinon.assert.calledOnce(userInformationService.getFullName);
             });

             it('should set userInformationService for fullName.', () => {
                 expect(service.fullName).toBe(fullname);
             });
         });

         describe('getPersonalInformation', () => {
             describe('get', () => {
                 beforeEach(() => {
                     authRepo.get.returns(applicantPersonalInformationApiEndpoint);
                     result = service.getPersonalInformation();
                 });

                 it('should call authRepo with endpoint string.', () => {
                     sinon.assert.calledWith(authRepo.get, applicantPersonalInformationApiEndpoint);
                 });

                 it('should return what authRepo get returns.', () => {
                     expect(result).toBe(applicantPersonalInformationApiEndpoint);
                 });
             });

             describe('resolve', () => {
                 describe('error', () => {
                     beforeEach(() => {
                         service.resolveGetPersonalInformation(errorResponse);
                     });

                     it('should call genericGetRepo.', () => {
                         sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfInformation, errorResponse);
                     });

                     it('should not call getGraduatingYear().', () => {
                         sinon.assert.notCalled(userInformationService.getGraduatingYear);
                     });
                 });

                 describe('success', () => {
                     var serverResponseModel;

                     describe('first time', () => {
                         beforeEach(() => {
                             userInformationService.getGraduatingYear.returns(graduatingYear);
                             genericResolver.genericGetResolver.returns(null);
                             service.resolveGetPersonalInformation(successResponse);
                         });

                         it('should call genericGetRepo.', () => {
                             sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfInformation, successResponse);
                         });

                         it('should set applicantPersonalInformation with the users graduatingYear.', () => {
                             expect(service.applicantPersonalInformation.graduatingYear).toBe(graduatingYear);
                         });

                         it('should set applicantPersonalInformation birthdate to undefined.', () => {
                             expect(service.applicantPersonalInformation.birthdate).toBeUndefined();
                         });

                         it('should set applicantPersonalInformation lastUpdated to undefined.', () => {
                             expect(service.applicantPersonalInformation.lastUpdated).toBeUndefined();
                         });

                         it('should call getGraduatingYear.', () => {
                             sinon.assert.calledOnce(userInformationService.getGraduatingYear);
                         });
                     });

                     describe('update', () => {
                         var data = {
                             graduatingYear: graduatingYear,
                             birthdate: birthday,
                             lastUpdated: lastUpdated
                         }
                         
                         beforeEach(() => {
                             genericResolver.genericGetResolver.returns(data);
                             service.resolveGetPersonalInformation(successResponse);
                         });

                         it('should call genericGetRepo.', () => {
                             sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfInformation, successResponse);
                         });

                         it('should set the applicantPersonalInformations graduatingYear.', () => {
                             expect(service.applicantPersonalInformation.graduatingYear).toBe(graduatingYear);
                         });

                         it('should set the applicantPersonalInformations birthdate.', () => {
                             expect(service.applicantPersonalInformation.birthdate).toBe(birthday);
                         });

                         it('should set the applicantPersonalInformations lastUpdated.', () => {
                             expect(service.applicantPersonalInformation.lastUpdated).toBe(lastUpdated);
                         });

                         it('should call getGraduatingYear.', () => {
                             sinon.assert.notCalled(userInformationService.getGraduatingYear);
                         });
                     });
                 });
             });
         });
             

         describe('postPersonalInformation', () => {
             describe('post', () => {
                 var personalInformationModel;

                 beforeEach(() => {
                     authRepo.post.returns(applicantPersonalInformationApiEndpoint);
                     personalInformationModel = new Dtos.Applicant.PersonalInformation.ApplicantPersonalInformationModel(graduatingYear, birthday, lastUpdated);
                     result = service.postPersonalInformation(personalInformationModel);
                 });

                 it('should call authRepo post.', () => {
                     sinon.assert.calledWith(authRepo.post, personalInformationModel, applicantPersonalInformationApiEndpoint);
                 });

                 it('should call return what post returns.', () => {
                     expect(result).toBe(applicantPersonalInformationApiEndpoint);
                 });
             });

             describe('resolve', () => {
                 describe('failure', () => {
                     it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                         service.resolvePostPersonalInformation(errorResponse);
                         sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfInformation, errorResponse);
                     });
                 });
                 
                 describe('success', () => {
                     var successFromServer = new Common.Models.ServerResponseModel(null, true);
                     beforeEach(() => {
                         service.applicantPersonalInformation = new Dtos.Applicant.PersonalInformation.ApplicantPersonalInformationModel(graduatingYear);
                         service.resolvePostPersonalInformation(successFromServer); 
                     });

                     it('should set the lastUpdated to now.', () => {
                         var date = new Date();
                         expect(Math.abs(date.getMilliseconds() - service.applicantPersonalInformation.lastUpdated.getMilliseconds())).toBeLessThan(2);
                     });

                     it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                         sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfInformation, successFromServer);
                     });
                 });
             });
         });
     });
 }