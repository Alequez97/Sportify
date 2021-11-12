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
      markers: [],
      newLocationMarker: null,
      currentLocation: null
    }
  },
  watch: {
    geolocations(newGeolocations) {
      if (this.map !== undefined && this.map !== null) {
        newGeolocations.forEach((g) => {
          this.addMarkerToMap(g, g.type.replaceAll(" ", "_"));
        });
      }
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
        this.$emit('mapOnLoad');
      })
      .catch((e) => {
        console.log(e);
      });
  },
  methods: {
    addMarkerToMap(geolocation, iconName = 'Default') {
      const markerPosition = { lat: geolocation.lat, lng: geolocation.lng };
      const marker = new this.google.maps.Marker({
        position: markerPosition,
        icon: `/icons/map/${iconName}.png`
      });

      if (geolocation.description !== undefined && geolocation.description !== null) {
        const infoWindow = new this.google.maps.InfoWindow({
          content: this.getLocationInfoHtml(geolocation)
        });

        const map = this.map;
        marker.addListener("click", () => {
          infoWindow.open({
            anchor: marker,
            map,
            shouldFocus: false
          });
        });
      }

      marker.setMap(this.map);
      this.markers.push(marker);
      return marker;
    },
    addNewLocationMarker() {
      const geolocation = { lat: this.map.getCenter().lat(), lng: this.map.getCenter().lng() };
      this.newLocationMarker = this.addMarkerToMap(geolocation);
      this.map.addListener('center_changed', () => {
        if (this.newLocationMarker !== undefined && this.newLocationMarker !== null) {
          this.newLocationMarker.setPosition(this.map.getCenter());
        }
      });
    },
    async saveNewLocation(properties, type = 'Default') {
      if (this.newLocationMarker !== undefined && this.newLocationMarker !== null) {
        const latLng = { lat: this.newLocationMarker.getPosition().lat(), lng: this.newLocationMarker.getPosition().lng() };
        const fullAddress = await this.getAddressFromGeolocation(latLng);

        const data = {
          ...properties,
          lat: latLng.lat,
          lng: latLng.lng,
          ...fullAddress
        }

        // this.geolocations.push(latLng);
        this.addMarkerToMap(latLng, type);
        this.newLocationMarker.setMap(null);
        this.newLocationMarker = null;

        await this.$axios.post("/api/map/save", data);
      }
    },
    cancelAddingNewLocation() {
      if (this.newLocationMarker !== undefined && this.newLocationMarker !== null) {
        this.newLocationMarker.setMap(null);
        this.newLocationMarker = null;
      }
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
    },
    getAddressFromGeolocation(latLng) {
      return new Promise((resolve, reject) => {
        const geocoder = new this.google.maps.Geocoder();
        geocoder
          .geocode({ location: latLng })
          .then((response) => {
            if (response.results[0]) {
              const addressComponents = response.results[0].address_components;
              const fullAddress = {
                country: addressComponents.filter(c => c.types.includes("country"))[0]?.long_name,
                city: addressComponents.filter(c => c.types.includes("locality"))[0]?.long_name,
                district: addressComponents.filter(c => c.types.includes("sublocality"))[0]?.long_name,
                street: addressComponents.filter(c => c.types.includes("route"))[0]?.long_name,
                houseNumber: addressComponents.filter(c => c.types.includes("street_number"))[0]?.long_name
              };
              resolve(fullAddress);
            } else {
              const fullAddress = {
                country: null,
                city: null,
                district: null,
                street: null,
                houseNumber: null
              };
              resolve(fullAddress);
            }
          })
          .catch(e => reject(e));
      });
    },
    getLocationInfoHtml(geolocation) {
      const infoHtml =
      "<div class=\"info-window-wrapper\">" +
        "<div class=\"info-window-information-wrapper\">" +
          "<h3 class=\"infow-window-header\">" + geolocation.type + "</h3>" +
          "<p class=\"infow-window-description\">" + geolocation.description + "</h3>" +
        "</div>" +
        "<div class=\"info-window-images-wrapper\">" +
          "<img class=\"info-window-image\" src=\"/images/sportsGroundImages/test1.jpg\">" +
          "<img class=\"info-window-image\" src=\"/images/sportsGroundImages/test2.jpg\">" +
          "<img class=\"info-window-image\" src=\"/images/sportsGroundImages/test3.jpg\">" +
        "</div>" +
      "</div>"

      return infoHtml;
    }
  }
};
</script>

<style>
  #google-map {
    width: 100%;
    height: 100%;
  }
  .info-window-wrapper {
    width: 300px;
    height: 300px;
  }
  .info-window-information-wrapper {
    text-align: center;
  }
  .info-window-information-wrapper h3 {
    margin-bottom: 10px;
  }
  .info-window-images-wrapper {
    width: 100%;
  }
  .info-window-image {
    width: 100%;
  }
</style>
