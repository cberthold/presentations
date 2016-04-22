(function () {
    'use strict';


    // Demonstrate how to register services
    // In this case it is a simple value service.
    angular.module('app.services', [])

    .value('version', '0.1')

    .factory('randomPersonService', ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {
        var data = [];

        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16).substring(1);
        }

        var guid = function () {
            return s4() + s4() + '-' + s4() + '-' + s4() +
              '-' + s4() + '-' + s4() + s4() + s4();
        };

        var sendSaleBroadcast = function () {
            $rootScope.$broadcast('handleNewSale');
        };
        
        var getAllSales = function () {
            var deferred = $q.defer();

            //var randomId = guid();
            //var url = 'http://api.randomuser.me/?seed=' + randomId;
            var url = "/api/randomsale/all";
            $http.get(url)
            .success(function (d) {
                // get the user from the result
                var sales = d.payload;

                data.clear();

                angular.forEach(sales,
                    function (value) {
                        data.push(value);
                    });

                //console.log(user);
                // send broadcast of sale to other angular app controllers/directives/etc
                sendSaleBroadcast();
                // resolve out our promise
                deferred.resolve(data);
            });
            return deferred.promise;
        };

        var getRandomPerson = function () {
            var deferred = $q.defer();

            //var randomId = guid();
            //var url = 'http://api.randomuser.me/?seed=' + randomId;
            var url = "/api/randomsale/random";
            $http.get(url)
            .success(function (d) {
                // get the user from the result
                var sales = d.payload;

                data.clear();

                angular.forEach(sales,
                    function (value) {
                        data.push(value);
                    });
                
                //console.log(user);
                // send broadcast of sale to other angular app controllers/directives/etc
                sendSaleBroadcast();
                // resolve out our promise
                deferred.resolve(data);
            });
            return deferred.promise;
        };

        return {
            data: data,

            getAllSales: getAllSales,
            getRandomPerson: getRandomPerson,
            guid: guid
        };
    }]);

    angular.module('d3', [])
      .factory('d3Service', ['$document', '$q', '$rootScope',
        function ($document, $q, $rootScope) {
            var d = $q.defer();

            var allLoaded = false;
            var script1Loaded = false;
            var script2Loaded = false;
            var script3Loaded = false;

            function onScript1Load() {
                script1Loaded = true;
                //cdnjs.cloudflare.com/ajax/libs/topojson/1.1.0/topojson.min
                onD3ScriptLoad();
            }

            function onScript2Load() {
                script2Loaded = true;
                //cdnjs.cloudflare.com/ajax/libs/topojson/1.1.0/topojson.min
                onD3ScriptLoad();
            }

            function onScript3Load() {
                script3Loaded = true;
                //cdnjs.cloudflare.com/ajax/libs/topojson/1.1.0/topojson.min
                onD3ScriptLoad();
            }

            function onD3ScriptLoad() {

                if (!script1Loaded || !script2Loaded || !script3Loaded) return;

                allLoaded = true;

                // Load client in the browser
                $rootScope.$apply(function () { d.resolve(window.d3); });
            }
            // Create a script tag with d3 as the source
            // and call our onScriptLoad callback when it
            // has been loaded
            var scriptTag = $document[0].createElement('script');
            scriptTag.type = 'text/javascript';
            scriptTag.async = true;
            scriptTag.src = '//d3js.org/d3.v3.min.js';
            scriptTag.onreadystatechange = function () {
                if (this.readyState == 'complete') onScript1Load();
            }
            scriptTag.onload = onScript1Load;

            var scriptTag2 = $document[0].createElement('script');
            scriptTag2.type = 'text/javascript';
            scriptTag2.async = true;
            scriptTag2.src = '//cdnjs.cloudflare.com/ajax/libs/topojson/1.1.0/topojson.min.js';
            scriptTag2.onreadystatechange = function () {
                if (this.readyState == 'complete') onScript2Load();
            }
            scriptTag2.onload = onScript2Load;

            var scriptTag3 = $document[0].createElement('script');
            scriptTag3.type = 'text/javascript';
            scriptTag3.async = true;
            scriptTag3.src = '/scripts/datamaps.usa.min.js';
            scriptTag3.onreadystatechange = function () {
                if (this.readyState == 'complete') onScript3Load();
            }
            scriptTag3.onload = onScript3Load;

            var s = $document[0].getElementsByTagName('body')[0];
            s.appendChild(scriptTag);
            s.appendChild(scriptTag2);
            s.appendChild(scriptTag3);

            return {
                d3: function () {

                    if (allLoaded)
                        d.resolve(window.d3);
                    return d.promise;
                }
            };
        }]);
}());