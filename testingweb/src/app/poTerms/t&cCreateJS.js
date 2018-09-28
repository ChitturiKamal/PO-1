(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('CreateController', CreateController) 
 
	 function CreateController(POServices, $scope, $state, $http, $location) {
		$scope.$parent.vm.mode = 'edit';
		
		// <This is for GetPickList>
		
		getPickList();
        function getPickList() {
            var request = {
				"PickListNames": ["TermsAndConditionType"]			
            };
            POServices.callServices("getPickList", request).then(               
				function(response) {
					//$scope.items = response.data[0].PickLstNmeVluMap.TermsAndConditionType;				
					var item=response.data[0].PickLstNmeVluMap.TermsAndConditionType;
					$scope.operators =item;
                }
			);
		}		
	
	// <This is for PostData>
	
			$scope.sendData = function () {
				
            var request = {
				"CreatedBy":"Sai",	
				"Title": $('#termTitle').val(),
				"Type": $('#termType').val(),
				"Condition": $('.Editor-editor').html()				
            };

            POServices.callServices("postMaster", request).then(               
				function(data) {
					 // $state.go('main/poTerms/DetailTerms');
					 // $scope.$parent.vm.refresh();
					 $location.path('/DetailTerms');
					
                }
			);
			
		 }
			 
			 // $scope.initFirst();

// ---------------------------------------------------------------------------------			 
			 
		// https://docs.angularjs.org/api/ngRoute/service/$route
		
// ---------------------------------------------------------------------------------


		
		// $scope.Title = "";
		// $scope.selectedItem = "";
		
		// $scope.SendData = function () {

			// var data = {
				// "CreatedBy":"Sai",	
				// "Title": $scope.Title,
				// "Type":$scope.selectedItem
			    // "Type": $('#termType').val(),
				// "Title": $('#termTitle').val()
				
            // };
			
			// console.log('type' + $('#termType').val());
			// console.log('title' + $('#termTitle').val());
			
            // var config = {
                // headers : {
                    // 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                // }
            // }

            // $http.post('http://192.168.51.248/TEComplaintsManagementAPI/api/TEPODetails/PostMasterTerms', data, config)
            // .success(function (data, status, headers, config) {
                // $scope.PostDataResponse = data;
            // })
            // .error(function (data, status, header, config) {
                // $scope.ResponseDetails = "Data: " + data +
                    // "<hr />status: " + status +
                    // "<hr />headers: " + header +
                    // "<hr />config: " + config;
            // });
        // };
				
		
		
// ----------------------------------------------------------------------------------
	
	// $scope.hello = {name: "Boaz"};
    // $scope.newName = "";
    // $scope.sendPost = function() {
        // var data = {
				// "CreatedBy":"Sai",	
				// "Title": $scope.Title,
				// "Type":$scope.selectedItem
				// json: JSON.stringify({
                // name: $scope.newName
				// })
        // };
        // $http.post("http://192.168.51.248/TEComplaintsManagementAPI/api/TEPODetails/PostMasterTerms", data).success(function(data, status) {
            // $scope.hello = data;
        // })
    // }  
	
// ------------------------------------------------------------------------------
		 
		/*$http.get('path/to/json').then(function(data) {
			$scope.languages = data;
		  });*/
		  // inputting json directly for this example
			
		  // $scope.languages = [        
			// {name:"English", value:0},
			// {name:"Spanish", value:1},
			// {name:"German", value:3},
			// {name:"Russian", value:2},
			// {name:"Korean", value:1}
		  // ];
		  // $scope.save = function() {
			// $http.post('http://192.168.51.248/TEComplaintsManagementAPI/api/TEPODetails/PostMasterTerms', $scope.languages).then(function(data) {
			  // $scope.msg = 'Data saved';
			// });
			// $scope.msg = 'Data sent: '+ JSON.stringify($scope.languages);
		  // };
		  
		  
		  
		  
			// $route.reload();
	  
		}
 
 
	 

})();

