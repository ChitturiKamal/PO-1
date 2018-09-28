angular.module('TEPOApp')
.controller('FundCenterUserPOManagerCtrl', function($scope,$rootScope,$sessionStorage,$element,User_FundMapFactory,AlertMessages,$sce){
	$scope.pagenumber= 1;$scope.pageprecounts=10;
	$scope.currentdate = new Date();
	$scope.getFundcentertPOManagerList=function(){
		if($scope.pagenumber<='0')$scope.pagenumber=1;
        User_FundMapFactory.getAllFundcenter_POManager_Mapping({'page_number':$scope.pagenumber,'pagepercount':$scope.pageprecounts}).success(function(response){
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

	User_FundMapFactory.getFundCenterCall().then(function (response) {
		$scope.FundCenter = response.data.result;
	})

	User_FundMapFactory.getMangers().success(function (response) {
		$scope.ManagersResult = response.result;
	});
	
	$scope.FromView = function(data){
		User_FundMapFactory.getFundcenter_POManager_Mapping_ById({'FundCenterPOMgrMappingId':data}).then(function (response) {
			$scope.fundCenterPOManagerById = response.data.result;
			$scope.FundCenterPOMgrMappingId =$scope.fundCenterPOManagerById[0].FundCenterPOMgrMappingId;
		})
	}

	$scope.FromView = function (ApprovalCondition) {
		$scope.MultiApprovalUpd = [];
		$scope.MultiApprovalUpd1 = [];
		$scope.getFundID = ApprovalCondition.FundCenterID;
		$scope.getMasterAppList = ApprovalCondition.FundCentMap;
		//$scope.POApprovalConditionDTO = ApprovalCondition.POApprovalConditionDTO;
			
		for(i=0; i<$scope.getMasterAppList.length;i++)
		{
			$scope.getMasterAppList[i].UserID = ($scope.getMasterAppList[i].UserID).toString()  + '';
		}
		for (i = 0; i < $scope.getMasterAppList.length; i++) {
			$scope.MultiApprovalUpd.push({ 'FundCentreID': $scope.getMasterAppList[i].FundCenterId, 'UniqueId': $scope.getMasterAppList[i].UserID, 'SubmitterName': $scope.getMasterAppList[i].ManagerName, 'Type': 'Submitter', 'SequenceId': i + 1 });
		}
		//angular.forEach($scope.getMasterAppList, function(value, index) {
		//	$scope.getMasterAppList[index].ApproverId = value.ApproverId.toString();
		//}); 
		//angular.forEach($scope.getMasterAppList, function (value, index) {
		//	$scope.MultiApprovalUpd.push({ 'ApproverId': value.ApproverId,'UniqueId': value.UniqueId, 'ApproverName': value.ApproverName, 'Type': 'Approver', 'SequenceId': index+1 });
		//});
	}

	$scope.FromUpdate = function () {
		var data = JSON.stringify($('.updateForm').serializeObject());
		var validate = JSON.parse(data);
		
		if ((validate.FundCentreCode).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center is Mandatory' }); return false; }
		 else {
			 //var obj = { "ApprovalCondition": { 'FundCenter': validate.FundCenter, 'LastModifiedBy': validate.LastModifiedBy, 'UniqueId': validate.uniqueid, 'MinAmount': validate.MinAmount, 'MaxAmount': validate.MaxAmount }, "MasterApproverlist": $scope.MALUpdate }

			 //poMasterApproversFactory.updateMasterApprover(data).success(function (response) {
				User_FundMapFactory.updateFundcenter_POManager_Mapping(data).success(function (response) {
				if (response.info.errorcode == '0') {
					$('.updateForm')[0].reset();
					$('#updateFundCenterPOManager').modal('hide');
					$scope.MultiSubmitter = [];
					$scope.MasterSubmitterlist = [];
					$scope.MultiSubmitterUpd = [];
				}
				AlertMessages.alertPopup(response.info);
				$scope.getFundcentertPOManagerList();
			});
		}
	}
	




	
	$scope.MultiSubmitterUpd = [];
		$scope.AddSubmitterListUpd = function () {
			var SubmitterIdNo = $scope.MultiSubmitterUpd.length + 1;
			$scope.MultiSubmitterUpd.push({ 'id': SubmitterIdNo });
			
		};

		
	$scope.MultiSubmitter = [];
	$scope.AddSubmitterList = function () {
		var SubmitterIdNo = $scope.MultiSubmitter.length + 1;
		$scope.MultiSubmitter.push({ 'id': SubmitterIdNo });
		
	};
		
		$scope.MasterSubmitterlist = [];
		$scope.getApprover = function (Aid, index) {
			var i = index;
			angular.forEach($scope.ManagersResult, function (value, key) {
				if (value.UserId == Aid) {
					if ($scope.MasterSubmitterlist.length > 0) {
						angular.forEach($scope.MasterSubmitterlist, function (value, index) {
							if (value.SequenceId == i) {
								$scope.MasterSubmitterlist.splice(index, 1);
							}
						});
					}
					  $scope.MasterSubmitterlist.push({ 'SubmiterID': Aid, 'CallName': value.UserName, 'Type': 'Submitter', 'SequenceId': index });
				}
			});
		}

		$scope.RemoveApprovalList = function (index) {
			$scope.MultiSubmitter.splice(index, 1);
			$scope.MasterSubmitterlist.splice((index + 1), 1);
			$scope.serializeIndex($scope.MasterSubmitterlist,1);
		}
		
		$scope.RemoveApprovalListUpd = function (index,indFilled) {
			if($scope.MALUpdate.length == 0)$scope.getPreApprupd();
			if(index != null)$scope.MultiSubmitterUpd.splice(index, 1);
			if(indFilled != null)$scope.MALUpdate.splice(indFilled, 1);
			if(indFilled < $scope.getMasterAppList.length && indFilled != null)
			$scope.getMasterAppList.splice(indFilled, 1);
			$scope.serializeIndex($scope.MALUpdate,2);
		}

		$scope.getPreApprupd = function(){
			$scope.MultiApprovalUpd.forEach((v, i) => {
				$scope.MALUpdate.push( $scope.MultiApprovalUpd[i]);
			});
		}
		$scope.MALUpdate = [];
		$scope.serializeIndex = function (MasterSubmitterlist,k) {
		    $scope.MasterSubmitterlist = [];
		    
			$scope.MALUpdate = [];
			var i = 1;
			angular.forEach(MasterSubmitterlist, function (value, key) {
				if (value.SequenceId != '0') {
					$scope.MasterSubmitterlist.push({ 'SubmiterID': value.ApproverId, 'UniqueId': value.UniqueId, 'ManagerName': value.ApproverName, 'Type': 'Submitter', 'SequenceId': i });
					i++;
				}
				else
					$scope.MasterSubmitterlist.push({ 'SubmiterID': value.ApproverId, 'UniqueId': value.UniqueId, 'ManagerName': value.ApproverName, 'Type': 'Submitter', 'SequenceId': $scope.ind });
			});
			if(k==2){
			    $scope.MasterSubmitterlist.forEach((v, i) => {
				    $scope.MALUpdate.push($scope.MasterSubmitterlist[i]);

				});
			}
		}

	$scope.FromCreate = function() {
		var data = JSON.stringify($('.addForm').serializeObject());
		var obj = $('.addForm').serializeObject();
		var validate = JSON.parse(data);
        if ((validate.FundCentreID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center is Mandatory' }); return false; }
			else if ((validate.ManagerName).length != getArrayCounts(validate.ManagerName)) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Manager is Mandatory' }); return false; }
			else {
				var obj = { 'FundCenter': validate.FundCentreID, "MasterSubmitterlist": $scope.MasterSubmitterlist, "LastModifiedBy": validate.LastModifiedBy }
				User_FundMapFactory.Fundcenter_POManager_Mapping(obj).success(function (response) {
					if (response.info.errorcode == '0') {
						$('.addForm')[0].reset();
						$('#addFundCenterPOManager').modal('hide');
						$scope.MultiSubmitter = [];
						$scope.MasterSubmitterlist = [];
					}
					else{ AlertMessages.alertPopup({ errorcode: '1', errormessage: response.info.errormessage }); return false; }
					AlertMessages.alertPopup(response.info);
					$scope.getFundcentertPOManagerList();
				});
			}
	}
	
	
	$scope.SetDeleteData = function (data) {
		$scope.FundCenterCodeID=data.FundCenterID;
	};

	$scope.FormDelete = function () {
		var data = JSON.stringify($('.deleteForm').serializeObject());
		var validate = JSON.parse(data);	   
        User_FundMapFactory.deleteFundcenter_POManager_Mapping({"FundCenter" : validate.FundCenterCodeID, 'LastModifiedBy': validate.LastModifiedBy}).success(function (response) {
				if(response.info.errorcode=='0'){$('#DeleteBroadcastPopUp').modal('hide');}
	            AlertMessages.alertPopup(response.info);
				$scope.getFundcentertPOManagerList();
	        });
	}
})
.directive("multipleSubmitter", function () {
	return {
		template: '<div class="headtxt-big" style="margin: 10px 0;" ng-repeat="data in MultiSubmitter">\
				<div class="row" style="margin-right: -3px;margin-left: 76px;width: 39%;">\
				<div class="col-sm-10 plr2">\
				<select class="form-control input-css" name="ManagerName" ng-model="data.UserId" ng-change="getApprover(data.UserId,$index+2);">\
				<option value="" style="display: none;" selected="selected">Select Submitter</option>\
				<option ng-repeat="data in ManagersResult" value="{{data.UserId}}">{{data.CallName}}</option>\
				</select>\
				</div>\
				<div class="col-sm-2 plr2" style="margin-left:-9%"><a href="javascript:void(0);" style="margin-left:5px; margin-top:2px;float:left;" ng-click="RemoveApprovalList($index);"><i class="fa fa-times" style="font-size:10pt;"></i></a></div>\
				</div>\
				</div>'
	};
})
.directive("multipleSubmitterUpd", function () {
	return {
		template: '<div class="headtxt-big" style="margin: 10px 0;" ng-repeat="data in MultiSubmitterUpd">\
				<div class="row" style="margin-right: -3px;margin-left: 86px;width: 60%;">\
				<div class="col-sm-10 plr2" >\
				<select class="form-control input-css" name="ManagerName" ng-model="data.UserId" ng-change="getApproverUpd(data.UserId,getMasterAppList.length+$index+1);">\
				<option value="" style="display: none;" selected="selected">Select Submitter</option>\
				<option ng-repeat="data in ManagersResult" value="{{data.UserId}}">{{data.CallName}}</option>\
				</select>\
				</div>\
				<div class="col-sm-2 plr2" style="margin-left:-4%"><a href="javascript:void(0);" style="margin-left:-1px; margin-top:2px;float:left;" ng-click="RemoveApprovalListUpd($index,MultiApprovalUpd.length+$index);"><i class="fa fa-times" style="font-size:10pt;"></i></a></div>\
				</div>\
				</div>'
	};
})