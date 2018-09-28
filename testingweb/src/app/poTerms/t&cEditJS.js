(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('EditController', EditController) 
   
   function EditController(POServices, $scope, $http, $stateParams, commonUtilService, $window) {
    $http.get('http://192.168.51.248/TEComplaintsManagementAPI/api/TEPODetails/GetMasterTermsAndConditions?pagecount=1').
        success(function(data) {
            $scope.editmaster = data;
			console.log("data");
	
		var vm = this;

        vm = {
            "CreatedBy":"Sai",			
			"Title":"Test1",
			"Type":"12889",
			"Condition":"tetsing condition1",
			"IsActive":"true"			
        };
		
        vm.validationList = {
            0: 'None',
            1: /^[0-9\s]+$/, //NumbersOnly
            2: /^[a-zA-Z\s]+$/, //CharactersOnly
            3: /^[a-zA-Z0-9\s]+$/, //NumbersAndCharacters
            4: /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/, //Email
            5: 'RequiredField'
        };

        vm.pickList = [];

        activate();

        function activate() {
            getPickList();
        }

        function getPickList() {
            var request = {
				"PickListNames": ["TermsAndConditionType"]			
            };
            POServices.callServices("getPickList", request).then(             
				function(response) {
					$scope.items = response.data[0].PickLstNmeVluMap.TermsAndConditionType;
                    $scope.selectedItem = $scope.items[0]
					
                }
			);
		}
					
	 
								
		
		

        });		
	}



       
    
	

})();

