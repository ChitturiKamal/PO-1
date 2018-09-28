angular.module('TEPOApp')
    .factory('ApprovedPRFactory', function($sessionStorage, $http) {
        var result = {};
        result.GetPRList = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetAllApprovedPRDetailsByLoginId', data, heads); }
        result.POPDFDefaultView = function(data) { return $http.post($sessionStorage.purchaseorder_url_mvc+'/POGeneratePDF/PODefaultView', data, heads); }
        result.ManualExpireOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExpireManually',data,heads);}
        result.ManualExpirExtensionOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExtendExpiryDate',data,heads);}
        result.PRDetails = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPRDetailsById', data, heads); }
        result.PRMaterialdetails = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPurchaseItemsByPRId', data, heads); }
        result.POPDFDefaultView = function(data) { return $http.post($sessionStorage.purchaseorder_url_mvc+'/POGeneratePDF/PODefaultView', data, heads); }
        result.PoPrHistory = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPODetailsByPRId', data, heads); }
        result.PrStatusUpdate = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/UpdatePurchaseRequestPOStatus', data, heads); }
        result.deliveryScheduleList = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetDeliveryScheduleDetailsByPRId', data, heads); }
        return result;
    })