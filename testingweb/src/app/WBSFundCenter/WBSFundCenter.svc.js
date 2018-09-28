(function() {
    'use strict';

    angular
        .module('procurementApp')
        .factory('WBSFundCenterServices', WBSFundCenterServices).config(function ($httpProvider) {
            $httpProvider.defaults.headers.common = {};
            $httpProvider.defaults.headers.post = {};
            $httpProvider.defaults.headers.put = {};
            $httpProvider.defaults.headers.patch = {};
            //$httpProvider.defaults.headers.options = {};
          });

    /* @ngInject */
    /* function config() {}*/
    
    function WBSFundCenterServices($http, $window, commonUtilService) {
    var createWBSCall = [];
    var MasterUrl = commonUtilService.getUrl('VendorMaster');
    var result = {
        getCategory:getCategory,
        getSubCategory:getSubCategory,
        getWBSCode:getWBSCode,
        getFundcenter:getFundcenter,
        createWBSFundcenter:createWBSFundcenter
    },
    createWBSFCCall = {
        createWBSCall: [MasterUrl+'/api/PurchaseOrder/Wbs_FundcenterMapping', 'post']
    };
    return result;

     function getCategory(){return $http.get(MasterUrl+'/api/PurchaseOrder/GetCategories');}

    function getSubCategory(data){return $http.get(MasterUrl+'/api/PurchaseOrder/GetSubCategories'+'/'+data);}

    function getWBSCode(c1,c2){return $http.get(MasterUrl+'/api/PurchaseOrder/GetWBSCodes'+'/'+c1+'/'+c2);}

    function getFundcenter(){return $http.get(MasterUrl+'/api/PurchaseOrder/GetAllFundCenters');}

    // function createVendorMaster(data){return $http.post(MasterUrl+'/api/poapi/Wbs_FundcenterMapping',data);}

    // result.createVendorMaster = function(data){return $http.post(MasterUrl+'/api/poapi/Wbs_FundcenterMapping',data);}
    function createWBSFundcenter(api, obj) {
        var url = createWBSFCCall[api][0];
        return $http({
            method: createWBSFCCall[api][1],
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