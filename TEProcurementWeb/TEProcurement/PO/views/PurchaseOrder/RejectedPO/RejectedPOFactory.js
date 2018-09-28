angular.module('TEPOApp')
    .factory('RejectPoFactory', function($sessionStorage, $http) {
        var result = {};
        result.PurchaseOrderByUser = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/TEPurchaseHomeByUser', data, heads); }
        result.RejectPOWithDrawl = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/POWithDrawl', data, heads); }
        result.ManualExpireOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExpireManually',data,heads);}
	    result.ManualExpirExtensionOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExtendExpiryDate',data,heads);}
        return result;
    })