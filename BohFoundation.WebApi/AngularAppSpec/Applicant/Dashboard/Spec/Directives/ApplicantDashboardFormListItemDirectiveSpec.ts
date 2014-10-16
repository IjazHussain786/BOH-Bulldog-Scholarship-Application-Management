 module BohFoundation.Applicant.Dashboard.Spec.Directives {
     describe('ApplicantDashboardListItemDirective', () => {
         var scope;
         var el;
         var title = "Care of Zoe";
         var link = "/link/link/link";
         
         beforeEach(module('BohFoundation.Applicant'));
         beforeEach(module('ng-Templates'));

         describe('new', () => {
             beforeEach(inject(($compile, $rootScope) => {
                 scope = $rootScope;
                 scope.newItem = new Models.ApplicantDashboardListItemInputModel(title, link, Common.Enums.StyleEnum.Warning);
                 el = angular.element('<applicant-dashboard-list-item dashboard-item="newItem" />');
                 $compile(el)(scope);

                 digest();
             }));

             it('should contain the title.', () => {
                 expect(el.html()).toContain('<h4 class="ng-binding">Care of Zoe');
             });

             it('should hide the lastUpdated tag.', () => {
                 expect(el.html()).toContain('class="ng-binding ng-hide"');
             });

             it('should contain the link.', () => {
                 expect(el.html()).toContain('href="' + link + '"');
             });

             it('should have a class of btn btn-warning.', () => {
                 expect(el.html()).toContain('class="btn btn-warning"');
             });

             it('should show not completed.', () => {
                 expect(el.html()).toContain('class=""');
             });

             function digest() {
                 scope.$digest();
             }
         });

         describe('update', () => {
             var date;

             beforeEach(inject(($compile, $rootScope) => {
                 date = new Date(2019, 0, 1);

                 scope = $rootScope;
                 scope.newItem2 = new Models.ApplicantDashboardListItemInputModel(title, link, Common.Enums.StyleEnum.Success, date);
                 el = angular.element('<applicant-dashboard-list-item dashboard-item="newItem2" />');
                 $compile(el)(scope);

                 digest();
             }));

             it('should contain the title.', () => {
                 expect(el.html()).toContain('<h4 class="ng-binding">Care of Zoe');
             });

             it('should hide the lastUpdated tag.', () => {
                 expect(el.html()).toNotContain('class="ng-binding ng-hide"');
             });

             it('should contain the link.', () => {
                 expect(el.html()).toContain('href="' + link + '"');
             });

             it('should have a class of btn btn-warning.', () => {
                 expect(el.html()).toContain('class="btn btn-success"');
             });

             it('should contain the date, pretty.', () => {
                 expect(el.html()).toContain('Last Updated: January 1, 2019');
             });

             it('should not show not completed.', () => {
                 expect(el.html()).toContain('class="ng-hide"');
             });

             function digest() {
                 scope.$digest();
             }
         });


         describe('info', () => {
             var date;

             beforeEach(inject(($compile, $rootScope) => {
                 date = new Date(2019, 0, 1);

                 scope = $rootScope;
                 scope.newItem2 = new Models.ApplicantDashboardListItemInputModel(title, link, Common.Enums.StyleEnum.Info, date);
                 el = angular.element('<applicant-dashboard-list-item dashboard-item="newItem2" />');
                 $compile(el)(scope);

                 digest();
             }));

             it('should contain the title.', () => {
                 expect(el.html()).toContain('<h4 class="ng-binding">Care of Zoe');
             });

             it('should hide the lastUpdated tag.', () => {
                 expect(el.html()).toNotContain('class="ng-binding ng-hide"');
             });

             it('should contain the link.', () => {
                 expect(el.html()).toContain('href="' + link + '"');
             });

             it('should have a class of btn btn-warning.', () => {
                 expect(el.html()).toContain('class="btn btn-info"');
             });

             it('should contain the date, pretty.', () => {
                 expect(el.html()).toContain('Last Updated: January 1, 2019');
             });

             it('should not show not completed.', () => {
                 expect(el.html()).toContain('class="ng-hide"');
             });

             function digest() {
                 scope.$digest();
             }
         });


     });
 }