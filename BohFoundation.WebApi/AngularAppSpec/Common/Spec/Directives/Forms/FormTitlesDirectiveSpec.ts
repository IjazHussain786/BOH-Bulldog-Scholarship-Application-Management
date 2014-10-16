module BohFoundation.Common.Spec.Directives {
    describe('FormTitleDirective', () => {
        var scope;
        var el;

        beforeEach(module('BohFoundation.Common'));
        beforeEach(module('ng-Templates'));

        beforeEach(inject(($compile, $rootScope) => {
            scope = $rootScope;
            el = angular.element('<form-title title="ZOE IS AMAZING" />');
            $compile(el)(scope);

            digest();
        }));

        it('should contain the title passed in.', () => {
            expect(el.html()).toContain('<h4 class="ng-binding">ZOE IS AMAZING</h4>');
        });

        function digest() {
            scope.$digest();
        }
    });
}