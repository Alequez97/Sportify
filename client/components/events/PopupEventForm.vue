<template>
  <v-row id="kek" justify="center" class="mb-1">
    <v-dialog v-model="dialog" persistent max-width="600px">
      <template #activator="{ on, attrs }">
        <v-btn color="deep-purple" dark v-bind="attrs" v-on="on">
          Create Event
        </v-btn>
      </template>

      <v-card>
        <v-card-title>
          <span class="text-h5">Create Event</span>
        </v-card-title>
        <v-card-text>
          <v-form>
            <v-text-field v-model="title" label="Title" color="deep-purple" />
            <v-select v-model="categoryId" :items="categories" label="Category" item-text="name" item-value="id" color="deep-purple" />
            <v-text-field v-model="briefDesc" label="Brief Description" color="deep-purple" />
            <v-textarea v-model="description" label="Description" color="deep-purple" />
            <v-select v-model="countryId" :items="countries" label="Country" item-text="name" item-value="id" color="deep-purple" @change="onCountrySelect()" />
            <v-select v-model="cityId" :items="cities" label="City" item-text="name" item-value="id" color="deep-purple" :disabled="countrySelected" />
            <v-text-field v-model="address" label="Address" color="deep-purple" />
            <v-date-picker v-model="timeOfTheEvent" class="mt-4" full-width />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="deep-purple" text @click="dialog = false">
            Cancel
          </v-btn>
          <v-btn color="deep-purple" text @click="createEvent()">
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
    this.categories = await this.getCategories();
    this.countries = await this.getCountries();
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
      await this.$axios.post("https://localhost:44314/api/event/create", data);
    },
    async getCategories() {
      const res = await this.$axios.get("https://localhost:44314/api/categories");
      return res.data;
    },
    async getCountries() {
      const res = await this.$axios.get("https://localhost:44314/api/countries");
      return res.data;
    },
    async onCountrySelect() {
      const res = await this.$axios.get(
        "https://localhost:44314/api/cities/" + this.countryId
      );
      this.cities = res.data;
    }
  }
}
</script>
