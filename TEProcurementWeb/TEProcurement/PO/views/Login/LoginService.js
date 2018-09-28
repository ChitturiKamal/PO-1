angular.module('TEPOApp')
.factory('AuthenticationService',
    ['$http', '$localStorage', '$rootScope', '$timeout','$sessionStorage','$location',
    function ($http, $localStorage, $rootScope, $timeout,$sessionStorage,$location) {
        var service = {};
        service.Login = function (email, password, callback) {
			$http.post($sessionStorage.server_url+'/TEUserProfileAPI/AuthUser', { UserName: email, Password: password },heads).success(function (response){
				if(response.info.errorcode==0){$localStorage.Mycheck=true;}
				callback(response);
			});
        };
        service.LoginAgent = function (email, password, callback) {
			$http.post($sessionStorage.server_url+'/TEUserProfileAPI/AuthUserCallCenter', { UserName: email, Password: password },heads).success(function (response){
				callback(response);
			});
        };
        service.SetCredentials = function (response){
			var roles = [];
			for(var i=0;i<response.result.length;i++){
				roles.push(response.result[i].RoleName);
			}
            $rootScope.globals = {
                currentUser: {
					loginID:response.Token.UserID,
					CallName:response.result[0].CallName,
					UserId:response.Token.UserID,
					UserEmailID:response.result[0].Email,
					AuthToken:response.Token.Token,
					RoleNames:roles,
                }
            };
			$rootScope.privilages=response.privilages;
            $localStorage.globals=$rootScope.globals;
			$localStorage.privilages=$rootScope.privilages;
			$timeout(function(){				
				$location.path('Dashboard');
				$timeout(function(){location.reload();}, 300);

			}, 300);
        };
        service.SetCredentialsGuest = function (response){
			var roles = [];
			for(var i=0;i<response.result.length;i++){
				roles.push(response.result[i].RoleName);
			}
            $rootScope.globals = {
                currentUser: {
					loginID:response.Token.UserID,
					CallName:response.result[0].CallName,
					UserId:response.Token.UserID,
					UserEmailID:response.result[0].Email,
					AuthToken:response.Token.Token,
					RoleNames:roles,
                }
            };
			$rootScope.privilages=response.privilages;
            $localStorage.globals=$rootScope.globals;
			$localStorage.privilages=$rootScope.privilages;
			$timeout(function(){
				//$location.path('Dashboard');
				//$timeout(function(){location.reload();},0);

			}, 300);
        };
        service.ClearCredentials = function () {
            $rootScope.globals = {};
			delete $localStorage.globals;
			delete $localStorage.privilages;
        };

        return service;
    }])
.factory('NotificationsService', function($sessionStorage,$http){
	var result={};
	result.Notifications = function(data){return $http.post($sessionStorage.server_url+'/TENotificationsAPI/GetNotificationsByUserID',data,heads);}
	return result;
})