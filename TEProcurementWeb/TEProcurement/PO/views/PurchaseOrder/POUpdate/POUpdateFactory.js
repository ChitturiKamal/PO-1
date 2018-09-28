angular.module('TEPOApp')
.factory('POUpdateFactoryServices', function($sessionStorage,$http){
	var result={};
    result.GetPurchaseRateHistoryByItemCode=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPurchaseRateHistoryByItemCode',data,heads);}
    result.updatePO=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/UpdatePO',data,heads);}
    //Material and Service
    result.searchMaterialGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchGroups_Materials',data,heads);}
    result.searchServiceGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchGroups_Services',data,heads);}
    result.searchServiceByIDGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchBy_Services',data,heads);}

    result.searchWithinMaterialGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchWithinGroups_Materials',data,heads);}
    result.searchWithinServiceGroups=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchWithinGroups_Services',data,heads);}
    result.getPODetailsById=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPODetailsById',data,heads);}
    result.getProjectsbycmpcode=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetProjectsByCompanyCode',data,heads);}
    result.getProjectClientInfo=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetClientProjectInfoByProjectCode',data,heads);}
    result.GerServiceDefinitionData=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetServiceClasificationDefinition',data,heads);}
    result.getMaterialAnnexureCheckListData=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOAnnexureSpecificationsByHeaderStructureId',data,heads);}
    result.getServiceAnnexureCheckListData=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOServiceAnnexureByHeaderStructureId',data,heads);}
    result.getAllMaterials=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/TEPurchase_GetAllMaterials',data,heads);}
    result.getMaterialSec=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/TEPurchase_GetMaterialSpecifications',data,heads);}
    //Special T & C Tab APIS
    result.saveupdateSpecialTandC=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SaveorUpdatePOSpecialTermsandConditions',data,heads);}
    result.deleteSpecialTandC=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/DeletePOSpecialTermsandConditions',data,heads);}
    result.getSpecialTandC=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOSpecialTandC',data,heads);}
    //Payment Terms Tab APIS
    //result.saveupdatePoPaymentTerm=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SaveorUpdatePaymentTerm',data,heads);}
    result.saveupdatePoPaymentTerm=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SaveorUpdatePaymentTermList',data,heads);}

    result.DeletePaymentTerm=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/DeletePaymentTerm',data,heads);}
    result.getPOPamentTerms=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOPaymentTermsById',data,heads);}
    //Purchase Details Tab APIS
    result.searchbyMaterialGroups = function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SearchBy_Materials',data,heads);}
    result.savePurchaseItem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SavePurchaseItem',data,heads);}
    result.copyCurrentPurchaseItem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/CopyCurrentPurchaseItem',data,heads);}
    result.updatePurchaseItem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/UpdatePurchaseItems',data,heads);}
    result.AddServiceHeaderitem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/AddServiceHeader',data,heads);}
    result.UpdServiceHeaderitem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/UpdateServiceHeader',data,heads);}
    result.delServiceHeaderitem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/DeleteServiceHeader',data,heads);}
    result.DeletePurchaseItem=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/DeletePurchaseItemStructure',data,heads);}
    result.multiplepurchaseSave=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SaveMultiplePurchaseItems',data,heads);}
    result.saveExpenseOrderstems=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SaveMultipleExpensePurchaseItems',data,heads);}
    result.GetPurchaseItemsPOId=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPurchaseItemsByPOHeaderId',data,heads);}
    // result.GetServiceHeadItemsList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetWorkServiceHeadItemsbyHeadStructID',data,heads);}
    result.GetServiceHeadItemsList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetServiceHeadItemsbyHeadStructID',data,heads);}
    result.getGLAccDetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetGLCodeDetails',data,heads);}
    //result.getWBSList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetWBSCodeDetails',data,heads);}
    result.getWBSList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/VendorShippingDetailsAPI/GetSearchedWBSCode',data,heads);}

    //HSN Calls
    result.getHSNList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/VendorShippingDetailsAPI/GetHSNDetailsForExpenseOrder',data,heads);}
    //HSN Calls
    

    //result.gettaxdetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetTaxDetailsForOrderItem',data,heads);}
    result.gettaxdetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetTaxDtlsForItem',data,heads);}

    //General T & C Tab APIS
    result.GetPOGeneralTandC=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOGeneralTermsandConditions',data,heads);}
    result.SaveGeneralTandC=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SavePOGeneralTermsandConditions',data,heads);}
    result.DeleteGeneralTandC=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/DeletePOGeneralTermsandConditions',data,heads);}
    result.GetPOSPecificTermCoditions=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOAllSPecificTermsandCoditions',data,heads);}
    result.GetAnnexSPecifications=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOAnnexureSPecifications',data,heads);}
    //Linked PO  Tab APIS
    result.SaveLinkP=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/SaveMultipleLinkPurchaseOrders',data,heads);}
    result.DeleteLinkedPO=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/DeleteLinkedPO',data,heads);}
   //result.GetLinkedPO = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/TEPurchaseApproved_Pagination', data, heads); }
    result.GetLinkedPO = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetLinkedDetails', data, heads); }
    result.materialCheckListInfo = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/MaterialSpecsWithCheckListId', data, heads); }
    //Specific Term & conditions Tab APIS
    result.savePOSpecificTC = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/POSpecificTermandConditionsAPI/SavePOSpecificTandC', data, heads); }
    result.updatePOSpecificTC = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/POSpecificTermandConditionsAPI/UpdatePOSpecificTandC', data, heads); }
    result.deletePOSpecificTC = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/POSpecificTermandConditionsAPI/DeletePOSpecificTandC_SingleDelete', data, heads); }
    result.getPOSpecificTCTitle = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/POSpecificTermandConditionsAPI/GetPOSpecificTCTitles', data, heads); }
    result.getPOSpecificTCSubTitle = function(data) { return $http.post($sessionStorage.purchaseorder_url+'/POSpecificTermandConditionsAPI/GetPOSpecificTCSubTitleDetails', data, heads); }
    //result.getAllPOSpecificTC= function(data) { return $http.post($sessionStorage.purchaseorder_url+'/POSpecificTermandConditionsAPI/GetAllPOSpecificTCDetails', data, heads); }
    result.getAllPOSpecificTC= function(data) { return $http.post($sessionStorage.purchaseorder_url+'/POSpecificTermandConditionsAPI/GetPOSpecificTermsandConditions', data, heads); }
    result.getPOSpecificTCById= function(data) { return $http.post($sessionStorage.purchaseorder_url+'/POSpecificTermandConditionsAPI/TEPOSpecificTCDetailByID', data, heads); }
    result.submitForApproval = function(data){return $http.post($sessionStorage.purchaseorder_url+'/TEPOSearchAPI/SubmitForApprove',data,heads);}

    result.ItemInfoByItemCode = function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/ItemViewInfoByUniqueId',data,heads);}

   //Basic & General Tab APIS
    result.GetProjectTypes = function(data){return $http.post($sessionStorage.server_url+'/TEPickListItemItemAPI/GetPickListByNameNew',data,heads);}
    result.getCustomersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllCustomers',data,heads);}
    //result.getVendorsList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllVendors',data,heads);}
    result.getVendorsList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/VendorShippingDetailsAPI/GetSearchedVendors',data,heads);}
    
    //result.getFundCentersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllFundCenters',data,heads);}
    result.getFundCentersList=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetAllFundCentersBySearch',data,heads);}
    result.GetFundCenterByCode=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetFundCenterByCode',data,heads);}

    result.getVendor=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetVendor',data,heads);}
    result.getFundCenter =function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetFundCenter',data,heads);}
    result.getCompany=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetCustomer/',data,heads);}
    result.getProject=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetProject/',data,heads);}
    result.getProjectDetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetProjectDetails/',data,heads);}
    result.getTermsConditions=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetGeneralTermsConditions',data,heads);}
    result.getPOTypes=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/GetPOTypes',data,heads);}
    result.getBilling=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/BillingandShippingInfoByCompCode',data,heads);}
    result.getShippingDetails=function(data){return $http.post($sessionStorage.purchaseorder_url+'/PurchaseOrder/getShippingDetails',data,heads);}
    result.getMangers=function(data){return $http.post($sessionStorage.purchaseorder_url+'/GLAccountAPI/GetManagersByFundCenterID',data,heads);}
    
    //For GLUpdate
    result.GetPurchaseItemswithPOIdForGLupdate = function(data){return $http.post($sessionStorage.purchaseorder_url+'/GLAccountAPI/GetPurchaseItemswithPOIdForGLupdate', data,heads);}
    result.GetGLAccountDetailsList = function(data) {  return $http.post($sessionStorage.purchaseorder_url+'/GLAccountAPI/GetGLAccountDetails', data, heads); }
    result.UpdateGLAccountDetails = function(data) {  return $http.post($sessionStorage.purchaseorder_url+'/GLAccountAPI/UpdateGLAccountsForPurchaseItems', data, heads); }
 	return result;
})