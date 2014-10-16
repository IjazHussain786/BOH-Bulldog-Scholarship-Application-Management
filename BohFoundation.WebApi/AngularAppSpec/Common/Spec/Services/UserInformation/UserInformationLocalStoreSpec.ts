module BohFoundation.Common.Spec.Services.UserInformation {
     describe('UserInformationLocalStore', () => {

         var localStorageService;
         var objectToSave: Models.UserInformationModel;
         var token = "token";
         var email = "email@ail.com";
         var firstName = "firstName";
         var lastName = "lastName";
         var userInformationLocalStore: Common.Services.UserInformation.IUserInformationLocalStore;
         var localStorageName = "BohFoundation-token-store";
         var rememberedOnDevice = true;
         var guid = 'guid';
         var date = new Date();

         beforeEach(()=> {
             localStorageService = sinon.stub({
                 set: () => { },
                 get: () => { },
                 remove: () => { }
             });

             objectToSave = new Models.UserInformationModel(token, email, firstName, lastName, rememberedOnDevice, guid, date, true, false, false, false);

             userInformationLocalStore = new Common.Services.UserInformation.UserInformationLocalStore(localStorageService);
         });

         describe('add', ()=> {
             it('should call set with the appropriate inputs.', ()=> {
                 expect(localStorageService.set.calledWithExactly(localStorageName, objectToSave)).toBeFalsy();

                 userInformationLocalStore.add(objectToSave);
                 
                 expect(localStorageService.set.calledWithExactly(localStorageName, objectToSave)).toBeTruthy();
             });
         });

         describe('get', ()=> {
             it('should return the object.', ()=> {
                 localStorageService.get.returns(objectToSave);
                 expect(localStorageService.get.calledWith(localStorageName)).toBeFalsy();

                 var result = userInformationLocalStore.get();

                 expect(result).toBe(objectToSave);
                 expect(localStorageService.get.calledWith(localStorageName)).toBeTruthy();
             });
         });

         describe('remove', ()=> {
             it('should call the remove function on the key.', ()=> {
                 expect(localStorageService.remove.calledWithExactly(localStorageName)).toBeFalsy();

                 userInformationLocalStore.remove();

                 expect(localStorageService.remove.calledWithExactly(localStorageName)).toBeTruthy();
             });
         });

     });
 }