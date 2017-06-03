function initMap() {
    var centerLatLng = getCenter(new google.maps.LatLng({ lat: 40.6448081, lng: -111.8475633 }));

    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 18,
        center: centerLatLng,
        disableDefaultUI: true,
        zoomControl: true
    });

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            centerLatLng = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            map.setCenter(centerLatLng);
        }, function () {
            console.log('Geolocation service failed.');
        });
    } 


    directionsService = new google.maps.DirectionsService;
    directionsDisplay = new google.maps.DirectionsRenderer({
        draggable: true,
        map: map
    });

    directionsDisplay.addListener('directions_changed', function () {
        computeTotalDistance(directionsDisplay.getDirections());
    });

    google.maps.event.addListener(map, 'click', function (event) {
        if (originMarker == null) {
            originMarker = generateMarker(map, event.latLng, true);
        }
        else {
            directionsService.route({
                origin: originMarker.position,
                destination: event.latLng,
                travelMode: 'WALKING',
                unitSystem: google.maps.UnitSystem.METRIC
            }, directionsCallback);

            originMarker.setMap(null);
            originMarker = null;
        }
    });

    if (dirReq != null) {
        directionsRequest(dirReq);
    }

    document.getElementById('IsLoopRoute').addEventListener('click', isLoopRouteChecked);
}

function getCenter(defaultLatLng) {
    var origin = document.getElementById('route_origin');
    if (origin.value != '') {
        return getLatLngFromString(origin.value)
    }
    return defaultLatLng;
}


