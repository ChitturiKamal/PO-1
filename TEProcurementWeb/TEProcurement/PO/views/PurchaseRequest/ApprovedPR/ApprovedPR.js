'use strict';
angular.module('TEPOApp').controller('ApprovedPR', function ($scope, filterFilter, $filter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, ApprovedPRFactory, $uibModal, $location) {
    $rootScope.visibleSearch = true;
    $scope.TempCustList;
    $scope.CurrentDate = new Date();
    var x = [];
    $scope.count = 0;
    $scope.pagenumber = 1;
    $scope.pageprecounts = 10;   
    $scope.getApprovedPRList = function () {
        $rootScope.loading = true;
        ApprovedPRFactory.GetPRList({
            "LoginId": $localStorage.globals.currentUser.UserId,
             'FilterBy': $scope.SearchText,
            'pageNumber': $scope.pagenumber,
            'pagePerCount': $scope.pageprecounts
        }).success(function (response) {
            $rootScope.loading = false;
            $scope.CustList = response.result;
            $scope.info = response.info;
            $scope.TempCustList = $scope.CustList;
        });
    }
    $scope.getApprovedPRList();
     $scope.pagenationss = function(a, b) {
        var b = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a;
        $scope.pageprecounts = b;
        $scope.getApprovedPRList();
    }
    $scope.pagenationss_clk = function(a, b) {
        if ("0" == b) var a1 = --a;
        else var a1 = ++a;
        "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1;
        var c = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a1;
        $scope.pageprecounts = c;
        $scope.getApprovedPRList();
    }
    $scope.searchApprovedPR = function (search) {
        $scope.SearchText = search
        $scope.getApprovedPRList();
    }
    $scope.filterSearchList = function (search) {
        $scope.TempCustList = $filter('filter')($scope.CustList, search);
    }
    $scope.redirectToDetails = function (prid) {
        $localStorage.prid = prid;
        $sessionStorage.BackLink = "ApprovedPR";
        $location.path("/PRDetail");
    }

    $scope.MyPoGrid = {
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
        },
        {
            field: 'ApprovedOn',
            displayName: 'PR Date',
            width: 130,
           cellTemplate: '<div class="optionsGrid">{{row.entity.ApprovedOn ?(row.entity.ApprovedOn|date:"dd-MMM-yyyy"):\'NA\'}}</div>'
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
           cellTemplate: '<div class="optionsGrid">{{row.entity.ApprovedOn ?(row.entity.ApprovedOn|date:"dd-MMM-yyyy"):\'NA\'}}</div>'
        },
        {
            field: 'status',
            displayName: 'Status'
        },
      {
            field: '',
            displayName: 'Options',
            width: 100,
            cellTemplate:'<div class="optionsGrid">\
            <a href="javascript:void(0)" title="PR View" ng-click="redirectToDetails(row.entity.PurchaseRequestId)"><i class="fa fa-television"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="getDetailsForView(row.entity.PurchaseRequestId)" title="view" style="margin:3px;"><i class="fa fa-eye"></i></a>\
            <a data-toggle="modal" data-target="#purchaseRequestHistoryPopUp" ng-click="getHistoryDetails(row.entity.PurchaseRequestId)" ><i class="fa fa-database" aria-hidden="true" title="PO Details" style="font-size:12pt;margin: 2px"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#deliverySchedulelist" ng-click="getDeliveryScheduleDetails(row.entity.PurchaseRequestId)" title="view schedule" style="margin:3px;"><i class="fa fa-info"></i></a>\
            </div>'
        }
        ]
    };
    $scope.genPOPDF = function (HeaderUniqueid) {
        $rootScope.loading = true;
        ApprovedPRFactory.POPDFDefaultView({ 'POID': HeaderUniqueid }).success(function (response) {
            $rootScope.loading = false;
            //if(response.info.errorcode==0) $scope.PDFUrl='data:application/pdf;base64,'+response.ResultPdf;
            $scope.titleStatus = status;
            if (response.info.errorcode == 0) $scope.PDFUrl = response.Result;
            else { $scope.PDFUrl = ""; AlertMessages.alertPopup(response.info); }
        });
    }
    $scope.getDeliveryScheduleDetails = function(prid){
        
        ApprovedPRFactory.deliveryScheduleList({ 'PurchaseRequestId':  prid}).success(function (response) {      
            $scope.DeliverySchedulesByPRItemId= response.result;       
        });
    }
    $scope.loadDataToUpdate = function(PurchaseRequestId){
        $rootScope.PurchaseRequestIdToUPdate = PurchaseRequestId;
    }
    $scope.updatePRStatus = function(){


        if($scope.PrStatus ==""||!$scope.PrStatus){
            console.log($scope.PrStatus);
        }else{
            $rootScope.loading = true;    
            ApprovedPRFactory.PrStatusUpdate({ 'PurchaseRequestId':  $rootScope.PurchaseRequestIdToUPdate,"PRPoStatus":$scope.PrStatus,"LastModifiedByID":$scope.UserDetails.currentUser.UserId }).success(function (response) {      
                $rootScope.loading = false;    
                $('#PRStatusPopUp').modal('hide');
                AlertMessages.alertPopup(response.info);         
            });
        }
    }
    $scope.getHistoryDetails = function(prid){
        $rootScope.loading = true;
        ApprovedPRFactory.PoPrHistory({ 'PurchaseRequestId': prid }).success(function (response) {      
            $rootScope.loading = false;    
            $scope.prHistorydetails = response.result;           
        });
    }
    $scope.getDetailsForView = function(prid){
        $rootScope.loading = true;
        ApprovedPRFactory.PRDetails({ 'PRId': prid }).success(function (response) {          
            $scope.prdetails = response.result;           
        });
        ApprovedPRFactory.PRMaterialdetails({ 'PurchaseRequestId': prid }).success(function (response) {
            $rootScope.loading = false;
            $scope.prMaterialdetails = response.result;
        });
    };
    $scope.infoPage=function(Approvers){
        $scope.appr = Approvers;
    }
    $scope.infoGrid = {
        data: 'appr',
        showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 40,
         rowTemplate:'<div style="height: 100%" ng-class=""><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
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
    $rootScope.loading = false;
})

