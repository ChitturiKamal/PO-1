angular.module('TEPOApp')
.factory('AuthService',
    ['$http', '$localStorage','$sessionStorage', '$rootScope', '$timeout', '$location',
    function ($http, $localStorage,$sessionStorage, $rootScope, $timeout,$location) {
        var service = {};
        service.AuthLogin = function (callback) {
            if(!$localStorage.globals || !$localStorage.globals.currentUser.loginID || $localStorage.globals.currentUser.loginID=="" || $localStorage.globals.currentUser.loginID=="0"){
				$rootScope.globals = {};
				localStorage.clear();
				$localStorage.$reset();
				var authContext = new AuthenticationContext({clientId: '7b32c697-48e5-4046-a507-25acb1eb8f2a',postLogoutRedirectUri: $rootScope.IpServer + '/TEProcurement/'});
				authContext.logOut();
			}
        };
        service.Logout = function () {
			$http.post($sessionStorage.server_url + '/TEUserProfileAPI/ReleaseToken', {'authUser':$localStorage.globals.currentUser.UserId,'authToken':$localStorage.globals.currentUser.AuthToken}, heads).success(function(response) {});		
			$rootScope.globals = {};
			localStorage.clear();
			var authContext = new AuthenticationContext({clientId: '7b32c697-48e5-4046-a507-25acb1eb8f2a',postLogoutRedirectUri: $rootScope.IpServer + '/TEProcurement/'});
			authContext.logOut();
        };
		service.CheckPrivilage = function (module,privilage,callback) {
			//callback(true);
			if($localStorage.privilages){
				if($localStorage.privilages[module]){
					if($localStorage.privilages[module][privilage]){
						if($localStorage.privilages[module][privilage]=='True'){callback(true);}else callback(false);
					}else{callback(false);}
				}else{callback(false);}
			}else{callback(false);}
        };
		service.PrivilageView = function (module,callback) {
			if($localStorage.privilages){
				if($localStorage.privilages[module]){
					if($localStorage.privilages[module].View=='True'){
					}else{$location.path('/Login');}
				}else{$location.path('/Login');}
			}else{$location.path('/Login');}
        };
		service.PrivilageCreate = function (module,callback) {
			if($localStorage.privilages){
				if($localStorage.privilages[module]){
					if($localStorage.privilages[module].Save=='True'){
					}else{$location.path('/Login');}
				}else{$location.path('/Login');}
			}else{$location.path('/Login');}
        };
		service.AuditStorage = function (AuditID,callback) {
			$http.post($sessionStorage.server_ip + '/Portfolio/api/UIAuditAPI/SaveUIAudit', {'UniqueID':AuditID,'LastModifiedBy':$localStorage.globals.currentUser.UserId}, heads).success(function(response) {});		
        };

        return service;
    }])