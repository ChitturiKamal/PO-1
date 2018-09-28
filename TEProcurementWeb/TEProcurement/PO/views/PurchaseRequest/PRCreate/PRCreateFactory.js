angular.module('TEPOApp')
.factory('PRSaveFactory', function($sessionStorage,$http){
	var result={};
//	result.getList = function(data){return $http.post($sessionStorage.server_url+'/TEProjectRuleAPI/GetAllTEProjectRuleByProjectId_Pagination',data,heads);}
	

    result.savePO=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/SavePurchaseRequestBasicInfo',data,heads);}
    result.searchMaterialGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchGroups_Materials',data,heads);}
    result.searchWithinMaterialGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchWithinGroups_Materials',data,heads);}
    result.getProjectClientInfo=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetClientProjectInfoByProjectCode',data,heads);}
    result.vendorshippingInfo=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetVendorShippingInfo',data,heads);}
    result.vendorshipFromInfo=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetVendorShippingInfoById',data,heads);}
    result.searchServiceGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchGroups_Services',data,heads);}
    result.getMangers=function(data){return $http.post($sessionStorage.purchaseorder_url+'/GLAccountAPI/GetManagersList',data,heads);}
    result.GetProjectTypes = function(data){return $http.post($sessionStorage.server_url+'/TEPickListItemItemAPI/GetPickListByNameNew',data,heads);}
    result.getCustomersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllCustomers',data,heads);}
    result.getVendorsList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllVendors',data,heads);}
    // result.getFundCentersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllFundCenters',data,heads);}
    result.getVendor=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetVendor',data,heads);}
    // result.getFundCenter =function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetFundCenter',data,heads);}
    result.getFundCentersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllFundCentersBySearch',data,heads);}
    result.getCompany=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetCustomer/',data,heads);}
    result.getProjectsbycmpcode=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetProjectsByCompanyCode',data,heads);}
    result.getProject=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetProject/',data,heads);}
    result.getProjectDetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetProjectDetails/',data,heads);}
    result.getTermsConditions=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetGeneralTermsConditions',data,heads);}
    result.getPOTypes=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOTypes',data,heads);}
    result.getBilling=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/BillingandShippingInfoByCompCode',data,heads);}
    result.getShippingDetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/getShippingDetails',data,heads);}
    result.getAllMaterials=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/TEPurchase_GetAllMaterials',data,heads);}
    result.getMaterialSec=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/TEPurchase_GetMaterialSpecifications',data,heads);}
    result.getpo=function(data){return $http.post('http://localhost:50282/api/PurchaseOrder/GetPurchaseOrderByID/',data,heads);}
    result.searchWithinServiceGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchWithinGroups_Services',data,heads);}
    
    result.getWBSList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetWBSCodeDetails',data,heads);}

    result.SavePurchaseDetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/SavePurchaseRequestDetails/',data,heads);}

    result.GerServiceDefinitionData=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetServiceClasificationDefinition',data,heads);}
    result.PRMaterialdetails = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPurchaseItemsByPRId', data, heads); }
    result.SaveDeliveryScheduleByPRItemId = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/SaveDeliveryScheduleByPRItemId', data, heads); }
    result.GetAllDeliverySchedulesByPRItemId = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetAllDeliverySchedulesByPRItemId', data, heads); }
    result.DeleteDeliverySchedule = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/DeleteDeliverySchedule', data, heads); }
    result.GetFundCenterByCode=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetFundCenterByCode',data,heads);}
	return result;
})
