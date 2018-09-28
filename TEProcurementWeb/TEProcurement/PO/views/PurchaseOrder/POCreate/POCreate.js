'use strict';
angular.module('TEPOApp')
    .controller('POSaveController', function ($filter, $scope, $rootScope, $element, $sessionStorage, POSaveFactory, $uibModal, AlertMessages, $state, $location) {
        $scope.FundCenterID = 0;$scope.date=$filter('date')(new Date(), 'yyyy-MM-dd');
        $scope.ShowValidation=function(){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Save Basic Info before going to other tabs' });}
        $scope.getPOTypes=function(){POSaveFactory.getPOTypes().success(function (response){$scope.orderTypeList = response.result;});}
        $scope.getPOTypes();
        //Get and Select PO Manager Starts
        $scope.getMangersList = function (fndcenterId){POSaveFactory.getMangers({'FundCenterID':fndcenterId}).success(function (response){$scope.ManagersResult = response.result;});}
        $scope.setManagerData = function (a) {
            if(a){
                $scope.selectedManager=a;$("#ManagerDetailsPopup").modal("hide");
            }else AlertMessages.alertPopup({errorcode:'1',errormessage:'PO Manager is Manadatory'});
        }
        //Get and Select PO Manager Ends
        //Get and Select WBS Head Starts
        $scope.getshippingplantstorageInfo = function(projcode){POSaveFactory.getBilling({'ProjectCode':projcode}).success(function (response){$scope.billingData=response.BillingData;});}
        $scope.GetFundCenterByCodeCall = function (a) {
            POSaveFactory.GetFundCenterByCode({'SearchText':a}).success(function(response){
                if(response.result !=null){
                    $scope.FundCenterID=response.result.Uniqueid;
                    $scope.WBSProjectCOde=response.result.ProjectCode;
                    $scope.getshippingplantstorageInfo(response.result.ProjectCode);
                    $scope.getMangersList(response.result.Uniqueid);
                    POSaveFactory.getProjectClientInfo({'ProjectCode':response.result.ProjectCode }).success(function (response){$scope.ClientprojectInfo=response.result;});
                }
                else{$scope.FundCenterID="";$scope.WBSProjectCOde="";$scope.billingData={};$scope.ManagersResult=[];$scope.ClientprojectInfo={};}
            });
        }
        $scope.getFundCenters = function (a) {
            if(a.length>3){
                $scope.fundCentersList=[];
                POSaveFactory.getFundCentersList({"SearchText":a}).success(function (response){$scope.fundCentersList=response.result;});
                $scope.GetFundCenterByCodeCall(a);
            }else AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 4 charactors to Search' }); return false;
        }
        //Get and Select WBS Head Ends
        //Search & Select Vendors Starts
        $scope.pagenumberVD=1;$scope.pageprecountsVD=100;
        $scope.getVendors = function () {
            if($scope.pagenumberVD <= '0')$scope.pagenumberVD=1;
            $scope.VendorsList=[];
            var data = JSON.stringify($('.VenderDetailsForm').serializeObject());
            $rootScope.loading = true;
            POSaveFactory.getVendorsList(data).success(function(response){$scope.VendorsList=response.result;$rootScope.loading = false;});
        }
        $scope.pagenationVD = function(a, b) {
            var b = $("select[name='pageprecountsVD']").val();$scope.pagenumberVD=a;$scope.pageprecountsVD=b;
            setTimeout(() => {$scope.getVendors();}, 300);
        }
        $scope.pagenationVD_Clk= function(a, b) {
            if ("0" == b) var a1 = --a; else var a1 = ++a;
            "0" >= a1 ? (a1 = 0, $scope.pagenumberVD=1) : $scope.pagenumberVD=a1;
            var c = $("select[name='pageprecountsVD']").val();
            $scope.pagenumberVD=a1;$scope.pageprecountsVD=c;
            setTimeout(() => {$scope.getVendors();}, 300);
        }
        $scope.setVendorData = function (a) {
            if(a){
                $scope.selectedVendorDetails=a;$("#VenderDetailsPopup").modal("hide");
            }else AlertMessages.alertPopup({errorcode:'1',errormessage:'Vendor is Manadatory'});
        }
        $scope.getVendorsByReset= function(){
            $scope.f1="",$scope.f2="",$scope.f3="",$scope.f4="",$scope.f5="",$scope.f6="";
            setTimeout(() => {$scope.getVendors();}, 300);
        }
        //Search & Select Vendors Ends
        //Save PO Starts
        $scope.basicInfoSavePO = function () {
            var data = JSON.stringify($('.basicInformation').serializeObject());
            var validate = JSON.parse(data);
            if (validate.ProjectCode.trim()  == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Head is Mandatory' }); return false; }
            else if (validate.POManagerID.trim()  == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PO Manager is Mandatory' }); return false; }
            else if (validate.Vendor_OwnerName.trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor is Mandatory' }); return false; }
            else if (validate.PO_OrderTypeID.trim()  == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PO Type is Mandatory' }); return false; }
            else if (validate.PO_Title.trim()  == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PO Title is Mandatory' }); return false; }
            else if (validate.PODescription.trim()  == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Description is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                POSaveFactory.savePO(data).success(function (response) {
                    $rootScope.loading = false;
                    if (response.info.errorcode=='0'){
                        AlertMessages.alertPopup(response.info);
                        $sessionStorage.POHeaderStructureID = response.UniqueId;
                        $sessionStorage.POUpdateDefaultTabNo = 2;
                        $location.path('UpdatePO');
                    }else AlertMessages.alertPopup(response.info);
                });
            }
        }
        //Save PO Ends
    })
