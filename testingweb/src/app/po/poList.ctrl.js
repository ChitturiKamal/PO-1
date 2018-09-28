(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('POListController', POListController)
        .directive('whenScrollEnds', whenScrollEnds);
    

    /** @ngInject */
    function POListController(POServices, $mdDialog, $scope, $window, notificationService, commonUtilService, $stateParams, $rootScope,$filter,$location,$state) {
        var vm = this;

        vm.showTab = function(st){
            alert();
        }
        var userInfo = commonUtilService.getProfile();

        var userId = userInfo.userId;
        var emailId = userInfo.email;
        var loginUserName=userInfo.userName;

        var fuguePONumber = "TESUPPORT&TESUPPORT";
        var aproverUniqueid;
        var aproverName;
        var sequenceId;
        var releaseCode;

        var status = "All";
        var sortBy = "PoNumber"
        var pageIndex = 1;
        var isNextCallAllowed = true;
        var isFilter = false;

        vm.poList = [];
        vm.poDetails=[];
        vm.poMilestoneList = [];
        vm.poTermsList=[];
        vm.poSpecTermsList=[];
         $scope.draftClass = [];
         vm.poApproverList=[];


        vm.dateFormate = "dd-MMM-yy";
        vm.selectedPONumber = "";
        
        vm.selectedPOId = ""; // added by sai.k
        vm.WbsHeads=""; // added by sai.k
        vm.UpdateType=""; // added by sai.k
        vm.dfAmt=0;// added by sai.k
        vm.dfPer=0;// added by sai.k
        vm.modal=false;// added by sai.k
        vm.isGenAvail=false; // added by sai.k
        vm.isSpecAvail=false; // added by sai.k
        vm.termType=""; // added by sai.k
        vm.SpecificTitle=""; // added by sai.k
        vm.SpecificCondition=""; // added by sai.k
        vm.termType=""; // added by sai.k
        vm.SpecificTypeId="";  // added by sai.k
        vm.termDate="";  // added by sai.k
         vm.comments="";  // added by sai.k
         vm.Aprcomments="";  // added by sai.k
        vm.isApprover = false;
        vm.isSubmitter=true;
        vm.isWithdraw=false;
        vm.isPOdetail=false;
        vm.isdraft=true;
        vm.ApprAvail=false;
        vm.searchText="";
       
        vm.isNoDataPresent = false;
        vm.isNewSearchText = false;

        vm.statusIcon = {
            "Approved": "fa-check-circle status-approve",
            "Pending for Approval": "fa-exclamation-circle status-pending",
            "Inqueue": "fa-exclamation-circle status-inqueue",
            "Rejected": "fa-ban status-reject"
        };

        vm.loadMoreRecords = loadMoreRecords;
        vm.getSelectedPO = getSelectedPO;
        vm.showItemDetails = showItemDetails;
        vm.poAction = poAction;
        vm.sendEmail = sendEmail;
        vm.searchPO = searchPO;
         vm.IsMilestoneSave=false;
         vm.IsMilestoneAvail=false;
         vm.IsSpecSave=true;
         vm.POServiceCondition="";
         vm.POServiceType="";
         vm.IsPOSubmited=false;
         vm.GSTDate="";
         vm.IsAnnexurePrint=false;
         // $scope.StorageLocation=["Emil", "Tobias", "Linus"];
          function getStorageLocation() {
            var data = {
                Code:vm.poDetails.PlantRegionCode
            };
            POServices.callServices("GETAllStorageDetails",data).then(
                function(response) {
                        $scope.StorageLocation= response.data;
                }
            );
        }
        $scope.updateShipTo = function(selectedLocation){
            vm.poDetails.ShipToCode = selectedLocation.StorageLocationCode;
            vm.poDetails.ShipToName=selectedLocation.ProjectName;
            vm.poDetails.ShipToAddress=selectedLocation.Address;
            vm.poDetails.ShipToCountry=selectedLocation.CountryCode;
            vm.poDetails.ShipToGSTCode=selectedLocation.GSTIN;
         };
         $scope.updatepacking = function(packing,item){
            item.PackingForwardingCondition=packing.ChildDescription;
         };
         $scope.updateFreight = function(Freight,item){
            item.FreightCondition=Freight.ChildDescription;
         };
         $scope.updateOtherServices = function(OtherServices,item){
            item.OtherServicesCondition=OtherServices.ChildDescription;
         };
         $scope.updateServiceType = function(ServiceType,item){
            item.OtherServicesType=ServiceType.ChildDescription;
         };
          //$('.pdftable').addClass('watermark-img');
        activate();
      
        function activate() {
            status = ($stateParams && $stateParams.status) ? $stateParams.status : "All";
            if(status=="Finalize")
            { 

                   vm.isWithdraw=true;
                   console.log("Finalize");
                  getFinalizeList();  
            }                    
            else
            { 

                console.log("all");
                 getPoList();
            }
           
        }
        function getFinalizeList(){

           var data = {
                UserId: userId,                
                PageCount: pageIndex,
                FilterBy:vm.searchText
               
            }
           POServices.callServices("getFinalizeList", data).then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        isNextCallAllowed = true;

                        vm.poList = response.data;
                       // vm.poList = vm.poList.concat(getStatus(response.data));
                        
                        // if (pageIndex == 1) {
                        //     vm.selectedPONumber = vm.poList[0].Purchasing_Order_Number
                        //     getSelectedPO();
                        // }

                    }
                    vm.isNoDataPresent = (vm.poList.length > 0) ? false : true;
                }
            );
        }


        function getPoList() {
            var data = {
                UserId: userId,
                Status: status,
                PageCount: pageIndex,
                SortBy: sortBy
            }
            POServices.callServices("getPOList", data).then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        isNextCallAllowed = true;
                        vm.poList = vm.poList.concat(getStatus(response.data));
                        
                        /** commented by sai **/
                        // if (pageIndex == 1) {
                        //     vm.selectedPONumber = vm.poList[0].Purchasing_Order_Number
                        //     getSelectedPO();
                        // }
                        getStorageLocation();
                    }
                    vm.isNoDataPresent = (vm.poList.length > 0) ? false : true;
                }
            );
        }

        function getStatus(poList) {
            angular.forEach(poList, function(value) {

                console.log(userInfo.isPOAdmin);
                if (userInfo.isPOAdmin)
                    value.ReleaseCodeStatus = value.POStatus;
                else
                    value.ReleaseCodeStatus = $window._.pluck($window._.filter(value.Approvers, { 'ApproverId': userId }), 'Status')[0];
                value.ReleaseCodeStatus = value.ReleaseCodeStatus == 'Draft' ? 'Inqueue' : value.ReleaseCodeStatus;
                value.pendingApprovers = $window._.pluck($window._.filter(value.Approvers, function(value) {
                    return value.Status == 'Pending for Approval' || value.Status == 'Draft'
                }), 'ApproverName').join('\n');
            });
            return poList;
        }

        function loadMoreRecords() {
            if (isNextCallAllowed) {
                isNextCallAllowed = false;
                pageIndex++;
                if (isFilter)
                    getSearchPoList();
                else
                {
                     status = ($stateParams && $stateParams.status) ? $stateParams.status : "All";
                    if(status=="Finalize")
                    { 
                        
                          getFinalizeList();  
                    }                    
                    else
                    {
                         getPoList();
                    }
                           
                 }
            }
        }

        function poAction(action) {
            if (action) {
                var actionType;
                if (action == 'approve')
                    actionType = 'Approved';
                else
                    actionType = 'Rejected';

                if(!vm.Aprcomments)
                {
                     if(actionType=='Rejected')
                     {
                        notificationService.error('Please enter rejection comments');
                     }  
                     else
                     {
                         poApproveReject(action,actionType);
                     }
                }
                else
                {
                   poApproveReject(action,actionType);
                }
            }
                             
                
            
        }

