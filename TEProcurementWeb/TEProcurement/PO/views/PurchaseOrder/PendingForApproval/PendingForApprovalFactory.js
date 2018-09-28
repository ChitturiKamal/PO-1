angular.module('TEPOApp')
    .factory('POPendingForApprovalFactory', function($sessionStorage, $http) {
        var result = {};
        result.POPendingForApproval_Pagination = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/TEPurchasePendingForApproval_Pagination', data, heads); }
        result.POPDFDefaultView = function(data) { return $http.post($sessionStorage.purchaseorder_url_mvc+'/POGeneratePDF/PODefaultView', data, heads); }
        result.PendingForPOApprove = function(data) {  return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/POApprove', data, heads); }
        result.PendingForPOReject = function(data) {  return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/POReject', data, heads); }
        result.ManualExpireOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExpireManually',data,heads);}
	    result.ManualExpirExtensionOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExtendExpiryDate',data,heads);}
        return result;
    })