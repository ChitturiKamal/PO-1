(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('vendorMasterCtrl', vendorMasterCtrl);

    /* @ngInject */
    /* function config() {}*/
    function vendorMasterCtrl($http, $window,VendorMasterServices,AlertMessages,$rootScope, $state,$timeout,$scope) {
        var vm = this;
        vm.callData = function (){
            vm.vendorList = [];
              VendorMasterServices.getAllVendorMaster().then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        //this.isLoading = false;
                        vm.vendorList = vm.vendorList.concat(response.data);
                    }
                }
            );
        }
       vm. callData();

       vm.CountryDrop = function(){
        vm.CountryDropList = VendorMasterServices.CountryDropList();
       }
    vm.CountryDrop();

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

    // $rootScope.autoHide =function(){
    //     $timeout(function(){
    //         $rootScope.alerts.splice(0, 1);}, 3000);
    //     }

    vm.FromCreate = function() {
        var data = JSON.stringify($('.addForm').serializeObject());
        var obj = $('.addForm').serializeObject();
        var validate = JSON.parse(data);
         if ((validate.Vendor_Code).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Code is Mandatory' }); return false; }
        else if ((validate.Vendor_Owner).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Owner is Mandatory' }); return false; }
        else if ((validate.GSTIN).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GSTIN is Mandatory' }); return false; }
        else if ((validate.Vat).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vat is Mandatory' }); return false; }
        else if ((validate.ServiceTax).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Service Tax is Mandatory' }); return false; }
        else if ((validate.PanNumber).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Pan Number is Mandatory' }); return false; }
        else if ((validate.Country).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Country is Mandatory' }); return false; }
          {
            VendorMasterServices.createVendorMaster("createVendor",obj)
            .then(
                function(response) {
                    if (response.statusText == "OK") {
                         $('#addVendorMaster').modal('hide');
                         //vm. callData();
                       
                         AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Data successfully saved' });
                    }
                      //$state.reload();
                }
            );
        }
    }

    vm.FromView = function(id){
        VendorMasterServices.getVendorMasterById(id)
        .then(
            function(response) {
                if (response.statusText == "OK") {
                    vm.getVM = response.data;
                }
            }
        );
    }

    vm.TEClaimItems = [];
    vm.TEClaimItems1 = [];

    vm.Fromsave = function() {
        var data = JSON.stringify($('.updateForm').serializeObject());
        var obj = $('.updateForm').serializeObject();
        var validate = JSON.parse(data);
         if ((validate.Vendor_Code).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Code is Mandatory' }); return false; }
        else if ((validate.Vendor_Owner).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Owner is Mandatory' }); return false; }
        else if ((validate.GSTIN).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GSTIN is Mandatory' }); return false; }
        else if ((validate.Vat).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vat is Mandatory' }); return false; }
        else if ((validate.ServiceTax).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Service Tax is Mandatory' }); return false; }
        else if ((validate.PanNumber).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Pan Number is Mandatory' }); return false; }
        else if ((validate.Country).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Country is Mandatory' }); return false; }
          {
            VendorMasterServices.saveVendorMaster("saveVendor",obj)
            .then(
                function(response) {
                    if (response.statusText == "OK") {
                         $('#editVendorMaster').modal('hide');
                         //vm. callData();
                       
                         AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Data successfully Updated' });
                    }
                      //$state.reload();
                }
            );
        }
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
