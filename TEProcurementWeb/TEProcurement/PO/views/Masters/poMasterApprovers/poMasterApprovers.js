angular.module('TEPOApp')
	.controller('poMasterApproversCtrl', function ($scope, $rootScope, $sessionStorage, $element, poMasterApproversFactory, AlertMessages, $sce) {
		$scope.pagenumber = 1; $scope.pageprecounts = 10;
		$scope.currentdate = new Date();
		$scope.GetPoMasterapproverList = function () {
			$rootScope.loading = true;
			if ($scope.pagenumber <= '0') $scope.pagenumber = 1;
			poMasterApproversFactory.GetPoMasterApproverData({ 'pageNumber': $scope.pagenumber, 'pagePerCount': $scope.pageprecounts }).success(function (response) {
				$rootScope.loading = false;
				$scope.masterAppronerList = response;
			});
		}
		$scope.pagenationss = function (a, b) { 
			var b = $("select[name='pageprecounts']").val(); 
			$scope.pagenumber = a; $scope.pageprecounts = b; $scope.GetPoMasterapproverList();
		}
		$scope.pagenationss_clk = function (a, b) {
			 if ("0" == b) var a1 = --a; 
			 else var a1 = ++a; "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1; 
			 var c = $("select[name='pageprecounts']").val(); 
			 $scope.pagenumber = a1; $scope.pageprecounts = c; $scope.GetPoMasterapproverList(); 
			}

		poMasterApproversFactory.getOrderTypeCall().then(function (response) {$scope.OrderType = response.data.result;})

		poMasterApproversFactory.getFundCenterCall().then(function (response) {$scope.FundCenter = response.data.result;})

		poMasterApproversFactory.getSubmitterCall().then(function (response) {$scope.Submitter = response.data.result;})

		$scope.MultiApproval = [];
		$scope.AddApproverList = function () {
			var ApproverIdNo = $scope.MultiApproval.length + 2;
			$scope.MultiApproval.push({ 'id': ApproverIdNo });
		};

		$scope.RemoveApprovalList = function (index) {
			$scope.MultiApproval.splice(index, 1);
			$scope.MasterApproverlist.splice((index + 1), 1);
			$scope.serializeIndex($scope.MasterApproverlist,1);
		}

		
		$scope.MultiApprovalUpd1 = [];
		$scope.AddApproverListUpd = function () {
			var ApproverIdNo = $scope.MultiApprovalUpd.length + 1;
			$scope.MultiApprovalUpd1.push({ 'id': ApproverIdNo });
			
		};

		$scope.RemoveApprovalListUpd = function (index,indFilled) {
			if($scope.MALUpdate.length == 0)$scope.getPreApprupd();
			if(index != null)$scope.MultiApprovalUpd1.splice(index, 1);
			if(indFilled != null)$scope.MALUpdate.splice(indFilled, 1);
			if(indFilled < $scope.getMasterAppList.length && indFilled != null)
			$scope.getMasterAppList.splice(indFilled, 1);
			$scope.serializeIndex($scope.MALUpdate,2);
		}

		$scope.serializeIndex = function (MasterApproverlist,k) {
			$scope.MALUpdate = [];
			var i = 1;
			angular.forEach(MasterApproverlist, function (value, key) {
				if (value.SequenceId != '0') {
					$scope.MasterApproverlist.push({ 'ApproverId': value.ApproverId, 'UniqueId': value.UniqueId, 'ApproverName': value.ApproverName, 'Type': 'Approver', 'SequenceId': i });
					i++;
				}
				else
					$scope.MasterApproverlist.push({ 'ApproverId': value.ApproverId, 'UniqueId': value.UniqueId, 'ApproverName': value.ApproverName, 'Type': 'submitter', 'SequenceId': $scope.ind });
			});
			if(k==2){
			    $scope.MasterApproverlist.forEach((v, i) => {
				    $scope.MALUpdate.push($scope.MasterApproverlist[i]);

				});
			}
		}

		$scope.MasterApproverlist = [0,0];
		$scope.getApprover = function (Aid, index) {
			var i = index;
			angular.forEach($scope.Submitter, function (value, key) {
				if (value.UserId == Aid) {
					if ($scope.MasterApproverlist.length > 0) {
						angular.forEach($scope.MasterApproverlist, function (value, index) {
							if (value.SequenceId == i) {
								$scope.MasterApproverlist.splice(index, 1);
							}
						});
					}
					  $scope.MasterApproverlist.push({ 'ApproverId': Aid, 'ApproverName': value.UserName, 'Type': 'Approver', 'SequenceId': index });
				}
			});
		}

		$scope.getPreApprupd = function(){
			$scope.MultiApprovalUpd.forEach((v, i) => {
				$scope.MALUpdate.push( $scope.MultiApprovalUpd[i]);
			});
		}

		$scope.MALUpdate = [];
		$scope.count = 0;
		$scope.getApproverUpd = function (Aid, index) {
			if($scope.count == 0){
				$scope.getPreApprupd();
				$scope.count++;
			}
			var i = index;
			var remove = 0;
			angular.forEach($scope.Submitter, function (value, key) {
				updatedObj = value;
				if (value.UserId == Aid) {
					if ($scope.MALUpdate.length > 0) {
						angular.forEach($scope.MALUpdate, function (value, index) {
							if (value.SequenceId == i) {
								// $scope.MALUpdate.splice(index, 1);
								$scope.MALUpdate[index]= { 'ApproverId': Aid,'UniqueId': value.UniqueId, 'ApproverName': updatedObj.UserName, 'Type': 'Approver', 'SequenceId': i};
								remove++;
							}
						});
					}
					if(remove == 0)$scope.MALUpdate.push({ 'ApproverId': Aid, 'UniqueId': 0,'ApproverName': value.UserName, 'Type': 'Approver', 'SequenceId': index });
				}
			});
		}

		//$scope.AppCond = function (val) {
		//	if (val == "1") { $scope.MinAmount = "0"; $scope.MaxAmount = "10,00,000"; $scope.ApprovalConditionId = val }
		//	if (val == "2") { $scope.MinAmount = "10,00,001"; $scope.MaxAmount = "50,00,000"; $scope.ApprovalConditionId = val }
		//	if (val == "3") { $scope.MinAmount = "50,00,001"; $scope.MaxAmount = "100,00,000"; $scope.ApprovalConditionId = val }
	    //}

		$scope.AppCond = function (val) {
		    if (val == "1") { $scope.MinAmount = "0"; $scope.MaxAmount = "9,99,999"; $scope.ApprovalConditionId = val }
		    if (val == "2") { $scope.MinAmount = "10,00,001"; $scope.MaxAmount = "49,99,999"; $scope.ApprovalConditionId = val }
		    if (val == "3") { $scope.MinAmount = "50,00,001"; $scope.MaxAmount = "100,00,000"; $scope.ApprovalConditionId = val }
		}


		//$scope.AppCondnew = function (val) {
		//    var res = {};
		//    if (val == "1") res = {"MinAmount":"0","MaxAmount":"10,00,000"};
		//    if (val == "2") res = { "MinAmount": "10,00,001", "MaxAmount": "50,00,000" };
		//    if (val == "3") res = { "MinAmount": "50,00,001", "MaxAmount": "100,00,000" };
		//    return res;
		//}
		$scope.FromCreate = function () {
			var data = JSON.stringify($('.addForm').serializeObject());
			var obj = $('.addForm').serializeObject();
			var validate = JSON.parse(data);
			if ((validate.FundCenter).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center is Mandatory' }); return false; }
			else if ((validate.Approval).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Approval is Mandatory' }); return false; }
			else if ((validate.ApproverName).length != getArrayCounts(validate.ApproverName)) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Approver is Mandatory' }); return false; }
			else {
				for(var i=2;i<$scope.MasterApproverlist.length;i++){
					
					if($scope.MasterApproverlist[i] != null){$scope.MasterApproverlist[i].SequenceId+=1;}
				}
				var obj = { "ApprovalCondition": {'FundCenter': validate.FundCenter, 'MinAmount': $scope.MinAmount, 'MaxAmount': $scope.MaxAmount }, "MasterApproverlist": $scope.MasterApproverlist }
				poMasterApproversFactory.saveMasterApprover(obj).success(function (response) {
					if (response.info.errorcode == '0') {
						$('.addForm')[0].reset();
						$('#addapprovalMaster').modal('hide');
						$scope.MultiApproval = [];
						$scope.MasterApproverlist = [];
					}
					AlertMessages.alertPopup(response.info);
					$scope.GetPoMasterapproverList();
				});
			}
		}

		$scope.MultiApprovalUpd = [];
		$scope.FromView = function (ApprovalCondition) {
			$scope.MultiApprovalUpd = [];
			$scope.MultiApprovalUpd1 = [];
			$scope.getApproverView = ApprovalCondition.ApprovalCondition;
			$scope.getMasterAppList = ApprovalCondition.MasterApproverlist;
			$scope.POApprovalConditionDTO = ApprovalCondition.POApprovalConditionDTO;
                
			for(i=0; i<$scope.getMasterAppList.length;i++)
			{
			    $scope.getMasterAppList[i].ApproverId = $scope.getMasterAppList[i].ApproverId.toString();
			}
			for (i = 0; i < $scope.getMasterAppList.length; i++) {
			    $scope.MultiApprovalUpd.push({ 'ApproverId': $scope.getMasterAppList[i].ApproverId, 'UniqueId': $scope.getMasterAppList[i].UniqueId, 'ApproverName': $scope.getMasterAppList[i].ApproverName, 'Type': 'Approver', 'SequenceId': i + 1 });
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
			
			if ((validate.uniqueid).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center is Mandatory' }); return false; }

			  
			 else {
                 
			   
			     //var obj = { "ApprovalCondition": { 'FundCenter': validate.FundCenter, 'LastModifiedBy': validate.LastModifiedBy, 'UniqueId': validate.uniqueid, 'MinAmount': validate.MinAmount, 'MaxAmount': validate.MaxAmount }, "MasterApproverlist": $scope.MALUpdate }

			     //poMasterApproversFactory.updateMasterApprover(data).success(function (response) {
			     poMasterApproversFactory.updateMasterApprover_new(data).success(function (response) {
					if (response.info.errorcode == '0') {
						$('.updateForm')[0].reset();
						$('#updateapprovalMaster').modal('hide');
						$scope.MultiApproval = [];
						$scope.MasterApproverlist = [];
					}
					AlertMessages.alertPopup(response.info);
					$scope.GetPoMasterapproverList();
				});
			}
		}

		$scope.SetDeleteData = function (data) {
			$scope.broadcastDeleteData = data;
		};
		
		$scope.FormDelete = function () {
			var data = JSON.stringify($('.deleteForm').serializeObject());
			var validate = JSON.parse(data);
			poMasterApproversFactory.deleteBroadCast(data).success(function (response) {
				if (response.info.errorcode == '0') { $('#DeleteBroadcastPopUp').modal('hide'); }
				AlertMessages.alertPopup(response.info);
				$scope.GetBroadcastList();
			});
		}
		$scope.updatePopoverData = function (a, b) { $rootScope.EmailSubject = a; $rootScope.EmailTemplateValue = b; }
		$scope.GetPoMasterapproverList();
	})
	.directive("multipleApproval", function () {
		return {
			template: '<div class="headtxt-big" style="margin: 10px 0;" ng-repeat="data in MultiApproval">\
					<div class="row" style="margin-right: -3px;margin-left: 76px;width: 39%;">\
					<div class="col-sm-10 plr2">\
					<select class="form-control input-css" name="ApproverName" ng-model="data.UserId" ng-change="getApprover(data.UserId,$index+2);">\
					<option value="" style="display: none;" selected="selected">Select Approver</option>\
					<option ng-repeat="data in Submitter" value="{{data.UserId}}">{{data.UserName}}</option>\
					</select>\
					</div>\
					<div class="col-sm-2 plr2" style="margin-left:-9%"><a href="javascript:void(0);" style="margin-left:5px; margin-top:2px;float:left;" ng-click="RemoveApprovalList($index);"><i class="fa fa-times" style="font-size:10pt;"></i></a></div>\
					</div>\
					</div>'
		};
	})
	.directive("multipleApprovalUpd", function () {
		return {
			template: '<div class="headtxt-big" style="margin: 10px 0;" ng-repeat="data in MultiApprovalUpd1">\
					<div class="row" style="margin-right: -3px;margin-left: 76px;width: 39%;">\
					<div class="col-sm-10 plr2" style="margin-left: 10px;width: 79%;">\
					<select class="form-control input-css" name="ApproverName" ng-model="data.UserId" ng-change="getApproverUpd(data.UserId,getMasterAppList.length+$index+1);">\
					<option value="" style="display: none;" selected="selected">Select Approver</option>\
					<option ng-repeat="data in Submitter" value="{{data.UserId}}">{{data.UserName}}</option>\
					</select>\
					</div>\
					<div class="col-sm-2 plr2" style="margin-left:-9%"><a href="javascript:void(0);" style="margin-left:-1px; margin-top:2px;float:left;" ng-click="RemoveApprovalListUpd($index,MultiApprovalUpd.length+$index);"><i class="fa fa-times" style="font-size:10pt;"></i></a></div>\
					</div>\
					</div>'
		};
	})