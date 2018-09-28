'use strict';
angular.module('TEPOApp').controller('POPendingForApprovalCtrl', function ($scope, filterFilter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, POPendingForApprovalFactory, $uibModal, $location,$state) {
    $rootScope.visibleSearch = true;
    $scope.TempCustList;
    $scope.CurrentDate = new Date();
    var x = [];
    $scope.count = 0;

    $scope.pagenumber = 1;
    $scope.pageprecounts = 10;
    $scope.GetallList = function() {
        $rootScope.loading = true;
        if ($scope.pagenumber <= '0') $scope.pagenumber = 1;
        POPendingForApprovalFactory.POPendingForApproval_Pagination({
            'UserId': $localStorage.globals.currentUser.UserId,
            'SortBy': '',
            'pageNumber': $scope.pagenumber,
            'pagePerCount': $scope.pageprecounts
        }).success(function(response) {
            $rootScope.loading = false;
            $scope.CustList = response.result;
            $scope.TempCustList = $scope.CustList;
        });
    }
    $scope.pagenationss = function(a, b) {
        var b = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a;
        $scope.pageprecounts = b;
        $scope.GetallList();
    }
    $scope.pagenationss_clk = function(a, b) {
        if ("0" == b) var a1 = --a;
        else var a1 = ++a;
        "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1;
        var c = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a1;
        $scope.pageprecounts = c;
        $scope.GetallList();
    }

    $scope.getCustList = function () {
        $rootScope.loading = true;
        POPendingForApprovalFactory.POPendingForApproval_Pagination({"UserId":$localStorage.globals.currentUser.UserId,"SortBy":"","pagePerCount":10,"pageNumber":1}).success(function (response) {
            $rootScope.loading = false;
            $scope.CustList = response.result;
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

    $scope.PendingForApprovalPop = function(poOrderNumber,POUniqueId) { 
        $rootScope.PurchaseOrderNumber = poOrderNumber;
        $scope.UserDetails = $localStorage.globals;
        $scope.POUniqueId = POUniqueId;
    }

    $scope.PendingForApprovalApproveBtn = function() {
        $rootScope.loading = true;
        var data = JSON.stringify($('.PendingForApprovalForm').serializeObject());
        POPendingForApprovalFactory.PendingForPOApprove(data).success(function(response) {
            AlertMessages.alertPopup(response.info);
            $state.reload();
            $rootScope.loading = false;
        }); 
    };

    $scope.PendingForApprovalRejectBtn = function() {
        $rootScope.loading = true;
        var data = JSON.stringify($('.PendingForApprovalForm').serializeObject());
        POPendingForApprovalFactory.PendingForPOReject(data).success(function(response) {
            AlertMessages.alertPopup(response.info);
            $state.reload();
            $rootScope.loading = false;
        }); 
    };

    $scope.genPOPDF=function(HeaderUniqueid,status){
        $rootScope.loading=true;
        POPendingForApprovalFactory.POPDFDefaultView({'POID':HeaderUniqueid}).success(function(response){
            $rootScope.loading=false;
            //if(response.info.errorcode==0) $scope.PDFUrl='data:application/pdf;base64,'+response.ResultPdf;
            $scope.titleStatus= status;
            if(response.info.errorcode==0) $scope.PDFUrl=response.Result;
            else{$scope.PDFUrl="";AlertMessages.alertPopup(response.info);}
        });
    }

    $scope.finalresultGrid = {
        data: 'TempCustList',
        showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 30,
         rowTemplate:'<div style="height: 100%" ng-class=""><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
        '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>' +
        '<div ng-cell></div>' +
        '</div></div>',
        columnDefs: [{
            field: 'ProjectCodes',
            displayName: 'Project Code'
        }, {
            field: 'WbsHeads',
            displayName: 'WBS Category'
        }, {
            field: 'Purchasing_Order_Number',
            displayName: 'PO Number',
             width:100
        }, {
            field: 'PoTitle',
            displayName: 'Title',
             width:100
        }, 
        // {
        //     field: 'Vendor_Account_Number',
        //     displayName: 'Vendor Acc No',
        //      width:70,
        //     cellTemplate: '<p class="gridCellItem ">{{row.entity.Vendor_Account_Number}}</p>'
        // }, {
        //     field: 'FundCenter_Description',
        //     displayName: 'Fund Center'
        // }, 
        {
            field: 'Purchasing_Document_Date',
            displayName: 'Creation Date',
            cellTemplate: '<p class="gridCellItem ">{{row.entity.Purchasing_Document_Date | date:"dd-MMM-yy"}}</p>'
        }, {
            field: 'VendorName',
            displayName: 'Vendor'
        }, {
            field: 'Amount',
            displayName: 'Amount',
            cellTemplate: '<div class="text-right numberRight ">{{row.entity.Amount?(row.entity.Amount|number):\'-\'}}</div>',
            width: 75
        }, {
            field: 'POStatus',
            displayName: 'Status',
             width:75
        }, {
            field: 'SubmitterName',
            displayName: 'Submitter'
        }, {
            field: 'ManagerName',
            displayName: 'Manager'
        }, {
            field: '',
            displayName: 'Options',
            cellTemplate:'<div class="optionsGrid"><a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFPagePopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);" title="PDF"><i class="fa fa-download"></i></a>&nbsp;&nbsp;<a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="infoPage(row.entity.Approvers);" title="Approvers" ng-show="row.entity.Approvers.length>0"><i class="fa fa-info-circle"></i></a></div>'
        }
        ]
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
        rowHeight: 30,
         rowTemplate:'<div style="height: 100%" ng-class=""><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
        '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>' +
        '<div ng-cell></div>' +
        '</div></div>',
        columnDefs: [{
            field: 'SequenceNumber',
            displayName: 'Sequence Number',
            width: 140
        }, {
            field: 'ApproverName',
            displayName: 'Approver Name'
        }, {
            field: 'Status',
            displayName: 'Status'
        }]
    };
})

