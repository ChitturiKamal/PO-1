angular.module('TEPOApp')
.controller('UserRoleMappingCtrl', function($scope,$rootScope,$sessionStorage,$element,UserRoleMappingFactory,AlertMessages,$sce){
	$scope.pagenumber= 1;$scope.pageprecounts=999999999;
	$scope.currentdate = new Date();
	$scope.GetList=function(){
		if($scope.pagenumber<='0')$scope.pagenumber=1;
        UserRoleMappingFactory.GetPagenation({'page_number':$scope.pagenumber,'pagepercount':$scope.pageprecounts}).success(function(response){$scope.result=response;});
	}
	$scope.GetList();
	$scope.pagenationss=function(a,b){
		var b = $("select[name='pageprecounts']").val();
		$scope.pagenumber=a;$scope.pageprecounts=b;$scope.GetList();
	}
	$scope.pagenationss_clk = function (a,b){
		if("0"==b)var a1=--a;
		else var a1=++a;"0">=a1?(a1=0,$scope.pagenumber=1):$scope.pagenumber=a1;
		var c=$("select[name='pageprecounts']").val();
		$scope.pagenumber=a1;$scope.pageprecounts=c;
		$scope.GetList();
	}
	$scope.FromCreate=function(){
		var data = JSON.stringify($('.addForm').serializeObject());
		var validate = JSON.parse(data);
		if (!validate.userID || validate.userID==''){ AlertMessages.alertPopup({ errorcode: '1', errormessage: 'User Name is Mandatory' }); return false; }
		else if(!validate.RoleID || (validate.RoleID).length<='1'){ AlertMessages.alertPopup({ errorcode: '1', errormessage: 'Role Name is Mandatory' }); return false; }
 		else {
			UserRoleMappingFactory.Save(data).success(function(response){
				if(response.info.errorcode=='0'){$('.addForm')[0].reset();$scope.resultNotAssigned=[];$('#addUserRole').modal('hide'); $scope.UserID=""; }
				AlertMessages.alertPopup(response.info);
				$scope.GetList();
			});
		}
	}
	$scope.ForDelete=function(a,b){$scope.SelRoleId=a;$scope.SelUserId=b;}
	$scope.FromDeleteFinal=function(a,b){
		UserRoleMappingFactory.Delete({"RoleId":a,"UserId":b}).success(function(response){
			if(response.info.errorcode=='0'){$('#DeleteRoles').modal('hide');}
			AlertMessages.alertPopup(response.info);
			$scope.GetList();
		});

	}
	$scope.GetUserRoles = function(a){UserRoleMappingFactory.RolesList({'UserID':a}).success(function(response){$scope.resultNotAssigned=response;});}
	UserRoleMappingFactory.UsersList({ 'page_number': 0,'pagepercount':10}).success(function(response){$scope.UserProfilesDb = response;});
			
})