module BohFoundation.Main {
    declare var Dropzone;
    //https://gist.github.com/compact/8118670
    Register.Common.directive("dropzone", () => {
        return (scope, element, attrs) => {
            var config, dropzone;

            config = scope[attrs.dropzone];

            // create a Dropzone for the element with the given options
            dropzone = new Dropzone(element[0], config.options);

            // bind the given event handlers
            //_.each(config.eventHandlers, function (handler, event) {
            //    dropzone.on(event, handler);
            //});
        };

    });
}