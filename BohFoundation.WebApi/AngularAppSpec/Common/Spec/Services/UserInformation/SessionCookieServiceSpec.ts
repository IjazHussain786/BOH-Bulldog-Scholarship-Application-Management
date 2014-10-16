 module BohFoundation.Common.Spec.Services.UserInformation {
     describe('sessionCookieService', () => {

         var userInformationToken: Models.UserInformationModel;
         var accessToken = "asdfjkna983nasjf0";
         var email = "email@email.com";
         var firstName = "firstName";
         var lastName = "lastName";
         var userInformationTokenString: string;
         var sessionCookieMagicString = "93511211-9DB9-4109-B555-320DC375D1B6";
         var sessionCookieService: Common.Services.UserInformation.ISessionCookieService;
         var rememberedOnDevice = true;
         var guid = "guid";
         var date = new Date();

         beforeEach(() => {
             userInformationToken = new Common.Models.UserInformationModel(accessToken, email, firstName, lastName, rememberedOnDevice, guid, date, true, false, false, false);
             userInformationTokenString = angular.toJson(userInformationToken);
             sessionCookieService = new Common.Services.UserInformation.SessionCookieService;
         });

         afterEach(()=> {
             document.cookie = sessionCookieMagicString + "=";
         });
         describe('getUserInformationFromCookie', ()=> {
             it('should try to get a cookie, but it not be there and return null.', () => {
                 var result = sessionCookieService.getUserInformationFromCookie();
                 expect(result).toBeNull();
             });

             it('should retrieve the cookie and return a userInformationModel.', ()=> {
                 document.cookie = sessionCookieMagicString + "=" + userInformationTokenString;

                 var result = sessionCookieService.getUserInformationFromCookie();
                 expect(result.accessToken).toBe(accessToken);
                 expect(result.firstName).toBe(firstName);
                 expect(result.lastName).toBe(lastName);
                 expect(result.emailAddress).toBe(email);
             });
         });

         describe('setCookie', ()=> {
             it('should set the cookie.', () => {
                 var expectNull = sessionCookieService.getUserInformationFromCookie();
                 expect(expectNull).toBeNull();

                 sessionCookieService.setCookie(userInformationToken);

                 var result = sessionCookieService.getUserInformationFromCookie();
                 expect(result.accessToken).toBe(accessToken);
                 expect(result.firstName).toBe(firstName);
                 expect(result.lastName).toBe(lastName);
                 expect(result.emailAddress).toBe(email);
             });
         });

         describe('deleteSessionCookie', ()=> {
             it('should delete the cookie.', ()=> {
                 var expectNull = sessionCookieService.getUserInformationFromCookie();
                 expect(expectNull).toBeNull();

                 sessionCookieService.setCookie(userInformationToken);

                 var result = sessionCookieService.getUserInformationFromCookie();
                 expect(result.accessToken).toBe(accessToken);
                 expect(result.firstName).toBe(firstName);
                 expect(result.lastName).toBe(lastName);
                 expect(result.emailAddress).toBe(email);

                 sessionCookieService.deleteSessionCookie();

                 var finalResult = sessionCookieService.getUserInformationFromCookie();
                 expect(finalResult).toBeNull();
             });
         });
     });
 }