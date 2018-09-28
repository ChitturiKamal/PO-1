angular.module('TEPOApp')
	.controller('WBSFundCenterCtrl', function ($scope, $rootScope, $sessionStorage, $element, WBSFundCenterFactory, AlertMessages, $sce) {
		$scope.pagenumber = 1; $scope.pageprecounts = 10;
		$scope.currentdate = new Date();
		$scope.WBFFundcentertList = function () {
			if ($scope.pagenumber <= '0') $scope.pagenumber = 1;
			WBSFundCenterFactory.getFundCenterlist({ 'page_number': $scope.pagenumber, 'pagepercount': $scope.pageprecounts }).success(function (response) {
				$scope.WBSList = response;
			});
		}
		
		$scope.pagenationss = function (a, b) {
			var b = $("select[name='pageprecounts']").val();
			$scope.pagenumber = a; $scope.pageprecounts = b;$scope.WBFFundcentertList();
		}
		$scope.pagenationss_clk = function (a, b) {
			if ("0" == b) var a1 = --a; else var a1 = ++a; "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1;
			var c = $("select[name='pageprecounts']").val();
			$scope.pagenumber = a1; $scope.pageprecounts = c;$scope.WBFFundcentertList();
		}

		

		$scope.getWBSCodeCall = function () {
			
			if (cat != null && subCat != null) {
				WBSFundCenterFactory.getWBSCode().then(function (response) {
					$scope.getWBSCode = response.data;
				})
			} else { $scope.getWBSCode = []; }
		}

		$scope.getFundcenter = function () {
			$rootScope.loading = true;
			WBSFundCenterFactory.getFundCenterCode().success(function (response) {
				$rootScope.loading = false;
				$scope.getFundCenterCodeList = response.result;
			});
		}
	   $scope.getFundcenter();

	   $scope.getFCCodeChange = function(){
		$rootScope.loading = true;
		var id =  $scope.FundCentreID;
		WBSFundCenterFactory.getFCDesc({"ID": id}).success(function (response) {
			$rootScope.loading = false;
			$scope.FundCenterDescription = response.result.FundCenter_Description;

		});
	   }
	   $scope.getAddFCCodeChange = function(){
		$rootScope.loading = true;
		var id =  $scope.FundAddCentreID;
		WBSFundCenterFactory.getFCDesc({"ID": id}).success(function (response) {
			$rootScope.loading = false;
			$scope.FundAddCenterDescription = response.result.FundCenter_Description;

		});

	   }

	   $scope.LengthRestriction = function(Value){

		//alert(Value.WBSAddCode.length());

	   }

		$scope.FromCreate = function () {
			var data = JSON.stringify($('.addForm').serializeObject());
			var obj = $('.addForm').serializeObject();
			var validate = JSON.parse(data);
			if ((validate.WBSAddCode).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Code is Mandatory' }); return false; }
			else if ((validate.WBSAddName).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Name is Mandatory' }); return false; }
			else if ((validate.FundAddCentreID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Centre Code is Mandatory' }); return false; }
			else if ((validate.FundAddCenterDescription).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center Description is Mandatory' }); return false; }
			else
			{
			var obj = {  'WBSCODE': $scope.WBSAddCode,'WBSName': $scope.WBSAddName, 'FundCentreID': validate.FundAddCentreID, 'FundCentreCode': $scope.FundAddCentreID}
			
			WBSFundCenterFactory.createWBSFundcenter(obj)
					.success(
					function (response) {
						if (response.info.errorcode == '0') {
							$('.addForm')[0].reset();
							$('#addWBSFundCenter').modal('hide');
							//$state.reload();
						}
						AlertMessages.alertPopup(response.info);
						$scope.WBFFundcentertList();
					}
					);
			}
		}

		$scope.FromView = function (id) {
			
			WBSFundCenterFactory.getWBSById( {"UniqueID" : id} ).success(function (response) {
				$scope.getWBSView = response.result;

				if($scope.getWBSView.WBSCode != null)
				$scope.WBSCode = $scope.getWBSView.WBSCode;
					
				if($scope.getWBSView.name != null)
				$scope.WBSname = $scope.getWBSView.name;
	
				if($scope.getWBSView.FundCentreCode != null)
				$scope.FundCentreID = $scope.getWBSView.FundcenterUniqueID;
	
				if($scope.getWBSView.FundCenter_Description != null)	
				$scope.FundCenterDescription = $scope.getWBSView.FundCenter_Description;

			});
			
		}
		$scope.FromUpdate = function () {
			var data = JSON.stringify($('.updateForm').serializeObject());
			var validate = JSON.parse(data);
			// if ((validate.CATEGORY).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Category is Mandatory' }); return false; }
			// else if ((validate.SUBCATEGORY).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Sub Category is Mandatory' }); return false; }
			if ((validate.WBSCode).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Code is Mandatory' }); return false; }
			else if ((validate.WBSName).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Name is Mandatory' }); return false; }
			else if ((validate.FundCentreID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center Code is Mandatory' }); return false; }
			else if ((validate.FundCenterDescription).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center Description is Mandatory' }); return false; }
			{

				var obj = { "UniqueID": validate.UniqueID, "WBSID": validate.WBSID, "WBSCode": validate.WBSCode, "FundCentreID": validate.FundCentreID, "FundCentreCode": validate.FundCenterDescription }
				WBSFundCenterFactory.updateWBSFundCenter(obj)
					.success(
					function (response) {
						if (response.info.errorcode == '0') {
							$('#editWBSFundCenter').modal('hide');
						}
						AlertMessages.alertPopup(response.info);
						$scope.WBFFundcentertList();
						//$state.reload();
					}
					);
			}
		}
		$scope.SetDeleteData = function (data) {
			$scope.UniqueID = data;
		};

		$scope.FormDelete = function () {
			var data = JSON.stringify($('.deleteForm').serializeObject());
			var validate = JSON.parse(data);

			WBSFundCenterFactory.DeleteWbs_Fundcenter(data).success(function (response) {
				if (response.info.errorcode == '0') { $('#DeleteWBSPopUp').modal('hide'); }
				AlertMessages.alertPopup(response.info);
				//$state.reload();
				$scope.hideScreen();
				$scope.WBFFundcentertList();
			});
		}
		$scope.updatePopoverData = function (a, b) { $rootScope.EmailSubject = a; $rootScope.EmailTemplateValue = b; }
		$scope.WBFFundcentertList();
	})