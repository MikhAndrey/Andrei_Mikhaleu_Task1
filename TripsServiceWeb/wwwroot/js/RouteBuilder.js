let map;
let markers = [];
let directionsService;
let directionsDisplay;
let startTimeZoneOffset, finishTimeZoneOffset;
const decimalSeparator = (1.1).toLocaleString().substring(1, 2);
let totalDuration = 0;

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
    map.addListener('click', async function (event) {
        addMarker(event.latLng);
        await calculateAndDisplayRoute(false);
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

async function removeMarker(marker) {
    const index = getMarkerIndexByLabel(marker.label);
    if (index > -1) {
        markers[index].marker.setMap(null);
        markers[index].letterMarker.setMap(null);
        markers.splice(index, 1);
        await calculateAndDisplayRoute(false);
    }
    if (markers.length == 1) {
        $("#endTimeInput").val('');
        $("#route-length-view").text('0');
        $("#route-duration-view").text('0 minutes');
    }
}

function addClickOnMarker(marker) {
    marker.letterMarker.addListener('click', async function () {
        await removeMarker(marker);
    });
}

function makeMarkerDraggable(marker){
    marker.letterMarker.setDraggable(true);
    google.maps.event.addListener(marker.letterMarker, 'dragend', async function (event) {
        const index = getMarkerIndexByLabel(marker.label);
        if (index > -1) {
            markers[index].marker.setPosition(event.latLng);
            await calculateAndDisplayRoute(false);
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
        scale: 7,
        labelOrigin: new google.maps.Point(0, 3),
        label: label
    };
}

function getMarkerIndexByLabel(label) {
    return markers.findIndex(el => el.label === label);
}

async function calculateAndDisplayRoute(routeIsReadOnly) {
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

    directionsService.route(request, async function (response, status) {
        if (status === 'OK') {
            directionsDisplay.setDirections(response);

            if (!routeIsReadOnly) {
                const legs = response.routes[0].legs;
                const totalDistance = legs.reduce((total, current) => total + current.distance.value, 0);
                totalDuration = legs.reduce((total, current) => total + current.duration.value, 0);
                const distanceText = getTextOfDistance(totalDistance);
                const durationText = getStringRepresentationOfTripDuration(totalDuration);

                storeDistance(totalDistance);
                await getAndSaveTimeZoneOffsets(markers[0], markers[markers.length - 1]);
                setEndDate(totalDuration);
                storeRoutePoints();

                $("#route-length-view").text(distanceText);
                $("#route-duration-view").text(durationText);
            }
        } else {
            window.alert('Directions request failed due to ' + status);
            removeMarker(markers[markers.length - 1]);
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
    $("input[name='RoutePointsAsString']").val(JSON.stringify(routePoints));
}

function storeDistance(distance) {
    $("input[name='Distance']").val((distance / 1000).toString().replace(".", decimalSeparator));
}

function setEndDate(tripDuration) {
    try {
        const startDate = new Date($("#startTimeInput").val());
        const localTimezoneOffset = startDate.getTimezoneOffset() * 60 * 1000;
        const endDate = new Date(startDate.getTime() - localTimezoneOffset + 1000 * (tripDuration - startTimeZoneOffset + finishTimeZoneOffset));
        $("#endTimeInput").val(endDate.toISOString().slice(0, 16));
    } catch{}
}

function getStringRepresentationOfTripDuration(tripDuration){
    const SECONDS_IN_DAY = 86400;
    const SECONDS_IN_HOUR = 3600;
    const SECONDS_IN_MINUTE = 60;

    const days = Math.floor(tripDuration / SECONDS_IN_DAY);
    const hours = Math.floor((tripDuration % SECONDS_IN_DAY) / SECONDS_IN_HOUR);
    const minutes = Math.floor((tripDuration % SECONDS_IN_HOUR) / SECONDS_IN_MINUTE);

    return `${days} days, ${hours} hours, ${minutes} minutes`;
};

function getTextOfDistance(distance){
    return `${distance / 1000}`;
};

async function getAndSaveTimeZoneOffsets(startMarker, finishMarker) {
    const apiKey = "AIzaSyAFD93X-nfhki6P2iGKcBv142KWS6SPrjI";
    let latitude = startMarker.marker.getPosition().lat();
    let longitude = startMarker.marker.getPosition().lng();
    const timestamp = Math.floor(new Date() / 1000);
    
    let requestUrl = `https://maps.googleapis.com/maps/api/timezone/json?location=${latitude},${longitude}&timestamp=${timestamp}&key=${apiKey}`;

    try {
        const response = await fetch(requestUrl)
        const data = await response.json();
        startTimeZoneOffset = data.dstOffset + data.rawOffset;
        $("input[name='StartTimeZoneOffset']").val((startTimeZoneOffset).toString().replace(".", decimalSeparator));
    } catch(error) {
        console.log("Failed to retrieve timezone information: " + error);
    }

    latitude = finishMarker.marker.getPosition().lat();
    longitude = finishMarker.marker.getPosition().lng();

    requestUrl = `https://maps.googleapis.com/maps/api/timezone/json?location=${latitude},${longitude}&timestamp=${timestamp}&key=${apiKey}`;

    try {
        const response = await fetch(requestUrl)
        const data = await response.json();
        finishTimeZoneOffset = data.dstOffset + data.rawOffset;
        $("input[name='FinishTimeZoneOffset']").val((finishTimeZoneOffset).toString().replace(".", decimalSeparator));
    } catch (error) {
        console.log("Failed to retrieve timezone information: " + error);
    }
};

$("#startTimeInput").on('change', function () {
    setEndDate(totalDuration);
});