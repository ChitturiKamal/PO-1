angular.module('TEPOApp')
	.controller('HSNTaxCodeMapCtrl', function ($scope, $rootScope, $sessionStorage, $element, HSNTaxCodeMapFactory, AlertMessages, $state) {
		$scope.pagenumber = 1; $scope.pageprecounts = 10;
		$scope.currentdate = new Date();
		$scope.getHSNTaxList= function () {
			if ($scope.pagenumber <= '0') $scope.pagenumber = 1;
			var data = JSON.parse(JSON.stringify($('.SearchHSN').serializeObject()));
			var FinalData={"HSNFilter":{"Applicability":data.Applicability,"Destination":data.Destination,"Vend_Region":data.Vend_Region,"Delivery_Plant":data.Delivery_Plant,"HSN_Code":data.HSN_Code,"Material_GST_Appl":data.Material_GST_Appl,"Vend_GST_Appl":data.Vend_GST_Appl,"TAX_Type":data.TAX_Type,"TAX_Code":data.TAX_Code,"TAX_Rate":data.TAX_Rate},"pageNumber":$scope.pagenumber,"pagePerCount": $scope.pageprecounts};
			$rootScope.loading = true;
			HSNTaxCodeMapFactory.GetHSNTaxRate(FinalData).success(function (response) {
				$rootScope.loading = false;
				$scope.HSNTaxList = response;
            });
		}
		$scope.getHSNTaxList();
		
		$scope.pagenationss = function (a, b) { var b = $("select[name='pageprecounts']").val(); $scope.pagenumber = a; $scope.pageprecounts = b; $scope.getHSNTaxList(); }
		$scope.pagenationss_clk = function (a, b) { if ("0" == b) var a1 = --a; else var a1 = ++a; "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1; var c = $("select[name='pageprecounts']").val(); $scope.pagenumber = a1; $scope.pageprecounts = c; $scope.getHSNTaxList(); }
    
		 $scope.getstatesdata = function () {
		 	$rootScope.loading = true;
		 	HSNTaxCodeMapFactory.getStates().success(function (response) {
		 		$rootScope.loading = false;
		 		$scope.stateList = response.result;
		 	});
		 }
		$scope.getstatesdata();

		$scope.getCountrydata = function () {
			$rootScope.loading = true;
			HSNTaxCodeMapFactory.getCountryList().success(function (response) {
				$rootScope.loading = false;
				$scope.countryList = response.result;
			});
		}
	   $scope.getCountrydata();

		 $scope.getHSNCodedata = function () {
			$rootScope.loading = true;
			 HSNTaxCodeMapFactory.getHSNCodeList().success(function (response) {
			 	$rootScope.loading = false;
			 	$scope.HSNCodeList = response.result;
			 });
		}
		$scope.getHSNCodedata();

		$scope.getMatGstdata = function(){
				$rootScope.loading = true;
				HSNTaxCodeMapFactory.GetMatAppl({"Applicable_To":"Material"}).success(function (response) {
					$rootScope.loading = false;
					$scope.MatGSTList = response.result;
				});			
		}
		$scope.getMatGstdata();

		$scope.getVenGstdata = function(){
			$rootScope.loading = true;
			HSNTaxCodeMapFactory.GetMatAppl({"Applicable_To":"Vendor"}).success(function (response) {
				$rootScope.loading = false;
				$scope.VenGSTList = response.result;
			});			
	}
	$scope.getVenGstdata();

		$scope.FromCreate = function () {
			
			var MatGST = '';
			var data = JSON.stringify($('.addForm').serializeObject());
			var obj = $('.addForm').serializeObject();
			var validate = JSON.parse(data);
			if ((validate.HSNCNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'HSN Code is Mandatory' }); return false; }
			else if ((validate.AppliID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Applicable To is Mandatory' }); return false; }
			// else if ((validate.GSTVenClass).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GSTN Vendor Classification is Mandatory' }); return false; }
			else if ((validate.VendStateID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Region is Mandatory' }); return false; }
			else if ((validate.DelPlReg).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Delivery Plant Region is Mandatory' }); return false; }
			else if ((validate.CountryCode).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Destination Country is Mandatory' }); return false; }
			else if ((validate.VenGstNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor GST Applicablity is Mandatory' }); return false; }
			else if((validate.AppliID).trim() == 'Material' && (validate.MatGstNam).trim() == '')
			{ AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Material GST Applicablity is Mandatory' }); return false; }
			else if ((validate.TaxYpNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Tax Type is Mandatory' }); return false; }
			else if ((validate.TaxCodeNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Tax Code is Mandatory' }); return false; }
			else if ((validate.TaxRatNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Tax Rate is Mandatory' }); return false; }
			
			else if ((validate.ValFrmNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Valid From is Mandatory' }); return false; }
			else if ((validate.ValToNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Valid To is Mandatory' }); return false; }
			else if(new Date(validate.ValFrmNam) > new Date(validate.ValToNam))
			{AlertMessages.alertPopup({ errorcode: '1', errormessage: 'From Date is Greater than To Date' }); return false;}

			else {
				if((validate.AppliID).trim() == 'Material'){MatGST = validate.MatGstNam;}
				var obj = {
					"ApplicableTo": validate.AppliID,
        			"DestinationCountry": validate.CountryCode,
        			"VendorRegionCode": validate.VendStateID,
        			"DeliveryPlantRegionCode": validate.DelPlReg,
        			// "GSTVendorClassification": validate.GSTVenClass,
        			"HSNCode": validate.HSNCNam,
        			"MaterialGSTApplicability": MatGST,
        			"VendorGSTApplicability": validate.VenGstNam,
        			"ValidFrom": validate.ValFrmNam,
        			"ValidTo": validate.ValToNam,
        			"TaxType": validate.TaxYpNam,
        			"TaxCode": validate.TaxCodeNam,
        			"TaxRate": validate.TaxRatNam,
				}
				HSNTaxCodeMapFactory.SaveHSNCodeDetails(obj).success(function (response) {
					if (response.info.errorcode == '0') {
						$('.addForm')[0].reset();
						$('#addHSNTaxCode').modal('hide');
					}
					AlertMessages.alertPopup(response.info);
					$state.reload();
					$scope.hideScreen();
				});
			
			$scope.ClearFormData();
			}
		}
	 	$scope.FromView = function (id) {
			
	 		HSNTaxCodeMapFactory.getHSNCodeById({ "HsnTaxCodeID": id }).success(function (response) {
	 			if (response.info.errorcode == '0') {
					 $scope.HSNCode = response.result;
					 
	 				if ($scope.HSNCode != null) {
						$scope.HsnTaxCodeID = id;
						if($scope.HSNCode.ApplicableTo != null)
							$scope.AppliID = $scope.HSNCode.ApplicableTo;
						if($scope.HSNCode.DestinationCountry != null)
							$scope.CountryCode = $scope.HSNCode.DestinationCountry;
						if($scope.HSNCode.VendorRegionCode != null)
						 	$scope.VendStateID = $scope.HSNCode.VendorRegionCode;
						 if($scope.HSNCode.DeliveryPlantRegionCode != null)
							 $scope.DelPlReg = $scope.HSNCode.DeliveryPlantRegionCode;
							 //alert(($scope.HSNCode.GSTVendorClassification).toString());
						//  if($scope.HSNCode.GSTVendorClassification != null)
						//  	$scope.GSTVenClass = ($scope.HSNCode.GSTVendorClassification).toString();
						 if($scope.HSNCode.HSNCode != null)
							 $scope.HSNCNam = $scope.HSNCode.HSNCode;
							 //alert($scope.HSNCode.MaterialGSTApplicability);
						 if($scope.HSNCode.MaterialGSTApplicability != null)
						 	$scope.MatGstNam = ($scope.HSNCode.MaterialGSTApplicability).toString();
						 if($scope.HSNCode.VendorGSTApplicability != null)
							$scope.VenGstNam = ($scope.HSNCode.VendorGSTApplicability).toString();
							if($scope.HSNCode.TaxType != null)
							$scope.TaxYpNam = $scope.HSNCode.TaxType;
						if($scope.HSNCode.TaxCode != null)
							$scope.TaxCodeNam = $scope.HSNCode.TaxCode;
						if($scope.HSNCode.TaxRate != null)
							$scope.TaxRatNam = $scope.HSNCode.TaxRate;
						if($scope.HSNCode.ValidFrom != null)
							$scope.ValUpdFrmNam = new Date(new Date(new Date($scope.HSNCode.ValidFrom).addDays(1)).toISOString().split("T")[0]);
						if($scope.HSNCode.ValidTo != null)
							$scope.ValUpdToNam = new Date(new Date(new Date($scope.HSNCode.ValidTo).addDays(1)).toISOString().split("T")[0]);
						

	 				}
	 			}
	 		});
		 }
		 
		 $scope.ValueChange = function(TaxRatNam){
			if(typeof(TaxRatNam) == "undefined")
			{
				$scope.TaxRatNam = parseFloat('0');
			}
			else
			{
				
			var SplitValue = (TaxRatNam.toString()+'').split(".");
			//alert( SplitValue[0] +"." + SplitValue[1].substring(0,2));
			var RepVal = parseFloat(SplitValue[0] +"." + SplitValue[1].substring(0,2));
			$scope.TaxRatNam = RepVal;
			
			}
			
		 }

		 

		 $scope.FromUpdate = function () {
			var MatGST = '';
			var data = JSON.stringify($('.updateForm').serializeObject());
			var validate = JSON.parse(data);
			//if ((validate.HSNCNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'HSN Code is Mandatory' }); return false; }
			if ((validate.AppliID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Applicable To is Mandatory' }); return false; }
			// else if ((validate.GSTVenClass).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GSTN Vendor Classification is Mandatory' }); return false; }
			else if ((validate.VendStateID).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Region is Mandatory' }); return false; }
			else if ((validate.DelPlReg).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Delivery Plant Region is Mandatory' }); return false; }
			else if ((validate.CountryCode).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Destination Country is Mandatory' }); return false; }
			else if ((validate.VenGstNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor GST Applicablity is Mandatory' }); return false; }
			else if((validate.AppliID).trim() == 'Material' && (validate.MatGstNam).trim() == '')
			{ AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Material GST Applicablity is Mandatory' }); return false; }
			else if ((validate.TaxYpNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Tax Type is Mandatory' }); return false; }
			else if ((validate.TaxCodeNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Tax Code is Mandatory' }); return false; }
			else if ((validate.TaxRatNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Tax Rate is Mandatory' }); return false; }
			else if ((validate.ValUpdFrmNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Valid From is Mandatory' }); return false; }
			else if ((validate.ValUpdToNam).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Valid To is Mandatory' }); return false; }
			else if(new Date(validate.ValUpdFrmNam) > new Date(validate.ValUpdToNam))
			{AlertMessages.alertPopup({ errorcode: '1', errormessage: 'From Date is Greater than To Date' }); return false;}

			else{
				if((validate.AppliID).trim() == 'Material'){MatGST = validate.MatGstNam;}
				var obj = {
					"ApplicableTo": validate.AppliID,
        			"DestinationCountry": validate.CountryCode,
        			"VendorRegionCode": validate.VendStateID,
        			"DeliveryPlantRegionCode": validate.DelPlReg,
        			//"GSTVendorClassification": validate.GSTVenClass,
        			//"HSNCode": validate.HSNCNam,
        			"MaterialGSTApplicability": MatGST,
        			"VendorGSTApplicability": validate.VenGstNam,
        			"ValidFrom": validate.ValUpdFrmNam,
        			"ValidTo": validate.ValUpdToNam,
        			"TaxType": validate.TaxYpNam,
        			"TaxCode": validate.TaxCodeNam,
					"TaxRate": validate.TaxRatNam,
					"UniqueID":$scope.HsnTaxCodeID
				}
			HSNTaxCodeMapFactory.UpdateHSNCodeDetails(obj).success(function (response) {
				if (response.info.errorcode == '0') {
					$('#updateHSNTaxCode').modal('hide');
				}
				AlertMessages.alertPopup(response.info);
				$scope.getHSNTaxList();
				$scope.ClearFormData();
				$scope.hideScreen();
				
			});
			
		}
	 	}

		$scope.SetDeleteData = function (data) {
			$scope.HsnTaxCodeID = data;
		};

		$scope.FormDelete = function () {
			var data = JSON.stringify($('.deleteForm').serializeObject());
			//alert(data)
			var validate = JSON.parse(data);
			
			HSNTaxCodeMapFactory.DeleteHSNCodeDetails(data).success(function (response) {
				if (response.info.errorcode == '0') { $('#DeleteHSNTaxCode').modal('hide'); }
				AlertMessages.alertPopup(response.info);
				$state.reload();
				$scope.hideScreen();
			});
		}

		$scope.ClearFormData = function(){
			$scope.HSNCNam = "";
			$scope.AppliID = "";
			//$scope.GSTVenClass = "";
			$scope.VendStateID = "";
			$scope.DelPlReg = "";
			$scope.CountryCode = "";
			$scope.VenGstNam = "";
			$scope.MatGstNam = "";
			$scope.TaxYpNam = "";
			$scope.TaxRatNam = "";
			$scope.TaxCodeNam = "";
			$scope.ValFrmNam = "";
			$scope.ValToNam = "";

		}
	// 	$scope.updatePopoverData = function (a, b) { $rootScope.EmailSubject = a; $rootScope.EmailTemplateValue = b; }
	//  })
	// .directive("multipleApproval", function () {
	// 	return {
	// 		template: '<div class="headtxt-big" style="margin: 10px 0;" ng-repeat="data in MultiApproval">\
	// 				<div class="row" style="margin-right: -3px;margin-left: 23%;">\
	// 				<div class="col-sm-10 plr2">\
	// 				<select class="form-control input-css" name="ApproverName" ng-model="data.UserId" ng-change="getApprover(data.UserId,$index+2);">\
	// 				<option value="" style="display: none;"></option>\
	// 				<option ng-repeat="data in Submitter.result" value="{{data.UserId}}">{{data.UserName}}</option>\
	// 				</select>\
	// 				</div>\
	// 				<div class="col-sm-2 plr2"><a href="javascript:void(0);" style="margin-left:5px; margin-top:2px;float:left;" ng-click="RemoveApprovalList($index);"><i class="fa fa-times" style="font-size:10pt;"></i></a></div>\
	// 				</div>\
	// 				</div>'
	// 	};
	})