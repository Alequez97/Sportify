<template>
  <div id="google-map" />
</template>

<script>
export default {
  name: 'GoogleMap',
  props: [
    'geolocations',
    'initZoomLevel',
    'detailedInfoZoomLevel',
    'enabledTypeIds',
    'showFilterButton',
    'movableMarkerEnabled',
    'mapId'
  ],
  data() {
    return {
      map: null,
      currentZoomLevel: null,
      google: null,
      geocoder: null,
      markers: [],
      renderedGeolocationsIds: [],
      markersAreShown: true,
      infoWindows: [],
      movableMarker: null,
      currentLocation: null
    }
  },
  watch: {
    geolocations(newGeolocationArray) {
      const newGeolocations = newGeolocationArray.filter(g => !this.renderedGeolocationsIds.includes(g.id));
      for (let i = 0; i < newGeolocations.length; i++) {
        const newGeolocation = newGeolocations[i];
        console.log('Creating new marker with id ' + newGeolocations[i].id); // TODO: Remove console log
        this.addMarkerToMapAsync(newGeolocation, newGeolocation.typeName.replaceAll(" ", "_"));
      }
    },
    enabledTypeIds(newTypes, oldTypes) {
      if (this.map?.getZoom() < this.detailedInfoZoomLevel) { // If zoom level is small no need to rerender icons
        return;
      }

      const wasEnabled = newTypes.length >= oldTypes.length;
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
  created() {
    this.currentZoomLevel = this.initZoomLevel;
  },
  async mounted() {
    await this.$store.dispatch('googleMap/prepare');
    this.currentLocation = await this.$store.dispatch('googleMap/getCurrentLocationAsync');

    const mapOptions = {
      center: {
        lat: this.currentLocation.lat,
        lng: this.currentLocation.lng
      },
      zoom: this.currentZoomLevel
    };

    this.google = this.$store.getters['googleMap/getGoogleObject'];
    this.geocoder = this.$store.getters['googleMap/getGeocoderObject'];
    this.map = new this.google.maps.Map(document.getElementById("google-map"), mapOptions);

    this.registerMapListeners();

    if (this.showFilterButton) {
      const controlDiv = document.createElement("div");
      this.getFilterGoogleButton(controlDiv);
      this.map.controls[this.google.maps.ControlPosition.TOP_LEFT].push(controlDiv);
    }

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
    registerMapListeners() {
      this.google.maps.event.addListenerOnce(this.map, 'idle', () => {
        const center = this.map.getCenter();
        this.$emit('mapOnLoad', { lat: center.lat(), lng: center.lng() }, this.map.getZoom(), this.getMapSize());
      });

      this.map.addListener('click', () => {
        this.infoWindows.forEach(i => i.close());
      });

      this.map.addListener('center_changed', () => {
        const center = this.map.getCenter();
        this.movableMarker.setPosition(center);
        this.$emit('centerChanged', { lat: center.lat(), lng: center.lng() }, this.map.getZoom(), this.getMapSize());
      });

      this.map.addListener('zoom_changed', () => {
        const newZoomLevel = this.map.getZoom();
        const center = this.map.getCenter();

        if (newZoomLevel < this.currentZoomLevel) {
          this.$emit('zoomOut', { lat: center.lat(), lng: center.lng() }, newZoomLevel, this.getMapSize());
        } else {
          this.$emit('zoomIn', { lat: center.lat(), lng: center.lng() }, newZoomLevel, this.getMapSize());
        }

        if (newZoomLevel < this.detailedInfoZoomLevel && this.markersAreShown) {
          this.hideAllMarkers();
        }
        if (newZoomLevel >= this.detailedInfoZoomLevel && !this.markersAreShown) {
          this.showAllMarkers();
        }

        this.currentZoomLevel = newZoomLevel;
      });
    },
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

        this.renderedGeolocationsIds.push(geolocation.id);
        this.markers.push({ marker, typeId: geolocation.typeId });
        resolve(marker);
      });
    },
    showAllMarkers() {
      for (let i = 0; i < this.markers.length; i++) {
        const marker = this.markers[i].marker;
        if (this.enabledTypeIds.includes(this.markers[i].typeId)) {
          marker.setMap(this.map);
        }
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
    getMovableMarkerPosition() {
      const position = {
        lat: this.movableMarker.getPosition().lat(),
        lng: this.movableMarker.getPosition().lng()
      };

      return position;
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
    getMapSize() {
      const bounds = this.map.getBounds();
      const center = this.map.getCenter();
      const SWCorner = bounds.getSouthWest();

      const size1 = Math.abs(center.lng() - SWCorner.lng());
      const size2 = Math.abs(center.lat() - SWCorner.lat());

      return size1 > size2 ? size1 : size2;
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
      let imagesHtml = '';
      if (geolocation.images !== null && geolocation.images !== undefined) {
        for (let i = 0; i < geolocation.images.length; i++) {
          const imagePath = geolocation.images[i];
          const imageSource = "images\\sportsGroundImages\\" + imagePath;
          imagesHtml += "<a href=\"" + imageSource + "\"><img class=\"info-window-image\" src=\"" + imageSource + "\"></a>"
        }
      }

      const infoHtml =
      "<div class=\"info-window-wrapper\">" +
        "<div class=\"info-window-information-wrapper\">" +
          "<h3 class=\"infow-window-header\">" + geolocation.typeName + "</h3>" +
          "<p class=\"infow-window-description\">" + geolocation.description + "</h3>" +
        "</div>" +
        "<div class=\"info-window-images-wrapper\">" +
          imagesHtml +
        "</div>" +
      "</div>"

      return infoHtml;
    }
  }
};
</script>

<style>
 #google-map-wrapper {
    width: 100%;
    height: 100%;
  }
  #google-map {
    width: 100%;
    height: 100%;
  }
  .info-window-wrapper {
    max-width: 300px;
    width: 100%;
    height: 100%;
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