function poApproveReject(action,actionType)
{
var confirm = $mdDialog.confirm()
                    .title('Confirmation Needed')
                    .textContent('Are you sure? You want to ' + action + ' PO')
                    .ok('Ok')
                    .cancel('cancel');

                $mdDialog.show(confirm).then(function() {
                    var data = {
                        UserId: userId,
                        Purchasing_Order_Number: vm.selectedPONumber,
                        Fugue_Purchasing_Order_Number: fuguePONumber,
                        AproverUniqueid: aproverUniqueid,
                        AproverName: aproverName,
                        SequenceId: sequenceId,
                        ReleaseCode: releaseCode,
                        ReleaseCodeStatus: actionType,
                        Comments:vm.Aprcomments,
                        POUniqueId:vm.selectedPOId
                    };

                    POServices.callServices("poApproveReject", data).then(
                        function(response) {
                            if (response.data) {
                                if (response.data[0].ReleaseCodeStatus == 'Success') {
                                    var isRefresh = false;
                                    notificationService.success('PO ' + actionType + ' successfully');
                                    $rootScope.reload();
                                    
                                    /** commented by sai ****/
                                    if (status == 'All') {
                                        isRefresh = true;
                                    } else {
                                        var index = $window._.findIndex(vm.poList, { 'Purchasing_Order_Number': vm.selectedPONumber });
                                        //vm.selectedPONumber = vm.poList[index + 1].Purchasing_Order_Number;
                                        vm.poList.splice(index, 1);
                                    }
                                    //getSelectedPO(isRefresh);
                                    //$stateParams.status = "Pending for Approval";
                                    //vm.searchText="";
                                   //$state.go('main.poList',{status:'Pending for Approval'});
                                 

                                  // $window.location.reload();
                                   //vm.poList.splice(index, 1);
                                   vm.searchText="";
                                   vm.isPOdetail=false;
                                    getPoList();
                                } else {
                                    notificationService.error('failed to ' + action + ' PO, please contact fugue support');
                                }
                            } else {
                                notificationService.error('failed to ' + action + ' PO, please contact fugue support');
                            }
                        }
                    );
                }, function() {

                });


}

        function updatePOGrid() {
            if (vm.selectedPONumber) {
                var index = $window._.findIndex(vm.poList, { 'Purchasing_Order_Number': vm.selectedPONumber });
                vm.poList[index].Approvers = vm.poApproverList;
                if (index) {
                    vm.poList[index] = getStatus([vm.poList[index]])[0];
                }
            }
        }

        function searchPO() {
            pageIndex = 1;
            isFilter = true;
            isNextCallAllowed = true;
            vm.isApprover = false;
            vm.poList = [];
            vm.selectedPONumber = "";
            if (vm.searchText) {
            status = ($stateParams && $stateParams.status) ? $stateParams.status : "All";
                if(status=="Finalize")
                { 
                    
                      getFinalizeList();  
                }                    
                else
                {
                     getSearchPoList();
                }

               
                vm.isNewSearchText = true
            } else {
                isFilter = false;
                vm.isNewSearchText = false;
                 if(status=="Finalize")
                { 
                    
                      getFinalizeList();  
                }                    
                else
                {
                     getPoList();
                }
              
            }
        }

        function getSearchPoList() {
            var data = {
                UserId: userId,
                Status: status,
                PageCount: pageIndex,
                FilterBy: vm.searchText
            }
            POServices.callServices("getPOSearch", data).then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        isNextCallAllowed = true;
                        vm.poList = vm.poList.concat(getStatus(response.data));
                        if (pageIndex == 1) {
                            vm.selectedPONumber = vm.poList[0].Purchasing_Order_Number
                            getSelectedPO();
                        }
                    }
                    if (vm.poList.length > 0)
                        vm.isNoDataPresent = false;
                    else
                        vm.isNoDataPresent = true;
                }
            );
        }

        function sendEmail(id) {
            if (emailId) {
                if (id) {
                    var data = {
                        Purchasing_Order_Number: id,
                        UserId: userId
                    };
                    POServices.callServices("getPOById", data).then(
                        function(response) {
                            if (response.data && response.data.length > 0) {
                                var url = commonUtilService.getUrl("TESAPPurchaseService");
                                var poDetails = response.data[0];
                                var body = poDetails.DocumentBody;
                                var Path = url + poDetails.Path.substring(poDetails.Path.lastIndexOf("\\") + 1);
                                body = body.replace("$Employee", userInfo.name);
                                body = body.replace("$PdfUrl", Path);
                                var data = {
                                    Subject: "PO Document:" + id,
                                    From: "",
                                    To: emailId,
                                    Html: body,
                                    SenderType: "Individual"
                                };
                                POServices.callServices("sendMail", data).then(
                                    function(response) {
                                        if (response.data && response.data.StatusDescription) {
                                            notificationService.success('Mail send successfully');
                                        } else {
                                            notificationService.error('Sending mail failed');
                                        }
                                    }
                                );
                            }
                        }
                    );
                }
            } else {
                notificationService.info('No email id present for logged-in user');
            }
        }

        function getSelectedPO(isRefresh) {
            vm.isApprover = false;
            if (vm.selectedPONumber) {
                getPODetails();
                //getPOItemList();
                getPickList();
               // getPOMilestones();
                getPOGenTerms();
                getPOSpecificTerms();
                getPOApproverList(isRefresh);
               // getTermAndConditions()

               getPOItemAnnexureList();
            }
        }

        function getPODetails() {
            var data = {
                Purchasing_Order_Number: vm.selectedPONumber,
                UserId: userId,
                POUniqueId:vm.selectedPOId
            };
            POServices.callServices("getPOById", data).then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        vm.poDetails = response.data[0];
                        vm.isPOdetail=true;
                        vm.GSTDate=new Date('2017/07/03');
                        vm.poDetails.PODate=new Date(vm.poDetails.PODate);
                        getPOMilestones();
                         /** grand total words **/ 
                          var num=0;
                        if (vm.poDetails.Amount.indexOf('.') > -1) {
            
                            num=vm.poDetails.Amount.split('.')[0];
             
                          }
                          else
                          {
                            num=vm.poDetails.Amount;
                          }                       
                        vm.GrandTotal=NumToWord(num);
                   
                    }
                    if(vm.poDetails.ReleaseCode2Status=='Draft')
                    {
                      //$('.watermark').show();
                      
                           vm.isdraft=true;
                        vm.isSubmitter=true;
                        vm.isWithdraw=false;
                        console.log(vm.isdraft);
                       // $('.pdftable').addClass('watermark-img');

                        $scope.draftClass.push('watermark-img');
                    }
                    else if(vm.poDetails.ReleaseCode2Status=='Pending for Approval')
                    {
                       //$('.watermark').hide();
                       vm.isdraft=true;
                         vm.isSubmitter=false;
                         //vm.isWithdraw=true;
                        // $('.pdftable').addClass('watermark-img');
                         $scope.draftClass.push('watermark-img');
                    }
                    else if(vm.poDetails.ReleaseCode2Status=='Approved')
                    {
                        
                          vm.isSubmitter=false;
                        //$('.watermark').hide();
                        vm.isdraft=false;
                         $('.pdftable').removeClass('watermark-img');
                         $scope.draftClass.pop('watermark-img');
                         
                      status = ($stateParams && $stateParams.status) ? $stateParams.status : "All";
                      if(status=="Finalize")
                     { 
                        vm.isWithdraw=false;
                     }

                    }   
                    else if(vm.poDetails.ReleaseCode2Status=='Rejected')
                    {
                          vm.isSubmitter=false;
                        //$('.watermark').hide();
                        vm.isdraft=true;
                        //$('.pdftable').addClass('watermark-img');
                        $scope.draftClass.push('watermark-img');
                    }
                    getPOItemList();
                    getStorageLocation();

                }
            );
        }

        function getPOItemList() {
            vm.poItemList="";
            vm.itemTotal="";
            vm.NatureofSupply="";
            vm.NatureofTransaction="";
            var data = {
                Purchasing_Order_Number: vm.selectedPONumber,
                POUniqueId:vm.selectedPOId
            };
            POServices.callServices("getPurchaseItemListById", data).then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        vm.poItemList = response.data;
                        vm.itemTotal = $window._.sum($window._.map(vm.poItemList, 'itemTotal'));
                        vm.NatureofSupply="";
                        vm.NatureofTransaction=""; 
                        var material=0;
                        var service=0;
                        angular.forEach(vm.poItemList, function(itemlist) {
                            if (itemlist.Material_Number == '' && itemlist.Item_Category == 'D'){
                                service=1;
                                }
                            else{
                                material=1;
                            }
                        });
                        if(material==1 && service==1){
                            vm.NatureofSupply='Goods & Services';
                        }
                        else if(material==0 && service==1){
                            vm.NatureofSupply='Services';
                        }
                        else if(material==1 && service==0){
                            vm.NatureofSupply='Goods';
                        }
                        if(vm.poDetails.Currency_Key!="INR"){
                            vm.NatureofTransaction="Import";
                        }else if(vm.poDetails.VenderRegionCode==vm.poDetails.PlantRegionCode){
                            vm.NatureofTransaction="Intra state";
                        }
                        else{
                            vm.NatureofTransaction="Inter state";
                        }
                    }
                }
            );
        }

        function showItemDetails(item, isVisible) {
            
            if (item.Material_Number == '' && item.Item_Category == 'D' && isVisible) {
                var data = {
                    Purchasing_Order_Number: vm.selectedPONumber,
                    ItemNumber: item.ItemNumber,
                    POUniqueId:vm.selectedPOId
                };
                POServices.callServices("getPurchaseItemDetailsById", data).then(
                    function(response) {
                        if (response.data && response.data.length > 0) {
                            item.poItemSubList = response.data;
                            item.subItemTotal = $window._.sum($window._.map(item.poItemSubList, 'itemTotal'));
                            //return response.data;
                        }
                    }
                );
            }
        }

        function getPOItemAnnexureList() {
            vm.poItemAnnexureList="";
            vm.AnnexureitemTotal="";
            vm.IsAnnexurePrint=false;
            var data = {
                Purchasing_Order_Number: vm.selectedPONumber,
                POUniqueId:vm.selectedPOId
            };
            POServices.callServices("getPurchaseItemAnnexureListById", data).then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        vm.poItemAnnexureList = response.data;
                        angular.forEach(vm.poItemAnnexureList, function(itemlist) {
                            itemlist.ConditionTypeTotal=(itemlist.PackingForwardingValue+itemlist.OtherServicesValue+itemlist.FreightValue);
                        });
                        vm.AnnexureitemTotal = $window._.sum($window._.map(vm.poItemAnnexureList, 'ConditionTypeTotal'));
                        // if(vm.AnnexureitemTotal>0){
                        //     $('.AnnexureNoPrint').removeClass('no-print');
                        // }
                    }
                }
            );
        }


