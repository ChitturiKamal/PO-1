angular.module('TEPOApp')
    .factory('UpCommingPoFactory', function($sessionStorage, $http) {
        var result = {};
        result.PurchaseOrderByUser = function(data) { return $http.post('http://182.18.177.27/PO/api/PurchaseOrder/TEPurchaseHomeByUser', data, heads); }
        result.ManualExpireOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExpireManually',data,heads);}
	    result.ManualExpirExtensionOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExtendExpiryDate',data,heads);}
        return result;
    })