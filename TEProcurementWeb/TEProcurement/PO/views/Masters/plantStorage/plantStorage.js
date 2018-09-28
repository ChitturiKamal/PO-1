angular.module('TEPOApp')
	.controller('plantStorageCtrl', function ($scope, $rootScope, $sessionStorage, $element, plantStorageFactory, AlertMessages, $state) {
		$scope.pagenumber = 1; $scope.pageprecounts = 10;
		$scope.currentdate = new Date();
		$scope.getplantStorageList= function () {
			$rootScope.loading = true;
			if ($scope.pagenumber <= '0') $scope.pagenumber = 1;
			plantStorageFactory.plantStoragePagination({ 'page_number': $scope.pagenumber, 'pagepercount': $scope.pageprecounts }).success(function (response) {
				$rootScope.loading = false;
				$scope.plantStorageList = response;
			});
		}
		$scope.getplantStorageList();
		$scope.pagenationss = function (a, b) { var b = $("select[name='pageprecounts']").val(); $scope.pagenumber = a; $scope.pageprecounts = b; $scope.getplantStorageList(); }
		$scope.pagenationss_clk = function (a, b) { if ("0" == b) var a1 = --a; else var a1 = ++a; "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1; var c = $("select[name='pageprecounts']").val(); $scope.pagenumber = a1; $scope.pageprecounts = c; $scope.getplantStorageList(); }

		plantStorageFactory.ProjectMaster({ 'page_number': 0, 'pagepercount': 10 }).success(function (response) {
			$scope.projectList = response.result;
		});

		plantStorageFactory.CompanyMasterCall().then(function (response) {
			$scope.CompanyMaster = response.data.result;
		})
		$scope.getstatesdata = function () {
			$rootScope.loading = true;
			plantStorageFactory.getStates().success(function (response) {
				$rootScope.loading = false;
				$scope.stateList = response.result;
			});
		}
		$scope.getstatesdata();
		$scope.getCountrydata = function () {
			$rootScope.loading = true;
			plantStorageFactory.getCountryList().success(function (response) {
				$rootScope.loading = false;
				$scope.countryList = response.result;
			});
		}
		$scope.getCountrydata();
		$scope.FromCreate = function () {
			var data = JSON.stringify($('.addForm').serializeObject());
			var obj = $('.addForm').serializeObject();
			var validate = JSON.parse(data);
			if ((validate.ProjectID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Project is Mandatory' }); return false; }
			else if ((validate.CompanyCode).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Company is Mandatory' }); return false; }
			else if ((validate.StateID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'State is Mandatory' }); return false; }
			else if ((validate.GSTIN).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GSTIN is Mandatory' }); return false; }
			else if ((validate.Type).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Type is Mandatory' }); return false; }
			else if ((validate.Address).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Address is Mandatory' }); return false; }
			else {
				var obj = {
					'ProjectID': validate.ProjectID,
					'Address': validate.Address,
					'CompanyCode': validate.CompanyCode,
					'CountryCode': validate.CountryCode,
					"GSTIN": validate.GSTIN,
					"PlantStorageCode": validate.PlantStorageCode,
					"StorageLocationCode": validate.StorageLocationCode,
					"StateID": validate.StateID,
					"Type": validate.Type,
				}
				plantStorageFactory.savePlantStorage(obj).success(function (response) {
					if (response.info.errorcode == '0') {
						$('.addForm')[0].reset();
						$('#addPlantStorage').modal('hide');
					}
					AlertMessages.alertPopup(response.info);
					$state.reload();
					$scope.hideScreen();
				});
			}
		}
		$scope.FromView = function (id) {
			plantStorageFactory.getPlantStorageById({ "PlantStorageDetailsID": id }).success(function (response) {
				if (response.info.errorcode == '0') {
					$scope.Plantstorage = response.result;
					if ($scope.Plantstorage.StateID != null) {
						$scope.Plantstorage.StateID = $scope.Plantstorage.StateID.toString();
					}
				}
			});
		}
		$scope.FromUpdate = function () {
			var data = JSON.stringify($('.updateForm').serializeObject());
			var validate = JSON.parse(data);
			if ((validate.ProjectID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Project is Mandatory' }); return false; }
			else if ((validate.CompanyCode).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Company is Mandatory' }); return false; }
			else if ((validate.StateID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'State is Mandatory' }); return false; }
			else if ((validate.GSTIN).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GSTIN is Mandatory' }); return false; }
			else if ((validate.Type).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Type is Mandatory' }); return false; }
			else if ((validate.Address).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Address is Mandatory' }); return false; }
			else
				var obj = {
					'PlantStorageDetailsID': validate.PlantStorageDetailsID,
					'ProjectID': validate.ProjectID,
					'Address': validate.Address,
					'CompanyCode': validate.CompanyCode,
					'CountryCode': validate.CountryCode,
					"GSTIN": validate.GSTIN,
					"PlantStorageCode": validate.PlantStorageCode,
					"StorageLocationCode": validate.StorageLocationCode,
					"StateID": validate.StateID,
					"Type": validate.Type,
				}
			plantStorageFactory.updatePlantStorage(obj).success(function (response) {
				if (response.info.errorcode == '0') {
					$('#updatePlantStorage').modal('hide');
				}
				AlertMessages.alertPopup(response.info);
				$state.reload();
				$scope.hideScreen();
			});
		}

		$scope.SetDeleteData = function (data) {
			$scope.PlantStorageDetailsID = data;
		};

		$scope.FormDelete = function () {
			var data = JSON.stringify($('.deleteForm').serializeObject());
			var validate = JSON.parse(data);
			plantStorageFactory.deletePlantStorage(data).success(function (response) {
				if (response.info.errorcode == '0') { $('#DeletePlantStorage').modal('hide'); }
				AlertMessages.alertPopup(response.info);
				$state.reload();
				$scope.hideScreen();
			});
		}
		$scope.updatePopoverData = function (a, b) { $rootScope.EmailSubject = a; $rootScope.EmailTemplateValue = b; }
	})
	.directive("multipleApproval", function () {
		return {
			template: '<div class="headtxt-big" style="margin: 10px 0;" ng-repeat="data in MultiApproval">\
					<div class="row" style="margin-right: -3px;margin-left: 23%;">\
					<div class="col-sm-10 plr2">\
					<select class="form-control input-css" name="ApproverName" ng-model="data.UserId" ng-change="getApprover(data.UserId,$index+2);">\
					<option value="" style="display: none;"></option>\
					<option ng-repeat="data in Submitter.result" value="{{data.UserId}}">{{data.UserName}}</option>\
					</select>\
					</div>\
					<div class="col-sm-2 plr2"><a href="javascript:void(0);" style="margin-left:5px; margin-top:2px;float:left;" ng-click="RemoveApprovalList($index);"><i class="fa fa-times" style="font-size:10pt;"></i></a></div>\
					</div>\
					</div>'
		};
	})