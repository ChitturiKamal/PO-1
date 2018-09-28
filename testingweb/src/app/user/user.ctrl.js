(function() {
    'use strict';

    angular
        .module('procurementApp')
        .controller('UserController', UserController)
        .directive('backgroundChanger', backgroundChanger);

    /** @ngInject */
    function UserController(UserServices, $state, $window, commonUtilService) {
        var vm = this;
        vm.configRole = commonUtilService.config();
        vm.login123=login123;
        vm.Authentication=Authentication;

function Authentication(){
    console.log("Authentication");
var authContext = new AuthenticationContext({clientId: '7b32c697-48e5-4046-a507-25acb1eb8f2a',postLogoutRedirectUri: 'http://localhost:3000/#/login'});
  if (authContext.isCallback(window.location.hash)) {
    authContext.handleWindowCallback();
    var err = authContext.getLoginError();
    if(err){}
  }else{
    var credentials = authContext.getCachedUser();
    console.log(credentials);
    console.log("credentials");
    if(credentials){
        login(credentials);
        //login();
        //window.location="/main";
         //angular.element('#angularData').scope()
        authContext.acquireToken('https://graph.microsoft.com',function(error,token){
        if(error || !token){console.log(token);return;}
        var xhr = new XMLHttpRequest();
        xhr.open('GET', 'https://graph.microsoft.com/v1.0/me', true);
        xhr.open('GET', '', true);
        xhr.setRequestHeader('Authorization', 'Bearer ' + token);
        xhr.onreadystatechange=function(){
          if(xhr.readyState === 4 && xhr.status === 200){console.log(xhr.responseText);}
          else{}
        };
        xhr.send();
      });
    }else{authContext.login();
    }
    //authContext.login();
  }
}

Authentication();

 function login(e){
    $.ajax({
      url: 'http://localhost:39070/api/TERule/GetUserRoles',
      type: "GET",
      //data:{"UserName":e.userName},
      data:{"UserName":'test.user1@total-environment.com'},
      success: function(response) {
         if (response) {
                        localStorage.setItem('TEUserCredentials',JSON.stringify(response));
                        console.log("login");
                        //return false;
                        vm.configRole = commonUtilService.config();
                        // if(vm.configRole.action.POApproval){
                        //window.location="/main/po/All";
                         $state.go('main.poList', {
                            status: 'All'
                        });
                        // }
                        // else{
                        //    //$('#myModal1').modal('show');
                        // }
                    }
                    else
                    {

                    }
      }
    });
  }

     function login123() {
            var json = vm.request;
            UserServices.callServices("userLogin", json).then(
                function(response) {
                    if (response.data && response.data.SuccessResult) {
                        $window.localStorage.setItem('TEUserCredentials', JSON.stringify(response.data.SuccessResult));
                        commonUtilService.setUrl('FugueLangingPage', 'login');
                        // if(vm.configRole.action.AssetManagement)
                        // {
                         $state.go('main', {
                            // 'relationId': 0
                        });
                     // }
                     // else{
                     //    notificationService.error('You do not have required access rights.');
                     // }
                        // $state.go('main.contact.detail', {
                        //     'relationId': 0
                        // });
                    } else {
                        // notificationService.info('Invalid user name or password');
                    }
                }
            );
        }

    }

    function backgroundChanger($window, $interval) {
        return {
            restrict: 'A',
            link: function() {
                var changeImage = function() {
                    var img = $window._.random(1, 9);
                    angular.element('body').css({
                        'background-image': 'url(assets/images/' + img + '.jpg)',
                        'background-size': '100%'
                    });
                };
                $interval(changeImage, 7000);
                changeImage();
            }
        }
    }
})();
