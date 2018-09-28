'use strict';
angular.module('TEPOApp').controller('initiatePO', function ($scope, filterFilter, $filter, $rootScope, $element, $localStorage, $sessionStorage, AlertMessages, InitiatePOFactory, $uibModal, $location) {
    $scope.pagenumber = 1;
    $scope.pageprecounts = 10; 

    $scope.getApprovedPRList = function () {
       
            $rootScope.loading = true;
            //$scope.UserDetails.currentUser.UserId
            InitiatePOFactory.PRDraftSearch({
                'LoginId': $localStorage.globals.currentUser.UserId,
                'pageNumber': $scope.pagenumber,
                'pagePerCount': $scope.pageprecounts
               
            }).success(function (response) {
                $rootScope.loading = false;
                $scope.CustList = response.result;
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
    $scope.redirectToDetails = function (headerUniqueid) {
        $sessionStorage.POHeaderStructureID = headerUniqueid;
        $sessionStorage.BackLink = "initiatePO";
        $sessionStorage.POUpdateDefaultTabNo = 1;
        $location.path("/PRDetail");
    }
    $scope.filterSearchList = function (search) {
        $scope.TempCustList = $filter('filter')($scope.CustList, search);
    }
    
    $scope.searchDraftGrid = {
        data: 'TempCustList',
        showGroupPanel: true,
        jqueryUIDraggable: true,
        enableColumnResize: true,
        enableRowSelection: false,
        rowHeight: 30,
       
        columnDefs: [{
            field: 'PurchaseRequestId',
            displayName: 'PR Number',
            width: 90
        },{
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
            field: 'POStatus',
            displayName: 'Status'
        },
      {
            field: '',
            width:80,
            displayName: 'Options',
            cellTemplate:'<div class="optionsGrid">\
            <a href="javascript:void(0)" title="PO View" ng-click="redirectToDetails(row.entity.HeaderUniqueid)"><i class="fa fa-television"></i></a>\
            <a href="javascript:void(0)" data-toggle="modal" data-target="#infoPagePopUp" ng-click="getDetailsForView(row.entity.PurchaseRequestId)" title="View" style="margin:3px;"><i class="fa fa-eye"></i></a>\
            <a data-toggle="modal" data-target="#PRStatusPopUp" ng-click="loadDataToUpdate(row.entity.PurchaseRequestId)"><span class="glyphicon glyphicon-edit" aria-hidden="true" title="Update PR Status" style="font-size:12pt;margin: 2px"></span></a>\
            </div>'
        }
        ]
    };
    $scope.loadDataToUpdate = function(PurchaseRequestId){
        $rootScope.PurchaseRequestIdToUPdate = PurchaseRequestId;
    }
    $scope.updatePRStatus = function(){
        
        
                if($scope.PrStatus ==""||!$scope.PrStatus){
                    console.log($scope.PrStatus);
                }else{
                    $rootScope.loading = true;    
                    InitiatePOFactory.PrStatusUpdate({ 'PurchaseRequestId':  $rootScope.PurchaseRequestIdToUPdate,"PRPoStatus":$scope.PrStatus,"LastModifiedByID":$scope.UserDetails.currentUser.UserId }).success(function (response) {      
                        $rootScope.loading = false;    
                        $scope.getApprovedPRList();
                        $('#PRStatusPopUp').modal('hide');
                        AlertMessages.alertPopup(response.info);         
                    });
                }
            }
    $scope.filterSearchList = function (search) {
        $scope.TempCustList = $filter('filter')($scope.CustList, search);
    }

   $scope.initiatePO = function(){

   
        var newDataList = [];
               
        $(".cmplibrary:checked").each(function () {
            newDataList.push(JSON.parse($(this).attr("data")));
          //  materialCodes.push($(this).attr("materialcodedata"));
        });
        if(newDataList.length >0){
        var dataToInitiatePo = {
            "PurchaseRequestId":$scope.prdetails.PurchaseRequestId,
            "FundCenterId":$scope.prdetails.FundCenterId,
            "PurchaseRequestTitle":$scope.prdetails.PurchaseRequestTitle,
            "PurchaseRequestDesc":$scope.prdetails.PurchaseRequestDesc,
            "PurchaseItemList":newDataList,
        }

        $rootScope.loading = true;
        InitiatePOFactory.initiatePO(dataToInitiatePo).success(function (response) {
          
            AlertMessages.alertPopup(response.info);
            $rootScope.loading = false;
            $('#infoPagePopUp').modal('hide');
        });
    }else{
        
        AlertMessages.alertPopup({
            "listcount": 0,
            "errorcode": 1,
            "errormessage": "Please Select atleast One item under Purchase Details",
            "fromrecords": 0,
            "torecords": 0,
            "totalrecords": 0,
            "pages": null
        });
    }
        
   }
    $scope.getDetailsForView = function(prid){
        $rootScope.loading = true;
        InitiatePOFactory.PRDetails({ 'PRId': prid }).success(function (response) {
          
            $scope.prdetails = response.result;
           
        });
        InitiatePOFactory.PRMaterialdetails({ 'PurchaseRequestId': prid }).success(function (response) {
            $rootScope.loading = false;
            $scope.prMaterialdetails = response.result;
        });
    };
})

