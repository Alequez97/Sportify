let map;
let newLocationMarker;

function initMap() {
    const jurecLat = 56.81543608008677;
    const jurecLng = 24.599863529284583;

    const mapCenter = { lat: jurecLat, lng: jurecLng};

    map = new google.maps.Map(document.getElementById("google-map"), {
        zoom: 15,
        center: mapCenter,
    });
}

function addMarker(lat, lng) {
    const markerPosition = { lat: lat, lng: lng };
    
    let marker = new google.maps.Marker({
        position: markerPosition,
      });
    //   mapMarkers.push(marker1);
    //   marker1.addListener("click", () => {
    //     console.log(marker1.getPosition());
    //   });

    marker.setMap(map);
}

function addNewLocationMarker() {
    let mapCenter = map.getCenter();
    let currentMapCenter = { lat: mapCenter.lat(), lng: mapCenter.lng() };
    newLocationMarker = new google.maps.Marker({
        position: currentMapCenter,
        store_id: "new-location-marker",
    });
    newLocationMarker.setMap(map);

    map.addListener('center_changed', () => {
        if (newLocationMarker != undefined)
        {
          newLocationMarker.setPosition(map.getCenter());
        }
    });

    showSaveAndCancelButton();
}

function openLocationInfoForm() {
    axios.get("https://randomuser.me/api/").then(response => {
        console.log(response);
    });
}

function storeNewLocation() {
    
}

function removeNewLocationMarker() {
    newLocationMarker.setMap(null);
    newLocationMarker = undefined;
    showAddNewLocationButton();
}

function showAddNewLocationButton() {
    document.getElementById("cancel-new-location-container").style.display = "none";
    document.getElementById("add-new-location-container").style.display = "block";
}

function showSaveAndCancelButton() {
    document.getElementById("cancel-new-location-container").style.display = "block";
    document.getElementById("add-new-location-container").style.display = "none";
}