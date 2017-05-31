var originMarker, map, directionsService, directionsDisplay;
var dirReq;

function initMap() {
    var centerLatLng = { lat: 40.6448081, lng: -111.8475633 };
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 18,
        center: centerLatLng,
        disableDefaultUI: true,
        zoomControl: true
    });

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
}

function directionsRequest(dirReq) {
    directionsService.route({
        origin: getLatLngFromString(dirReq.origin),
        destination: getLatLngFromString(dirReq.dest),
        travelMode: 'WALKING',
        unitSystem: google.maps.UnitSystem.METRIC
    }, directionsCallback);
}

function generateMarker(map, latLng, draggable) {
    var marker = new google.maps.Marker({
        position: latLng,
        map: map,
        draggable: true
    });
    marker.addListener('rightclick', function () {
        marker.setMap(null);
        marker = null;
    })

    return marker;
}

function directionsCallback(response, status) {
    if (status == 'OK') {
        directionsDisplay.setDirections(response);
    } else {
        window.alert('Directions request failed due to ' + status);
    }
}

function getLatLngFromString(str) {
    var latlng = str.split(/,/)
    return new google.maps.LatLng(parseFloat(latlng[0]), parseFloat(latlng[1]));
}

function computeTotalDistance(result) {
    var total = 0;
    var route = result.routes[0];
    for (var i = 0; i < route.legs.length; i++) {
        total += route.legs[i].distance.value;
    }
    total = total / 1000;
    console.log('distance: ' + total + ' km');
    var endIndex = route.overview_path.length - 1;
    document.getElementById('route_distance').value = total; /*+ ' km';*/
    document.getElementById('route_origin').value = route.overview_path[0].lat() + ',' + route.overview_path[0].lng();
    document.getElementById('route_destination').value = route.overview_path[endIndex].lat() + ',' + route.overview_path[endIndex].lng();
    return total;
}
