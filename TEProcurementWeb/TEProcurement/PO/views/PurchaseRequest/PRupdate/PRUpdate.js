'use strict';
angular.module('TEPOApp')
    .controller('PRUpdateCtrl', function ($filter, $scope, $rootScope, $element, $localStorage, $sessionStorage, PRUpdateFactory, $uibModal, AlertMessages, $state, $location) {
        $scope.shippingDet = [];
        $scope.FundCenterID = 0;
        $scope.VendorID = 0;
        $scope.tab = 1;
        $scope.prId = $localStorage.prid;
        $scope.backLink = $sessionStorage.BackLink;
        $scope.tempMaterialLists = [];
        $scope.PreviousSelectedItemsList = [];
        $scope.SelectedItemsList = [];
        $scope.SelectedMatItemsList = [];
        $scope.brandData = [];
        $scope.selectTab = function (setTab) {
            $scope.tab = setTab;
        };
        $scope.IsTabSelected = function (checkTab) {
            return $scope.tab === checkTab;
        };
        $scope.date = $filter('date')(new Date(), 'yyyy-MM-dd');
        $scope.prScheduleDate = new Date();

        $scope.DateChange = function(){
            var day = new Date();
           var x = new Date(day.getTime() + 14*24*60*60*1000);
             
             //alert(x);
             $scope.prScheduleDate = $filter('date')(x, 'yyyy-MM-dd');
            // $scope.Currdate = $filter('date')($scope.setDay, 'yyyy-MM-dd');
        }

       $scope.DateChange();
        //alert($scope.Currdate);
        $scope.checkFromSave = function () {
            if ($localStorage.fromPrSave) {
                $scope.tab = 2;
            }
        }
        $scope.checkFromSave();
        $scope.materialInformation = function (MaterialInfo) {
            $scope.MaterialFullInfo = MaterialInfo;
        }
        $scope.setBrandData = function (brandInfo,type,key) {
            if(type=='service') $scope.brandServData[key] = JSON.parse(brandInfo);
            else $scope.brandData[key] = JSON.parse(brandInfo);
        }
        // $rootScope.loading = true;
        // //     PRUpdateFactory.getFundCentersList().success(function (response) {
        // //         $rootScope.loading = false;
        // //         $scope.fundCentersList = response.result;
        // //     });
        $scope.GetFundCenterByCodeCall = function (a) {
            PRUpdateFactory.GetFundCenterByCode({'SearchText':a}).success(function(response){
                if(response.result !=null){
                    $scope.FundCenterID=response.result.Uniqueid;
                }
            });
        }
		$scope.getFundCenters = function (a) {
            if(a.length>3){
                $scope.fundCentersList=[];
                PRUpdateFactory.getFundCentersList({"SearchText":a}).success(function (response){ $scope.fundCentersList=response.result;})
                $scope.GetFundCenterByCodeCall(a);
            }else { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 4 charactors to Search' }); return false; }
        }
        $scope.GerServiceDefinition = function (serviceCode) {
            $rootScope.loading = true;
            PRUpdateFactory.GerServiceDefinitionData({ 'ServiceCode': serviceCode }).success(function (response) {
                $rootScope.loading = false;
                $scope.ClassificationDefinitionResult = response.result;
            });
        }

        $scope.UpdServiceHeaderPopup = function(UniqueId, Title, Description){
            //alert(UniqueId+"-"+ Title+"-"+  Description)
            $scope.ServUpdHead = Title;
            $scope.ServUpdHeadDesc = Description;
            $scope.ServUpdUniqueID = UniqueId;
    
            $("#ServiceHeadUpdatePopup").modal("show");
         }

         $scope.DelIndex = 0;
         $scope.DelHeaderpopup = function(DelIndexVal, UniqueId){
             $scope.delHeaderUniqueID = UniqueId
             $scope.DelIndex = DelIndexVal
             $("#DeleteheaderPopUp").modal("show");
         }
         $scope.deleteServiceHead = function(){
 
             $rootScope.loading = false;
             var data = JSON.stringify($('.HeaddeleteForm').serializeObject());
             var validate = JSON.parse(data);
 
             PRUpdateFactory.DeletServiceHeader({"UniqueID":validate.delHeaderUniqueID, "PRHeaderStructureid": $scope.prId})
             .success(function(response){
                 if(response.info.errorcode == 0)
                 {
                //  if($scope.HeadItem.ItemStructureList.length < 1 && $scope.HeadItem.ItemStructureList[0].HeadTitle == null)
                //      $scope.PO_Status = 1;
 
                //      //alert($scope.DelIndex);
                //      try{
                //      $scope.SelectedItemsList[$scope.DelIndex].newObj = [];
                //      }catch(e){}
 
                 $('#DeleteheaderPopUp').modal('hide');
                 $scope.getDetailsForView();
                 }else
                 {AlertMessages.alertPopup({ errorcode: response.info.errorcode, errormessage: response.info.errormessage }); return false;}
             });
         }


        $scope.serviceInformation = function (serviceInfo) {
            $scope.ServiceFullInfo = serviceInfo;
            $scope.GerServiceDefinition(serviceInfo.MaterialCode);
        }
        $scope.getshippingplantstorageInfo = function (projcode) {
            $rootScope.loading = true;
            PRUpdateFactory.getBilling({ 'ProjectCode': projcode }).success(function (response) {
                $rootScope.loading = false;
                $scope.billingData = response.BillingData;
                $scope.shippingData = response.ShippingData;
            });
        }
        $scope.getProjClientInfo = function (prjcode) {
            //$scope.FundCenterID = $("select[name='ProjectCode'] option:selected").attr('fndcenterUniqueID');
            $rootScope.ProjectCode = $("select[name='ProjectCode'] option:selected").attr('fndCenterPrjectCode');
            $scope.getWBSDetails();
        }
        // $scope.getFundCenters = function () {
        //     $rootScope.loading = true;
        //     PRUpdateFactory.getFundCentersList().success(function (response) {
        //         $rootScope.loading = false;
        //         $scope.fundCentersList = response.result;
        //     });
        // }
        $scope.getFundCenter = function (fundcenterId) {
            $rootScope.loading = true;
            PRUpdateFactory.getFundCenter({ 'ID': parseInt(fundcenterId) }).success(function (response) {
                $rootScope.loading = false;
                $scope.fundCenter = response.result;
            });
        }
        $scope.shippingDetails = function (shiptoId) {
            $rootScope.loading = true;
            PRUpdateFactory.getShippingDetails({ 'ID': parseInt(shiptoId) }).success(function (response) {
                $rootScope.loading = false;
                $scope.shippingDet = response.result;
            });
        }
        $scope.searchMaterial = function (searchObj) {
            if (searchObj != null) {
                if (searchObj.length < 3) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 charactors to Search' }); return false;
                }
                else {
                    $rootScope.loading = true;
                    PRUpdateFactory.searchMaterialGroups({ 'Search': searchObj }).success(function (response) {
                        $rootScope.loading = false;
                        $scope.SearchedMaterialGroupList = response.result;
                    });
                }
            }
            else {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 charactors to Search' }); return false;
            }
        }
        $scope.searchwithinGroup = function (grpName, searchObj) {
            if (grpName == null || grpName == "") {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please select searched material groups' }); return false;
            }
            else {
                grpName = grpName.trim()
                $rootScope.loading = true;
                PRUpdateFactory.searchWithinMaterialGroups({ 'groupname': grpName, 'searchQuery': searchObj }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.MaterialLists = response.result;
                });
            }
        }

        $scope.AddServiceHeader = function(){

            $rootScope.loading = false;
            var data = JSON.stringify($('.ServIDHead').serializeObject());
    
            var validate = JSON.parse(data);
    
    if(validate.ServHead == "")
    {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please Enter the Header Title ' }); return false;}
    else if(validate.ServHead.length > 40)
    {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please Enter the Header Title below 40 Characters ' }); return false;}
    else if(validate.ServHeadDesc == "")
    {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please Enter the Header Description ' }); return false;}
    else if(validate.ServHead.length > 132)
    {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please Enter the Header Description below 40 Characters' }); return false;}
    else
    {        PRUpdateFactory.AddServiceHeaderitem({"Title":validate.ServHead, "Description":validate.ServHeadDesc, "PRHeaderStructureid":$scope.prId}).success(function(response){
            if(response.info.errorcode == 0)
            {
            $('#ServiceHeadPopup').modal('hide');
            
        // for(var i = 0; i < $scope.HeadItem.ItemStructureList.length; i++)
        // {
        //     $scope.HeadCount[i] = SelectedMatItemsList[Headkey].Matlist
        // }
        $scope.getDetailsForView();
            
            $('.ServHead').val("")
            $('.ServHeadDesc').val("")
            //$scope.ServHeadDesc = "";
            //$scope.Numbering();
            $scope.PO_Status = 0;
            }
            else{AlertMessages.alertPopup({ errorcode: response.info.errorcode, errormessage: response.info.errormessage }); return false;}
            });
    }
        }
 
        $scope.UpdServiceHeader = function(){
            $rootScope.loading = false;
            var data = JSON.stringify($('.ServupdIDHead').serializeObject());
    
            var validate = JSON.parse(data);
    
           if(validate.ServUpdHead == "")
    {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please Enter the Header Title ' }); return false;}
    else if(validate.ServUpdHead.length > 40)
    {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please Enter the Header Title below 40 Characters ' }); return false;}
    else if(validate.ServUpdHeadDesc == "")
    {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please Enter the Header Description ' }); return false;}
    else if(validate.ServUpdHeadDesc.length > 132)
    {AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please Enter the Header Description below 40 Characters' }); return false;}
    else
    {
        PRUpdateFactory.UpdServiceHeaderitem({"UniqueID":validate.ServUpdUniqueID, "Title":validate.ServUpdHead, "Description":validate.ServUpdHeadDesc, "PRHeaderStructureid":$scope.prId}).success(function(response){
    
    
            $('.ServupdIDHead').ServUpdHead = {};
            $('.ServupdIDHead').ServUpdHeadDesc = {};
            if(response.info.errorcode == 0)
            {
            $('#ServiceHeadUpdatePopup').modal('hide');
            $scope.getDetailsForView();
            }
    
            {AlertMessages.alertPopup({ errorcode: response.info.errorcode, errormessage: response.info.errormessage }); return false;}
            });
    }
        }

        $scope.getWBSDetails = function () {
            $rootScope.loading = true;
            PRUpdateFactory.getWBSList({ "FundCentreID": $scope.prsavedDetails.FundCenterId, "ProjectCode": $scope.prsavedDetails.ProjectCode }).success(function (response) {
                $rootScope.loading = false;
                $scope.WBSList = response.result;
            });
        }
        $scope.GetAllMaterials = function () {
            $rootScope.loading = true;
            PRUpdateFactory.getAllMaterials().success(function (response) {
                $rootScope.loading = false;
                $scope.MaterialLists = response.result;
                $scope.tempMaterialLists = response.result;
            });
        }
        $scope.getPOTypes = function () {
            $rootScope.loading = true;
            PRUpdateFactory.getPOTypes().success(function (response) {
                $rootScope.loading = false;
                $scope.orderTypeList = response.result;;
            });
        }
        $scope.getPOTypes();
        $scope.RemoveBindedMaterialElement = function (Headindex, index) {
            $scope.SelectedItemsList[Headindex].ServList.splice(index, 1);
        };

        $scope.RemoveBindedMatElement = function (Headindex, index) {
            $scope.SelectedMatItemsList[Headindex].MatList.splice(index, 1);
        };

        $scope.swapTempMaterialData = function () {
            $scope.MaterialLists = $scope.tempMaterialLists;
            $scope.SearchedMaterialGroupList = [];
            $scope.SearchedServiceGroupList = [];
            $scope.ServiceLists = [];
            $('#SearchServiceID').val('');
            $('#SearchMatID').val('');
            $('#searchObjID').val('');
            $('#searchServiceObjID').val('');
            $('#SearchServID').val('');
            // $scope.searchObj = "";
            // $scope.searchServiceObj = "";
            setTimeout(function(){$('select[name="grpName"]').val('');},300);
            setTimeout(function(){$('select[name="grpServName"]').val('');},300);
        };
        $scope.myTotalScheduledQtySum = 0;
        $scope.PROriginalQuantity = 0;
        $scope.getPRItemsSchedulesByItemId = function (prItemId) {
            $rootScope.loading = true;
            PRUpdateFactory.GetAllDeliverySchedulesByPRItemId({ 'PRItemId': prItemId }).success(function (response) {
                $rootScope.loading = false;
                $scope.myTotalScheduledQtySum = 0;
                $scope.DeliverySchedulesByPRItemId = response.result;
                angular.forEach(response.result, function (data) {
                    $scope.myTotalScheduledQtySum += data.DeliveryQty;
                });
            });
        }
        var materialCodes = [];
        $scope.AddMaterial = function () {
            var newDataList = [];
            var count = $(".cmplibrary:checked").length;
            $(".cmplibrary:checked").each(function (key, val) {
                var currentmaterialrowData = JSON.parse($(this).attr("data"));
                newDataList.push(currentmaterialrowData);
                if (key == (count - 1)) {
                    $('#ComponentLibraryPopup').modal('hide');
                    $scope.SearchedMaterialGroupList = [];
                    $scope.searchObj = '';
                    $scope.searchbyObj = '';
                    $scope.mataced(newDataList);
                    $scope.MaterialLists = [];
                }
            });
        };
        $scope.mataced = function (MaterialListsea) {
           
            var Len = $scope.HeadItem.length - 1;

            if ($scope.isMatfirsttimeadding[Len]) {
                $scope.isMatfirsttimeadding[Len] = false;
                var MatList = "SelectedItemsList";
                //$scope.SelectedItemsList = MaterialListsea;
                //$scope.PreviousSelectedItemsList = MaterialListsea;
                $scope.SelectedMatItemsList[Len] = {};
                $scope.SelectedMatItemsList[Len].MatList = [];
                $scope.wbscodeMatPart[Len] = [];
            }
            else {
                $scope.mergedItemsList = $scope.PreviousSelectedItemsList.concat(MaterialListsea);
                //$scope.SelectedItemsList = $scope.mergedItemsList;
               // $scope.PreviousSelectedItemsList = $scope.mergedItemsList;
            }
            var i=0;
             angular.forEach(MaterialListsea,function(v,k){
                 $scope.SelectedMatItemsList[Len].MatList.push(v);
            });
        }
        $scope.searchbyMaterial = function (searchObj) {
            if (searchObj != null) {
                if (searchObj.length < 3) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
                }
                else {
                    $rootScope.loading = true;
                    PRUpdateFactory.searchbyMaterialGroups({ 'Search': searchObj }).success(function (response) {
                        $rootScope.loading = false;
                        $scope.MaterialLists = response.result;
                        //$scope.MaterialLists = [];
                        //$scope.grpName = " ";
                        setTimeout(function(){$('select[name="grpName"]').val('');},300);
                    });
                }
            }
            else {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
            }
        }

        $scope.searchbyService = function (searchObj) {
            if (searchObj != null) {
                if (searchObj.length < 3) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
                }
                else {
                    $rootScope.loading = true;
                    PRUpdateFactory.searchServiceByIDGroups({ 'Search': searchObj }).success(function (response) {
                        $rootScope.loading = false;
                        $scope.ServiceLists = response.result;
                       // $scope.MaterialLists = [];
                        //$scope.grpName = " ";
                        setTimeout(function(){$('select[name="grpName"]').val('');},300);
                    });
                }
            }
            else {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
            }
        }
        $scope.serviceHeadID  = 0;
        $scope.GetServicepopup = function (Headkey) {
                $("#ServicePopup").modal("show");
                $scope.serviceHeadID = Headkey;
        }

        $scope.getDetailsForView = function () {
            $rootScope.loading = true;
            PRUpdateFactory.PRDetails({ 'PRId': $localStorage.prid }).success(function (response) {
                // $scope.getFundCenters();
                $scope.prsavedDetails = response.result;

                $scope.prsavedDetails.FundCenterId = response.result.FundCenterId.toString();
                $scope.prId = $scope.prsavedDetails.PurchaseRequestId;
                $scope.getWBSDetails();
                PRUpdateFactory.GetPRServiceHeader({ 'PRHeaderStructureid': $localStorage.prid }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.HeadItem = response.result;

                    $scope.isfirsttimeadding = [];
                    $scope.isMatfirsttimeadding = [];
                    var Len = $scope.HeadItem.length - 1;
                    $scope.isMatfirsttimeadding[Len] = true;
                    angular.forEach($scope.HeadItem,function(v,k){
                        //$scope.Numbering();
                        $scope.isfirsttimeadding[k] = true;
                    });

                });
            });
        };
        //open add tab in pr delivery schedules
        $scope.openAddDiv = function (PRItemId, quantity) {
            $scope.PRItemId = PRItemId;
            $scope.PROriginalQuantity = quantity;
            $scope.prQuantity = quantity;
            $scope.getPRItemsSchedulesByItemId(PRItemId);
        }
        // end addd tab

        // add delivery schedules
        $scope.addSchedule = function () {
            //alert($scope.prScheduleDate);
            $rootScope.loading = false;
            var json = {
                "LastModifiedByID": $scope.UserDetails.currentUser.UserId,
                "DeliveryQty": $scope.prQuantity,
                "DeliveryDate": $filter('date')(new Date($scope.prScheduleDate), 'yyyy-MM-dd'),
                "DeliveryRemarks": $scope.prRemarks == undefined ? " " : $scope.prRemarks,
                "PRItemId": $scope.PRItemId
            }
            if ($scope.prQuantity == "" || $scope.prQuantity == undefined) {
                AlertMessages.alertPopup({ "errorcode": 1, "errormessage": "Quantity is mandatory" });
                return false;
            }
            else if ($scope.prScheduleDate == "" || $scope.prScheduleDate == undefined) {
                AlertMessages.alertPopup({ "errorcode": 1, "errormessage": "Delivery date is mandatory" });
                return false;
            }
            else if (parseInt($scope.prQuantity) < 0) {
                AlertMessages.alertPopup({ "errorcode": 1, "errormessage": "Quantity Should be greaterthan zero" });
                return false;
            }
            else if (($scope.myTotalScheduledQtySum + parseFloat($scope.prQuantity)) > parseFloat($scope.PROriginalQuantity)) {
                AlertMessages.alertPopup({ "errorcode": 1, "errormessage": "Schedule Quantity Should be less than PR Quantity" });
                return false;
            } else {
                $rootScope.loading = true;
                PRUpdateFactory.SaveDeliveryScheduleByPRItemId(json).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $scope.prQuantity = '';
                        $scope.prScheduleDate = '';
                        $scope.prRemarks = '';
                        $scope.getPRItemsSchedulesByItemId($scope.PRItemId);
                        $scope.DateChange();
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }
        // end add delivery schedules
        // begin delete delivery schedule
        $scope.loadDeliveryScheduleId = function (DeliveryScheduleId) {
            $rootScope.DeliveryScheduleId = DeliveryScheduleId;
        }
        $scope.deletedeliverySchedule = function () {
            $rootScope.loading = true;
            PRUpdateFactory.DeleteDeliverySchedule({ 'DeliveryScheduleId': $rootScope.DeliveryScheduleId, 'LastModifiedByID': $scope.UserDetails.currentUser.UserId }).success(function (response) {
                if (response.info.errorcode == '0') {
                    $rootScope.loading = false;
                    $("#SubmitPRPopUp").modal("hide");
                    $scope.getPRItemsSchedulesByItemId($scope.PRItemId);
                }
                AlertMessages.alertPopup(response.info);
            });
        }

        $scope.deleteAddedMaterial = function (ItemStructureID) {
            $rootScope.loading = true;
            PRUpdateFactory.PRMaterialsDetailsDelete({ PRItemId: ItemStructureID, LastModifiedByID: $scope.UserDetails.currentUser.UserId }).success(function (response) {

                AlertMessages.alertPopup(response.info);
                $scope.getDetailsForView();
                $rootScope.loading = false;
            });
        }
        $scope.getDetailsForView();
        $scope.aced = function (MaterialListsea, ServHeadKey) {
           


            if ($scope.isfirsttimeadding[ServHeadKey]) {
                $scope.isfirsttimeadding[ServHeadKey] = false;
                var MatList = "SelectedItemsList";
                //$scope.SelectedItemsList = MaterialListsea;
                //$scope.PreviousSelectedItemsList = MaterialListsea;
                $scope.SelectedItemsList[ServHeadKey] = {};
                $scope.SelectedItemsList[ServHeadKey].ServList = [];
                $scope.wbscodePartServ[ServHeadKey] = [];
            }
            else {
                $scope.mergedItemsList = $scope.PreviousSelectedItemsList.concat(MaterialListsea);
                //$scope.SelectedItemsList = $scope.mergedItemsList;
               // $scope.PreviousSelectedItemsList = $scope.mergedItemsList;
            }
            var i=0;
             angular.forEach(MaterialListsea,function(v,k){
                 $scope.SelectedItemsList[ServHeadKey].ServList.push(v);
            });
        }

        $scope.specList = [];
        var cols = [];
        $scope.materialSpec = function (obj) {
            PRUpdateFactory.getMaterialSec(obj).success(function (response) {
                $scope.specList = response;
                $.each($scope.specList.result, function (value, key) {
                    $.each(value, function (value, key) {
                        if (angular.isArray(value))
                            cols.push(value);
                    })
                });
                $scope.specLists = cols;
                $scope.specLists = $scope.specLists;
                var a = '';
                var colspans = 0;
                $.each($scope.specLists, function (value) {
                    a = value.length;
                    if (a > colspans) {
                        colspans = a;
                        $scope.colspans = colspans;
                    }
                });
            })
        }

        $scope.EnableCurrentRow = function (key) {
            $scope.enableRowToEdit = key;
        }
        $scope.CopyCurrentLineItem = function (copyData) {
            var json = {
                PRStructureId: copyData.Uniqueid,
                LastModifiedByID: $scope.UserDetails.currentUser.UserId,
            }
            $rootScope.loading = true;
            PRUpdateFactory.CopyCurrentLineItem(json).success(function (response) {
                $rootScope.loading = false;
                $scope.getDetailsForView();
                AlertMessages.alertPopup(response.info);
                $scope.enableRowToEdit = "";
            });
        }
        $scope.updateExistingData = function (data) {
            var json = {
                Uniqueid: data.Uniqueid,
                Order_Qty: data.Order_Qty,
                LastModifiedBy: $scope.UserDetails.currentUser.UserId,
                WBSElementCode: data.WBSElementCode
            }
            if (data.WBSElementCode == " " || data.WBSElementCode == "") {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Element Code is Mandatory for ' }); return false;
            }
            else if (!data.Order_Qty || data.Order_Qty == '' || data.Order_Qty == "0") {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity is Mandatory ' }); return false;
            }
            else if (parseFloat(data.Order_Qty) <= 0) {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity should be greaterthan Zero' }); return false;
            }
            PRUpdateFactory.UpdatePurchaseDetails(json).success(function (response) {
                $rootScope.loading = false;
                $scope.getDetailsForView();
                AlertMessages.alertPopup(response.info);
                $scope.enableRowToEdit = "";
            });
        }
        $scope.basicInfoSavePO = function () {
            $scope.FundCenterID = $("select[name='ProjectCode'] option:selected").attr('fndcenterUniqueID');
            var data = JSON.stringify($('.basicInformation').serializeObject());
            var validate = JSON.parse(data);
            $scope.fundcenterId = validate.FundCenterID;
            console.log(validate.FundCenterID);
            $rootScope.ProjectCode = $("select[name='ProjectCode'] option:selected").attr('fndCenterPrjectCode');
            if (!validate.FundCenterID || validate.FundCenterID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center is Mandatory' }); return false; }
            else if (!validate.PRTitle || validate.PRTitle == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PR title is Mandatory' }); return false; }
            else if (!validate.PRDescription || validate.PRDescription == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Description is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                PRUpdateFactory.UpdaatePr(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        // $sessionStorage.PRHeaderStructureID = response.PRId;
                        // $scope.prId = response.PRId;
                        // $location.path('UpdatePO');
                        // $('.basicInformation')[0].reset();
                        // $scope.getFundCenters();
                        $scope.getDetailsForView();
                        $scope.tab = 2;
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                    $scope.getWBSDetails();
                });
            }
        }
        $scope.generalInfoUpdatePO = function () {
            var data = JSON.stringify($('.generalInformation').serializeObject());
            var validate = JSON.parse(data);
            if (!validate.ShippedToID || validate.ShippedToID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Ship To is Mandatory' }); return false; }
            else if (!validate.ShippedFromID || validate.ShippedFromID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Ship From is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                PRUpdateFactory.UpdaatePr(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $sessionStorage.POHeaderStructureID = response.UniqueId;
                        $location.path('UpdatePO');
                        $('.generalInformation')[0].reset();
                        $scope.getDetailsForView();
                        $scope.tab = 3;
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }
        // save purchase details
        $scope.MultipleSavePurchaseItem = function () {
            var itemcount = $('input[name="Order_Qty"]').length;
            var data = JSON.stringify($('.purchaseItemform').serializeObject());
            var validate = JSON.parse(data);
            var savedCount = $scope.prSavedMaterialdetails ? $scope.prSavedMaterialdetails.length : 0;
            if (itemcount == 1) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Add an item to save' }); return false; }

            else {
                for (var cnt = 1; cnt < itemcount; cnt++) {
                    //   if (!validate.WBSElementCode[cnt] || validate.WBSElementCode[cnt] == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Element is Mandatory' }); return false; }
                    var brandListItems = "";
                    if (validate.brandCheck[cnt] != "") {
                        var brandListItems = JSON.parse(validate.brandCheck[cnt].toString());
                    }
                    if (brandListItems.length != 0 && brandListItems != 0 && brandListItems != "") {
                        // if (!validate.Brand[cnt] || validate.Brand[cnt] == '') {
                        //     AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Item Brand Name is Mandatory for item' + (cnt + savedCount) }); return false;
                        // }
                        // else 
                        if (validate.WBSElementCode[cnt] == " " || validate.WBSElementCode[cnt] == "") {
                            AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Element Code is Mandatory for item' + (cnt + savedCount) }); return false;
                        }
                        else if (!validate.Order_Qty[cnt] || validate.Order_Qty[cnt] == '' || validate.Order_Qty[cnt] == "0") {
                            AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity is Mandatory for item' + (cnt + savedCount) }); return false;
                        }
                        else if (parseFloat(validate.Order_Qty[cnt]) <= 0) {
                            AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity should be greaterthan Zero for item' + (cnt + savedCount) }); return false;
                        }
                    }
                    else if (validate.WBSElementCode[cnt] == " " || validate.WBSElementCode[cnt] == "") {
                        AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Element Code is Mandatory for item' + (cnt + savedCount) }); return false;
                    }
                    else if (!validate.Order_Qty[cnt] || validate.Order_Qty[cnt] == '' || validate.Order_Qty[cnt] == "0") {
                        AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity is Mandatory for item' + (cnt + savedCount) }); return false;
                    }
                    else if (parseFloat(validate.Order_Qty[cnt]) <= 0) {
                        AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity should be greaterthan Zero for item' + (cnt + savedCount) }); return false;
                    }
                }
                $rootScope.loading = true;
                PRUpdateFactory.SavePurchaseDetails(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $scope.SelectedItemsList = [];
                        $scope.SelectedMatItemsList = [];
                        $scope.getDetailsForView();
                        $scope.isfirsttimeadding = true;
                       // $scope.tab = 2;
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }
        $scope.wbsPartRowNo = 0;
        $scope.wbsHeadPartRowNo = 0;
        $scope.wbscodePart1 = [];
        $scope.wbscodeMatPart = [];
        $scope.wbscodePartServ = [];
        $scope.isFirsttimeWBS = [];

        $scope.isexistMaterial = false;
        $scope.setrowclickedKey = function (Headkey, index, isexist) {
          
            $scope.wbsPartRowNo = index;
            $scope.wbsHeadPartRowNo = Headkey;
            $scope.isexistMaterial = isexist;
        }

        $scope.emptyComponentList = function () {
            $scope.MaterialLists = [];
            $scope.ServiceLists = [];
            $scope.searchServiceObj = '';
            $scope.SearchedServiceGroupList = [];
            $scope.grpServName = "";
        }
        $scope.setWbsPart1Code = function (key, data) {
            $scope.wbsPart1bgColor = [];
            if ($scope.isexistMaterial) {
                $scope.prSavedMaterialdetails[$scope.wbsPartRowNo].WBSElementCode = data.wbsCode;
            } else {
               
                //console.log(data.wbsCode);
                $scope.wbscodeMatPart[$scope.wbsHeadPartRowNo][$scope.wbsPartRowNo] = data.wbsCode;
            }
            $scope.prevClickedKey = key;
            $scope.wbsPart1bgColor[key] = 'darkseagreen';
        }

        $scope.setWbsPartServCode = function (key, data) {
            $scope.wbsPart1bgColor = [];
            if ($scope.isexistMaterial) {
                $scope.prSavedMaterialdetails[$scope.wbsPartRowNo].WBSElementCode = data.wbsCode;
            } else {
                
                //console.log(data.wbsCode);
                $scope.wbscodePartServ[$scope.wbsHeadPartRowNo][$scope.wbsPartRowNo] = data.wbsCode;
            }
            $scope.prevClickedKey = key;
            $scope.wbsPart1bgColor[key] = 'darkseagreen';
        }
        $scope.searchService = function (searchObj) {
            if (searchObj != null) {
                if (searchObj.length < 3) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
                }
                else {
                    $rootScope.loading = true;
                    PRUpdateFactory.searchServiceGroups({ 'Search': searchObj }).success(function (response) {
                        $rootScope.loading = false;
                        $scope.SearchedServiceGroupList = response.result;
                    });
                }
            }
            else {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
            }
        }

        $scope.Numbering = function(){
            
            var Count = 1;
                for(var i = 0; i < $scope.HeadItem.ItemStructureList.length; i++)
                {
                    if(i != 0)
                    {
                        $scope.HeadCount[i] = Count;
                        Count++;
                    }
                    $scope.MatHeadCount[i] = [];
                    try
                    {
                    if( $scope.SelectedMatItemsList[i].Matlist)
                    {
                        for(var j = 0; j< $scope.SelectedMatItemsList[i].Matlist.length; j++)
                        {
                        $scope.MatHeadCount[i][j] = Count;
                        Count++;
                        }
                    }
                }catch(ex){}
                }

        }

        $scope.searchwithinServGroup = function (grpName, searchObj) {
            if (grpName == null || grpName == "") {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please select searched material groups' }); return false;
            }
            else {
                grpName = grpName.trim()
                $rootScope.loading = true;
                PRUpdateFactory.searchWithinServiceGroups({ 'groupname': grpName, 'searchQuery': searchObj }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.ServiceLists = response.result;
                });
            }
        }
        $scope.AddService = function () {
            var newDataList = [];
            var count = $(".cmplibrary_Service:checked").length;
            $(".cmplibrary_Service:checked").each(function (key, val) {
                var hsncode = $(this).attr("hsncode");
                var currentmaterialrowData = JSON.parse($(this).attr("data"));

                newDataList.push(currentmaterialrowData);
                if (key == (count - 1)) {
                    $('#ServicePopup').modal('hide');
                    $scope.SearchedServiceGroupList = [];
                    $scope.searchServiceObj = '';
                    $scope.searchbyServObj = '';
                    var SelectHeadKey = $scope.serviceHeadID;
                    $scope.aced(newDataList, SelectHeadKey);
                    $scope.ServiceLists = [];
                    //$scope.getDetailsForView();
                    
                }
            });
        };
    })
    .directive("addmainmaterialelement", function () {
        return {
            restrict: 'A',
            template: '<tr ng-repeat="choicew in MainMaterialElementList" class="removeablediv">\
    <td style="text-align:right">{{$index+1}}</td>\
    <td colspan="4">\
    <input type="email" class="form-control" id="Email20" placeholder="">\
    </td>\
    <td colspan="14"></td>\
    <td>\
    <a data-toggle="modal" data-target="#ComponentLibraryPopup" aria-hidden="true"><i class="fa fa-plus" aria-hidden="true" style="font-size: 16pt;color: darkseagreen;"></i></a>\
    </td>\
    <td>\
    <a ng-click="RemoveMainMaterialElement();"><span class="glyphicon glyphicon-trash" aria-hidden="true" title="Delete" style="font-size:12pt;"></span>\
    </a>\
    </td>\
</tr>'
        };
    })