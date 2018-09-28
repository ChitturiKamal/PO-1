(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('MainController', MainController)
        .directive('resize', resize);

    /** @ngInject */
    function MainController(commonUtilService, POServices, $rootScope) {
        var vm = this;

        vm.statusToggle = true;
        vm.getProfile = commonUtilService.getProfile();
        $rootScope.reload = getStatusCount;

        activate();

        function activate() {
            getStatusCount();
        }

        function getStatusCount() {
            var data = {
                UserId: vm.getProfile.userId
            };
            POServices.callServices("getPOStatusCount", data).then(
                function(response) {
                    if (response.data) {
                        vm.statusCount = response.data;
                    }
                }
            );
        }
    }

    function resize($window) {
        return function() {
            var w = angular.element($window);
            var changeHeight = function() {
                angular.element('.scorlbar').css('height', (w.height() - 128) + 'px');
                angular.element('.containerHeight').css('height', (w.height() - 84) + 'px');
                angular.element('.reportTableHeight').css('height', (w.height() - 112) + 'px');
                //angular.element('.duplicateContainerHeight').css('height', (w.height() - 186) + 'px');
            };
            w.bind('resize', function() {
                changeHeight(); // when window size gets changed             
            });
            changeHeight(); // when page loads          
        }
    }
})();
