<template>
  <v-card height="100%" width="100%" class="mx-auto">
    <GoogleMap :geolocations="geolocations" ref="map" />
    <v-row class="move-top" justify="center" v-if="showAddNewLocation">
      <v-btn rounded color="primary" dark @click="addNewLocationMarker">Add new location</v-btn>
    </v-row>
    <v-row class="move-top" justify="center" v-else>
      <v-btn rounded color="red accent-2" dark @click="cancelAddingNewLocation">Cancel</v-btn>
      <v-btn rounded color="primary" dark @click="saveNewLocation" class="ml-2">Save</v-btn>
    </v-row>
  </v-card>
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
      ],
      showAddNewLocation: true
    }
  },
  methods: {
    addNewLocationMarker() {
      this.$refs.map.addNewLocationMarker();
      this.showAddNewLocation = false;
    },
    saveNewLocation() {
      this.$refs.map.saveNewLocation();
    },
    cancelAddingNewLocation() {
      this.$refs.map.cancelAddingNewLocation();
      this.showAddNewLocation = true;
    }
  }
}
</script>

<style scoped>
  .move-top {
    position: relative;
    bottom: 60px;
  }
</style>
