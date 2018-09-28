angular.module('TEPOApp').config(['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', function($stateProvider, $urlRouterProvider, $ocLazyLoadProvider) {
    $ocLazyLoadProvider.config({
      debug: false,
      events: true,
    });
    $urlRouterProvider.otherwise('/SearchPO');
    $stateProvider.state('Login', {
      url: '/Login',
      templateUrl: 'views/Login/Login.html',
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['views/Login/LoginService.js', 'views/Login/Login.js'],
          })
        }
      }
    }).state('SearchPO', {
      url: '/SearchPO',
      templateUrl: 'views/PurchaseOrder/PurchaseOrder/PurchaseOrder.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/PurchaseOrder/PurchaseOrder.js', 'views/PurchaseOrder/PurchaseOrder/PurchaseOrderFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('SearchPR', {
      url: '/SearchPR',
      templateUrl: 'views/PurchaseRequest/PurchaseRequest/PurchaseRequest.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '2';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseRequest/PurchaseRequest/PurchaseRequest.js', 'views/PurchaseRequest/PurchaseRequest/PurchaseRequestFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('DraftPR', {
      url: '/DraftPR',
      templateUrl: 'views/PurchaseRequest/PurchaseRequestDraft/PurchaseRequestDraft.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '2';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseRequest/PurchaseRequestDraft/PurchaseRequestDraft.js', 'views/PurchaseRequest/PurchaseRequestDraft/PurchaseRequestFactoryDraft.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    })
    .state('ApprovedPR', {
      url: '/ApprovedPR',
      templateUrl: 'views/PurchaseRequest/ApprovedPR/ApprovedPR.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '2';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseRequest/ApprovedPR/ApprovedPR.js', 'views/PurchaseRequest/ApprovedPR/ApprovedPRFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('CreatePR', {
      url: '/CreatePR',
      templateUrl: 'views/PurchaseRequest/PRCreate/PRCreate.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '2';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Customers/Customers-style.css', 'views/PurchaseRequest/PRCreate/PRCreate.js', 'views/PurchaseRequest/PRCreate/PRCreateFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('PRupdate', {
      url: '/PRupdate',
      templateUrl: 'views/PurchaseRequest/PRupdate/PRUpdate.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '2';
        $scope.PRDiffer = false;
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Customers/Customers-style.css', 'views/PurchaseRequest/PRupdate/PRUpdate.js', 'views/PurchaseRequest/PRupdate/PRUpdateFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('PRDetail', {
      url: '/PRDetail',
      templateUrl: 'views/PurchaseRequest/PRupdate/PRUpdate.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '2';
        $scope.PRDiffer = true;
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Customers/Customers-style.css', 'views/PurchaseRequest/PRupdate/PRUpdate.js', 'views/PurchaseRequest/PRupdate/PRUpdateFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('initiatePO', {
      url: '/initiatePO',
      templateUrl: 'views/PurchaseOrder/InitiatePo/InitiatePO.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/InitiatePo/InitiatePO.js', 'views/PurchaseOrder/InitiatePo/InitiatePOFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('PORejected', {
      url: '/PORejected',
      templateUrl: 'views/PurchaseOrder/RejectedPO/RejectedPO.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/RejectedPO/RejectedPO.js', 'views/PurchaseOrder/RejectedPO/RejectedPOFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('MyPO', {
      url: '/MyPO',
      templateUrl: 'views/PurchaseOrder/ApprovedPO/ApprovedPO.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/ApprovedPO/ApprovedPO.js', 'views/PurchaseOrder/ApprovedPO/ApprovedPOFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('POUpcomming', {
      url: '/POUpcomming',
      templateUrl: 'views/PurchaseOrder/UpComingPo/UpComingPo.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/UpComingPo/UpComingPo.js', 'views/PurchaseOrder/UpComingPo/UpComingPoFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('POPending', {
      url: '/POPending',
      templateUrl: 'views/PurchaseOrder/PendingForApproval/PendingForApproval.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/PendingForApproval/PendingForApproval.js', 'views/PurchaseOrder/PendingForApproval/PendingForApprovalFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('DraftPO', {
      url: '/DraftPO',
      templateUrl: 'views/PurchaseOrder/Draft/DraftPO.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/Draft/DraftPO.js', 'views/PurchaseOrder/Draft/DraftPOFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('CreatePO', {
      url: '/CreatePO',
      templateUrl: 'views/PurchaseOrder/POCreate/POCreate.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/POCreate/POCreate.js', 'views/PurchaseOrder/POCreate/POCreateFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('UpdatePO', {
      url: '/UpdatePO',
      templateUrl: 'views/PurchaseOrder/POUpdate/POUpdate.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
        $scope.poDiffer = false;
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/POUpdate/POUpdate.js', 'views/PurchaseOrder/POUpdate/POUpdateFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('DetailPO', {
      url: '/DetailPO',
      templateUrl: 'views/PurchaseOrder/POUpdate/POUpdate.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '1';
        $scope.poDiffer = true;
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/POUpdate/POUpdate.js', 'views/PurchaseOrder/POUpdate/POUpdateFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    })
    // .state('DetailPO', {
    //   url: '/DetailPO',
    //   templateUrl: 'views/PurchaseOrder/PODetails/PODetails.html',
    //   controller: function($scope, $stateParams, $rootScope) {
    //     $scope.MainMenuTitle = '1';
    //   },
    //   resolve: {
    //     loadMyDirectives: function($ocLazyLoad) {
    //       return $ocLazyLoad.load({
    //         name: 'TEPOApp',
    //         files: ['css/Trackers/TrackersMain.css', 'css/Customers/Customers-style.css', 'views/PurchaseOrder/PODetails/PODetails.js', 'views/PurchaseOrder/PODetails/PODetailsFactory.js', ]
    //       })
    //     },
    //     authCheck: function(AuthService) {
    //       AuthService.AuthLogin()
    //     }
    //   }
    // })
    //Master ROutes Starts
    .state('plantStorage', {
      url: '/plantStorage',
      templateUrl: 'views/Masters/plantStorage/plantStorage.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '4';$scope.SubmenuMenuTitle = '4d';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/MastersStyle.css', 'views/Masters/plantStorage/plantStorageFactory.js','views/Masters/plantStorage/plantStorage.js']
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    })
    .state('poMasterApprovers', {
      url: '/poMasterApprovers',
      templateUrl: 'views/Masters/poMasterApprovers/poMasterApprovers.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '4';$scope.SubmenuMenuTitle = '4c';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/MastersStyle.css', 'views/Masters/poMasterApprovers/poMasterApproversFactory.js','views/Masters/poMasterApprovers/poMasterApprovers.js']
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    })
    .state('VendorMaster', {
      url: '/VendorMaster',
      templateUrl: 'views/Masters/VendorMaster/VendorMaster.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '4';$scope.SubmenuMenuTitle = '4a';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/MastersStyle.css', 'views/Masters/VendorMaster/VendorMasterFactory.js','views/Masters/VendorMaster/VendorMaster.js']
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    })
    .state('WBSFundCenter', {
      url: '/WBSFundCenter',
      templateUrl: 'views/Masters/WBSFundCenter/WBSFundCenter.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '4';$scope.SubmenuMenuTitle = '4b';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/MastersStyle.css', 'views/Masters/WBSFundCenter/WBSFundCenterFactory.js','views/Masters/WBSFundCenter/WBSFundCenter.js']
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('FundCenterPOManagerMap', {
      url: '/FundCenterPOManagerMap',
      templateUrl: 'views/Masters/FundCenterPOManagerMap/FundCenterPOManagerMap.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '5';
      },
      resolve: {
          loadMyDirectives: function ($ocLazyLoad) {
              return $ocLazyLoad.load({
                  name: 'TEPOApp',
                  files: ['css/MastersStyle.css', 'views/Masters/FundCenterPOManagerMap/FundCenterPOManagerMap.js', 'views/Masters/FundCenterPOManagerMap/FundCenterPOManagerMapFactory.js',]
              })
          },
          authCheck: function (AuthService) {
              AuthService.AuthLogin()
          }
      }
    }).state('FundCenterUserMap', {
      url: '/FundCenterUserMap',
      templateUrl: 'views/Masters/User-FundCenterMapping/User_FundCenterMap.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '5';
      },
      resolve: {
          loadMyDirectives: function ($ocLazyLoad) {
              return $ocLazyLoad.load({
                  name: 'TEPOApp',
                  files: ['css/MastersStyle.css', 'views/Masters/User-FundCenterMapping/User_FundCenterMap.js', 'views/Masters/User-FundCenterMapping/User_FundMapFactory.js',]
              })
          },
          authCheck: function (AuthService) {
              AuthService.AuthLogin()
          }
      }
    }).state('UserRoleMapping', {
      url: '/UserRoleMapping',
      templateUrl: 'views/Masters/UserRoleMapping/UserRoleMapping.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '5';
      },
      resolve: {
          loadMyDirectives: function ($ocLazyLoad) {
              return $ocLazyLoad.load({
                  name: 'TEPOApp',
                  files: ['css/MastersStyle.css', 'views/Masters/UserRoleMapping/UserRoleMapping.js', 'views/Masters/UserRoleMapping/UserRoleMappingFactory.js',]
              })
          },
          authCheck: function (AuthService) {
              AuthService.AuthLogin(), AuthService.PrivilageView('POUserRoleMapping');
          }
      }
    })
    
    
    .state('MyPOApprovals', {
      url: '/MyApprovals',
      templateUrl: 'views/MyApprovals/MyPOApproval/MyPOApproval.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '5';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/Trackers/TrackersMain.css', 'css/Trackers/MileStoneTracker.css', 'css/Customers/Customers-style.css', 'views/MyApprovals/MyPOApproval/MyPOApproval.js', 'views/MyApprovals/MyPOApproval/MyPOApprovalFactory.js', ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin()
        }
      }
    }).state('HSNTaxCodeMap', {
      url: '/HSNTaxCodeMap',
      templateUrl: 'views/Masters/HSNTaxCodeMap/HSNTaxCodeMap.html',
      controller: function($scope, $stateParams, $rootScope) {
        $scope.MainMenuTitle = '5';
      },
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['css/MastersStyle.css', 'views/Masters/HSNTaxCodeMap/HSNTaxCodeMapFactory.js', 'views/Masters/HSNTaxCodeMap/HSNTaxCodeMap.js' ]
          })
        },
        authCheck: function(AuthService) {
          AuthService.AuthLogin(), AuthService.PrivilageView('POHSNRatesMaster');
        }
      }
    }).state('RE350', {
      url: '/RE350',
      templateUrl: 'views/Login/Login.html',
      resolve: {
        loadMyDirectives: function($ocLazyLoad) {
          return $ocLazyLoad.load({
            name: 'TEPOApp',
            files: ['views/Login/LoginService.js', 'views/Login/Login.js'],
          })
        }
      }
    })


    //Master ROutes Ends
  }])
  .factory('sessionInjector', ['$localStorage', function($localStorage) {
    var sessionInjector = {
      request: function(config) {
        if ($localStorage.globals) {
          config.headers['Content-Type'] = 'application/json';
          config.headers['authUser'] = $localStorage.globals.currentUser.loginID;
          config.headers['authToken'] = $localStorage.globals.currentUser.AuthToken;
        } else {
          config.headers['Content-Type'] = 'application/json';
        }
        return config;
      }
    };
    return sessionInjector;
  }]).config(['$httpProvider', function($httpProvider) {
    $httpProvider.interceptors.push('sessionInjector');
  }]).run(['$location', '$rootScope', function($location, $rootScope) {
    $rootScope.$on('$stateChangeStart', function(event, toState, toParams) {
      if (toState.title) $rootScope.title = toState.title;
      else $rootScope.title = "Fugue";
    });
  }])