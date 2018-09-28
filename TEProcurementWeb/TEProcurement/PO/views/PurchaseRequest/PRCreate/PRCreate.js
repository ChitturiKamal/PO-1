'use strict';
angular.module('TEPOApp')
    .controller('PRSaveCtrl', function ($filter, $scope, $localStorage, $rootScope, $element, $sessionStorage, PRSaveFactory, $uibModal, AlertMessages, $state, $location) {
        $scope.shippingDet = [];
        $scope.FundCenterID = 0;
        $scope.VendorID = 0;
        $scope.tab = 1;
        $scope.tempMaterialLists = [];
        $scope.PreviousSelectedItemsList = [];
        $scope.selectTab = function (setTab) {
            $scope.tab = setTab;
        };
        $scope.IsTabSelected = function (checkTab) {
            return $scope.tab === checkTab;
        };
        $scope.roundValue = function () {
            $scope.Qty = Math.floor($scope.Qty);
        }
        $scope.date = $filter('date')(new Date(), 'yyyy-MM-dd');
        $scope.getCustomers = function () {
            $rootScope.loading = true;
            PRSaveFactory.getCustomersList().success(function (response) {
                $rootScope.loading = false;
                $scope.CustomersList = response.result;
            });
            // enable after editing above call
            $rootScope.loading = false;
        }
        $scope.getCustomers();

        $scope.getMangersList = function () {
            $rootScope.loading = true;
            PRSaveFactory.getMangers().success(function (response) {
                $rootScope.loading = false;
                $scope.ManagersResult = response.result;
            });
        }
        $scope.getMangersList();

        $scope.materialInformation = function (MaterialInfo) {
            $scope.MaterialFullInfo = MaterialInfo;
        }
        $scope.GerServiceDefinition = function (serviceCode) {
            $rootScope.loading = true;
            PRSaveFactory.GerServiceDefinitionData({ 'ServiceCode': serviceCode }).success(function (response) {
                $rootScope.loading = false;
                $scope.ClassificationDefinitionResult = response.result;
            });
        }
        $scope.serviceInformation = function (serviceInfo) {
            $scope.ServiceFullInfo = serviceInfo;
            $scope.GerServiceDefinition(serviceInfo.MaterialCode);
        }
        $scope.getprojects = function (cmpcode) {
            $rootScope.loading = true;
            PRSaveFactory.getProjectsbycmpcode({ 'CompanyCode': cmpcode }).success(function (response) {
                $rootScope.loading = false;
                $scope.projectsList = response.result;
            });
        }
        $scope.GetVendorShippingInfo = function (vendrId) {
            $rootScope.loading = true;
            PRSaveFactory.vendorshippingInfo({ 'VendorID': parseInt(vendrId) }).success(function (response) {
                $rootScope.loading = false;
                $scope.vendorshippingDet = response.ShippingData;
            });
        }
        $scope.vendorshipFromDetails = function (vendorshipId) {
            $rootScope.loading = true;
            PRSaveFactory.vendorshipFromInfo({ 'VendorShippingID': parseInt(vendorshipId) }).success(function (response) {
                $rootScope.loading = false;
                $scope.vendorBilledByDetails = response.ShippingData;
            });
        }

        $scope.getshippingplantstorageInfo = function (projcode) {
            $rootScope.loading = true;
            PRSaveFactory.getBilling({ 'ProjectCode': projcode }).success(function (response) {
                $rootScope.loading = false;
                $scope.billingData = response.BillingData;
                $scope.shippingData = response.ShippingData;
            });
        }
        $scope.getProjClientInfo = function (prjcode) {
            $scope.FundCenterID = $("select[name='ProjectCode'] option:selected").attr('fndcenterUniqueID');
            $scope.getshippingplantstorageInfo(prjcode);
            $rootScope.loading = true;
            PRSaveFactory.getProjectClientInfo({ 'ProjectCode': prjcode }).success(function (response) {
                $rootScope.loading = false;
                $scope.ClientprojectInfo = response.result;
            });
        }
        // $scope.getFundCenters = function () {
        //     $rootScope.loading = true;
        //     PRSaveFactory.getFundCentersList().success(function (response) {
        //         $rootScope.loading = false;
        //         $scope.fundCentersList = response.result;
        //     });
        // }
        // $scope.getFundCenters();
        $scope.GetFundCenterByCodeCall = function (a) {
            PRSaveFactory.GetFundCenterByCode({'SearchText':a}).success(function(response){
                if(response.result !=null){
                    $scope.FundCenterID=response.result.Uniqueid;
                }
            });
        }
        $scope.getFundCenters = function (a) {
            if(a.length>3){
                $scope.fundCentersList=[];
                PRSaveFactory.getFundCentersList({"SearchText":a}).success(function (response){$scope.fundCentersList=response.result;});
                $scope.GetFundCenterByCodeCall(a);
            }else{ AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 4 charactors to Search' }); return false;}
        }
        // $scope.getFundCenter = function (fundcenterId) {
        //     $rootScope.loading = true;
        //     PRSaveFactory.getFundCenter({ 'ID': parseInt(fundcenterId) }).success(function (response) {
        //         $rootScope.loading = false;
        //         $scope.fundCenter = response.result;
        //     });
        // }
        $scope.shippingDetails = function (shiptoId) {
            $rootScope.loading = true;
            PRSaveFactory.getShippingDetails({ 'ID': parseInt(shiptoId) }).success(function (response) {
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
                    PRSaveFactory.searchMaterialGroups({ 'Search': searchObj }).success(function (response) {
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
                PRSaveFactory.searchWithinMaterialGroups({ 'groupname': grpName, 'searchQuery': searchObj }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.MaterialLists = response.result;
                });
            }
        }
        $scope.getVendors = function () {
            $rootScope.loading = true;
            PRSaveFactory.getVendorsList().success(function (response) {
                $rootScope.loading = false;
                $scope.VendorsList = response.result;
            });
        }
        $scope.getVendors();
        $scope.getVendorData = function (venderId) {
            $rootScope.loading = true;
            PRSaveFactory.getVendor({ 'ID': parseInt(venderId) }).success(function (response) {
                $rootScope.loading = false;
                $scope.vendorData = response.result;
            });
        }
        $("#selectedFundCenter").on('input', function () {
            var val = this.value;
            if (val != null && val != "") {
                var fnduniqueId = val.split("-")[0];
                var Id = parseInt(fnduniqueId);
                if (Id > 0) {
                    $scope.FundCenterID = Id;
                    $scope.getFundCenter(Id);
                }
            }
        });

        $("#selectedvendor").on('input', function () {
            var val = this.value;
            if (val != null && val != "") {
                var vendorid = val.split(" ")[0];
                var Id = parseInt(vendorid);
                if (Id > 0) {
                    $scope.VendorID = Id;
                    $scope.getVendorData(Id);
                }
            }
        });
        $scope.GetAllMaterials = function () {
            $rootScope.loading = true;
            PRSaveFactory.getAllMaterials().success(function (response) {
                $rootScope.loading = false;
                $scope.MaterialLists = response.result;
                $scope.tempMaterialLists = response.result;
            });
        }
        $scope.setVendorData = function (key, data) {
            $scope.bgcolor = [];
            $scope.selectedVendorDetails = data;
            $scope.GetVendorShippingInfo(data.Uniqueid);
            $scope.bgcolor[key] = 'darkseagreen';
            //$('#VenderDetailsPopup').modal('hide');
        }

        // $scope.GetAllMaterials();

        $scope.getPOTypes = function () {
            $rootScope.loading = true;
            PRSaveFactory.getPOTypes().success(function (response) {
                $rootScope.loading = false;
                $scope.orderTypeList = response.result;;
            });
        }
        $scope.getPOTypes();
        $scope.RemoveBindedMaterialElement = function (index) {
            $scope.SelectedItemsList.splice(index, 1);
        };

        $scope.swapTempMaterialData = function () {
            $scope.MaterialLists = $scope.tempMaterialLists;
        };
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
                    $scope.aced(newDataList);
                    $scope.MaterialLists = [];
                }
            });
        };
        $scope.aced = function (MaterialListsea) {
            if ($scope.isfirsttimeadding) {
                $scope.isfirsttimeadding = false;
                $scope.SelectedItemsList = MaterialListsea;
                $scope.PreviousSelectedItemsList = MaterialListsea;
            }
            else {
                $scope.mergedItemsList = $scope.PreviousSelectedItemsList.concat(MaterialListsea);
                $scope.SelectedItemsList = $scope.mergedItemsList;
                $scope.PreviousSelectedItemsList = $scope.mergedItemsList;
            }
            //$scope.materialCodes = materialCodes;
            //$scope.materialSpec($scope.materialCodes);
        }
        /* $scope.aced = function (MaterialListsea, materialCodes) {
             $scope.SelectedItemsList = MaterialListsea;
             $scope.materialCodes = materialCodes;
             $scope.materialSpec($scope.materialCodes);
         } */
        $scope.specList = [];
        var cols = [];
        $scope.materialSpec = function (obj) {
            PRSaveFactory.getMaterialSec(obj).success(function (response) {
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
        // $scope.materialSpec = function (obj) {
        //     $scope.Serviced(obj).then(function (response) {
        //         $scope.specList = response.data;
        //         _.each($scope.specList.result, function (value, key) {
        //             _.each(value, function (value, key) {
        //                 if (angular.isArray(value))
        //                     cols.push(value);
        //             })
        //         });
        //         console.log(cols);
        //         $scope.specLists = cols;
        //         $scope.specLists = $scope.specLists;
        //         var a = '';
        //         var colspans = 0;
        //         _.each($scope.specLists, function (value) {
        //             a = value.length;
        //             if (a > colspans) {
        //                 colspans = a;
        //                 $scope.colspans = colspans;
        //                 alert($scope.colspans);
        //             }
        //         });
        //     })
        // }
        // $scope.Serviced = function (obj) {
        //     $rootScope.loading = true;
        //     PRSaveFactory.getMaterialSec(obj).success(function (response) {
        //         $rootScope.loading = false;
        //         return response;
        //     });
        // }
        $scope.basicInfoSavePO = function () {
            var data = JSON.stringify($('.basicInformation').serializeObject());
            var validate = JSON.parse(data);
            $rootScope.fundcenterId = validate.FundCenterID;
            $rootScope.ProjectCode = validate.ProjectCode;
            if (!validate.ProjectCode || validate.ProjectCode == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center is Mandatory' }); return false; }
            else if (!validate.PRTitle || validate.PRTitle == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PR title is Mandatory' }); return false; }
            else if (!validate.PRDescription || validate.PRDescription == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Description is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                PRSaveFactory.savePO(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $sessionStorage.PRHeaderStructureID = response.PRId;
                        $scope.prId = response.PRId;
                        $localStorage.prid = $scope.prId
                        $location.path('PRupdate');
                        $localStorage.fromPrSave = true;
                        $('.basicInformation')[0].reset();

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
                PRSaveFactory.savePO(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $sessionStorage.POHeaderStructureID = response.UniqueId;
                        $location.path('UpdatePO');
                        $('.generalInformation')[0].reset();
                        $scope.tab = 3;
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }

        $rootScope.loading = false;

        // view details for schdule
        $scope.getDetailsForView = function (
            prid) {
            $rootScope.loading = true;

            PRSaveFactory.PRMaterialdetails({
                'PurchaseRequestId': prid
            }).success(function (response) {
                $rootScope.loading = false;
                $scope.prMaterialdetails =
                    response.result;
            });
        }
        // end view details 

        //open add tab in pr delivery schedules



        $scope.openAddDiv = function (PRItemId, quantity) {
            $rootScope.loading = true;
            $scope.PRItemId = PRItemId;
            $scope.totalQuantity = quantity;
            var json = {
                PRItemId: $scope.PRItemId
            }
            PRSaveFactory.GetAllDeliverySchedulesByPRItemId(json).success(function (response) {
                $scope.DeliverySchedulesByPRItemId = response.result;
                var addedQuantity = 0;
                if ($scope.DeliverySchedulesByPRItemId != null) {
                    for (var i = 0; i < $scope.DeliverySchedulesByPRItemId.length; i++) {
                        addedQuantity += $scope.DeliverySchedulesByPRItemId[i].DeliveryQty;
                    }
                    if (addedQuantity == quantity) {
                        $scope.disable = true;
                    } else {
                        $scope.disable = false;
                    }
                } else {
                    $scope.disable = false;
                }
                $rootScope.loading = false;
            });

        }
        // end addd tab

        // add delivery schedules

        $scope.addSchedule = function () {
            $rootScope.loading = false;
            var json = {
                LastModifiedByID: $scope.UserDetails.currentUser.UserId,
                DeliveryQty: $scope.prQuantity,
                DeliveryDate: $filter('date')(new Date($scope.prScheduleDate), 'yyyy-MM-dd'),
                DeliveryRemarks: $scope.prRemarks == undefined ? " " : $scope.prRemarks,
                PRItemId: $scope.PRItemId
            }

            if ($scope.prQuantity == "" || $scope.prQuantity == undefined) {
                AlertMessages.alertPopup({ "listcount": 0, "errorcode": 1, "errormessage": "Quantity is mandatory", "fromrecords": 0, "torecords": 0, "totalrecords": 0, "pages": null });
                return false;
            } else if ($scope.prScheduleDate == "" || $scope.prScheduleDate == undefined) {
                AlertMessages.alertPopup({ "listcount": 0, "errorcode": 1, "errormessage": "Delivery date is mandatory", "fromrecords": 0, "torecords": 0, "totalrecords": 0, "pages": null });
                return false;
            } else if (parseFloat($scope.prQuantity) <= 0) {
                AlertMessages.alertPopup({ "listcount": 0, "errorcode": 1, "errormessage": "Quantity Should be greaterthan zero", "fromrecords": 0, "torecords": 0, "totalrecords": 0, "pages": null });
                return false;
            }
            else {
                PRSaveFactory.GetAllDeliverySchedulesByPRItemId({
                    PRItemId: $scope.PRItemId
                }).success(function (response) {
                    $scope.DeliverySchedulesByPRItemId = response.result;
                    $rootScope.loading = false;
                });
                var addedQuantity = 0;
                if ($scope.DeliverySchedulesByPRItemId != null) {
                    for (var i = 0; i < $scope.DeliverySchedulesByPRItemId.length; i++) {
                        addedQuantity += $scope.DeliverySchedulesByPRItemId[i].DeliveryQty;
                    }
                    if (addedQuantity == $scope.totalQuantity) {
                        $scope.disable = true;
                    } else {
                        $scope.disable = false;
                        if ((addedQuantity + parseInt($scope.prQuantity)) > parseInt($scope.totalQuantity)) {
                            AlertMessages.alertPopup({ "listcount": 0, "errorcode": 1, "errormessage": "Added Quantity is greater than material quantity", "fromrecords": 0, "torecords": 0, "totalrecords": 0, "pages": null });
                        } else {
                            PRSaveFactory.SaveDeliveryScheduleByPRItemId(json).success(function (response) {
                                if (response.info.errorcode == '0') {
                                    $rootScope.loading = true;
                                    PRSaveFactory.GetAllDeliverySchedulesByPRItemId({ PRItemId: $scope.PRItemId }).success(function (response) {
                                        $scope.DeliverySchedulesByPRItemId = response.result;
                                        var currentQuantity = 0;
                                        for (var j = 0; j < $scope.DeliverySchedulesByPRItemId.length; j++) {
                                            currentQuantity += $scope.DeliverySchedulesByPRItemId[j].DeliveryQty;
                                        }
                                        if (currentQuantity == $scope.totalQuantity) {
                                            $scope.disable = true;
                                        }
                                        $rootScope.loading = false;
                                    });
                                }
                                AlertMessages.alertPopup(response.info);
                                $rootScope.loading = false;
                            });
                        }
                    }
                } else {
                    if (parseInt($scope.prQuantity) <= parseInt($scope.totalQuantity)) {
                        PRSaveFactory.SaveDeliveryScheduleByPRItemId(json).success(function (response) {
                            if (response.info.errorcode == '0') {
                                $rootScope.loading = true;
                                PRSaveFactory.GetAllDeliverySchedulesByPRItemId({ PRItemId: $scope.PRItemId }).success(function (response) {
                                    $scope.DeliverySchedulesByPRItemId = response.result;
                                    $rootScope.loading = false;
                                });
                            }
                            AlertMessages.alertPopup(response.info);
                            $rootScope.loading = false;
                        });
                    } else {

                        $scope.disable = false;

                        AlertMessages.alertPopup({ "listcount": 0, "errorcode": 1, "errormessage": "Added Quantity is greater than material quantity", "fromrecords": 0, "torecords": 0, "totalrecords": 0, "pages": null });

                    }
                }


            }
        }
        // end add delivery schedules
        // begin delete delivery schedule

        $scope.deletedeliverySchedule = function (DeliveryScheduleId) {
            var data = {
                DeliveryScheduleId: DeliveryScheduleId,
                LastModifiedByID: $scope.UserDetails.currentUser.UserId
            }
            $rootScope.loading = true;
            PRSaveFactory.DeleteDeliverySchedule(data).success(function (response) {
                if (response.info.errorcode == '0') {
                    PRSaveFactory.GetAllDeliverySchedulesByPRItemId({ PRItemId: $scope.PRItemId }).success(function (response) {
                        $scope.DeliverySchedulesByPRItemId = response.result;
                        var currentQuantity = 0;
                        for (var j = 0; j < $scope.DeliverySchedulesByPRItemId.length; j++) {
                            currentQuantity += $scope.DeliverySchedulesByPRItemId[j].DeliveryQty;
                        }
                        if (currentQuantity == $scope.totalQuantity) {
                            $scope.disable = true;
                        } else {
                            $scope.disable = false;
                        }

                    });
                }
                AlertMessages.alertPopup(response.info);
                $rootScope.loading = false;
            });
        }
        // end delete delivery schedule
        // save purchase details
        $scope.MultipleSavePurchaseItem = function (brandList) {
            var itemcount = $('input[name="Order_Qty"]').length;
            var data = JSON.stringify($('.purchaseItemform').serializeObject());
            var validate = JSON.parse(data);
            if (itemcount == 1) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Select an Item' }); return false; }

            else {

                for (var cnt = 1; cnt < itemcount; cnt++) {
                    //   if (!validate.WBSElementCode[cnt] || validate.WBSElementCode[cnt] == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Element is Mandatory' }); return false; }
                    var brandListItems = "";
                    if (validate.brandCheck[cnt] != "") {
                        var brandListItems = JSON.parse(validate.brandCheck[cnt].toString());
                    }

                    if (brandListItems.length != 0 && brandListItems != 0 && brandListItems[0].BrandSeries != "") {
                        if (!validate.Brand[cnt]) {
                            AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Item Brand Name is Mandatory for item' + cnt }); return false;
                        }
                        else if (validate.WBSElementCode[cnt] == " " || validate.WBSElementCode[cnt] == "") {
                            AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Element Code is Mandatory for item' + cnt }); return false;
                        }
                        else if (!validate.Order_Qty[cnt] || validate.Order_Qty[cnt] == '' || validate.Order_Qty[cnt] == "0") {
                            AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity is Mandatory for item' + cnt }); return false;
                        }
                        else if (parseFloat(validate.Order_Qty[cnt]) <= 0) {
                            AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity should be greaterthan Zero' + cnt }); return false;
                        }
                    } else if (validate.WBSElementCode[cnt] == " " || validate.WBSElementCode[cnt] == "") {
                        AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Element Code is Mandatory for item' + cnt }); return false;
                    }
                    else if (!validate.Order_Qty[cnt] || validate.Order_Qty[cnt] == '' || validate.Order_Qty[cnt] == "0") {
                        AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity is Mandatory for item' + cnt }); return false;
                    }
                    else if (parseFloat(validate.Order_Qty[cnt]) <= 0) {
                        AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity should be greaterthan Zero' + cnt }); return false;
                    }

                }
                $rootScope.loading = true;
                PRSaveFactory.SavePurchaseDetails(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $scope.tab = 3;
                        $scope.getDetailsForView(validate.PR_ID);
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }

        $scope.emptyComponentList = function () {
            $scope.MaterialLists = [];
            $scope.ServiceLists = [];
            $scope.searchServiceObj = '';
            $scope.SearchedServiceGroupList = [];
            $scope.grpServName = "";
        }


        $scope.wbsPartRowNo = 0;
        $scope.wbscodePart1 = [];
        $scope.wbscodePart2 = [];
        $scope.isexistMaterial = false;
        $scope.setrowclickedKey = function (index, isexist) {
            //$scope.wbsPart1bgColor[$scope.prevClickedKey] = '';
            $scope.wbsPartRowNo = index;
            $scope.isexistMaterial = isexist;
        }

        $scope.setWbsPart1Code = function (key, data) {
            $scope.wbsPart1bgColor = [];
            if ($scope.isexistMaterial) {
                $scope.ExistpurchaseItemsResult[$scope.ClickedIndexValue].WBSElementCode = data.wbsCode;
            } else {
                $scope.wbscodePart1[$scope.wbsPartRowNo] = data.wbsCode;
            }
            $scope.prevClickedKey = key;
            $scope.wbsPart1bgColor[key] = 'darkseagreen';
        }

        $scope.setWbsPart2Code = function (key, data) {
            $scope.wbsPart2bgColor = [];
            if ($scope.isexistMaterial) {
                $scope.ExistpurchaseItemsResult[$scope.ClickedIndexValue].WBSElementCode2 = data.wbsCode;
            } else {
                $scope.wbscodePart2[$scope.wbsPartRowNo] = data.wbsCode;
            }
            $scope.wbsPart2bgColor[key] = 'darkseagreen';
        }

        $scope.getWBSDetails = function () {
            $rootScope.loading = true;
            PRSaveFactory.getWBSList({ "FundCentreID": $rootScope.fundcenterId, "ProjectCode": $rootScope.ProjectCode }).success(function (response) {
                $rootScope.loading = false;
                $scope.WBSList = response.result;
            });
        }

        $scope.searchService = function (searchObj) {
            if (searchObj != null) {
                if (searchObj.length < 3) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
                }
                else {
                    $rootScope.loading = true;
                    PRSaveFactory.searchServiceGroups({ 'Search': searchObj }).success(function (response) {
                        $rootScope.loading = false;
                        $scope.SearchedServiceGroupList = response.result;
                    });
                }
            }
            else {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
            }
        }
        $scope.searchwithinServGroup = function (grpName, searchObj) {
            if (grpName == null || grpName == "") {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please select searched material groups' }); return false;
            }
            else {
                grpName = grpName.trim()
                $rootScope.loading = true;
                PRSaveFactory.searchWithinServiceGroups({ 'groupname': grpName, 'searchQuery': searchObj }).success(function (response) {
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
                    $scope.aced(newDataList);
                    $scope.ServiceLists = [];
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

    .directive("cancellationelementssave", function () {
        return {
            restrict: 'A',
            template: '<tr ng-repeat="choicew in MainMaterialElementList" class="removeablediv">\
    <th scope="row" width="3%" class="vam">{{$index+2}}</th>\
    <td><input class="form-control" name="InstallmentName" placeholder="Installment Name"/></td>\
    <td><input class="form-control" name="InstallmentDate" type="date"/></td>\
    <td>\
        <select class="form-control" name="PaymentType">\
            <option value="">Payment Type</option>\
            <option value="Cash">Account Transfer </option>\
            <option value="Cash">Cash </option>\
            <option value="Cheque">Cheque</option>\
            <option value="DD">DD</option>\
            <option value="Wire Transfer">Wire Transfer </option>\
        </select>\
    </td>\
    <td><input class="form-control intvalid" name="Amount" placeholder="Amount"/></td>\
    <td><input class="form-control intvalid" style="visibility:hidden" name ="Status" size="5" value="Draft"/></td>\
    <td width="1%" class="vam"><i class="glyphicon glyphicon-remove" ng-click="RemoveElementList($index)"></i></td>\
</tr>'
        };
    })