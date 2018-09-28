(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('poMasterApproverCtrl', poMasterApproverCtrl)
        .directive("multiplemobiles", function() {
            return {
                template: '<div class="headtxt-big" style="margin: 10px 0;" ng-repeat="data in vm.MultiApproval">\
                            <div class="row" style="margin-right:0;">\
                            <div class="col-sm-10 plr2">\
                            <select class="form-control input-css" name="ApproverName" ng-model="data.UserId" ng-change="vm.getApprover(data.UserId,$index+2);">\
                            <option value="" style="display: none;"></option>\
                            <option ng-repeat="data in vm.Submitter" value="{{data.UserId}}">{{data.UserName}}</option>\
                            </select>\
                            </div>\
                            <div class="col-sm-2 plr2"><a href="javascript:void(0);" style="margin-left:5px; margin-top:2px;float:left;" ng-click="vm.RemoveApprovalList($index);"><i class="fa fa-times" style="font-size:10pt;"></i></a></div>\
                            </div>\
                            </div>'
            };
        })

    /* @ngInject */
    /* function config() {}*/
    function poMasterApproverCtrl($http, $window,poMasterApproverServices,AlertMessages,$rootScope,$timeout,$uibModal,$state) {
        var vm = this;
        vm.pagenumber = 1;
        vm.pageprecounts = 10;

        vm.callData = function (){
            vm.vendorList = [];
            poMasterApproverServices.getAllApproverMaster().then(
                function(response) {
                    if (response.data && response.data.length > 0) {
                        //this.isLoading = false;
                        vm.vendorList = vm.vendorList.concat(response.data);
                    }
                }
            );
        }
       vm. callData();

       vm.pagenationss = function(a, b) {
        var b = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a;
        $scope.pageprecounts = b;
        $scope.GetallList();
      }
      vm.pagenationss_clk = function(a, b) {
        if ("0" == b) var a1 = --a;
        else var a1 = ++a;
        "0" >= a1 ? (a1 = 0, $scope.pagenumber = 1) : $scope.pagenumber = a1;
        var c = $("select[name='pageprecounts']").val();
        $scope.pagenumber = a1;
        $scope.pageprecounts = c;
        $scope.GetallList();
      }

        poMasterApproverServices.getOrderTypeCall().then(function(response){
            vm.OrderType = response.data;
        })
        
        poMasterApproverServices.getFundCenterCall().then(function(response){
            vm.FundCenter = response.data;
        })

        poMasterApproverServices.getSubmitterCall().then(function(response){
            vm.Submitter = response.data;
        })

        vm.MultiApproval = [];
        vm.AddApproverList = function() {
            var ApproverIdNo = vm.MultiApproval.length + 2;
            vm.MultiApproval.push({ 'id':  ApproverIdNo });
        };

        vm.RemoveApprovalList = function(index){
            vm.MultiApproval.splice(index, 1);
            vm.MasterApproverlist.splice((index+1), 1);
            vm.serializeIndex(vm.MasterApproverlist);
        }

        vm.serializeIndex = function(MasterApproverlist){
             vm. MasterApproverlist = [];
             var i=1;
            angular.forEach(MasterApproverlist, function(value, key) {
                 if(value.SequenceId != '0'){
                    vm.MasterApproverlist.push({'ApproverId':value.ApproverId,'ApproverName':value.ApproverName,'Type':'Approver','SequenceId':i});
                    i++;
                 }
                 else
                 vm.MasterApproverlist.push({'ApproverId':value.ApproverId,'ApproverName':value.ApproverName,'Type':'submitter','SequenceId':vm.ind});
              });
        }

        vm.MasterApproverlist = [];
        vm.getSubmitter = function(submId){
            angular.forEach(vm.Submitter, function(value, key) {
                if(value.UserId == submId){
                    vm.MasterApproverlist.push({'ApproverId':submId,'ApproverName':value.UserName,'Type':'submitter','SequenceId':'0'});
                }
              });
        }
        
        vm.getApprover = function(Aid,index){
            angular.forEach(vm.Submitter, function(value, key) {
                if(value.UserId == Aid){
                    vm.MasterApproverlist.push({'ApproverId':Aid,'ApproverName':value.UserName,'Type':'Approver','SequenceId':index});
                }
              });
        }

        vm.AppCond = function(val){
            if(val == "1"){vm.MinAmount = "0";vm.MaxAmount = "10,00,000"}
            if(val == "2"){vm.MinAmount = "10,00,001";vm.MaxAmount = "50,00,000"}
            if(val == "3"){vm.MinAmount = "50,00,001";vm.MaxAmount = "100,00,000"}
        }
        
     vm.FromCreate = function() {
         var data = JSON.stringify($('.addForm').serializeObject());
         var obj = $('.addForm').serializeObject();
         var validate = JSON.parse(data);
        if ((validate.OrderType).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Order Type is Mandatory' }); return false; }
        else if ((validate.FundCenter).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Fund Center is Mandatory' }); return false; }
        else if ((validate.Approval).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Approval is Mandatory' }); return false; }
        else if ((validate.SubmitterName).trim() == '') { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Submitter is Mandatory' }); return false; }
        else if ((validate.ApproverName).length != getArrayCounts(validate.ApproverName)) { AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Approver is Mandatory' }); return false; }
        else
        {
            var obj = {"ApprovalCondition": {'OrderType':validate.OrderType,'FundCenter':validate.FundCenter,'MinAmount':vm.MinAmount,'MaxAmount':vm.MaxAmount},"MasterApproverlist":vm.MasterApproverlist}
            poMasterApproverServices.saveMasterApprover('masterApprover',obj)
            .then(
                function(response) {
                    if (response.statusText == "OK") {
                          $('#addForm').modal('hide');
                        //  vm. callData();
                         //$state.reload();
                         AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Data successfully saved' });
                    }
                  
                }
            );
    //         // .success(function(response) {
    //         //     if (response.info.errorcode == '0'){
    //         //       $('#addSalesPlan').modal('hide');
    //         //       setTimeout(function() {$state.reload();}, 300);
    //         //     }
    //         //     AlertMessages.alertPopup(response.info);
    //         // });
           
         }
     }

     $rootScope.autoHide =function(){$timeout(function(){$rootScope.alerts.splice(0, 1);}, 3000);}

     $.fn.serializeObject = function() {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function() {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
    function getArrayCounts(array) {
        return (array.filter(item => item.trim() !== '')).length;
    }

    $rootScope.autoHide =function(){
        $timeout(function(){
            $rootScope.alerts.splice(0, 1);}, 3000);
        }

    }
})();
