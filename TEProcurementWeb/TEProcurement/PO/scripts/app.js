//var heads={headers:{'Content-Type': 'application/json'}};
var heads = "";
angular.module('TEPOApp', ['ngStorage', 'ngAnimate', 'oc.lazyLoad', 'ui.router', 'ui.bootstrap', 'ngSanitize', 'ngGrid', 'textAngular', 'ui.select', 'ui.bootstrap.datetimepicker', 'angular.filter', 'infinite-scroll', 'ngFileUpload', 'ngPrint', 'checklist-model'])
    .controller('TEPOAppCtrl', ['$scope', '$rootScope', '$localStorage', '$sessionStorage', '$location', '$http', 'AuthService', '$uibModal', '$filter', 'AlertMessages', function($scope, $rootScope, $localStorage, $sessionStorage, $location, $http, AuthService, $uibModal, $filter, AlertMessages) {
        $scope.adalidtoken = sessionStorage.getItem('adal.idtoken');  
        $scope.UserDetails = $localStorage.globals;       
        $scope.justShow=false;     
        // $sessionStorage.server_url = "http://182.18.177.27/portfolio/api"; //This is to Call API's in are included in portfolio
        // $sessionStorage.purchaseorder_url = "http://182.18.177.27/PO/api"; //This is used for Purchase Order WEB API services
        // $sessionStorage.purchaseorder_url_mvc = "http://182.18.177.27/PO"; //This is used for Purchase Order MVC services
        
        $sessionStorage.server_url = "http://13.228.221.95/portfolio_ProductionTest/api"; //This is to Call API's in are included in portfolio
        $sessionStorage.purchaseorder_url = "http://13.228.221.95/TEPO/api"; //This is used for Purchase Order WEB API services
        $sessionStorage.purchaseorder_url_mvc = "http://13.228.221.95/TEPO"; //This is used for Purchase Order MVC services
        $sessionStorage.server_ip = "http://13.228.221.95";
        $sessionStorage.server_url_lead = "http://13.228.221.95/lead_ProductionTest/api"; //This is to Call API's in are included in portfolio
        $sessionStorage.PortfolioAPI = "http://13.228.221.95/portfolio_ProductionTest/api/";

        // $sessionStorage.server_url = "http://106.51.8.218/portfolio_ProductionTest/api"; //This is to Call API's in are included in portfolio
        // $sessionStorage.purchaseorder_url = "http://106.51.8.218/TEPO/api"; //This is used for Purchase Order WEB API services
        // $sessionStorage.purchaseorder_url_mvc = "http://106.51.8.218/TEPO"; //This is used for Purchase Order MVC services
        // $sessionStorage.server_ip = "http://106.51.8.218";
        // $sessionStorage.server_url_lead = "http://106.51.8.218/lead_ProductionTest/api"; //This is to Call API's in are included in portfolio
        
        if ($localStorage.globals) {
            //var heads={headers:{'authUser':$localStorage.globals.currentUser.loginID,'authToken':$localStorage.globals.currentUser.AuthToken}};
        }
        $scope.hideScreen = function(){
            $(".modal-backdrop.in").css("display","none");
             //.modal-backdrop {display:none;}
        }
        $scope.diabledef = function($event) {
            $event.preventDefault();
        }
        
         $(document).on('show.bs.modal', '.modal', function () {
            var zIndex = 1040 + (10 * $('.modal:visible').length);
            $(this).css('z-index', zIndex);
            setTimeout(function () {
                $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
            }, 0);
        });
       
        $http.post($sessionStorage.server_url + '/TEProjectAPI/GetAllTEProjects', heads).success(function(response) {
            $sessionStorage.portfolioMain = response.result;
            $scope.projects = $sessionStorage.portfolioMain;
        });
        $scope.Logout = function() {
            AuthService.Logout();
        }       
        $scope.CheckPrivilage = function(a, b) {
            AuthService.CheckPrivilage(a, b, function(response) {
                $scope.response = response;
            });
            return $scope.response;
        }

        $rootScope.provideAccess = function(fb,v){
          var res =  $scope.CheckPrivilage(fb,v);
          return res;
        }

        $scope.SaveAudit = function(a) {
            AuthService.AuditStorage(a);
        }
    }])
    .service('AlertMessages', ['$rootScope', function($rootScope) {
        this.alertPopup = function(data) {
            "0" == data.errorcode ? data.errorcode = "success" : data.errorcode = "danger";
            $rootScope.alerts = [{
                type: data.errorcode,
                msg: data.errormessage
            }];
            $rootScope.autoHide();
        }
    }])
    .service('LoadingLoad', ['$rootScope', '$timeout', function($rootScope, $timeout) {
        this.ShowLoad = function() {
            $rootScope.ShowLoading = true;
            $timeout(function() {
                $rootScope.ShowLoading = false;
            }, 500);
        }
    }])
    .filter('number', function() {
        return function(input, Deci) {
            input = Number(input);
            if (!isNaN(input)) {
                input = input.toFixed(Deci);
                var result = input.toString().split('.');
                var lastThree = result[0].substring(result[0].length - 3);
                var otherNumbers = result[0].substring(0, result[0].length - 3);
                if (otherNumbers != '')
                    lastThree = ',' + lastThree;
                var output = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;

                if (result.length > 1) {
                    output += "." + result[1];
                }
                return output;
            } else return input;
        }
    })
    .filter('trustAsResourceUrl', ['$sce', function($sce) { return function(val) { return $sce.trustAsResourceUrl(val); }; }])
    .factory('LocationGoogleAPI', function($sessionStorage, $http) {
        var result = {};
        result.getList = function(val) { return $http.get('https://maps.googleapis.com/maps/api/geocode/json', { params: { address: val, sensor: false } }).then(function(response) { return response.data.results.map(function(item) { return item.formatted_address; }); }); }
        return result;
    })
    
    .filter('numnone', function() {
        return function(input) {
            result = Math.round(input);
            return result;
        }
    })
    .filter('numnoneFlot', function() {
        return function(input) {
            result = Math.round(input*100)/100;
            return result;
        }
    })
    
    .filter('SetNegitiveFilter', function($filter) {
        return function(input) {
             input=Math.round(input);
            var numberFilter = $filter('number');
            if (typeof input === 'number') {
                if (input % 1 === 0) {
                    if (parseFloat(input) < 0) {
                        result = numberFilter((input - (input * 2)), 0);
                        result = "<span class=\"text-danger\">(" + result + ")</span>";
                    } else { result = numberFilter(input, 0); }
                } else { result = 0; }
            } else { result = 0; }
            return result;
        }
    })
    .filter('SetNegitiveFilterCr', function($filter) {
        return function(input) {
            input=Math.round(input);
            var numberFilter = $filter('number');
            if (typeof input === 'number') {
                if (input % 1 === 0) {
                    input = input / 10000000;
                    if (parseFloat(input) < 0) {
                        result = numberFilter((input - (input * 2)), 2);
                        result = "<span class=\"text-danger\">(" + result + ")</span>";
                    } else { result = numberFilter(input, 2); }
                } else { result = 0; }
            } else { result = 0; }
            return result;
        }
    })

    .filter('AmountWords', function($filter) {
        return function(e) {
            var a = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine ', 'Ten ', 'Eleven ', 'Twelve ', 'Thirteen ', 'Fourteen ', 'Fifteen ', 'Sixteen ', 'Seventeen ', 'Eighteen ', 'Nineteen '];
            var b = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
            var Num = parseInt(e);
            if (Num.length > 9) return 'Overflow';
            var n = ('000000000' + Num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
            if (!Num) return;
            Num = '';
            Num += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'Crore' + (n[1] > 1 ? 's ' : ' ') : '';
            Num += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'Lakh' + (n[2] > 1 ? 's ' : ' ') : '';
            Num += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'Thousand ' : '';
            Num += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'Hundred ' : '';
            Num += (n[5] != 0) ? ((Num != '') ? 'And ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + 'Only ' : 'Only';
            return Num;
        }
    })
    .filter('dateDiff', function($filter) {
        return function(secondDate) {
            var CurrentDates = new Date();
            secondDate = new Date(secondDate);
            var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds    
            //var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime())/(oneDay)));
            var diffDays = Math.round((CurrentDates.getTime() - secondDate.getTime()) / (oneDay));
            if (diffDays <= 0) { diffDays = 0; }
            return diffDays;

        }
    })
    .filter('split', function() {
        return function(input, splitChar, splitIndex) {
            return input.split(splitChar)[splitIndex];
        }
    })
    .directive('autoActive', ['$location', function($location) {
        return {
            restrict: 'A',
            scope: false,
            link: function(scope, element) {
                function setActive() {
                    var path = $location.path();
                    if (path) {
                        angular.forEach(element.find('li'), function(li) {
                            var anchor = li.querySelector('a');
                            if (anchor.href.match('#' + path + '(?=\\?|$)')) {
                                angular.element(li).addClass('active');
                            } else {
                                angular.element(li).removeClass('active');
                            }
                        });
                    }
                }
                setActive();
                scope.$on('$locationChangeSuccess', setActive);
            }
        }
    }])
    .directive('toggleClass', function() {
        return {
            restrict: 'A',
            link: function(scope, element, attrs) {
                element.bind('click', function() {
                    element.toggleClass(attrs.toggleClass);
                });
            }
        };
    })
    .filter('htmlToPlaintext', function() {
        return function(text) {
            return text ? String(text).replace(/<[^>]+>/gm, '') : '';
        };
    })
    .filter('roundNum', function($filter) {
        return function(e) {
            var floatRegex = /^-?\d+(?:[.,]\d*?)?$/;
            if (floatRegex.test(e)) { e = parseFloat(e); return e.toFixed(2); } else return 0;
        }

    })


function getDateTime() {
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    if (month.toString().length == 1) {
        var month = '0' + month;
    }
    if (day.toString().length == 1) {
        var day = '0' + day;
    }
    if (hour.toString().length == 1) {
        var hour = '0' + hour;
    }
    if (minute.toString().length == 1) {
        var minute = '0' + minute;
    }
    if (second.toString().length == 1) {
        var second = '0' + second;
    }
    var dateTime = day + '-' + month + '-' + year + ' ' + hour + ':' + minute + ':' + second;
    return dateTime;
}

function selectedKey(e, n, t) {
    for (var ii = 0; ii < e.length; ii++) { if (e[ii][n] == t) return e[ii] };
}

function selectedKeyIndex(e, n, t) {
    for (var jj = 0; jj < e.length; jj++) { if (e[jj][n] == t) return jj; }
}
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
Array.prototype.ArrayTriming = function(val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] === val) {
            this.splice(i, 1);
            i--;
        }
    }
    return this;
}
Array.prototype.getUnique = function() {
    var u = {},
        a = [];
    for (var i = 0, l = this.length; i < l; ++i) {
        if (u.hasOwnProperty(this[i])) {
            continue;
        }
        a.push(this[i]);
        u[this[i]] = 1;
    }
    return a;
}

