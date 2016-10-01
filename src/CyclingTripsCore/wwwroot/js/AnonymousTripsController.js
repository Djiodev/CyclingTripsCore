(function () {

    "use strict";

    angular.module("app-anonymousTrips")
        .controller("AnonymousTripsController", AnonymousTripsController);

    function AnonymousTripsController($http) {
        var vm = this;
        vm.trips = [];

        $http.get("/api/alltrips/")
            .then(function (response) {
                angular.copy(response.data, vm.trips);
            }, function (error) {
                vm.errorMessage = "Failed to load the trips."
            });
    }


})();