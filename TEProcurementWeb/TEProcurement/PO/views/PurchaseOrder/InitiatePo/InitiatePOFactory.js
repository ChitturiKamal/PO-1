angular.module('TEPOApp')
    .factory('InitiatePOFactory', function($sessionStorage, $http) {
        var result = {};
        result.PRDraftSearch = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetAllApprovedPRsForInitiatePO', data, heads); }
       
        result.PRDetails = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPRDetailsById', data, heads); }
        result.PRMaterialdetails = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPurchaseItemsByPRIdForInitiatePO', data, heads); }
        result.initiatePO = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/InitiatePOFromPR', data, heads); }
        result.PrStatusUpdate = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/UpdatePurchaseRequestPOStatus', data, heads); }
        return result;
    })