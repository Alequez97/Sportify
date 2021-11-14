<template>
  <v-row justify="center" class="mb-0 mt-3">
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
            <v-date-picker v-model="timeOfTheEvent" color="teal" class="mt-4" full-width />
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

      title: '',
      categoryId: null,
      briefDesc: '',
      description: '',
      countryId: null,
      cityId: null,
      address: '',
      timeOfTheEvent: null
    };
  },
  computed: {
    categories() { return this.$store.getters['events/getCategories'] },
    countries() { return this.$store.getters['events/getCountries'] },
    cities() { return this.$store.getters['events/getCities'] },
    countrySelected() {
      return !this.cities.length;
    }
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

      await this.$axios.post("https://localhost:44314/api/event/create", data);
    },
    async onCountrySelect() {
      await this.$store.dispatch('events/fetchCities', this.countryId);
    }
  }
}
</script>
