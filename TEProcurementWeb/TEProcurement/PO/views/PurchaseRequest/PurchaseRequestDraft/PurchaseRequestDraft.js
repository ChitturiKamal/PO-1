'use strict';
angular.module('TEPOApp').controller('PurchaseRequestDraft', function ($scope, filterFilter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, PurchaseRequestDraftFactory, $uibModal, $location, $filter) {
    $rootScope.visibleSearch = true;
    $scope.TempCustList;
    $scope.CustList;
    $scope.CurrentDate = new Date();
    var x = [];
    $scope.count = 0;
    $scope.pagenumber = 1;
    $scope.pageprecounts = 10;
    $scope.searchkey = "";   
    $scope.getDraftPRList = function () {
        $rootScope.loading = true;
        PurchaseRequestDraftFactory.PRDraftSearch({
            'LoginId': $localStorage.globals.currentUser.UserId,
            'FilterBy': $scope.SearchText,
            'pageNumber': $scope.pagenumber,
            'pagePerCount': $scope.pageprecounts

        }).success(function (response) {
            $rootScope.loading = false;
            $scope.info = response.info;
            $scope.CustList = response.result;
            $scope.TempCustList = $scope.CustList;
        });
    }
    $scope.getDraftPRList();
     $scope.pagenationss = function(a, b) {
        var b = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a;
        $scope.pageprecounts = b;
        $scope.getDraftPRList();
    }
    $scope.pagenationss_clk = function(a, b) {
        if ("0" == b) var a1 = --a;
        else var a1 = ++a;
        "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1;
        var c = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a1;
        $scope.pageprecounts = c;
        $scope.getDraftPRList();
    }
    $scope.searchDraftPR = function (search) {
        $scope.SearchText = search
        $scope.getDraftPRList();
    }

    $scope.filterSearchList = function (search) {
        $scope.TempCustList = $filter('filter')($scope.CustList, search);
    }
    $scope.SetSubmitRequestData = function (data) {
        $scope.SubmitRequestData = data;
    }
    $scope.GLPage = function (prid) {
        $localStorage.fromPrSave = false;
        $localStorage.prid = prid;
        $sessionStorage.BackLink = "DraftPR";
        $location.path('/PRupdate');
    }
    $scope.infoPage = function (Approvers) {
        $scope.appr = Approvers;
    }
    $scope.SubmitPRtoPO = function () {
        $rootScope.loading = true;
        PurchaseRequestDraftFactory.PRApprove({ "PRId": $scope.SubmitRequestData.PurchaseRequestId, "LastModifiedByID": $localStorage.globals.currentUser.UserId }).success(function (response) {
            if (response.info.errorcode == '0') {
                $('#SubmitPRPopUp').modal('hide');
                $rootScope.loading = false;
                $scope.getDraftPRList();
            }
            AlertMessages.alertPopup(response.info);

            $rootScope.loading = false;
        });
    }
    $scope.searchDraftGrid = {
        data: 'TempCustList',
        showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 37,

        columnDefs: [{
            field: 'PurchaseRequestId',
            displayName: 'PR Number',
            width: 90
        }, {
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
        },{
            field: 'status',
            displayName: 'Status'
        },
        {
            field: 'CreatedBy',
            displayName: 'Submitter',
            width: 120,
        },
        {
            field: 'CreatedOn',
            displayName: 'Submitted Date',
            width: 130,
            cellTemplate: '<div class="optionsGrid">{{row.entity.CreatedOn ?(row.entity.CreatedOn|date:"dd-MMM-yyyy"):\'NA\'}}</div>'
        },
        {
            field: '',
            displayName: 'Options',
            width: 100,
            cellTemplate: '<div class="optionsGrid">\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#GLPagePopUp" ng-click="GLPage(row.entity.PurchaseRequestId);"  style="margin:3px;" title="Update" ng-if="row.entity.status==\'Draft\'"><i class="fa fa-pencil"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#SubmitPRPopUp" ng-click="SetSubmitRequestData(row.entity);"  style="margin:3px;" title="Submit for PO creation" ng-if="row.entity.status==\'Draft\'"><i class="fa fa-cog"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="getDetailsForView(row.entity.PurchaseRequestId)" title="view" style="margin:3px;"><i class="fa fa-eye"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#deliverySchedulelist" ng-click="getDeliveryScheduleDetails(row.entity.PurchaseRequestId)" title="view schedule" style="margin:3px;"><i class="fa fa-info"></i></a>\
            </div>'
        }
        ]
    };

    $scope.getDeliveryScheduleDetails = function(prid){
        
        PurchaseRequestDraftFactory.deliveryScheduleList({ 'PurchaseRequestId':  prid}).success(function (response) {      
            $scope.DeliverySchedulesByPRItemId= response.result;       
        });
    }
    $scope.infoGrid = {
        data: 'appr',
        showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 40,
        rowTemplate: '<div style="height: 100%" ng-class=""><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
        '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>' +
        '<div ng-cell></div>' +
        '</div></div>',
        columnDefs: [{
            field: 'SequenceNumber',
            displayName: 'Seq No'
        }, {
            field: 'ApproverName',
            displayName: 'Approver Name'
        }, {
            field: 'Status',
            displayName: 'Status'
        }, {
            field: 'LastModifiedOn',
            displayName: 'Last Modified On',
            cellTemplate: '<p class="gridCellItem ">{{row.entity.LastModifiedOn | date:"dd-MMM-yyyy"}}</p>'
        }]
    };

    $scope.getDetailsForView = function (prid) {
        $rootScope.loading = true;
        PurchaseRequestDraftFactory.PRDetails({ 'PRId': prid }).success(function (response) {

            $scope.prdetails = response.result;

        });
        PurchaseRequestDraftFactory.PRMaterialdetails({ 'PurchaseRequestId': prid }).success(function (response) {
            $rootScope.loading = false;
            $scope.prMaterialdetails = response.result;
        });
    };
})

