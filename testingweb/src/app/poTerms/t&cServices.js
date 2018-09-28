// (function() {
    // 'use strict';

    // angular
        // .module('procurementApp')
        // .factory('EdmServices', EdmServices);

    // /* @ngInject */
    // function EdmServices($http, $window) {
        // var TEContactManagementAPI = $window.localStorage.getItem('TEContactManagemantApi');
        // var TEMailApi = $window.localStorage.getItem('TEMailApi');
        // var service = {
                // callServices: callServices
            // },
            // urlItemMapper = {
                // getEdms: [TEContactManagementAPI + '/api/TEEDM/GetAllEdm', 'post'],
                // sendMail: [TEMailApi + '/api/TEEmailSending/SendComplexMessage', 'post']
            // };
        // return service;

        // ////////////////
        // function callServices(api, json) {
            // var url = urlItemMapper[api][0];
            // if (urlItemMapper[api][1].toLowerCase() === 'get') {
                // if (angular.isDefined(json)) {
                  // var request="";
                    // url += '?';
                    // $window._.forIn(json, function(value, key){
                      // request += key + "=" + value + "&"; 
                    // });
                    // url += request.substring(0, request.length-1);
                // }
            // }
            // return $http({
                // method: urlItemMapper[api][1],
                // url: url,
                // dataType: 'json',
                // data: json,
                // headers: {
                    // 'Content-Type': 'application/json; charset=UTF-8'
                // }
            // }).then(function(data) {
                // return data;
            // }, function(data) {
                // return data;
            // });
        // }
    // }
// })();
