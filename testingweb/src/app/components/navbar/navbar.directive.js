(function() {
    'use strict';

    angular
        .module('procurementApp')
        .directive('acmeNavbar', acmeNavbar);

    /** @ngInject */
    function acmeNavbar() {
        var directive = {
            restrict: 'E',
            templateUrl: 'app/components/navbar/navbar.html',
            scope: {
                creationDate: '=',
                profileInfo: '='
            },
            controller: NavbarController,
            controllerAs: 'vm',
            bindToController: true
        };

        return directive;

        /** @ngInject */
        function NavbarController(commonUtilService) {
            var vm = this;
            vm.profileImg = 'assets/images/profile.png';
            vm.configRole = commonUtilService.config();
            commonUtilService.isImagePresent(vm.profileInfo.photo).then(function(result) {
                if (result)
                  vm.profileImg = vm.profileInfo.photo;
            });

            vm.logout = commonUtilService.logout;
        }
    }

})();
