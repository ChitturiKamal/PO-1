angular.module('TEPOApp')
    .factory('PurchaseRequestFactory', function($sessionStorage, $http) {
        var result = {};
        result.PRSearch = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetAllPRDetails', data, heads); }
        result.POPDFDefaultView = function(data) { return $http.post($sessionStorage.purchaseorder_url_mvc+'/POGeneratePDF/PODefaultView', data, heads); }
        result.ManualExpireOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExpireManually',data,heads);}
        result.ManualExpirExtensionOfferCall = function(data){return $http.post($sessionStorage.server_url+'/OfferManualExpiry/ExtendExpiryDate',data,heads);}
        result.PRDetails = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPRDetailsById', data, heads); }
        result.PRMaterialdetails = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPurchaseItemsByPRId', data, heads); }
        result.deliveryScheduleList = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetDeliveryScheduleDetailsByPRId', data, heads); }
        result.TEPRSearchForSearchTab = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/TEPRSearchForSearchTab', data, heads); }
        return result;
    })