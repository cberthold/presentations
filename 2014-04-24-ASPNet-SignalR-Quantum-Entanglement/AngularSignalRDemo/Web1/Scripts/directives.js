(function () {
    'use strict';


    angular.module('app.directives', ['d3'])

        .directive('appVersion', ['version', function (version) {
            return function (scope, elm, attrs) {
                elm.text(version);
            };
        }])

        .directive('d3map', ['d3Service', function (d3Service) {

            var height = 500;
            var width = 800;
            var zipArray0;

            return {
                restrict: 'EA',
                scope: {},
                controller: function ($scope, $element, $attrs, randomPersonService) {
                    var el = $element[0];
                    var projection;

                    function updateData() {
                        var data = randomPersonService.data;

                        change(data);
                    };

                    function change(zipArray1) {
                        zipArray0 = zipArray1;
                        d3Service.d3().then(function (d3) {
                            // Select old canvases to remove after fade.
                            var canvas0 = d3.selectAll("canvas");

                            // Add a new canvas, initially with opacity 0, to show the new zipcodes.
                            var canvas1 = d3.select(el).select("svg");

                            for (var i = 0, n = zipArray1.length; i < n; ++i) {
                                var sale = zipArray1[i];
                                var user = sale.user;
                                var color = "black";
                                switch (sale.createdBy)
                                {
                                    case "Alice":
                                        color = "aqua";
                                        break;
                                    case "Bob":
                                        color = "brown";
                                        break;
                                    default:
                                        color = "green";
                                }

                                var zipcode = user.location.zip;
                                var lat = sale.latitude;
                                var long = sale.longitude;
                                var p = projection([+sale.longitude, +sale.latitude]);

                                if (p) {
                                    var x = Math.round(p[0]), y = Math.round(p[1]);

                                    var content =
                                        'User:' + user.name.first + ' ' + user.name.last + '<br/>' +
                                        'Location: ' + user.location.city + ', ' + user.location.state + '<br/>' +
                                        'Sale $' + sale.amount + '<br/>' +
                                        'Sales Rep: ' + sale.createdBy + '<br/>';

                                    var title =
                                        'Sale in ' + user.location.city + ', ' + user.location.state;


                                    canvas1.append("circle")
                                        .style("fill", color)
                                      .attr("transform", function (d2) { return "translate(" + x + "," + y + ")"; })
                                      .attr("r", function (d, i) { return 4; })
                                      .attr("data-toggle", "tooltip")
                                      .attr('data-content', content)
                                      .attr("title", title);
                                }

                            }


                            // Use a transition to fade-in the new canvas.
                            // When this transition finishes, remove the old canvases.
                            canvas1.transition()
                                .duration(350)
                                .style("opacity", 1)
                                .each("end", function () { canvas0.remove(); });
                        });
                    };

                    d3Service.d3().then(function (d3) {

                        projection = d3.geo.albersUsa()
                            .scale(1070)
                            .translate([width / 2, height / 2]);

                        var path = d3.geo.path()
                            .projection(projection);

                        var svg = d3.select($element[0]).append("svg")
                            .attr("width", width)
                            .attr("height", height);

                        svg.append("rect")
                            .attr("class", "background")
                            .attr("width", width)
                            .attr("height", height);

                        var g = svg.append("g");

                        d3.json("/content/us.json.txt", function (error, us) {
                            g.append("g")
                                .attr("id", "states")
                              .selectAll("path")
                                .data(topojson.feature(us, us.objects.states).features)
                              .enter().append("path")
                                .attr("d", path);

                            g.append("path")
                                .datum(topojson.mesh(us, us.objects.states, function (a, b) { return a !== b; }))
                                .attr("id", "state-borders")
                                .attr("d", path);
                        });

                    });


                    $scope.$on('handleNewSale', function () {
                        updateData();
                    });

                }

            };
        }]);
}());