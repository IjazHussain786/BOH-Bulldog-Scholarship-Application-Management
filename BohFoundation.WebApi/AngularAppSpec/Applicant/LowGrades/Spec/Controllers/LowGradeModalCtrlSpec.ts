 module BohFoundation.Applicant.LowGrades.Spec.Controllers {
     describe('LowGradeModalCtrl', () => {
         var modalInstance, scope: LowGrades.Controllers.ILowGradeModalCtrlScope, lowGradeModel;
         var grade = "A";
         var classTitle = "class";
         var gradeLevel = Common.Enums.YearOfHighSchool.Junior;
         var explanation = "exkjnasdfjk";
         var service;
         var form;

         beforeEach(inject(($rootScope) => {
             modalInstance = sinon.stub({
                 close: () => { },
                 dismiss: () => {}
             });

             form = sinon.stub({
                 $valid: false
             });

             lowGradeModel = new Dtos.Applicant.Academic.LowGradeModel(grade, classTitle, true, gradeLevel, explanation);

             scope = $rootScope;

             service = new Applicant.LowGrades.Controllers.LowGradeModalCtrl(scope, modalInstance, lowGradeModel);
         }));

         describe("ok", () => {
             describe('invalid form', () => {
                 it('should not call close.', () => {
                     scope.ok(form);
                     sinon.assert.notCalled(modalInstance.close);
                 });
             });
             describe('valid form', () => {
                 it('should call close with object.', () => {
                     form.$valid = true;
                     scope.ok(form);
                     sinon.assert.calledWith(modalInstance.close, lowGradeModel);
                 });
             });
         });

         describe('cancel', () => {
             it('should call dismiss.', () => {
                 scope.cancel();
                 sinon.assert.calledOnce(modalInstance.dismiss);
             });
         });

         describe("defaulting option boxes", () => {
             describe('fully formed object', () => {
                 it('should maintain grade.', () => {
                     expect(scope.lowGradeModel.grade).toBe(grade);
                 });

                 it('should maintain year of hs.', () => {
                     expect(scope.lowGradeModel.yearOfHighSchool).toBe(gradeLevel);
                 });
             });

             describe('raw object', () => {
                 beforeEach(() => {
                     lowGradeModel = new Dtos.Applicant.Academic.LowGradeModel();
                     service = new Applicant.LowGrades.Controllers.LowGradeModalCtrl(scope, modalInstance, lowGradeModel);
                 });

                 it('should set grade.', () => {
                     expect(scope.lowGradeModel.grade).toBe("A"); 
                 });

                 it('should set year of hs.', () => {
                     expect(scope.lowGradeModel.yearOfHighSchool).toBe(Common.Enums.YearOfHighSchool.Freshman);
                 });
             });
         });
     });
 }