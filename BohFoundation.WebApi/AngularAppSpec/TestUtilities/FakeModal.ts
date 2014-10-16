module TestHelpers {
    export interface IFakeModal {
        createFakeModel(): any;
        createModalStub(): SinonStub;
        createModalInstanceStub(): SinonStub;
    }

     export class FakeModal implements IFakeModal{
         createFakeModel() {
             return {
                 result: {
                     then: function(confirmCallback, cancelCallback) {
                         //Store the callbacks for later when the user clicks on the OK or Cancel button of the dialog
                         this.confirmCallBack = confirmCallback;
                         this.cancelCallback = cancelCallback;
                     }
                 },
                 close: function(item) {
                     //The user clicked OK on the modal dialog, call the stored confirm callback with the selected item
                     this.result.confirmCallBack(item);
                 },
                 dismiss: function(type) {
                     //The user clicked cancel on the modal dialog, call the stored cancel callback
                     this.result.cancelCallback(type);
                 }
             };
         }

        createModalStub() {
            return sinon.stub({
                open: () => { }
            });
        }

        createModalInstanceStub() {
            return sinon.stub({
                    close: () => { },
                    dismiss: () => { }
                });
        }
     }
 }