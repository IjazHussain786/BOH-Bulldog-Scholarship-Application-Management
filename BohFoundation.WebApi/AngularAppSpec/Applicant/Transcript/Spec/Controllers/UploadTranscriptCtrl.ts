 module BohFoundation.Applicant.Transcript.Spec.Controllers {
     describe('UploadTranscriptCtrl', () => {
         var scope: Transcript.Controllers.IUploadTranscriptCtrlScope, userInformationService, transcriptService;
         var q, deferred, promise, resource;
         var authHeader = "auinasf";
         var usersGuid = "asdfjnkj";
         var graduatingYear = 3910;

         beforeEach(inject(($rootScope, $q) => {
             scope = $rootScope;
             q = $q;

             deferred = q.defer();
             promise = deferred.promise;
             resource = { $promise: promise };

             transcriptService = sinon.stub({
                 getWhenTranscriptWasLastUpdatedFromServer: () => { },
                 resolveGetLastUpdatedTranscript: () => { },
                 getLastUpdated: () => {}
             });

             userInformationService = sinon.stub({
                 getUserAuthHeader: () => { },
                 getUserGuid: () => { },
                 getGraduatingYear: () => { }
             });

             userInformationService.getUserAuthHeader.returns(authHeader);
             userInformationService.getUserGuid.returns(usersGuid);
             userInformationService.getGraduatingYear.returns(graduatingYear);

             transcriptService.getWhenTranscriptWasLastUpdatedFromServer.returns(resource);
             new Transcript.Controllers.UploadTranscriptCtrl(scope, userInformationService, transcriptService);
         }));

         describe('construction', () => {
             describe('get', () => {
                 it('should call getWhenTranscriptWasLastUpdatedFromServer', () => {
                     sinon.assert.calledOnce(transcriptService.getWhenTranscriptWasLastUpdatedFromServer);
                 });
             });
             describe('resolve', () => {
                 describe('error', () => {
                     beforeEach(() => {
                         deferred.reject();
                         scope.$apply();
                     });

                     it('should call resolve with a null false', () => {
                         sinon.assert.calledWith(transcriptService.resolveGetLastUpdatedTranscript, new Common.Models.ServerResponseModel(null, false));
                     });
                 });

                 describe('success', () => {
                     beforeEach(() => {
                         deferred.resolve(usersGuid);
                         scope.$apply();
                     });

                     it('should call resolve with the date returned from service.', () => {
                         sinon.assert.calledWith(transcriptService.resolveGetLastUpdatedTranscript, new Common.Models.ServerResponseModel(usersGuid, true));
                     });
                 });
             });
         });

         describe('getLastUpdated', () => {
             it('should return whatever the service returns.', () => {
                 transcriptService.getLastUpdated.returns(null);
                 expect(scope.getLastUpdated()).toBeNull();
             });
         });

         describe('dropzoneConfig', () => {
             var config;

             beforeEach(() => {
                 config = scope.dropzoneConfig.options;
             });

             it('should have an api endpoint.', () => {
                 expect(config.url).toBe('/api/applicant/transcript');
             });

             it('should have a file named with Guid_Transcript_Year.pdf', () => {
                 expect(config.paramName).toBe(usersGuid + "_transcript_" + graduatingYear + ".pdf");
             });

             it('should have a header with an userauthheader.', () => {
                 expect(config.headers.Authorization).toBe(authHeader);
             });

             it('should have a .pdf type of file.', () => {
                 expect(config.acceptedFiles).toBe(".pdf");
             });

             it('should have 1 maxFiles.', () => {
                 expect(config.maxFiles).toBe(1);
             });

             it('should have 3 as maxfilesize', () => {
                 expect(config.maxFilesize).toBe(3);
             });
         });
     });
 }