function getArrayCounts(array) {
    return (array.filter(item => item.trim() !== '')).length;
}

function isempty(x) { if ((String(x)).trim() !== "") return true; }

function getArrayCounts1(array) {
    var res = array.filter(isempty);
    return res.length;
}

function getArrayCountsofZero(array) {
    return (array.filter(item => item.trim() !== 0)).length;
}
File.prototype.convertToBase64 = function(callback) {
    var reader = new FileReader();
    reader.onload = function(e) {
        callback(e.target.result)
    };
    reader.onerror = function(e) {
        callback(null);
    };
    reader.readAsDataURL(this);
}
Date.prototype.addDays = function(days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};
Number.prototype.isFloat = function() {
    return (this % 1 != 0);
}
Date.prototype.addMonths = function(m) {
    var d = new Date(this);
    var years = (m / 12);
    var months = m - (years * 12);
    if (years) d.setFullYear(d.getFullYear() + years);
    if (months) d.setMonth(d.getMonth() + months);
    return d;
}

function isFloat(n) {
    var er = /^-?[0-9]+$/;
    return er.test(n);
}

function isDate(val) {
    var d = new Date(val);
    return !isNaN(d.valueOf());
}

function sumArray(input) {

    if (toString.call(input) !== "[object Array]")
        return false;

    var total = 0;
    for (var i = 0; i < input.length; i++) {
        if (isNaN(input[i])) {
            continue;
        }
        total += Number(input[i]);
    }
    return total;
}