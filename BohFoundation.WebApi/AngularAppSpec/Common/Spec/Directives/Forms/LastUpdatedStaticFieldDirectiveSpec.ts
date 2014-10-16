module BohFoundation.Common.Spec.Directives {
    describe('lastUpdatedStaticField', () => {
        var scope;
        var el;
        var compile;

        beforeEach(module('BohFoundation.Common'));
        beforeEach(module('ng-Templates'));

        var date = new Date(2014,6,1);
        beforeEach(inject(($compile, $rootScope) => {
            scope = $rootScope;
            scope.lastUpdated = date;
            el = angular.element('<last-updated-static-field last-updated="lastUpdated" />');
            $compile(el)(scope);

            digest();

            compile = $compile;
        }));

        it('should contain the title passed in.', () => {
            expect(el.html()).toContain(' <p class="form-control-static ng-binding">Tuesday, July 1, 2014</p>');
        });

        it('should hide if date is undefined.', () => {
            scope.lastUpdated = undefined;
            el = angular.element('<last-updated-static-field last-updated="lastUpdated" />');
            compile(el)(scope);
            digest();
            expect(el.html()).toContain('<div class="form-group ng-hide"');
        });

        function digest() {
            scope.$digest();
        }
    });
}