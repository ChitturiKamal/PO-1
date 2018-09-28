(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('WBSFundCenterCtrl', WBSFundCenterCtrl)
    /* @ngInject */
    /* function config() {}*/
    function WBSFundCenterCtrl($http, $window,WBSFundCenterServices,AlertMessages,$rootScope,$timeout) {
        var vm = this;
    //     vm.callData = function (){
    //         vm.vendorList = [];
    //           VendorMasterServices.getAllVendorMaster().then(
    //             function(response) {
    //                 if (response.data && response.data.length > 0) {
    //                     vm.vendorList = vm.vendorList.concat(response.data);
    //                 }
    //             }
    //         );
    //     }
    //    vm. callData();

    //    vm.CountryDrop = function(){
    //     vm.CountryDropList = VendorMasterServices.CountryDropList();
    //    }
    // vm.CountryDrop();
    
    $rootScope.autoHide =function(){
        $timeout(function(){
            $rootScope.alerts.splice(0, 1);}, 3000);
        }

        WBSFundCenterServices.getCategory().then(function(response){
            vm.Categories = response.data;
        })
        
        vm.getSubCategoryCall = function(id){
            if(id){
                WBSFundCenterServices.getSubCategory(id).then(function(response){
                    if(response){
                        vm.getSubCategory = response.data;
                    }else{vm.getSubCategory=[];}
                })
            }else{vm.getSubCategory=[];}
        }

        vm.getWBSCodeCall = function(cat,subCat){
            alert(cat+ ' '+ subCat);
            if(cat != null && subCat != null){
                WBSFundCenterServices.getWBSCode(cat,subCat).then(function(response){
                    vm.getWBSCode = response.data;
                })
            }else{vm.vm.getWBSCode=[];}
        }

        WBSFundCenterServices.getFundcenter().then(function(response){
            vm.getFundCenterCode = response.data;
        })

        vm.getWBSID = function(WBSid){
            angular.forEach(vm.getWBSCode, function(value, key) {
                if(value.WBSID == WBSid){
                    vm.NAME = value.name;
                    vm.WBSCODE = value.WBSCODE
                }
              });
        }

        vm.getFCCode = function(FCid){
            angular.forEach(vm.getFundCenterCode, function(value, key) {
                if(value.Uniqueid == FCid){
                    vm.FundCentreCode = value.FundCenter_Code;
                    vm.FundCenterDescription = value.FundCenter_Description;
                }
              });
        }
        
     vm.FromSave = function() {
         var data = JSON.stringify($('.addForm').serializeObject());
         //var obj = $('.addForm').serializeObject();
         var validate = JSON.parse(data);
         if ((validate.CATEGORY).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Category is Mandatory' }); return false; }
        else if ((validate.SUBCATEGORY).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Sub category is Mandatory' }); return false; }
        else if ((validate.WBSID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Code is Mandatory' }); return false; }
        else if ((validate.WBSName).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Name is Mandatory' }); return false; }
        else if ((validate.FundCentreID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Centre Code is Mandatory' }); return false; }
        else if ((validate.FundCenterDescription).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center Description is Mandatory' }); return false; }
       var obj = {'WBSID':validate.WBSID,'WBSCODE':validate.WBSCODE,'FundCentreID':validate.FundCentreID,'FundCentreCode':validate.FundCentreCode,'LastModifiedBy':validate.LastModifiedBy}
         {
            WBSFundCenterServices.createWBSFundcenter('createWBSCall',obj)
            .then(
                function(response) {
                    if (response.statusText == "OK") {
                        //  $('#addVendorMaster').modal('hide');
                        //  vm. callData();
                         //$state.reload();
                         AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Data successfully saved' });
                    }
                }
            );
    //         // .success(function(response) {
    //         //     if (response.info.errorcode == '0'){
    //         //       $('#addSalesPlan').modal('hide');
    //         //       setTimeout(function() {$state.reload();}, 300);
    //         //     }
    //         //     AlertMessages.alertPopup(response.info);
    //         // });
           
         }
     }

     $.fn.serializeObject = function() {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function() {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $rootScope.autoHide =function(){
        $timeout(function(){
            $rootScope.alerts.splice(0, 1);}, 3000);
        }

    // vm.vendorApprove = function() {
    //     // $rootScope.ApprovalPrimaryID = a;
    //     var modalInstance = $uibModal.open({
    //         animation: true,
    //         templateUrl: 'SendForApprovalPage.html',
    //        // controller: 'venderMasterPopupCtrl',
    //     });
    // };

    }



})();
