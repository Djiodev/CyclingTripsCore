
(function () {

    var googleMap;
    var autocomplete;
    var stops = [];

    var initializeMap = function () {
        var mapOptions = {
            center: new google.maps.LatLng(49.933330, 1.0833300),
            zoom: 12,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var inputDiv = document.getElementById('autocompleteDiv');

        googleMap = new google.maps.Map(document.getElementById('googleMap'), mapOptions);
        googleMap.controls[google.maps.ControlPosition.TOP_LEFT].push(inputDiv);

        var acOptions = {
            types: ['geocode']
        };


        autocomplete = new google.maps.places.Autocomplete(document.getElementById('googleMap'), acOptions);
        autocomplete.bindTo('bounds', googleMap);
        var infoWindow = new google.maps.InfoWindow();
        var marker = new google.maps.Marker({
            map: googleMap
        });

    }


    var mapVisualizationType = function () {
        var mapTypeId = "google.maps.MapTypeId." + $(this).attr("data-MapType");

        googleMap.setMapTypeId(mapTypeId);
    }

    var showStopOnMap = function () {
        var latitude = $(this).attr("data-latitude").replace(",", ".");
        var longitude = $(this).attr("data-longitude").replace(",", ".");
        var location = $(this).attr("data-stopName");
        var arrival = $(this).attr("data-stopArrival");

        var latLng = new google.maps.LatLng(latitude, longitude);

        stops.push(latLng);

        var marker = new google.maps.Marker({
            position: latLng,
            map: googleMap
        });

        googleMap.setCenter({
            lat: latLng.lat(),
            lng: latLng.lng()
        });
        var infoWindowsOptions = {
            content:
                '<div class="stopWindow">' +
                '<h4>' + location + ', ' + arrival + '</h4>' +
                '</div>'
        };

        var infoWindow = new google.maps.InfoWindow(infoWindowsOptions);
        google.maps.event.addListener(marker, 'click', function (e) {
            infoWindow.open(googleMap, marker);
        });

    }


    initializeMap();

    $("a[href='#stop']").each(showStopOnMap);
    $("#RoadBtn, #TerrainBtn, #SatelliteBtn").on("click", mapVisualizationType);
    $("a[href='#stop']").on("click", showStopOnMap);

    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        infoWindow.close();
        var place = autocomplete.getPlace();
        if (place.geometry.viewport) {
            googleMap.fitBounds(place.geometry.viewport);
        } else {
            googleMap.setCenter(place.geometry.location);
            googleMap.setZoom(17);
        }
        marker.setPosition(place.geometry.location);
        infoWindow.setContent('<div><strong>' + place.name + '</strong><br>');
        infoWindow.open(googleMap, marker);
        google.maps.event.addListener(marker, 'click', function (e) {

            infoWindow.open(googleMap, marker);

        });
    });

    //function drawPath() {
    //    alert("drawing");
    //    var path = new google.maps.drawPolyline({
    //        path: stops,
    //        stokeColor: '#273b4e',
    //        strokeOpacity: 0.5,
    //        strokeWeight: 2
    //    });
    //    path.setMap(googleMap)
    //}

    //drawPath();

})();

