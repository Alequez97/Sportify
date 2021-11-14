<template>
  <v-card height="100%" width="100%" class="mx-auto">
    <v-dialog v-model="filterDialog" scrollable max-width="300px">
      <v-card>
        <v-card-title class="justify-center">Filter types</v-card-title>
        <v-divider></v-divider>
        <v-card-text style="height: 400px;">
          <v-list>
            <v-list-item>
              <v-switch v-model="enableAllTypesSwitch" :color="enableAllSwitchColor" :label="enableAllTypesSwitchText" :value="true" @change="enableAllTypesOnChange" inset></v-switch>
            </v-list-item>
            <v-list-item v-for="type in types" :key="type.id" link>
              <v-switch v-model="enabledTypeIds" color="teal" :label="type.name" :value="type.id" @change="enableTypeOnChange" inset></v-switch>
            </v-list-item>
          </v-list>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-title>
          <v-btn color="teal" text class="mx-auto" @click="filterDialog = false">
            Close
          </v-btn>
        </v-card-title>
      </v-card>
    </v-dialog>

    <GoogleMap
      :geolocations="geolocations"
      :enabledTypeIds="enabledTypeIds"
      :showFilterButton="true"
      ref="map"
      v-on:mapOnLoad="mapOnLoad"
      v-on:filterOnClick="filterOnClick"
    />

    <v-row class="move-top" justify="center" v-if="showAddNewLocation && mapIsLoaded">
      <v-btn rounded color="primary" dark @click="addNewLocationMarker">Add new location</v-btn>
    </v-row>
    <v-row class="move-top" justify="center" v-if="showCancelButton">
      <v-btn rounded color="red accent-2" dark @click="cancelAddingNewLocation">Cancel</v-btn>
      <v-dialog v-model="saveLocationDialog" max-width="600px">
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
              <v-file-input v-model="images" multiple chips counter />
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="teal" text @click="saveLocationDialog = false">
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
      enabledTypeIds: [],
      types: [],

      enableAllTypesSwitch: true,
      enableAllTypesSwitchText: 'Disable all',
      enableAllSwitchColor: 'red',

      showAddNewLocation: true,
      showCancelButton: false,
      mapIsLoaded: false,
      saveLocationDialog: false,
      filterDialog: false,

      typeId: '',
      description: '',
      images: []
    }
  },
  created() {
    this.getTypes();
  },
  methods: {
    enableAllTypesOnChange() {
      if (this.enableAllTypesSwitch) {
        this.enabledTypeIds = this.types.map(t => t.id);
        this.enableAllTypesSwitchText = 'Disable all'
      } else {
        this.enabledTypeIds = [];
        this.enableAllTypesSwitchText = 'Enable all'
      }
    },
    enableTypeOnChange() {
      if (this.enabledTypeIds.length > 0) {
        this.enableAllTypesSwitchText = 'Disable all';
        this.enableAllTypesSwitch = true;
      } else {
        this.enableAllTypesSwitchText = 'Enable all';
        this.enableAllSwitchColor = 'red';
        this.enableAllTypesSwitch = false;
      }
    },
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
      this.saveLocationDialog = false;
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
          this.enabledTypeIds = this.types.map(t => t.id);
        }).catch((error) => {
          console.log(error);
        });
    },
    async mapOnLoad() {
      await this.getLocationsAround();
      this.mapIsLoaded = true;
    },
    filterOnClick() {
      this.filterDialog = true;
    },
    saveFilters() {
      this.filterDialog = false;
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
