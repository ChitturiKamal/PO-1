angular.module('TEPOApp')
.factory('WBSFundCenterFactory', function($sessionStorage, $http) {
    var result = {};
    result.getFundCenterlist = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/getWBSFundcenterMappingPagination', data, heads); }
    result.getFundCenterCode = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetAllFundCenters', data, heads); }
    result.getWBSById = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/getWbs_FundcenterMapping', data, heads); }
    result.getFCDesc = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetFundCenter', data, heads); }
    result.updateWBSFundCenter = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/updateWbs_FundcenterMapping', data, heads); }
    result.DeleteWbs_Fundcenter = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/DeleteWbs_FundcenterMapping', data, heads); }
   result.createWBSFundcenter =  function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/Wbs_FundcenterMapping', data, heads); }
    return result;
})