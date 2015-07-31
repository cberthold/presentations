(function () {
    'use strict';


    // Google Analytics Collection APIs Reference:
    // https://developers.google.com/analytics/devguides/collection/analyticsjs/

    angular.module('app.controllers', [])

        // Path: /
        .controller('HomeCtrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
            $scope.$root.title = 'AngularJS SPA Template for Visual Studio';
            //$scope.$on('$viewContentLoaded', function () {
            //    $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            //});
        }])

        // Path: /about
        .controller('SalesController', ['$rootScope', '$scope', '$location', '$window', 'randomPersonService', '$interval', function ($rootScope, $scope, $location, $window, randomPersonService, $interval) {
            $scope.data = [];

            var randomSaleTimer;

            $scope.addSale = function()
            {   
                randomPersonService.getRandomPerson().then(function (data) {
                    $scope.data = data;

                    if (!data || !data.length) return;

                    var sale = data[0];
                    var user = sale.user;

                    var text = 'New sale $' + sale.amount + ' for: ' + user.name.first + ' ' + user.name.last + ' Location: ' + user.location.city + ', ' + user.location.state + ' made by: ' + sale.createdBy;
                    //toast notification here
                    toastr.info(text);
                });
            };

            $scope.refreshSales = function () {
                // load up existing sales
                randomPersonService.getAllSales().then(function (data) {
                    $scope.data = data;
                });
            };

            var getRandomInt = function (min, max) {
                return Math.floor(Math.random() * (max - min + 1)) + min;
            };

            var turnOnAutoSales = function()
            {
                if (angular.isDefined(randomSaleTimer)) return;
                // generate a random interval
                var timerInt = getRandomInt(750, 1500);
                // create our timer interval
                randomSaleTimer = $interval(function () {
                    $scope.addSale();
                }, timerInt);
            };

            var turnOffAutoSales = function()
            {
                if (!angular.isDefined(randomSaleTimer)) return;
                // cancel timer and reset timer to undefined
                $interval.cancel(randomSaleTimer);
                randomSaleTimer = undefined;
            };
            

            $scope.toggleAutoSales = function () {
                if (angular.isDefined(randomSaleTimer))
                    turnOffAutoSales();
                else
                    turnOnAutoSales();
            };

            // start by refreshing current sales
            $scope.refreshSales();
            
        }])

        // Path: /about
        .controller('AboutCtrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
            $scope.$root.title = 'AngularJS SPA | About';
            //$scope.$on('$viewContentLoaded', function () {
            //    $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            //});
        }])

        // Path: /login
        .controller('LoginCtrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
            $scope.$root.title = 'AngularJS SPA | Sign In';
            // TODO: Authorize a user
            $scope.login = function () {
                $location.path('/');
                return false;
            };
            //$scope.$on('$viewContentLoaded', function () {
            //    $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            //});
        }])

        // Path: /error/404
        .controller('Error404Ctrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
            $scope.$root.title = 'Error 404: Page Not Found';
            //$scope.$on('$viewContentLoaded', function () {
            //    $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            //});
        }]);

}());