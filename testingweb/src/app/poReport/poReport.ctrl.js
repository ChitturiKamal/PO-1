(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('POReportController', POReportController)
        .directive('whenScrollEnds', whenScrollEnds);

    /** @ngInject */
    function POReportController(POReportServices, commonUtilService, $window) {
        var vm = this;
        var userInfo = commonUtilService.getProfile();
        var isNextCallAllowed = true;
        var pageIndex = 1;

        vm.isNoDataPresent = false;
        
        vm.poList = [];
        

        vm.loadMoreRecords = getPoReport;
        activate();

        function activate() {
            getPoReport();
        }

        function getPoReport() {
            if (isNextCallAllowed) {
                isNextCallAllowed = false;
                var data = {
                    UserId: userInfo.userId,
                    Status: 'All',
                    PageCount: pageIndex++,
                    SortBy: 'PoNumber'
                }
                POReportServices.callServices("getPOList", data).then(
                    function(response) {
                        if (response.data && response.data.length > 0) {
                            isNextCallAllowed = true;
                            vm.poList = vm.poList.concat(getStatus(response.data));
                        }
                        vm.isNoDataPresent = (vm.poList.length == 0);
                    }
                );
            }
        }

        function getStatus(poList) {
            angular.forEach(poList, function(value) {
                if (userInfo.isPOAdmin)
                    value.ReleaseCodeStatus = value.POStatus;
                else
                    value.ReleaseCodeStatus = $window._.pluck($window._.filter(value.Approvers, { 'ApproverId': userInfo.userId }), 'Status')[0];
                value.pendingApprovers = $window._.pluck($window._.filter(value.Approvers, function(value) {
                    return value.Status == 'Pending for Approval' || value.Status == 'Draft'
                }), 'ApproverName').join('\n');
            });
            return poList;
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
