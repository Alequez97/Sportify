<template>
  <v-card height="100%" width="100%" class="mx-auto">
    <GoogleMap :geolocations="geolocations" ref="map" v-on:mapOnLoad="mapOnLoad"/>
    <v-row class="move-top" justify="center" v-if="showAddNewLocation && mapIsLoaded">
      <v-btn rounded color="primary" dark @click="addNewLocationMarker">Add new location</v-btn>
    </v-row>
    <v-row class="move-top" justify="center" v-if="showCancelButton">
      <v-btn rounded color="red accent-2" dark @click="cancelAddingNewLocation">Cancel</v-btn>
      <v-dialog v-model="dialog" persistent max-width="600px">
        <template #activator="{ on, attrs }">
          <v-btn rounded color="primary" dark v-bind="attrs" v-on="on" class="ml-2">
            Save
          </v-btn>
        </template>

        <v-card>
          <v-card-title>
            <span class="text-h5">Add new sports ground</span>
          </v-card-title>
          <v-card-text>
            <v-form>
              <v-select v-model="typeId" :items="types" label="Type" item-text="name" item-value="id" color="teal" />
              <v-textarea v-model="description" label="Description" color="teal" />
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="teal" text @click="dialog = false">
              Cancel
            </v-btn>
            <v-btn color="teal" text @click="saveNewLocation()">
              Save
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
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
      geolocations: [],
      showAddNewLocation: true,
      showCancelButton: false,
      mapIsLoaded: false,
      dialog: false,
      typeId: '',
      description: '',

      types: []
    }
  },
  created() {
    this.getTypes();
  },
  methods: {
    addNewLocationMarker() {
      this.$refs.map.addNewLocationMarker();
      this.showAddNewLocation = false;
      this.showCancelButton = true;
    },
    saveNewLocation() {
      const properties = {
        typeId: this.typeId,
        description: this.description
      }
      const typeName = this.types.filter(t => t.id === this.typeId)[0].name.replaceAll(" ", "_");
      this.$refs.map.saveNewLocation(properties, typeName);
      this.dialog = false;
      this.showAddNewLocation = true;
      this.showCancelButton = false;
    },
    cancelAddingNewLocation() {
      this.$refs.map.cancelAddingNewLocation();
      this.showAddNewLocation = true;
      this.showCancelButton = false;
    },
    async getTypes() {
      await this.$axios.get("https://localhost:44314/api/map/types").then((response) => {
          this.types = response.data;
        }).catch((error) => {
          console.log(error);
        });
    },
    async mapOnLoad() {
      await this.getLocationsAround();
      this.mapIsLoaded = true;
    },
    async getLocationsAround() {
      // Identify where user is located
      await this.$axios.get("https://localhost:44314/api/map", { params: { country: 'Latvia' } }).then((response) => {
          this.geolocations = response.data;
        }).catch((error) => {
          console.log(error);
        });
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
