angular.module('TEPOApp')
.controller('FundCenterPOManagerCtrl', function($scope,$rootScope,$sessionStorage,$element,FundCenterPOManagerFactory,AlertMessages,$sce){
	$scope.pagenumber= 1;$scope.pageprecounts=10;
	$scope.currentdate = new Date();
	$scope.getFundcentertPOManagerList=function(){
		if($scope.pagenumber<='0')$scope.pagenumber=1;
        FundCenterPOManagerFactory.getAllFundcenter_POManager_Mapping({'page_number':$scope.pagenumber,'pagepercount':$scope.pageprecounts}).success(function(response){
			$scope.result=response;
		});
		}
	$scope.getFundcentertPOManagerList();
	
	$scope.pagenationss=function(a,b){
		var b = $("select[name='pageprecounts']").val();
		$scope.pagenumber=a;$scope.pageprecounts=b;$scope.getFundcentertPOManagerList();
	}
	$scope.pagenationss_clk = function (a,b){
		if("0"==b)var a1=--a;
		else var a1=++a;"0">=a1?(a1=0,$scope.pagenumber=1):$scope.pagenumber=a1;
		var c=$("select[name='pageprecounts']").val();
		$scope.pagenumber=a1;$scope.pageprecounts=c;
		$scope.getFundcentertPOManagerList();
	}

	FundCenterPOManagerFactory.getFundCenterCall().then(function (response) {
		$scope.FundCenter = response.data.result;
	})

	FundCenterPOManagerFactory.getMangers().success(function (response) {
		$scope.ManagersResult = response.result;
	});
	
	$scope.FromView = function(data){
		FundCenterPOManagerFactory.getFundcenter_POManager_Mapping_ById({'FundCenterPOMgrMappingId':data}).then(function (response) {
			$scope.fundCenterPOManagerById = response.data.result;
			$scope.FundCenterPOMgrMappingId =$scope.fundCenterPOManagerById[0].FundCenterPOMgrMappingId;
		})
	}
	
	$scope.FromCreate = function() {
		var data = JSON.stringify($('.addForm').serializeObject());
		var obj = $('.addForm').serializeObject();
        var validate = JSON.parse(data);
         if ((validate.FundCentreID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center is Mandatory' }); return false; }
         else if ((validate.ManagerId).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Manager is Mandatory' }); return false; }
		else {
			var obj = { 'POManagerId': validate.ManagerId,  'FundCenterId': validate.FundCentreID,  'LastModifiedBy': validate.LastModifiedBy }
            FundCenterPOManagerFactory.Fundcenter_POManager_Mapping(obj)
		   .then(
			   function(response) {
				   if (response.data.info.errorcode=='0') {
					   $('.addForm')[0].reset();
						 $('#addFundCenterPOManager').modal('hide');
						//$state.reload();
				   }
				   AlertMessages.alertPopup(response.data.info);
				    $scope.getFundcentertPOManagerList();
			   }
		   );
		}
	}
	
	$scope.FromUpdate = function() {
        var data = JSON.stringify($('.updateForm').serializeObject());
        var validate = JSON.parse(data);
		if ((validate.FundCentreID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center is Mandatory' }); return false; }
		else if ((validate.ManagerId).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Manager is Mandatory' }); return false; }
        else if ((validate.FundCenterPOMgrMappingId).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PO Manager Mapping ID is Mandatory' }); return false; }
           {
        
           var obj = {'FundCenterPOMgrMappingId':validate.FundCenterPOMgrMappingId, 'POManagerId': validate.ManagerId,  'FundCenterId': validate.FundCentreID,  'LastModifiedBy': validate.LastModifiedBy }
		   FundCenterPOManagerFactory.updateFundcenter_POManager_Mapping(obj)
            .then(
                function(response) {
                    if (response.data.info.errorcode=='0') {
                         $('#updateFundCenterPOManager').modal('hide');
                    }
					AlertMessages.alertPopup(response.data.info);
					$scope.getFundcentertPOManagerList();
                }
            );
         }
	}
	
	$scope.SetDeleteData = function (data) {
		$scope.FundCenterPOMgrMappingId=data.FundCenterPOMgrMappingId;
	};

	$scope.FormDelete = function () {
		var data = JSON.stringify($('.deleteForm').serializeObject());
		var validate = JSON.parse(data);	   
        FundCenterPOManagerFactory.deleteFundcenter_POManager_Mapping(data).success(function (response) {
				if(response.info.errorcode=='0'){$('#DeleteBroadcastPopUp').modal('hide');}
	            AlertMessages.alertPopup(response.info);
				$scope.getFundcentertPOManagerList();
	        });
	}
})