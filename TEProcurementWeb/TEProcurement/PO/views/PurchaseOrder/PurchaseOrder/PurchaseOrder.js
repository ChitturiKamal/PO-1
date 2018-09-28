'use strict';
angular.module('TEPOApp').controller('PurchaseOrder', function ($scope, $state, filterFilter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, PurchaseOrderFactory, $uibModal, $location) {
    $rootScope.visibleSearch = true;
    $scope.TempCustList;
    $scope.CustList;
    $scope.CurrentDate = new Date();
    var x = [];
    $scope.count = 0;

    $scope.pagenumber = 1;
    $scope.pageprecounts = 10;
    $scope.searchkey = "";
    $scope.GetallList = function () {
        if ($scope.searchkey != null && $scope.searchkey != "") {
            $rootScope.loading = true;
            if ($scope.pagenumber <= '0') $scope.pagenumber = 1;
            PurchaseOrderFactory.POSearch_Pagination({
                'UserId': $localStorage.globals.currentUser.UserId,
                'Search': $scope.searchkey,
                'pageNumber': $scope.pagenumber,
                'pagePerCount': $scope.pageprecounts
            }).success(function (response) {
                $rootScope.loading = false;
                $scope.CustList = response.result;
                $scope.TempCustList = $scope.CustList;
            });
        }
    }
    // $scope.pagenationss = function(a, b) {
    //     var b = $("select[name='pageprecounts']").val();
    //     $scope.pagenumber = a;
    //     $scope.pageprecounts = b;
    //     $scope.GetallList();
    // }
    // $scope.pagenationss_clk = function(a, b) {
    //     if ("0" == b) var a1 = --a;
    //     else var a1 = ++a;
    //     "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1;
    //     var c = $("select[name='pageprecounts']").val();
    //     $scope.pagenumber = a1;
    //     $scope.pageprecounts = c;
    //     $scope.GetallList();
    // }

    $scope.getCustList = function () {
        if ($scope.searchkey != null && $scope.searchkey != "") {
            $rootScope.loading = true;
            //$scope.UserDetails.currentUser.UserId
            PurchaseOrderFactory.POSearch_Pagination({ "UserId": $localStorage.globals.currentUser.UserId, "Search": $scope.searchkey, "pagePerCount": 10, "pageNumber": 1 }).success(function (response) {
                $rootScope.loading = false;
                $scope.CustList = response.result;
                $scope.TempCustList = $scope.CustList;
            });
        }
    }
    $scope.getCustList();
    $scope.filterSearchList = function (search) {
        if (search.length >= 3) {
            $scope.searchkey = search;
            $scope.GetallList();
        }
    }
      $scope.poVersionDetails = function (ponumber) {
        PurchaseOrderFactory.poVersionInfo({ 'PONumber': ponumber, 'LastModifiedBy': $scope.UserDetails.currentUser.UserId }).success(function (response) {
            if (response.info.errorcode == '0') {
                $scope.POVersionResult = response.result;
            }
            else {
                AlertMessages.alertPopup(response.info);
            }
        });
    }
     $(document).on('show.bs.modal', '.modal', function() {
            var zIndex = 1040 + (10 * $('.modal:visible').length);
            $(this).css('z-index', zIndex);
            setTimeout(function() {
                $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
            }, 0);
        });

    $scope.genPOPDF = function (HeaderUniqueid, status) {
        $rootScope.loading = true;
        PurchaseOrderFactory.POPDFDefaultView({ 'POID': HeaderUniqueid }).success(function (response) {
            $rootScope.loading = false;
            //if(response.info.errorcode==0) $scope.PDFUrl='data:application/pdf;base64,'+response.ResultPdf;
            $scope.titleStatus = status;
            if (response.info.errorcode == 0) $scope.PDFUrl = response.Result;
            else { $scope.PDFUrl = ""; AlertMessages.alertPopup(response.info); }
        });
    }
    $scope.CallAPI = function(){
        PurchaseOrderFactory.APICallForRole({ 'RoleName':'POGlobalView' }).success(function (response) {
            if(response.result == 1) $scope.IsHavingRole = true;
            else $scope.IsHavingRole = false;
        })
    };$scope.CallAPI();
    $scope.setPOData = function (podata) {
        $scope.PurchaseOrderData = podata;
    }
    $scope.clonepuchaseorder = function (poid) {
        PurchaseOrderFactory.clonepo({ 'HeaderID': poid, 'LastModifiedBy': $scope.UserDetails.currentUser.UserId }).success(function (response) {
            if (response.info.errorcode == '0') { $('#ClonePopup').modal('hide'); $('.modal-backdrop').hide(); $state.reload(); }
            AlertMessages.alertPopup(response.info);
        });
    }
	$scope.redirectToDetails = function (headerUniqueid) {
        $sessionStorage.POHeaderStructureID = headerUniqueid;
        $sessionStorage.BackLink = "SearchPO";
        $sessionStorage.POUpdateDefaultTabNo = 1;
        $location.path("/DetailPO");
    }
    $scope.finalresultGrid = {
        data: 'TempCustList',
        showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 30,
        rowTemplate: '<div style="height: 100%" ng-class=""><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
        '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>' +
        '<div ng-cell></div>' +
        '</div></div>',
        columnDefs: [{
            field: 'ProjectCodes',
            width: 90,
            displayName: 'Project',
            cellTemplate: '<p class="gridCellItem text-center">{{row.entity.ProjectCodes}}</p>'
            //cellTemplate: '<p class="gridCellItem text-center">{{row.entity.ProjectCodes}}-{{row.entity.ProjectShortName}}</p>'
        },{
            field: 'WbsHeads',
            displayName: 'WBS Category'
        },{
            field: '',
            displayName: 'PO Number',
            cellTemplate: '<p class="gridCellItem text-center">{{row.entity.Purchasing_Order_Number?row.entity.Purchasing_Order_Number:\'-\'}}</p>'
        },{
            field: 'PoTitle',
            displayName: 'Title',
            width: 100
        },{ 
            field: '',
            displayName: 'Creation Date',
            width: 100, 
            cellTemplate: '<div class="optionsGrid">{{row.entity.Purchasing_Document_Date ?(row.entity.Purchasing_Document_Date|date:"dd-MMM-yyyy"):\'NA\'}}</div>'
         },{
            field: 'POStatus',
            displayName: 'Status',
            width: 75
        },{
            field: 'VendorName',
            displayName: 'Vendor'
        }, {
            field: 'Amount',
            displayName: 'Amount',
            cellTemplate: '<div class="text-right numberRight">{{row.entity.Amount?(row.entity.Amount|number):\'-\'}}</div>',
            width:80
        },{
            field: 'SubmitterName',
            displayName: 'Submitter'
        }, {
            field: 'ManagerName',
            displayName: 'PO Manager'
        },{
            field: '',
            displayName: 'PR Number',
            cellTemplate: '<p class="gridCellItem">{{row.entity.PurchaseRequestId?row.entity.PurchaseRequestId:\'-\'}}</p>',
            width: 75
        }, {
            field: 'HeaderUniqueid',
            displayName: 'Fugue ID',
             width: 70
        },{
            field: 'Version',
            displayName: 'Version',
             width: 60
        },{
            field: '',
            width: 120,
            displayName: 'Options',
            cellTemplate: '<div class="optionsGrid">\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFPagePopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);" title="PDF"  style="margin:3px;"><i class="fa fa-download"></i></a>\
		    <a href="javascript:void(0)" ng-show="IsHavingRole" title="PO View" ng-click="redirectToDetails(row.entity.HeaderUniqueid)"><i class="fa fa-television"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="infoPage(row.entity.Approvers);" title="Approvers" ng-if="row.entity.Approvers.length>0"><i class="fa fa-eye"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#viewVersionHistory" title="PO Version History" ng-click="poVersionDetails(row.entity.Purchasing_Order_Number);" style="margin:3px;"><i class="fa fa-database"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#ClonePopup" ng-click="setPOData(row.entity);" ng-show="row.entity.ReleaseCodeStatus==\'Approved\'" title="Revise PO" style="margin:3px;"><i class="fa fa-clone"></i></a>\
            </div>'
        }
        ]
    };
 // ng-if="(row.entity.POStatus==\'Draft\'&& row.entity.CreatedBy==UserDetails.currentUser.UserId)"   ng-show="row.entity.ReleaseCodeStatus==\'Approved\'"


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
        rowTemplate: '<div style="height: 100%" ng-class=""><div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell ">' +
        '<div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>' +
        '<div ng-cell></div>' +
        '</div></div>',
        columnDefs: [{
            field: 'SequenceNumber',
            displayName: '#',
            width:30
        }, {
            field: 'ApproverName',
            displayName: 'Approver Name'
        }, {
            field: 'Status',
            displayName: 'Status'
        },{
            field: 'Comments',
            displayName: 'Remarks'
        },{
            field: '',
            displayName: 'Date',
            cellTemplate: '<p class="gridCellItem ">{{row.entity.ApprovedOn ?(row.entity.ApprovedOn | date:"dd-MMM-yyyy"):\'NA\'}}</p>',
            width:90
        }]
    };
})

