<template>
  <div id="google-map" class="align-center" />
</template>

<script>
import { Loader } from '@googlemaps/js-api-loader';

export default {
  name: 'GoogleMap',
  props: ['geolocations'],
  data() {
    return {
      map: null,
      google: null,
      newLocationMarker: null,
      currentPosition: null
    }
  },
  async mounted() {
    const loader = new Loader({
      apiKey: "AIzaSyD_u7kDh3P6m58MutrCTCDsF2Oiy-jPMf0"
    });

    // const currentObject = this;

    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position) => {
        debugger;
        const positionLat = position.coords.latitude;
        const positionLng = position.coords.longitude;
        this.currentPosition = { lat: positionLat, lng: positionLng };
        // const infoWindowHTML = "Latitude: " + currentLatitude + "<br>Longitude: " + currentLongitude;
        // const infoWindow = new this.google.maps.InfoWindow({ map: this.map, content: infoWindowHTML });
        // const currentLocation = { lat: currentLatitude, lng: currentLongitude };
        // infoWindow.setPosition(currentLocation);
        // document.getElementById("btnAction").style.display = 'none';
      });
    } else {
      const currentPositionLat = 56.81543608008677;
      const currentPositionLng = 24.599863529284583;
      this.currentPosition = { lat: currentPositionLat, lng: currentPositionLng };
    }
    debugger;
    console.log(this.currentPosition);
    const mapOptions = {
      center: {
        lat: this.currentPosition.lat,
        lng: this.currentPosition.lng
      },
      zoom: 15
    };

    await loader
        .load()
        .then((google) => {
          this.google = google;
          this.map = new google.maps.Map(document.getElementById("google-map"), mapOptions);
        })
        .catch((e) => {
          console.log(e);
        });
    if (this.geolocations !== undefined && this.geolocations.length > 0) {
      this.geolocations.forEach((g) => {
        const markerPosition = { lat: g.lat, lng: g.lng };

        const marker = new this.google.maps.Marker({
          position: markerPosition
        });

        marker.setMap(this.map);
      });
    }
  },
  methods: {
    addNewLocationMarker() {
      this.newLocationMarker = new this.google.maps.Marker({
          position: this.map.getCenter()
      });

      this.newLocationMarker.setMap(this.map);

      this.map.addListener('center_changed', () => {
        if (this.newLocationMarker !== undefined && this.newLocationMarker !== null) {
          this.newLocationMarker.setPosition(this.map.getCenter());
        }
      });
    },
    saveNewLocation() {
      console.log(this.newLocationMarker.getPosition().lat() + "; " + this.newLocationMarker.getPosition().lng());
    },
    cancelAddingNewLocation() {
      this.newLocationMarker.setMap(null);
      this.newLocationMarker = null;
    },
    currentLocation() {
      // document.getElementById("btnAction").disabled = true;
      // document.getElementById("btnAction").innerHTML = "Processing...";
      if ("geolocation" in navigator) {
        navigator.geolocation.getCurrentPosition(function(position) {
          const currentLatitude = position.coords.latitude;
          const currentLongitude = position.coords.longitude;

          const infoWindowHTML = "Latitude: " + currentLatitude + "<br>Longitude: " + currentLongitude;
          const infoWindow = new this.google.maps.InfoWindow({ map: this.map, content: infoWindowHTML });
          const currentLocation = { lat: currentLatitude, lng: currentLongitude };
          infoWindow.setPosition(currentLocation);
          // document.getElementById("btnAction").style.display = 'none';
        });
      }
    }
  }
};
</script>

<style scoped>
    #google-map {
        width: 80%;
        height: 400px;
    }

    .align-center {
        margin: 0 auto;
    }
</style>
