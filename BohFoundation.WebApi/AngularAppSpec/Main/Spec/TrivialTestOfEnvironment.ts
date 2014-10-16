 module BohFoundation.Spec.TrivialTestOfEnvironment {
     
     describe("Basic jasmine test", () => {

         it("should add two numbers", () => {

             expect(1 + 2).toBe(3);
         });
     });

     describe("Sinon spies", () => {
         it("should use a build-in assert", () => {
             var spy = sinon.spy();
             expect(spy.called).toBe(false);
         });

         it("should use a sinon assert", () => { 
             var spy = sinon.spy();
             sinon.assert.notCalled(spy);
         });
     });

 }