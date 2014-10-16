 module BohFoundation.Applicant.AcademicInformation.Spec.Services {
     describe("AcademicInformationServices", () => {
         var typeOfData = "academic";
         var academicInformationApiEndpoint = "/api/applicant/academicinformation";
         var service;
         var authRepo, result, errorResponse;
         var successResponse, genericResolver;

         beforeEach(() => {
             errorResponse = new Common.Models.ServerResponseModel(null, false);
             successResponse = new Common.Models.ServerResponseModel(null, true);

             genericResolver = new TestHelpers.CommonStubs().createGenericResolver();
             
             authRepo = new TestHelpers.CommonStubs().createAuthRepo();
             
             service = new AcademicInformation.Services.AcademicInformationServices(authRepo, genericResolver);
         });

         describe('getApplicantAcademicInformationModel', () => {
             it('should return whatever is on applicantAcademicInformation.', () => {
                 service.applicantAcademicInformation = authRepo;
                 expect(service.getApplicantAcademicInformationModel()).toBe(authRepo);
             });
         });

         describe('getAcademicInformation', () => {
             describe('get', () => {
                 beforeEach(() => {
                     authRepo.get.returns(typeOfData);
                     result = service.getAcademicInformation();
                 });

                 it('should call authRepo with endpoint string.', () => {
                     sinon.assert.calledWith(authRepo.get, academicInformationApiEndpoint);
                 });

                 it('should return what authRepo get returns.', () => {
                     expect(result).toBe(typeOfData);
                 });
             });

             describe('resolve', () => {
                 describe('error', () => {
                     beforeEach(() => {
                         service.resolveGetAcademicInformation(errorResponse);
                     });

                     it('should call genericGetReolver.', () => {
                         sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, errorResponse);
                     });
                 });

                 describe('success', () => {
                     describe('first time all undefined', () => {
                         var serverResponseWithNullData;
                         beforeEach(() => {
                             genericResolver.genericGetResolver.returns(null);
                             serverResponseWithNullData = new Common.Models.ServerResponseModel({ data: null }, true);
                             service.resolveGetAcademicInformation(serverResponseWithNullData);
                         });

                         it('should call genericGetResolver.', () => {
                             sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, serverResponseWithNullData);
                         });

                         describe('classRankModel', () => {
                             it('should set classNumericalRank.', () => {
                                 expect(service.applicantAcademicInformation.classRank.classNumericalRank).toBeUndefined();
                             });

                             it('should set graduatingClassSize.', () => {
                                 expect(service.applicantAcademicInformation.classRank.graduatingClassSize).toBeUndefined();
                             });

                             it('should set classPercentile.', () => {
                                 expect(service.applicantAcademicInformation.classRank.classPercentile).toBeUndefined();
                             });

                             it('should set lastUpdated.', () => {
                                 expect(service.applicantAcademicInformation.classRank.lastUpdated).toBeUndefined();
                             });
                         });

                         describe('academicInformationModel', () => {
                             it('should be defined.', () => {
                                 expect(service.applicantAcademicInformation).toBeDefined();
                             });

                             it('should set gpa.', () => {
                                 expect(service.applicantAcademicInformation.gpa).toBeUndefined();
                             });

                             it('should set careerChoice.', () => {
                                 expect(service.applicantAcademicInformation.careerChoice).toBeUndefined();
                             });

                             it('should set probableNextSchool.', () => {
                                 expect(service.applicantAcademicInformation.probableNextSchool).toBeUndefined();
                             });

                             it('should set lastUpdated.', () => {
                                 expect(service.applicantAcademicInformation.lastUpdated).toBeUndefined();
                             });

                             it('should set classRank to defined.', () => {
                                 expect(service.applicantAcademicInformation.classRank).toBeDefined();
                             });
                         });
                     });
                     describe('update', () => {
                         var numericalRank = 10;
                         var graduatingClassSize = 10203;
                         var classPercentile = numericalRank / graduatingClassSize;
                         var lastUpdatedClassRank = new Date();

                         var lastUpdatedAcademic = new Date(1925, 5);
                         var gpa = 3.2;
                         var careerChoice = "Scientist";
                         var probableNextSchool = "WVU";
                         var serverResponseWithData;

                         beforeEach(() => {
                             var classRank = new Dtos.Applicant.Academic.ClassRankModel(numericalRank, graduatingClassSize, classPercentile, lastUpdatedClassRank);
                             var academicInfo = new Dtos.Applicant.Academic.AcademicInformationModel(classRank, gpa, careerChoice, probableNextSchool, lastUpdatedAcademic);
                             serverResponseWithData = new Common.Models.ServerResponseModel({ data: academicInfo }, true);
                             genericResolver.genericGetResolver.returns(academicInfo);
                             service.resolveGetAcademicInformation(serverResponseWithData);
                         });

                         it('should call genericGetResolver.', () => {
                             sinon.assert.calledWith(genericResolver.genericGetResolver, typeOfData, serverResponseWithData);
                         });

                         describe('classRankModel', () => {
                             it('should set classNumericalRank.', () => {
                                 expect(service.applicantAcademicInformation.classRank.classNumericalRank).toBe(numericalRank);
                             });

                             it('should set graduatingClassSize.', () => {
                                 expect(service.applicantAcademicInformation.classRank.graduatingClassSize).toBe(graduatingClassSize);
                             });

                             it('should set classPercentile.', () => {
                                 expect(service.applicantAcademicInformation.classRank.classPercentile).toBe(classPercentile);
                             });

                             it('should set lastUpdated.', () => {
                                 expect(service.applicantAcademicInformation.classRank.lastUpdated).toBe(lastUpdatedClassRank);
                             });
                         });

                         describe('academicInformationModel', () => {
                             it('should set gpa.', () => {
                                 expect(service.applicantAcademicInformation.gpa).toBe(gpa);
                             });

                             it('should set careerChoice.', () => {
                                 expect(service.applicantAcademicInformation.careerChoice).toBe(careerChoice);
                             });

                             it('should set probableNextSchool.', () => {
                                 expect(service.applicantAcademicInformation.probableNextSchool).toBe(probableNextSchool);
                             });

                             it('should set lastUpdated.', () => {
                                 expect(service.applicantAcademicInformation.lastUpdated).toBe(lastUpdatedAcademic);
                             });
                         });
                     });
                 });
             });
         });

         describe('postAcademicInformation', () => {
             describe('post', () => {
                 var model;

                 beforeEach(() => {
                     model = new Dtos.Applicant.Academic.AcademicInformationModel(new Dtos.Applicant.Academic.ClassRankModel());
                     authRepo.post.returns(academicInformationApiEndpoint);
                     result = service.postAcademicInformation(model);
                 });

                 it('should call authRepo post.', () => {
                     sinon.assert.calledWith(authRepo.post, model, academicInformationApiEndpoint);
                 });

                 it('should return what authRepo returns.', () => {
                     expect(result).toBe(academicInformationApiEndpoint);
                 });
             });

             describe('resolve', () => {
                 describe('failure', () => {
                     it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                         service.resolvePostAcademicInformation(errorResponse);
                         sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, errorResponse);
                     });
                 });

                 describe('success', () => {
                     beforeEach(() => {
                         service.applicantAcademicInformation = new Dtos.Applicant.Academic.AcademicInformationModel(new Dtos.Applicant.Academic.ClassRankModel);
                         service.resolvePostAcademicInformation(successResponse);
                     });

                     it('should pass whatever is passed in to the resolver with the correct information type to the generic resolver.', () => {
                         sinon.assert.calledWith(genericResolver.genericPostResolver, typeOfData, successResponse);
                     });

                     it('should set the lastUpdated to now.', () => {
                         var date = new Date();
                         expect(Math.abs(date.getMilliseconds() - service.applicantAcademicInformation.lastUpdated.getMilliseconds())).toBeLessThan(2);
                     });
                 });
             });
         });
     });
 }