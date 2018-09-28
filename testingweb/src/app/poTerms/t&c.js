(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('POTermsController', POTermsController) 
		// .directive('whenScrollEnds', whenScrollEnds);
   
   function POTermsController(POServices, $scope, $http) {
	   
	     var vm = this;		
		
		$scope.initFirst=function()
	{	
		$http.get('http://192.168.51.248/TEComplaintsManagementAPI/api/TEPODetails/GetMasterTermsAndConditions?pagecount=1').
			success(function(data) {
				// $scope.getmaster = data;
				console.log("data");
				
				var mydatas = data;
				$scope.getmaster = mydatas;
				// $scope.getmaster = mydatas.data[0].UniqueId;
						
				var pagesShown = 1;

				var pageSize = 10;

				$scope.paginationLimit = function(data) {
				 return pageSize * pagesShown;
				};

				$scope.hasMoreItemsToShow = function() {
				 return pagesShown < ($scope.getmaster.length / pageSize);
				};

				$scope.showMoreItems = function() {
				 pagesShown = pagesShown + 1;       
				}; 
			
			});		
	}
	
	
	
		
	}
	

 

})();

