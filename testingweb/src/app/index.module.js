(function() {
    'use strict';
    angular.module('procurementApp', ['ngAnimate',
            'ngCookies',
            'ngSanitize',
            'ngMessages',
            'ngAria',
            'ui.router',
            'ui.bootstrap',
            'toastr',
            'blockUI',
            'textAngular',
            'jlareau.pnotify',
            'ngMaterial',
            '720kb.datepicker'
        ])
        .config(config);

    function config(blockUIConfig) {
        blockUIConfig.autoInjectBodyBlock = true;
        blockUIConfig.delay = 0;
    }
})();