/****** written by sai.k ***/
  $scope.showSubItemDetails=function(item, isVisible) {
            console.log("showsubItemDetails");
            if (item.Material_Number == '' && item.Item_Category == 'D' && isVisible) {
                var data = {
                    Purchasing_Order_Number: vm.selectedPONumber,
                    ItemNumber: item.ItemNumber,
                    POUniqueId:vm.selectedPOId
                };
                POServices.callServices("getPurchaseItemDetailsById", data).then(
                    function(response) {
                        if (response.data && response.data.length > 0) {
                            item.poItemSubList = response.data;
                            item.subItemTotal = $window._.sum($window._.map(item.poItemSubList, 'itemTotal'));
                            //return response.data;
                        }
                    }
                );
            }
        }
  $scope.CreateMileStone=function(){

                console.log("CreateMileStone");

                if($('.trCreate').find('#pTerm').val()!="")
                {

                   var dt= $filter('date')(vm.termDate, 'yyyy-MM-dd HH:mm:ss Z', 'UTC');
                   console.log(dt);
           var data = {
                    CreatedBy: loginUserName,
                    LastModifiedBy: loginUserName,
                    PaymentTerm: $('.trCreate').find('#pTerm').val(),
                    Amount: $('.trCreate').find('#pamt').val(),
                    Percentage: $('.trCreate').find('#pPer').val(),
                    Date: dt,
                    Remarks: $('.trCreate').find('#pRemarks').val(),
                    ContextIdentifier:vm.selectedPONumber
                };
                
                POServices.callServices("postMilestone", data).then(
                    function(response) {
                        
                        
                        getPOMilestones();
                        if (response.data && response.data.length > 0) {
                           
                           vm.IsMilestoneSave=true;
                           vm.termDate="";
                           $('.trCreate').find('input').attr('readonly',true);
                           $('.Mls').removeClass('.trCreate');
                           $('.trCreate').find('#pRemarks').val("");
                           vm.dfPer=0;
                           vm.dfAmt=0;
                        }
                    }
                );
            }
            else
            {
                 notificationService.error('Payment term is mandatory');
            }

       }
       
    $scope.showSubItemAnnexureDetails=function(item, isVisible) {
            console.log("showsubItemAnnexureDetails");
            if (item.Material_Number == '' && item.Item_Category == 'D' && isVisible) {
                var data = {
                    Purchasing_Order_Number: vm.selectedPONumber,
                    ItemNumber: item.ItemNumber,
                    POUniqueId:vm.selectedPOId
                };
                POServices.callServices("getPurchaseItemAnnexureDetailsById", data).then(
                    function(response) {
                        if (response.data && response.data.length > 0) {
                            item.poItemAnnexureSubList = response.data;
                            item.AnnexuresubItemTotal = $window._.sum($window._.map(item.poItemAnnexureSubList, 'itemTotal'));
                            //return response.data;
                        }
                    }
                );
            }
        }

        $scope.isShipToDropdown=function(){
             vm.poDetails.ShipToCode="";
            vm.poDetails.ShipToName="";
            vm.poDetails.ShipToAddress="";
            vm.poDetails.ShipToCountry="";
            vm.poDetails.ShipToGSTCode="";
        }
        $scope.openModal=function(){
             
              vm.modal=true;
        }
        $scope.openModal=function(){
             
              vm.modal=true;
        }
        $scope.trShow=function(ms){
             
              ms.isOver=true;
              
        }
        $scope.trHide=function(ms){
             
              ms.isOver= false;
              
        }
       $scope.termShow=function(pTerms){
             
              pTerms.IsTermedit=true;
        }
        $scope.termHide=function(ms){
             
              ms.isOver= false;
        }
        $scope.CalAmount=function(ms){
           
            
              ms.Amount=(ms.Percentage*parseFloat(vm.poDetails.AmountExclTax))/100;
              ms.Amount=Math.round(ms.Amount * 100) / 100;
          }
        $scope.CalPercentage=function(ms){
          
         
              ms.Percentage=(ms.Amount/parseFloat(vm.poDetails.AmountExclTax))*100;
              ms.Percentage=Math.round(ms.Percentage * 100) / 100;
        }

        $scope.CalicualteAmt=function(){
          
            // vm.dfAmt=(vm.poDetails.Amount*vm.dfPer)*100;
             vm.dfAmt=(vm.dfPer* parseFloat(vm.poDetails.AmountExclTax) )/100;
             vm.dfAmt=Math.round(vm.dfAmt * 100) / 100;
          }
        $scope.CaliculatePer=function(){
          
        
              vm.dfPer=(vm.dfAmt/ parseFloat(vm.poDetails.AmountExclTax))*100;
              vm.dfPer=Math.round(vm.dfPer * 100) / 100;
          }
        
       
        $scope.UpdateMs=function($event,ms){
             
              ms.isEdit= true;

              //$($event.target).parent().parent().parent().find('#').attr('readonly','false');

               var dt= $filter('date')(ms.Date, 'yyyy-MM-dd HH:mm:ss Z', 'UTC');
                   console.log(dt);
        
           var data = {
                    
                    LastModifiedBy: loginUserName,
                    PaymentTerm: ms.PaymentTerm,
                    Amount: ms.Amount,
                    Percentage: ms.Percentage,
                    Date: dt,
                    Remarks: ms.Remarks,
                    ContextIdentifier:vm.selectedPONumber,
                    UniqueId:ms.UniqueId
                };
         if(vm.UpdateType=="Delete")
         {
             data = {
                    
                    LastModifiedBy: loginUserName,
                    IsDeleted:true,
                    UniqueId:ms.UniqueId
                };
         }
                
                POServices.callServices("postMilestone", data).then(
                    function(response) {
                        console.log("success");
                        
                        getPOMilestones();
                        if (response.data && response.data.length > 0) {
                           
                           // vm.IsMilestoneSave=false;
                           // $('.trCreate').find('input').attr('readonly',true);
                           // $('.Mls').removeClass('.trCreate');

                           $('.trCreate').find('#pRemarks').val("");
                           vm.dfPer=0;
                           vm.dfAmt=0;
                            vm.termDate="";
                        }
                    }
                );


              //var ele=$event.currentTarget;
              //$($event.target).parent().parent().parent().find('.mInput').attr('readonly','false');
              
        }
        $scope.DeleteMs=function($event,ms){
             
              ms.isEdit= true;
              //$($event.target).parent().parent().parent().find('#').attr('readonly','false');
         console.log(vm.UpdateType);
           var data = {
                    
                    LastModifiedBy: loginUserName,
                    IsDeleted:true,
                    UniqueId:ms.UniqueId
                };
         
                
                POServices.callServices("postMilestone", data).then(
                    function(response) {
                        console.log("success");
                        
                        getPOMilestones();
                        if (response.data && response.data.length > 0) {
                           
                           // vm.IsMilestoneSave=false;
                           // $('.trCreate').find('input').attr('readonly',true);
                           // $('.Mls').removeClass('.trCreate');
                        }
                    }
                );



        }

