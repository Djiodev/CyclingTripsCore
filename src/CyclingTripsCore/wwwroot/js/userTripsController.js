(function () {

	"use strict";

	angular.module("app-userTrips")
		.controller("userTripsController", userTripsController);

	function userTripsController($http) {

	    var vm = this;
	    vm.trips = [];
	    vm.newTrip = {};
	    vm.errorMessage = "";


	    var getTrips = function (response) {
	        angular.copy(response.data, vm.trips);
	    };

	    var errTrips = function (error) {
	        vm.errorMessage = "Failed to load data: " + error.message;
	    };

	    var tripAdd = function (response) {
	        vm.trips.push(response.data);
	        vm.newTrip = {};
	    };

	    var tripAddErr = function (error) {
	        vm.errorMessage = "Failed to save new trip";
	    };

	    $http.get("/api/trips")
            .then(getTrips, errTrips);

	    vm.addTrip = function () {
	        vm.errorMessage = "";
	        $http.post("/api/trips", vm.newTrip)
	            .then(tripAdd, tripAddErr);
	    };	 

	    vm.deleteTrip = function (trip) {
	        vm.errorMessage = "";
	        $http.delete("/api/trips/" + trip.name)
	            .then(function () {
	                vm.trips.pop(trip);
	            },
                function () {
                    vm.errorMessage = "Failed to delete the trip."
                });
	    }
	}

})();