(function() {
    'use strict';

    angular
        .module('procurementApp')
        .factory('POCreateServices', POCreateServices).config(function ($httpProvider) {
            $httpProvider.defaults.headers.common = {};
            $httpProvider.defaults.headers.post = {};
            $httpProvider.defaults.headers.put = {};
            $httpProvider.defaults.headers.patch = {};
          });

    /* @ngInject */
    /* function config() {}*/
    function POCreateServices($http, $window, commonUtilService) {
       // var TEComplaintsUrl = commonUtilService.getUrl('TEComplaintsUrl');
        var service = {
                callServices: callServices
            },
            urlItemMapper = {
                getCustomersList: ['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetAllCustomers', 'get',''],
                getVendorsList: ['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetAllVendors', 'get',''],
                getFundCentersList:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetAllFundCenters', 'get',''],
                getVendor:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetVendor/','get',''],
                getFundCenter :['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetFundCenter/','get',''],
                getCompany:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetCustomer/','get',''],
                getProjectsList:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetAllProjects/','get',''],
                getProject:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetProject/','get',''],
                getProjectDetails:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetProjectDetails/','get',''],
                getTermsConditions:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetGeneralTermsConditions','get',''],
                getPOTypes:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetPOTypes','get'],
                getBilling:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/billing/','get','config'],
                getShippingDetails:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/getShippingDetails/','get'],
                getAllMaterials:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/TEPurchase_GetAllMaterials','get'],
                getMaterialSec:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/TEPurchase_GetMaterialSpecifications','get']
            };
        return service;

        //////////////// 
        function callServices(api, id) {
            var url;
            if(id!=null&&id!=undefined)
             url = urlItemMapper[api][0]+id;
            else
             url= urlItemMapper[api][0];
            // if (urlItemMapper[api][1].toLowerCase() === 'get') {
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
                method: urlItemMapper[api][1],
                url: url,
                //dataType: 'json',
               // data: obj,
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
