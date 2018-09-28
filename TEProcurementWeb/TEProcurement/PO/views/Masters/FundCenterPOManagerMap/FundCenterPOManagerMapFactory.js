angular.module('TEPOApp')
.factory('FundCenterPOManagerFactory', function($sessionStorage, $http) {
    var result = {};
    result.getMangers=function(data){return $http.post($sessionStorage.purchaseorder_url+'/GLAccountAPI/GetManagersList',data,heads);}
    result.getFundCenterCall = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetAllFundCenters', data, heads); }
    result.getAllFundcenter_POManager_Mapping = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/FundCenterManagerMapping/getAllFundcenter_POManager_Mapping', data, heads); }
    result.getFundcenter_POManager_Mapping_ById = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/FundCenterManagerMapping/getFundcenter_POManager_Mapping_byid', data, heads); }
     result.Fundcenter_POManager_Mapping = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/FundCenterManagerMapping/Fundcenter_POManager_Mapping', data, heads); }
     result.updateFundcenter_POManager_Mapping = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/FundCenterManagerMapping/updateFundcenter_POManager_Mapping', data, heads); }
     result.deleteFundcenter_POManager_Mapping = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/FundCenterManagerMapping/DeleteFundcenter_POManager_Mapping', data, heads); }
    // result.getFundcenter_POManager_Mapping = function (data) { return $http.post($sessionStorage.po_url_mvc + '/FundCenterManagerMapping/getFundcenter_POManager_Mapping', data, heads); }
    // result.GetAllUsers = function (data) { return $http.post($sessionStorage.po_url_mvc + '/FundCenterManagerMapping/GetAllUsers', data, heads); }
    // result.GetAllgetFundcenter = function (data) { return $http.post($sessionStorage.po_url_mvc + '/FundCenterManagerMapping/GetAllgetFundcenter', data, heads); }
    return result;
})