'use strict';
angular.module('TEPOApp').controller('UpCommingPo', function ($scope, filterFilter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, UpCommingPoFactory, $uibModal, $location) {
    $rootScope.visibleSearch = true;
    $scope.TempCustList;
    $scope.CurrentDate = new Date();
    var x = [];
    $scope.count = 0;
    $scope.getCustList = function () {
        $rootScope.loading = true;
        UpCommingPoFactory.PurchaseOrderByUser({"UserId":$scope.UserDetails.currentUser.UserId,"Status":"Draft","PageCount":1,"SortBy":"PoNumber"}).success(function (response) {
            $rootScope.loading = false;
            $scope.CustList = response.res;
            $scope.TempCustList = $scope.CustList;
        });
    }
    $scope.getCustList();
    $scope.filterSearchList = function (search) {
        $scope.count = 1;
        if (x.length <= $scope.CustList.length) {
            x = $scope.CustList;
        }
        $scope.TempCustList = filterFilter(x, search);
    }
    $scope.finalresultGrid = {
        data: 'TempCustList',
        showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 25,
         rowTemplate:'<div style="height: 100%" ng-class=""><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
        '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>' +
        '<div ng-cell></div>' +
        '</div></div>',
        columnDefs: [{
            field: 'Purchasing_Order_Number',
            displayName: 'PO Number',
             width:100
          
        }, {
            field: 'PoTitle',
            displayName: 'Title',
             width:100
         },
        //   {
        //     field: 'Vendor_Account_Number',
        //     displayName: 'Vendor Acc No',
        //      width:70,
        //     cellTemplate: '<p class="gridCellItem ">{{row.entity.Vendor_Account_Number}}</p>'
        // },
        {
            field: 'VendorName',
            displayName: 'Vendor'
        }, {
            field: 'Purchasing_Document_Date',
            displayName: 'Document Date',
            cellTemplate: '<p class="gridCellItem ">{{row.entity.Purchasing_Document_Date | date:"dd-MMM-yy"}}</p>'
        }, {
            field: 'FundCenter_Description',
            displayName: 'Fund Center'
        }, {
            field: 'Amount',
            displayName: 'Amount',
            cellTemplate: '<div class="text-right numberRight ">{{row.entity.Amount?(row.entity.Amount|number):\'-\'}}</div>',
            width: 80
        }, {
            field: 'POStatus',
            displayName: 'Status',
             width:75
        }, {
            field: 'ProjectCodes',
            displayName: 'Project Code'
        }, {
            field: 'WbsHeads',
            displayName: 'WBS Category'
        }, {
            field: 'SubmitterName',
            displayName: 'Submitter'
        }, {
            field: 'ManagerName',
            displayName: 'Manager'
        }
        ]
    };
})

