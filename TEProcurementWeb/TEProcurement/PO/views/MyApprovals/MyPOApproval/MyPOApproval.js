'use strict';
angular.module('TEPOApp').controller('MyPOApprovalCtrl', function ($scope, filterFilter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, MyPOApprovalFactory, $uibModal, $location, $state) {
    $rootScope.visibleSearch = true;
    $scope.CurrentDate = new Date();
    var x = [];
    $scope.count = 0;
    $scope.getPOApprovalList = function () {
        $rootScope.loading = true;
        MyPOApprovalFactory.getMyPOApprovals({ "UserId": $localStorage.globals.currentUser.UserId}).success(function (response) {
            $rootScope.loading = false;
            $scope.POApprovalInfo = response;
        });
    }
    $scope.getPOApprovalList();
    $scope.SendForApprovalPop = function (data) {
        $scope.currentApproveRequestData = data;
        $rootScope.PurchaseOrderNumber = data.Purchasing_Order_Number;
        $scope.UserDetails = $localStorage.globals;
        $scope.POUniqueId = data.HeaderUniqueid;
    };
    $scope.SendForApprovalBtn = function () {
        $rootScope.loading = true;
        var data = JSON.stringify($('.SendForApprovalForm').serializeObject());
        MyPOApprovalFactory.submitForApproval(data).success(function (response) {
            //$rootScope.ClosePopup();
            AlertMessages.alertPopup(response.info);
            $state.reload();
            $scope.hideScreen();
            $rootScope.loading = false;
        });
    };
    $scope.PendingForApprovalApproveBtn = function () {
        $rootScope.loading = true;
        var data = JSON.stringify($('.PendingForApprovalForm').serializeObject());
        MyPOApprovalFactory.PendingForPOApprove(data).success(function (response) {
            AlertMessages.alertPopup(response.info);
            $state.reload();
            $scope.hideScreen();
            $rootScope.loading = false;
        });
    };

    $scope.PendingForApprovalRejectBtn = function () {
        var data = JSON.stringify($('.PendingForApprovalForm').serializeObject());
        var validate = JSON.parse(data);
        if ((validate.SubmitterComments).trim() == '') {
            AlertMessages.alertPopup({
                errorcode: '1',
                errormessage: 'Remarks is Mandatory'
            });
            return false;
        } else {
            $rootScope.loading = true;
            MyPOApprovalFactory.PendingForPOReject(data).success(function (response) {
                AlertMessages.alertPopup(response.info);
                $('#PendingForApprovalPagePopUp').modal('hide');
                $scope.hideScreen();
                $state.reload();
                $rootScope.loading = false;
            });
        }
    };
    $scope.POWithDrawBtn = function () {
        var data = JSON.stringify($('.PendingForApprovalForm').serializeObject());
        var validate = JSON.parse(data);
        if ((validate.SubmitterComments).trim() == '') {
            AlertMessages.alertPopup({
                errorcode: '1',
                errormessage: 'Remarks is Mandatory'
            });
            return false;
        } else {
            $rootScope.loading = true;
            MyPOApprovalFactory.POWithDrawl(data).success(function (response) {
                AlertMessages.alertPopup(response.info);
                $('#PendingForApprovalPagePopUp').modal('hide');
                $scope.hideScreen();
                $state.reload();
                $rootScope.loading = false;
            });
        }
    };

    $scope.genPOPDF = function (HeaderUniqueid, status) {
        $rootScope.loading = true;
        MyPOApprovalFactory.POPDFDefaultView({ 'POID': HeaderUniqueid }).success(function (response) {
            $rootScope.loading = false;
            $scope.titleStatus = status;
            if (response.info.errorcode == 0) $scope.PDFUrl = response.Result;
            else { $scope.PDFUrl = ""; AlertMessages.alertPopup(response.info); }
        });
    }
    $scope.redirectToDetails = function (headerUniqueid) {
        $sessionStorage.POHeaderStructureID = headerUniqueid;
        $sessionStorage.BackLink = "MyApprovals";
        $sessionStorage.POUpdateDefaultTabNo = 1;
        $location.path("/DetailPO");
    }
    $scope.redirectToUpdate = function (headerUniqueid) {
        $sessionStorage.POHeaderStructureID = headerUniqueid;
        $location.path("/UpdatePO");
    }
    $scope.finalresultGrid = {
        data: 'POApprovalInfo.result',
        showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 30,
        columnDefs: [{
        field:'ProjectCodes',displayName:'Project',width:80},
        {field: 'WbsHeads',displayName: 'WBS Category'},
        {
        field: '',
        displayName: 'PO Number',
        cellTemplate: '<p class="gridCellItem text-center">{{row.entity.Purchasing_Order_Number?row.entity.Purchasing_Order_Number:\'-\'}}</p>'
        }, {
        field: 'PoTitle',
        displayName: 'Title',
        width: 100
        }, {
        field: '',
        displayName: 'Creation Date',
        width: 100,
        cellTemplate: '<div class="optionsGrid">{{row.entity.CreatedOn ?(row.entity.CreatedOn|date:"dd-MMM-yyyy"):\'NA\'}}</div>'
        }, {
        field: 'VendorName',
        displayName: 'Vendor', width: 200
        }, {
        field: 'Amount',
        displayName: 'Amount',
        cellTemplate: '<div class="text-right numberRight">{{row.entity.Amount?(row.entity.Amount|number):\'-\'}}</div>',
        width: 80
        }, {
        field: 'SubmitterName',
        displayName: 'Submitter'
        }, {
        field: 'ManagerName',
        displayName: 'PO Manager'
        }, {
        field: '',
        displayName: 'PR Number',
        cellTemplate: '<p class="gridCellItem">{{row.entity.PurchaseRequestId?row.entity.PurchaseRequestId:\'-\'}}</p>',
        width: 75
        }, {
        field: 'HeaderUniqueid',
        displayName: 'Fugue ID',
        width: 70
        }, {
        field: 'Version',
        displayName: 'Version',
        width: 60
        },
        {
            field: '',
            displayName: 'Options',
            width: 120,
            cellTemplate: '<div class="optionsGrid">\
            <a href="javascript:void(0)" title="PO View" ng-click="redirectToDetails(row.entity.HeaderUniqueid)"><i class="fa fa-television"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFPagePopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);SendForApprovalPop(row.entity);"  style="margin:3px;" title="PDF"><i class="fa fa-download"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFApprovalPopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);SendForApprovalPop(row.entity);"   style="margin:3px;" title="Send For Approval" ng-if="row.entity.POStatus==\'Draft\' &&                       row.entity.isCurrentApprover==true"><i class="fa fa-cog"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFApprovalPopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);SendForApprovalPop(row.entity);"   style="margin:3px;" title="Approve/Reject" ng-if="row.entity.POStatus==\'Pending For Approval\' &&           row.entity.isCurrentApprover==true"><i class="fa fa-cog"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="infoPage(row.entity.Approvers);"  style="margin:3px;" title="Approvers" ng-if="row.entity.Approvers.length>0"><i class="fa fa-eye"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#GLPagePopUp" ng-click="GLPage(row.entity.HeaderUniqueid);"  style="margin:3px;" title="GLupdate" ng-if="row.entity.POStatus==\'Pending For Approval\'"><i class="fa fa-pencil"></i></a>\
          </div>'
        }]
    };

    $scope.infoPage = function (Approvers) {
        $scope.appr = Approvers;
    }
    $scope.GLPage = function (POHeaderStructureID) {
        $rootScope.loading = true;
        MyPOApprovalFactory.GetPurchaseItemswithPOIdForGLupdate({ 'POHeaderStructureID': POHeaderStructureID }).success(function (response) {
            $rootScope.loading = false;
            $scope.GLupdate = response.result;
            $scope.LastmodifiedByName = $scope.UserDetails.currentUser.CallName;
        });
    }
    $scope.GLAccountDetails = function () {
        $rootScope.loading = true;
        MyPOApprovalFactory.GetGLAccountDetails().success(function (response) {
            $rootScope.loading = false;
            $scope.GLAccountResult = response.result;
        });
    }
    $scope.GLAccountDetails();
    $scope.GLRowNo = 0;
    $scope.setGLrowclicked = function (index) {
        $scope.GLRowNo = index;
    }
    $scope.GLAccountDetailsClick = function (key, data) {
        $scope.GLAccountbgColor = [];
        $scope.GLupdate[$scope.GLRowNo].GLAccountNo = data.GLAccountCode;
        $scope.GLAccountbgColor[key] = 'darkseagreen';
    }
    $scope.UpdateGLAccount = function () {
        var data = JSON.stringify($('.UpdateGLAccountForm').serializeObject());
        MyPOApprovalFactory.UpdateGLAccountDetails(data).success(function (response) {
            if (response.info.errorcode == '0') { $('#GLPagePopUp').modal('hide'); }
            AlertMessages.alertPopup(response.info);
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