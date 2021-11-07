<template>
  <v-row id="kek" justify="center" class="mb-1">
    <v-dialog v-model="dialog" persistent max-width="600px">
      <template #activator="{ on, attrs }">
        <v-btn color="teal" dark v-bind="attrs" v-on="on">
          Create Event
        </v-btn>
      </template>

      <v-card>
        <v-card-title>
          <span class="text-h5">Create Event</span>
        </v-card-title>
        <v-card-text>
          <v-form>
            <v-text-field v-model="title" label="Title" color="teal" />
            <v-select v-model="categoryId" :items="categories" label="Category" item-text="name" item-value="id" color="teal" />
            <v-text-field v-model="briefDesc" label="Brief Description" color="teal" />
            <v-textarea v-model="description" label="Description" color="teal" />
            <v-select v-model="countryId" :items="countries" label="Country" item-text="name" item-value="id" color="teal" @change="onCountrySelect()" />
            <v-select v-model="cityId" :items="cities" label="City" item-text="name" item-value="id" color="teal" :disabled="countrySelected" />
            <v-text-field v-model="address" label="Address" color="teal" />
            <v-date-picker v-model="timeOfTheEvent" class="mt-4" full-width />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="teal" text @click="dialog = false">
            Cancel
          </v-btn>
          <v-btn color="teal" text @click="createEvent()">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
export default {
  data() {
    return {
      dialog: false,

      title: "",
      categoryId: null,
      briefDesc: "",
      description: "",
      countryId: null,
      cityId: null,
      address: "",
      timeOfTheEvent: null,

      categories: [],
      countries: [],
      cities: []
    };
  },
  computed: {
    countrySelected() {
      return !this.cities.length;
    }
  },
  async created() {
    this.categories = await this.$store.dispatch('events/fetchCategories');
    this.countries = await this.$store.dispatch('events/fetchCountries')
  },
  methods: {
    async createEvent() {
      this.dialog = false;

      const data = {
        title: this.title,
        categoryId: this.categoryId,
        briefDesc: this.briefDesc,
        description: this.description,
        timeOfTheEvent: this.timeOfTheEvent,
        countryId: this.countryId,
        cityId: this.cityId,
        address: this.address
      }

      // const config = {
      //     headers: { Authorization: `Bearer ${token}` }
      // };
      const config = {
          headers: { Authorization: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlcm5hbWUxNjM2MzE2MTc4MTMyIiwianRpIjoiZjJhZTNmMGEtMjc2OS00NTdkLWIxZGEtZGVlYzNkMjI2OTAzIiwiZXhwIjoxNjM2MzI2OTg0LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjYxOTU1IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0MjAwIn0.ZZd2DQS7yloETShX3vWhG4SXQzNJDsGaVymb1MNRTr8` }
      };
      debugger;
      await this.$axios.post("https://localhost:44314/api/event/create", data, config);
    },
    async onCountrySelect() {
      this.cities = await this.$store.dispatch('events/fetchCities', this.countryId);
    }
  }
}
</script>
