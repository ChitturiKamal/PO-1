(function() {
    'use strict';

    angular
        .module('procurementApp')
        .config(routerConfig);

    /** @ngInject */
    function routerConfig($stateProvider, $urlRouterProvider,$locationProvider) {
        $locationProvider.html5Mode(true).hashPrefix('!');
        $stateProvider
        .state('login', {
            url: '/',
            templateUrl: 'app/user/login.html',
            controller: 'UserController as vm'
            //requireADLogin: true
        })
        .state('main', {
            abstract: true,
            url: '/main',
            templateUrl: 'app/main/main.html',
            controller: 'MainController as vm'
        })
        // .state('main.poListAll', {
        //     url: '/po/All',
        //     // loaded into ui-view of parent's template
        //    // abstract: true,
        //     templateUrl: 'app/po/po.html',
        //     controller: 'POListController as vm'
        // })
        .state('main.poList', {
            url: '/po/:status',
            // loaded into ui-view of parent's template
           // abstract: true,
            templateUrl: 'app/po/po.html',
            controller: 'POListController as vm'
        })
//STARTED FROM HERE
        .state('main.poCreate', {
            url: '/POCreate/:status',
            // loaded into ui-view of parent's template
           // abstract: true,
            templateUrl: 'app/POCreate/POCreate.html',
            controller: 'POListController as vm'
        })
        .state('main.VendorMaster', {
            url: '/VendorMaster',
            // loaded into ui-view of parent's template
            templateUrl: 'app/vendorMaster/vendorMaster.html',
            controller: 'vendorMasterCtrl as vm'
            //controller: 'POVendorController as vm'
        })

        .state('main.WBSFundCenter', {
            url: '/WBSFundCenter',
            // loaded into ui-view of parent's template
            templateUrl: 'app/WBSFundCenter/WBSFundCenter.html',
            controller: 'WBSFundCenterCtrl as vm'
        })

        .state('main.poMasterApproversMine', {
            url: '/poMasterApproversMine',
            // loaded into ui-view of parent's template
            templateUrl: 'app/poMasterApproversMine/poMasterApproversMine.html',
            controller: 'poMasterApproverCtrl as vm'
        })
        
        .state('main.POApprovalMaster', {
            url: '/POApprovalMaster',
            // loaded into ui-view of parent's template
            templateUrl: 'app/POApprovalMaster/POApprovalMaster.html',
            controller: 'POVendorController as vm'
        })
        
        .state('main.fundCenterMapping', {
            url: '/fundCenterMapping',
            // loaded into ui-view of parent's template
            templateUrl: 'app/fundCenterMapping/fundCenterMapping.html',
            controller: 'POVendorController as vm'
        })

        .state('main.commitmentItem', {
            url: '/commitmentItem',
            // loaded into ui-view of parent's template
            templateUrl: 'app/commitmentItem/commitmentItem.html',
            controller: 'POVendorController as vm'
        })
        
        .state('main.internalOrderMaster', {
            url: '/internalOrderMaster',
            // loaded into ui-view of parent's template
            templateUrl: 'app/internalOrderMaster/internalOrderMaster.html',
            controller: 'POVendorController as vm'
        })

        .state('main.vendorCategoryMaster', {
            url: '/vendorCategoryMaster',
            // loaded into ui-view of parent's template
            templateUrl: 'app/vendorCategoryMaster/vendorCategoryMaster.html',
            controller: 'POVendorController as vm'
        })

//internalOrderMaster

        // .state('main.poList.detail', {
        //     url: '/detail',
        //     // loaded into ui-view of parent's template
        //     templateUrl: 'app/po/poDetails.html'
        // })
       .state('main.poList.poFinalize', {
            url: '/detail',
            // loaded into ui-view of parent's template
            templateUrl: 'app/po/poFinalize.html'
        })
        .state('main.poReport', {
            url: '/Report',
            // loaded into ui-view of parent's template
            templateUrl: 'app/poReport/poReport.html',
            controller: 'POReportController as vm'
        })

        .state('main.poVendor', {
            url: '/Vendor',
            // loaded into ui-view of parent's template
            templateUrl: 'app/vendor/vendor.html',
            controller: 'POVendorController as vm'
        })
		
		.state('main.poTerms', {
            url: '/Terms',
            // loaded into ui-view of parent's template
            templateUrl: 'app/poTerms/t&c.html',
            controller: 'POTermsController as vm'
        })
		
		.state('main.poTerms.DetailTerms', {
            url: '/DetailTerms',
            // loaded into ui-view of parent's template
            templateUrl: 'app/poTerms/Detailst&c.html'
        })
		
		 .state('main.poTerms.editTerms', {
            url: '/editTerms',
            // loaded into ui-view of parent's template
            templateUrl: 'app/poTerms/t&cEdit.html',
            controller: 'EditController as vm'
        })
		
		 .state('main.poTerms.CreateTerms', {
            url: '/CreateTerms',
            // loaded into ui-view of parent's template
            templateUrl: 'app/poTerms/t&cCreate.html',
            controller: 'CreateController as vm'
        })
		
		
        // $urlRouterProvider.otherwise('/main/po/All');

        $urlRouterProvider.otherwise('/');

    }

})();

