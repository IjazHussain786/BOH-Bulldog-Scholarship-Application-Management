module BohFoundation.Common.Models {
    'use strict';
     export class UserInformationModel
     {
         constructor(public accessToken: string,
             public emailAddress: string,
             public firstName: string,
             public lastName: string,
             public rememberedOnDevice: boolean,
             public guid: string,
             public expireDate: Date,
             public applicant: boolean,
             public admin: boolean,
             public reference: boolean,
             public applicationEvaluator: boolean,
             public graduatingYear?: number) { }
     }
 }