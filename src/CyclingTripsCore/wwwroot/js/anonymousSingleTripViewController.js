(function () {

    "use strict";

    angular.module("app-anonymousTrips")
        .controller("anonymousSingleTripViewController", anonymousSingleTripViewController);

    function anonymousSingleTripViewController($http, $routeParams, $filter) {
        var vm = this;
        vm.tripName = $routeParams.tripName;
        vm.username = $routeParams.username;
        vm.stops = [];
        vm.comments = [];
        vm.newComment = "";
        vm.errorMessage = "";
        if (vm.username === "Djio") {
            vm.src = "../images/Djio.jpg";
        }
        else {
            vm.src = "../images/User.jpg";
        }

        

        $http.get("/api/alltrips/" + vm.tripName + "/" + vm.username + "/stops")
            .then(function (response) {
                angular.copy(response.data, vm.stops);
                _showMap(vm.stops);

            },
            function (err) {
                vm.errorMessage = "Failed to load stops";
            });

        setTimeout(
            $http.get("/api/alltrips/" + vm.tripName + "/" + vm.username + "/comments")
                .then(function (response) {
                    angular.copy(response.data, vm.comments);
                    for (var i = 0; i < vm.comments.length; i++) {
                        if (vm.comments[i].username === "Djio") {
                            vm.comments[i].src = "../images/Djio.jpg";
                        }
                        else {
                            vm.comments[i].src = "../images/User.jpg";
                        }
                    }
                },
                function (err) {
                    vm.commentsErrorMessage = "Failed to load comments";
                }), 1000);


        function _showMap(stops) {

            vm.map = new GMaps({
                div: "#anonymousMap",
                lat: stops[0].latitude,
                lng: stops[0].longitude,
                zoom: 8
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

            vm.setMapCenter = function (stop) {
                vm.map.setCenter({
                    lat: stop.latitude,
                    lng: stop.longitude
                });
                vm.map.setZoom(8);
            };


        }

        vm.addNewComment = function () {
            $http.post("/api/alltrips/" + vm.tripName + "/" + vm.username + "/comments", JSON.stringify(vm.newComment))
                .then(function (response) {
                    vm.comments.push(response.data);
                    vm.newComment = "";
                }, function (err) {
                    alert(vm.tripName + ", " + vm.username + ", " + vm.newComment);
                    if (err.status == 401) {
                        vm.newCommentErrMessage = err.statusText + ". Please register first!";
                    }
                    else {
                        vm.newCommentErrMessage = "Failed to add new comment";
                    }
                });
        }
    }
})();