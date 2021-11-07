<template>
  <div id="google-map" />
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
      currentLocation: null
    }
  },
  async mounted() {
    const loader = new Loader({
      apiKey: "AIzaSyD_u7kDh3P6m58MutrCTCDsF2Oiy-jPMf0"
    });

    this.currentLocation = await this.getCurrentLocation();

    const mapOptions = {
      center: {
        lat: this.currentLocation.lat,
        lng: this.currentLocation.lng
      },
      zoom: 14
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
    getCurrentLocation() {
      return new Promise((resolve) => {
        if (navigator.geolocation) {
          navigator.geolocation.getCurrentPosition((position) => {
            const currentPosition = { lat: position.coords.latitude, lng: position.coords.longitude };
            resolve(currentPosition);
          });
        } else {
          // Implement other logic if geolocation is disabled
          const currentPositionLat = 56.81543608008677;
          const currentPositionLng = 24.599863529284583;
          const currentPosition = { lat: currentPositionLat, lng: currentPositionLng };
          resolve(currentPosition);
        }
      });
    }
  }
};
</script>

<style scoped>
  #google-map {
      width: 100%;
      height: 100%;
  }
</style>
