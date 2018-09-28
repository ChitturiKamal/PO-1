angular.module('TEPOApp')
    .factory('ApprovedPOFactory', function($sessionStorage, $http) {
        var result = {};
        //result.POApproved_Pagination = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/TEPurchaseApproved_Pagination', data, heads); }
        result.POApproved_Pagination = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/TEPOSearchAPI/GetSearchedMyPOs', data, heads); }
        result.POPDFDefaultView = function(data) { return $http.post($sessionStorage.purchaseorder_url_mvc+'/POGeneratePDF/PODefaultView', data, heads); }
        result.ManualExpireOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExpireManually',data,heads);}
	    result.ManualExpirExtensionOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExtendExpiryDate',data,heads);}
        result.clonepo = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/ClonePO', data, heads); }
        result.poVersionInfo = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetVersionHistoryByPONumber', data, heads); }
        result.ApprovalList = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/POApprovalList', data, heads); }
        
        return result;
    })