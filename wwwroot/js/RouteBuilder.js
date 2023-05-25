let map;
let markers = [];
let directionsService;
let directionsDisplay;

function initMap(latitude, longitude, zoom) {
    map = new google.maps.Map(document.getElementById('map'), {
        center: {
            lat: latitude,
            lng: longitude
        },
        zoom: zoom
    });

    directionsService = new google.maps.DirectionsService();
    directionsDisplay = new google.maps.DirectionsRenderer();
    directionsDisplay.setMap(map);
}

function addClickOnMap() {
    map.addListener('click', function (event) {
        addMarker(event.latLng);
        calculateAndDisplayRoute(false);
        const currentMarker = markers[markers.length - 1];
        addClickOnMarker(currentMarker);
        makeMarkerDraggable(currentMarker);
    });
}

function addMarker(location) {
    const marker = new google.maps.Marker({
        position: location,
        map: map
    });
    
    const label = String.fromCharCode(65 + markers.length);
    const letterMarker = new google.maps.Marker({
        position: location,
        icon: getLetterMarkerIcon(label),
        map: map
    });

    markers.push({
        marker: marker,
        letterMarker: letterMarker,
        label: label
    });
}

function addClickOnMarker(marker) {
    marker.letterMarker.addListener('click', function () {
        const index = getMarkerIndexByLabel(marker.label);
        if (index > -1) {
            markers[index].marker.setMap(null);
            markers[index].letterMarker.setMap(null);
            markers.splice(index, 1);
            calculateAndDisplayRoute(false);
        }
    });
}

function makeMarkerDraggable(marker){
    marker.letterMarker.setDraggable(true);
    google.maps.event.addListener(marker.letterMarker, 'dragend', function (event) {
        const index = getMarkerIndexByLabel(marker.label);
        if (index > -1) {
            markers[index].marker.setPosition(event.latLng);
            calculateAndDisplayRoute();
        }
    });
}

function getLetterMarkerIcon(label) {
    return {
        path: google.maps.SymbolPath.CIRCLE,
        fillColor: '#ff0000',
        fillOpacity: 1,
        strokeColor: '#000000',
        strokeWeight: 1,
        scale: 6,
        labelOrigin: new google.maps.Point(0, 3),
        label: label
    };
}

function getMarkerIndexByLabel(label) {
    return markers.findIndex(el => el.label === label);
}

function calculateAndDisplayRoute(routeIsReadOnly) {
    if (markers.length < 2) {
        directionsDisplay.setDirections({
            routes: []
        });
        return;
    }

    const start = markers[0].marker;
    const end = markers[markers.length - 1].marker;
    const waypoints = [];
    for (let i = 1; i < markers.length - 1; i++) {
        waypoints.push({
            location: markers[i].marker.position,
            stopover: true
        });
    }

    const request = {
        origin: start.position,
        destination: end.position,
        waypoints: waypoints,
        travelMode: 'DRIVING'
    };

    directionsService.route(request, function (response, status) {
        if (status === 'OK') {
            directionsDisplay.setDirections(response);

            if (!routeIsReadOnly) {
                const legs = response.routes[0].legs;
                const totalDistance = legs.reduce((total, current) => total + current.distance.value, 0);
                const totalDuration = legs.reduce((total, current) => total + current.duration.value, 0);
                const durationAndDistanceText = getTextOfDurationAndDistance(totalDuration, totalDistance);

                storeDistance(totalDistance);
                setEndDate(totalDuration);
                storeRoutePoints();

                $("#route-length-view").text(durationAndDistanceText.length);
                $("#route-duration-view").text(durationAndDistanceText.duration);
            }
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
}

function storeRoutePoints() {
    const routePoints = markers.map((marker, index) => {
        return {
            Ordinal: index,
            Longitude: marker.marker.position.lng(),
            Latitude: marker.marker.position.lat()
        };
    });
    $("input[name='routePoints']").val(JSON.stringify(routePoints));
}

function storeDistance(distance) {
    const decimalSeparator = (1.1).toLocaleString().substring(1, 2);
    $("input[name='Distance']").val((distance / 1000).toString().replace(".", decimalSeparator));
}

function setEndDate(tripDuration) {
    const startDate = new Date($("#startTimeInput").val());
    const timezoneOffset = startDate.getTimezoneOffset() * 60 * 1000;
    const endDate = new Date(startDate.getTime() + 1000 * tripDuration - timezoneOffset);
    $("#endTimeInput").val(endDate.toISOString().slice(0, 16));
}

function getTextOfDurationAndDistance(duration, distance){
    const distanceText = `${distance / 1000} км`;

    const SECONDS_IN_YEAR = 31536000;
    const SECONDS_IN_MONTH = 2592000;
    const SECONDS_IN_DAY = 86400;
    const SECONDS_IN_HOUR = 3600;
    const SECONDS_IN_MINUTE = 60;

    const years = Math.floor(duration / SECONDS_IN_YEAR);
    const months = Math.floor((duration % SECONDS_IN_YEAR) / SECONDS_IN_MONTH);
    const days = Math.floor((duration % SECONDS_IN_MONTH) / SECONDS_IN_DAY);
    const hours = Math.floor((duration % SECONDS_IN_DAY) / SECONDS_IN_HOUR);
    const minutes = Math.floor((duration % SECONDS_IN_HOUR) / SECONDS_IN_MINUTE);

    const yearsText = years > 0 ? years + " г. " : "";
    const monthsText = months > 0 ? months + " м. " : "";
    const daysText = days > 0 ? days + " д. " : "";
    const hoursText = hours > 0 ? hours + " ч. " : "";
    const minutesText = minutes + " мин.";
    return {
        duration: yearsText + monthsText + daysText + hoursText + minutesText,
        length: distanceText
    };
};