$scope.getMs=function(){

getPOMilestones();

}

            function getPOMilestones(){
              console.log("method");
                  var data = {
                        
                        PONumber:vm.selectedPONumber
                    };
                    POServices.callServices("getPoMilestones", data).then(
                        function(response) {
                            if (response.data && response.data.length > 0) {
                                vm.poMilestoneList = response.data;
                                vm.IsMilestoneAvail=true;
                                 vm.IsMilestoneSave=false;
                                //vm.itemTotal = $window._.sum($window._.map(vm.poItemList, 'itemTotal'));

                            $('.trCreate').find('#pRemarks').val("");
                           vm.dfPer=0;
                           vm.dfAmt=0;
                           vm.termDate="";
                                for(var u=0;u< vm.poMilestoneList.length;u++)
                                {
                                     vm.poMilestoneList[u].Percentage=(vm.poMilestoneList[u].Amount/parseFloat(vm.poDetails.AmountExclTax))*100;
                                      vm.poMilestoneList[u].Percentage=Math.round(vm.poMilestoneList[u].Percentage * 100) / 100;
                                      console.log( vm.poMilestoneList[u].Percentage);
                                }
                            }
                            else
                            {
                                vm.IsMilestoneAvail=false;
                                vm.poMilestoneList =[];
                               
                                //vm.IsMilestoneSave=true;
                            }
                        }
                     );

            }

         $scope.getMasterGeneralTerms=function(){
                       
                  var data = {
                        
                       Type:"General"
                    };
                    POServices.callServices("getMasterTandC", data).then(
                        function(response) {
                            if (response.data && response.data.length > 0) {
                                vm.masterTermsList = response.data;
                                }
                           
                        }
                     );

            }
             $scope.getMasterSpecificTerms=function(){
                       
                  var data = {
                        
                       Type:"Specific"
                    };
                    POServices.callServices("getMasterTandC", data).then(
                        function(response) {
                            if (response.data && response.data.length > 0) {
                                vm.masterTermsList = response.data;

                                }
                           
                        }
                     );

            }
