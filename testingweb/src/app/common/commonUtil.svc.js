(function() {
    'use strict';

    angular
        .module('procurementApp')
        .factory('commonUtilService', commonUtilService)
        // .controller('commonCtrl',commonCtrl)
        .filter('currencyFormat', currencyFormat)
        .filter('currencyDecimalFormat',currencyDecimalFormat)
        .filter('transactiontype',transactiontype)
        .filter('ConvertWords', function() {
  function isInteger(x) {
        return x % 1 === 0;
    }

  
  return function(value) {
    if (value && isInteger(value))
      return  toWords(value);
    
    return value;
  };

})



.service('AlertMessages', ['$rootScope', function($rootScope,$timeout) {
    this.alertPopup = function(data) {
        "0" == data.errorcode ? data.errorcode = "success" : data.errorcode = "danger";
        $rootScope.alerts = [{
            type: data.errorcode,
            msg: data.errormessage
        }];
        $rootScope.autoHide();
        $rootScope.autoHide =function(){
            $timeout(function(){
                $rootScope.alerts.splice(0, 1);}, 3000);
            }
    }
    
}])  




    /* @ngInject */
    function commonUtilService($filter, $window, $q, $location, $state,$rootScope) {
        var service = {
            dateUtil: dateUtil,
            settings: {
                dateFormat: 'dd MMM, yyyy'
            },
            config: config,
            getProfile: getProfile,
            isImagePresent: isImagePresent,
            logout: logout,
            getUrl: getUrl,
            setUrl: setUrl
        };
        var url = {
           
            // TEComplaintsUrl: 'http://192.168.51.251/TEComplaintsManagementAPI/',       
            // TEEmailApi: 'http://192.168.51.251/TEEmailAPI/',
            // FugueLangingPage: 'http://192.168.51.251/fuguelanding/index.html',
            // TESAPPurchaseService: 'http://182.72.251.226/sappurchaseservice/podocuments/',
            // TELeadManagementAPI:'http://192.168.51.251/TELeadManagementAPI/',
            // PoWebUrl:'http://localhost:3000/styles/',
            // TEHRISApi:'http://localhost:39070'

            //TEComplaintsUrl: 'http://10.18.18.23/TEComplaintsManagementAPI/', 
            // TEComplaintsUrl:'http://182.18.177.27/TEcomplaintsmanagement'  ,    
            // TEEmailApi: 'http://10.18.18.23/TEEmailAPI/',
            // FugueLangingPage: 'http://10.18.18.23/fuguelanding/index.html',
            // TESAPPurchaseService: 'http://182.72.251.226/sappurchaseservice/podocuments/',
            // TELeadManagementAPI:'http://10.18.18.23/TELeadManagementAPI/',
            // PoWebUrl:'http://localhost:3000/styles/',
            // TEHRISApi:'http://182.18.177.27/hris',
            // TEHRISApi:'http://localhost:39070'  ,          
           // VendorMaster: 'http://10.18.18.22/PurchaseOrder'
        //    VendorMaster: 'http://182.18.177.27/PurchaseOrder'

        TESAPPurchaseService: 'http://182.72.251.226/sappurchaseservice/podocuments/',
        // TELeadManagementAPI:'http://192.168.51.251/TELeadManagementAPI/',
      TEComplaintsUrl: 'http://10.18.18.22/TEComplaintsManagementAPI/',     
       //TEComplaintsUrl:'http://localhost:44271/',  
        TEEmailApi: 'http://10.18.18.22/TEEmailAPI/',
        FugueLangingPage: 'http://10.18.18.22/fuguelanding/index.html',
      //  TESAPPurchaseService: 'http://10.18.18.22/sappurchaseservice/podocuments/',
        TELeadManagementAPI:'http://10.18.18.22/TELeadManagementAPI/',
        PoWebUrl:'http://localhost:3000/styles/',
        TEHRISApi:'http://182.18.177.27/TEHRIS'
       // TEHRISApi:'http://localhost:39070'


        };
        return service;

        
  
        function getUrl(host) {
            return url[host];
        }

        function setUrl(key, value) {
            url[key] = value;
        }

        function config() {
            var TEUserCredentials = angular.fromJson($window.localStorage.getItem('TEUserCredentials'));
            var links = {
                tabs: {},
                action: {}
            };
            if (TEUserCredentials && TEUserCredentials.Roles) {
                var roles = TEUserCredentials.Roles;
                if (roles && roles.length > 0) {
                    for (var i = 0; i < roles.length; i++) {
                        switch (roles[i].RoleName) {
                            case 'PO Approval':
                                links.action.POApproval = true;
                                break;
                            case 'PO  Approval Admin':
                                links.action.POApprovalAdmin = true;
                                break;
                                default:links.action.POApprovalAdmin = true;
                        }
                    }
                }
            }
            return links;
        }

        function getProfile() {
            var TEUserCredentials = angular.fromJson($window.localStorage.getItem('TEUserCredentials'));
            var profileInfo = {
                name: '',
                photo: ''
            };
            if (TEUserCredentials) {
                var roles = TEUserCredentials.Roles;
                console.log(TEUserCredentials);
                profileInfo.name = $window._.startCase(TEUserCredentials.CallName) || TEUserCredentials.UserName;
                profileInfo.userName = TEUserCredentials.UserName;
                profileInfo.photo = TEUserCredentials.Photo;
                profileInfo.email = TEUserCredentials.UserEmail;
                profileInfo.userId = TEUserCredentials.UserId;
                profileInfo.isPOAdmin =true;// ($window._.findIndex(roles, { "RoleName": "PO Approval Admin" }) > -1);
            } else {
                //$state.go('login', {
                            // 'relationId': 0
                        //});
               logout();
            }
            return profileInfo;
        }

        function dateUtil(date, options) {
            if (options && options.format) {
                return $filter('date')(date, options.format);
            }
            var parsedDate = new Date(date);
            return parsedDate;
        }

        function isImagePresent(src) {
            var deferred = $q.defer();
            var image = new Image();
            image.onerror = function() {
                deferred.resolve(false);
            };
            image.onload = function() {
                deferred.resolve(true);
            };
            image.src = src;

            return deferred.promise;
        }

        function logout() {
            $window.localStorage.removeItem('TEUserCredentials');
            var authContext = new AuthenticationContext({clientId: '7b32c697-48e5-4046-a507-25acb1eb8f2a',postLogoutRedirectUri: 'http://localhost:3000/#/login'});
            authContext.logOut();
            //window.location="/";
            // var path = url.FugueLangingPage;
            // if (path) {
            //     if (path == 'login')
            //         $state.go('login');
            //     else
            //         $window.location.href = path;
            // }
        }
    }

    function currencyFormat() {
        return function(input) {
            var number = parseFloat(input)
            if (number == input) {
                var n1, n3 = angular.copy(Math.round(number));
                n3 = (Math.round(n3 * 100) / 100) + '' || '';
                n1 = n3.split('.');
                n1 = n1[0].replace(/(\d)(?=(\d\d)+\d$)/g, "$1,");
                return (n1);
            }
        }
    }
      function currencyDecimalFormat() {
        return function(input) {
          
             var dec="";
             var request="";
              request=String(input);
             
         if (request.indexOf('.') > -1) {
            
             dec=request.split('.')[1];
             if(dec.length>2)
             {
                
                dec=dec.substr(0, 2);
             }
         }
          
            var number = parseFloat(request)
           
            if (number == request) {
                //var n1, n3 = angular.copy(Math.round(number));
                var n1, n3 = angular.copy(number);
                 
               //n3=parseFloat(n3);
               n3 = n3 + '' || '';
               
              console.log(n3);  
                n1 = n3.split('.');
               
                        
                n1 = n1[0].replace(/(\d)(?=(\d\d)+\d$)/g, "$1,");
                if(dec!="00" && dec!="000" && dec!="")
                {
                      n1=n1+"."+dec;
                   
                }
                else
                {
                    //alert(dec);
                    n1=n1;
                }
               
                return (n1);
            }
        }
    }


//     function isInteger(x) {
//              return x % 1 === 0;
//             }

//  function ConvertWords()
// {
//   return function(input) {
//     if (input && isInteger(input))
//       return  toWords(input);
    
//     return input;
//     }
//   }


    var th = ['','thousand','million', 'billion','trillion'];
    var dg = ['zero','one','two','three','four', 'five','six','seven','eight','nine']; 
    var tn = ['ten','eleven','twelve','thirteen', 'fourteen','fifteen','sixteen', 'seventeen','eighteen','nineteen'];
    var tw = ['twenty','thirty','forty','fifty', 'sixty','seventy','eighty','ninety']; 


function toWords(input)
{  
    input = input.toString(); 
    input = input.replace(/[\, ]/g,''); 
    if (input != parseFloat(input)) return 'not a number'; 
    var x = input.indexOf('.'); 
    if (x == -1) x = input.length; 
    if (x > 15) return 'too big'; 
    var n = input.split(''); 
    var str = ''; 
    var sk = 0; 
    for (var i=0; i < x; i++) 
    {
        if ((x-i)%3==2) 
        {
            if (n[i] == '1') 
            {
                str += tn[Number(n[i+1])] + ' '; 
                i++; 
                sk=1;
            }
            else if (n[i]!=0) 
            {
                str += tw[n[i]-2] + ' ';
                sk=1;
            }
        }
        else if (n[i]!=0) 
        {
            str += dg[n[i]] +' '; 
            if ((x-i)%3==0) str += 'hundred ';
            sk=1;
        }


        if ((x-i)%3==1)
        {
            if (sk) str += th[(x-i-1)/3] + ' ';
            sk=0;
        }
    }
    if (x != input.length)
    {
        var y = input.length; 
        str += 'point '; 
        for (var i=x+1; i<y; i++) str += dg[n[i]] +' ';
    }
    return str.replace(/\s+/g,' ');
}


//Deepak 06-24-2017
function transactiontype() {
        return function(input) {
            console.log(input);
            if ('INR' == input) {
                return ('Domestic');
            }else{
                 return ('International');
            }
        }
    }

    // $.fn.serializeObject = function() {
    //     var o = {};
    //     var a = this.serializeArray();
    //     $.each(a, function() {
    //         if (o[this.name] !== undefined) {
    //             if (!o[this.name].push) {
    //                 o[this.name] = [o[this.name]];
    //             }
    //             o[this.name].push(this.value || '');
    //         } else {
    //             o[this.name] = this.value || '';
    //         }
    //     });
    //     return o;
    // };

    function getArrayCounts(array) {
        return (array.filter(item => item.trim() !== '')).length;
    }

    

})();
