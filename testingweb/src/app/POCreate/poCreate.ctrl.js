(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('POCreateController', POCreateController)
        .config(['$httpProvider', function($httpProvider) {
            //Reset headers to avoid OPTIONS request (aka preflight)
            $httpProvider.defaults.headers.common = {};
            $httpProvider.defaults.headers.post = {};
            $httpProvider.defaults.headers.put = {};
            $httpProvider.defaults.headers.patch = {};
        }])
    /** @ngInject */
    function POCreateController(POCreateServices, commonUtilService, $window, $http, $uibModal, $uibModalStack, $scope, $rootScope) {
        var vm = this;

        //***********//Pusing Object//****************//
        // Item List Arrays
        vm.items = [];
        vm.checked = [];
        //vm.MaterialLists=[];
        vm.specLists = [];
        vm.personalDetails = [{
            'Condition': ""
        }];
        $rootScope.SelectedItemsList = [];
        $rootScope.specList = [];
        $rootScope.materialCodes = [];
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
        vm.addNew = function(personalDetail) {
            vm.personalDetails.push({
                'Condition': ""
            });
        };

        vm.remove = function() {
            var newDataList = [];
            vm.selectedAll = false;
            angular.forEach(vm.personalDetails, function(selected) {
                if (!selected.selected) {
                    newDataList.push(selected);
                }
            });
            vm.personalDetails = newDataList;
        };

        // Add a Item to the list
        vm.addItem = function() {
            vm.items.push({
                amount: vm.itemAmount,
                name: vm.itemName
            });
            // Clear input fields after push
            vm.itemAmount = "";
            vm.itemName = "";
        };
        // Add Item to Checked List and delete from Unchecked List
        vm.toggleChecked = function(index) {
            vm.checked.push(vm.items[index]);
            vm.items.splice(index, 1);
        };

        // Get Total Items
        // $scope.getTotalItems = function () {
        //     return $scope.items.length;
        // };

        // Get Total Checked Items
        // $scope.getTotalCheckedItems = function () {
        //     return $scope.checked.length;
        // };




        //var userInfo = commonUtilService.getProfile();


        getCustomers();

        function getCustomers() {

            POCreateServices.callServices("getCustomersList").then(
                function(response) {
                    if (response.data) {

                        vm.Customers = response.data;
                        // vm.isNoDataPresent = (vm.poList.length == 0);
                    }

                }
            );
        }
        getVendors();

        function getVendors() {
            POCreateServices.callServices("getVendorsList").then(
                function(response) {
                    if (response.data) {

                        vm.Vendors = response.data;
                        // vm.isNoDataPresent = (vm.Vendors.length == 0);
                    }

                }
            );
        }

        getFundCenters();

        function getFundCenters() {


            POCreateServices.callServices("getFundCentersList").then(
                function(response) {
                    if (response.data) {

                        vm.fundCeters = response.data;
                        // vm.isNoDataPresent = (vm.projects.length== 0);
                    }

                }
            );
        }
        vm.shippingDetails = function(ship) {
            POCreateServices.callServices('getShippingDetails', ship).then(function(response) {
                vm.shippingDet = response.data;
                alert(response.data);
            })
        }

        vm.POP = function() {
            vm.GetAllMaterials();
            $uibModal.open({
                templateUrl: 'CreateNotesPopup.html',
                size: 'xxlg',
                controller: 'POCreateController as vm'
            });

        }; //controller:'LeadDetCtrl',animation:true,
        vm.cancel = function() {
            $uibModalStack.close();
        };
        vm.getCompany = function(id) {
            var dd = parseInt(id);
            var obj = {
                Uniqueid: parseInt(id)
            }
            POCreateServices.callServices("getBilling", id).then(
                function(response) {
                    if (response.data) {
                        vm.billingResult = response.data;
                        if (vm.billingResult[0] != null)
                            vm.billingData = vm.billingResult[0];

                        if (vm.billingResult[1] != null)
                            vm.shippingData = vm.billingResult[1];

                        // vm.isNoDataPresent = (vm.company.length == 0);
                    }
                }
            );

            //     POCreateServices.callServices("getCompany",dd).then(
            //         function(response){
            //             if(response.data){
            //                 vm.company=response.data;
            //                 // vm.isNoDataPresent = (vm.company.length == 0);
            //             }

            //     }
            // );
            // POCreateServices.callServices("getProjectsList",dd).then(           
            //     function(response) {

            //         if (response.data) {
            //            // alert(response.data.length);                   
            //             vm.projects = response.data;
            //             // vm.isNoDataPresent = (vm.projects.length == 0);
            //         }

            //     }
            // );
        }
        vm.getFundCenter = function(id) {
            var dd = parseInt(id);
            var obj = {
                Uniqueid: parseInt(id)
            }
            POCreateServices.callServices("getFundCenter", dd).then(
                function(response) {
                    if (response.data) {
                        vm.fundCenter = response.data;
                        // vm.isNoDataPresent = (vm.fundCenter.length == 0);
                    }

                }
            );
        }
        vm.getVendor = function(id) {
            var dd = parseInt(id);
            var obj = {
                Uniqueid: parseInt(id)
            }
            POCreateServices.callServices("getVendor", dd).then(
                function(response) {
                    if (response.data) {
                        vm.vendor = response.data;
                        //  vm.isNoDataPresent = (vm.vendor.length==0);
                    }

                }
            );
        }
        vm.getProject = function(projectID) {
            var ID = parseInt(projectID);
            var obj = {
                Uniqueid: parseInt(projectID),
                // StateID:parseInt(obj1.StateID)
            }
            POCreateServices.callServices("getProject", projectID).then(
                function(response) {
                    if (response.data) {
                        vm.RESULT = response.data;
                        vm.project = vm.RESULT[0].Project;
                        vm.projectGSTIN = vm.RESULT[0].State;
                        //  vm.isNoDataPresent = (vm.project.length==0);
                    }

                }
            );
            POCreateServices.callServices("getProjectDetails", ID).then(
                function(response) {
                    if (response.data) {
                        vm.projectDetails = response.data;
                        //  vm.isNoDataPresent = (vm.projectDetails.length==0);
                    }

                }
            );
            // alert(obj1.StateID);
            //  POCreateServices.callServices("getGSTIN").then(
            //     function(response){
            //             if(response.data){
            //                 vm.projectGSTIN=response.data;
            //                  vm.isNoDataPresent = (vm.projectGSTIN.length==0);
            //             }
            //     }
            //  );
        }
        GetTermsConditons();

        function GetTermsConditons() {
            POCreateServices.callServices("getTermsConditions").then(
                function(response) {
                    if (response.data) {
                        vm.termsConditions = response.data;
                        //  vm.isNoDataPresent = (vm.vendor.length==0);
                    }

                }
            );
        }
        GetPOTypes();

        function GetPOTypes() {
            POCreateServices.callServices("getPOTypes").then(
                function(response) {
                    if (response.data) {
                        vm.poTypes = response.data;
                        //  vm.isNoDataPresent = (vm.vendor.length==0);
                    }

                }
            );
        }
        vm.GetAllMaterials = function() {
            POCreateServices.callServices("getAllMaterials").then(function(response) {
                if (response.data) {
                    vm.MaterialLists = response.data;
                }
            });
        }

        var materialCodes = [];
        vm.AddMaterial = function() {
            var data = JSON.stringify($('.matrials').serializeObject());
            var obj = $('.matrials').serializeObject();
            var newDataList = [];

            vm.selectedAll = false;
            angular.forEach(vm.MaterialLists.result, function(selected) {
                if (selected.selected) {
                    newDataList.push(selected);
                    materialCodes.push(selected.MaterialCode);
                }
            });
            vm.MaterialListse = newDataList;
            $scope.aced(vm.MaterialListse, materialCodes);
        };
        $scope.aced = function(MaterialListsea, materialCodes) {
            $rootScope.SelectedItemsList = MaterialListsea;
            $rootScope.materialCodes = materialCodes;
            console.log($rootScope.SelectedItemsList, $rootScope.materialCodes);
            vm.materialSpec($rootScope.materialCodes);
        }
        var specList = [];
        var cols = [];
        vm.materialSpec = function(obj) {

            vm.Serviced(obj).then(function(response) {
                vm.specList = response.data;
                _.each(vm.specList.result, function(value, key) {
                    _.each(value, function(value, key) {
                        if (angular.isArray(value))
                            cols.push(value);
                    })
                });
                console.log(cols);
                vm.specLists = cols;
                $rootScope.specLists = vm.specLists;
                

            })


        }
        vm.saveMaterial=function(){
            if($rootScope.SelectedItemsList.hsn_code!=null){
            vm.HSN_Code=$rootScope.SelectedItemsList.hsn_code;
        }else{
            vm.HSN_Code="a";
        }
            if($rootScope.SelectedItemsList.MaterialCode!=null){
            vm.Material_Number=$rootScope.SelectedItemsList.MaterialCode;
        }else{
            vm.Material_Number="a";
        }
            if($rootScope.SelectedItemsList.wbs_code!=null){
                vm.WBS_Element=$rootScope.SelectedItemsList.wbs_code;
        }else{
            vm.WBS_Element="a";
        }
        if($rootScope.SelectedItemsList.short_description!=null){
            vm.Short_Text=$rootScope.SelectedItemsList.short_description;
    }else{
        vm.Short_Text="a";
    } if($rootScope.SelectedItemsList.unit_of_measure!=null){
            vm.Unit_Measure=$rootScope.SelectedItemsList.unit_of_measure;
    }else{
        vm.Unit_Measure="a";
    }
          
            
            var obj={
                materials:$rootScope.SelectedItemsList,
                specifications:$rootScope.specLists,
                potype:vm.poType,
                paymentTerms:vm.Paymentterms,
                HSN_Code: vm.HSN_Code,
                Material_Number:vm.Material_Number,
                Short_Text: vm.Short_Text,
                Storage_Location:vm.shippingDet.StorageLocationCode,
                Unit_Measure:vm.Unit_Measure,
                Fund_Center:vm.fundCenter.FundCenter_Code,
                ShortText: vm.Short_Text,
                UnitMeasure:vm.Unit_Measure,
                WBS_Element: vm.WBS_Element,
                PaymentTerm:vm.PaymentTerm,
                Percentage:vm.Percentage,
                Remarks:vm.remarks,
                VendorCode:vm.vendor.Vendor_Code,
                VendorGSTIN:vm.vendor.GSTIN,
                CompanyCode:vm.companyID
            };
            vm.SaveService(obj).then(function(response){
                vm.message=response.data;
                alert(vm.message);
            });
        }
        vm.Serviced = function(obj) {
            return $http({
                method: 'POST',
                url: 'http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/TEPurchase_GetMaterialSpecifications',
                dataType: 'json',
                data: obj,
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function(data) {
                return data;
                console.log(specList);
            }, function(data) {
                console.log(data);
                return data;
            });
        }
        vm.SaveService = function(obj) {
            return $http({
                method: 'POST',
                url: 'http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/TEPurchase_SavePO',
                dataType: 'json',
                data: obj,
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function(data) {
                return data;
                console.log(data);
            }, function(data) {
                console.log(data);
                return data;
            });
        }
    }

})();