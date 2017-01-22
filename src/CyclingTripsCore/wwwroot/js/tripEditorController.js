(function () {

    "use strict";

    angular.module("app-userTrips")
        .controller("tripEditorController", tripEditorController);


    function tripEditorController($scope, $routeParams, $http, $filter) {
        var vm = this;
        vm.newStop = {};
        vm.tripName = $routeParams.tripName;

        vm.stops = [];
        vm.errorMessage = "";
        vm.comments = [];

        var showStops = function () {
            $http.get("/api/trips/" + vm.tripName + "/stops")
                .then(function (response) {
                    angular.copy(response.data, vm.stops);
                    _showMap(vm.stops);
                }, function (error) {
                    vm.errorMessage = "Failed to load stops";
                });
        };

        setTimeout(
            $http.get("/api/trips/" + vm.tripName + "/comments")
                .then(function (response) {
                    angular.copy(response.data, vm.comments);
                    console.log(vm.comments[0]);
                    },
                    function (err) {
                        vm.commentsErrorMessage = "Failed to load comments";
                    }), 1000);


        showStops();

        function _showMap(stops) {

            var currentLat = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    lng: item.longitude,
                    title: item.location
                }
            });
            var lat = 41.902783;
            var lng = 12.496366;
            if (stops.length > 0) {
                lat = stops[0].latitude;
                lng = stops[0].longitude;
            }

            vm.map = new GMaps({
                div: "#map",
                lat: lat,
                lng: lng,
                zoom: 7
            });
            var path = [];
            angular.forEach(stops, function (item) {
                path.push(new google.maps.LatLng(item.latitude, item.longitude));
                vm.map.addMarker({
                    lat: item.latitude,
                    lng: item.longitude,
                    title: item.location,
                    infoWindow: {
                        content:
							'<div class="stopWindow">' +
							'<h4>' + item.location + ', ' + $filter('date')(item.arrival, 'dd MMM yyyy') + '</h4>' +
                            '<a ng-click="vm.infoWindowDelete()" class="btn btn-danger btn-sm">Delete</a>' +
							'</div>'+
                            '<div class="arrival">' +
                            '<div class="month">' + $filter('date')(item.arrival, 'MMM') + '</div>' +
                            '<div class="day">' + $filter('date')(item.arrival, 'd') + '</div>' +
                            '</div>' +
                            '<div class="location">{{ stop.location }}</div>' +
                            '<div class="delete-btn">' +
                                '<a ng-click="deleteStop(stop)" class="btn btn-danger btn-sm">Delete</a>' +
                            '</div>'
                    }
                });
            });

            vm.map.drawPolyline({
                path: path,
                strokeColor: "##FF0000",
                strokeOpacity: 1.0,
                strokeWeight: 2,
                geodesic: true,
            });


        }

        vm.infoWindowDelete = function () {
            alert("InfoWindow Delete!");
        };

        $scope.centerStop = function (stop) {
            vm.map.setCenter({
                lat: stop.latitude,
                lng: stop.longitude
            });
            vm.map.setZoom(9);
        };

        vm.addStop = function () {
            $http.post("/api/trips/" + vm.tripName + "/stops", vm.newStop)
                .then(function (response) {
                    vm.stops.push(response.data);
                    showStops();
                    vm.newStop = {};
                }, function (err) {
                    vm.errorMessage = "failed to add the new stop";

                });
        };

        $scope.deleteStop = function (stopToDelete) {
            $http.delete("/api/trips/" + vm.tripName + "/stops/" + stopToDelete.location)
                .then(function (response) {
                    vm.stops.pop(stopToDelete);
                    stopToDelete = {};
                    showStops();
                },
                function (err) {
                    vm.errorMessage = "Failed to delete the Stop.";
                });

        }

        vm.deleteComment = function (commentToDelete) {
            var id = commentToDelete.id;
            $http.delete("/api/trips/" + vm.tripName + "/comments/" + id)
                .then(function (response) {
                    vm.comments.pop(commentToDelete);
                    commentToDelete = {};
                },
                function (err) {
                    vm.errorMessage = "Failed to delete the comment.";
                });
        }

        vm.getCoord = function () {
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ "address": vm.newStop.location }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK && results.length > 0) {
                    var location = results[0].geometry.location;
                    vm.newStop.latitude = results[0].geometry.location.lat();
                    vm.newStop.longitude = results[0].geometry.location.lng();
                    vm.map.panTo(location);
                    vm.map.setZoom(12);
                    setTimeout(vm.addStop(), 500);
                }
            });

        }

    }

})();