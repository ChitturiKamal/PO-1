angular.module('TEPOApp')
.factory('HSNTaxCodeMapFactory', function($sessionStorage, $http) {
    var result = {};

    //result.GetHSNTaxRate = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/POHSNRateAPI/GetHSNTaxRate', data, heads); }
    result.GetHSNTaxRate = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/TEPOSearchAPI/GetHSNSearched', data, heads); }
    result.getCountryList = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PlantStorageDetailsAPI/GetCountries', data, heads); }
    result.getStates = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PlantStorageDetailsAPI/GetStates', data, heads); }  
    result.getHSNCodeList = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/POHSNRateAPI/GetHSNCode', data, heads); }  
    result.getMatGSTList = function(data){return $http.post($sessionStorage.purchaseorder_url + '/POHSNRateAPI/GetMatGstAppl', data, heads); }  
    result.getHSNCodeById = function(data){return $http.post($sessionStorage.purchaseorder_url + '/POHSNRateAPI/GettHSNCodeByID', data, heads); }  
    result.GetVenCatMast = function(data){return $http.post($sessionStorage.purchaseorder_url + '/POHSNRateAPI/GetVenCatMast', data, heads); }  
    result.SaveHSNCodeDetails = function(data){return $http.post($sessionStorage.purchaseorder_url + '/POHSNRateAPI/SaveHSNCodeDetails', data, heads); }  
    result.UpdateHSNCodeDetails = function(data){return $http.post($sessionStorage.purchaseorder_url + '/POHSNRateAPI/UpdateHSNCodeDetails', data, heads); }  
    result.DeleteHSNCodeDetails = function(data){return $http.post($sessionStorage.purchaseorder_url + '/POHSNRateAPI/DeleteHSNCodeDetails', data, heads); }  
    result.GetMatAppl = function(data){return $http.post($sessionStorage.purchaseorder_url + '/POHSNRateAPI/GetMatAppl', data, heads); }  



    return result;
})