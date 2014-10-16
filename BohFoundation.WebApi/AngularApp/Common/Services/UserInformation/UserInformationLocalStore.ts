module BohFoundation.Common.Services.UserInformation {
    'use strict';
     export interface ILocalStorageService {
         set(key: string, value: any): boolean;
         get(key: string): any;
         remove(key: string): boolean;
     }

     export interface IUserInformationLocalStore {
         add(objectToSave: Models.UserInformationModel): boolean;
         get(): Models.UserInformationModel;
         remove(): boolean;        
     }

     export class UserInformationLocalStore implements IUserInformationLocalStore{

         private key = "BohFoundation-token-store";

         static $inject = ['localStorageService'];
         constructor(private localStorageService: ILocalStorageService) { } 

         add(objectToSave: Models.UserInformationModel) {
             return this.localStorageService.set(this.key,objectToSave);
         }

         get() : Models.UserInformationModel{
             return this.localStorageService.get(this.key);
         }

         remove() {
             return this.localStorageService.remove(this.key);
         }
     }
 }

module BohFoundation.Main {
    Register.Common.service('UserInformationLocalStore', Common.Services.UserInformation.UserInformationLocalStore);
}