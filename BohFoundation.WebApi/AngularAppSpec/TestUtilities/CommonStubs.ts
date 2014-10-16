 module TestHelpers {
     export interface ICommonStubs {
         createGenericResolver(): SinonStub;
         createAuthRepo(): SinonStub;
         getAlertHelperServiceStub(): SinonStub;
         getUserInformationServiceStub(): SinonStub;
         getAllowAnonymousRepositoryStub(): SinonStub;
         getModalStub():SinonStub;
     }

     export class CommonStubs implements ICommonStubs {
         createGenericResolver() {
             return sinon.stub({
                 genericPostResolver: () => { },
                 genericGetResolver: () => { }
             });
         }

         createAuthRepo() {
             return sinon.stub({
                 post: () => { },
                 get: () => { },
                 delete: () => { }
             });
         }

         create$Location() {
             return sinon.stub({
                 path: () => { }
             });
         }

         get$Window() {
             return sinon.stub({
                 open: () => {}
             });
         }

         getAlertHelperServiceStub() {
             return sinon.stub({
                 addSuccessAlert: () => { },
                 addDangerAlert: () => { },
                 addGenericInformationGetError: () => { },
                 addGenericInformationPostSuccess: () => { },
                 addGenericInformationPostError: () => { }
             });
         }

         getUserInformationServiceStub() {
             return sinon.stub({
                 getFirstName: () => { },
                 getLastName: () => { },
                 getGraduatingYear: () => { },
                 getFullName: () => { },
                 getUserEmail: () => { },
                 getLastnameFirst: () => { },
                 isApplicant: () => { },
                 isAdmin: () => { },
                 isReference: () => { },
                 isApplicationEvaluator: () => { },
                 getUserAuthHeader: () => { },
                 getUserGuid: () => { },
                 getRemembered: () => { },
                 getLoggedIn: () => { },
                 persistLogin: () => { },
                 logOut: () => { },
                 removeTokenFromLocalStorage: () => { }
             });
         }

         getAllowAnonymousRepositoryStub() {
             return sinon.stub({
                 post: () => { },
                 get: () => { }
             });
         }

         getModalStub() {
             return sinon.stub({
                 open: () => { }
             });
         }
     }
 }