//karma updaes
//npm update karma-ng-html2js-preprocessor
//npm update karma
//npm update karma-ie-launcher

module.exports = function (config) {
    config.set({

        // base path, that will be used to resolve files and exclude
        basePath: '',

        // frameworks to use
        frameworks: ['jasmine'],


        // list of files / patterns to load in the browser
        files: [
            'Scripts/cdn/angular/angular.js',
            'Scripts/**/angular-*.js',
            'Scripts/sinon-*.js',
            'AngularApp/Main/Register.js',
            'Scripts/minorfiles/*.js',
            'Scripts/cdn/*.js',
            { pattern: 'AngularApp/**/*.js', include: true },
            { pattern: 'AngularAppSpec/**/*.js', include: true },
            '**/*.html'
        ],

        // list of files to exclude
        exclude: [
            'Scripts/angular-scenario.js',
            'AngularApp/Common/Services/Utilities/ScriptjsValue.js'
        ],


        // Testing Directives
        preprocessors: {
            '**/*.html': ['ng-html2js']
        },

        ////ngHtml2
        ngHtml2JsPreprocessor: {
            // strip this from the file path
            //stripPrefix: '/',
            // prepend this to the
            prependPrefix: '/',

            //////    // or define a custom transform function
            //////    //cacheIdFromPath: function(filepath) {
            //////    //    return cacheId;
            //////    //},

            //////    // setting this option will create only a single module that contains templates
            //////    // from all the files, so you can load them all with module('foo')
            moduleName: 'ng-Templates'
        },


        // test results reporter to use
        // possible values: 'dots', 'progress', 'junit', 'growl', 'coverage'
        reporters: ['dots'],


        // web server port
        port: 9876,

        //html
        // enable / disable colors in the output (reporters and logs)
        colors: true,


        // level of logging
        // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
        logLevel: config.LOG_INFO,


        // enable / disable watching file and executing tests whenever any file changes
        autoWatch: true,

        reportSlowerThan: 100,

        // Start these browsers, currently available:
        // - Chrome
        // - ChromeCanary
        // - Firefox
        // - Opera (has to be installed with `npm install karma-opera-launcher`)
        // - Safari (only Mac; has to be installed with `npm install karma-safari-launcher`)
        // - PhantomJS
        // - IE (only Windows; has to be installed with `npm install karma-ie-launcher`)
        browsers: ['Chrome'],
        // , 'IE' 'Firefox',


        // If browser does not capture in given timeout [ms], kill it
        captureTimeout: 60000,


        // Continuous Integration mode
        // if true, it capture browsers, run tests and exit
        singleRun: false
    });
};