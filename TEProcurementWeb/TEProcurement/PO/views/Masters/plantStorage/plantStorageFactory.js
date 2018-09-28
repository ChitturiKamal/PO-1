angular.module('TEPOApp')
.factory('plantStorageFactory', function($sessionStorage, $http) {
    var result = {};
    result.plantStoragePagination = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PlantStorageDetailsAPI/GetPlantStorageDetails_Pagination', data, heads); }
    result.ProjectMaster = function(data) { return $http.post($sessionStorage.server_url + '/TEProjectAPI/GetAllTEProjects', data, heads); }
    result.CompanyMasterCall = function(data) { return $http.post($sessionStorage.server_url + '/TEProjectAPI/GetAllcompanies', data, heads); }
    result.getStates = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PlantStorageDetailsAPI/GetStates', data, heads); }  
    result.getCountryList = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PlantStorageDetailsAPI/GetCountries', data, heads); }
    result.getOrderTypeCall = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetPOTypes', data, heads); }
    result.getFundCenterCall = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetAllFundCenters', data, heads); }
    result.getSubmitterCall = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetAllUsers', data, heads); }
    result.savePlantStorage = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PlantStorageDetailsAPI/SavePlantStorageDetails', data, heads); }
    result.getPlantStorageById = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PlantStorageDetailsAPI/GetPlantStorageDetailsByID', data, heads); }
    result.updatePlantStorage = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PlantStorageDetailsAPI/UpdatePlantStorageDetails', data, heads); }
    result.deletePlantStorage = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PlantStorageDetailsAPI/DeletePlantStorageDetails', data, heads); }
    return result;
})