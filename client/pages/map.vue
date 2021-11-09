<template>
  <v-card height="100%" width="100%" class="mx-auto">
    <GoogleMap :geolocations="geolocations" ref="map" v-on:mapOnLoad="mapOnLoad"/>
    <v-row class="move-top" justify="center" v-if="showAddNewLocation && mapIsLoaded">
      <v-btn rounded color="primary" dark @click="addNewLocationMarker">Add new location</v-btn>
    </v-row>
    <v-row class="move-top" justify="center" v-else-if="!showAddNewLocation">
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
              <v-select v-model="typeId" :items="types" label="Type" item-text="name" item-value="id" color="deep-purple" />
              <v-textarea v-model="description" label="Description" color="deep-purple" />
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="deep-purple" text @click="dialog = false">
              Cancel
            </v-btn>
            <v-btn color="deep-purple" text @click="saveNewLocation()">
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
      mapIsLoaded: false,
      dialog: false,
      typeId: '',
      description: '',

      types: []
    }
  },
  created() {
    this.types = this.getTypes();
  },
  methods: {
    addNewLocationMarker() {
      this.$refs.map.addNewLocationMarker();
      this.showAddNewLocation = false;
    },
    saveNewLocation() {
      const properties = {
        typeId: this.typeId,
        description: this.description
      }
      this.$refs.map.saveNewLocation(properties);
      this.dialog = false;
      this.showAddNewLocation = true;
    },
    cancelAddingNewLocation() {
      this.$refs.map.cancelAddingNewLocation();
      this.showAddNewLocation = true;
    },
    getTypes() {
      return [{ name: 'Basketball', id: 1 }, { name: 'Tennis', id: 2 }];
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
