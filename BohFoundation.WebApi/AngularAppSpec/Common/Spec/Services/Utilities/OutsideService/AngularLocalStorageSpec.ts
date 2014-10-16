module BohFoundation.Spec.TestOfAngularLocalStorage {
    describe("localStorage", () => {

        // Doc: http://gregpike.net/demos/angular-local-storage/demo/demo.html

        var key: string, user: string, defaultListIndex: number;

        beforeEach(module("BohFoundation.Common"));

        beforeEach(() => {
            key = "test";
            user = "user";
            defaultListIndex = 5;
        });


        it("should persist data to local storage.", (inject((localStorageService) => {

            expect(localStorageService.isSupported).toBeTruthy();

            var getOne = get();

            expect(getOne).toBeNull();

            var objectAtTimeOne = "one";

            add(objectAtTimeOne);

            var resultOfFirstQuery = get();
            expect(resultOfFirstQuery).toBe(objectAtTimeOne);

            var objectAtTimeTwo = "two";

            add(objectAtTimeTwo);

            var resultOfSecondQuery = get();
            expect(resultOfSecondQuery).toBe(objectAtTimeTwo);

            var remove = localStorageService.remove(key);
            expect(remove).toBeTruthy();

            var resultOfThirdQuery = get();

            expect(resultOfThirdQuery).toBeNull();


            function add(objectToAdd) {
                var result = localStorageService.add(key, objectToAdd);
                expect(result).toBeTruthy();
            }

            function get(): number{
                return localStorageService.get(key);
            }

        })));


    });
}  