'use strict';
angular.module('TEPOApp').controller('PurchaseRequestCtrl', function ($scope, filterFilter, $filter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, PurchaseRequestFactory, $uibModal, $location) {
    $rootScope.visibleSearch = true;
    $scope.TempSearchPRList;
    $scope.PurchaseRequestList;
    $scope.CurrentDate = new Date();
    var x = [];
    $scope.count = 0;
    $scope.pagenumber = 1;
    $scope.pageprecounts = 10;
    $scope.searchkey = "";
    $scope.getPRList = function () {
        $rootScope.loading = true;
        PurchaseRequestFactory.PRSearch({
            'UserId': $localStorage.globals.currentUser.UserId,
            'Search': $scope.searchkey,
            'pageNumber': $scope.pagenumber,
            'pagePerCount': $scope.pageprecounts
        }).success(function (response) {
            $rootScope.loading = false;
            $scope.PurchaseRequestList = response;
            $scope.TempSearchPRList =$scope.PurchaseRequestList;
            $scope.TempSearchPRList1 =  $scope.TempSearchPRList.result;
        });
    }
   // $scope.getPRList();
       $scope.pagenationss = function(a, b) {
        var b = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a;
        $scope.pageprecounts = b;
        $scope.getPRList();
    }
    $scope.pagenationss_clk = function(a, b) {
        if ("0" == b) var a1 = --a;
        else var a1 = ++a;
        "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1;
        var c = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a1;
        $scope.pageprecounts = c;
        $scope.getPRList();
    }
    $scope.redirectToDetails = function (prid) {
        $localStorage.prid = prid;
        $sessionStorage.BackLink = "SearchPR";
        $location.path("/PRDetail");
    }
    $scope.filterSearchList = function (search) {
        $rootScope.loading = true;
        PurchaseRequestFactory.TEPRSearchForSearchTab({ 'LoginId': $localStorage.globals.currentUser.UserId,
         "Search": search }).success(function (response) {
           
            $scope.TempSearchPRList1 = response.result
            $rootScope.loading = false;
        });
    }    
    $scope.searchPrGrid = {data: 'TempSearchPRList1',showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 37,
        columnDefs: [{
            field: 'PurchaseRequestId',
            displayName: 'PR Number',
            width: 90
        },
        {
            field: 'ApprovedOn',
            displayName: 'PR Date',
            width: 130,
           cellTemplate: '<div class="optionsGrid">{{row.entity.ApprovedOn ?(row.entity.ApprovedOn|date:"dd-MMM-yyyy"):\'-\'}}</div>'
        },
        {
            field: 'FundCenter_Code',
            displayName: 'Fund Center Code',
            width: 120
        }, {
            field: 'FundCenter_Owner',
            displayName: 'Fund Center Owner'
        }, 
         {
            field: 'PurchaseRequestTitle',
            displayName: 'PR Title'
        },
        {
            field: 'PurchaseRequestDesc',
            displayName: 'Description'
        }, {
            field: 'status',
            displayName: 'Status'
        },
        {
            field: 'CreatedBy',
            displayName: 'Submitter',
            width: 120,
        },
        {
            field: 'ApprovedOn',
            displayName: 'Submitted Date',
            width: 130,
           cellTemplate: '<div class="optionsGrid">{{row.entity.ApprovedOn ?(row.entity.ApprovedOn|date:"dd-MMM-yyyy"):\'-\'}}</div>'
        },
      {
            field: '',
            displayName: 'Options',
             width: 100,
            cellTemplate: '<div class="optionsGrid">\
            <a href="javascript:void(0)" title="PR View" ng-click="redirectToDetails(row.entity.PurchaseRequestId)"><i class="fa fa-television"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="getDetailsForView(row.entity.PurchaseRequestId)" title="view" style="margin:3px;"><i class="fa fa-eye"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#deliverySchedulelist" ng-click="getDeliveryScheduleDetails(row.entity.PurchaseRequestId)" title="view schedule" style="margin:3px;"><i class="fa fa-info"></i></a>\
            </div>'
        }
        ]
    };
    $scope.getDeliveryScheduleDetails = function(prid){
        
        PurchaseRequestFactory.deliveryScheduleList({ 'PurchaseRequestId':  prid}).success(function (response) {      
            $scope.DeliverySchedulesByPRItemId= response.result;       
        });
    }
    $scope.infoPage = function (Approvers) {
        $scope.appr = Approvers;
    }
    $scope.infoGrid = {
        data: 'appr',
        showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 40,
        rowTemplate: '<div style="height: 100%" ><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
        '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>' +
        '<div ng-cell></div>' +
        '</div></div>',
        columnDefs: [{
            field: 'SequenceNumber',
            displayName: 'Seq No'
        },
        {
            field: 'ApproverName',
            displayName: 'Approver Name'
        },
        {
            field: 'Status',
            displayName: 'Status'
        },
        {
            field: 'LastModifiedOn',
            displayName: 'Last Modified On',
            cellTemplate: '<p class="gridCellItem ">{{row.entity.LastModifiedOn | date:"dd-MMM-yyyy"}}</p>'
        }
        ]
    };
    $scope.getDetailsForView = function (
        prid) {
        $rootScope.loading = true;
        PurchaseRequestFactory.PRDetails({
            'PRId': prid
        }).success(function (response) {
            $scope.prdetails = response.result;
        });
        PurchaseRequestFactory.PRMaterialdetails({
            'PurchaseRequestId': prid
        }).success(function (response) {
            $rootScope.loading = false;
            $scope.prMaterialdetails =
                response.result;
        });
    }
})