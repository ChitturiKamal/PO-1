(function() {
    'use strict';

    angular
        .module('procurementApp')
        .factory('commonServices', commonServices);

    /* @ngInject */
    function commonServices($http, $window, commonUtilService) {
        var TEContactManagementAPI = commonUtilService.getUrl('TEContactManagemantApi');
        var TELeadManagementAPI = commonUtilService.getUrl('TELeadManagementAPI');
        var service = {
                callServices: callServices
            },
            urlItemMapper = {
                getContacts: [TEContactManagementAPI + '/api/TEContactProfile/GetByFilter', 'post'],
                getFullTextSearch: [TEContactManagementAPI + '/api/TEContactAdvanceSearch/GetFullTextSearchResult', 'post'],
                getPickList: [TELeadManagementAPI + '/api/Account/GetPickListBy', 'post'],
                getSuggation: [TEContactManagementAPI + '/api/TEContactAutoSearch/GetSuggation', 'post'],
                addCategory: [TEContactManagementAPI + '/api/TERelationshipCategory/Post', 'post'],
                getCategory: [TEContactManagementAPI + '/api/TERelationshipCategory/Get', 'get'],
                deleteCategory: [TEContactManagementAPI + '/api/TERelationshipCategory/Delete', 'post'],
                uploadDocument: [TELeadManagementAPI + '/api/TEDocumentUpload/Post', 'post']
            };
        return service;

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
