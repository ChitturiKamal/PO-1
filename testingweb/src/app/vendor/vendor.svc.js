(function() {
    'use strict';

    angular
        .module('procurementApp')
        .factory('POVendorServices', POVendorServices);

    /* @ngInject */
    /* function config() {}*/
    function POVendorServices($http, $window, commonUtilService) {
        var TEComplaintsUrl = commonUtilService.getUrl('TEComplaintsUrl');
        var service = {
                callServices: callServices
            },
            urlItemMapper = {
                getVendors: [TEComplaintsUrl + 'api/TEPODetails/GetVendors', 'get']
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
