function initMap() {
    var latLng = { lat: 40.6448081, lng: -111.8475633 };
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 18,
        center: latLng,
        disableDefaultUI: true,
        zoomControl: true
    });

    var destLatLng = { lat: 40.649209, lng: -111.8557767 };

    var directionsService = new google.maps.DirectionsService;
    var directionsDisplay = new google.maps.DirectionsRenderer({
        draggable: true,
        map: map
    });

    directionsDisplay.addListener('directions_changed', function () {
        computeTotalDistance(directionsDisplay.getDirections());
    });

    directionsService.route({
        origin: latLng,
        destination: destLatLng,
        travelMode: 'WALKING',
        unitSystem: google.maps.UnitSystem.METRIC
    }, function (response, status) {
        if (status == 'OK') {
            directionsDisplay.setDirections(response);
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });

    document.getElementById('btn_foo').onclick = function () {
        foo(directionsDisplay.getDirections());
    }

}

function computeTotalDistance(result) {
    var total = 0;
    var myroute = result.routes[0];
    for (var i = 0; i < myroute.legs.length; i++) {
        total += myroute.legs[i].distance.value;
    }
    total = total / 1000;
    console.log('distance: ' + total + ' km');
    //document.getElementById('total').innerHTML = total + ' km';
    return total;
}

function foo(directions) {
    var route = directions.routes[0];
    var latLng = route.overview_path[0];
    var polyline = '';
    for (var i = 0; i < route.overview_path.length; ++i) {
        latLng = route.overview_path[i];
        polyline += latLng.lat() + ',' + latLng.lng();
        if (i != route.overview_path.length - 1) {
            polyline += '|';
        }
    }
    console.log(polyline);

    var data = {
        name: 'My Route',
        polyline: polyline,
        distance: computeTotalDistance(directions)
    }

    $.ajax({
        type: 'POST',
        url: baseURL + 'api/routes',
        data: data,
        contentType: 'json',
        complete: function (xhr, textStatus) {
            console.log(textStatus);
        }
    });
}