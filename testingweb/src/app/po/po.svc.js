(function() {
    'use strict';

    angular
        .module('procurementApp')
        .factory('POServices', POServices);

    /* @ngInject */
    /* function config() {}*/
    function POServices($http, $window, commonUtilService) {
        var TEComplaintsUrl = commonUtilService.getUrl('TEComplaintsUrl');
        var TEEmailApi = commonUtilService.getUrl('TEEmailApi');
        var TELeadManagementAPI= commonUtilService.getUrl('TELeadManagementAPI');
        var service = {
                callServices: callServices
            },
            urlItemMapper = {
                getPOList: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseHomeByUser', 'get'],
                getPOStatusCount: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseOrdersCount', 'get'],
                getPOSearch: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseHomeByUserFilter', 'get'],
                getPOById: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseItemListheader', 'get'],
                getPOApproversById: [TEComplaintsUrl + 'api/TEPODetails/GetPOApprovers', 'get'],
                getPurchaseItemListById: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseItemList', 'get'],
                getPurchaseItemDetailsById: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseItemlistDetails', 'get'],
                getPOTermsAndConditions: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseTerms', 'get'],
                poApproveReject: [TEComplaintsUrl + 'api/TEPODetails/PurchaseApproveReject', 'post'],
                sendMail: [TEEmailApi + 'api/TEEmailSending/SendComplexMessage', 'post'],
                getFinalizeList: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseDraftList', 'get'],
                postMilestone: [TEComplaintsUrl + 'api/TEPODetails/PostPOMilestones', 'post'],
                getPoMilestones: [TEComplaintsUrl + 'api/TEPODetails/GetPOMilestones', 'get'],
                getMasterTandC: [TEComplaintsUrl + 'api/TEPODetails/GetMasterTermsAndConditionsByType', 'get'],//by  shiva
                //postMaster: [TEComplaintsUrl + 'api/TEPODetails/PostMasterTerms', 'post'], //by  shiva
                getPickList: [TELeadManagementAPI + 'api/Account/GetPickListBy', 'post'], //by  shiva
                postPOTerms: [TEComplaintsUrl + 'api/TEPODetails/PostTermsAndConditions', 'post'],
                getPoTerms: [TEComplaintsUrl + 'api/TEPODetails/GetTermsAndConditionsByType', 'get'],
                PoSubmit: [TEComplaintsUrl + 'api/TEPODetails/POSubmitforApproval', 'post'],
                PoWithdraw: [TEComplaintsUrl + 'api/TEPODetails/POwithdraw', 'get'],
                GETAllStorageDetails: [TEComplaintsUrl + 'api/TEPODetails/GETAllStorageDetailsByStorageCode', 'get'],
                getPurchaseItemAnnexureListById: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseItemListForAnnexure', 'get'],
                getPurchaseItemAnnexureDetailsById: [TEComplaintsUrl + 'api/TEPODetails/TEPurchaseItemlistDetailsForAnnexure', 'get']
            };
        return service;

        ////////////////
        function callServices(api, json) {
            var url = urlItemMapper[api][0];
            if (urlItemMapper[api][1].toLowerCase() === 'get') {
                if (angular.isDefined(json)) {
                    var request = "";
                    url += '?';
                    $window._.forIn(json, function(value, key) {
                        request += key + "=" + value + "&";
                    });
                    url += request.substring(0, request.length - 1);
                }
            }
            return $http({
                method: urlItemMapper[api][1],
                url: url,
                dataType: 'json',
                data: json,
                headers: {
                    'Content-Type': 'application/json; charset=UTF-8'
                }
            }).then(function(data) {
                return data;
            }, function(data) {
                return data;
            });
        }
    }
})();
