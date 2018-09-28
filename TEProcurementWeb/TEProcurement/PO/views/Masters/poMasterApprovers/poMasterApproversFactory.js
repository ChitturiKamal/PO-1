angular.module('TEPOApp')
.factory('poMasterApproversFactory', function($sessionStorage, $http) {
    var result = {};
    result.GetPoMasterApproverData = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetAllMasterAprovers', data, heads); }
    result.getOrderTypeCall = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetPOTypes', data, heads); }
    result.getFundCenterCall = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetAllFundCenters', data, heads); }
    result.getSubmitterCall = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetAllUsers', data, heads); }
    result.saveMasterApprover = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/SaveTEPurchaseMasterApprovers', data, heads); }
    result.geApproverById = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/GetAllMasterAprovers', data, heads); }
    result.updateMasterApprover = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/UpdateTEPurchaseMasterApprovers', data, heads); }
    result.updateMasterApprover_new = function (data) { return $http.post($sessionStorage.purchaseorder_url + '/PurchaseOrder/UpdateTEPurchaseMasterApprovers_new', data, heads); }

    return result;
})