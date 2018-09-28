'use strict';
angular.module('TEPOApp').controller('DraftPOCtrl', function ($scope, filterFilter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, DraftPOFactory, $uibModal, $location, $state) {
    $rootScope.visibleSearch = true;
    $scope.TempCustList;
    $scope.CurrentDate = new Date();
    var x = [];
    $scope.count = 0;
    $scope.pagenumber = 1;
    $scope.pageprecounts = 10;
    $scope.GetallList = function () {
        $rootScope.loading = true;
        if ($scope.pagenumber <= '0') $scope.pagenumber = 1;
        DraftPOFactory.PODraftList_Pagination({
            'UserId': $localStorage.globals.currentUser.UserId,
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
    $scope.searchDraftPO = function (search) {
        $scope.SearchText = search
        $scope.GetallList();
    }
    $scope.getpoDraftList = function () {
        $rootScope.loading = true;
        DraftPOFactory.PODraftList_Pagination({ "UserId": $localStorage.globals.currentUser.UserId, "FilterBy": $scope.SearchText, "pagePerCount": 10, "pageNumber": 1 }).success(function (response) {
            $rootScope.loading = false;
            $scope.CustList = response.result;
            $scope.info = response.info;
            $scope.TempCustList = $scope.CustList;
        });
    }
    $scope.getpoDraftList();
    $scope.filterSearchList = function (search) {
        $scope.count = 1;
        if (x.length <= $scope.CustList.length) {
            x = $scope.CustList;
        }
        $scope.TempCustList = filterFilter(x, search);
    }

    $scope.SendForApprovalPop = function (data) {
        $scope.currentApproveRequestData=data;
        $rootScope.PurchaseOrderNumber = data.Purchasing_Order_Number;
        $scope.UserDetails = $localStorage.globals;
        $scope.POUniqueId = data.HeaderUniqueid;

        // var modalInstance = $uibModal.open({
        //     animation: true,
        //     templateUrl: 'SendForApprovalPage.html',
        //     controller: 'PoApprovalCtrlUnits',
        //     size: 'md',
        // });
    };
    $scope.SendForApprovalBtn = function () {
        var data = JSON.stringify($('.SendForApprovalForm').serializeObject());
        var validate = JSON.parse(data);
        if ((validate.SubmitterComments).trim() == '') {
            AlertMessages.alertPopup({
                errorcode: '1',
                errormessage: 'Remarks is Mandatory'
            });
            return false;
        } else {
            //data.PurchaseOrderNumber = $rootScope.PurchaseOrderNumber;
             $rootScope.loading = true;
            DraftPOFactory.submitForApproval(data).success(function (response) {
                //$rootScope.ClosePopup();
                AlertMessages.alertPopup(response.info);
                $state.reload();
                $scope.hideScreen();
                $rootScope.loading = false;
            });
        }
    };

    $scope.PendingForApprovalApproveBtn = function () {
        $rootScope.loading = true;
        var data = JSON.stringify($('.PendingForApprovalForm').serializeObject());
        DraftPOFactory.PendingForPOApprove(data).success(function (response) {
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
            DraftPOFactory.PendingForPOReject(data).success(function (response) {
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
            DraftPOFactory.POWithDrawl(data).success(function (response) {
                AlertMessages.alertPopup(response.info);
                $('#PendingForApprovalPagePopUp').modal('hide');
                $scope.hideScreen();
                $state.reload();
                $rootScope.loading = false;
            });
        }
    };

    $scope.genPOPDF = function (HeaderUniqueid, status) {
        // $rootScope.loading = true;
        DraftPOFactory.POPDFDefaultView({ 'POID': HeaderUniqueid }).success(function (response) {
            $rootScope.loading = false;
            //if(response.info.errorcode==0) $scope.PDFUrl='data:application/pdf;base64,'+response.ResultPdf;
            $scope.titleStatus = status;
            if (response.info.errorcode == 0) $scope.PDFUrl = response.Result;
            else { $scope.PDFUrl = ""; AlertMessages.alertPopup(response.info); }
        });
    }

    $scope.redirectToDetails = function (headerUniqueid) {
        $sessionStorage.POHeaderStructureID = headerUniqueid;
        $sessionStorage.BackLink = "DraftPO";
        $sessionStorage.POUpdateDefaultTabNo = 1;
        $location.path("/DetailPO");
    }

    $scope.redirectToUpdate = function (headerUniqueid) {
        $sessionStorage.POHeaderStructureID = headerUniqueid;
        $sessionStorage.BackLink = "DraftPO";
        $sessionStorage.POUpdateDefaultTabNo = 1;
        $location.path("/UpdatePO");
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
        }, {
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
            displayName: 'Vendor',
        }, {
            field: 'Amount',
            displayName: 'Amount',
            cellTemplate: '<div class="text-right numberRight ">{{row.entity.Amount?(row.entity.Amount|number):\'-\'}}</div>',
            width: 80
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
        },{
            field: 'OrderType',
            displayName: 'Order Type',
            width: 90
        },{
            field: 'HeaderUniqueid',
            displayName: 'Fugue ID',
             width: 70
        },{
            field: 'Version',
            displayName: 'Version',
             width: 60
        },{
            field: '',
            displayName: 'Options',
            width: 120,
            cellTemplate: '<div class="optionsGrid">\
            <a href="javascript:void(0)" title="PO View" ng-click="redirectToDetails(row.entity.HeaderUniqueid)"><i class="fa fa-television"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFPagePopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);SendForApprovalPop(row.entity);"  style="margin:3px;" title="PDF"><i class="fa fa-download"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFApprovalPopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);SendForApprovalPop(row.entity);"   style="margin:3px;" title="Send For Approval" ng-if="row.entity.POStatus==\'Draft\' && row.entity.isCurrentApprover==true"><i class="fa fa-cog"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFApprovalPopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);SendForApprovalPop(row.entity);"   style="margin:3px;" title="Approve/Reject" ng-if="row.entity.POStatus==\'Pending For Approval\' && row.entity.isCurrentApprover==true"><i class="fa fa-cog"></i></a>\
            <a href="javascript:void(0)" ng-click="redirectToUpdate(row.entity.HeaderUniqueid)" ng-if="(row.entity.POStatus==\'Draft\'&& row.entity.CreatedBy==UserDetails.currentUser.UserId)"><i class="fa fa-pencil-square-o"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="infoPage(row.entity.Approvers);"  style="margin:3px;" title="Approvers" ng-if="row.entity.Approvers.length>0"><i class="fa fa-eye"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#WithDrawPagePopUp" ng-click="SendForApprovalPop(row.entity);"  style="margin:3px;" title="Withdraw"\
                ng-if="(row.entity.POStatus==\'Pending For Approval\'&& row.entity.CreatedBy==UserDetails.currentUser.UserId)"><i class="fa fa-undo"></i></a>\
           </div>'
        }

        // ng-if="!(row.entity.POStatus==\'Draft\'&& row.entity.CreatedBy==UserDetails.currentUser.UserId)"
        // ,{
        //     field: '',
        //     displayName: 'Options',
        //     width: 120,
        //     cellTemplate: '<div class="optionsGrid">\
        //     <a href="javascript:void(0)" data-toggle="modal" data-target="#genPOPDFPagePopUp" ng-click="genPOPDF(row.entity.HeaderUniqueid,row.entity.POStatus);SendForApprovalPop(row.entity);"  style="margin:3px;" title="PDF"><i class="fa fa-download"></i></a>\
        //     <a href="javascript:void(0)" data-toggle="modal" data-target="#SendForApprovalPagePopUp" ng-click="SendForApprovalPop(row.entity);"  style="margin:3px;" title="Send For Approval" ng-if="row.entity.POStatus==\'Draft\' && row.entity.isCurrentApprover==true"><i class="fa fa-cog"></i></a>\
        //     <a href="javascript:void(0)" data-toggle="modal" data-target="#PendingForApprovalPagePopUp" ng-click="SendForApprovalPop(row.entity);"  style="margin:3px;" title="Approve/Reject" ng-if="row.entity.POStatus==\'Pending For Approval\'&& row.entity.isCurrentApprover==true"><i class="fa fa-cog"></i></a>\
        //     <a href="javascript:void(0)" ng-click="redirectToUpdate(row.entity.HeaderUniqueid)" ng-if="(row.entity.POStatus==\'Draft\'&& row.entity.CreatedBy==UserDetails.currentUser.UserId)"><i class="fa fa-pencil-square-o"></i></a>\
        //     <a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="infoPage(row.entity.Approvers);"  style="margin:3px;" title="Approvers" ng-if="row.entity.Approvers.length>0"><i class="fa fa-eye"></i></a>\
        //     <a href="javascript:void(0)" data-toggle="modal" data-target="#GLPagePopUp" ng-click="GLPage(row.entity.Purchasing_Order_Number);"  style="margin:3px;" title="GLupdate" ng-if="row.entity.POStatus==\'Pending For Approval\'"><i class="fa fa-pencil"></i></a>\
        //     <a href="javascript:void(0)" data-toggle="modal" data-target="#WithDrawPagePopUp" ng-click="SendForApprovalPop(row.entity);"  style="margin:3px;" title="Withdraw"\
        //         ng-if="(row.entity.POStatus==\'Pending For Approval\'&& row.entity.CreatedBy==UserDetails.currentUser.UserId)"><i class="fa fa-undo"></i></a>\
        //    </div>'
        // }
        ]
    };


    $scope.infoPage = function (Approvers) {
        $scope.appr = Approvers;
    }
   
    $(document).on('show.bs.modal', '.modal', function() {
        var zIndex = 1040 + (10 * $('.modal:visible').length);
        $(this).css('z-index', zIndex);
        setTimeout(function() {
            $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
        }, 0);
    });

   
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
// .controller('PoApprovalCtrlUnits', ['$scope', '$rootScope', '$localStorage', '$sessionStorage',
// 	'DraftPOFactory', '$uibModal', 'AlertMessages', '$location', '$uibModalInstance', '$state',
// 	function($scope, $rootScope, $localStorage, $sessionStorage, DraftPOFactory, $uibModal, AlertMessages, $location, $uibModalInstance, $state) {
//         $scope.UserDetails = $localStorage.globals;
//         //alert($sessionStorage.selected_pkey);
//         //$scope.projectDet = $sessionStorage.portfolioMainList[$sessionStorage.selected_pkey];
//         $rootScope.ClosePopup = function() {
//             $uibModalInstance.dismiss('cancel');
//         };
//         $scope.SendForApprovalBtn = function() {
//             $rootScope.loading = true;
//             var data = JSON.stringify($('.SendForApprovalForm').serializeObject());
// 			//data.PurchaseOrderNumber = $rootScope.PurchaseOrderNumber;
//             DraftPOFactory.submitForApproval(data).success(function(response) {
//                 $rootScope.ClosePopup();
//                 AlertMessages.alertPopup(response.info);
//                 $state.reload();
//                 $rootScope.loading = false;
//             }); 
//         }
//     }])
