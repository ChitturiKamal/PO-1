angular.module('TEPOApp')
.factory('PRUpdateFactory', function($sessionStorage,$http){
	var result={};
//	result.getList = function(data){return $http.post($sessionStorage.server_url+'/TEProjectRuleAPI/GetAllTEProjectRuleByProjectId_Pagination',data,heads);}
    
result.AddServiceHeaderitem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/AddServiceHeader',data,heads);}
result.UpdServiceHeaderitem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/UpdateServiceHeader',data,heads);}
result.DeletServiceHeader=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/DeleteServiceHeader',data,heads);}
result.GetPRServiceHeader=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetServiceHeadItemsbyHeadStructID',data,heads);}

result.searchbyMaterialGroups = function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchBy_Materials',data,heads);}
    result.UpdaatePr=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/UpdatePurchaseRequestBasicInfo',data,heads);}
    result.searchMaterialGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchGroups_Materials',data,heads);}
    result.searchWithinMaterialGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchWithinGroups_Materials',data,heads);}
    result.getProjectClientInfo=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetClientProjectInfoByProjectCode',data,heads);}
    result.vendorshippingInfo=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetVendorShippingInfo',data,heads);}
    result.vendorshipFromInfo=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetVendorShippingInfoById',data,heads);}
    result.searchServiceGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchGroups_Services',data,heads);}
    result.searchServiceByIDGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchBy_Services',data,heads);}
    result.getMangers=function(data){return $http.post($sessionStorage.purchaseorder_url+'/GLAccountAPI/GetManagersList',data,heads);}
    result.GetProjectTypes = function(data){return $http.post($sessionStorage.server_url+'/TEPickListItemItemAPI/GetPickListByNameNew',data,heads);}
    result.getCustomersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllCustomers',data,heads);}
    result.getVendorsList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllVendors',data,heads);}
    // result.getFundCentersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllFundCenters',data,heads);}
    result.getFundCentersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllFundCentersBySearch',data,heads);}
    result.getVendor=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetVendor',data,heads);}
    result.getFundCenter =function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetFundCenter',data,heads);}
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
    
    result.CopyCurrentLineItem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/CopyCurrentPRLineItem/',data,heads);}
    result.UpdatePurchaseDetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/UpdatePurchaseRequestDetails/',data,heads);}
    result.PRDetails = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPRDetailsById', data, heads); }
    result.PRMaterialdetails = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetPurchaseItemsByPRId', data, heads); }
    result.SavePurchaseDetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/SavePurchaseRequestDetails/',data,heads);}
    result.GerServiceDefinitionData=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetServiceClasificationDefinition',data,heads);}
    result.PRMaterialsDetailsDelete = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/DeletePRItemDetails', data, heads); }

    result.SaveDeliveryScheduleByPRItemId = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/SaveDeliveryScheduleByPRItemId', data, heads); }
    result.GetAllDeliverySchedulesByPRItemId = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/GetAllDeliverySchedulesByPRItemId', data, heads); }
    result.DeleteDeliverySchedule = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseRequest/DeleteDeliverySchedule', data, heads); }
    result.GetFundCenterByCode=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetFundCenterByCode',data,heads);}
	return result;
})


// (function() {
//     'use strict';

//     angular
//         .module('procurementApp')
//         .factory('POCreateServices', POCreateServices).config(function ($httpProvider) {
//             $httpProvider.defaults.headers.common = {};
//             $httpProvider.defaults.headers.post = {};
//             $httpProvider.defaults.headers.put = {};
//             $httpProvider.defaults.headers.patch = {};
//           });

//     /* @ngInject */
//     /* function config() {}*/
//     function POCreateServices($http, $window, commonUtilService) {
//        // var TEComplaintsUrl = commonUtilService.getUrl('TEComplaintsUrl');
//         var service = {
//                 callServices: callServices
//             },
//             urlItemMapper = {
//                 getCustomersList: ['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetAllCustomers', 'get',''],
//                 getVendorsList: ['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetAllVendors', 'get',''],
//                 getFundCentersList:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetAllFundCenters', 'get',''],
//                 getVendor:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetVendor/','get',''],
//                 getFundCenter :['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetFundCenter/','get',''],
//                 getCompany:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetCustomer/','get',''],
//                 getProjectsList:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetAllProjects/','get',''],
//                 getProject:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetProject/','get',''],
//                 getProjectDetails:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetProjectDetails/','get',''],
//                 getTermsConditions:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetGeneralTermsConditions','get',''],
//                 getPOTypes:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/GetPOTypes','get'],
//                 getBilling:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/billing/','get','config'],
//                 getShippingDetails:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/getShippingDetails/','get'],
//                 getAllMaterials:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/TEPurchase_GetAllMaterials','get'],
//                 getMaterialSec:['http://182.18.177.27/PurchaseOrder/api/PurchaseOrder/TEPurchase_GetMaterialSpecifications','get'],
//                 getpo:['http://localhost:50282/api/PurchaseOrder/GetPurchaseOrderByID/','get']
//             };
//         return service;

//         //////////////// 
//         function callServices(api, id) {
//             var url;
//             if(id!=null&&id!=undefined)
//              url = urlItemMapper[api][0]+id;
//             else
//              url= urlItemMapper[api][0];
//             // if (urlItemMapper[api][1].toLowerCase() === 'get') {
//             //     if (angular.isDefined(json)) {
//             //         var request = "";
//             //         url += '?';
//             //         $window._.forIn(json, function(value, key) {
//             //             request += key + "=" + value + "&";
//             //         });
//             //         url += request.substring(0, request.length - 1);
//             //     }
//             // }
//             return $http({
//                 method: urlItemMapper[api][1],
//                 url: url,
//                 //dataType: 'json',
//                // data: obj,
//                 headers: {
//                     'Content-Type': 'application/json'
//                 }
//             }).then(function(data) {
//                 return data;
//             }, function(data) {
//                 console.log(data);
//                 return data;
//             });
//         }
//     }
// })();
