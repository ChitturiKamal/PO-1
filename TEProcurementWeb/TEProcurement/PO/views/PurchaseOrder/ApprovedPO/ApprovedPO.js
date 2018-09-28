'use strict';
angular.module('TEPOApp').controller('ApprovedPO', function ($filter, $scope, filterFilter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, ApprovedPOFactory, $uibModal, $location) {
    $rootScope.visibleSearch = true;
    $scope.TempCustList;
    $scope.CurrentDate = new Date();
    var x = [];
    $scope.count = 0;
    $scope.pagenumber = 1;
    $scope.pageprecounts = 100;
    $scope.GetallList = function () {
        $rootScope.loading = true;
        if ($scope.pagenumber <= '0') $scope.pagenumber = 1;
        ApprovedPOFactory.POApproved_Pagination({'UserId': $localStorage.globals.currentUser.UserId,'FilterBy': $scope.SearchText,'pageNumber': $scope.pagenumber,'pagePerCount': $scope.pageprecounts}).success(function (response){
            $rootScope.loading = false;
            $scope.CustList = response.result;
            $scope.info = response.info;
            $scope.TempCustList = $scope.CustList;
        });
    }
    $scope.pagenationss = function (a, b) {
        var b = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a;
        $scope.pageprecounts = b;
        $scope.GetallList();
    }
    $scope.pagenationss_clk = function (a, b) {
        if ("0" == b) var a1 = --a;
        else var a1 = ++a;
        "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1;
        var c = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a1;
        $scope.pageprecounts = c;
        $scope.GetallList();
    }
    $scope.searchApprovedPO = function (search) {
        $scope.SearchText = search
        $scope.GetallList();
    }
    $scope.searchApprovedPO("");
    $scope.getPOList = function () {
        $rootScope.loading = true;
        ApprovedPOFactory.POApproved_Pagination({ "UserId": $localStorage.globals.currentUser.UserId, "SortBy": $scope.SearchText, "pagePerCount": 10, "pageNumber": 1 }).success(function (response) {
            $rootScope.loading = false;
            $scope.CustList = response.result;
            $scope.info = response.info;
            $scope.TempCustList = $scope.CustList;
        });
    }
    //$scope.getPOList();
    $scope.filterSearchList = function (search) {
        $scope.count = 1;
        if (x.length <= $scope.CustList.length) {
            x = $scope.CustList;
        }
        $scope.TempCustList = filterFilter(x, search);
    }

    $scope.genPOPDF = function (HeaderUniqueid, status) {
        $rootScope.loading = true;
        ApprovedPOFactory.POPDFDefaultView({ 'POID': HeaderUniqueid }).success(function (response) {
            $rootScope.loading = false;
            //if(response.info.errorcode==0) $scope.PDFUrl='data:application/pdf;base64,'+response.ResultPdf;
            $scope.titleStatus = status;
            if (response.info.errorcode == 0) $scope.PDFUrl = response.Result;
            else { $scope.PDFUrl = ""; AlertMessages.alertPopup(response.info); }
        });
    }

    $scope.clonepuchaseorder = function (poid) {
		$rootScope.loading = true;
        ApprovedPOFactory.clonepo({ 'HeaderID': poid, 'LastModifiedBy': $scope.UserDetails.currentUser.UserId }).success(function (response) {
			$rootScope.loading = false;
            if (response.info.errorcode == '0') { $('#ClonePopup').modal('hide'); $scope.getPOList(); }
            AlertMessages.alertPopup(response.info);
        });
    }
     $(document).on('show.bs.modal', '.modal', function() {
            var zIndex = 1040 + (10 * $('.modal:visible').length);
            $(this).css('z-index', zIndex);
            setTimeout(function() {
                $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
            }, 0);
        });
        
    $scope.setPOData = function (podata) {
        $scope.PurchaseOrderData = podata;
    }

    $scope.redirectToDetails = function (headerUniqueid) {
        $sessionStorage.POHeaderStructureID = headerUniqueid;
        $sessionStorage.BackLink = "MyPO";
        $sessionStorage.POUpdateDefaultTabNo = 1;
        $location.path("/DetailPO");
    }

    $scope.poVersionDetails = function (ponumber) {
        ApprovedPOFactory.poVersionInfo({ 'PONumber': ponumber, 'LastModifiedBy': $scope.UserDetails.currentUser.UserId }).success(function (response) {
            if (response.info.errorcode == '0') {
                $scope.POVersionResult = response.result;
            }
            else {
                AlertMessages.alertPopup(response.info);
            }
        });
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
            field: 'Purchasing_Document_Date',
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
        },{
            field: 'Amount',
            displayName: 'Amount',
           cellTemplate: '<div class="text-right numberRight">{{row.entity.Amount?(row.entity.Amount|number):\'-\'}}</div>',
            width: 80
        },{
            field: 'ManagerName',
            displayName: 'PO Manager'
        },{
            field: '',
            displayName: 'PR Number',
            cellTemplate: '<p class="gridCellItem">{{row.entity.PurchaseRequestId?row.entity.PurchaseRequestId:\'-\'}}</p>',
            width: 75
        },{
            field: 'HeaderUniqueid',
            displayName: 'Fugue ID',
             width: 70
        },{
            field: 'Version',
            displayName: 'Version',
             width: 60
        }, {
            field: '',
            displayName: 'Options',
            cellTemplate: '<div class="optionsGrid">\
            <a href="javascript:void(0)" title="PO View" ng-click="redirectToDetails(row.entity.HeaderUniqueid)"><i class="fa fa-television"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFPagePopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);" style="margin:3px;" title="PDF"><i class="fa fa-download"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="infoPage(row.entity);" title="Approvers" style="margin:3px;"><i class="fa fa-eye"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#ClonePopup" ng-click="setPOData(row.entity);" title="Revise PO" style="margin:3px;" ng-show="row.entity.isRevisionAllowed && row.entity.POStatus==\'Approved\' && (row.entity.CreatedBy==UserDetails.currentUser.UserId || row.entity.IsNewPO==false)"><i class="fa fa-clone"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#viewVersionHistory" title="PO Version History" ng-click="poVersionDetails(row.entity.Purchasing_Order_Number);" style="margin:3px;" ng-show="row.entity.POStatus==\'Approved\'"><i class="fa fa-database"></i></a>\
            </div>'
        }
        ]
    };
    // ng-hide="row.entity.isRevisionAllowed && row.entity.POStatus==\'Approved\' && (row.entity.CreatedBy==UserDetails.currentUser.UserId || row.entity.IsNewPO==false)"

    $scope.infoPage = function (a) {
        ApprovedPOFactory.ApprovalList({ 'UniqueID': a.HeaderUniqueid}).success(function (response) {
            if (response.info.errorcode == '0'){$scope.appr = response.result;}
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
    .filter('displayDate', function ($filter) {
        return function (time) {
            if (!time) return "";
            else { alert(time);
                var time = new Date(time);
                return $filter('date')(time, 'dd-MMM-yyyy');
            }
        }
    })

