(function() {
    'use strict';

    angular
        .module('procurementApp')
        .factory('poMasterApproverServices', poMasterApproverServices).config(function ($httpProvider) {
            $httpProvider.defaults.headers.common = {};
            $httpProvider.defaults.headers.post = {};
            $httpProvider.defaults.headers.put = {};
            $httpProvider.defaults.headers.patch = {};
            //$httpProvider.defaults.headers.options = {};
          });

    /* @ngInject */
    /* function config() {}*/
    function poMasterApproverServices($http, $window, commonUtilService) {
        var masterApprover = [];
        var MasterUrl = commonUtilService.getUrl('VendorMaster');
        var result = {
            getAllApproverMaster:getAllApproverMaster,
            getOrderTypeCall:getOrderTypeCall,
            getFundCenterCall:getFundCenterCall,
            getSubmitterCall:getSubmitterCall,
            saveMasterApprover:saveMasterApprover
        },
        masterApproverCall = {
            masterApprover: [MasterUrl+'/api/purchaseorder/TEPurchase_MasterApprovers', 'post']
        };
        return result;api/PurchaseOrder/GetAllMasterAprovers
         //api/PurchaseOrder/GetAllMasterAprovers


        function getAllApproverMaster(){return $http.get(MasterUrl+'/api/purchaseorder/GetAllMasterAprovers/10/1');}
    
        function getOrderTypeCall(){return $http.get(MasterUrl+'/api/PurchaseOrder/GetPOTypes');}
    
        function getFundCenterCall(){return $http.get(MasterUrl+'/api/PurchaseOrder/GetAllFundCenters');}
    
        function getSubmitterCall(){return $http.get(MasterUrl+'/api/PurchaseOrder/GetAllUsers');}
    
        // function createVendorMaster(data){return $http.post(MasterUrl+'/api/poapi/Wbs_FundcenterMapping',data);}
    
        // result.createVendorMaster = function(data){return $http.post(MasterUrl+'/api/poapi/Wbs_FundcenterMapping',data);}
        function saveMasterApprover(api, obj) {
            var url = masterApproverCall[api][0];
            // if (createVendorCall[api][1].toLowerCase() === 'post') {
            //     if (angular.isDefined(json)) {
            //         var request = "";
            //         url += '?';
            //         $window._.forIn(json, function(value, key) {
            //             request += key + "=" + value + "&";
            //         });
            //         url += request.substring(0, request.length - 1);
            //     }
            // }
            return $http({
                method: masterApproverCall[api][1],
                url: url,
                //dataType: 'json',
                data: obj,
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function(data) {
                return data;
            }, function(data) {
                console.log(data);
                return data;
            });
        } 
        }
})();