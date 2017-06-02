var originMarker, map, directionsService, directionsDisplay;
var dirReq;
var isLoopRoute = false;

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

function isLoopRouteChecked() {
    isLoopRoute = !isLoopRoute;
    var dist = document.getElementById('route_distance');
    if (dist.value != null && dist.value != "") {
        dist.value = isLoopRoute ? dist.value * 2 : dist.value / 2;
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
    total = isLoopRoute ? (total / 1000) * 2 : total / 1000;
    console.log('distance: ' + total + ' km');
    var endIndex = route.overview_path.length - 1;
    document.getElementById('route_distance').value = total; /*+ ' km';*/
    document.getElementById('route_origin').value = route.overview_path[0].lat() + ',' + route.overview_path[0].lng();
    document.getElementById('route_destination').value = route.overview_path[endIndex].lat() + ',' + route.overview_path[endIndex].lng();
    return total;
}