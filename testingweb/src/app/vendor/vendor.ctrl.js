(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('POVendorController', POVendorController)
        .directive('whenScrollEnds', whenScrollEnds);

    /** @ngInject */
    function POVendorController(POVendorServices) {
        var vm = this;

        var pageIndex = 1;
        var isNextCallAllowed = true;

        vm.isNoDataPresent = false;
        vm.vendorList = [];
        
        vm.loadMoreRecords = getVendors;
        activate();

        function activate() {
            getVendors();
        }

        function getVendors() {
            if (isNextCallAllowed) {
                isNextCallAllowed = false;
                var data = {
                    PageCount: pageIndex++
                }
                POVendorServices.callServices("getVendors", data).then(
                    function(response) {
                        if (response.data && response.data.length > 0) {
                            isNextCallAllowed = true;
                            vm.vendorList = vm.vendorList.concat(response.data);
                        }
                        vm.isNoDataPresent = (vm.vendorList.length == 0);
                    }
                );
            }
        }
    }

    function whenScrollEnds() {
        return {
            restrict: "A",
            link: function(scope, element, attrs) {
                var visibleHeight = element.height();
                var threshold = 10;

                element.scroll(function() {
                    var scrollableHeight = element.prop('scrollHeight');
                    var hiddenContentHeight = scrollableHeight - visibleHeight;
                    if (hiddenContentHeight - element.scrollTop() <= threshold) {
                        // Scroll is almost at the bottom. Loading more rows
                        scope.$apply(attrs.whenScrollEnds);
                    }
                });
            }
        };
    }
})();
