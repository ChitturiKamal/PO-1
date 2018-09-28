(function() {
  'use strict';

  angular
    .module('procurementApp')
    .run(runBlock);

  /** @ngInject */
  function runBlock($log) {

    $log.debug('runBlock end');
  }

})();
