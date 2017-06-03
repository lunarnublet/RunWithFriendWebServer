var map;
var directionsService;
var directionsDisplay;

function init() {
    var table = document.getElementById('routes');

    if (table.tBodies[0].rows.length > 0) {
        var row = table.tBodies[0].rows[0];
        var origin = row.cells[1];
        var dest = row.cells[2];

        console.log(origin.innerHTML + ' ' + dest.innerHTML)

        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 15,
            center: getLatLngFromString(origin.innerHTML),
            disableDefaultUI: true,
            zoomControl: true
        })

        directionsService = new google.maps.DirectionsService;
        directionsDisplay = new google.maps.DirectionsRenderer({
            map: map
        });

        directionsService.route({
            origin: getLatLngFromString(origin.innerHTML),
            destination: getLatLngFromString(dest.innerHTML),
            travelMode: 'WALKING',
            unitSystem: google.maps.UnitSystem.METRIC
        }, function (response, status) {
            if (status == 'OK') {
                directionsDisplay.setDirections(response);
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
    }
}

function updateDirections(row) {
    var origin = row.cells[1];
    var dest = row.cells[2];

    directionsService.route({
        origin: getLatLngFromString(origin.innerHTML),
        destination: getLatLngFromString(dest.innerHTML),
        travelMode: 'WALKING',
        unitSystem: google.maps.UnitSystem.METRIC
    }, function (response, status) {
        if (status == 'OK') {
            directionsDisplay.setDirections(response);
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
}

function getLatLngFromString(str) {
    var latlng = str.split(/,/)
    return new google.maps.LatLng(parseFloat(latlng[0]), parseFloat(latlng[1]));
}