$scope.selection=[];
  $scope.toggleSelection = function toggleSelection(terms) {
     console.log(terms);
    var idx = $scope.selection.indexOf(terms);
 
     // is currently selected
     if (idx > -1) {
       $scope.selection.splice(idx, 1);
     }
 
     // is newly selected
     else {
       $scope.selection.push(terms);
       console.log($scope.selection);
     }
   };

    $scope.selectTerms=function(){

          $('.close').click();

          console.log($scope.selection);
        //  $scope.$log = vm.masterTermsList;
          for(var u=0;u<= $scope.selection.length;u++)
          {
            
             var data = {
                    
                    CreatedBy:loginUserName,
                    LastModifiedBy: loginUserName,                    
                    Title:$scope.selection[u].Title,
                    Type:$scope.selection[u].Type,
                    Condition:$scope.selection[u].Condition,
                    IsActive:true,
                    MasterId:$scope.selection[u].UniqueId,
                    ContextIdentifier:vm.selectedPONumber
                };
         
                
                POServices.callServices("postPOTerms", data).then(
                    function(response) {
                        console.log("success");
                           $scope.selection=[];
                        getPOGenTerms();
                        getPOSpecificTerms();
                        //getPOMilestones();
                        if (response.data && response.data.length > 0) {
                           
                           // vm.IsMilestoneSave=false;
                           // $('.trCreate').find('input').attr('readonly',true);
                           // $('.Mls').removeClass('.trCreate');
                        }
                    }
                );
          }
   }

   function getPOGenTerms(){

                  var data = {
                        PONumber:vm.selectedPONumber,
                         Type:"General"
                    };
                    POServices.callServices("getPoTerms", data).then(
                        function(response) {
                            if (response.data && response.data.length > 0) {
                                vm.poTermsList = response.data;
                                vm.isGenAvail=true;
                            }
                               else
                               {
                                 vm.poTermsList=[];
                                  vm.isGenAvail=false;
                               }
                        }
                     );

            }
             function getPOSpecificTerms(){

                  var data = {
                        PONumber:vm.selectedPONumber,
                        Type:"Specific"
                    };
                    POServices.callServices("getPoTerms", data).then(
                        function(response) {
                            if (response.data && response.data.length > 0) {
                                vm.poSpecTermsList = response.data;
                                vm.isSpecAvail=true;
                            }
                               else
                               {
                                 vm.poSpecTermsList=[];
                                  vm.isSpecAvail=false;
                               }
                        }
                     );

            }

            $scope.UpdateTerms=function(pTerms){

               var data = {

                        LastModifiedBy: loginUserName,
                        UniqueId:pTerms.UniqueId,                       
                        Type:pTerms.Type,
                        ContextIdentifier:pTerms.ContextIdentifier,
                        SequenceId:pTerms.SequenceId,                    
                        Title:pTerms.Title,                       
                        Condition:pTerms.Condition,
                        IsActive:true
                        
                   
                  
                    };
                       POServices.callServices("postPOTerms", data).then(
                    function(response) {
                        
                      
                        getPOGenTerms();
                        getPOSpecificTerms();
                      
                        if (response.data && response.data.length > 0) {
                           
                           // vm.IsMilestoneSave=false;
                           // $('.trCreate').find('input').attr('readonly',true);
                           // $('.Mls').removeClass('.trCreate');
                        }
                    }
                );

            }
            $scope.deleteTerms=function(pTerms){

               var data = {

                        LastModifiedBy: loginUserName,
                        UniqueId:pTerms.UniqueId,
                        IsDeleted:true,
                        Type:pTerms.Type,
                        ContextIdentifier:vm.selectedPONumber
                    };
                       POServices.callServices("postPOTerms", data).then(
                    function(response) {
                        
                      
                        getPOGenTerms();
                        getPOSpecificTerms();
                      
                        if (response.data && response.data.length > 0) {
                           
                           // vm.IsMilestoneSave=false;
                           // $('.trCreate').find('input').attr('readonly',true);
                           // $('.Mls').removeClass('.trCreate');
                        }
                    }
                );

            }
            $scope.CreateNewCondion=function(){


                  var data = {
                    
                    CreatedBy:loginUserName,
                    LastModifiedBy: loginUserName,                                     
                    Title:vm.SpecificTitle,
                    Type:vm.SpecificTypeId,
                    Condition:vm.SpecificCondition,
                    IsActive:true,                    
                    ContextIdentifier:vm.selectedPONumber
                };
         
                
                POServices.callServices("postPOTerms", data).then(
                    function(response) {
                        console.log("success");
                           $scope.selection=[];
                        getPOGenTerms();
                        getPOSpecificTerms();
                       
                        if (response.data && response.data.length > 0) {
                           
                           // vm.IsMilestoneSave=false;
                           // $('.trCreate').find('input').attr('readonly',true);
                           // $('.Mls').removeClass('.trCreate');
                        }
                    }
                );
            }

            function getPickList(){


                  var data = {
                    
                    "PickListNames": ["TermsAndConditionType","POServiceCondition","POServiceType"]
                    
                };
         
                
                POServices.callServices("getPickList", data).then(
                    function(response) {
                        
                          
                        if (response.data && response.data.length > 0) {
                           var item=response.data[0].PickLstNmeVluMap.TermsAndConditionType;
                         for (var u = 0; u < item.length; u++) {
                         if(item[u].ChildDescription=="Specific")
                         {
                            vm.SpecificTypeId=item[u].PickListId;
                           
                         }
                         }
                           // vm.IsMilestoneSave=false;
                           // $('.trCreate').find('input').attr('readonly',true);
                           // $('.Mls').removeClass('.trCreate');
                        }
                        if (response.data && response.data.length > 0) {
                           vm.POServiceCondition=response.data[0].PickLstNmeVluMap.POServiceCondition;
                           // $scope.packing=vm.POServiceCondition[0];
                           // $scope.Freight=vm.POServiceCondition[0];
                           // $scope.OtherServices=vm.POServiceCondition[0];
                        }
                        if (response.data && response.data.length > 0) {
                           vm.POServiceType=response.data[0].PickLstNmeVluMap.POServiceType;
                           // $scope.ServiceType=vm.POServiceType[0];
                        }
                    }
                );
            }

              $scope.MilestoneCheck=function(){

                 
                vm.MileStoneTotal = $window._.sum($window._.map(vm.poMilestoneList, 'Amount'));

                vm.MileStoneTotal=Math.round(vm.MileStoneTotal * 100) / 100;
                console.log(vm.MileStoneTotal);
                console.log(vm.poDetails.AmountExclTax);
                if(vm.poDetails.AmountExclTax==vm.MileStoneTotal)
                {
                    if(vm.comments!="")
                    submitforApproval();
                    else
                     notificationService.error('Comments are mandatory');
                }
                else
                {
                    notificationService.error('Milestone total not matching with grand total');
                }
              }
            function submitforApproval(){
            if(vm.poDetails.PODate >vm.GSTDate){
           var data = {
                    
                      POUniqueId:vm.selectedPOId,
                      PurchaseOrderNumber: vm.selectedPONumber,
                      SubmitterComments:vm.comments,
                      UserId: userId,
                      //shipTo:vm.poDetails.ShipToCode,
                      annexureModel:vm.poItemAnnexureList
                };
            }else{
                var data = {
                    
                      POUniqueId:vm.selectedPOId,
                      PurchaseOrderNumber: vm.selectedPONumber,
                      SubmitterComments:vm.comments,
                      UserId: userId,
            }
             console.log(data);
            }
            POServices.callServices("PoSubmit", data).then(
                function(response) {
                    if (response.data =="Success") {
                       vm.isSubmitter=false;
                       vm.isWithdraw=true;
                       vm.IsPOSubmited=true;
                       vm.comments="";
                       notificationService.success('PO submitted successfully');
                        getPOApproverList(true);
                    }
                    else
                    {
                        notificationService.error('Operation failed, contact Fugue support.');
                    }
                }
            );

            }
               $scope.PoWithdraw=function(){
         
           var data = {
                    
                      POUniqueId:vm.selectedPOId,
                      
                      UserId: userId
                };
             
            POServices.callServices("PoWithdraw", data).then(
                function(response) {

                   

                    if (response.data =="Success") {
                        vm.isSubmitter=true;
                       vm.isWithdraw=false;
                       vm.IsPOSubmited=false;
                       notificationService.success('PO withdrawn successfully');
                        getPOApproverList(true);
                        vm.ApprAvail=false;
                    }
                    else
                    {
                        notificationService.error('Operation failed, contact Fugue support.');
                    }
                }
            );

            }

 function NumToWord(inputNumber)
{
    var str = new String(inputNumber)
    var splt = str.split("");
    var rev = splt.reverse();
    var once = ['Zero', ' One', ' Two', ' Three', ' Four', ' Five', ' Six', ' Seven', ' Eight', ' Nine'];
    var twos = ['Ten', ' Eleven', ' Twelve', ' Thirteen', ' Fourteen', ' Fifteen', ' Sixteen', ' Seventeen', ' Eighteen', ' Nineteen'];
    var tens = ['', 'Ten', ' Twenty', ' Thirty', ' Forty', ' Fifty', ' Sixty', ' Seventy', ' Eighty', ' Ninety'];

    var numLength = rev.length;
    var word = new Array();
    var j = 0;

    for (i = 0; i < numLength; i++) {
        switch (i) {

            case 0:
                if ((rev[i] == 0) || (rev[i + 1] == 1)) {
                    word[j] = '';
                }
                else {
                    word[j] = '' + once[rev[i]];
                }
                word[j] = word[j];
                break;

            case 1:
                aboveTens();
                break;

            case 2:
                if (rev[i] == 0) {
                    word[j] = '';
                }
                else if ((rev[i - 1] == 0) || (rev[i - 2] == 0)) {
                    word[j] = once[rev[i]] + " Hundred ";
                }
                else {
                    word[j] = once[rev[i]] + " Hundred and";
                }
                break;

            case 3:
                if (rev[i] == 0 || rev[i + 1] == 1) {
                    word[j] = '';
                }
                else {
                    word[j] = once[rev[i]];
                }
                if ((rev[i + 1] != 0) || (rev[i] > 0)) {
                    word[j] = word[j] + " Thousand";
                }
                break;

                
            case 4:
                aboveTens();
                break;

            case 5:
                if ((rev[i] == 0) || (rev[i + 1] == 1)) {
                    word[j] = '';
                }
                else {
                    word[j] = once[rev[i]];
                }
                if (rev[i + 1] !== '0' || rev[i] > '0') {
                    word[j] = word[j] + " Lakh";
                }
                 
                break;

            case 6:
                aboveTens();
                break;

            case 7:
                if ((rev[i] == 0) || (rev[i + 1] == 1)) {
                    word[j] = '';
                }
                else {
                    word[j] = once[rev[i]];
                }
                if (rev[i + 1] !== '0' || rev[i] > '0') {
                    word[j] = word[j] + " Crore";
                }                
                break;

            case 8:
                aboveTens();
                break;

            //            This is optional. 

            //            case 9:
            //                if ((rev[i] == 0) || (rev[i + 1] == 1)) {
            //                    word[j] = '';
            //                }
            //                else {
            //                    word[j] = once[rev[i]];
            //                }
            //                if (rev[i + 1] !== '0' || rev[i] > '0') {
            //                    word[j] = word[j] + " Arab";
            //                }
            //                break;

            //            case 10:
            //                aboveTens();
            //                break;

            default: break;
        }
        j++;
    }

    function aboveTens() {
        if (rev[i] == 0) { word[j] = ''; }
        else if (rev[i] == 1) { word[j] = twos[rev[i - 1]]; }
        else { word[j] = tens[rev[i]]; }
    }

    word.reverse();
    var finalOutput = '';
    for (i = 0; i < numLength; i++) {
        finalOutput = finalOutput + word[i];
    }
    //document.getElementById(outputControl).innerHTML = finalOutput;
    //console.log(finalOutput);
    return finalOutput;
}

     $scope.printDiv=function(divID) {
      // var popupWin;

       var printContents = document.getElementById(divID).innerHTML;
      // popupWin = window.open('', '_blank', 'width=1100,height=600');
      // popupWin.document.open();
      // popupWin.document.write('<!doctype html><html ><head><link rel="stylesheet" media="print"  href="http://192.168.51.248/teProcurement/styles/pdf_print.css" /><link rel="stylesheet"  href="http://192.168.51.248/teProcurement/styles/main.css" />'+
      //  '</head><body onload="setTimeout(function(){ window.print() }, 300);">'+printContents + '</body></html>');
      // popupWin.document.close();
    

    //  printElement(document.getElementById(divID));
    
    // var modThis = document.querySelector("#printSection");
    // console.log(modThis);
    // modThis.appendChild(document.createTextNode(" new"));
    
    // window.print();

        
        //printHtml(printContents);
         Popup($('<div/>').append($(printContents).clone()).html());
        }     

 function Popup(data) 
{

    var url=commonUtilService.getUrl('PoWebUrl');
    var mywindow = window.open('', 'my div', 'height=400,width=600');
    mywindow.document.write('<!DOCTYPE html><html><head><title></title>');
    mywindow.document.write('<link rel="stylesheet" media="print"  href="http://localhost:3000/styles/pdf_print.css" /><link rel="stylesheet"  href="http://localhost:3000/styles/main.css" />');
   
    mywindow.document.write('</head><body >');
    mywindow.document.write(data);
    mywindow.document.write('</body></html>');
setTimeout(function(){

  mywindow.print();
  mywindow.close();

},2000);
   
  
    return true;
}
var printHtml = function (html) {

    console.log("testing");
    var hiddenFrame = $('<iframe style="display: none"></iframe>').appendTo('body')[0];
    hiddenFrame.contentWindow.printAndRemove = function() {
        hiddenFrame.contentWindow.print();
        $(hiddenFrame).remove();
    };
    var htmlDocument = "<!doctype html>"+
                "<html>"+
                    '<body onload="printAndRemove();">' + // Print only after document is loaded
                        html +
                    '</body>'+
                "</html>";
    var doc = hiddenFrame.contentWindow.document.open("text/html", "replace");
    doc.write(htmlDocument);
    doc.close();
};

 function printElement(elem) {
    var domClone = elem.cloneNode(true);
    
    var $printSection = document.getElementById("printSection");
    
    if (!$printSection) {
        var $printSection = document.createElement("div");
        $printSection.id = "printSection";
        document.body.appendChild($printSection);
    }
    
    $printSection.innerHTML = "";
    
    $printSection.appendChild(domClone);
}
/****** written by sai.k end ***/
     

        function getPOApproverList(isRefresh) {
            var data = {
                PurchasingOrderNumber: vm.selectedPONumber,
                POUniqueId:vm.selectedPOId

            };
            POServices.callServices("getPOApproversById", data).then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        vm.poApproverList = response.data;
                         vm.ApprAvail=true;

                        checkForApprover();
                        if (isRefresh)
                            updatePOGrid();
                    }
                    else
                    {
                        vm.poApproverList=[];
                    }
                }
            );
        }

        function checkForApprover() {
            var approver = $window._.find(vm.poApproverList, { 'ApproverId': userId, Status: 'Pending for Approval' });
            if (approver) {
               
                var status = ($stateParams && $stateParams.status) ? $stateParams.status : "All";

                if(status!="Finalize")
                {
                     vm.isApprover = true;
                }
               
                vm.isSubmitter=false;
                


                aproverUniqueid = approver.UniqueId;
                aproverName = approver.ApproverName;
                sequenceId = approver.SequenceNumber;
                releaseCode = approver.ReleaseCode;
            }
        }

        function getTermAndConditions() {
            var data = {
                Purchasing_Order_Number: vm.selectedPONumber
            };
            POServices.callServices("getPOTermsAndConditions", data).then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        vm.poTermsAndConditions = response.data;
                        // $mdDialog.show({
                        //     flex: '66',
                        //     scope: $scope,
                        //     preserveScope: true,
                        //     templateUrl: 'app/po/poTermsAndConditions.tmpl.html',
                        //     controller: function DialogController($scope, $mdDialog) {
                        //         $scope.closeDialog = function() {
                        //             $mdDialog.hide();
                        //         }
                        //     }
                        // });
                    }
                }
            );
        }
    }

    function whenScrollEnds() {
        return {
            restrict: "A",
            link: function(scope, element, attrs) {
                var visibleHeight = element.height();
                var threshold = 500;

                element.scroll(function() {
                    var scrollableHeight = element.prop('scrollHeight');
                    var hiddenContentHeight = scrollableHeight - visibleHeight;
                    if (hiddenContentHeight - element.scrollTop() <= threshold) {
                        // Scroll is almost at the bottom. Loading more rows
                        scope.$apply(attrs.whenScrollEnds);
                    }
                });
            }
        };
    }
   
   



 
})();
