(function () {

    "use strict";

    angular.module("app-userTrips", ["ngRoute", "ngAutocomplete"])
        .config(function ($routeProvider) {

            $routeProvider.when("/", {
                controller: "userTripsController",
                controllerAs: "vm",
                templateUrl: "/views/tripsView.html"
            })
            .when("/editor/:tripName", {
                controller: "tripEditorController",
                controllerAs: "vm",
                templateUrl: "/views/tripEditorView.html"
            })
            $routeProvider.otherwise({ redirectTo: "/" })

        });



})();