<template>
    <div>
        <GoogleMap :geolocations="geolocations" ref="map" />
        <div class="centered-div">
            <div id="add-new-location-container">
                <button id="add-new-location-button" @click="addNewLocationMarker">Add new location</button>
                <button id="add-new-location-button" @click="saveNewLocation">Save</button>
                <button id="add-new-location-button" @click="cancelAddingNewLocation">Cancel</button>
            </div>
            <div id="cancel-new-location-container" style="position:relative; bottom:130px;">
                <p>hello</p>
            </div>
        </div>
    </div>
</template>

<script>
import GoogleMap from "../components/GoogleMap";

export default {
  components: {
    GoogleMap
  },
  data() {
    return {
      geolocations: [
        { lat: 56.80, lng: 24.58 },
        { lat: 56.81, lng: 24.59 }
      ]
    }
  },
  methods: {
    addNewLocationMarker() {
      this.$refs.map.addNewLocationMarker();
    },
    saveNewLocation() {
      this.$refs.map.saveNewLocation();
    },
    cancelAddingNewLocation() {
      this.$refs.map.cancelAddingNewLocation();
    }
  }
}
</script>

<style>
    .centered-div {
        margin: 0 auto;
        display: table;
    }

    #cancel-new-location-container {
        display: none;
    }
</style>
