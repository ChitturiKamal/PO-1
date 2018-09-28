angular.module('TEPOApp') 
.controller('LoginController',
    ['$scope', '$rootScope', '$location', 'AuthenticationService','NotificationsService','$state',
    function ($scope, $rootScope, $location, AuthenticationService,NotificationsService,$state) {
        AuthenticationService.ClearCredentials(); 
        $scope.login = function () {
            AuthenticationService.Login($scope.email, $scope.password, function(response) {
                if(response.info.errorcode=='0') {
                    AuthenticationService.SetCredentials(response);
                    NotificationsService.Notifications({UserID:response.Token.UserID,'page_number':0,'pagepercount':'15'}).success(function(response){$rootScope.NotificaitonInfo=response.info;$rootScope.NotificaitonData=response;});
                }
				else {
                    $scope.error = "Invalid Login Credentials";
                }
            });
        };
    }]);