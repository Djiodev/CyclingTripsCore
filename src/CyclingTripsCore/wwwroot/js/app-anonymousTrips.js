(function () {

    "use strict";

    angular.module("app-anonymousTrips", ["ngRoute"])
        .config(function ($routeProvider) {

            $routeProvider.when("/", {
                controller: "AnonymousTripsController",
                controllerAs: "vm",
                templateUrl: "/views/anonymousTripsView.html"
            })
            .when("/:tripName/:username", {
                controller: "anonymousSingleTripViewController",
                controllerAs: "vm",
                templateUrl: "/views/anonymousSingleTripView.html"
            })
            $routeProvider.otherwise({ redirectTo: "/" });

        });

})();