angular.module('TEPOApp')
.factory('POSaveFactory', function($sessionStorage,$http){
	var result={};
    result.getPOTypes=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOTypes',data,heads);}
    result.getMangers=function(data){return $http.post($sessionStorage.purchaseorder_url+'/GLAccountAPI/GetManagersByFundCenterID',data,heads);}
    result.getBilling=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/BillingandShippingInfoByCompCode',data,heads);}
    result.GetFundCenterByCode=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetFundCenterByCode',data,heads);}
    result.getProjectClientInfo=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetClientProjectInfoByProjectCode',data,heads);}
    result.getFundCentersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllFundCentersBySearch',data,heads);}
    result.getVendorsList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/VendorShippingDetailsAPI/GetSearchedVendors',data,heads);}
    result.savePO=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SavePO',data,heads);}
	return result;
})