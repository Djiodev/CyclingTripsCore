(function () {

    var app = angular.module("anonymous", [])

    var AnonymousController = function ($scope, $http) {
        var vm = this;
        function getStops() {
            var tripName = vm.name;
            alert(vm.name);
            vm.stops = [];
            $http.get("/api/Anonymous/" + tripName)
                .then(function (response) {
                    angular.copy(response.data, vm.stops);
                    alert("stop success");
                }, function () {
                    alert("err");
                });
        }

        getStops();
    }

    app.controller("AnonymousController", AnonymousController);

})();