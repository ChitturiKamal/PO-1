angular.module('TEPOApp')
    .factory('PurchaseOrderFactory', function($sessionStorage, $http) {
        var result = {};
        result.POSearch_Pagination = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/TEPurchaseSearch_Pagination', data, heads); }
        result.POPDFDefaultView = function(data) { return $http.post($sessionStorage.purchaseorder_url_mvc+'/POGeneratePDF/PODefaultView', data, heads); }
        result.poVersionInfo = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetVersionHistoryByPONumber', data, heads); }
        result.APICallForRole = function(data) { return $http.post($sessionStorage.server_url_lead+'/TEContactAPI/CheckTheRoleByName', data, heads); }
        result.clonepo = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/ClonePO', data, heads); }
        return result;
    })