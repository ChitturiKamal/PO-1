angular.module('TEPOApp')
	.controller('VendorMasterCtrl', function ($scope, $rootScope, $sessionStorage, $element, vendorMasterFactory, AlertMessages, $sce, $state, $timeout){
		$scope.pagenumber=1;$scope.pageprecounts=20;
		$scope.GetVendorList=function(){
			$rootScope.loading = true;
			if ($scope.pagenumber <= '0') $scope.pagenumber = 1;
			vendorMasterFactory.getAllVendorDetails({ 'page_number': $scope.pagenumber, 'pagepercount': $scope.pageprecounts }).success(function (response) {
				$rootScope.loading = false;
				$scope.result = response;
			});
		};$scope.GetVendorList();
		$scope.pagenationss = function (a, b) { var b = $("select[name='pageprecounts']").val(); $scope.pagenumber = a; $scope.pageprecounts = b; $scope.GetVendorList(); }
		$scope.pagenationss_clk = function (a, b) { if ("0" == b) var a1 = --a; else var a1 = ++a; "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1; var c = $("select[name='pageprecounts']").val(); $scope.pagenumber = a1; $scope.pageprecounts = c; $scope.GetVendorList(); }
		//For Delete Vendor Starts
		$scope.ForDeleteVendor=function(vedorData){$scope.VendorMainDetails=vedorData;}
		$scope.deleteVendor=function(){
			var data = JSON.stringify($('.deleteForm').serializeObject());
			vendorMasterFactory.deletemainvendor(data).success(function (response) {
				if (response.info.errorcode=='0'){$('#DeleteVendorPopUp').modal('hide');$scope.GetVendorList();}
				AlertMessages.alertPopup(response.info);
			});
		}
		$scope.VMData={};
		//For Delete Vendor Ends
		$scope.VndrShippingChange = function(a,b,c){
			if(b){
				var data = JSON.stringify($('.addForm').serializeObject());
				var validate = JSON.parse(data);
				$('.ShipCode_'+a).val(validate.BillingPostalCode[(a+2)]);
				$('.ShipCity_'+a).val(validate.BillingCity[(a+2)]);
				$('.ShipAdd_'+a).val(validate.BillingAddress[(a+2)]);
			}
		}
		$scope.prevent = function(e)
		{
			var valid = (e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) || (e.which >= 97 && e.which <= 122);
			if (!valid) {
    			e.preventDefault();
			}
		}
		// $('.preventSpcl').bind('keypress', function(e){
		// 	var valid = (e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) || (e.which >= 97 && e.which <= 122);
		// 	if (!valid) {
    	// 		e.preventDefault();
		// 	}
		// })
		$scope.VndrShippingChangeUpdate = function(a,b,c){
			if(b){
				var data = JSON.stringify($('.updateForm').serializeObject());
				var validate = JSON.parse(data);
				$('.ShipCode_Up_'+a).val(validate.BillingPostalCode[(a+2)]);
				$('.ShipCity_Up_'+a).val(validate.BillingCity[(a+2)]);
				$('.ShipAdd_Up_'+a).val(validate.BillingAddress[(a+2)]);
			}
		}
		$scope.AddShippingDetails=function(){
			// if($scope.vendorBillingDetails[0] != undefined){
			// 	$scope.vendorBillingDetails.push({'id':'0','VendorAccountGroup':$scope.vendorBillingDetails[0].VendorAccountGroup,'VendorAccountGroupId':$scope.vendorBillingDetails[0].VendorAccountGroupId});
			// }
			// else{
				$scope.vendorBillingDetails.push({'id':'0'});
			// }
			$scope.RepresentorData.push({'id':'0'});
			$scope.WithholdingTaxDetailsListMUl.push([[]]);
		};
		$scope.RemoveElementList = function (index){
			$scope.vendorBillingDetails.splice(index,1);
			$scope.RepresentorData.splice(index,1);
			$scope.WithholdingTaxDetailsListMUl.splice(index,1);
		};
		//With Holding Tax Details Starts
		$scope.WithholdingTaxDetailsListMUl=[];
		$scope.PushNewTaxDetMul=function(a){
			$scope.WithholdingTaxDetailsListMUl[a].push({"id":"C"});
		};
		$scope.RemoveNewTaxDetMul=function (index,a){$scope.WithholdingTaxDetailsListMUl[a].splice(index, 1);};
		//With Holding Tax Details Ends

		$scope.getstatesdata=function(){vendorMasterFactory.getStates().success(function (response){$scope.statesList=response.result;});}
		$scope.VendorAccountGroups=function(){vendorMasterFactory.VendorAccountGroups().success(function (response){$scope.VendorGroupsList=response.result;});}
		$scope.VendorAccountCategories=function(){vendorMasterFactory.VendorAccountCategories().success(function (response){$scope.VendorCategoryList=response.result;});}
		$scope.VendorSchemaGroups=function(){vendorMasterFactory.VendorSchemaGroups().success(function (response){$scope.VendorSchemaList=response.result;});}
		$scope.VendorWithHoldTaxTypes=function(){vendorMasterFactory.VendorWithHoldTaxTypes().success(function(response){$scope.VendorWithHoldTaxList=response.result;});}
		$scope.vendorGSTApplicabilities=function(){vendorMasterFactory.vendorGSTApplicabilities().success(function (response){$scope.GSTApplicabilityList=response.result;});}
		$scope.getCountrydata=function(){vendorMasterFactory.getCountryList().success(function(response){$scope.countryList=response.result;});}
		$scope.getcurrencies=function(){vendorMasterFactory.getcurrencies().success(function (response){$scope.currencyList = response.result;});}
		$scope.getGLAccountdata=function(){vendorMasterFactory.glaccountdetails().success(function(response){$scope.GLAccountList=response.result;});}
		$scope.CountryDrop = vendorMasterFactory.CountryDropList();
		$scope.OnUpdate=function(){
			$scope.getcurrencies();
			$scope.getCountrydata();
			$scope.getstatesdata();
			$scope.VendorAccountGroups();
			$scope.VendorAccountCategories();
			$scope.VendorSchemaGroups();
			$scope.VendorWithHoldTaxTypes();
			$scope.vendorGSTApplicabilities();
			$scope.getGLAccountdata();
		}
		$scope.getVendorDetailsById=function(a,b){
			if(a){
				$scope.TransferContact={};
				$scope.RepresentorData=[];$scope.WithholdingTaxDetailsListMUl=[];
				vendorMasterFactory.getVendorDataById({"VendorMasterId":a}).success(function (response) {
					if(response.info.errorcode==0){
						$scope.VMData=response.result;
						if(b==2){
							$scope.TransferContact.UniqueID=response.result.VendorContactId;
							$scope.TransferContact.ContactName=response.result.VendorName;
							$scope.vendorBillingDetails=response.result.VendorMasterDetails;
							for(var i=0;i<$scope.vendorBillingDetails.length;i++){
								var edc={"ContactMobile":$scope.vendorBillingDetails[i].RepresentContactNumber,"ContactEmail":$scope.vendorBillingDetails[i].RepresentEmailID,"ContactName":$scope.vendorBillingDetails[i].RepresentName,"UniqueID":$scope.vendorBillingDetails[i].RepresentContactId};
								$scope.RepresentorData.push(edc);
								$scope.WithholdingTaxDetailsListMUl.push($scope.vendorBillingDetails[i].WithHoldApplicabilityDetails);
							}
							$("#EditvendorMaster").modal('show');
							$scope.OnUpdate();
						}else $("#viewvendorMaster").modal('show');
					}else{AlertMessages.alertPopup(response.info);return false;}
				});
			}else{AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor is Mandatory'});return false;}
		}
		$scope.OnCreation=function(){
			$scope.getcurrencies();
			$scope.getCountrydata();
			$scope.getstatesdata();
			$scope.VendorAccountGroups();
			$scope.VendorAccountCategories();
			$scope.VendorSchemaGroups();
			$scope.VendorWithHoldTaxTypes();
			$scope.vendorGSTApplicabilities();
			$scope.getGLAccountdata();
			$scope.vendorBillingDetails=[];
			$scope.RepresentorData=[];
			$scope.TransferContact={};
			$scope.AddShippingDetails();
		}
		$scope.rDups=function(a){
			var arr=[];
			for(var e=0;e<a.length;e++){
				if(a[e]!=0 && (a[e]).trim()!=""){
					arr.push(a[e]);
				}
			}
			if(arr.length>0) return arr.every(num => arr.indexOf(num) === arr.lastIndexOf(num));
			else return true;
		}
		// function checkBillingPostalCodeWithIndia(BPCodeID,countryId)
		// {
		// 	var tempCount = 0;
		// 	for(var i=2; i<BPCodeID.length;i++){
		// 		if(countryId[2] == '101'){
		// 			if(BPCodeID[i].length != 6)tempCount++;
		// 		}
		// 	}
		// 	return tempCount > 0 ? true : false; 
		// }
		// function checkBillingPostalCodeWithForeign(BPCodeID,countryId)
		// {
		// 	var tempCount = 0;
		// 	for(var i=2; i<BPCodeID.length;i++){
		// 		if(countryId[2] != '101'){
		// 			if(BPCodeID[i].length!=6){
		// 				tempCount++;
		// 			}
		// 		}
		// 	}
		// 	return tempCount > 0 ? true : false; 
		// }
		// function checkShippingPostalCodeWithIndia(SHCodeID,countryId)
		// {
		// 	var tempCount = 0;
		// 	for(var i=2; i<SHCodeID.length;i++){
		// 		if(countryId[2] == '101'){
		// 			if(SHCodeID[i].length != 6)tempCount++;
		// 		}
		// 	}
		// 	return tempCount > 0 ? true : false; 
		// }
		// function checkShippingPostalCodeWithForeign(SHCodeID,countryId)
		// {
		// 	var tempCount = 0;
		// 	for(var i=2; i<SHCodeID.length;i++){
		// 		if(countryId[2] != '101'){
		// 			if(SHCodeID[i].length!=6)tempCount++;
		// 		}
		// 	}
		// 	return tempCount > 0 ? true : false; 
		// }
		function checkPostalCodeLength(postalCode)
		{
			var tempCount = 0;
			for(var i=2; i<postalCode.length;i++){
					if(postalCode[i].length!=6)tempCount++;
			}
			return tempCount > 0 ? true : false; 
		}
		$scope.saveVendordetails = function () {
			var data = JSON.stringify($('.addForm').serializeObject());
			var validate = JSON.parse(data);
			if ((validate.VendorContactId).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Name is mandatory' });return false;}
			else if ((validate.VendorName).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Name is mandatory' });return false;}
			else if ((validate.Currency).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Currency is mandatory' });return false;}
			else if ((validate.Currency).trim()=="INR" && (validate.PAN).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PAN Number is mandatory' });return false;}
			else if ((validate.RepresentContactId).length != getArrayCounts(validate.RepresentContactId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Representative is Mandatory'});return false;}
			else if ((validate.RepresentName).length != getArrayCounts(validate.RepresentName)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Representative is Mandatory'});return false;}
			else if ((validate.BillingPostalCode).length != getArrayCounts(validate.BillingPostalCode)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Zip/Postal Code is Mandatory'});return false;}
			else if ((validate.BillingCity).length != getArrayCounts(validate.BillingCity)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing City is Mandatory'});return false;}
			else if ((validate.BillingAddress).length != getArrayCounts(validate.BillingAddress)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Address is Mandatory'});return false;}
			else if ((validate.ShippingPostalCode).length != getArrayCounts(validate.ShippingPostalCode)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Zip/Postal Code is Mandatory'});return false;}
			else if ((validate.ShippingCity).length != getArrayCounts(validate.ShippingCity)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping City is Mandatory'});return false;}
			else if ((validate.ShippingAddress).length != getArrayCounts(validate.ShippingAddress)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Address is Mandatory'});return false;}
			else if ((validate.CountryId).length != getArrayCounts(validate.CountryId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Country is Mandatory'});return false;}

			// else if (checkPostalCodeLength(validate.BillingPostalCode)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Postal code minimum length must be 6'});return false}
			// else if (checkPostalCodeLength(validate.ShippingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Postal code minimum length must be 6'});return false}
			// else if (checkBillingPostalCodeWithIndia(validate.BillingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Postal code minimum length must be 6'});return false}
			// else if (checkBillingPostalCodeWithForeign(validate.BillingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Postal Code length must be 5 or 10'});return false}
			// else if (checkShippingPostalCodeWithIndia(validate.ShippingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Postal code minimum length must be 6'});return false}
			// else if (checkShippingPostalCodeWithForeign(validate.ShippingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Postal Code length must be 5 or 10'});return false}
			
			else if ((validate.GSTApplicabilityId).length != getArrayCounts(validate.GSTApplicabilityId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'GST Applicability is Mandatory'});return false;}
			//else if (((validate.GSTIN).length != getArrayCounts(validate.GSTIN))) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'GSTIN is Mandatory'});return false;}
			else if (checkGSTApplicabilityIdGSTIN(validate.GSTApplicabilityId,validate.GSTIN)) {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GSTIN is mandatory' });return false;}
			else if ((validate.VendorAccountGroupId).length != getArrayCounts(validate.VendorAccountGroupId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Accounting Group is Mandatory'});return false;}
			else if ((validate.VendorCategoryId).length != getArrayCounts(validate.VendorCategoryId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Vendor Category is Mandatory'});return false;}
			else if ((validate.GLAccountId).length != getArrayCounts(validate.GLAccountId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'GL Account is Mandatory'});return false;}
			else if ((validate.SchemaGroupId).length != getArrayCounts(validate.SchemaGroupId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Schema Group is Mandatory'});return false;}
			//else if (!$scope.rDups(validate.GSTIN)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Duplicate GSTIN not allowed'});return false;}
			else{
				var finaData={"VendorContactId":validate.VendorContactId,"VendorName":validate.VendorName,"Currency":validate.Currency,"PAN":validate.PAN,"CIN":validate.CIN,"ServiceTax":validate.ServiceTax,"VendorMasterDetails":[]};
				var cnt=0;var fc=0;var lc=0;var wiScre=[];
				for(var i=0;i<validate.WithholdFor.length;i++){
					wiScre.push([]);
					if(i==0){fc=0;lc=0}
					else{fc=cnt; var lc=(parseInt(cnt)+parseInt(validate.WithholdFor[i])-1);}
					for(var j=fc;j<=lc;j++){
						var AccDet ={"WithHoldingTaxTypeId":validate.WithholdTaxTypeId[j],"WithHoldingCodeId":validate.WithholdTaxCodeId[j],"WithHoldingApplicability":validate.WithholdApplicability[j]};
						wiScre[i].push(AccDet);
					}
					cnt=parseInt(cnt)+parseInt(validate.WithholdFor[i]);
				}
				var acre=[];
				for(var i=0;i<validate.BillingAddress.length;i++){
					if(validate.BillingAddress[i]!=0 && (validate.BillingAddress[i]).trim() !=""){
						var AccDet ={"VendorCode":"","BillingAddress":validate.BillingAddress[i],"BillingCity":validate.BillingCity[i],"BillingPostalCode":validate.BillingPostalCode[i],"ShippingAddress":validate.ShippingAddress[i],"ShippingCity":validate.ShippingCity[i],"ShippingPostalCode":validate.ShippingPostalCode[i],"RegionId":validate.RegionId[i],"CountryId":validate.CountryId[i],"GSTApplicabilityId":validate.GSTApplicabilityId[i],"GSTIN":validate.GSTIN[i],"VendorAccountGroupId":validate.VendorAccountGroupId[i],"VendorCategoryId":validate.VendorCategoryId[i],"SchemaGroupId":validate.SchemaGroupId[i],"GLAccountId":validate.GLAccountId[i],"BankAccountName":validate.BankAccountName[i],"BankAccountNumber":validate.BankAccountNumber[i],"BankName":validate.BankName[i],"IFSCCode":validate.IFSCCode[i],"RepresentName":validate.RepresentName[i],"RepresentContactNumber":validate.RepresentContactNumber[i],"RepresentEmailID":validate.RepresentEmailID[i],"RepresentContactId":validate.RepresentContactId[i],"WithHoldApplicabilityDetails":wiScre[i]};
						acre.push(AccDet);
					}
				}
				finaData.VendorMasterDetails=acre;
				$rootScope.loading=true;
				vendorMasterFactory.saveVendor(finaData).success(function (response){
					$rootScope.loading=false;
					if(response.info.errorcode==0){
						$("#addvendorMaster").modal('hide');
						AlertMessages.alertPopup(response.info);
						setTimeout(() => {$state.reload();}, 300); 
					}else{AlertMessages.alertPopup(response.info);}
				});
			}
		}
		function checkGSTApplicabilityIdGSTIN(GSTApplicabilityId,GSTIN){
			var tempCount = 0;
			for(var i=2; i<GSTApplicabilityId.length; i++)if(GSTApplicabilityId[i]!=6 && GSTIN[i].trim()=="")tempCount++;
			return tempCount > 0 ? true : false;
		}
		$scope.UpdateVendordetails = function () {
			var data = JSON.stringify($('.updateForm').serializeObject());
			var validate = JSON.parse(data);
			if ((validate.VendorContactId).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Name is mandatory' });return false;}
			else if ((validate.VendorName).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor Name is mandatory' });return false;}
			else if ((validate.Currency).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Currency is mandatory' });return false;}
			else if ((validate.Currency).trim()=="INR" && (validate.PAN).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PAN Number is mandatory' });return false;}
			else if ((validate.RepresentContactId).length != getArrayCounts(validate.RepresentContactId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Representative is Mandatory'});return false;}
			else if ((validate.RepresentName).length != getArrayCounts(validate.RepresentName)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Representative is Mandatory'});return false;}
			else if ((validate.IsActive).length != getArrayCounts(validate.IsActive)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'SAP Status is Mandatory'});return false;}
			else if ((validate.BillingPostalCode).length != getArrayCounts(validate.BillingPostalCode)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Zip/Postal Code is Mandatory'});return false;}
			else if ((validate.BillingCity).length != getArrayCounts(validate.BillingCity)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing City is Mandatory'});return false;}
			else if ((validate.BillingAddress).length != getArrayCounts(validate.BillingAddress)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Address is Mandatory'});return false;}
			else if ((validate.ShippingPostalCode).length != getArrayCounts(validate.ShippingPostalCode)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Zip/Postal Code is Mandatory'});return false;}
			else if ((validate.ShippingCity).length != getArrayCounts(validate.ShippingCity)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping City is Mandatory'});return false;}
			else if ((validate.ShippingAddress).length != getArrayCounts(validate.ShippingAddress)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Address is Mandatory'});return false;}
			else if ((validate.CountryId).length != getArrayCounts(validate.CountryId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Country is Mandatory'});return false;}
			
			// else if (checkBillingPostalCodeWithIndia(validate.BillingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Postal code minimum length must be 6'});return false}
			// else if (checkBillingPostalCodeWithForeign(validate.BillingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Postal Code length must be 5 or 10'});return false}
			// else if (checkShippingPostalCodeWithIndia(validate.ShippingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Postal code minimum length must be 6'});return false}
			// else if (checkShippingPostalCodeWithForeign(validate.ShippingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Postal Code length must be 5 or 10'});return false}
			// else if (checkPostalCodeLength(validate.BillingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Billing Postal code minimum length must be 6'});return false}
			// else if (checkPostalCodeLength(validate.ShippingPostalCode, validate.CountryId)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Shipping Postal code minimum length must be 6'});return false}

			else if ((validate.GSTApplicabilityId).length != getArrayCounts(validate.GSTApplicabilityId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'GST Applicability is Mandatory'});return false;}
			//else if ((validate.GSTIN).length != getArrayCounts(validate.GSTIN)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'GSTIN is Mandatory'});return false;}
			else if (checkGSTApplicabilityIdGSTIN(validate.GSTApplicabilityId,validate.GSTIN)) {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GSTIN is mandatory' });return false;}
			else if ((validate.VendorAccountGroupId).length != getArrayCounts(validate.VendorAccountGroupId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Accounting Group is Mandatory'});return false;}
			else if ((validate.VendorCategoryId).length != getArrayCounts(validate.VendorCategoryId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Vendor Category is Mandatory'});return false;}
			else if ((validate.GLAccountId).length != getArrayCounts(validate.GLAccountId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'GL Account is Mandatory'});return false;}
			else if ((validate.SchemaGroupId).length != getArrayCounts(validate.SchemaGroupId)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Schema Group is Mandatory'});return false;}
			//else if (!$scope.rDups(validate.GSTIN)) {AlertMessages.alertPopup({errorcode: '1',errormessage: 'Duplicate GSTIN not allowed'});return false;}
			else{
				var finaData={"POVendorMasterId":validate.POVendorMasterId,"VendorContactId":validate.VendorContactId,"VendorName":validate.VendorName,"Currency":validate.Currency,"PAN":validate.PAN,"CIN":validate.CIN,"ServiceTax":validate.ServiceTax,"IsActive":validate.IsActiveFugue,"VendorMasterDetails":[]};
				var cnt=0;var fc=0;var lc=0;var wiScre=[];
				for(var i=0;i<validate.WithholdFor.length;i++){
					wiScre.push([]);
					if(i==0){fc=0;lc=0}
					else{fc=cnt; var lc=(parseInt(cnt)+parseInt(validate.WithholdFor[i])-1);}
					for(var j=fc;j<=lc;j++){
						var edc=validate.POVendorWithHoldApplicabilityDetailId[j];
						if(edc==null || edc=="")edc=0;
						var AccDet ={"POVendorWithHoldApplicabilityDetailId":edc,"WithHoldingTaxTypeId":validate.WithholdTaxTypeId[j],"WithHoldingCodeId":validate.WithholdTaxCodeId[j],"WithHoldingApplicability":validate.WithholdApplicability[j]};
						wiScre[i].push(AccDet);
					}
					cnt=parseInt(cnt)+parseInt(validate.WithholdFor[i]);
				}
				var acre=[];
				for(var i=0;i<validate.BillingAddress.length;i++){
					if(validate.BillingAddress[i]!=0 && (validate.BillingAddress[i]).trim() !=""){
						var AccDet ={"POVendorDetailId":validate.POVendorDetailId[i],"VendorCode":validate.VendorCode[i],"BillingAddress":validate.BillingAddress[i],"BillingCity":validate.BillingCity[i],"BillingPostalCode":validate.BillingPostalCode[i],"ShippingAddress":validate.ShippingAddress[i],"ShippingCity":validate.ShippingCity[i],"ShippingPostalCode":validate.ShippingPostalCode[i],"RegionId":validate.RegionId[i],"CountryId":validate.CountryId[i],"GSTApplicabilityId":validate.GSTApplicabilityId[i],"GSTIN":validate.GSTIN[i],"VendorAccountGroupId":validate.VendorAccountGroupId[i],"VendorCategoryId":validate.VendorCategoryId[i],"SchemaGroupId":validate.SchemaGroupId[i],"GLAccountId":validate.GLAccountId[i],"BankAccountName":validate.BankAccountName[i],"BankAccountNumber":validate.BankAccountNumber[i],"BankName":validate.BankName[i],"IFSCCode":validate.IFSCCode[i],"RepresentName":validate.RepresentName[i],"RepresentContactNumber":validate.RepresentContactNumber[i],"RepresentEmailID":validate.RepresentEmailID[i],"RepresentContactId":validate.RepresentContactId[i],"IsActive":validate.IsActive[i],"WithHoldApplicabilityDetails":wiScre[i]};
						acre.push(AccDet);
					}
				}
				finaData.VendorMasterDetails=acre;
				$rootScope.loading=true;
				vendorMasterFactory.updateVendor(finaData).success(function (response){
					$rootScope.loading=false;
					if(response.info.errorcode==0){
						$("#EditvendorMaster").modal('hide');
						AlertMessages.alertPopup(response.info);
						setTimeout(() => {$state.reload();}, 300); 
					}else{AlertMessages.alertPopup(response.info);}
				});
			}
		}

		

		//Creating and Setting Contact Data Starts
		$scope.SetContactType=function(a,b){$scope.ContactSelFor=a;$scope.ContactSelKey=b;}
		$scope.SetValueForTOData = function (data) {
			if ($scope.ContactSelFor=='Vendor')$scope.TransferContact=data;
			else $scope.RepresentorData[$scope.ContactSelKey]=data;
			$("#ContactSelectingPop").modal('hide');
		}
		$scope.SetValueForRETOData = function (data) {
			if ($scope.ContactSelFor=='Vendor')$scope.TransferContact=data;
			else $scope.RepresentorData[$scope.ContactSelKey]=data;
			$("#RepresentSelectingPop").modal('hide');
		}
		$scope.pagenumberCAT = 1;
		$scope.pageprecountsCAT = 10;
		$scope.GetContactList = function (aa, bb, cc) {
			if ($scope.pagenumberCAT <= '0') $scope.pagenumberCAT = 1;
			setTimeout(function () { $('select[name="pageprecountsCAT"]').val($scope.pageprecountsCAT); }, 300);
			if (!aa) aa = "";
			if (!bb) bb = "";
			if (!cc) cc = "";
			var data = { 'ContactName': aa, 'ContactMobile': bb, 'ContactEmail': cc, 'PageNumber': $scope.pagenumberCAT, 'PageperCount': $scope.pageprecountsCAT };
			vendorMasterFactory.TEContactGetAllData(data).success(function (response) { $scope.ContactsListTransfer = response; });
		}
		$scope.pagenumberCATRE = 1;
		$scope.pageprecountsCATRE = 10;
		$scope.GetRepresntList = function (aa, bb, cc) {
			if ($scope.pagenumberCATRE <= '0') $scope.pagenumberCATRE = 1;
			setTimeout(function () { $('select[name="pageprecountsCATRE"]').val($scope.pageprecountsCATRE); }, 300);
			if (!aa) aa = "";
			if (!bb) bb = "";
			if (!cc) cc = "";
			var data = { 'ContactName': aa, 'ContactMobile': bb, 'ContactEmail': cc, 'PageNumber': $scope.pagenumberCATRE, 'PageperCount': $scope.pageprecountsCATRE };
			vendorMasterFactory.TERepresentativeGetAllData(data).success(function (response) { $scope.ContactsListTransferRe = response; });
		}
		$scope.pagenationssCAT = function (a, b) {
			var b = $("select[name='pageprecountsCAT']").val();
			$scope.pagenumberCAT = a;
			$scope.pageprecountsCAT = b;
			$scope.GetContactList($scope.CName, $scope.Cmobile, $scope.CEmail);
		}
		$scope.pagenationss_clkCAT = function (a, b) {
			if ("0" == b) var a1 = --a;
			else var a1 = ++a;
			"0" >= a1 ? (a1 = 0, $scope.pagenumberCAT = 1) : $scope.pagenumberCAT = a1;
			var c = $("select[name='pageprecountsCAT']").val();
			$scope.pagenumberCAT = a1;
			$scope.pageprecountsCAT = c;
			$scope.GetContactList($scope.CName, $scope.Cmobile, $scope.CEmail);
		}
		$scope.pagenationssCATRepresent = function (a, b) {
			var b = $("select[name='pageprecountsCAT']").val();
			$scope.pagenumberCAT = a;
			$scope.pageprecountsCAT = b;
			$scope.GetContactList($scope.CName, $scope.Cmobile, $scope.CEmail);
		}
		$scope.pagenationssCATRepresent = function (a, b) {
			var b = $("select[name='pageprecountsCATRE']").val();
			$scope.pagenumberCAT = a;
			$scope.pageprecountsCAT = b;
			$scope.GetRepresntList($scope.CName, $scope.Cmobile, $scope.CEmail);
		}
		$scope.pagenationss_clkCATRepresent = function (a, b) {
			if ("0" == b) var a1 = --a;
			else var a1 = ++a;
			"0" >= a1 ? (a1 = 0, $scope.pagenumberCAT = 1) : $scope.pagenumberCAT = a1;
			var c = $("select[name='pageprecountsCAT']").val();
			$scope.pagenumberCAT = a1;
			$scope.pageprecountsCAT = c;
			$scope.GetRepresntList($scope.CName, $scope.Cmobile, $scope.CEmail);
		}

		$scope.setCalName = function () {
			$scope.callname = '';
			var a = $('select[name="Salutation"]').val();
			var b = $('input[name="FirstName"]').val();
			var c = $('input[name="LastName"]').val();
			if (!a) { a = "" }
			if (!b) { b = "" }
			if (!c) { c = "" }
			$scope.callname = a + ' ' + b + ' ' + c;
		}
		$scope.setCalNameRE = function () {
			$scope.callname = '';
			var a = $('select[name="SalutationRE"]').val();
			var b = $('input[name="FirstNameRE"]').val();
			var c = $('input[name="LastNameRE"]').val();
			if (!a) { a = "" }
			if (!b) { b = "" }
			if (!c) { c = "" }
			$scope.callname = a + ' ' + b + ' ' + c;
		}
		
		$scope.RelationShipID = 1;
		$scope.getRelID = function () {
			$.ajax({
				type: "POST",cache: false,async: true,
				url: 'http://' + $sessionStorage.server_ip + '/TEContactManagementAPI/api/TERelationshipCategory/Get?type=Tag',
				success: function (response) { for (var ee = 0; ee < acer.length; ee++) { if (acer[ee].Name == "Vendor") { $scope.RelationShipID = acer[ee].UniqueId; } } }
			});
		};$scope.getRelID();
		$scope.CountryChange = function (a) { $scope.CountryCode = a; }
		$scope.SaveNewContactTO = function () {
			var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
			var data = JSON.stringify($('.CreateNewContactFormTO').serializeObject());
			var validate = $.parseJSON(data);
			if ((validate.FirstName).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'First Name is mandatory' }); }
			
			else if ((validate.Salutation).trim() != "M/S" && !(/^[A-Za-z0-9\s]+$/.test(validate.FirstName))){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'First Name must have a-z and A-Z and white space' }); }
			else if ((validate.CallName).trim() == "") { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Call Name is mandatory' }); }
			else if ((validate.Mobile).trim() == "" && (validate.Emailid).trim() == "") { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Contact Number or Email ID is mandatory' }); }
			else if (validate.Mobile.trim() != "" && (validate.CountryCode).trim() == "") { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Code is mandatory' }); }
			else if ((validate.Mobile).trim() != "" && parseInt(((validate.Mobile).trim()).length) > '0' && parseInt(((validate.Mobile).trim()).length) < '8') {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Mobile Number should 8-15 Digits' });return false;}
			else if ((validate.Mobile).trim() != "" && parseInt(((validate.Mobile).trim()).length) > '15') {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Mobile Number should 8-15 Digits' });return false;}
			else if ((validate.Emailid).trim() != "" && (!filter.test(validate.Emailid))) {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter Valid Email ID' });return false;}
			else {
				var FinalMobile=[];var FinalEmail=[];
				if(validate.Mobile!='') FinalMobile=[{ "CreatedBy": $scope.UserDetails.currentUser.CallName, "Mobile": validate.Mobile,"CountryCode": validate.CountryCode,"Type":"Home"}];
				if(validate.Emailid!='') FinalEmail=[{"CreatedBy":$scope.UserDetails.currentUser.CallName,"Emailid":validate.Emailid,"Type":"Home"}];
				var datafinal = JSON.stringify({ "ValidationType": "Red", "teContact": { "CreatedBy": $scope.UserDetails.currentUser.CallName, "ISProfileSPublic": false, "IsAdvertise": true, "Status": "Active", "Salutation": validate.Salutation, "FirstName": validate.FirstName, "LastName": validate.LastName, "CallName": validate.CallName, "ContactType": "Vendor", "CountryCode": validate.CountryCode, "TEContactEmails": FinalEmail, "TEContactMobiles": FinalMobile, "R_TERelationshipsOfContact": [{ "TERelationshipCategory": validate.RelationShipID, "StartDate": "", "EndDate": "", "Status": "" }] } });
				$rootScope.loading = true;
				$.ajax({
					beforeSend: function (xhrObj){xhrObj.setRequestHeader("Content-Type", "application/json"); xhrObj.setRequestHeader("Accept", "application/json");},
					url: $sessionStorage.server_ip + '/TEContactManagementAPI/api/TEContactProfile/Post',
					type: "POST",cache: false,async: false,dataType: "json",
					data: datafinal,
					success: function (response) {
						if (response.Result == null) {
							$rootScope.loading = false;
							AlertMessages.alertPopup({ errorcode: '1', errormessage: response.ValidationResult.ValidationError.Message });
							return false;
						}else if (response.Result != "") {
							$scope.PartnerCompanyContactID = response.Result;
							$('.CreateNewContactFormTO')[0].reset();
							$scope.CnewCon = false;
							setTimeout(function () { $scope.GetContactList(validate.CallName, validate.Mobile[1], validate.Emailid[1]) }, 200);
							AlertMessages.alertPopup({ errorcode: '0', errormessage: 'Contact created Successfully' });
							$rootScope.loading = false;
						}else {
							$rootScope.loading = false;
							AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Contact Cannot Be Created, Try again Later!' });
							return false;
						}
					}
				});
			}
		}
		$scope.SaveNewRepresentTO = function () {
			var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
			var data = JSON.stringify($('.CreateNewRepresnetFormTO').serializeObject());
			var validate = $.parseJSON(data);
			if ((validate.FirstNameRE).trim()==""){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'First Name is mandatory' }); }
			
			else if ((validate.SalutationRE).trim() != "M/S" && !(/^[A-Za-z\s]+$/.test(validate.FirstName))){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'First Name must have a-z and A-Z and white space' }); }
			else if ((validate.CallNameRE).trim() == "") { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Call Name is mandatory' }); }
			else if ((validate.Mobile).trim() == "" && (validate.Emailid).trim() == "") { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Contact Number or Email ID is mandatory' }); }
			else if (validate.Mobile.trim() != "" && (validate.CountryCode).trim() == "") { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Code is mandatory' }); }
			else if ((validate.Mobile).trim() != "" && parseInt(((validate.Mobile).trim()).length) > '0' && parseInt(((validate.Mobile).trim()).length) < '8') {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Mobile Number should 8-15 Digits' });return false;}
			else if ((validate.Mobile).trim() != "" && parseInt(((validate.Mobile).trim()).length) > '15') {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Mobile Number should 8-15 Digits' });return false;}
			else if ((validate.Emailid).trim() != "" && (!filter.test(validate.Emailid))) {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter Valid Email ID' });return false;}
			else {
				var FinalMobile=[];var FinalEmail=[];
				if(validate.Mobile!='') FinalMobile=[{ "CreatedBy": $scope.UserDetails.currentUser.CallName, "Mobile": validate.Mobile,"CountryCode": validate.CountryCode,"Type":"Home"}];
				if(validate.Emailid!='') FinalEmail=[{"CreatedBy":$scope.UserDetails.currentUser.CallName,"Emailid":validate.Emailid,"Type":"Home"}];
				var datafinal = JSON.stringify({ "ValidationType": "Red", "teContact": { "CreatedBy": $scope.UserDetails.currentUser.CallName, "ISProfileSPublic": false, "IsAdvertise": true, "Status": "Active", "Salutation": validate.SalutationRE, "FirstName": validate.FirstNameRE, "LastName": validate.LastNameRE, "CallName": validate.CallNameRE, "ContactType": "VendorRepresentative", "CountryCode": validate.CountryCode, "TEContactEmails": FinalEmail, "TEContactMobiles": FinalMobile, "R_TERelationshipsOfContact": [{ "TERelationshipCategory": validate.RelationShipID, "StartDate": "", "EndDate": "", "Status": "" }] } });
				$rootScope.loading = true;
				$.ajax({
					beforeSend: function (xhrObj){xhrObj.setRequestHeader("Content-Type", "application/json"); xhrObj.setRequestHeader("Accept", "application/json");},
					url: $sessionStorage.server_ip + '/TEContactManagementAPI/api/TEContactProfile/Post',
					type: "POST",cache: false,async: false,dataType: "json",
					data: datafinal,
					success: function (response) {
						if (response.Result == null) {
							$rootScope.loading = false;
							AlertMessages.alertPopup({ errorcode: '1', errormessage: response.ValidationResult.ValidationError.Message });
							return false;
						}else if (response.Result != "") {
							$scope.PartnerCompanyContactID = response.Result;
							$('.CreateNewContactFormTO')[0].reset();
							$scope.CnewCon = false;
							setTimeout(function () { $scope.GetRepresntList(validate.CallName, validate.Mobile[1], validate.Emailid[1]) }, 200);
							AlertMessages.alertPopup({ errorcode: '0', errormessage: 'Contact created Successfully' });
							$rootScope.loading = false;
						}else {
							$rootScope.loading = false;
							AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Contact Cannot Be Created, Try again Later!' });
							return false;
						}
					}
				});
			}
		}
		//Creating and Setting Contact Data Starts
	})