'use strict';
angular.module('TEPOApp')
    .controller('POUpdateController', function ($filter, $scope, $rootScope, $element, $localStorage, $sessionStorage, POUpdateFactoryServices, $uibModal, AlertMessages, $state) {
        $scope.shippingDet = [];
        $scope.ProjectCode = '';
        $scope.VendorID = 0;
        $scope.tab = $sessionStorage.POUpdateDefaultTabNo;
        $sessionStorage.ServiceHeadID = [];
        if ($sessionStorage.POUpdateDefaultTabNo == undefined || $sessionStorage.POUpdateDefaultTabNo < 1) { $scope.tab = 1; }
        // $scope.tab = 2;
        $scope.SumofExistPaymentTermAmount = 0;
        $scope.ExistpurcItemCount = 0;
        $scope.taxresult = [];
        $scope.PODetailsResult = {};
        $scope.selectedManager = {};
        //$scope.isfirsttimeadding = true;
        $scope.PreviousSelectedItemsList = [];
        $scope.SelectedItemsList = [];
        $scope.SelectedMatItemsList = [];
        $scope.ClickedIndexValue = 1000;
        $scope.ClickedHeadIndexValue = 1000;
        $scope.currentPaymentTermAmount = 0;
        $scope.PO_HSID = $sessionStorage.POHeaderStructureID;
        $scope.enabledisable = false;
        $scope.enablecurrentrow = true;
        $scope.tempMaterialLists = [];
        $scope.HeadCount = [];
        $scope.MatHeadCount = [];
        $scope.FirstTimeHeadCount = [];
        $scope.headuniqueid = "";
        $scope.PO_Status = "1";
        $scope.DelIndex = 1000;
        $scope.User = $localStorage.globals.currentUser;
        $scope.materialInformation = function (MaterialInfo) {
            $scope.MaterialFullInfo = MaterialInfo;
        }
        $scope.GerServiceDefinition = function (serviceCode) {
            $rootScope.loading = true;
            POUpdateFactoryServices.GerServiceDefinitionData({ 'ServiceCode': serviceCode }).success(function (response) {
                $rootScope.loading = false;
                $scope.ClassificationDefinitionResult = response.result;
            });
        }
        $scope.serviceInformation = function (serviceInfo) {
            $scope.ServiceFullInfo = serviceInfo;
            $scope.GerServiceDefinition(serviceInfo.MaterialCode);
        }
        $scope.getItemInfo = function (code, type) {
            if (type == "MaterialOrder") {
                $rootScope.loading = true;
                POUpdateFactoryServices.ItemInfoByItemCode({ 'ItemCode': code, 'ItemType': type }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.MaterialFullInfo = response.result;

                });
            } else {
                $rootScope.loading = true;
                POUpdateFactoryServices.ItemInfoByItemCode({ 'ItemCode': code, 'ItemType': type }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.ServiceFullInfo = response.result;
                });
            }
        }
        $scope.GLPagePop = function(itemStructureID){
            $rootScope.loading = true;
            POUpdateFactoryServices.GetPurchaseItemswithPOIdForGLupdate({'ItemStructureID':itemStructureID}).success(function (response){
                $rootScope.loading = false;
                $scope.GLupdate = response.result;
                $scope.LastmodifiedByID = $scope.User.UserId;
            });
        }
        // $scope.GLAccountDetails = function(){
        //     $rootScope.loading = true;
        //     POUpdateFactoryServices.GetGLAccountDetailsList().success(function(response){
        //         $rootScope.loading = false;
        //         $scope.GLAccountList = response.result;
        //     });
        // }
        // $scope.GLAccountDetails();
        $scope.GLRowNo = 0;
        $scope.setGLrowclicked = function (index) {
            $scope.GLRowNo = index;
        }
        $scope.GLAccountDetailsClick = function (key, data) {
            $scope.GLAccountbgColor = [];
            $scope.GLupdate[$scope.GLRowNo].GLAccountNo = data.GLAccountCode;
            $scope.GLAccountbgColor[key] = 'darkseagreen';
        }
        $scope.UpdateGLAccount = function () {
            var data = JSON.stringify($('.UpdateGLAccountForm').serializeObject());
            POUpdateFactoryServices.UpdateGLAccountDetails(data).success(function (response) {
                if (response.info.errorcode == '0') { $('#GLPagePopUp').modal('hide'); }
                AlertMessages.alertPopup(response.info);
            });
        }

        $scope.purchaseRateHistory = function (itemCode) {
            $scope.PRHistory = [];
            $scope.WeightedAvgRate = 0;
            $rootScope.loading = true;
            POUpdateFactoryServices.GetPurchaseRateHistoryByItemCode({ 'itemCode': itemCode }).success(function (response) {
                if (response.info.errorcode == '0') {
                    var totalRate = 0;
                    $scope.PRHistory = response.result;
                    $scope.WeightedAvgRate = response.weightedAverage;
                    // angular.forEach(response.result, function (data) {
                    //     totalRate += data.Rate;
                    // });
                    // $scope.WeightedAvgRate = totalRate / response.result.length;
                }//else AlertMessages.alertPopup(response.info);
                $rootScope.loading = false;
            });
        }
        $scope.getTotalAmountTaxGross = function() {
            $scope.TotalAmount = 0;
            $scope.TotalTaxes = 0;
            $scope.TotalGross = 0;
            //Exist
            for (var Head_i = 0; Head_i < $scope.HeadItem.ItemStructureList.length; Head_i++) {
                if ($scope.HeadItem.ItemStructureList[Head_i].Itemstructure) {
                    for (var i = 0; i < $scope.HeadItem.ItemStructureList[Head_i].Itemstructure.length; i++) {
                        var exist = $scope.HeadItem.ItemStructureList[Head_i].Itemstructure[i];
                        var ttlAmnt = exist.Order_Qty * exist.Rate;
                        ttlAmnt=$filter('numnoneFlot')(ttlAmnt);
                        // var TotalTaxes = Math.round(((exist.IGSTRate + exist.SGSTRate + exist.CGSTRate) * ttlAmnt) / 100);
                        var TotalTaxes = (((exist.IGSTRate + exist.SGSTRate + exist.CGSTRate) * ttlAmnt) / 100);
                        $scope.TotalAmount += ttlAmnt;
                        $scope.TotalTaxes += TotalTaxes;
                        $scope.TotalGross += ttlAmnt + TotalTaxes;
                    }
                }
            }
            if ($scope.PODetailsResult.PO_OrderTypeID != 3) {
                var Count_i = 0
                //Current
                for (var Head_i = 0; Head_i < $scope.HeadItem.ItemStructureList.length; Head_i++) {
                    var Qty_c = document.getElementsByClassName("Qty");
                    var Rate_c = document.getElementsByClassName("Rate");
                    try {
                        if ($scope.SelectedItemsList[Head_i].newObj) {
                            for (var i = 0; i < $scope.SelectedItemsList[Head_i].newObj.length; i++) {
                                var current = $scope.SelectedItemsList[Head_i].newObj[i];
                                //alert($scope.Qty[Head_i][i].QVal)
                                var Qty = Qty_c[Count_i].value;
                                var Rate = Rate_c[Count_i].value;
                                if (current.last_purchase_rate != "" && current.purchase_rate_threshold != "") {
                                    var ans = (current.last_purchase_rate * (current.purchase_rate_threshold + 100)) / 100;
                                    $scope.ratePlaceholder = Rate;
                                    if (Rate > ans) {
                                        AlertMessages.alertPopup({
                                            errorcode: '1',
                                            errormessage: 'Rate Value Exceeded'
                                        });
                                        return false;
                                    }
                                }
                                var ttlAmnt = (isNaN(Qty) ? 0 : Qty) * (isNaN(Rate) ? 0 : Rate);
                                ttlAmnt=$filter('numnoneFlot')(ttlAmnt);
                                var TotalTaxes = Math.round(((current.PurchaseTaxDetails.IGSTTaxRate + current.PurchaseTaxDetails.SGSTTaxRate + current.PurchaseTaxDetails.CGSTTaxRate) * ttlAmnt) / 100);
                                $scope.TotalAmount += ttlAmnt;
                                $scope.TotalTaxes += TotalTaxes;
                                $scope.TotalGross += ttlAmnt + TotalTaxes;
                                Count_i++;
                            }
                        }
                    } catch (e) {}
                    try{
                        if ($scope.SelectedMatItemsList[Head_i].Matlist) {
                            for (var i = 0; i < $scope.SelectedMatItemsList[Head_i].Matlist.length; i++) {
                                var current = $scope.SelectedMatItemsList[Head_i].Matlist[i];
                                //alert($scope.Qty[Head_i][i].QVal)
                                var Qty = Qty_c[Count_i].value;
                                var Rate = Rate_c[Count_i].value;
                                if (current.last_purchase_rate != "" && current.purchase_rate_threshold != "") {
                                    var ans = (current.last_purchase_rate * (current.purchase_rate_threshold + 100)) / 100;
                                    $scope.ratePlaceholder = Rate;
                                    if (Rate > ans) {
                                        AlertMessages.alertPopup({
                                            errorcode: '1',
                                            errormessage: 'Rate Value Exceeded'
                                        });
                                        return false;
                                    }
                                }
                                var ttlAmnt = (isNaN(Qty) ? 0 : Qty) * (isNaN(Rate) ? 0 : Rate);
                                ttlAmnt=$filter('numnoneFlot')(ttlAmnt);
                                var TotalTaxes = Math.round(((current.PurchaseTaxDetails.IGSTTaxRate + current.PurchaseTaxDetails.SGSTTaxRate + current.PurchaseTaxDetails.CGSTTaxRate) * ttlAmnt) / 100);
                                $scope.TotalAmount += ttlAmnt;
                                $scope.TotalTaxes += TotalTaxes;
                                $scope.TotalGross += ttlAmnt + TotalTaxes;
                                Count_i++;
                            }
                        }

                   } catch (e) {}
                }
            }
            //Current
            if ($scope.MainMaterialElementList) {
                var Qty_c = document.getElementsByClassName("Qty");
                var Rate_c = document.getElementsByClassName("Rate");
                for (var i = 0; i < $scope.MainMaterialElementList.length; i++) {
                    var current = $scope.MainMaterialElementList[i];
                    var Qty = Qty_c[i].value;
                    var Rate = Rate_c[i].value;
                    var ttlAmnt = (isNaN(Qty) ? 0 : Qty) * (isNaN(Rate) ? 0 : Rate);
                    ttlAmnt=$filter('numnoneFlot')(ttlAmnt);
                    var TotalTaxes = Math.round(((current.PurchaseTaxDetails.IGSTTaxRate + current.PurchaseTaxDetails.SGSTTaxRate + current.PurchaseTaxDetails.CGSTTaxRate) * ttlAmnt) / 100);
                    $totaltaxes = +Totaltax;
                    $scope.TotalAmount += ttlAmnt;
                    $scope.TotalGross += $totaltaxes + ttlAmnt;
                }
            }
            //Current expense
            if ($scope.MainExpenseElementList) {
                var Qty_c = document.getElementsByClassName("Qty");
                var Rate_c = document.getElementsByClassName("Rate");
                for (var i = 0; i < $scope.MainExpenseElementList.length; i++) {
                    var current = $scope.MainExpenseElementList[i];
                    var Qty = Qty_c[i].value;
                    var Rate = Rate_c[i].value;
                    var ttlAmnt = (isNaN(Qty) ? 0 : Qty) * (isNaN(Rate) ? 0 : Rate);
                    ttlAmnt=$filter('numnoneFlot')(ttlAmnt);
                    var Totaltax = (((current.IGSTTaxRate + current.SGSTTaxRate + current.CGSTTaxRate) * ttlAmnt) / 100);
                    //var TotalTaxes = Math.round(((current.PurchaseTaxDetails.IGSTTaxRate + current.PurchaseTaxDetails.SGSTTaxRate + current.PurchaseTaxDetails.CGSTTaxRate) * ttlAmnt) / 100);
                    $scope.TotalTaxes += Totaltax;
                    $scope.TotalAmount += ttlAmnt;
                    $scope.TotalGross += Totaltax + ttlAmnt;
                }
            }
        }
        $scope.getTaxDetails = function () {
            $scope.IGST = 0;
            $scope.SGST = 0;
            $scope.CGST = 0;
            //Exist
            if ($scope.HeadVal.Itemstructure) {
                for (var i = 0; i < $scope.HeadVal.Itemstructure.length; i++) {
                    var exist = $scope.HeadVal.Itemstructure[i];
                    var ttlAmnt = exist.Order_Qty * exist.Rate;
                    ttlAmnt=$filter('numnoneFlot')(ttlAmnt);
                    // var TotalTaxes = Math.round(((exist.IGSTRate + exist.SGSTRate + exist.CGSTRate) * ttlAmnt) / 100);
                    var TotalTaxes = (((exist.IGSTRate + exist.SGSTRate + exist.CGSTRate) * ttlAmnt) / 100);
                    $scope.TotalAmount += ttlAmnt;
                    $scope.TotalTaxes += TotalTaxes;
                    $scope.TotalGross += ttlAmnt + TotalTaxes;
                }
            }
            //Current
            if ($scope.SelectedItemsList[Headkey].newObj) {
                var Qty_c = document.getElementsByClassName("Qty");
                var Rate_c = document.getElementsByClassName("Rate");
                for (var i = 0; i < $scope.SelectedItemsList[Headkey].newObj.length; i++) {
                    var current = $scope.SelectedItemsList[Headkey].newObj[i];
                    var Qty = Qty_c[i].value;
                    var Rate = Rate_c[i].value;
                    if (current.last_purchase_rate != "" && current.purchase_rate_threshold != "") {
                        var ans = (current.last_purchase_rate * (current.purchase_rate_threshold + 100)) / 100;
                        $scope.ratePlaceholder = Rate;
                        if (Rate > ans) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Rate Value Exceeded' }); return false; }
                    }
                    var ttlAmnt = (isNaN(Qty) ? 0 : Qty) * (isNaN(Rate) ? 0 : Rate);
                    ttlAmnt=$filter('numnoneFlot')(ttlAmnt);
                    var TotalTaxes = Math.round(((current.PurchaseTaxDetails.IGSTTaxRate + current.PurchaseTaxDetails.SGSTTaxRate + current.PurchaseTaxDetails.CGSTTaxRate) * ttlAmnt) / 100);
                    $scope.TotalAmount += ttlAmnt;
                    $scope.TotalTaxes += TotalTaxes;
                    $scope.TotalGross += ttlAmnt + TotalTaxes;
                }
            }
            //Current
            if ($scope.MainMaterialElementList) {
                var Qty_c = document.getElementsByClassName("Qty");
                var Rate_c = document.getElementsByClassName("Rate");
                for (var i = 0; i < $scope.MainMaterialElementList.length; i++) {
                    var current = $scope.MainMaterialElementList[i];
                    var Qty = Qty_c[i].value;
                    var Rate = Rate_c[i].value;
                    var ttlAmnt = (isNaN(Qty) ? 0 : Qty) * (isNaN(Rate) ? 0 : Rate);
                    ttlAmnt=$filter('numnoneFlot')(ttlAmnt);
                    $scope.TotalAmount += ttlAmnt;
                    $scope.TotalGross += ttlAmnt;
                }
            }
        }

        $scope.alphabet = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];
        // retrning letter of the alphabet based on index
        $scope.getLetter = function (index) {
            return $scope.alphabet[index];
        };

        $scope.selectTab = function (setTab) {
            $scope.tab = setTab;
        };
        $scope.IsTabSelected = function (checkTab) {
            return $scope.tab === checkTab;
        };
        $scope.EnableCurrentRow = function (index, HeadIndVal) {
            //alert(index + ' '+ HeadIndVal)
            $scope.ClickedIndexValue = index;
            $scope.ClickedHeadIndexValue = HeadIndVal;
        };
        $scope.EnableCurrentPatymentRow = function (index, paymentAmnt) {
            $scope.ClickedIndexValue = index;
            $scope.currentPaymentTermAmount = paymentAmnt;
        };
        $scope.checkenabledisable = function (index, predefinedindex) {
            if (index != predefinedindex) return true;
            else return false;
        };
        $scope.date = $filter('date')(new Date(), 'yyyy-MM-dd');

        $scope.getprojects = function (cmpcode) {
            $rootScope.loading = true;
            POUpdateFactoryServices.getProjectsbycmpcode({ 'CompanyCode': cmpcode }).success(function (response) {
                $rootScope.loading = false;
                $scope.projectsList = response.result;
            });
        }
        $scope.getProjClientExistInfo = function (prjcode) {
            $scope.getshippingplantstorageInfo(prjcode);
            $rootScope.loading = true;
            POUpdateFactoryServices.getProjectClientInfo({ 'ProjectCode': prjcode }).success(function (response) {
                $rootScope.loading = false;
                $scope.ClientprojectInfo = response.result;
            });
        }
        $scope.getProjClientInfo = function (fndcenterId) {
            var prjcode = $("select[name='FundCenterID_2'] option:selected").attr('fundcentercode');
            $scope.getMangersList(fndcenterId);
            $scope.getshippingplantstorageInfo(prjcode);
            $rootScope.loading = true;
            POUpdateFactoryServices.getProjectClientInfo({ 'ProjectCode': prjcode }).success(function (response) {
                $rootScope.loading = false;
                $scope.ClientprojectInfo = response.result;
            });
        }
        $scope.getMangersList = function (fndcenterId) {
            $rootScope.loading = true;
            POUpdateFactoryServices.getMangers({ 'FundCenterID': fndcenterId }).success(function (response) {
                $rootScope.loading = false;
                $scope.ManagersResult = response.result;
            });
        }
        //Get and Select WBS Head Starts
        $scope.getshippingplantstorageInfo = function (projcode) {
            $rootScope.loading = true;
            POUpdateFactoryServices.getBilling({ 'ProjectCode': projcode }).success(function (response) {
                $rootScope.loading = false;
                $scope.billingData = response.BillingData;
                $scope.shippingData = response.ShippingData;
            });
        }
        $scope.GetFundCenterByCodeCall = function (a) {
            POUpdateFactoryServices.GetFundCenterByCode({ 'SearchText': a }).success(function (response) {
                if(response.result !=null){
                    $scope.FundCenterID = response.result.Uniqueid;
                    $scope.PODetailsResult.FundCenterID = response.result.Uniqueid;
                    $scope.WBSProjectCOde=response.result.ProjectCode;
                    var prjcode = response.result.ProjectCode;
                    $scope.getshippingplantstorageInfo(prjcode);
                    $scope.getMangersList($scope.FundCenterID);
                    POUpdateFactoryServices.getProjectClientInfo({ 'ProjectCode': prjcode }).success(function (response) {
                        $scope.ClientprojectInfo = response.result;
                    });
                }
                else{$scope.FundCenterID = "";$scope.WBSProjectCOde="";$scope.ClientprojectInfo="";}
            });
        }
        $scope.getFundCenters = function (a) {
            if(a.length>3){
                $scope.fundCentersList=[];
                POUpdateFactoryServices.getFundCentersList({"SearchText":a}).success(function (response){$scope.fundCentersList=response.result;});
                $scope.GetFundCenterByCodeCall(a);
            }else AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 4 charactors to Search' }); return false;
        }
        //Get and Select WBS Head Ends
        //Search & Select WBS Element Code Starts
        $scope.pagenumberWBSD=1;$scope.pageprecountsWBSD=100;
        $scope.getWBSDetails = function () {
            if($scope.pagenumberWBSD <= '0')$scope.pagenumberWBSD=1;
            $scope.WBSList=[];
            var data = JSON.stringify($('.WBSDetailsForm').serializeObject());
            $rootScope.loading = true;
            POUpdateFactoryServices.getWBSList(data).success(function(response){$scope.WBSList=response.result;$rootScope.loading = false;});
        }
        $scope.pagenationWBSD = function(a, b) {
            var b = $("select[name='pageprecountsWBSD']").val();$scope.pagenumberWBSD=a;$scope.pageprecountsWBSD=b;
            setTimeout(() => {$scope.getWBSDetails();}, 300);
        }
        $scope.pagenationWBSD_Clk= function(a, b) {
            if ("0" == b) var a1 = --a; else var a1 = ++a;
            "0" >= a1 ? (a1 = 0, $scope.pagenumberWBSD=1) : $scope.pagenumberWBSD=a1;
            var c = $("select[name='pageprecountsWBSD']").val();
            $scope.pagenumberWBSD=a1;$scope.pageprecountsWBSD=c;
            setTimeout(() => {$scope.getWBSDetails();}, 300);
        }
        $scope.setWBSData = function (a) {
            if(a){

                $scope.wbsPart1bgColor = [];
                if ($scope.isexistMaterial) {
                    $scope.HeadItem.ItemStructureList[$scope.HeaderPartRowNo].Itemstructure[$scope.ClickedIndexValue].WBSElementCode = a.wbsCode;
                } else {
                    if($scope.LineType == 0)
                    $scope.wbscodePart1[$scope.HeaderPartRowNo][$scope.wbsPartRowNo] = a.wbsCode;
                    else if($scope.LineType == 1)
                    $scope.wbscodePart2[$scope.HeaderPartRowNo][$scope.wbsPartRowNo] = a.wbsCode;
                }
                $("#WBSDetailsPart1Popup").modal("hide");
            }else AlertMessages.alertPopup({errorcode:'1',errormessage:'WBS is Manadatory'});
        }

        $scope.setWBSDataMain = function (a) {
            if(a){

                $scope.wbsPart1bgColor = [];
                if ($scope.isexistMaterial) {
                    $scope.HeadItem.ItemStructureList[$scope.HeaderPartRowNo].Itemstructure[$scope.ClickedIndexValue].WBSElementCode = a.wbsCode;
                } else {
                    $scope.wbscodePart1[$scope.HeaderPartRowNo][$scope.wbsPartRowNo] = a.wbsCode;
                }
                $("#WBSDetailsPart1Popup").modal("hide");
            }else AlertMessages.alertPopup({errorcode:'1',errormessage:'WBS is Manadatory'});
        }

        $scope.setWBSDataExpMain = function (a) {
            if(a){

                $scope.wbsPart1bgColor = [];
                if ($scope.isexistMaterial) {
                    $scope.wbscodePart1[$scope.wbsPartRowNo] = a.wbsCode;
                } else {
                    $scope.wbscodePart1[$scope.wbsPartRowNo] = a.wbsCode;
                }
                $("#WBSDetailsPart1Popup").modal("hide");
            }else AlertMessages.alertPopup({errorcode:'1',errormessage:'WBS is Manadatory'});
        }

        $scope.getWBSDetailsByReset= function(){
            $scope.Wf1="",$scope.Wf2="",$scope.Wf3="",$scope.Wf4="";
            setTimeout(() => {$scope.getWBSDetails();}, 300);
        }
        //Search & Select WBS Element Code Ends
        //Search & Select HSN Element Code Starts
        $scope.pagenumberHSN=1;$scope.pageprecountsHSN=100;
        $scope.getHSNDetails = function () {
            if($scope.pagenumberHSN <= '0')$scope.pagenumberHSN=1;
            $scope.HSNList=[];
            var data = JSON.stringify($('.HSNDetailsForm').serializeObject());
            $rootScope.loading = true;
            POUpdateFactoryServices.getHSNList(data).success(function(response){$scope.HSNList=response.result;$rootScope.loading = false;});
        }
        $scope.pagenationHSN = function(a, b) {
            var b = $("select[name='pageprecountsHSN']").val();$scope.pagenumberHSN=a;$scope.pageprecountsHSN=b;
            setTimeout(() => {$scope.getHSNDetails();}, 300);
        }
        $scope.pagenationHSN_Clk= function(a, b) {
            if ("0" == b) var a1 = --a; else var a1 = ++a;
            "0" >= a1 ? (a1 = 0, $scope.pagenumberHSN=1) : $scope.pagenumberHSN=a1;
            var c = $("select[name='pageprecountsHSN']").val();
            $scope.pagenumberHSN=a1;$scope.pageprecountsHSN=c;
            setTimeout(() => {$scope.getHSNDetails();}, 300);
        }
        $scope.setHSNData = function (a) {
            if(a){
                $("#HSNDetailsPart1Popup").modal("hide");
                $scope.AddExpenses(a.HSNCode,$scope.SelKeys);
            }else AlertMessages.alertPopup({errorcode:'1',errormessage:'HSN is Manadatory'});
        }
        $scope.getHSNDetailsByReset= function(a){
            $scope.hf1="",$scope.hf2="";
            $scope.SelKeys=a;
            setTimeout(() => {$scope.getHSNDetails();}, 300);
        }
        //Search & Select HSN Element Code Ends


        $scope.getPOfullDetailsById = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getPODetailsById({ 'UniqueId': $scope.PO_HSID }).success(function (response) {
                //$rootScope.loading = false;
                $scope.PODetailsResult = response.result;
                $scope.POApproversList = response.ApproversList;
                $scope.NatureofSupply = response.NatureofSupply;
                $scope.NatureofTransaction = response.NatureofTransaction;
                $scope.TransactionType = response.TransactionType;
                if ($scope.PODetailsResult != null) {
                    //Begin of Assigning Data to Local Object
                    $scope.selectedManager.CallName = $scope.PODetailsResult.POManagerName;
                    $scope.selectedManager.UserId = $scope.PODetailsResult.POManagerID;
                    $scope.selectedVendorDetails = {};
                    $scope.selectedVendorDetails.POVendorDetailId = $scope.PODetailsResult.VendorID;
                    $scope.selectedVendorDetails.VendorName = $scope.PODetailsResult.VendorName;
                    $scope.selectedVendorDetails.VendorCode = $scope.PODetailsResult.VendorCode;
                    //End  of Assigning Data to Local Object
                    //$scope.getWBSDetails();
                    $scope.poCode = $scope.PODetailsResult.PO_OrderTypeCode;
                    $scope.getProjClientExistInfo($scope.PODetailsResult.ProjectCode);
                    $scope.getMangersList($scope.PODetailsResult.FundCenterID);
                    $scope.shippingDet = $scope.PODetailsResult.ShipToData;
                    if ($scope.PODetailsResult.ShippedToID != null)
                        $scope.PODetailsResult.ShippedToID = $scope.PODetailsResult.ShippedToID.toString();
                    if ($scope.PODetailsResult.FundCenterID != null)
                        $scope.PODetailsResult.FundCenterID = $scope.PODetailsResult.FundCenterID.toString();
                    if ($scope.PODetailsResult.PO_OrderTypeID != null)
                        $scope.PODetailsResult.PO_OrderTypeID = $scope.PODetailsResult.PO_OrderTypeID.toString();
                }
            });
        }
        $scope.getPOfullDetailsById();

        // $scope.GetPurchaseItems = function () {
        //     $rootScope.loading = true;
        //     POUpdateFactoryServices.GetPurchaseItemsPOId({ 'POHeaderStructureID': $scope.PO_HSID }).success(function (response) {
        //         $rootScope.loading = false;
        //         $scope.ExistpurcItemCount = response.info.totalrecords;
        //         $scope.ExistpurchaseItemsResult = response.result;
        //     });
        // };$scope.GetPurchaseItems();

        $scope.GetPurchaseItems = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.GetServiceHeadItemsList({ 'POHeaderStructureID': $scope.PO_HSID }).success(function (response) {
                $rootScope.loading = false;
                $scope.ExistpurcItemCount = response.result.ItemStructureList[0].Itemstructure.length;
                if(response.result.ItemStructureList.length > 0 && response.result.ItemStructureList[0].HeadTitle != null)
                    $scope.PO_Status = 0;
                    else
                    $scope.PO_Status = 1;

                $scope.HeadItem = response.result;

                $scope.isfirsttimeadding = [];
                $scope.isMatfirsttimeadding = [];

                $scope.getTotalAmountTaxGross();
                var len = $scope.HeadItem.ItemStructureList.length - 1;
                
                $scope.isMatfirsttimeadding[len] = true;
                
                angular.forEach($scope.HeadItem.ItemStructureList,function(v,k){
                    $scope.Numbering();
                    $scope.isfirsttimeadding[k] = true;
                });
            });
        };$scope.GetPurchaseItems();

        $scope.getLinkedPO = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.GetLinkedPO({ 'POHeaderStructureID': $scope.PO_HSID }).success(function (response) {
                $rootScope.loading = false;
                $scope.LinkedPOList = response.LinkedPOList;
                $scope.NonLinkedPODetails = response.POList;
            });
        }
        $scope.getLinkedPO();
        $scope.getGeneralTermsandCond = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.GetPOGeneralTandC({ 'POHeaderstructureID': $scope.PO_HSID }).success(function (response) {
                $rootScope.loading = false;
                $scope.GeneralTermsandConditionsList = response;
            });
        }
        $scope.getGeneralTermsandCond();
        $scope.getPOSpecificTermsandCond = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.GetPOSPecificTermCoditions({ 'POHeaderStructureID': $scope.PO_HSID }).success(function (response) {
                $rootScope.loading = false;
                $scope.POSpecTCResult = response;
            });
        }
        $scope.getPOSpecificTermsandCond();
        $scope.getPOTechnicalSpecs = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.GetAnnexSPecifications({ 'POHeaderStructureID': $scope.PO_HSID }).success(function (response) {
                $rootScope.loading = false;
                $scope.POTechnicalSpecsList = response.result;
            });
        }
        $scope.getPOTechnicalSpecs();
        $scope.getPOAnnexSpecs = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getMaterialAnnexureCheckListData({ 'POHeaderStructureID': $scope.PO_HSID }).success(function (response) {
                $rootScope.loading = false;
                $scope.AnnexureList = response;
            });
        }
        $scope.getPOAnnexSpecs();
        $scope.getPOServiceAnnexSpecs = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getServiceAnnexureCheckListData({ 'POHeaderStructureID': $scope.PO_HSID }).success(function (response) {
                $rootScope.loading = false;
                $scope.ServiceAnnexureList = response;
            });
        }
        $scope.getPOServiceAnnexSpecs();

        $scope.SelectAllTandC = function () {
            $scope.genAll = $('input[name="TandCID"]').length == $('input[name="TandCID"]:checked').length ? true : false;
        }
        $scope.SelectAllLink = function () {
            $scope.linkAll = $('input[name="LinkCID"]').length == $('input[name="LinkCID"]:checked').length ? true : false;
        }
        $scope.SaveTermConditions = function () {
            var TCList = [];
            $('input[name="TandCID"]').each(function () {
                if (this.checked) {
                    TCList.push($(this).attr('value'));
                }
            });
            if (TCList.length == 0) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Select at least one Condition' }); return false; }
            else {
                $rootScope.loading = true;
                POUpdateFactoryServices.SaveGeneralTandC({
                    'POHeaderstructureID': $scope.PO_HSID,
                    'PickListItemId': $scope.GeneralTermsandConditionsList.picklistitemitmId,
                    'MasterTandCId': TCList,
                    'LastModifiedBy': $scope.UserDetails.currentUser.UserId
                }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.getGeneralTermsandCond();
                });
            }
        }
        $scope.deleteGenTandC = function () {
            var data = JSON.stringify($('.deleteTCForm').serializeObject());
            POUpdateFactoryServices.DeleteGeneralTandC(data).success(function (response) {
                if (response.info.errorcode == '0') { $('#DeleteGenTCPopUp').modal('hide'); $scope.getPOSpecialTandC(); }
                AlertMessages.alertPopup(response.info);
                $scope.getGeneralTermsandCond();
            });
        }
        $scope.SaveLinkPo = function () {
            var LPList = [];
            $('input[name="LinkCID"]').each(function () {
                if (this.checked) {
                    LPList.push($(this).attr('value'));
                }
            });
            if (LPList.length == 0) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Select at least one Condition' }); return false; }
            else {
                $rootScope.loading = true;
                POUpdateFactoryServices.SaveLinkP({
                    'MainPOID': $scope.PO_HSID,
                    'LinkedPOID': LPList,
                    'LastModifiedBy': $scope.UserDetails.currentUser.UserId
                }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.getLinkedPO();
                });
            }
        }

        $scope.deleteLinkedP = function (uniqueId, lastModifiedBy) {
            var data = { "UniqueID": uniqueId, "LastModifiedBy": lastModifiedBy };
            POUpdateFactoryServices.DeleteLinkedPO(data).success(function (response) {
                if (response.info.errorcode == '0') { $('#DeleteLinkedPopUp').modal('hide'); $scope.getPOSpecialTandC(); }
                AlertMessages.alertPopup(response.info);
                $scope.getLinkedPO();
            });
        }
        $scope.getCustomers = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getCustomersList().success(function (response) {
                $rootScope.loading = false;
                $scope.CustomersList = response.result;
            });
        }
        $scope.getCustomers();
        // $scope.getFundCenters = function () {
        //     $rootScope.loading = true;
        //     POUpdateFactoryServices.getFundCentersList().success(function (response) {
        //         $rootScope.loading = false;
        //         $scope.fundCentersList = response.result;
        //     });
        // }
        //$scope.getFundCenters();
        $scope.getFundCenter = function (fundcenterId) {
            $rootScope.loading = true;
            POUpdateFactoryServices.getFundCenter({ 'ID': parseInt(fundcenterId) }).success(function (response) {
                $rootScope.loading = false;
                $scope.fundCenter = response.result;
            });
        }
        $scope.shippingDetails = function (shiptoId) {
            $rootScope.loading = true;
            POUpdateFactoryServices.getShippingDetails({ 'ID': parseInt(shiptoId) }).success(function (response) {
                $rootScope.loading = false;
                $scope.shippingDet = response.result;
            });
        }
        $scope.searchMaterial = function (searchObj) {
            if (searchObj != null) {
                if (searchObj.length < 3) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
                }
                else {
                    $rootScope.loading = true;
                    POUpdateFactoryServices.searchMaterialGroups({ 'Search': searchObj }).success(function (response) {
                        $rootScope.loading = false;
                        $scope.SearchedMaterialGroupList = response.result;
                        $scope.MaterialLists = [];
                        //$scope.grpName = " ";
                        setTimeout(function(){$('select[name="grpName"]').val('');},300);
                    });
                }
            }
            else {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
            }
        }
        $scope.searchbyMaterial = function (searchObj) {
            if (searchObj != null) {
                if (searchObj.length < 3) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
                }
                else {
                    $rootScope.loading = true;
                    POUpdateFactoryServices.searchbyMaterialGroups({ 'Search': searchObj }).success(function (response) {
                        $rootScope.loading = false;
                        $scope.MaterialLists = response.result;
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

        $scope.searchbyService = function (searchObj) {
            if (searchObj != null) {
                if (searchObj.length < 3) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
                }
                else {
                    $rootScope.loading = true;
                    POUpdateFactoryServices.searchServiceByIDGroups({ 'Search': searchObj }).success(function (response) {
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

        $scope.searchService = function (searchObj) {
            if (searchObj != null) {
                if (searchObj.length < 3) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
                }
                else {
                    $rootScope.loading = true;
                    POUpdateFactoryServices.searchServiceGroups({ 'Search': searchObj }).success(function (response) {
                        $rootScope.loading = false;
                        $scope.SearchedServiceGroupList = response.result;
                        $scope.ServiceLists = [];
                        //$scope.grpName = " ";
                        setTimeout(function(){$('select[name="grpServName"]').val('');},300);

                    });
                }
            }
            else {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Enter minimum 3 characters to Search' }); return false;
            }
        }

        $scope.searchwithinGroup = function (grpName, searchObj) {
            if (grpName == null || grpName == "") {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please select searched material groups' }); return false;
            }
            else {
                grpName = grpName.trim()
                $rootScope.loading = true;
                POUpdateFactoryServices.searchWithinMaterialGroups({ 'groupname': grpName, 'searchQuery': searchObj }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.MaterialLists = response.result;
                });
            }
        }
        $scope.searchwithinServGroup = function (grpName, searchObj) {
            if (grpName == null || grpName == "") {
                AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Please select searched material groups' }); return false;
            }
            else {
                grpName = grpName.trim()
                $rootScope.loading = true;
                POUpdateFactoryServices.searchWithinServiceGroups({ 'groupname': grpName, 'searchQuery': searchObj }).success(function (response) {
                    $rootScope.loading = false;
                    $scope.ServiceLists = response.result;
                });
            }
        }
        // $scope.getVendors = function () {
        //     $rootScope.loading = true;
        //     POUpdateFactoryServices.getVendorsList().success(function (response) {
        //         $rootScope.loading = false;
        //         $scope.VendorsList = response.result;
        //     });
        // }
        //$scope.getVendors();
        $scope.getPOSpecificTCTitle = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getPOSpecificTCTitle().success(function (response) {
                $rootScope.loading = false;
                $scope.POSpecTitleList = response.result;
            });
        }
        $scope.getPOSpecificTCTitle();
        $scope.getpoSpecTCSubTitles = function (titleid) {
            $rootScope.loading = true;
            POUpdateFactoryServices.getPOSpecificTCSubTitle({ "SpecificTCTitleMasterId": titleid }).success(function (response) {
                $rootScope.loading = false;
                $scope.POSpecSubTitleList = response.result;
            });
        }
        $scope.getVendorData = function (venderId) {
            $rootScope.loading = true;
            POUpdateFactoryServices.getVendor({ 'ID': parseInt(venderId) }).success(function (response) {
                $rootScope.loading = false;
                $scope.vendorData = response.result;
            });
        }
        $scope.MaterialHeadID = 0;
        $scope.GetAllMaterials = function (Headkey) {

            var temp;
            var data = JSON.stringify($('.generalInformation').serializeObject());
            var validate = JSON.parse(data);
            if (!validate.ShippedToID || validate.ShippedToID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Ship To is Mandatory' }); return false; }
            if (validate.CountryCode == 'India') {
                if (validate.ShiptoStateCode == '' || validate.ShipfromStateCode == '') {
                    temp = false; AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Ship  To GST State Code and Ship from GST State Code are Mandatory ' }); return false;
                } else temp = true;
            }

            else {
                temp = true;
            }
            if (temp == true) {
                $("#ComponentLibraryPopup").modal("show");
                $scope.MaterialHeadID = Headkey;
//alert($scope.MaterialHeadID);
                $scope.MaterialLists = [];
                $scope.tempMaterialLists = [];
            }
        }


        $scope.GetServicepopup = function (Headkey) {

            var temp;
            var data = JSON.stringify($('.generalInformation').serializeObject());
            var validate = JSON.parse(data);
            if (!validate.ShippedToID || validate.ShippedToID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Ship To is Mandatory' }); return false; }
            if (validate.CountryCode == 'India') {
                if (validate.ShiptoStateCode == '' || validate.ShipfromStateCode == '') {
                    temp = false; AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Ship  To GST State Code and Ship from GST State Code are Mandatory ' }); return false;
                } else temp = true;
            }

            else {
                temp = true;
            }
            if (temp) {
                $("#ServicePopup").modal("show");
                $scope.serviceHeadID = Headkey;

                $scope.MaterialLists = [];
                $scope.tempMaterialLists = [];
            }
        }

        $scope.CancelServiceHeader = function(){
            $('.ServHead').val("");
            $('.ServHeadDesc').val("");

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
{        POUpdateFactoryServices.AddServiceHeaderitem({"Title":validate.ServHead, "Description":validate.ServHeadDesc, "POHeaderStructureid":$scope.PO_HSID}).success(function(response){
        if(response.info.errorcode == 0)
        {
        $('#ServiceHeadPopup').modal('hide');
        
    // for(var i = 0; i < $scope.HeadItem.ItemStructureList.length; i++)
    // {
    //     $scope.HeadCount[i] = SelectedMatItemsList[Headkey].Matlist
    // }
        $scope.GetPurchaseItems();
        
        $('.ServHead').val("")
        $('.ServHeadDesc').val("")
        //$scope.ServHeadDesc = "";
        $scope.Numbering();
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
        POUpdateFactoryServices.UpdServiceHeaderitem({"UniqueID":validate.ServUpdUniqueID, "Title":validate.ServUpdHead, "Description":validate.ServUpdHeadDesc, "POHeaderStructureid":$scope.PO_HSID}).success(function(response){


        $('.ServupdIDHead').ServUpdHead = {};
        $('.ServupdIDHead').ServUpdHeadDesc = {};
        if(response.info.errorcode == 0)
        {
        $('#ServiceHeadUpdatePopup').modal('hide');
        $scope.GetPurchaseItems();
        }

        {AlertMessages.alertPopup({ errorcode: response.info.errorcode, errormessage: response.info.errormessage }); return false;}
        });
}
    }
    $scope.UpdServiceHeaderPopup = function(UniqueId, Title, Description){
        //alert(UniqueId+"-"+ Title+"-"+  Description)
        $scope.ServUpdHead = Title;
        $scope.ServUpdHeadDesc = Description;
        $scope.ServUpdUniqueID = UniqueId;

        $("#ServiceHeadUpdatePopup").modal("show");
     }

        $scope.GetHeaderpopup = function(){

           $("#ServiceHeadPopup").modal("show");


        }


        $scope.HeaderLengthCheck = function(){
           // alert($scope.ServHead)

        }
$scope.EditMode = function(BoolVal, headermodel)
{
        $scope.isEditText = BoolVal;
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

            POUpdateFactoryServices.delServiceHeaderitem({"UniqueID":validate.delHeaderUniqueID})
            .success(function(response){
                if(response.info.errorcode == 0)
                {
                if($scope.HeadItem.ItemStructureList.length < 1 && $scope.HeadItem.ItemStructureList[0].HeadTitle == null)
                    $scope.PO_Status = 1;

                    //alert($scope.DelIndex);
                    try{
                    $scope.SelectedItemsList[$scope.DelIndex].newObj = [];
                    }catch(e){}

                $('#DeleteheaderPopUp').modal('hide');
                $scope.GetPurchaseItems();
                }else
                {AlertMessages.alertPopup({ errorcode: response.info.errorcode, errormessage: response.info.errormessage }); return false;}
            });
        }


        $scope.setManagerData = function (key, data) {
            $scope.ManagerbgColor = [];
            $scope.selectedManager = data;
            $scope.ManagerbgColor[key] = 'darkseagreen';
        }
        //Search & Select Vendors Starts
        $scope.pagenumberVD=1;$scope.pageprecountsVD=100;
        $scope.getVendors = function () {
            if($scope.pagenumberVD <= '0')$scope.pagenumberVD=1;
            $scope.VendorsList=[];
            var data = JSON.stringify($('.VenderDetailsForm').serializeObject());
            $rootScope.loading = true;
            POUpdateFactoryServices.getVendorsList(data).success(function(response){$scope.VendorsList=response.result;$rootScope.loading = false;});
        }
        $scope.pagenationVD = function(a, b) {
            var b = $("select[name='pageprecountsVD']").val();$scope.pagenumberVD=a;$scope.pageprecountsVD=b;
            setTimeout(() => {$scope.getVendors();}, 300);
        }
        $scope.pagenationVD_Clk= function(a, b) {
            if ("0" == b) var a1 = --a; else var a1 = ++a;
            "0" >= a1 ? (a1 = 0, $scope.pagenumberVD=1) : $scope.pagenumberVD=a1;
            var c = $("select[name='pageprecountsVD']").val();
            $scope.pagenumberVD=a1;$scope.pageprecountsVD=c;
            setTimeout(() => {$scope.getVendors();}, 300);
        }
        $scope.setVendorData = function (a) {
            if(a){
                $scope.selectedVendorDetails=a;$("#VenderDetailsPopup").modal("hide");
            }else AlertMessages.alertPopup({errorcode:'1',errormessage:'Vendor is Manadatory'});
        }
        $scope.getVendorsByReset= function(){
            $scope.f1="",$scope.f2="",$scope.f3="",$scope.f4="",$scope.f5="",$scope.f6="";
            setTimeout(() => {$scope.getVendors();}, 300);
        }
        //Search & Select Vendors Ends




        $scope.wbsPartRowNo = 0;
        $scope.HeaderPartRowNo = 0;
        $scope.wbscodePart1 = [];
        $scope.wbscodePart2 = [];
        $scope.isexistMaterial = false;
        $scope.isexistServ = false;
            $scope.LineType = 0;
        $scope.setrowclickedKey = function (index, headerIndex, isexist, Type = 0) {
            $scope.LineType = Type;
            $scope.wbsPartRowNo = index;
            $scope.HeaderPartRowNo = headerIndex;
            $scope.isexistMaterial = isexist;
        }

        $scope.setrowclickedKeyMain = function (index, isexist) {
            $scope.wbsPartRowNo = index;
            $scope.isexistMaterial = isexist;
        }

        $scope.setWbsPart1Code = function (key, data) {
            $scope.wbsPart1bgColor = [];
            if ($scope.isexistMaterial) {
                $scope.HeadVal.Itemstructure[$scope.ClickedIndexValue].WBSElementCode = data.wbsCode;
            } else {
                $scope.wbscodePart1[$scope.wbsPartRowNo] = data.wbsCode;
            }
            $scope.wbsPart1bgColor[key] = 'darkseagreen';
        }

        $scope.setWbsPart2Code = function (key, data) {
            $scope.wbsPart2bgColor = [];
            if ($scope.isexistMaterial) {
                $scope.HeadVal.Itemstructure[$scope.ClickedIndexValue].WBSElementCode2 = data.wbsCode;
            } else {
                $scope.wbscodePart2[$scope.wbsPartRowNo] = data.wbsCode;
            }
            $scope.wbsPart2bgColor[key] = 'darkseagreen';
        }
        $scope.getGLAccDetails = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getGLAccDetails().success(function (response) {
                $rootScope.loading = false;
                $scope.getGLAccountList = response.result;
            });
        }
        $scope.getGLAccDetails();

        $scope.gettaxdetailsforItem = function (hsn) {
            $rootScope.loading = true;
            POUpdateFactoryServices.gettaxdetails({ "HSNCode": hsn, "CountryCode": $scope.PODetailsResult.Country, "VendorRegionCode": $scope.PODetailsResult.RegionCode, "PlantRegionCode": $scope.PODetailsResult.ShipToData.StateCode }).success(function (response) {
                $rootScope.loading = false;
                $scope.taxresult = response.result;
            });
        }
        $scope.getMaterialCheckListInfoBySerializerValue = function (data) {
            var validate = JSON.parse(data);
            $rootScope.loading = true;
            POUpdateFactoryServices.materialCheckListInfo({ 'CheckListId': validate.id }).success(function (response) {
                $rootScope.loading = false;
                $scope.MaterialCheckListResult = response.result;;
            });
        }
        $scope.getMaterialCheckListInfo = function (checklistId) {
            $rootScope.loading = true;
            POUpdateFactoryServices.materialCheckListInfo({ 'CheckListId': checklistId }).success(function (response) {
                $rootScope.loading = false;
                $scope.MaterialCheckListResult = response.result;;
            });
        }
        $scope.getPOTypes = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getPOTypes().success(function (response) {
                $rootScope.loading = false;
                $scope.orderTypeList = response.result;;
            });
        }
        $scope.getPOTypes();
        $scope.getPoTypeCode = function () {
            $scope.poCode = $("select[name='PO_OrderID'] option:selected").attr('poCode');
        }
        $scope.RemoveBindedMaterialElement = function (headIndex,index) {
            $scope.SelectedItemsList[headIndex].newObj.splice(index, 1);
        };
        $scope.swapTempMaterialData = function () {
            $scope.MaterialLists = $scope.tempMaterialLists;
            $scope.SearchedMaterialGroupList = [];
            $scope.SearchedServiceGroupList = [];
            $scope.ServiceLists = [];
            $('#SearchServiceID').val('');
            $('.SearchMatID').val('');
            $('.SearchServID').val('');
            // $scope.searchObj = "";
            // $scope.searchServiceObj = "";
            setTimeout(function(){$('select[name="grpName"]').val('');},300);
            setTimeout(function(){$('select[name="grpServName"]').val('');},300);
        };
        var ObjMatrlCode = {};
        $scope.AddMaterial = function () {
            var MaterialHeadID = $scope.MaterialHeadID
            //if ($scope.PODetailsResult.Country != null && $scope.PODetailsResult.Country != '' &&
            //    $scope.PODetailsResult.ShipToData.StateCode != null && $scope.PODetailsResult.ShipToData.StateCode != '' &&
            //    $scope.PODetailsResult.RegionCode != null && $scope.PODetailsResult.RegionCode != '') {
                var newDataList = [];
                var count = $(".cmplibrary:checked").length;
                if (count == 0) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Select at least one Material' });
                    return false;
                }
                $(".cmplibrary:checked").each(function (key, val) {
                    var hsncode = $(this).attr("hsncode");
                    var currentmaterialrowData = JSON.parse($(this).attr("data"));
                    if (hsncode != null && hsncode != undefined && hsncode != "") {
                        POUpdateFactoryServices.gettaxdetails({ "HSNCode": hsncode, "CountryCode": $scope.PODetailsResult.Country, "VendorRegionCode": $scope.PODetailsResult.RegionCode, "PlantRegionCode": $scope.PODetailsResult.ShipToData.StateCode, "VendorGSTApplicabilityId": $scope.PODetailsResult.VendorGSTApplicability, "MaterialGSTApplicabilityId": currentmaterialrowData.gst_procurement, "OrderType":"MaterialOrder"}).success(function (response) {
                            if (response.info.errorcode == '0' && response.result != null && response.result != "") {
                                currentmaterialrowData.PurchaseTaxDetails = response.result;
                     if(currentmaterialrowData.PurchaseTaxDetails.CheckDat == '0')
                     {
                        newDataList = [];
                     AlertMessages.alertPopup({ errorcode: '1', errormessage: 'HSN Code is Not Mapped in the Master Data please Contact Anitha to do.' });
                        return false;}
                            }
                        });
                    }
                    else {
                        newDataList = [];
                        AlertMessages.alertPopup({ errorcode: '1', errormessage: 'HSN Code is Not Available in Component Library' });
                        return false;
                    }
                    newDataList.push(currentmaterialrowData);
                    if (key == (count - 1)) {
                        $('#ComponentLibraryPopup').modal('hide');
                        $scope.SearchedMaterialGroupList = [];
                        $scope.searchObj = '';
                        $(".SearchMatID").val("");
                        //$scope.SelectedItemsList = [];
                        $scope.Mataced(newDataList, MaterialHeadID);
                        $scope.MaterialLists = [];
                    }
                });
            //}
            //else { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Insufficient Basic or General Information' }); $rootScope.loading = false; }
        };
        $scope.CGSTTaxRate = [];
        $scope.IGSTTaxRate = [];
        $scope.PurchasingOrderNumber = [];
        $scope.SGSTTaxRate = [];
        $scope.TaxCode = [];
        $scope.AddExpenses = function (hsncode,key) {
            if ($scope.PODetailsResult.Country != null && $scope.PODetailsResult.Country != '' &&
                $scope.PODetailsResult.ShipToData.StateCode != null && $scope.PODetailsResult.ShipToData.StateCode != '' &&
                $scope.PODetailsResult.RegionCode != null && $scope.PODetailsResult.RegionCode != '') {

                if (hsncode != null && hsncode != undefined && hsncode != "") { $rootScope.loading = true;
                    POUpdateFactoryServices.gettaxdetails({ "HSNCode": hsncode, "CountryCode": $scope.PODetailsResult.Country, "VendorRegionCode": $scope.PODetailsResult.RegionCode, "PlantRegionCode": $scope.PODetailsResult.ShipToData.StateCode, "VendorGSTApplicabilityId": $scope.PODetailsResult.VendorGSTApplicability, "MaterialGSTApplicabilityId": "", "OrderType":"ExpenseOrder"}).success(function (response) {
                        $rootScope.loading = false;
                        if (response.info.errorcode == '0' && response.result != null && response.result != "") {
                            var data = response.result;
                            //console.log(data);
                            $scope.MainExpenseElementList[key].HSNCode = hsncode;
                            $scope.MainExpenseElementList[key].CGSTTaxRate = $scope.CGSTTaxRate[key] = data.CGSTTaxRate;
                            $scope.MainExpenseElementList[key].IGSTTaxRate = $scope.IGSTTaxRate[key] = data.IGSTTaxRate;
                            $scope.MainExpenseElementList[key].SGSTTaxRate = $scope.SGSTTaxRate[key] = data.SGSTTaxRate;
                            $scope.PurchasingOrderNumber[key] = data.PurchasingOrderNumber;
                            $scope.TaxCode[key] = data.TaxCode;
                        }
                    });
                }
            }
            else { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Insufficient Basic or General Information' }); }
        };
        $scope.AddService = function (serviceHeadID) {
            //if ($scope.PODetailsResult.Country != null && $scope.PODetailsResult.Country != '' &&
            //    $scope.PODetailsResult.ShipToData.StateCode != null && $scope.PODetailsResult.ShipToData.StateCode != '' &&
            //    $scope.PODetailsResult.RegionCode != null && $scope.PODetailsResult.RegionCode != '') {
                // $sessionStorage.ServiceHeadID.push(serviceHeadID);
                
                var newDataList = [];
                var count = $(".cmplibrary_Service:checked").length;
                if (count == 0) {
                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Select at least one Service' });
                    return false;
                }
                var flagValue = $scope.HeadItem.ItemStructureList[serviceHeadID].Itemstructure[0];
                var igstRate, sgstRate, cgstRate;
                if(flagValue != undefined)
                {
                    igstRate = $scope.HeadItem.ItemStructureList[serviceHeadID].Itemstructure[0].IGSTRate;
                    sgstRate = $scope.HeadItem.ItemStructureList[serviceHeadID].Itemstructure[0].SGSTRate;
                    cgstRate = $scope.HeadItem.ItemStructureList[serviceHeadID].Itemstructure[0].CGSTRate;
                }

                $(".cmplibrary_Service:checked").each(function (key, val) {
                    var hsncode = $(this).attr("hsncode");
                    var currentmaterialrowData = JSON.parse($(this).attr("data"));
                    if (hsncode != null && hsncode != undefined && hsncode != "") {
                        var flag = 0;
                        POUpdateFactoryServices.gettaxdetails({ "HSNCode": hsncode, "CountryCode": $scope.PODetailsResult.Country, "VendorRegionCode": $scope.PODetailsResult.RegionCode, "PlantRegionCode": $scope.PODetailsResult.ShipToData.StateCode, "VendorGSTApplicabilityId": $scope.PODetailsResult.VendorGSTApplicability, "MaterialGSTApplicabilityId":"", "OrderType":"ServiceOrder"}).success(function (response) {
                            if (response.info.errorcode == '0' && response.result != null && response.result != "") {
                                currentmaterialrowData.PurchaseTaxDetails = response.result;
                                newDataList.push(currentmaterialrowData);
                                if(currentmaterialrowData.PurchaseTaxDetails.CheckDat == '0'){
                                    newDataList = [];
                                    AlertMessages.alertPopup({ errorcode: '1', errormessage: 'HSN Code is Not Mapped in the Master Data please Contact Anitha Nataraj to do.' });
                                    flag = 1;
                                    //return false;
                                }
                                
                                if(igstRate != undefined)
                                {
                                    if(currentmaterialrowData.PurchaseTaxDetails.IGSTTaxRate != igstRate || currentmaterialrowData.PurchaseTaxDetails.SGSTTaxRate != sgstRate || currentmaterialrowData.PurchaseTaxDetails.CGSTTaxRate != cgstRate)
                                    {
                                        AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Different tax record found.' });
                                        flag = 1;
                                        //return false;
                                    }
                                }else{
                                    igstRate = currentmaterialrowData.PurchaseTaxDetails.IGSTTaxRate;
                                    sgstRate = currentmaterialrowData.PurchaseTaxDetails.CGSTTaxRate;
                                    cgstRate = currentmaterialrowData.PurchaseTaxDetails.SGSTTaxRate;
                                }
                                if (key == (count - 1) && flag == 0) {
                                    
                                            $('.SearchServID').val('');
                                            $('#SearchServiceID').val('');
                                            $('#ServicePopup').modal('hide');
                                            $scope.SearchedServiceGroupList = [];
                                            $scope.searchServiceObj = '';
                                            //$scope.SelectedItemsList = [];
                                            $scope.aced(newDataList, serviceHeadID);
                                            $scope.ServiceLists = [];
                                        }
                            }
                            //console.log($scope.HeadItem.ItemStructureList[serviceHeadID].Itemstructure[0]);
                        });
                    } else {
                        newDataList = [];
                        AlertMessages.alertPopup({ errorcode: '1', errormessage: 'HSN Code is Not Available in Component Library' });
                        return false;
                    }
                    // newDataList.push(currentmaterialrowData);
                    // if (key == (count - 1)) {
                    //     console.log(newDataList);
                    //             $('.SearchServID').val('');
                    //             $('#SearchServiceID').val('');
                    //             $('#ServicePopup').modal('hide');
                    //             $scope.SearchedServiceGroupList = [];
                    //             $scope.searchServiceObj = '';
                    //             //$scope.SelectedItemsList = [];
                    //             $scope.aced(newDataList, serviceHeadID);
                    //             $scope.ServiceLists = [];
                    //         }
                });
            //}
            //else { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Insufficient Basic or General Information' }); $rootScope.loading = false; }

        };

        function onlyUnique(value, index, self) {
            return self.indexOf(value) === index;
        }

        $scope.aced = function (MaterialListsea, serviceHeadID) {

            if ($scope.isfirsttimeadding[serviceHeadID]) {
                $scope.isfirsttimeadding[serviceHeadID] = false;
                var MatList = "SelectedItemsList";
                //$scope.SelectedItemsList = MaterialListsea;
                //$scope.PreviousSelectedItemsList = MaterialListsea;
                $scope.SelectedItemsList[serviceHeadID] = {};
                $scope.SelectedItemsList[serviceHeadID].newObj = [];
                $scope.wbscodePart1[serviceHeadID] = [];
            }
            else {
                $scope.mergedItemsList = $scope.PreviousSelectedItemsList.concat(MaterialListsea);
                //$scope.SelectedItemsList = $scope.mergedItemsList;
               // $scope.PreviousSelectedItemsList = $scope.mergedItemsList;
            }
            var i=0;
             angular.forEach(MaterialListsea,function(v,k){
                 $scope.SelectedItemsList[serviceHeadID].newObj.push(v);

            });
            if(i > 0 ) AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Selected duplicate items are ingored' });
        }

        $scope.Mataced = function (MaterialListsea, serviceHeadID) {

            var len = $scope.HeadItem.ItemStructureList.length - 1;
            if ($scope.isMatfirsttimeadding[len]) {
                $scope.isMatfirsttimeadding[len] = false;
                var MatList = "SelectedItemsList";
                //$scope.SelectedItemsList = MaterialListsea;
                //$scope.PreviousSelectedItemsList = MaterialListsea;
                $scope.SelectedMatItemsList[len] = {};
                
                $scope.SelectedMatItemsList[len].Matlist = [];
                $scope.wbscodePart2[len] = [];
            }
            
             angular.forEach(MaterialListsea,function(v,k){
                 $scope.SelectedMatItemsList[len].Matlist.push(v);
            });

            $scope.Numbering();

        }

        $scope.Numbering = function(){
            
            var Count = 1;
                for(var i = 1; i < $scope.HeadItem.ItemStructureList.length; i++)
                {
                    $scope.HeadCount[i] = Count;
                    $scope.MatHeadCount[i] = [];
                    Count++;
                //     try
                //     {
                //     if( $scope.SelectedMatItemsList[i].Matlist)
                //     {
                //         for(var j = 0; j< $scope.SelectedMatItemsList[i].Matlist.length; j++)
                //         {
                //         $scope.MatHeadCount[i][j] = Count;
                //         Count++;
                //         }
                //     }
                // }catch(ex){}
                }

        }
        $scope.preventSpcl = function(e){
            var valid = ((e.which == 46) || (e.which >= 48 && e.which <= 57));
            if(!valid){
             e.preventDefault();
            }
           }

        $scope.specList = [];
        var cols = [];
        $scope.materialSpec = function (obj) {
            POUpdateFactoryServices.getMaterialSec(obj).success(function (response) {
                if (response.StatusCode == "200") {
                    $scope.AnnexureList = response.result;
                }
            })
        }
        $scope.basicInfoUpdatePO = function () {
            var data = JSON.stringify($('.basicInformation').serializeObject());
            var validate = JSON.parse(data);
            if (!validate.FundCenterID || validate.FundCenterID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Head is Mandatory' }); return false; }
            else if (!validate.Vendor_OwnerName || validate.Vendor_OwnerName.trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Vendor is Mandatory' }); return false; }
            else if (!validate.POManagerID || validate.POManagerID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Manager is Mandatory' }); return false; }
            else if (!validate.PO_OrderTypeID || validate.PO_OrderTypeID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PO type is Mandatory' }); return false; }
            else if (!validate.PO_Title || validate.PO_Title == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'PO title is Mandatory' }); return false; }
            else if (!validate.PODescription || validate.PODescription == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Description is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                POUpdateFactoryServices.updatePO(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $scope.getPOfullDetailsById();
                        $scope.tab = 2;
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }
        $scope.generalInfoUpdatePO = function () {
            var temp;
            var data = JSON.stringify($('.generalInformation').serializeObject());
            var validate = JSON.parse(data);
            if (!validate.ShippedToID || validate.ShippedToID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Ship To is Mandatory' }); return false; }
            if (validate.CountryCode == 'India') {
                if (validate.ShiptoStateCode == '' || validate.ShipfromStateCode == '') {
                    temp = false; AlertMessages.alertPopup({ errorcode: '1', errormessage: 'State Code and Region Code is Mandatory ' }); return false;
                } else temp = true;
            }
                //else if (!validate.ShippedFromID || validate.ShippedFromID == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Ship From is Mandatory' }); return false; }
            else {
                temp = true;
            }
            if (temp) {
                $rootScope.loading = true;
                POUpdateFactoryServices.updatePO(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $scope.getPOfullDetailsById();
                        $scope.GetPurchaseItems();
                        $scope.tab = 3;
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }
        $scope.SendForApprovalPop = function (poOrderNumber, POUniqueId) {
            $rootScope.PurchaseOrderNumber = poOrderNumber;
            $scope.UserDetails = $localStorage.globals;
            $scope.POUniqueId = POUniqueId;
        };
        $scope.SendForApprovalBtn = function () {
            var data = JSON.stringify($('.SendForApprovalForm').serializeObject());
            var validate = JSON.parse(data);
            if ((validate.SubmitterComments).trim() == '') {
                AlertMessages.alertPopup({
                    errorcode: '1',
                    errormessage: 'Remarks is Mandatory'
                });
                return false;
            } else {
                $rootScope.loading = true;
                POUpdateFactoryServices.submitForApproval(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $('.SendForApprovalForm')[0].reset();
                        $('#SendForApprovalPagePopUp').modal('hide');
                        $scope.hideScreen();
                        $scope.getPOfullDetailsById();
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        };
        $scope.setCurrenSpecTandCData = function (data) {
            $scope.CurrenSpecTandCData = data;
        }
        $scope.setBrandData = function (brandInfo) {
            $scope.brandData = JSON.parse(brandInfo);
        }
        $scope.setCurrenSpecificTCData = function (data) {
            $scope.getpoSpecTCSubTitles(data.SpecificTCTitleMasterId);
            data.SpecificTCTitleMasterId = data.SpecificTCTitleMasterId.toString();
            data.SpecificTCSubTitleMasterId = data.SpecificTCSubTitleMasterId.toString();
            $scope.CurrenSpecificTCData = data;
        }
        $scope.setTermConditionData = function (data) {
            $scope.TermConditionData = data;
        }
        $scope.setLinkedPOData = function (unique) {
            $scope.LinkedPOID = unique;
        }
        $scope.getAllPOSpecificTC = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getAllPOSpecificTC({ 'POHeaderStructureId': $scope.PO_HSID }).success(function (response) {
                $rootScope.loading = false;
                $scope.POSpecTandCList = response.result;
            });
        }
        $scope.getAllPOSpecificTC();
        $scope.getPOSpecificTCById = function (specId) {
            $rootScope.loading = true;
            POUpdateFactoryServices.getPOSpecificTCById({ 'SpecificTCId': specId, 'LastModifiedBy': $scope.UserDetails.currentUser.UserId }).success(function (response) {
                $rootScope.loading = false;
                $scope.POSpecificTCData = response.result;
            });
        }
        $scope.getPOSpecialTandC = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getSpecialTandC({ 'POUniqueID': $scope.PO_HSID }).success(function (response) {
                $rootScope.loading = false;
                $scope.SpecialTandCList = response.result;
            });
        }
        $scope.getPOSpecialTandC();
        $scope.createNewSpecialTandC = function () {
            var data = JSON.stringify($('.specialTandCform').serializeObject());
            var validate = JSON.parse(data);
            if (!validate.Title || (validate.Title).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Title is Mandatory' }); return false; }
            else if (!validate.SequenceId || (validate.SequenceId).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Sequence No is Mandatory' }); return false; }
            else if (!validate.Condition || (validate.Condition).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Special Condition is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                POUpdateFactoryServices.saveupdateSpecialTandC(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $('.specialTandCform')[0].reset();
                        $scope.Condition = "";
                        $('#AddNewSpecialTandCPopup').modal('hide');
                        $scope.getPOSpecialTandC();
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }
        $scope.updateSpecialTandC = function () {
            var data = JSON.stringify($('.editspecTandCform').serializeObject());
            var validate = JSON.parse(data);
            if (!validate.Title || (validate.Title).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Title is Mandatory' }); return false; }
            else if (!validate.SequenceId || (validate.SequenceId).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Sequence No is Mandatory' }); return false; }
            else if (!validate.Condition || (validate.Condition).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Special Condition is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                POUpdateFactoryServices.saveupdateSpecialTandC(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $('.editspecTandCform')[0].reset();
                        $('#editSpecialTandCPopup').modal('hide');
                        $scope.getPOSpecialTandC();
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }
        $scope.deleteSpecTandC = function () {
            var data = JSON.stringify($('.deleteForm').serializeObject());
            POUpdateFactoryServices.deleteSpecialTandC(data).success(function (response) {
                if (response.info.errorcode == '0') { $('#DeleteSpecPopUp').modal('hide'); $scope.getPOSpecialTandC(); }
                AlertMessages.alertPopup(response.info);
                $scope.GetGridList();
            });
        }
        $scope.deleteSpecificTCTandC = function () {
            var data = JSON.stringify($('.specificTCdeleteForm ').serializeObject());
            POUpdateFactoryServices.deletePOSpecificTC(data).success(function (response) {
                if (response.info.errorcode == '0') { $('#DeleteSpecificTCPopUp').modal('hide'); $scope.getPOSpecialTandC(); }
                AlertMessages.alertPopup(response.info);
                $scope.getAllPOSpecificTC();
            });
        }
        $scope.getPOpaymentterms = function () {
            $rootScope.loading = true;
            POUpdateFactoryServices.getPOPamentTerms({ 'UniqueId': $scope.PO_HSID }).success(function (response) {
                $scope.SumofExistPaymentTermAmount = 0;
                $rootScope.loading = false;
                $scope.PaymentTermList = response.result;
                if ($scope.PaymentTermList != null) {
                    for (var i = 0; i < $scope.PaymentTermList.length; i++) {
                        $scope.SumofExistPaymentTermAmount += $scope.PaymentTermList[i].Amount;
                        if ($scope.PaymentTermList[i].Date !== null && $scope.PaymentTermList[i].Date !== undefined) $scope.PaymentTermList[i].Date = $filter('date')($scope.PaymentTermList[i].Date, "yyyy-MM-dd");
                    }
                }
                setTimeout(()=>{$scope.SettheBalance();},300);
            });
        }
        $scope.createSpecificTandC = function () {
            var data = JSON.stringify($('.specificTandCform').serializeObject());
            var validate = JSON.parse(data);
            if (!validate.SpecificTCTitleMasterId || (validate.SpecificTCTitleMasterId).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Title is Mandatory' }); return false; }
            else if (!validate.SpecificTCSubTitleMasterId || (validate.SpecificTCSubTitleMasterId).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Sub Title is Mandatory' }); return false; }
            else if (!validate.Description || (validate.Description).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Condition is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                POUpdateFactoryServices.savePOSpecificTC(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $('.specificTandCform')[0].reset();
                        $scope.Description = "";
                        //document.forms["specificTandCform"]["Description"].value;
                        document.forms["specificTandCform"]["Description"].value = null;
                        $('#AddSpecificTCPopup').modal('hide');
                        $scope.getAllPOSpecificTC();
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }
        $scope.updateSpecificTandC = function () {
            var data = JSON.stringify($('.editspecificTandCform').serializeObject());
            var validate = JSON.parse(data);
            if (!validate.SpecificTCTitleMasterId || (validate.SpecificTCTitleMasterId).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Title is Mandatory' }); return false; }
            else if (!validate.SpecificTCSubTitleMasterId || (validate.SpecificTCSubTitleMasterId).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Sub Title is Mandatory' }); return false; }
            else if (!validate.Description || (validate.Description).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Condition is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                POUpdateFactoryServices.updatePOSpecificTC(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $('.editspecificTandCform')[0].reset();
                        $scope.Description = "";
                        //document.forms["specificTandCform"]["Description"].value;
                        document.forms["editspecificTandCform"]["Description"].value = null;
                        $('#editSpecificTCPopup').modal('hide');
                        $scope.getAllPOSpecificTC();
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }
        $scope.getPOpaymentterms();

        $scope.UpdateCurrentPurchaseItem = function (ItemStructID, name, description, code, hsn, WBSElementCode, Brand, Qty, Rate,
            IGSTRate, SGSTRate, CGSTRate, IGSTAmount, SGSTAmount, CGSTAmount, TotalTaxAmount, GrossAmount) {

            if (!WBSElementCode || WBSElementCode == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Element is Mandatory' }); return false; }
            // else if (!InternalOrderNumber || InternalOrderNumber == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Internal Order is Mandatory' }); return false; }
            // else if (!GLAccountNo || GLAccountNo == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GL Account is Mandatory' }); return false; }
            //  else if (!Brand || Brand == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Brand is Mandatory' }); return false; }
            else if (!Qty || Qty == '' || Qty <= 0) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity is Mandatory' }); return false; }
            else if (!Rate || Rate == '' || Rate <= 0) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Rate is Mandatory' }); return false; }
            else {
                $rootScope.loading = true;
                POUpdateFactoryServices.updatePurchaseItem(
                    {
                        'Material_Number': code,
                        'Short_Text': name,
                        'Long_Text': description,
                        'HSN_Code': hsn,
                        'HeaderStructureID': $scope.PO_HSID,
                        'ItemStructureID': ItemStructID,
                        'WBSElementCode': WBSElementCode,
                        'Brand': Brand,
                        'Rate': Rate,
                        'Order_Qty': Qty,
                        'IGSTRate': IGSTRate,
                        'CGSTRate': CGSTRate,
                        'SGSTRate': SGSTRate,
                        'TotalTaxAmount': TotalTaxAmount,
                        'GrossAmount': GrossAmount,
                        'LastModifiedBy': $scope.UserDetails.currentUser.UserId
                    }).success(function (response) {
                        $rootScope.loading = false;
                        if (response.info.errorcode == '0') {
                            $scope.ClickedIndexValue = 1000;
                            $scope.GetPurchaseItems();
                            $scope.tab = 3;
                        }
                        AlertMessages.alertPopup(response.info);
                    });
            }
        }
        $scope.deleteCurrentPurchaseItem = function (ItemStructID) {
            $rootScope.loading = true;
           // alert(ItemStructID);
            POUpdateFactoryServices.DeletePurchaseItem(
                {
                    'HeaderStructureID': $scope.PO_HSID,
                    'ItemStructureID': ItemStructID,
                    'LastModifiedBy': $scope.UserDetails.currentUser.UserId
                }).success(function (response) {
                    $rootScope.loading = false;
                    if (response.info.errorcode == '0') {
                        $scope.ClickedIndexValue = 1000;
                        $scope.GetPurchaseItems();
                        $scope.getPOTechnicalSpecs();
                        $scope.getPOAnnexSpecs();
                        $scope.getPOServiceAnnexSpecs();
                        // $state.reload();
                        $scope.tab = 3;
                    }
                    AlertMessages.alertPopup(response.info);
                });
        }
        $scope.MultipleSavePurchaseItem = function () {
            var itemcount = $('input[name="Order_Qty"]').length;
            var data = JSON.stringify($('.purchaseItemform').serializeObject());
            var validate = JSON.parse(data);
            
            for(var i = 0; i < $sessionStorage.ServiceHeadID.length; i++)
            {
                console.log($scope.HeadItem.ItemStructureList[i].Itemstructure[0].IGSTRate);
            }


            if (itemcount == 1) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Add any new Item to Save' }); return false; }
            else if ($scope.poCode == "ZEX") {
                for (var cnt = 1; cnt < itemcount; cnt++) {
                    // (!validate.Short_Text[cnt] || validate.Short_Text[cnt] == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Material Name is Mandatory' }); return false; }
                    // else if (!validate.Long_Text[cnt] || validate.Long_Text[cnt] == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Description is Mandatory' }); return false; }
                    if (!validate.HSN_Code[cnt] || validate.HSN_Code[cnt] == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'HSN/SAC is Mandatory' }); return false; }
                    else if (!validate.Order_Qty[cnt] || validate.Order_Qty[cnt] == '' || validate.Order_Qty[cnt] <= 0) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity is Mandatory' }); return false; }
                    else if (!validate.Rate[cnt] || validate.Rate[cnt] == '' || validate.Rate[cnt] <= 0) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Rate is Mandatory' }); return false; }
                }
                $rootScope.loading = true;
                POUpdateFactoryServices.saveExpenseOrderstems(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $scope.SelectedItemsList = [];
                        $scope.MainMaterialElementList = [];
                        $scope.MainExpenseElementList = [];

                        $scope.GetPurchaseItems();
                        // $scope.getPOTechnicalSpecs();
                        // $scope.getPOAnnexSpecs();
                        // $scope.getPOServiceAnnexSpecs();
                        $scope.tab = 4;
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
            else {
                for (var cnt = 1; cnt < itemcount; cnt++) {
                    if (!validate.WBSElementCode[cnt] || validate.WBSElementCode[cnt] == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'WBS Element is Mandatory' }); return false; }
                    // else if (!validate.InternalOrderNumber[cnt] || validate.InternalOrderNumber[cnt] == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Internal Order is Mandatory' }); return false; }
                    //else if (!validate.GLAccountNo[cnt] || validate.GLAccountNo[cnt] == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'GL Account is Mandatory' }); return false; }
                    // else if (!validate.Brand[cnt] || validate.Brand[cnt] == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Brand is Mandatory' }); return false; }
                    else if (!validate.Order_Qty[cnt] || validate.Order_Qty[cnt] == '' || validate.Order_Qty[cnt] <= 0) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Quantity is Mandatory' }); return false; }
                    else if (!validate.Rate[cnt] || validate.Rate[cnt] == '' || validate.Rate[cnt] <= 0) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Rate is Mandatory' }); return false; }
                }
                $rootScope.loading = true;
                POUpdateFactoryServices.multiplepurchaseSave(data).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $('.purchaseItemform')[0].reset();
                        $scope.SelectedItemsList = [];
                        $scope.PreviousSelectedItemsList = [];
                        $scope.MainMaterialElementList = [];
                        $scope.MainExpenseElementList = [];
                        $scope.SelectedMatItemsList = [];
                        $scope.GetPurchaseItems();
                        $scope.getPOTechnicalSpecs();
                        $scope.getPOAnnexSpecs();
                        $scope.getPOServiceAnnexSpecs();
                        //  $state.reload();
                        $scope.tab = 4;
                    }
                    AlertMessages.alertPopup(response.info);
                    $rootScope.loading = false;
                });
            }
        }

        $scope.MainMaterialElementList = [];

        $scope.addMainMaterialElement = function () { $scope.MainMaterialElementList.push({ 'id': 'choice' }); };
        $scope.RemoveMainMaterialElement = function (index) { $scope.MainMaterialElementList.splice(index, 1); };

        $scope.MainExpenseElementList = [];
        $scope.addMainExpenseElement = function () { $scope.MainExpenseElementList.push({ 'id': 'choice', 'CGSTTaxRate': 0, 'IGSTTaxRate':0,'SGSTTaxRate':0 }); };
        $scope.RemoveMainExpenseElement = function (index) { $scope.MainExpenseElementList.splice(index, 1); };


        //Adding and Removing PaymentTerms Starts
        $scope.NewPTList=[];$scope.ActualSum=0;
        $scope.addNewElement=function(){$scope.NewPTList.push({'id':'0'});};
        $scope.RemoveElementList=function(index){$scope.NewPTList.splice(index,1);};
        $scope.SettheBalance=function(){
            $scope.BalanceAmt=0; $scope.ActualSum=0;
            var data = JSON.stringify($('.paymenttermsaveform').serializeObject());
            var validate = JSON.parse(data);
            for(var i=0;i<validate.Amount.length;i++){
                if(validate.Amount[i]==null || validate.Amount[i]=="")validate.Amount[i]=0;
                var CAmt=validate.Amount[i];
                CAmt=$filter('numnoneFlot')(CAmt);
                $scope.ActualSum+=CAmt;
            }
            console.log($scope.ActualSum);
            if($scope.ActualSum>$scope.TotalAmount){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Amount Exceeded' });}
            $scope.BalanceAmt=$scope.TotalAmount-$scope.ActualSum;
        }
        $scope.BalValidate=function(a){
            var Vsum=0;
            for(var i=0;i<a.length;i++){
                if(a[i]==null || a[i]=="")a[i]="0";
                var CAmt=a[i];
                Vsum+=parseFloat(CAmt);
            }
            if(Vsum>$scope.TotalAmount){return false;}
            else return true;
        }
        $scope.saveupdatepaymentTerm = function (ptid, pterm, percent, amt, date, remrk) {
            var data = JSON.stringify($('.paymenttermsaveform').serializeObject());
            var validate = JSON.parse(data);
            if((validate.PaymentTerm).length != getArrayCounts(validate.PaymentTerm)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Payment Term is Mandatory'});return false;}
            else if((validate.Amount).length != getArrayCounts(validate.Amount)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Amount is Mandatory'});return false;}
            else if((validate.Percentage).length != getArrayCounts(validate.Percentage)){AlertMessages.alertPopup({errorcode: '1',errormessage: 'Percentage is Mandatory'});return false;}
            else if(!$scope.BalValidate(validate.Amount)){AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Amount Exceeded' });}
            else {
                var finalData=[];
                for(var i=0;i<validate.PaymentTerm.length;i++){
                    if(validate.PaymentTerm[i]!='0'){
                        finalData.push({"UniqueId":validate.UniqueId[i],"ContextIdentifier":validate.ContextIdentifier[i],"Remarks":validate.Remarks[i],"PaymentTerm":validate.PaymentTerm[i],"Amount":validate.Amount[i],"Percentage":validate.Percentage[i],"Date":validate.Date[i]});
                    }
                }
                $rootScope.loading = true;
                POUpdateFactoryServices.saveupdatePoPaymentTerm(finalData).success(function (response) {
                    if (response.info.errorcode == '0') {
                        $scope.NewPTList=[];
                        $scope.getPOpaymentterms();
                        $scope.ClickedIndexValue = 1000;
                    }
                    AlertMessages.alertPopup(response.info);
                });
            }
        }
        $scope.deletepaymentTerm = function (ptid) {
            $rootScope.loading = true;
            POUpdateFactoryServices.DeletePaymentTerm({ 'UniqueId': ptid, 'LastModifiedBy': $scope.UserDetails.currentUser.UserId }).success(function (response) {
                if (response.info.errorcode == '0') {
                    $scope.getPOpaymentterms();
                }
                AlertMessages.alertPopup(response.info);
            });
        }
        //Adding and Removing PaymentTerms Ends
    })
    .filter('displayDate', function ($filter) {
        return function (time) {
            if (!time) return "";
            else {
                time = new Date(time);
                return $filter('date')(time, 'dd-MMM-yyyy');
            }
        }
    })
    .directive("paymenttermupdate", function () {
        return {
            restrict: 'A',
            template: '<tr ng-repeat="payterm in PaymentTermList" class="removeablediv">\
                        <th scope="row" width="3%" class="vam">{{$index+1}}</th>\
                        <td><input class="form-control" name="PaymentTerm" ng-modal="payterm.PaymentTerm" value="{{payterm.PaymentTerm}}" placeholder="Payment Term"/></td>\
                        <td><input class="form-control" name="Percentage" ng-modal="payterm.Percentage" value="{{payterm.Percentage}}" placeholder="Percentage"/></td>\
                        <td><input class="form-control" name="Amount" ng-modal="payterm.Amount" value="{{payterm.Amount}}" placeholder="Amount"/></td>\
                        <td><input class="form-control" name="Date" ng-modal="payterm.Date"  value="{{payterm.Date}}" type="date"/></td>\
                        <td><input class="form-control" name="Remarks" ng-modal="payterm.Remarks" value="{{payterm.Remarks}}" placeholder="Remarks"/></td>\
                        <td><a ng-click="updatepaymentTerm(payterm);"><span class="glyphicon glyphicon-floppy-disk" aria-hidden="true" title="Save Changes" style="font-size:12pt;"></span></a></td>\
                        <td><a ng-click="deletepaymentTerm(payterm);"><span class="glyphicon glyphicon-trash" aria-hidden="true" title="Delete" style="font-size:12pt;"></span></a></td>\
                </tr>'
        };
    })



    .directive("addmainmaterialelement", function () {
        return {
            restrict: 'A',
            template: '<tr ng-repeat-start="(key,subitem) in MainMaterialElementList" class="removeablediv">\
        <td style="text-align:right">\
            <input type="text" name="ItemType"  value="ExpenseOrder"  hidden="true" />\
            {{key+ExistpurcItemCount+1}}</td>\
            <td></td>\
            <td ng-show="false"><input type="text" name="Short_Text" class="form-control"  placeholder="Name"/></td>\
            <td><input type="text" name="Material_Number" class="form-control"  placeholder="Material code"/></td>\
            <td><input type="text" name="Long_Text" class="form-control"  placeholder="Material Description"/></td>\
            <td class="text-center">-</td>\
            <td><input type="text" name="HSN_Code" class="form-control"  placeholder="HSN/SAC"/></td>\
            <td>\
            <div class="input-group input-group-sm">\
                <input type="text" class="form-control" name="WBSElementCode" ng-model="wbscodePart1[key]" placeholder="Select WBS Part1" ng-readonly="true" />\
                <div class="input-group-addon" data-toggle="modal" data-target="#WBSDetailsPart1Popup" ng-click="setrowclickedKeyMain(key,false);getWBSDetailsByReset();"><i class="fa fa-search"></i></div>\
            </div>\
            </td>\
            <td><input type="text" class="form-control decimalPrecision2 intvalid Qty" name="Order_Qty" ng-model="Qty" ng-change="getTotalAmountTaxGross()" placeholder="Quantity" ng-init="Qty=0"/></td>\
            <td class="text-center">-</td>\
            <td style="text-align:right">\
            <input  type="text" class="form-control decimalPrecision2 intvalid Rate" name="Rate" ng-model="Rate" ng-change="getTotalAmountTaxGross()" ng-init="Rate=0"/>\
            </td>\
            <td style="text-align:right">{{(Qty * Rate)|numnoneFlot}}</td>\
            <td class="text-center">-</td>\
            <td class="text-center">-</td>\
            <td class="text-center">-</td>\
            <td class="text-center">-</td>\
            <td class="text-center">-</td>\
            <td>\
            <a ng-click="RemoveMainMaterialElement(key);" title="Remove">\
            <span class="glyphicon glyphicon-trash" aria-hidden="true" title="Delete" style="font-size:12pt;"></span>\
            </a>\
            </td>\
        </tr><tr ng-repeat-end></tr><tr ng-repeat-end></tr>'
        };
    })
    .directive("limitTo", function() {
        return {
            restrict: "A",
            link: function(scope, elem, attrs) {
                var limit = parseInt(attrs.limitTo);
                angular.element(elem).on("keypress", function(e) {
                    if (this.value.length == limit) e.preventDefault();
                });
            }
        };
    })

.directive("addmainexpenselement", function () {
    return {
        restrict: 'A',
        template: '<tr ng-repeat="(key,subitem) in MainExpenseElementList" class="removeablediv">\
        <td style="text-align:right">\
            <input type="text" name="ItemType"  value="ExpenseOrder"  hidden="true" />\
            {{key+ExistpurcItemCount+1}}\
            <input type="hidden" class="form-control" name="Tax_salespurchases_code" ng-model="subitem.TaxCode"/>\
            </td>\
            <td></td>\
            <td ng-show="false"><input type="text" name="Short_Text" class="form-control"  placeholder="Name"/></td>\
            <td><input type="text" name="Long_Text" class="form-control"  placeholder="Expense Description"/></td>\
            <td>\
            <div class="input-group input-group-sm">\
                <input type="text" class="form-control" name="HSN_Code" ng-model="subitem.HSNCode" placeholder="Select HSN/SAC" ng-readonly="true" />\
                <div class="input-group-addon" data-toggle="modal" data-target="#HSNDetailsPart1Popup" ng-click="getHSNDetailsByReset(key);"><i class="fa fa-search"></i></div>\
            </div>\
            </td>\
            <td>\
            <div class="input-group input-group-sm" style="width:200px;">\
                <input type="text" class="form-control" name="WBSElementCode" ng-model="wbscodePart1[key]" placeholder="Select WBS Part1" ng-readonly="true" />\
                <div class="input-group-addon" data-toggle="modal" data-target="#WBSDetailsPart1Popup" ng-click="getWBSDetailsByReset();setrowclickedKeyMain(key,false);"><i class="fa fa-search"></i></div>\
            </div>\
            </td>\
            <td><input type="text" class="form-control decimalPrecision2 intvalid Qty" ng-keypress="preventSpcl($event)" name="Order_Qty" ng-model="Qty" ng-change="getTotalAmountTaxGross()" placeholder="Quantity" ng-init="Qty=1"/></td>\
            <td><input type="text" name="Unit_Measure" class="form-control" value="Lumpsum" ng-readonly="true"/></td>\
            <td style="text-align:right">\
            <input  type="text" class="form-control decimalPrecision2 intvalid Rate" ng-keypress="preventSpcl($event)" name="Rate" ng-model="Rate" ng-change="getTotalAmountTaxGross()" ng-init="Rate=0"/>\
            </td>\
            <td style="text-align:right"><input type="text" name="TotalAmount" value="{{(Qty * Rate)|numnoneFlot}}" ng-hide="true" />{{Qty * Rate}}</td>\
            <td class="text-right"><input type="text" name="IGSTRate" ng-model="IGSTTaxRate[key]" value="{{IGSTTaxRate[key]}}" ng-hide="true" />{{IGSTTaxRate[key]}}</td>\
            <td class="text-right"><input type="text" name="SGSTRate" ng-model="SGSTTaxRate[key]" value="{{SGSTTaxRate[key]}}" ng-hide="true" />{{SGSTTaxRate[key]}}</td>\
            <td class="text-right"><input type="text" name="CGSTRate" ng-model="SGSTTaxRate[key]" value="{{CGSTTaxRate[key]}}" ng-hide="true" />{{CGSTTaxRate[key]}}</td>\
            <td class="text-right"><input type="text" name="TotalTaxAmount" ng-model="((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty* Rate))/100 ? (((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty*Rate))/100|number:2):0" value="{{((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty* Rate))/100 ? (((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty*Rate))/100|number:2):0}}" ng-hide="true" />{{((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty* Rate))/100 ? (((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty*Rate))/100|number:2):0}}</td>\
            <td class="text-right"><input type="text" name="GrossAmount" ng-model="(Qty* Rate)+((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty* Rate))/100 ? ((Qty* Rate)+((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty* Rate))/100 |number:2):0" value="{{(Qty* Rate)+((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty* Rate))/100 ? ((Qty* Rate)+((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty* Rate))/100 |number:2):0}}" ng-hide="true" />{{(Qty* Rate)+((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty* Rate))/100 ? ((Qty* Rate)+((IGSTTaxRate[key]+SGSTTaxRate[key]+CGSTTaxRate[key])*(Qty* Rate))/100 |number:2):0}}</td>\
            <td>\
            <a ng-click="RemoveMainExpenseElement(key);" title="Remove"><span class="glyphicon glyphicon-trash" aria-hidden="true" title="Delete" style="font-size:12pt;"></span></a>\
            </td>\
        </tr>'
    };
})
function getDecVal(a){
    a=a.toString();
    var res = a.split(".");
    var v4=res[0];
    var v5=res[1];
    if(!v5)v5="00";
    v5=v5.substring(0,2);
    var FinalVal=v4+"."+v5;
    return FinalVal;
}