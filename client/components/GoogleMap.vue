<template>
  <div id="google-map" />
</template>

<script>
export default {
  name: 'GoogleMap',
  props: [
    'geolocations',
    'enabledTypeIds',
    'showFilterButton',
    'movableMarkerEnabled',
    'mapId'
  ],
  data() {
    return {
      map: null,
      google: null,
      geocoder: null,
      markers: [],
      markersAreShown: true,
      infoWindows: [],
      movableMarker: null,
      currentLocation: null
    }
  },
  watch: {
    async geolocations(newGeolocations) {
      for (let i = 0; i < newGeolocations.length; i++) {
        const newGeolocation = newGeolocations[i];
        const drawenMarker = this.markers.find(m => m.marker.id === newGeolocation.id);
        if (drawenMarker === undefined) {
          console.log('Creating new marker with id ' + newGeolocations[i].id);
          await this.addMarkerToMapAsync(newGeolocation, newGeolocation.typeName.replaceAll(" ", "_"));
        }
      }
    },
    enabledTypeIds(newTypes, oldTypes) {
      const wasEnabled = newTypes.length > oldTypes.length;
      if (wasEnabled) {
        const lastSelectTypeIds = newTypes.filter(x => !oldTypes.includes(x));

        lastSelectTypeIds.forEach((typeId) => {
          const filteredMarkers = this.markers.filter(m => m.typeId === typeId);
          filteredMarkers.forEach(m => m.marker.setMap(this.map));
        });
      } else {
        const lastSelectTypeIds = oldTypes.filter(x => !newTypes.includes(x));

        lastSelectTypeIds.forEach((typeId) => {
          const filteredMarkers = this.markers.filter(m => m.typeId === typeId);
          filteredMarkers.forEach(m => m.marker.setMap(null));
        });
      }
    },
    movableMarkerEnabled() {
      if (this.movableMarkerEnabled) {
        this.movableMarker.setMap(this.map);
      } else {
        this.movableMarker.setMap(null);
      }
    }
  },
  async mounted() {
    await this.$store.dispatch('googleMap/prepare');
    this.currentLocation = await this.$store.dispatch('googleMap/getCurrentLocationAsync');

    const mapOptions = {
      center: {
        lat: this.currentLocation.lat,
        lng: this.currentLocation.lng
      },
      zoom: 14
    };

    this.google = this.$store.getters['googleMap/getGoogleObject'];
    this.map = new this.google.maps.Map(document.getElementById("google-map"), mapOptions);

    this.google.maps.event.addListenerOnce(this.map, 'idle', () => {
      const center = this.map.getCenter();
      this.$emit('mapOnLoad', { lat: center.lat(), lng: center.lng() }, this.getMapWidth());
    });

    this.geocoder = this.$store.getters['googleMap/getGeocoderObject'];

    if (this.showFilterButton) {
      const controlDiv = document.createElement("div");
      this.getFilterGoogleButton(controlDiv);
      this.map.controls[this.google.maps.ControlPosition.TOP_LEFT].push(controlDiv);
    }

    this.map.addListener('click', () => {
      this.infoWindows.forEach(i => i.close());
    });
    this.map.addListener('center_changed', () => {
      this.movableMarker.setPosition(this.map.getCenter());
      const center = this.map.getCenter();
      this.$emit('centerChanged', { lat: center.lat(), lng: center.lng() }, this.getMapWidth());
    });
    this.map.addListener('zoom_changed', () => {
      const zoomLevel = this.map.getZoom();
      if (zoomLevel <= 12 && this.markersAreShown) {
        this.hideAllMarkers();
      }
      if (zoomLevel > 12 && !this.markersAreShown) {
        this.showAllMarkers();
      }
    });

    this.createMovableMarker(this.movableMarkerEnabled);

      // const addresses = [
      //   'Latvia, Riga, Eksporta iela 2a',
      //   'Latvia, Riga, Lenču iela 1',
      //   'Latvia, Riga, Sporta iela'
      // ];
      // addresses.forEach(async (address) => {
      //   const geolocation = await this.getGeolocationFromAddressAsync(address);
      //   const latLng = geolocation.geometry.location;
      //   this.addMarkerToMapAsync({ lat: latLng.lat(), lng: latLng.lng() });
      // });
  },
  methods: {
    addMarkerToMapAsync(geolocation, iconName = 'Default') {
      return new Promise((resolve) => {
        const markerPosition = { lat: geolocation.lat, lng: geolocation.lng };
        const marker = new this.google.maps.Marker({
          position: markerPosition,
          icon: `/icons/map/${iconName}.png`
        });

        if (geolocation.description !== undefined && geolocation.description !== null) {
          const infoWindow = new this.google.maps.InfoWindow({
            content: this.getLocationInfoHtml(geolocation)
          });

          this.infoWindows.push(infoWindow);

          const map = this.map;
          marker.addListener("click", () => {
            infoWindow.open({
              anchor: marker,
              map,
              shouldFocus: false
            });
          });
        }

        if (geolocation.typeId === undefined || this.enabledTypeIds.includes(geolocation.typeId)) {
          marker.setMap(this.map);
        }

        this.markers.push({ marker, typeId: geolocation.typeId });
        resolve(marker);
      });
    },
    showAllMarkers() {
      for (let i = 0; i < this.markers.length; i++) {
        const marker = this.markers[i].marker;
        marker.setMap(this.map);
      }
      this.markersAreShown = true;
    },
    hideAllMarkers() {
      for (let i = 0; i < this.markers.length; i++) {
        const marker = this.markers[i].marker;
        marker.setMap(null);
      }
      this.markersAreShown = false;
    },
    async saveMovableMarkerPosition(properties, type = 'Default') {
      const geolocation = {
        lat: this.movableMarker.getPosition().lat(),
        lng: this.movableMarker.getPosition().lng(),
        typeId: properties.typeId
      };
      const fullAddress = await this.$store.dispatch('googleMap/getAddressFromGeolocationAsync', geolocation);

      const data = {
        ...properties,
        lat: geolocation.lat,
        lng: geolocation.lng,
        ...fullAddress
      }

      // this.geolocations.push(geolocation);
      this.addMarkerToMapAsync(geolocation, type);

      await this.$axios.post("/api/map/save", data);
    },
    createMovableMarker(enabled, iconName = 'Default') {
      const mapCenter = this.map.getCenter();
      const markerPosition = { lat: mapCenter.lat(), lng: mapCenter.lng() };
      this.movableMarker = new this.google.maps.Marker({
        position: markerPosition,
        icon: `/icons/map/${iconName}.png`
      });

      if (enabled) {
        this.movableMarker.setMap(this.map);
      }
    },
    getMapWidth() {
      const bounds = this.map.getBounds();
      const center = this.map.getCenter();
      const SWCorner = bounds.getSouthWest();

      return Math.abs(center.lng() - SWCorner.lng());
    },
    getFilterGoogleButton(controlDiv) {
      // Set CSS for the control border.
      const controlUI = document.createElement("div");

      controlUI.style.backgroundColor = "#fff";
      controlUI.style.border = "2px solid #fff";
      controlUI.style.borderRadius = "3px";
      controlUI.style.boxShadow = "0 2px 6px rgba(0,0,0,.3)";
      controlUI.style.cursor = "pointer";
      controlUI.style.marginTop = "8px";
      controlUI.style.marginBottom = "22px";
      controlUI.style.textAlign = "center";
      controlUI.title = "Click to filter types";
      controlDiv.appendChild(controlUI);

      // Set CSS for the control interior.
      const controlText = document.createElement("div");

      controlText.style.color = "rgb(25,25,25)";
      controlText.style.fontFamily = "Roboto,Arial,sans-serif";
      controlText.style.fontSize = "16px";
      controlText.style.lineHeight = "38px";
      controlText.style.paddingLeft = "5px";
      controlText.style.paddingRight = "5px";
      controlText.innerHTML = "Filter types";
      controlUI.appendChild(controlText);
      // Setup the click event listeners: simply set the map to Chicago.
      controlUI.addEventListener("click", () => {
        this.$emit('filterOnClick');
      });
    },
    getLocationInfoHtml(geolocation) {
      const infoHtml =
      "<div class=\"info-window-wrapper\">" +
        "<div class=\"info-window-information-wrapper\">" +
          "<h3 class=\"infow-window-header\">" + geolocation.typeName + "</h3>" +
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
    max-width: 300px;
    width: 100%;
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
