'use strict';
angular.module('TEPOApp')
    .controller('AlertDemoCtrl', ["$scope", "$rootScope", "$timeout", function($scope, $rootScope, $timeout) {
        $scope.closeAlert = function(index) { $scope.alerts.splice(index, 1); };
        $rootScope.autoHide = function() { $timeout(function() { $rootScope.alerts.splice(0, 1); }, 8000); }
    }])
    .controller('LoadingCtrl', ["$scope", "$rootScope", "$timeout", function($scope, $rootScope, $timeout) {
        $rootScope.closeLoading = function(index) { $rootScope.ShowLoading = false; };
        $rootScope.autoHideLoading = function() { $timeout(function() { $rootScope.ShowLoading = false; }, 3000); }
    }])

    .factory('CommonApprovalsFactory', function($sessionStorage, $http) {
        var result = {};
        //Units Approvals Starts
        result.SendForApprovalUnits = function(data) { return $http.post($sessionStorage.server_url + '/TEUnitsAPI/SendForApprove', data, heads); }
        result.FinalApprovalUnits = function(data) { return $http.post($sessionStorage.server_url + '/TEUnitsAPI/ApprovalProcessing', data, heads); }
        result.FinalRejectUnits = function(data) { return $http.post($sessionStorage.server_url + '/TEUnitsAPI/RejectProcessing', data, heads); }
        //Units Approvals Ends
        return result;
    })
    .factory('PickListsListing', function($sessionStorage, $http) {
        var result = {};
        result.GetPickLists = function(data) { return $http.post($sessionStorage.server_url + '/TEPickListItemItemAPI/GetPickListByNameNew', data, heads); }
        return result;
    })
    
.directive('loading', function() {
    return {
        restrict: 'E',
        replace: true,
        template: '<div class="loadingg"><div class="lod"></div>' +
            '<div class="sk-wave">' +
            '<div class="sk-rect sk-rect1"></div>' +
            '<div class="sk-rect sk-rect2"></div>' +
            '<div class="sk-rect sk-rect3"></div>' +
            '<div class="sk-rect sk-rect4"></div>' +
            '<div class="sk-rect sk-rect5"></div>' +
            '</div>' +
            '</div>',
        //template: 'localhost\Fugue\Lead\views\loading.html',
        link: function(scope, element, attr) {
            scope.$watch('loading', function(val) {
                if (val) $(element).show();
                else $(element).hide();
            });
        }
    }
})